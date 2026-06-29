using EBookManagerApp.Models;
using Microsoft.Data.Sqlite;

namespace EBookManagerApp.Services
{
    public class DatabaseService
    {
        private readonly string _databasePath;

        public DatabaseService()
        {
            var appFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "EBookManagerApp"
            );

            Directory.CreateDirectory(appFolder);

            _databasePath = Path.Combine(appFolder, "ebooks.db");
        }

        public void Initialize()
        {
            using var connection = CreateConnection();
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText =
            """
            CREATE TABLE IF NOT EXISTS Books (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                FolderPath TEXT NOT NULL UNIQUE,
                Series TEXT,
                Author TEXT,
                Genre TEXT,
                NicePoint TEXT,
                Tags TEXT
            );

            CREATE TABLE IF NOT EXISTS BookmarkPages (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                BookId INTEGER NOT NULL,
                PageIndex INTEGER NOT NULL,
                ImagePath TEXT NOT NULL,
                Memo TEXT,
                CreatedAt TEXT NOT NULL,
                UpdatedAt TEXT NOT NULL,
                UNIQUE(BookId, PageIndex),
                FOREIGN KEY(BookId) REFERENCES Books(Id)
            );

            CREATE TABLE IF NOT EXISTS ReadingProgress (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                BookId INTEGER NOT NULL UNIQUE,
                PageIndex INTEGER NOT NULL,
                ImagePath TEXT NOT NULL,
                UpdatedAt TEXT NOT NULL,
                FOREIGN KEY(BookId) REFERENCES Books(Id)
            );
            """;

            command.ExecuteNonQuery();
        }

        public void MergeSavedData(List<Book> scannedBooks)
        {
            using var connection = CreateConnection();
            connection.Open();

            var savedBooks = GetAllBooksByFolderPath(connection);

            foreach (var scannedBook in scannedBooks)
            {
                if (!savedBooks.TryGetValue(scannedBook.FolderPath, out var savedBook))
                {
                    InsertBook(connection, scannedBook);
                    continue;
                }

                scannedBook.Id = savedBook.Id;
                scannedBook.Name = savedBook.Name;
                scannedBook.Series = savedBook.Series;
                scannedBook.Author = savedBook.Author;
                scannedBook.Genre = savedBook.Genre;
                scannedBook.NicePoint = savedBook.NicePoint;
                scannedBook.Tags = savedBook.Tags;
            }
        }

        public void SaveBook(Book book)
        {
            using var connection = CreateConnection();
            connection.Open();

            if (book.Id <= 0)
            {
                InsertBook(connection, book);
            }
            else
            {
                UpdateBook(connection, book);
            }
        }

        public HashSet<int> GetBookmarkPageIndexes(Book book)
        {
            var result = new HashSet<int>();

            if (book.Id <= 0)
            {
                return result;
            }

            using var connection = CreateConnection();
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText =
            """
            SELECT PageIndex
            FROM BookmarkPages
            WHERE BookId = $bookId
            ORDER BY PageIndex;
            """;

            command.Parameters.AddWithValue("$bookId", book.Id);

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                result.Add(reader.GetInt32(0));
            }

            return result;
        }

        public List<BookmarkPage> GetBookmarkPages(Book book)
        {
            var result = new List<BookmarkPage>();

            if (book.Id <= 0)
            {
                return result;
            }

            using var connection = CreateConnection();
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText =
            """
            SELECT
                PageIndex,
                ImagePath,
                Memo,
                CreatedAt,
                UpdatedAt
            FROM BookmarkPages
            WHERE BookId = $bookId
            ORDER BY PageIndex;
            """;

            command.Parameters.AddWithValue("$bookId", book.Id);

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                result.Add(new BookmarkPage
                {
                    PageIndex = reader.GetInt32(0),
                    ImagePath = reader.GetString(1),
                    Memo = reader.IsDBNull(2) ? "" : reader.GetString(2),
                    CreatedAt = reader.IsDBNull(3) ? "" : reader.GetString(3),
                    UpdatedAt = reader.IsDBNull(4) ? "" : reader.GetString(4)
                });
            }

            return result;
        }

        public string GetBookmarkMemo(Book book, int pageIndex)
        {
            if (book.Id <= 0)
            {
                return "";
            }

            using var connection = CreateConnection();
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText =
            """
            SELECT Memo
            FROM BookmarkPages
            WHERE BookId = $bookId
              AND PageIndex = $pageIndex
            LIMIT 1;
            """;

            command.Parameters.AddWithValue("$bookId", book.Id);
            command.Parameters.AddWithValue("$pageIndex", pageIndex);

            var result = command.ExecuteScalar();

            return result?.ToString() ?? "";
        }

        public void SaveBookmarkPage(Book book, int pageIndex, string imagePath, string memo)
        {
            if (book.Id <= 0)
            {
                SaveBook(book);
            }

            if (book.Id <= 0)
            {
                return;
            }

            using var connection = CreateConnection();
            connection.Open();

            var now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            var command = connection.CreateCommand();

            command.CommandText =
            """
            INSERT INTO BookmarkPages (
                BookId,
                PageIndex,
                ImagePath,
                Memo,
                CreatedAt,
                UpdatedAt
            )
            VALUES (
                $bookId,
                $pageIndex,
                $imagePath,
                $memo,
                $createdAt,
                $updatedAt
            )
            ON CONFLICT(BookId, PageIndex)
            DO UPDATE SET
                ImagePath = excluded.ImagePath,
                Memo = excluded.Memo,
                UpdatedAt = excluded.UpdatedAt;
            """;

            command.Parameters.AddWithValue("$bookId", book.Id);
            command.Parameters.AddWithValue("$pageIndex", pageIndex);
            command.Parameters.AddWithValue("$imagePath", imagePath);
            command.Parameters.AddWithValue("$memo", memo);
            command.Parameters.AddWithValue("$createdAt", now);
            command.Parameters.AddWithValue("$updatedAt", now);

            command.ExecuteNonQuery();
        }

        public void DeleteBookmarkPage(Book book, int pageIndex)
        {
            if (book.Id <= 0)
            {
                return;
            }

            using var connection = CreateConnection();
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText =
            """
            DELETE FROM BookmarkPages
            WHERE BookId = $bookId
              AND PageIndex = $pageIndex;
            """;

            command.Parameters.AddWithValue("$bookId", book.Id);
            command.Parameters.AddWithValue("$pageIndex", pageIndex);

            command.ExecuteNonQuery();
        }

        public int GetLastReadPageIndex(Book book)
        {
            if (book.Id <= 0)
            {
                return -1;
            }

            using var connection = CreateConnection();
            connection.Open();

            return GetLastReadPageIndex(connection, book.Id);
        }

        public void SaveReadingProgress(Book book, int pageIndex, string imagePath)
        {
            if (book.Id <= 0)
            {
                SaveBook(book);
            }

            if (book.Id <= 0)
            {
                return;
            }

            using var connection = CreateConnection();
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText =
            """
            INSERT INTO ReadingProgress (
                BookId,
                PageIndex,
                ImagePath,
                UpdatedAt
            )
            VALUES (
                $bookId,
                $pageIndex,
                $imagePath,
                $updatedAt
            )
            ON CONFLICT(BookId)
            DO UPDATE SET
                PageIndex = excluded.PageIndex,
                ImagePath = excluded.ImagePath,
                UpdatedAt = excluded.UpdatedAt;
            """;

            command.Parameters.AddWithValue("$bookId", book.Id);
            command.Parameters.AddWithValue("$pageIndex", pageIndex);
            command.Parameters.AddWithValue("$imagePath", imagePath);
            command.Parameters.AddWithValue("$updatedAt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            command.ExecuteNonQuery();
        }

        public void ApplyBookListStatus(List<Book> books)
        {
            foreach (var book in books)
            {
                book.LastReadPageIndex = -1;
                book.BookmarkCount = 0;
            }

            using var connection = CreateConnection();
            connection.Open();

            var lastReadPageIndexes = GetAllLastReadPageIndexes(connection);
            var bookmarkCounts = GetAllBookmarkCounts(connection);

            foreach (var book in books)
            {
                if (book.Id <= 0)
                {
                    continue;
                }

                if (lastReadPageIndexes.TryGetValue(book.Id, out int pageIndex))
                {
                    book.LastReadPageIndex = pageIndex;
                }

                if (bookmarkCounts.TryGetValue(book.Id, out int bookmarkCount))
                {
                    book.BookmarkCount = bookmarkCount;
                }
            }
        }

        private Dictionary<string, Book> GetAllBooksByFolderPath(SqliteConnection connection)
        {
            var result = new Dictionary<string, Book>(StringComparer.OrdinalIgnoreCase);

            var command = connection.CreateCommand();

            command.CommandText =
            """
            SELECT
                Id,
                Name,
                FolderPath,
                Series,
                Author,
                Genre,
                NicePoint,
                Tags
            FROM Books;
            """;

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var book = new Book
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    FolderPath = reader.GetString(2),
                    Series = reader.IsDBNull(3) ? "" : reader.GetString(3),
                    Author = reader.IsDBNull(4) ? "" : reader.GetString(4),
                    Genre = reader.IsDBNull(5) ? "" : reader.GetString(5),
                    NicePoint = reader.IsDBNull(6) ? "" : reader.GetString(6),
                    Tags = reader.IsDBNull(7) ? "" : reader.GetString(7)
                };

                result[book.FolderPath] = book;
            }

            return result;
        }

        private Dictionary<int, int> GetAllLastReadPageIndexes(SqliteConnection connection)
        {
            var result = new Dictionary<int, int>();

            var command = connection.CreateCommand();

            command.CommandText =
            """
            SELECT
                BookId,
                PageIndex
            FROM ReadingProgress;
            """;

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                int bookId = reader.GetInt32(0);
                int pageIndex = reader.GetInt32(1);

                result[bookId] = pageIndex;
            }

            return result;
        }

        private Dictionary<int, int> GetAllBookmarkCounts(SqliteConnection connection)
        {
            var result = new Dictionary<int, int>();

            var command = connection.CreateCommand();

            command.CommandText =
            """
            SELECT
                BookId,
                COUNT(*)
            FROM BookmarkPages
            GROUP BY BookId;
            """;

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                int bookId = reader.GetInt32(0);
                int count = reader.GetInt32(1);

                result[bookId] = count;
            }

            return result;
        }

        private int GetLastReadPageIndex(SqliteConnection connection, int bookId)
        {
            var command = connection.CreateCommand();

            command.CommandText =
            """
            SELECT PageIndex
            FROM ReadingProgress
            WHERE BookId = $bookId
            LIMIT 1;
            """;

            command.Parameters.AddWithValue("$bookId", bookId);

            var result = command.ExecuteScalar();

            if (result == null)
            {
                return -1;
            }

            if (int.TryParse(result.ToString(), out int pageIndex))
            {
                return pageIndex;
            }

            return -1;
        }

        private void InsertBook(SqliteConnection connection, Book book)
        {
            var command = connection.CreateCommand();

            command.CommandText =
            """
            INSERT INTO Books (
                Name,
                FolderPath,
                Series,
                Author,
                Genre,
                NicePoint,
                Tags
            )
            VALUES (
                $name,
                $folderPath,
                $series,
                $author,
                $genre,
                $nicePoint,
                $tags
            );

            SELECT last_insert_rowid();
            """;

            AddBookParameters(command, book);

            var result = command.ExecuteScalar();

            if (result != null && int.TryParse(result.ToString(), out int newId))
            {
                book.Id = newId;
            }
        }

        private void UpdateBook(SqliteConnection connection, Book book)
        {
            var command = connection.CreateCommand();

            command.CommandText =
            """
            UPDATE Books
            SET
                Name = $name,
                FolderPath = $folderPath,
                Series = $series,
                Author = $author,
                Genre = $genre,
                NicePoint = $nicePoint,
                Tags = $tags
            WHERE Id = $id;
            """;

            command.Parameters.AddWithValue("$id", book.Id);
            AddBookParameters(command, book);

            command.ExecuteNonQuery();
        }

        private void AddBookParameters(SqliteCommand command, Book book)
        {
            command.Parameters.AddWithValue("$name", book.Name);
            command.Parameters.AddWithValue("$folderPath", book.FolderPath);
            command.Parameters.AddWithValue("$series", book.Series);
            command.Parameters.AddWithValue("$author", book.Author);
            command.Parameters.AddWithValue("$genre", book.Genre);
            command.Parameters.AddWithValue("$nicePoint", book.NicePoint);
            command.Parameters.AddWithValue("$tags", book.Tags);
        }

        private SqliteConnection CreateConnection()
        {
            return new SqliteConnection($"Data Source={_databasePath}");
        }
    }
}