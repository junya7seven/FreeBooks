using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBookAPI.Infrastructure.StorageAPI
{
    public static class EnsureSuccessStatusCode
    {
        public static void ProcessingStatusCode(HttpResponseMessage response)
        {
            switch ((int)response.StatusCode)
            {
                case 200:
                case 201:
                    break;
                case 400:
                    throw new HttpRequestException("Некорректный запрос. Проверьте переданные данные.");
                case 401:
                    throw new UnauthorizedAccessException("Ошибка авторизации. Проверьте токен доступа.");
                case 403:
                    throw new InsufficientStorageYandexException();
                case 404:
                    throw new FileNotFoundException("Ресурс не найден.");
                case 409:
                    throw new ConflictException();
                case 413:
                    throw new TooLargeFileYandexException();
                case 423:
                    throw new TechnicalWorkYandexException();
                case 503:
                    throw new UnavailableService();
                default:
                    break;
            }
        }
    }

    public class InsufficientStorageYandexException : Exception // 403
    {
        public InsufficientStorageYandexException() : base("Недостаточно места для завершения операции.") { }

        public InsufficientStorageYandexException(string message) : base(message) { }

        public InsufficientStorageYandexException(string message, Exception innerException)
            : base(message, innerException) { }
    }
    public class ConflictException : Exception // 409
    {
        public ConflictException() : base("Такой файл уже существует.") { }

        public ConflictException(string message) : base(message) { }

        public ConflictException(string message, Exception innerException)
            : base(message, innerException) { }
    }
    public class TooLargeFileYandexException : Exception // 413

    {
        public TooLargeFileYandexException() : base("Загрузка файла недоступна. Файл слишком большой.") { }

        public TooLargeFileYandexException(string message) : base(message) { }

        public TooLargeFileYandexException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    public class TechnicalWorkYandexException : Exception // 423
    {
        public TechnicalWorkYandexException() : base("Технические работы. Сейчас можно только просматривать и скачивать файлы.") { }

        public TechnicalWorkYandexException(string message) : base(message) { }

        public TechnicalWorkYandexException(string message, Exception innerException)
            : base(message, innerException) { }
    }

    public class UnavailableService : Exception // 503
    {
        public UnavailableService() : base("Сервис временно недоступен.") { }

        public UnavailableService(string message) : base(message) { }

        public UnavailableService(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
