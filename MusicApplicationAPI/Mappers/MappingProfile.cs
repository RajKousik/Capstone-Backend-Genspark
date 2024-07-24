using AutoMapper;
using MusicApplicationAPI.Models.DbModels;
using MusicApplicationAPI.Models.DTOs.PlaylistDTO;
using MusicApplicationAPI.Models.DTOs.PlaylistSongDTO;
using MusicApplicationAPI.Models.DTOs.SongDTO;
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

            #region Song

            CreateMap<Song, SongReturnDTO>().ReverseMap();
            CreateMap<Song, SongUpdateDTO>().ReverseMap();
            CreateMap<Song, SongAddDTO>().ReverseMap();
            
            CreateMap<SongReturnDTO, SongUpdateDTO>().ReverseMap();
            CreateMap<SongReturnDTO, SongAddDTO>().ReverseMap();
            
            CreateMap<SongUpdateDTO, SongAddDTO>().ReverseMap();
            #endregion

            #region Playlist
            CreateMap<Playlist, PlaylistAddDTO>().ReverseMap();
            CreateMap<Playlist, PlaylistReturnDTO>().ReverseMap();
            CreateMap<Playlist, PlaylistUpdateDTO>().ReverseMap();

            CreateMap<PlaylistAddDTO, PlaylistReturnDTO>().ReverseMap();
            CreateMap<PlaylistAddDTO, PlaylistUpdateDTO>().ReverseMap();

            CreateMap<PlaylistReturnDTO, PlaylistUpdateDTO>().ReverseMap();
            #endregion

            #region Playlist Song

            CreateMap<PlaylistSong, PlaylistSongDTO>().ReverseMap();
            CreateMap<PlaylistSong, PlaylistSongReturnDTO>().ReverseMap();

            CreateMap<PlaylistSongDTO, PlaylistSongReturnDTO>().ReverseMap();


            #endregion


        }
    }
}
