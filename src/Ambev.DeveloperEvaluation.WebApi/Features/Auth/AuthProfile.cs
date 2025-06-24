using Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;
using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUserFeature;
using Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUser;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Auth
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<AuthenticateUserRequest, AuthenticateUserCommand>().ReverseMap();
            CreateMap<AuthenticateUserRequest, AuthenticateUserResult>().ReverseMap();
            CreateMap<AuthenticateUserResult, AuthenticateUserResponse>().ReverseMap();

            CreateMap<GetUserResult, GetUserResponse>().ReverseMap();
        }
    }
}