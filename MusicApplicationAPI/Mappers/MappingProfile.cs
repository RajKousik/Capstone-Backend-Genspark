using AutoMapper;
using MusicApplicationAPI.Models.DbModels;
using MusicApplicationAPI.Models.DTOs.UserDTO;

namespace MusicApplicationAPI.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region User
            CreateMap<User, UserRegisterDTO>().ReverseMap();
            CreateMap<User, UserLoginDTO>().ReverseMap();
            CreateMap<User, UserLoginReturnDTO>().ReverseMap();
            CreateMap<User, UserRegisterReturnDTO>().ReverseMap();

            CreateMap<UserRegisterDTO, UserLoginDTO>().ReverseMap();
            CreateMap<UserRegisterDTO, UserLoginReturnDTO>().ReverseMap();
            CreateMap<UserRegisterDTO, UserRegisterReturnDTO>().ReverseMap();

            CreateMap<UserLoginDTO, UserLoginReturnDTO>().ReverseMap();
            CreateMap<UserLoginDTO, UserRegisterReturnDTO>().ReverseMap();

            CreateMap<UserRegisterReturnDTO, UserLoginReturnDTO>().ReverseMap();
            #endregion
        }
    }
}
