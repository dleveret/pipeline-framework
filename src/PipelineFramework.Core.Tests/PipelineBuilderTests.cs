using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PipelineFramework.Abstractions;
using PipelineFramework.Abstractions.Builder;
using PipelineFramework.Builder;
using PipelineFramework.Tests.SharedInfrastructure;
using System.Diagnostics.CodeAnalysis;

namespace PipelineFramework.Core.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class PipelineBuilderTests
    {
        [TestMethod]
        public void TestAsync()
        {
            // Act/Assert
            PipelineBuilder<TestPayload>.Async()
                .Should()
                .BeAssignableTo<IInitialPipelineComponentHolder<IAsyncPipeline<TestPayload>, IAsyncPipelineComponent<TestPayload>, TestPayload>>();
        }

        [TestMethod]
        public void TestNonAsync()
        {
            // Act/Assert
            PipelineBuilder<TestPayload>.NonAsync()
                .Should()
                .BeAssignableTo<IInitialPipelineComponentHolder<IPipeline<TestPayload>, IPipelineComponent<TestPayload>, TestPayload>>();
        }
    }
}