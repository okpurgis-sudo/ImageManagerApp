namespace EBookManagerApp.Services
{
    public static class AppSettingsService
    {
        private static readonly string SettingsFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "EBookManagerApp"
        );

        private static readonly string LastFolderFilePath = Path.Combine(
            SettingsFolder,
            "last_folder.txt"
        );

        public static string GetLastRootFolder()
        {
            try
            {
                if (!File.Exists(LastFolderFilePath))
                {
                    return "";
                }

                string folderPath = File.ReadAllText(LastFolderFilePath).Trim();

                if (string.IsNullOrWhiteSpace(folderPath))
                {
                    return "";
                }

                return folderPath;
            }
            catch
            {
                return "";
            }
        }

        public static void SaveLastRootFolder(string folderPath)
        {
            try
            {
                Directory.CreateDirectory(SettingsFolder);
                File.WriteAllText(LastFolderFilePath, folderPath);
            }
            catch
            {
                // 設定保存に失敗してもアプリ本体は使えるようにする
            }
        }
    }
}