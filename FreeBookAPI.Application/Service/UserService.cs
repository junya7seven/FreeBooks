using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FreeBookAPI.Application.Interfaces;
using FreeBookAPI.Interfaces;
using FreeBookAPI.Models;

namespace FreeBookAPI.Application.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IBookQuery _bookQuery;
        public UserService(IUserRepository userRepository, IBookQuery bookQuery)
        {
            _userRepository = userRepository;
            _bookQuery = bookQuery;
        }

        public async Task CreateUser(User user)
        {
            try
            {
                var existUser = await _userRepository.CheckExistUser(user.TelegramUserId);
                if (existUser != null)
                {
                    await _userRepository.CreateUser(user);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public async Task<User> GetUser(long userId)
        {
            return await _userRepository.GetUser(userId);
        }
        public async Task UpdateBookPageForUser(long id, int newPage, Guid bookId)
        {
            try
            {
                var existUser = await _userRepository.CheckExistUser(id);
                var existBook = await _bookQuery.GetBookById(bookId);
                if (existUser != null && existUser != null && newPage >= 0)
                {
                    await _userRepository.UpdateBookPageForUser(id, newPage, bookId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public async Task<int> GetCurrentBookPage(long id, Guid bookId)
        {
            return await _userRepository.GetCurrentBookPage(id, bookId);
        }

        public async Task AddFavoriteBook(long id, Guid bookId)
        {
            try
            {
                var existUser = await _userRepository.CheckExistUser(id);
                var existBook = await _bookQuery.GetBookById(bookId);

                var favofiteBook = new FavoriteBook
                {
                    BookId = bookId,
                    UserId = existUser.UserId,
                };


                if (existUser != null && existBook != null)
                {
                    var existFavoriteBook = await _userRepository.GetFavofiteBook(favofiteBook);
                    if (existFavoriteBook == null)
                    {
                        await _userRepository.AddFavoriteBook(favofiteBook);
                    }
                    else
                    {
                        await _userRepository.DeleteFavoriteBook(existFavoriteBook);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public async Task<IEnumerable<FavoriteBook?>> GetFavoriteBooks(long id)
        {
            var existUser = await _userRepository.GetUser(id);

            return await _userRepository.GetFavoriteBooks(existUser.UserId);
        }
    }
}
