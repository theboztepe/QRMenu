using System;

namespace DataAccess.Helper
{
    public class ImageControl
    {
        readonly string[] imageFormats = ["jpeg", "png", "jpg"];

        public bool ImageFileTypeControl(string base64String)
        {
            return GetMimeType(base64String) || FileExtensionControl(base64String);
        }

        private bool GetMimeType(string base64String)
        {
            foreach (string format in imageFormats)
            {
                string header = $"data:image/{format};base64,";
                if (base64String.StartsWith(header))
                    return true;
            }

            return false;
        }

        private static bool FileExtensionControl(string base64String)
        {
            var data = base64String[..5];

            return data.ToUpper() switch
            {
                "IVBOR" or "/9J/4" => true,
                _ => false,
            };
        }

        public bool ImageFileSizeControl(string base64String)
        {
            foreach (string format in imageFormats)
            {
                string header = $"data:image/{format};base64,";
                base64String = base64String.Replace(header, "");
            }

            byte[] imageBytes = Convert.FromBase64String(base64String);

            int fileSizeInBytes = imageBytes.Length;
            double fileSizeInKb = fileSizeInBytes / 1024.0;

            return fileSizeInKb <= 300;
        }
    }
}
