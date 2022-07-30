namespace ProductShop
{
    using AutoMapper;
    using DTOs.Category;
    using DTOs.CategoryProduct;
    using DTOs.Product;
    using DTOs.User;
    using Models;
    using System.Linq;

    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            //1
            this.CreateMap<ImportUserDto, User>();
            //2
            this.CreateMap<ImportProductDto, Product>();
            //3
            this.CreateMap<ImportCategoryDto, Category>();
            //4
            this.CreateMap<ImportCategoryProduct, CategoryProduct>();

            //5
            this.CreateMap<Product, ExportProductsInRangeDto>()
                .ForMember(d => d.SellerFullname, mo => mo.MapFrom(s => $"{s.Seller.FirstName} {s.Seller.LastName}"));

            //6
            //Innerdto
            this.CreateMap<Product, ExportUserSoldProductsDto>()
                .ForMember(d => d.BuyerFirstName, mo => mo.MapFrom(s => s.Buyer.FirstName))
                .ForMember(d => d.BuyerLastName, mo => mo.MapFrom(s => s.Buyer.LastName));

            //Outerdto
            this.CreateMap<User, ExportUsersWithSoldProductsDto>()
                .ForMember(d => d.SoldProducts, mo => mo.MapFrom(s => s.ProductsSold.Where(p => p.BuyerId.HasValue)));

            //7

            //8 Doesn't work with judge 
            this.CreateMap<Product, ExportSoldProductsShortInfoDto>();

            this.CreateMap<User, ExportSoldProductsFullInfoDto>()
                .ForMember(d => d.SoldProducts, mo => mo.MapFrom(s => s.ProductsSold.Where(p => p.BuyerId.HasValue)));

            this.CreateMap<User, ExportUsersWithFullProductInfo>()
                .ForMember(d => d.SoldProductsInfo, mo => mo.MapFrom(s => s));
        }
    }
}
