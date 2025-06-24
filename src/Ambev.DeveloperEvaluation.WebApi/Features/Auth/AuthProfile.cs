using Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;
using Ambev.DeveloperEvaluation.WebApi.Features.Auth.AuthenticateUserFeature;
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
        }
    }
}