namespace EBookManagerApp.Models
{
    public class BookmarkPage
    {
        public int PageIndex { get; set; }

        public int PageNumber => PageIndex + 1;

        public string ImagePath { get; set; } = "";

        public string Memo { get; set; } = "";

        public string CreatedAt { get; set; } = "";

        public string UpdatedAt { get; set; } = "";
    }
}