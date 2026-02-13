using Core.Extensions;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Core.CrossCuttingConcerns.Caching.Microsoft
{
    public class MemoryCacheManager : ICacheManager
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MemoryCacheManager()
        {
            _memoryCache = ServiceTool.ServiceProvider.GetService<IMemoryCache>();

            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
        }

        public void Add(string key, object value, int duration)
        {
            _memoryCache.Set(key, value, TimeSpan.FromMinutes(duration));
        }

        public T Get<T>(string key)
        {
            return _memoryCache.Get<T>(key);
        }

        public object Get(string key)
        {
            return _memoryCache.Get(key);
        }

        public bool IsAdd(string key)
        {
            return _memoryCache.TryGetValue(key, out _);
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        public void RemoveByPattern(string pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern))
                return;

            pattern = pattern.Trim();

            const string nsPrefix = "Business.Abstract.";
            if (!pattern.StartsWith(nsPrefix, StringComparison.OrdinalIgnoreCase))
                pattern = nsPrefix + pattern;

            if (_memoryCache is not MemoryCache memoryCache)
                return;

            string[] parts = pattern.Split('.', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length < 3)
                return;

            string service = parts[2];
            string userPartRegex;
            string methodPrefix = null;

            int currentUserId = Convert.ToInt32(_httpContextAccessor.HttpContext.User.ClaimRoles()[3].Value);

            if (parts.Length >= 5 && int.TryParse(parts[3], out int explicitUserId))
            {
                userPartRegex = Regex.Escape(explicitUserId.ToString());
                methodPrefix = parts[4];
            }
            else if (parts.Length >= 4)
            {
                userPartRegex = Regex.Escape(currentUserId.ToString());
                methodPrefix = parts[3];
            }
            else
            {
                userPartRegex = Regex.Escape(currentUserId.ToString());
            }

            string regexPattern =
                "^" + Regex.Escape("Business.Abstract.") +
                Regex.Escape(service) +
                "\\." + userPartRegex + "\\." +
                (string.IsNullOrEmpty(methodPrefix) ? "" : Regex.Escape(methodPrefix));

            Regex regex = new(regexPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

            var coherentStateField = typeof(MemoryCache).GetField("_coherentState", BindingFlags.NonPublic | BindingFlags.Instance);
            if (coherentStateField == null) return;

            var coherentState = coherentStateField.GetValue(memoryCache);
            if (coherentState == null) return;

            var entriesField = coherentState.GetType().GetField("_stringEntries", BindingFlags.NonPublic | BindingFlags.Instance);
            if (entriesField == null) return;

            if (entriesField.GetValue(coherentState) is not IDictionary entries)
                return;

            List<object> keysToRemove = [];

            foreach (DictionaryEntry entry in entries)
            {
                if (entry.Key is string key && regex.IsMatch(key))
                    keysToRemove.Add(entry.Key);
            }

            foreach (var key in keysToRemove)
                _memoryCache.Remove(key);
        }
    }
}
