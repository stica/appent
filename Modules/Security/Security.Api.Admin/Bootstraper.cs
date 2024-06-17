using AutoMapper;
using Security.Api.Admin.Contract.Dtos;
using Security.Api.Admin.Contract.Responses;
using Security.Domain.Contract.Entities;
using Security.Domain.Contract.Views;

namespace Security.Api.Public
{
    public static class Bootstraper
    {
        public static void SecurityAdminMaps(this IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<UserResponse, UserView>();
            cfg.CreateMap<UserDto, User>();
        }
    }
}
