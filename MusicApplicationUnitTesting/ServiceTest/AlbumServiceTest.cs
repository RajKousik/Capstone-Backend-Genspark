using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using MusicApplicationAPI.Exceptions.AlbumExceptions;
using MusicApplicationAPI.Exceptions.ArtistExceptions;
using MusicApplicationAPI.Interfaces.Repository;
using MusicApplicationAPI.Models.DbModels;
using MusicApplicationAPI.Models.DTOs.AlbumDTO;
using MusicApplicationAPI.Services;

public class AlbumServiceTests
{
    private readonly Mock<IAlbumRepository> _mockAlbumRepository;
    private readonly Mock<IArtistRepository> _mockArtistRepository;
    private readonly Mock<ISongRepository> _mockSongRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<AlbumService>> _mockLogger;
    private readonly AlbumService _albumService;

    public AlbumServiceTests()
    {
        _mockAlbumRepository = new Mock<IAlbumRepository>();
        _mockArtistRepository = new Mock<IArtistRepository>();
        _mockSongRepository = new Mock<ISongRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<AlbumService>>();
        _albumService = new AlbumService(
            _mockAlbumRepository.Object,
            _mockArtistRepository.Object,
            _mockMapper.Object,
            _mockLogger.Object,
            _mockSongRepository.Object
        );
    }

    [Test]
    public async Task AddAlbum_ShouldReturnAlbum_WhenSuccessful()
    {
        // Arrange
        var albumAddDTO = new AlbumAddDTO { ArtistId = 1 };
        var artist = new Artist { ArtistId = 1 };
        var album = new Album();
        var addedAlbum = new Album();
        var albumReturnDTO = new AlbumReturnDTO();

        _mockArtistRepository.Setup(repo => repo.GetById(1)).ReturnsAsync(artist);
        _mockMapper.Setup(m => m.Map<Album>(albumAddDTO)).Returns(album);
        _mockAlbumRepository.Setup(repo => repo.Add(album)).ReturnsAsync(addedAlbum);
        _mockMapper.Setup(m => m.Map<AlbumReturnDTO>(addedAlbum)).Returns(albumReturnDTO);

        // Act
        var result = await _albumService.AddAlbum(albumAddDTO);

        // Assert
        Assert.AreEqual(albumReturnDTO, result);
        _mockAlbumRepository.Verify(repo => repo.Add(album), Times.Once);
    }

    [Test]
    public async Task AddAlbum_ShouldThrowNoSuchArtistExistException_WhenArtistDoesNotExist()
    {
        // Arrange
        var albumAddDTO = new AlbumAddDTO { ArtistId = 1 };
        _mockArtistRepository.Setup(repo => repo.GetById(1)).ReturnsAsync((Artist)null);

        // Act & Assert
        Assert.ThrowsAsync<NoSuchArtistExistException>(async() => await _albumService.AddAlbum(albumAddDTO));
    }

    [Test]
    public async Task DeleteRangeAlbums_ShouldReturnTrue_WhenSuccessful()
    {
        // Arrange
        var albumIds = new List<int> { 1, 2, 3 };
        _mockAlbumRepository.Setup(repo => repo.DeleteRange(albumIds));

        // Act
        var result = await _albumService.DeleteRangeAlbums(albumIds);

        // Assert
        Assert.True(result);
    }

    [Test]
    public async Task UpdateAlbum_ShouldReturnUpdatedAlbum_WhenSuccessful()
    {
        // Arrange
        var albumId = 1;
        var albumUpdateDTO = new AlbumUpdateDTO();
        var album = new Album();
        var updatedAlbum = new Album();
        var albumReturnDTO = new AlbumReturnDTO();

        _mockAlbumRepository.Setup(repo => repo.GetById(albumId)).ReturnsAsync(album);
        _mockMapper.Setup(m => m.Map(albumUpdateDTO, album)).Returns(album);
        _mockAlbumRepository.Setup(repo => repo.Update(album)).ReturnsAsync(updatedAlbum);
        _mockMapper.Setup(m => m.Map<AlbumReturnDTO>(updatedAlbum)).Returns(albumReturnDTO);

        // Act
        var result = await _albumService.UpdateAlbum(albumId, albumUpdateDTO);

        // Assert
        Assert.AreEqual(albumReturnDTO, result);
        _mockAlbumRepository.Verify(repo => repo.Update(album), Times.Once);
    }

    [Test]
    public async Task GetAlbumById_ShouldReturnAlbum_WhenSuccessful()
    {
        // Arrange
        var albumId = 1;
        var album = new Album();
        var albumReturnDTO = new AlbumReturnDTO();

        _mockAlbumRepository.Setup(repo => repo.GetById(albumId)).ReturnsAsync(album);
        _mockMapper.Setup(m => m.Map<AlbumReturnDTO>(album)).Returns(albumReturnDTO);

        // Act
        var result = await _albumService.GetAlbumById(albumId);

        // Assert
        Assert.AreEqual(albumReturnDTO, result);
    }

    [Test]
    public async Task GetAllAlbums_ShouldReturnAllAlbums_WhenSuccessful()
    {
        // Arrange
        var albums = new List<Album> { new Album(), new Album() };
        var albumReturnDTOs = new List<AlbumReturnDTO> { new AlbumReturnDTO(), new AlbumReturnDTO() };

        _mockAlbumRepository.Setup(repo => repo.GetAll()).ReturnsAsync(albums);
        _mockMapper.Setup(m => m.Map<IEnumerable<AlbumReturnDTO>>(albums)).Returns(albumReturnDTOs);

        // Act
        var result = await _albumService.GetAllAlbums();

        // Assert
        Assert.AreEqual(albumReturnDTOs, result);
    }

    [Test]
    public async Task GetAlbumsByArtistId_ShouldReturnAlbums_WhenSuccessful()
    {
        // Arrange
        var artistId = 1;
        var albums = new List<Album> { new Album { ArtistId = artistId } };
        var albumReturnDTOs = new List<AlbumReturnDTO> { new AlbumReturnDTO() };

        _mockArtistRepository.Setup(repo => repo.GetById(artistId)).ReturnsAsync(new Artist { ArtistId = artistId });
        _mockAlbumRepository.Setup(repo => repo.GetAll()).ReturnsAsync(albums);
        _mockMapper.Setup(m => m.Map<IEnumerable<AlbumReturnDTO>>(albums)).Returns(albumReturnDTOs);

        // Act
        var result = await _albumService.GetAlbumsByArtistId(artistId);

        // Assert
        Assert.AreEqual(albumReturnDTOs, result);
    }

    [Test]
    public async Task DeleteAlbum_ShouldReturnDeletedAlbum_WhenSuccessful()
    {
        // Arrange
        var albumId = 1;
        var album = new Album();
        var deletedAlbum = new Album();
        var albumReturnDTO = new AlbumReturnDTO();

        _mockAlbumRepository.Setup(repo => repo.GetById(albumId)).ReturnsAsync(album);
        _mockAlbumRepository.Setup(repo => repo.Delete(albumId)).ReturnsAsync(deletedAlbum);
        _mockMapper.Setup(m => m.Map<AlbumReturnDTO>(deletedAlbum)).Returns(albumReturnDTO);

        // Act
        var result = await _albumService.DeleteAlbum(albumId);

        // Assert
        Assert.AreEqual(albumReturnDTO, result);
    }

    [Test]
    public async Task DeleteAlbum_ShouldThrowNoSuchAlbumExistException_WhenAlbumDoesNotExist()
    {
        // Arrange
        var albumId = 1;
        _mockAlbumRepository.Setup(repo => repo.GetById(albumId)).ReturnsAsync((Album)null);

        // Act & Assert
        Assert.ThrowsAsync<NoSuchAlbumExistException>(async() => await _albumService.DeleteAlbum(albumId));
    }
}
