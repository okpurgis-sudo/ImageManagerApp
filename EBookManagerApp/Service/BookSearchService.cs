using EBookManagerApp.Models;

namespace EBookManagerApp.Services
{
    public enum SearchMode
    {
        Or,
        And
    }

    public class BookSearchCriteria
    {
        public string Name { get; set; } = "";
        public string Series { get; set; } = "";
        public string Author { get; set; } = "";
        public string Genre { get; set; } = "";
        public string NicePoint { get; set; } = "";
        public string Tags { get; set; } = "";

        public bool IsEmpty()
        {
            return string.IsNullOrWhiteSpace(Name)
                && string.IsNullOrWhiteSpace(Series)
                && string.IsNullOrWhiteSpace(Author)
                && string.IsNullOrWhiteSpace(Genre)
                && string.IsNullOrWhiteSpace(NicePoint)
                && string.IsNullOrWhiteSpace(Tags);
        }
    }

    public class BookSearchService
    {
        public List<Book> Search(List<Book> books, BookSearchCriteria criteria, SearchMode mode)
        {
            if (criteria.IsEmpty())
            {
                return books.ToList();
            }

            return books
                .Where(book => IsMatch(book, criteria, mode))
                .ToList();
        }

        private bool IsMatch(Book book, BookSearchCriteria criteria, SearchMode mode)
        {
            var results = new List<bool>();

            AddCondition(results, book.Name, criteria.Name);
            AddCondition(results, book.Series, criteria.Series);
            AddCondition(results, book.Author, criteria.Author);
            AddCondition(results, book.Genre, criteria.Genre);
            AddCondition(results, book.NicePoint, criteria.NicePoint);
            AddCondition(results, book.Tags, criteria.Tags);

            if (results.Count == 0)
            {
                return true;
            }

            if (mode == SearchMode.Or)
            {
                return results.Any(result => result);
            }

            return results.All(result => result);
        }

        private void AddCondition(List<bool> results, string targetText, string keywordText)
        {
            if (string.IsNullOrWhiteSpace(keywordText))
            {
                return;
            }

            var keywords = SplitKeywords(keywordText);

            if (keywords.Count == 0)
            {
                return;
            }

            // 1つの検索欄の中では OR 検索にする
            // 例: "作品A 作品B" → 作品A または 作品B を含む
            bool isMatch = keywords.Any(keyword =>
                targetText.Contains(keyword, StringComparison.CurrentCultureIgnoreCase)
            );

            results.Add(isMatch);
        }

        private List<string> SplitKeywords(string keywordText)
        {
            return keywordText
                .Split(
                    new[] { ' ', '　', ',', '，', '、' },
                    StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries
                )
                .Where(keyword => !string.IsNullOrWhiteSpace(keyword))
                .Distinct(StringComparer.CurrentCultureIgnoreCase)
                .ToList();
        }
    }
}