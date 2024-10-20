using System.Threading.Tasks;
using EsamiOnline.Services;
using Grpc.Core;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace EsamiOnline.Tests.Services;

[TestSubject(typeof(GreeterService))]
public class GreeterServiceTest
{
    [Fact]
    public async Task SayHelloTest()
    {
        // Arrange
        var service = new GreeterService();
        var request = new HelloRequest { Name = "Alice" };
        var mockContext = new Mock<ServerCallContext>();

        // Act
        var reply = await service.SayHello(request, mockContext.Object);

        // Assert
        Assert.Equal("Hello Alice", reply.Message);
    }
}

