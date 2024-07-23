using ERPServer.Domain.Entities;
using ERPServer.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TS.Result;

namespace ERPServer.Application.Features.Customers.GetAllCustomer
{
    internal sealed class GetAllCustomerQueryHandler(ICustomerRepository customerRepository) : IRequestHandler<GetAllCustomerQuery, Result<List<Customer>>>
    {
        private readonly ICustomerRepository customerRepository = customerRepository;

        public async Task<Result<List<Customer>>> Handle(GetAllCustomerQuery request, CancellationToken cancellationToken)
        {
            var customers = await customerRepository.GetAll().OrderBy(p => p.Name).ToListAsync(cancellationToken);
            return customers;
        }
    }
}
