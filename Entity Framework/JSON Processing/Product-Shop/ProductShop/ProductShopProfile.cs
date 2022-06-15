using AutoMapper;
using ProductShop.DTO;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            this.CreateMap<UserDTO, User>();

            this.CreateMap<ProductDTO, Product>();

            this.CreateMap<CategoryDTO, Category>();

            this.CreateMap<CategoryProductDTO, CategoryProduct>();
        }
    }
}
