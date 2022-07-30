namespace ProductShop
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using DTOs.Category;
    using DTOs.CategoryProduct;
    using DTOs.Product;
    using DTOs.User;
    using Models;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            //Configuring mapper static class
            Mapper.Initialize(cfg => cfg.AddProfile(typeof(ProductShopProfile)));

            ProductShopContext dbContext = new ProductShopContext();

            //For reading json
            //string inputJson = File.ReadAllText("../../../Datasets/categories-products.json");

            //dbContext.Database.EnsureDeleted();
            //dbContext.Database.EnsureCreated();

            //01. Import Users
            //Adding users to ProductShopContext from Json and returning message
            //string output = ImportUsers(dbContext, inputJson);
            //Console.WriteLine(output);

            //02. Import Products
            //string output = ImportProducts(dbContext, inputJson);
            //Console.WriteLine(output);

            //03. Import Categories
            //string output = ImportCategories(dbContext, inputJson);
            //Console.WriteLine(output);

            //04. Import Categories and Products
            //string output = ImportCategoryProducts(dbContext, inputJson);
            //Console.WriteLine(output);

            //05. Export Products In Range
            //string json = GetProductsInRange(dbContext);
            //File.WriteAllText("../../../Datasets/products-in-range.json", json);

            //06. Export Sold Products
            //string json = GetSoldProducts(dbContext);
            //File.WriteAllText("../../../Datasets/users-sold-products.json", json);

            //07. Export Categories By Products Count
            //string json = GetCategoriesByProductsCount(dbContext);
            //File.WriteAllText("../../../Datasets/categories-by-products.json", json);

            //08. Export Users and Products
            //string json = GetUsersWithProducts(dbContext);
            //File.WriteAllText("../../../Datasets/users-and-products.json", json);
        }

        //01. Import Users
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            //Deserialize to Array 
            ImportUserDto[] userDtos = JsonConvert.DeserializeObject<ImportUserDto[]>(inputJson);

            ICollection<User> users = new List<User>();
            foreach (ImportUserDto dto in userDtos)
            {
                if (!IsValid(dto))
                {
                    continue;
                }

                //From source to destination, set in ProductShopProfile
                User user = Mapper.Map<User>(dto);
                users.Add(user);
            }

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count}";
        }

        //02. Import Products
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            ImportProductDto[] productDtos = JsonConvert.DeserializeObject<ImportProductDto[]>(inputJson);

            ICollection<Product> validProducts = new List<Product>();
            foreach (var dto in productDtos)
            {
                if (!IsValid(dto))
                {
                    continue;
                }

                Product product = Mapper.Map<Product>(dto);
                validProducts.Add(product);
            }

            context.Products.AddRange(validProducts);
            context.SaveChanges();

            return $"Successfully imported {validProducts.Count}";
        }

        //03. Import Categories
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            ImportCategoryDto[] categoryDtos = JsonConvert.DeserializeObject<ImportCategoryDto[]>(inputJson);

            ICollection<Category> validCategories = new List<Category>();
            foreach (ImportCategoryDto dto in categoryDtos)
            {
                if (!IsValid(dto))
                {
                    continue;
                }

                Category category = Mapper.Map<Category>(dto);
                validCategories.Add(category);
            }

            context.Categories.AddRange(validCategories);
            context.SaveChanges();

            return $"Successfully imported {validCategories.Count}";
        }

        //04. Import Categories and Products
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            ImportCategoryProduct[] categoryProducts = JsonConvert.DeserializeObject<ImportCategoryProduct[]>(inputJson);

            ICollection<CategoryProduct> validCategoryProducts = new List<CategoryProduct>();
            foreach (ImportCategoryProduct dto in categoryProducts)
            {
                CategoryProduct categoryProduct = Mapper.Map<CategoryProduct>(dto);
                validCategoryProducts.Add(categoryProduct);
            }

            context.CategoryProducts.AddRange(validCategoryProducts);
            context.SaveChanges();

            return $"Successfully imported {validCategoryProducts.Count}";
        }

        //05. Export Products In Range
        public static string GetProductsInRange(ProductShopContext context)
        {
            ExportProductsInRangeDto[] products = context
                .Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .ProjectTo<ExportProductsInRangeDto>()
                .ToArray();

            string json = JsonConvert.SerializeObject(products, Formatting.Indented);
            return json;
        }

        //06. Export Sold Products
        public static string GetSoldProducts(ProductShopContext context)
        {
            ExportUsersWithSoldProductsDto[] users = context
                .Users
                .Where(u => u.ProductsSold.Any(p => p.BuyerId.HasValue))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ProjectTo<ExportUsersWithSoldProductsDto>()
                .ToArray();

            string json = JsonConvert.SerializeObject(users, Formatting.Indented);
            return json;
        }

        //07. Export Categories By Products Count
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .Select(c => new
                {
                    category = c.Name,
                    productsCount = c.CategoryProducts.Count(),
                    averagePrice = c.CategoryProducts.Average(x => x.Product.Price).ToString("f2"),
                    totalRevenue = c.CategoryProducts.Sum(x => x.Product.Price).ToString("f2")
                })
                .OrderByDescending(x => x.productsCount)
                .ToArray();

            string result = JsonConvert.SerializeObject(categories, Formatting.Indented);

            return result;
        }

        //08. Export Users and Products
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            //Manuel mapping
            ExportUsersInfoDto serDto = new ExportUsersInfoDto()
            {
                Users = context
                    .Users
                    .Where(u => u.ProductsSold.Any(p => p.BuyerId.HasValue))
                    .OrderByDescending(u => u.ProductsSold.Count(p => p.BuyerId.HasValue))
                    .Select(u => new ExportUsersWithFullProductInfo()
                    {
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Age = u.Age,
                        SoldProductsInfo = new ExportSoldProductsFullInfoDto()
                        {
                            SoldProducts = u.ProductsSold
                                .Where(p => p.BuyerId.HasValue)
                                .Select(p => new ExportSoldProductsShortInfoDto()
                                {
                                    Name = p.Name,
                                    Price = p.Price
                                })
                                .ToArray()
                        }
                    })
                    .ToArray()
            };

            JsonSerializerSettings serializerSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            string json = JsonConvert.SerializeObject(serDto, Formatting.Indented, serializerSettings);
            return json;
        }

        //Executes all validation attributes in a class, can copy paste
        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult);
            return isValid;
        }
    }
}