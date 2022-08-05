namespace CarDealer
{
    using AutoMapper;
    using CarDealer.DTO;
    using CarDealer.Models;
    using System.Linq;

    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            //Supliers
            this.CreateMap<Supplier, SupplierIdDTO>();

            //Cars
            this.CreateMap<CarDTO, Car>();

            this.CreateMap<Customer, CustomerSalesDTO>()
                .ForMember(x => x.FullName, y => y.MapFrom(c => c.Name))
                .ForMember(x => x.BoughtCars, y => y.MapFrom(c => c.Sales.Count()))
                .ForMember(x => x.SpentMoney,
                    y => y.MapFrom(
                        c => c.Sales.Sum(s => s.Car.PartCars.Sum(pc => pc.Part.Price))));
        }
    }
}