using System.Threading;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;

namespace Catalog.UnitTests.Services;

public class CatalogTypeServiceTest
{
     private readonly ICatalogTypeService _catalogTypeService;

     private readonly Mock<ICatalogTypeRepository> _catalogTypeRepository;
     private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
     private readonly Mock<ILogger<CatalogService>> _logger;
     private readonly Mock<IMapper> _mapper;

     public CatalogTypeServiceTest()
    {
        _catalogTypeRepository = new Mock<ICatalogTypeRepository>();
        _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
        _logger = new Mock<ILogger<CatalogService>>();
        _mapper = new Mock<IMapper>();

        var dbContextTransaction = new Mock<IDbContextTransaction>();
        _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

        _catalogTypeService = new CatalogTypeService(_dbContextWrapper.Object, _logger.Object, _catalogTypeRepository.Object, _mapper.Object);
    }

     [Fact]
     public async Task AddAsync_Success()
    {
        // arrange
        var testResult = 1;
        var testType = "test";

        _catalogTypeRepository.Setup(s => s.Add(
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogTypeService.Add(testType);

        // assert
        result.Should().Be(testResult);
    }

     [Fact]
     public async Task AddAsync_Failed()
    {
        // arrange
        int? testResult = null;
        var testType = "test";

        _catalogTypeRepository.Setup(s => s.Add(
            It.IsAny<string>())).ReturnsAsync(testResult);

        // act
        var result = await _catalogTypeService.Add(testType);

        // assert
        result.Should().Be(testResult);
    }

     [Fact]
     public async Task DeleteAsync_Success()
    {
        // arrange
        var testResult = true;
        var testId = 1;

        _catalogTypeRepository.Setup(s => s.DeleteAsync(
            It.Is<int>(i => i == testId))).ReturnsAsync(testResult);

        // act
        var result = await _catalogTypeService.Delete(testId);

        // assert
        result.Should().Be(testResult);
    }

     [Fact]
     public async Task DeleteAsync_Failed()
    {
        // arrange
        var testId = 1000;
        bool? testResult = null;

        _catalogTypeRepository.Setup(s => s.DeleteAsync(
            It.Is<int>(i => i == testId))).ReturnsAsync(testResult);

        // act
        var result = await _catalogTypeService.Delete(testId);

        // assert
        result.Should().BeNull();
    }

     [Fact]
     public async Task UpdateAsync_Success()
    {
        // arrange
        var catalogTypeSuccess = new CatalogType()
        {
            Id = 1,
            Type = "TestBrand"
        };
        var catalogTypeDtoSuccess = new CatalogTypeDto()
        {
            Id = 1,
            Type = "TestBrand"
        };
        _catalogTypeRepository.Setup(s => s.UpdateAsync(
            It.IsAny<CatalogType>())).ReturnsAsync(catalogTypeSuccess);
        _mapper.Setup(s => s.Map<CatalogTypeDto>(
            It.Is<CatalogType>(i => i.Equals(catalogTypeSuccess)))).Returns(catalogTypeDtoSuccess);

        // act
        var result = await _catalogTypeService.Update(catalogTypeSuccess.Id, catalogTypeSuccess.Type);

        // assert
        result.Should().NotBeNull();
    }

     [Fact]
     public async Task UpdateAsync_Failed()
    {
        // arrange
        var testId = 1;
        var testType = "test";
        _catalogTypeRepository.Setup(s => s.UpdateAsync(
            It.IsAny<CatalogType>())).Returns((Func<CatalogTypeDto>)null!);

        // act
        var result = await _catalogTypeService.Update(testId, testType);

        // assert
        result.Should().BeNull();
    }
}