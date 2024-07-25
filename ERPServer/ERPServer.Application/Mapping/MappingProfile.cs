using AutoMapper;
using ERPServer.Application.Features.Customers.CreateCustomer;
using ERPServer.Application.Features.Customers.UpdateCustomer;
using ERPServer.Application.Features.Depots.CreateDepot;
using ERPServer.Application.Features.Depots.UpdateDepot;
using ERPServer.Application.Features.Invoices.CreateInvoice;
using ERPServer.Application.Features.Invoices.UpdateInvoice;
using ERPServer.Application.Features.Orders.CreateOrder;
using ERPServer.Application.Features.Orders.UpdateOrder;
using ERPServer.Application.Features.Productions.CreateProduction;
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
            CreateMap<UpdateOrderCommand, Order>()
                .ForMember(member => member.Details, opt => opt.Ignore());

            CreateMap<CreateInvoiceCommand, Invoice>()
                .ForMember(member => member.InvoiceType,
                options => options.MapFrom(p => InvoiceType.FromValue(p.InvoiceTypeValue)))
              .ForMember(member => member.Details, options =>
              options.MapFrom(p => p.Details.Select(x => new InvoiceDetail
              {
                  Price = x.Price,
                  Quantity = x.Quantity,
                  ProductId = x.ProductId,
                  DepotId = x.DepotId
              }).ToList()));
            CreateMap<UpdateInvoiceCommand, Invoice>()
                .ForMember(member => member.Details, opt => opt.Ignore());

            CreateMap<CreateProductionCommand, Production>();
        }
    }
}
