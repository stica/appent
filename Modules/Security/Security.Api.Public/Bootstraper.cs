using AutoMapper;
using Security.Api.Public.Contract.Dtos;
using Security.Api.Public.Contract.Requests;
using Security.Api.Public.Contract.Responses;
using Security.Domain.Contract.Commands;
using Security.Domain.Contract.Entities;
using Security.Domain.Contract.Views;

namespace Security.Api.Public
{
    public static class Bootstraper
    {
        public static void SecurityPublicMaps(this IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<AddUserRequest, AddUserCommand>();
            cfg.CreateMap<UpdateUserRequest, UpdateUserCommand>();
            cfg.CreateMap<CompanyResponse, CompanyView>();
            cfg.CreateMap<UserResponse, UserView>();
            cfg.CreateMap<UserDto, User>();
        }
    }
}
