# Vibe Vault - Music Management System

## Overview

**Vibe Vault - Music Management System** is a comprehensive platform designed to manage music-related operations. The system supports multiple user roles including Admin, Premium User, Normal User, and Artist. It features functionalities such as song management, playlist creation, favorite songs, and song ratings. The system also incorporates advanced technologies like Watchdog for logging, Hangfire for background jobs, and Stripe for payment processing and webhooks. Additionally, email notifications are integrated to enhance user engagement.

## Table of Contents

- [Features](#features)
- [Technologies](#technologies)
- [Installation](#installation)
- [Configuration](#configuration)
- [Usage](#usage)
- [API Endpoints](#api-endpoints)
- [Background Jobs](#background-jobs)
- [Payment Processing](#payment-processing)
- [Email Notifications](#email-notifications)
- [Contributing](#contributing)
- [License](#license)

## Features

- **User Management**: Admin, Premium User, Normal User, Artist
- **Music Management**: Songs, Playlists, Favorites, Ratings
- **Background Jobs**: Subscription checks, email notifications using Hangfire
- **Payment Processing**: Stripe integration and webhooks
- **Logging and Monitoring**: Watchdog for detailed logs
- **Email Notifications**: Automated email system for notifications and updates


## Testing

### NUNIT TESTING
- Tested Services
- Tested Repositories

![WhatsApp Image 2024-08-03 at 18 03 22_0733e60c](https://github.com/user-attachments/assets/e5652f20-b09d-4093-aba3-b7c8cf86149a)



## Project Structure

```
MusicApplicationSolution
├─ .dockerignore
├─ .github
│  └─ workflows
│     └─ master_vibevaultbackendapp.yml
├─ .gitignore
├─ azure-pipelines.yml
├─ Dockerfile
├─ MusicApplicationAPI
│  ├─ appsettings.Development.json
│  ├─ appsettings.json
│  ├─ Contexts
│  │  └─ MusicManagementContext.cs
│  ├─ Controllers
│  │  └─ v1
│  │     ├─ AdminController.cs
│  │     ├─ AlbumsController.cs
│  │     ├─ ArtistsController.cs
│  │     ├─ AuthController.cs
│  │     ├─ CheckoutController.cs
│  │     ├─ FavoritesController.cs
│  │     ├─ PlaylistsController.cs
│  │     ├─ PlaylistSongsController.cs
│  │     ├─ RatingsController.cs
│  │     ├─ SongsController.cs
│  │     ├─ StripeWebhookController.cs
│  │     └─ UsersController.cs
│  ├─ Exceptions
│  │  ├─ AlbumExceptions
│  │  │  ├─ NoAlbumsExistsException.cs
│  │  │  ├─ NoSuchExistAlbumException.cs
│  │  │  ├─ UnableToAddAlbumException.cs
│  │  │  ├─ UnableToDeleteAlbumException.cs
│  │  │  └─ UnableToUpdateAlbumException.cs
│  │  ├─ ArtistExceptions
│  │  │  ├─ ArtistNameAlreadyExists.cs
│  │  │  ├─ ArtistNotActiveException.cs
│  │  │  ├─ NoArtistsExistsException.cs
│  │  │  ├─ NoSuchArtistExistException.cs
│  │  │  ├─ UnableToAddArtistException.cs
│  │  │  ├─ UnableToDeleteArtistException.cs
│  │  │  └─ UnableToUpdateArtistException.cs
│  │  ├─ EmailExceptions
│  │  │  ├─ EmailNotVerifiedException.cs
│  │  │  ├─ EmailVerificationNotFoundException.cs
│  │  │  ├─ InvalidEmailVerificationCodeException.cs
│  │  │  ├─ UnableToSendMailException.cs
│  │  │  └─ VerificationExpiredException.cs
│  │  ├─ FavoriteExceptions
│  │  │  ├─ AlreadyMarkedAsFavorite.cs
│  │  │  ├─ NoFavoritesExistsException.cs
│  │  │  ├─ NoSuchFavoriteExistException.cs
│  │  │  ├─ NotMarkedAsFavorite.cs
│  │  │  ├─ UnableToAddFavoriteException.cs
│  │  │  ├─ UnableToDeleteFavoriteException.cs
│  │  │  ├─ UnableToRetrieveFavoritesException.cs
│  │  │  └─ UnableToUpdateFavoriteException.cs
│  │  ├─ PlaylistExceptions
│  │  │  ├─ MaximumPlaylistsReachedException.cs
│  │  │  ├─ NoPlaylistsExistsException.cs
│  │  │  ├─ NoSuchPlaylistExistException.cs
│  │  │  ├─ UnableToAddPlaylistException.cs
│  │  │  ├─ UnableToDeletePlaylistException.cs
│  │  │  └─ UnableToUpdatePlaylistException.cs
│  │  ├─ PlaylistSongExceptions
│  │  │  ├─ NoPlaylistSongsExistsException.cs
│  │  │  ├─ NoSongsInPlaylistException.cs
│  │  │  ├─ NoSuchPlaylistSongException.cs
│  │  │  ├─ UnableToAddPlaylistSongException.cs
│  │  │  ├─ UnableToClearPlaylistException.cs
│  │  │  ├─ UnableToDeletePlaylistSongException.cs
│  │  │  └─ UnableToUpdatePlaylistSongException.cs
│  │  ├─ RatingExceptions
│  │  │  ├─ AlreadyRatedException.cs
│  │  │  ├─ NoRatingsExistsException.cs
│  │  │  ├─ NoSuchRatingExistException.cs
│  │  │  ├─ UnableToAddRatingException.cs
│  │  │  ├─ UnableToDeleteRatingException.cs
│  │  │  ├─ UnableToRetrieveTopRatedSongsException.cs
│  │  │  └─ UnableToUpdateRatingException.cs
│  │  ├─ SongExceptions
│  │  │  ├─ InvalidGenreException.cs
│  │  │  ├─ InvalidSongDuration.cs
│  │  │  ├─ NoSongsExistsException.cs
│  │  │  ├─ NoSuchSongExistException.cs
│  │  │  ├─ UnableToAddSongException.cs
│  │  │  ├─ UnableToDeleteSongException.cs
│  │  │  └─ UnableToUpdateSongException.cs
│  │  └─ UserExceptions
│  │     ├─ ActiveSubscriptionException.cs
│  │     ├─ DuplicateEmailException.cs
│  │     ├─ InvalidPasswordException.cs
│  │     ├─ NoSuchPremiumUserExistException.cs
│  │     ├─ NoSuchUserExistException.cs
│  │     ├─ NoUsersExistsException.cs
│  │     ├─ PremiumSubscriptionExpiredException.cs
│  │     ├─ UnableToAddUserException.cs
│  │     ├─ UnableToDeleteUserException.cs
│  │     ├─ UnableToUpdateUserException.cs
│  │     └─ UnauthorizedUserException.cs
│  ├─ Interfaces
│  │  ├─ Repository
│  │  │  ├─ IAlbumRepository.cs
│  │  │  ├─ IArtistRepository.cs
│  │  │  ├─ IEmailVerificationRepository.cs
│  │  │  ├─ IFavoriteRepository.cs
│  │  │  ├─ IPlaylistRepository.cs
│  │  │  ├─ IPlaylistSongRepository.cs
│  │  │  ├─ IPremiumUserRepository.cs
│  │  │  ├─ IRatingRepository.cs
│  │  │  ├─ IRepository.cs
│  │  │  ├─ ISongRepository.cs
│  │  │  └─ IUserRepository.cs
│  │  └─ Service
│  │     ├─ AuthService
│  │     │  ├─ IAuthLoginService.cs
│  │     │  └─ IAuthRegisterService.cs
│  │     ├─ IAlbumService.cs
│  │     ├─ IArtistService.cs
│  │     ├─ IEmailSender.cs
│  │     ├─ IEmailVerificationService.cs
│  │     ├─ IFavoriteService.cs
│  │     ├─ IPasswordService.cs
│  │     ├─ IPlaylistService.cs
│  │     ├─ IPlaylistSongService.cs
│  │     ├─ IRatingService.cs
│  │     ├─ ISongService.cs
│  │     ├─ IUserService.cs
│  │     └─ TokenService
│  │        └─ ITokenService.cs
│  ├─ log4net.config
│  ├─ Mappers
│  │  └─ MappingProfile.cs
│  ├─ Migrations
│  │  ├─ 20240802104149_AzureInitialMigration.cs
│  │  ├─ 20240802104149_AzureInitialMigration.Designer.cs
│  │  └─ MusicManagementContextModelSnapshot.cs
│  ├─ Models
│  │  ├─ DbModels
│  │  │  ├─ Album.cs
│  │  │  ├─ Artist.cs
│  │  │  ├─ CheckOutRequest.cs
│  │  │  ├─ EmailVerification.cs
│  │  │  ├─ Favorite.cs
│  │  │  ├─ Playlist.cs
│  │  │  ├─ PlaylistSong.cs
│  │  │  ├─ PremiumUser.cs
│  │  │  ├─ Rating.cs
│  │  │  ├─ Song.cs
│  │  │  └─ User.cs
│  │  ├─ DTOs
│  │  │  ├─ AlbumDTO
│  │  │  │  ├─ AlbumAddDTO.cs
│  │  │  │  ├─ AlbumReturnDTO.cs
│  │  │  │  └─ AlbumUpdateDTO.cs
│  │  │  ├─ ArtistDTO
│  │  │  │  ├─ ArtistAddDTO.cs
│  │  │  │  ├─ ArtistLoginDTO.cs
│  │  │  │  ├─ ArtistLoginReturnDTO.cs
│  │  │  │  ├─ ArtistReturnDTO.cs
│  │  │  │  └─ ArtistUpdateDTO.cs
│  │  │  ├─ FavoriteDTO
│  │  │  │  ├─ FavoriteDTO.cs
│  │  │  │  ├─ FavoritePlaylistDTO.cs
│  │  │  │  ├─ FavoriteReturnDTO.cs
│  │  │  │  └─ FavoriteSongDTO.cs
│  │  │  ├─ OtherDTO
│  │  │  │  └─ ChangePasswordRequestDTO.cs
│  │  │  ├─ PlaylistDTO
│  │  │  │  ├─ PlaylistAddDTO.cs
│  │  │  │  ├─ PlaylistReturnDTO.cs
│  │  │  │  └─ PlaylistUpdateDTO.cs
│  │  │  ├─ PlaylistSongDTO
│  │  │  │  ├─ PlaylistSongDTO.cs
│  │  │  │  └─ PlaylistSongReturnDTO.cs
│  │  │  ├─ RatingDTO
│  │  │  │  ├─ RatingDTO.cs
│  │  │  │  ├─ RatingReturnDTO.cs
│  │  │  │  └─ SongRatingDTO.cs
│  │  │  ├─ SongDTO
│  │  │  │  ├─ SongAddDTO.cs
│  │  │  │  ├─ SongReturnDTO.cs
│  │  │  │  └─ SongUpdateDTO.cs
│  │  │  └─ UserDTO
│  │  │     ├─ PremiumRequestDTO.cs
│  │  │     ├─ UserLoginDTO.cs
│  │  │     ├─ UserLoginReturnDTO.cs
│  │  │     ├─ UserRegisterDTO.cs
│  │  │     ├─ UserRegisterReturnDTO.cs
│  │  │     ├─ UserReturnDTO.cs
│  │  │     ├─ UserUpdateDTO.cs
│  │  │     └─ VerificationCode.cs
│  │  ├─ Enums
│  │  │  ├─ GenreType.cs
│  │  │  └─ RoleType.cs
│  │  └─ ErrorModels
│  │     └─ ErrorModel.cs
│  ├─ MusicApplicationAPI.csproj
│  ├─ Program.cs
│  ├─ Properties
│  │  └─ launchSettings.json
│  ├─ Repositories
│  │  ├─ AlbumRepository.cs
│  │  ├─ ArtistRepository.cs
│  │  ├─ EmailVerificationRepository.cs
│  │  ├─ FavoriteRepository.cs
│  │  ├─ PlaylistRepository.cs
│  │  ├─ PlaylistSongRepository.cs
│  │  ├─ PremiumUserRepository.cs
│  │  ├─ RatingRepository.cs
│  │  ├─ SongRepository.cs
│  │  └─ UserRepository.cs
│  ├─ Services
│  │  ├─ AlbumService
│  │  │  └─ AlbumService.cs
│  │  ├─ ArtistService
│  │  │  └─ ArtistService.cs
│  │  ├─ EmailService
│  │  │  ├─ EmailSenderService.cs
│  │  │  └─ EmailVerificationService.cs
│  │  ├─ FavoriteService
│  │  │  └─ FavoriteService.cs
│  │  ├─ PasswordService.cs
│  │  ├─ PlaylistService
│  │  │  └─ PlaylistService.cs
│  │  ├─ PlaylistSongService
│  │  │  └─ PlaylistSongService.cs
│  │  ├─ RatingService
│  │  │  └─ RatingService.cs
│  │  ├─ SongService
│  │  │  └─ SongService.cs
│  │  ├─ StripeService.cs
│  │  ├─ SubscriptionService.cs
│  │  ├─ TokenService
│  │  │  └─ JwtTokenService.cs
│  │  └─ UserService
│  │     ├─ UserAuthService.cs
│  │     └─ UserService.cs
│  └─ WeatherForecast.cs
├─ MusicApplicationSolution.sln
└─ MusicApplicationUnitTesting

```

## Technologies

- **Backend**: ASP.NET Core
- **Database**: SQL Server
- **ORM**: Entity Framework Core
- **Authentication**: JWT
- **Logging**: Watchdog
- **Background Jobs**: Hangfire
- **Payment Gateway**: Stripe
- **Email**: SMTP (Simple Mail Transfer Protocol)
- **Others**: AutoMapper, Swagger

## Installation

1. **Clone the repository**:

   ```bash
   git clone https://github.com/RajKousik/Capstone-Backend-Genspark/tree/master
   cd MusicApplicationAPI
   ```

2. **Setup the database**:

   - Create a new database in SQL Server.
   - Update the connection string in `appsettings.json`.

3. **Install dependencies**:

   ```bash
   dotnet restore
   ```

4. **Apply migrations**:

   ```bash
   dotnet ef database update
   ```

5. **Run the application**:
   ```bash
   dotnet run
   ```

## Configuration

Configure the following settings in `appsettings.json`:

- **ConnectionStrings**:

  ```json
  "ConnectionStrings": {
    "DefaultConnection": "Server=your_server;Database=your_database;User Id=your_user;Password=your_password;"
  }
  "ConnectionStrings": {
    "defaultConnection": "Server=your_server;Database=your_database;User Id=your_user;Password=your_password;",
    "WatchDogConnection": "Server=your_server;Database=your_database;User Id=your_user;Password=your_password;",
    "HangFireConnection": "Server=your_server;Database=your_database;User Id=your_user;Password=your_password;"
  },
  ```

- **JWT**:

  ```json
  "TokenKey": {
    "JWT": "your_secret_key"
  },
  ```

- **Stripe**:

  ```json
  "Stripe": {
    "SecretKey": "your_secret_key" // Replace with your actual secret key
  },
  ```

- **Webhooks**:

  ```json
  "WebHooks": {
    "Stripe": {
      "SecretKey": "your_secret_key" // Replace with your actual secret key
    }
  },
  ```

- **FrontEnd URL**:

```json
 "FrontEnd": {
    "Domain": "your_domain", // Replace with your,
    "SuccessUrl": "your_success_url",
    "CancelUrl": "your_cancel_url"
  }
```

## Usage

### User Roles

- **Admin**: Manage all aspects of the system.
- **Premium User**: Access premium features such as unlimited playlist creation.
- **Normal User**: Access basic features with limitations.
- **Artist**: Manage their own songs and albums.

### Music Management

- **Songs**: CRUD operations for songs.
- **Playlists**: Create, update, and delete playlists.
- **Favorites**: Mark and unmark songs as favorites.
- **Ratings**: Rate songs.

## Background Jobs

- **Subscription Checks**: Scheduled using Hangfire to check for subscriptions ending in the next two days and send email notifications if necessary.

## Payment Processing

- **Stripe Integration**: Handles payment processing for premium subscriptions.
- **Stripe Webhooks**: Listens to Stripe events to update subscription status.

## Email Notifications

- **SMTP Integration**: Sends email notifications for various events such as subscription renewals, account verification, etc.

## Contributing

1. Fork the repository.
2. Create a new feature branch (`git checkout -b feature/your-feature`).
3. Commit your changes (`git commit -m 'Add your feature'`).
4. Push to the branch (`git push origin feature/your-feature`).
5. Open a Pull Request.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

---

For detailed documentation, please refer to the API documentation provided with Swagger in the application.
