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
                return base64String.StartsWith(header);
            }

            return false;
        }

        private bool FileExtensionControl(string base64String)
        {
            var data = base64String[..5];

            switch (data.ToUpper())
            {
                case "IVBOR":
                case "/9J/4":
                    return true;
                default:
                    return false;
            }
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

            return fileSizeInKb <= 100;
        }
    }
}
