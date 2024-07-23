using AutoMapper;
using ERPServer.Application.Features.Customers.CreateCustomer;
using ERPServer.Application.Features.Customers.UpdateCustomer;
using ERPServer.Application.Features.Depots.CreateDepot;
using ERPServer.Application.Features.Depots.UpdateDepot;
using ERPServer.Application.Features.Orders.CreateOrder;
using ERPServer.Application.Features.Products.CreateProduct;
using ERPServer.Application.Features.Products.UpdateProduct;
using ERPServer.Application.Features.RecipeDetails.CreateRecipeDetail;
using ERPServer.Application.Features.RecipeDetails.UpdateRecipeDetail;
using ERPServer.Domain.Entities;
using ERPServer.Domain.Enums;

namespace ERPServer.Application.Mapping
{
    public sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateCustomerCommand, Customer>();
            CreateMap<UpdateCustomerCommand, Customer>();

            CreateMap<CreateDepotCommand, Depot>();
            CreateMap<UpdateDepotCommand, Depot>();

            CreateMap<CreateProductCommand, Product>()
                .ForMember(mem => mem.Type,
                    opt => opt.MapFrom(p => ProductType.FromValue(p.TypeValue)));
            CreateMap<UpdateProductCommand, Product>()
                .ForMember(mem => mem.Type,
                    opt => opt.MapFrom(p => ProductType.FromValue(p.TypeValue)));

            CreateMap<CreateRecipeDetailCommand, RecipeDetail>();
            CreateMap<UpdateRecipeDetailCommand, RecipeDetail>();

            CreateMap<CreateOrderCommand, Order>()
                .ForMember(member => member.Details, options =>
                options.MapFrom(p => p.Details.Select(x => new OrderDetail
                {
                    UnitPrice = x.UnitPrice,
                    Quantity = x.Quantity,
                    ProductId = x.ProductId
                }).ToList()));
        }
    }
}
