using FluentAssertions;
using Linksy.Domain.DomainServices;
using Linksy.Domain.Entities.Url;
using Linksy.Domain.Exceptions;
using Linksy.Domain.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linksy.Tests.Unit.DomainServices
{
    public class GenerateShortenedUrlServiceTests
    {
        private readonly Mock<IUrlRepository> _urlRepositoryMock;
        private readonly Mock<TimeProvider> _timeProviderMock;
        private readonly GenerateShotenedUrlService _sut;

        public GenerateShortenedUrlServiceTests()
        {
            _urlRepositoryMock = new Mock<IUrlRepository>();
            _timeProviderMock = new Mock<TimeProvider>();

            _timeProviderMock.Setup(x => x.GetUtcNow())
                .Returns(new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero));

            _sut = new GenerateShotenedUrlService(_timeProviderMock.Object, _urlRepositoryMock.Object);
        }

        [Fact]
        public async Task GenerateShortenedUrlAsync_WithoutCustomCode_ReturnsUrlWithGeneratedCode()
        {
            // Arrange
            var originalUrl = "https://example.com";
            var userId = 1;

            // Act
            var result = await _sut.GenerateShortenedUrlAsync(
                originalUrl,
                customCode: null,
                tags: null,
                umtParameters: null,
                userId);

            // Assert
            result.Should().NotBeNull();
            result.OriginalUrl.Should().Be(originalUrl);
            result.Code.Should().NotBeNullOrEmpty()
                .And.HaveLength(6);
        }

        [Fact]
        public async Task GenerateShortenedUrlAsync_WithCustomCode_WhenCodeNotTaken_ReturnsUrlWithCustomCode()
        {
            // Arrange
            var originalUrl = "https://example.com";
            var customCode = "mycode";
            var userId = 1;

            _urlRepositoryMock.Setup(x => x.IsUrlCodeInUseAsync(customCode, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await _sut.GenerateShortenedUrlAsync(
                originalUrl,
                customCode,
                tags: null,
                umtParameters: null,
                userId);

            // Assert
            result.Should().NotBeNull();
            result.OriginalUrl.Should().Be(originalUrl);
            result.Code.Should().Be(customCode);

            _urlRepositoryMock.Verify(x => x.IsUrlCodeInUseAsync(customCode, It.IsAny<CancellationToken>()), 
                Times.Once);
        }

        [Fact]
        public async Task GenerateShortenedUrlAsync_WithCustomCode_WhenCodeTaken_ThrowsCustomCodeInUseException()
        {
            // Arrange
            var originalUrl = "https://example.com";
            var customCode = "taken";
            var userId = 1;

            _urlRepositoryMock.Setup(x => x.IsUrlCodeInUseAsync(customCode, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var act = async () => await _sut.GenerateShortenedUrlAsync(
                originalUrl,
                customCode,
                tags: null,
                umtParameters: null,
                userId);

            // Assert
            await act.Should().ThrowAsync<CustomCodeInUseException>();

            _urlRepositoryMock.Verify(x => x.IsUrlCodeInUseAsync(customCode, It.IsAny<CancellationToken>()), 
                Times.Once);
        }

        [Fact]
        public async Task GenerateShortenedUrlAsync_WithCancellationToken_PassesTokenToRepository()
        {
            // Arrange
            var originalUrl = "https://example.com";
            var customCode = "code";
            var userId = 1;
            var cancellationToken = new CancellationToken();

            _urlRepositoryMock.Setup(x => x.IsUrlCodeInUseAsync(customCode, cancellationToken))
                .ReturnsAsync(false);

            // Act
            await _sut.GenerateShortenedUrlAsync(
                originalUrl,
                customCode,
                tags: null,
                umtParameters: null,
                userId,
                cancellationToken);

            // Assert
            _urlRepositoryMock.Verify(x => x.IsUrlCodeInUseAsync(customCode, cancellationToken), Times.Once);
        }

        [Fact]
        public async Task GenerateShortenedUrlAsync_WithoutCustomCode_DoesNotCheckRepository()
        {
            // Arrange
            var originalUrl = "https://example.com";
            var userId = 1;

            // Act
            await _sut.GenerateShortenedUrlAsync(
                originalUrl,
                customCode: null,
                tags: null,
                umtParameters: null,
                userId);

            // Assert
            _urlRepositoryMock.Verify(
                x => x.IsUrlCodeInUseAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}
