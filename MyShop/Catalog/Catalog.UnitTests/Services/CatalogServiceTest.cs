using System.Threading;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Dtos;
using Catalog.Host.Models.Response;

namespace Catalog.UnitTests.Services;

public class CatalogServiceTest
{
    private readonly ICatalogService _catalogService;

    private readonly Mock<ICatalogItemRepository> _catalogItemRepository;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
    private readonly Mock<ILogger<CatalogService>> _logger;

    public CatalogServiceTest()
    {
        _catalogItemRepository = new Mock<ICatalogItemRepository>();
        _mapper = new Mock<IMapper>();
        _dbContextWrapper = new Mock<IDbContextWrapper<ApplicationDbContext>>();
        _logger = new Mock<ILogger<CatalogService>>();

        var dbContextTransaction = new Mock<IDbContextTransaction>();
        _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

        _catalogService = new CatalogService(_dbContextWrapper.Object, _logger.Object, _catalogItemRepository.Object, _mapper.Object);
    }

    [Fact]
    public async Task GetCatalogItemsAsync_Success()
    {
        // arrange
        var testPageIndex = 0;
        var testPageSize = 4;
        var testTotalCount = 12;

        var pagingPaginatedItemsSuccess = new PaginatedItems<CatalogItem>()
        {
            Data = new List<CatalogItem>()
            {
                new CatalogItem()
                {
                    Name = "TestName",
                },
            },
            TotalCount = testTotalCount,
        };

        var catalogItemSuccess = new CatalogItem()
        {
            Name = "TestName"
        };

        var catalogItemDtoSuccess = new CatalogItemDto()
        {
            Name = "TestName"
        };

        _catalogItemRepository.Setup(s => s.GetByPageAsync(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize),
            It.IsAny<int?>(),
            It.IsAny<int?>())).ReturnsAsync(pagingPaginatedItemsSuccess);

        _mapper.Setup(s => s.Map<CatalogItemDto>(
            It.Is<CatalogItem>(i => i.Equals(catalogItemSuccess)))).Returns(catalogItemDtoSuccess);

        // act
        var result = await _catalogService.GetCatalogItemsAsync(testPageSize, testPageIndex, null);

        // assert
        result.Should().NotBeNull();
        result?.Data.Should().NotBeNull();
        result?.Count.Should().Be(testTotalCount);
        result?.PageIndex.Should().Be(testPageIndex);
        result?.PageSize.Should().Be(testPageSize);
    }

    [Fact]
    public async Task GetCatalogItemsAsync_Failed()
    {
        // arrange
        var testPageIndex = 1000;
        var testPageSize = 10000;

        _catalogItemRepository.Setup(s => s.GetByPageAsync(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize),
            It.IsAny<int?>(),
            It.IsAny<int?>())).Returns((Func<PaginatedItemsResponse<CatalogItemDto>>)null!);

        // act
        var result = await _catalogService.GetCatalogItemsAsync(testPageSize, testPageIndex, null);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetCatalogItemByIdAsync_Success()
    {
        // arrange
        var catalogItemSuccess = new CatalogItem()
        {
            Id = 1,
            Name = "Name",
            Description = "Description",
            Price = 1000,
            AvailableStock = 100,
            CatalogBrandId = 1,
            CatalogTypeId = 1,
            PictureFileName = "1.png"
        };
        var catalogItemDtoSuccess = new CatalogItemDto()
        {
            Id = 1,
            Name = "Name",
            Description = "Description",
            Price = 1000,
            AvailableStock = 100,
            CatalogBrand = new CatalogBrandDto() { Id = 1, Brand = "testBrand" },
            CatalogType = new CatalogTypeDto() { Id = 1, Type = "testType" },
            PictureUrl = "www.alevelwebsite.com/assets/img/1"
        };
        _catalogItemRepository.Setup(s => s.GetByIdAsync(
            It.Is<int>(i => i == catalogItemSuccess.Id))).ReturnsAsync(catalogItemSuccess);
        _mapper.Setup(s => s.Map<CatalogItemDto>(
            It.Is<CatalogItem>(i => i.Equals(catalogItemSuccess)))).Returns(catalogItemDtoSuccess);

        // act
        var result = await _catalogService.GetCatalogItemByIdAsync(catalogItemSuccess.Id);

        // assert
        result.Should().NotBeNull();
        result.CatalogBrand.Should().NotBeNull();
        result.CatalogType.Should().NotBeNull();
    }

    [Fact]
    public async Task GetCatalogItemByIdAsync_Failed()
    {
        // arrange
        var testId = 1000;
        _catalogItemRepository.Setup(s => s.GetByIdAsync(
            It.IsAny<int>())).Returns((Func<CatalogItemDto>)null!);

        // act
        var result = await _catalogService.GetCatalogItemByIdAsync(testId);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetCatalogItemByBrandAsync_Success()
    {
        // arrange
        var testPageIndex = 0;
        var testPageSize = 4;
        var testTotalCount = 12;
        var testBrand = "test";
        var pagingPaginatedItemsSuccess = new PaginatedItems<CatalogItem>()
        {
            Data = new List<CatalogItem>()
            {
                new CatalogItem()
                {
                    Name = "TestName",
                },
            },
            TotalCount = testTotalCount,
        };
        var catalogItemSuccess = new CatalogItem()
        {
            Id = 1,
            Name = "Name",
            Description = "Description",
            Price = 1000,
            AvailableStock = 100,
            CatalogBrandId = 1,
            CatalogTypeId = 1,
            PictureFileName = "1.png"
        };
        var catalogItemDtoSuccess = new CatalogItemDto()
        {
            Id = 1,
            Name = "Name",
            Description = "Description",
            Price = 1000,
            AvailableStock = 100,
            CatalogBrand = new CatalogBrandDto() { Id = 1, Brand = "testBrand" },
            CatalogType = new CatalogTypeDto() { Id = 1, Type = "testType" },
            PictureUrl = "www.alevelwebsite.com/assets/img/1"
        };
        _catalogItemRepository.Setup(s => s.GetByBrandAsync(
            It.Is<string>(i => i == testBrand),
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize))).ReturnsAsync(pagingPaginatedItemsSuccess);
        _mapper.Setup(s => s.Map<CatalogItemDto>(
            It.Is<CatalogItem>(i => i.Equals(catalogItemSuccess)))).Returns(catalogItemDtoSuccess);

        // act
        var result = await _catalogService.GetCatalogItemByBrandAsync(testBrand, testPageSize, testPageIndex);

        // assert
        result.Should().NotBeNull();
        result?.Data.Should().NotBeNull();
        result?.Count.Should().Be(testTotalCount);
        result?.PageIndex.Should().Be(testPageIndex);
        result?.PageSize.Should().Be(testPageSize);
    }

    [Fact]
    public async Task GetCatalogItemByBrandAsync_Failed()
    {
        // arrange
        var testPageIndex = 1000;
        var testPageSize = 10000;
        var testBrand = "test";

        _catalogItemRepository.Setup(s => s.GetByBrandAsync(
            It.Is<string>(i => i == testBrand),
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize))).Returns((Func<PaginatedItemsResponse<CatalogItemDto>>)null!);

        // act
        var result = await _catalogService.GetCatalogItemByBrandAsync(testBrand, testPageSize, testPageIndex);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetCatalogItemByTypeAsync_Success()
    {
        // arrange
        var testPageIndex = 0;
        var testPageSize = 4;
        var testTotalCount = 12;
        var testType = "test";
        var pagingPaginatedItemsSuccess = new PaginatedItems<CatalogItem>()
        {
            Data = new List<CatalogItem>()
            {
                new CatalogItem()
                {
                    Id = 1,
                    Name = "Name",
                    Description = "Description",
                    Price = 1000,
                    AvailableStock = 100,
                    CatalogBrandId = 1,
                    CatalogTypeId = 1,
                    PictureFileName = "1.png"
                },
            },
            TotalCount = testTotalCount,
        };
        var catalogItemSuccess = new CatalogItem()
        {
            Id = 1,
            Name = "Name",
            Description = "Description",
            Price = 1000,
            AvailableStock = 100,
            CatalogBrandId = 1,
            CatalogTypeId = 1,
            PictureFileName = "1.png"
        };
        var catalogItemDtoSuccess = new CatalogItemDto()
        {
            Id = 1,
            Name = "Name",
            Description = "Description",
            Price = 1000,
            AvailableStock = 100,
            CatalogBrand = new CatalogBrandDto() { Id = 1, Brand = "testBrand" },
            CatalogType = new CatalogTypeDto() { Id = 1, Type = "testType" },
            PictureUrl = "www.alevelwebsite.com/assets/img/1"
        };
        _catalogItemRepository.Setup(s => s.GetByTypeAsync(
            It.Is<string>(i => i == testType),
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize))).ReturnsAsync(pagingPaginatedItemsSuccess);
        _mapper.Setup(s => s.Map<CatalogItemDto>(
            It.Is<CatalogItem>(i => i.Equals(catalogItemSuccess)))).Returns(catalogItemDtoSuccess);

        // act
        var result = await _catalogService.GetCatalogItemByTypeAsync(testType, testPageSize, testPageIndex);

        // assert
        result.Should().NotBeNull();
        result?.Data.Should().NotBeNull();
        result?.Count.Should().Be(testTotalCount);
        result?.PageIndex.Should().Be(testPageIndex);
        result?.PageSize.Should().Be(testPageSize);
    }

    [Fact]
    public async Task GetCatalogItemByTypeAsync_Failed()
    {
        // arrange
        var testPageIndex = 1000;
        var testPageSize = 10000;
        var testType = "test";

        _catalogItemRepository.Setup(s => s.GetByTypeAsync(
            It.Is<string>(i => i == testType),
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize))).Returns((Func<PaginatedItemsResponse<CatalogItemDto>>)null!);

        // act
        var result = await _catalogService.GetCatalogItemByTypeAsync(testType, testPageSize, testPageIndex);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetCatalogBrandsAsync_Success()
    {
        // arrange
        var testTotalCount = 12;

        var pagingPaginatedBrandsSuccess = new PaginatedItems<CatalogBrand>()
        {
            Data = new List<CatalogBrand>()
            {
                new CatalogBrand()
                {
                    Id = 1,
                    Brand = "test"
                },
            },
            TotalCount = testTotalCount,
        };

        var catalogBrandSuccess = new CatalogBrand()
        {
            Id = 1,
            Brand = "test"
        };

        var catalogBrandDtoSuccess = new CatalogBrandDto()
        {
            Id = 1,
            Brand = "test"
        };

        _catalogItemRepository.Setup(s => s.GetBrandsAsync())
            .ReturnsAsync(pagingPaginatedBrandsSuccess);
        _mapper.Setup(s => s.Map<CatalogBrandDto>(
            It.Is<CatalogBrand>(i => i.Equals(catalogBrandSuccess)))).Returns(catalogBrandDtoSuccess);

        // act
        var result = await _catalogService.GetCatalogBrandsAsync();

        // assert
        result.Should().NotBeNull();
        result?.Data.Should().NotBeNull();
        result?.Count.Should().Be(testTotalCount);
    }

    [Fact]
    public async Task GetCatalogBrandsAsync_Failed()
    {
        // arrange
        _catalogItemRepository.Setup(s => s.GetBrandsAsync())
            .Returns((Func<PaginatedItemsResponse<CatalogBrandDto>>)null!);

        // act
        var result = await _catalogService.GetCatalogBrandsAsync();

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetCatalogTypesAsync_Success()
    {
        // arrange
        var testTotalCount = 12;

        var pagingPaginatedTypesSuccess = new PaginatedItems<CatalogType>()
        {
            Data = new List<CatalogType>()
            {
                new CatalogType()
                {
                    Id = 1,
                    Type = "test"
                },
            },
            TotalCount = testTotalCount,
        };

        var catalogTypeSuccess = new CatalogType()
        {
            Id = 1,
            Type = "test"
        };

        var catalogTypeDtoSuccess = new CatalogTypeDto()
        {
            Id = 1,
            Type = "test"
        };

        _catalogItemRepository.Setup(s => s.GetTypesAsync()).ReturnsAsync(pagingPaginatedTypesSuccess);
        _mapper.Setup(s => s.Map<CatalogTypeDto>(
            It.Is<CatalogType>(i => i.Equals(catalogTypeSuccess)))).Returns(catalogTypeDtoSuccess);

        // act
        var result = await _catalogService.GetCatalogTypesAsync();

        // assert
        result.Should().NotBeNull();
        result?.Data.Should().NotBeNull();
        result?.Count.Should().Be(testTotalCount);
    }

    [Fact]
    public async Task GetCatalogTypesAsync_Failed()
    {
        // arrange
        _catalogItemRepository.Setup(s => s.GetTypesAsync())
            .Returns((Func<PaginatedItemsResponse<CatalogTypeDto>>)null!);

        // act
        var result = await _catalogService.GetCatalogTypesAsync();

        // assert
        result.Should().BeNull();
    }
}