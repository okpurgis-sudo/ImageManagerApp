using EBookManagerApp.Services;
using System.Runtime.InteropServices;

namespace EBookManagerApp.Service
{
    public static class ImageFileService
    {
        private static readonly HashSet<string> ImageExtensions = new HashSet<string>(
            StringComparer.OrdinalIgnoreCase
        )
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".bmp",
            ".gif",
            ".webp"
        };

        public static bool IsImageFile(string filePath)
        {
            return ImageExtensions.Contains(Path.GetExtension(filePath));
        }

        public static bool HasImageFiles(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                return false;
            }

            return Directory.EnumerateFiles(folderPath)
                .Any(IsImageFile);
        }

        public static List<string> GetImageFiles(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                return new List<string>();
            }

            var comparer = new NaturalStringComparer();

            return Directory.EnumerateFiles(folderPath)
                .Where(IsImageFile)
                .OrderBy(file => Path.GetFileName(file), comparer)
                .ToList();
        }


            [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
            private static extern int StrCmpLogicalW(string x, string y);
        }
    }
