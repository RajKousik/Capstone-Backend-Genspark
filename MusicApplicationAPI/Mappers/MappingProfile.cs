using AutoMapper;
using MusicApplicationAPI.Models.DbModels;
using MusicApplicationAPI.Models.DTOs.AlbumDTO;
using MusicApplicationAPI.Models.DTOs.ArtistDTO;
using MusicApplicationAPI.Models.DTOs.FavoriteDTO;
using MusicApplicationAPI.Models.DTOs.PlaylistDTO;
using MusicApplicationAPI.Models.DTOs.PlaylistSongDTO;
using MusicApplicationAPI.Models.DTOs.RatingDTO;
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
            CreateMap<User, UserUpdateDTO>().ReverseMap();
            CreateMap<User, UserReturnDTO>().ReverseMap();

            CreateMap<UserRegisterDTO, UserLoginDTO>().ReverseMap();
            CreateMap<UserRegisterDTO, UserLoginReturnDTO>().ReverseMap();
            CreateMap<UserRegisterDTO, UserRegisterReturnDTO>().ReverseMap();
            CreateMap<UserRegisterDTO, UserUpdateDTO>().ReverseMap();
            CreateMap<UserRegisterDTO, UserReturnDTO>().ReverseMap();

            CreateMap<UserLoginDTO, UserLoginReturnDTO>().ReverseMap();
            CreateMap<UserLoginDTO, UserRegisterReturnDTO>().ReverseMap();
            CreateMap<UserLoginDTO, UserUpdateDTO>().ReverseMap();
            CreateMap<UserLoginDTO, UserReturnDTO>().ReverseMap();

            CreateMap<UserRegisterReturnDTO, UserLoginReturnDTO>().ReverseMap();
            CreateMap<UserRegisterReturnDTO, UserUpdateDTO>().ReverseMap();
            CreateMap<UserRegisterReturnDTO, UserReturnDTO>().ReverseMap();

            CreateMap<UserLoginReturnDTO, UserUpdateDTO>().ReverseMap();
            CreateMap<UserLoginReturnDTO, UserReturnDTO>().ReverseMap();

            CreateMap<UserUpdateDTO, UserReturnDTO>().ReverseMap();


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

            #region Artist

            CreateMap<Artist, ArtistAddDTO>().ReverseMap();
            CreateMap<Artist, ArtistReturnDTO>().ReverseMap();
            CreateMap<Artist, ArtistUpdateDTO>().ReverseMap();


            CreateMap<ArtistAddDTO, ArtistReturnDTO>().ReverseMap();
            CreateMap<ArtistAddDTO, ArtistUpdateDTO>().ReverseMap();

            CreateMap<ArtistReturnDTO, ArtistUpdateDTO>().ReverseMap();




            #endregion

            #region Album

            CreateMap<Album, AlbumAddDTO>().ReverseMap();
            CreateMap<Album, AlbumReturnDTO>().ReverseMap();
            CreateMap<Album, AlbumUpdateDTO>().ReverseMap();

            CreateMap<AlbumAddDTO, AlbumReturnDTO>().ReverseMap();
            CreateMap<AlbumAddDTO, AlbumUpdateDTO>().ReverseMap();

            CreateMap<AlbumReturnDTO, AlbumUpdateDTO>().ReverseMap();

            #endregion

            #region Favorite
            CreateMap<Favorite, FavoriteDTO>().ReverseMap();
            CreateMap<Favorite, FavoriteReturnDTO>().ReverseMap();

            CreateMap<FavoriteDTO, FavoriteReturnDTO>().ReverseMap();
            #endregion

            #region Rating
            CreateMap<Rating, RatingDTO>().ReverseMap();
            CreateMap<Rating, RatingReturnDTO>().ReverseMap();
            CreateMap<Rating, SongRatingDTO>().ReverseMap();

            CreateMap<RatingDTO, RatingReturnDTO>().ReverseMap();
            CreateMap<RatingDTO, SongRatingDTO>().ReverseMap();

            CreateMap<RatingReturnDTO, SongRatingDTO>().ReverseMap();


            #endregion
        }
    }
}
