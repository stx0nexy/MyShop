using System.Threading;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;

namespace Catalog.UnitTests.Services;

public class CatalogBrandServiceTest
{
    private readonly ICatalogBrandService _catalogBrandService;

    private readonly Mock<ICatalogBrandRepository> _catalogBrandRepository;
    private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
    private readonly Mock<ILogger<CatalogService>> _logger;
    private readonly Mock<IMapper> _mapper;

    public CatalogBrandServiceTest()
    {
        _catalogBrandRepository = new Mock<ICatalogBrandRepository>();
        _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
        _logger = new Mock<ILogger<CatalogService>>();
        _mapper = new Mock<IMapper>();

        var dbContextTransaction = new Mock<IDbContextTransaction>();
        _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

        _catalogBrandService = new CatalogBrandService(_dbContextWrapper.Object, _logger.Object, _catalogBrandRepository.Object, _mapper.Object);
    }

    [Fact]
    public async Task AddAsync_Success()
    {
        // arrange
        var testResult = 1;
        var testBrand = "test";

        _catalogBrandRepository.Setup(s => s.Add(
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogBrandService.Add(testBrand);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task AddAsync_Failed()
    {
        // arrange
        int? testResult = null;
        var testBrand = "test";

        _catalogBrandRepository.Setup(s => s.Add(
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogBrandService.Add(testBrand);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task DeleteAsync_Success()
    {
        // arrange
        var testResult = true;
        var testId = 1;

        _catalogBrandRepository.Setup(s => s.DeleteAsync(
            It.Is<int>(i => i == testId))).ReturnsAsync(testResult);

        // act
        var result = await _catalogBrandService.Delete(testId);

        // assert
        result.Should().Be(testResult);
    }

    [Fact]
    public async Task DeleteAsync_Failed()
    {
        // arrange
        var testId = 1000;
        bool? testResult = null;

        _catalogBrandRepository.Setup(s => s.DeleteAsync(
            It.Is<int>(i => i == testId))).ReturnsAsync(testResult);

        // act
        var result = await _catalogBrandService.Delete(testId);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_Success()
    {
        // arrange
        var catalogBrandSuccess = new CatalogBrand()
        {
            Id = 1,
            Brand = "TestBrand"
        };
        var catalogBrandDtoSuccess = new CatalogBrandDto()
        {
            Id = 1,
            Brand = "TestBrand"
        };
        _catalogBrandRepository.Setup(s => s.UpdateAsync(
            It.IsAny<CatalogBrand>())).ReturnsAsync(catalogBrandSuccess);
        _mapper.Setup(s => s.Map<CatalogBrandDto>(
            It.Is<CatalogBrand>(i => i.Equals(catalogBrandSuccess)))).Returns(catalogBrandDtoSuccess);

        // act
        var result = await _catalogBrandService.Update(catalogBrandSuccess.Id, catalogBrandSuccess.Brand);

        // assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateAsync_Failed()
    {
        // arrange
        var testId = 1;
        var testBrand = "test";
        _catalogBrandRepository.Setup(s => s.UpdateAsync(
            It.IsAny<CatalogBrand>())).Returns((Func<CatalogBrandDto>)null!);

        // act
        var result = await _catalogBrandService.Update(testId, testBrand);

        // assert
        result.Should().BeNull();
    }
}