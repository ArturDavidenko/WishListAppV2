using AutoMapper;
using WebAPIWishList.Dto;
using WebAPIWishList.Models;

namespace WebAPIWishList.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles() 
        {
            CreateMap<WishItem, WishItemDto>();
            CreateMap<WishItemDto, WishItem>(); 
        }
    }
}
