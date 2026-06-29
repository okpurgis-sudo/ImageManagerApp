namespace EBookManagerApp.Models
{
    public class Book
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";
        public string FolderPath { get; set; } = "";

        public string Series { get; set; } = "";
        public string Author { get; set; } = "";
        public string Genre { get; set; } = "";
        public string NicePoint { get; set; } = "";
        public string Tags { get; set; } = "";

        // 最終閲覧ページ。0始まり。
        // 未読の場合は -1。
        public int LastReadPageIndex { get; set; } = -1;

        public string LastReadPageText
        {
            get
            {
                if (LastReadPageIndex < 0)
                {
                    return "";
                }

                return $"{LastReadPageIndex + 1}ページ";
            }
        }

        public int BookmarkCount { get; set; }

        public string BookmarkCountText
        {
            get
            {
                if (BookmarkCount <= 0)
                {
                    return "";
                }

                return $"{BookmarkCount}件";
            }
        }
    }
}