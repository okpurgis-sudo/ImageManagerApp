using EBookManagerApp.Models;
using EBookManagerApp.Service;

namespace EBookManagerApp.Services
{
    public class BookScanService
    {
        public List<Book> ScanBooks(string rootFolderPath)
        {
            var books = new List<Book>();

            if (!Directory.Exists(rootFolderPath))
            {
                return books;
            }

            foreach (var dir in Directory.EnumerateDirectories(rootFolderPath))
            {
                if (!ImageFileService.HasImageFiles(dir))
                {
                    continue;
                }

                books.Add(new Book
                {
                    Name = Path.GetFileName(dir),
                    FolderPath = dir
                });
            }

            return books;
        }
    }
}