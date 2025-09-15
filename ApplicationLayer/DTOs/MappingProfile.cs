using AutoMapper;
using ApplicationLayer.DTOs;
using DomainLayer.Entities;
using ApplicationLayer.Customers.DTOs;

namespace ApplicationLayer.DTOs;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Category
        CreateMap<Category, CategoryDto>();
        CreateMap<CreateCategoryDto, Category>();
        CreateMap<UpdateCategoryDto, Category>();

        // Product
        CreateMap<Product, ProductDto>();
        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>();

        // Customer
        CreateMap<Customer, CustomerDto>()
            .ForMember(d => d.FullName,
                opt => opt.MapFrom(s => $"{s.FirstName} {s.LastName}"));

        CreateMap<CreateCustomerDto, Customer>();
        CreateMap<UpdateCustomerDto, Customer>();
    }
}
