using AutoMapper;
using Snp.Api.Contract.Dtos;
using Snp.Api.Contract.Enums;
using Snp.Api.Contract.Requests;
using Snp.Api.Contract.Responses;
using Snp.Domain.Contract.Commands;
using Snp.Domain.Contract.Entities;
using Snp.Domain.Contract.Enums;
using Snp.Domain.Contract.Views;

namespace Snp.Api
{
    public static class Bootstraper
    {
        public static void SnpModuleMaps(this IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Customer, CustomerDto>().ReverseMap();
            cfg.CreateMap<CustomerStatus, CustomerStatusDto>().ReverseMap();
            cfg.CreateMap<CustomerView, CustomerResponse>().ReverseMap();
            cfg.CreateMap<Contact, ContactDto>().ReverseMap();
            cfg.CreateMap<CreateCustomerCommand, CreateCustomerRequest>().ReverseMap();
        }
    }
}
