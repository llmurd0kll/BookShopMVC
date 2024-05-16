using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BookShopMVCUI.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _db;

        public HomeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Genre>> Genres()
        {
            return await _db.Genres.ToListAsync();
        }

        /// <summary>
        /// Метод получения списка книг.
        /// </summary>
        /// <param name="sTerm">Условие поиска</param>
        /// <param name="genreId">Айди жанра книги</param>
        /// <returns>Список книг</returns>
        public async Task<IEnumerable<Book>> GetBooks(string sTerm = "", int genreId = 0)
        {
            sTerm = sTerm.ToLower();
            ///Объединяем две таблицы book и genre.
            IEnumerable<Book> books = await (from book in _db.Books
                         join genre in _db.Genres
                         on book.GenreId equals genre.Id
                         where string.IsNullOrWhiteSpace(sTerm) || (book!=null && book.BookName.ToLower().StartsWith(sTerm)) ///Если searchTerm пустой или null вернет весь список записей, 
                                                                                                                                       ///если есть, сравнит с BookName и вернет записи, отфильтрованные по условию searchTerm 
                         select new Book
                         {
                             Id=book.Id,
                             Image=book.Image,
                             AuthorName=book.AuthorName,
                             BookName=book.BookName,
                             GenreId=book.GenreId,
                             Price=book.Price,
                             GenreName=genre.GenreName
                         }
                         ).ToListAsync();
            ///Если есть genreId, фильтруем по genreId
            if ( genreId > 0 )
            {
                books = books.Where(a => a.GenreId==genreId).ToList();
            }
            return books;
        }
    }
}
