using AutoMapper;
using FreeBookAPI.Application.DTO;
using FreeBookAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeBookAPI.Application
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, BookDTO>()
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.BookImage.ImagePath)).ReverseMap();
        }
    }
}
