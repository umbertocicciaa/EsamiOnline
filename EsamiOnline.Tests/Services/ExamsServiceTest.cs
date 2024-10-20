using System.Threading.Tasks;
using EsamiOnline.Services;
using Grpc.Core;
using JetBrains.Annotations;
using Moq;
using Xunit;

namespace EsamiOnline.Tests.Services;

[TestSubject(typeof(ExamsService))]
public class ExamsServiceTest
{
    [Fact]
    public async Task SaveExamTest()
    {
    }
}