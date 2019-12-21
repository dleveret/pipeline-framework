﻿using FluentAssertions;
using LightInject;
using LightInject.Interception;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using PipelineFramework.Abstractions;
using PipelineFramework.LightInject.Logging;
using PipelineFramework.Tests.SharedInfrastructure;
using Serilog;
using System;
using System.Threading;

namespace PipelineFramework.LightInject.Tests.Interception
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestClass]
    public class PipelineComponentInterceptorTests
    {
        private IServiceContainer _container;
        private ILogger _logger;

        [TestInitialize]
        public void Init()
        {
            _logger = Substitute.For<ILogger>();
            _logger.ForContext(Arg.Any<string>(), Arg.Any<string>())
                .Returns(_logger);

            _container = new ServiceContainer();
            _container.RegisterInstance(_logger);
        }

        [TestMethod]
        public void Interceptor_Test()
        {
            const string name = nameof(FooComponent);
            _container.AddPipelineComponentLogging();
            _container.RegisterPipelineComponent<FooComponent, TestPayload>();

            var component = _container.GetInstance<IPipelineComponent<TestPayload>>();
            component.Initialize(name, null);
            var result = component.Execute(new TestPayload(), CancellationToken.None);

            result.Should().NotBeNull();

            _logger.Received().ForContext(Arg.Is("ComponentName"), Arg.Is(name));
            _logger.Received().Information(Arg.Is(LogMessageTemplates.SuccessMessage), Arg.Any<long>());
        }

        [TestMethod]
        public void Interceptor_ComponentException_Test()
        {
            const string name = nameof(BarExceptionComponent);
            _container.AddPipelineComponentLogging();
            _container.RegisterPipelineComponent<BarExceptionComponent, TestPayload>();

            var component = _container.GetInstance<IPipelineComponent<TestPayload>>();
            component.Initialize(name, null);
            Action act = () => component.Execute(new TestPayload(), CancellationToken.None);

            act.Should().ThrowExactly<NotImplementedException>();
            
            _logger.Received().ForContext(Arg.Is("ComponentName"), Arg.Is(name));
            _logger.Received().Error(Arg.Any<Exception>(), Arg.Is(LogMessageTemplates.ExceptionMessage), Arg.Any<long>());
        }

        [TestMethod]
        public void Interceptor_CustomInterceptor_Test()
        {
            const string name = nameof(FooComponent);
            _container.AddPipelineComponentLogging<CustomInterceptor>();
            _container.RegisterPipelineComponent<FooComponent, TestPayload>();

            var component = _container.GetInstance<IPipelineComponent<TestPayload>>();
            component.Initialize(name, null);
            var result = component.Execute(new TestPayload(), CancellationToken.None);

            result.Should().NotBeNull();

            _logger.Received().ForContext(Arg.Is("ComponentName"), Arg.Is(name));
            _logger.Received(2).ForContext(Arg.Any<string>(), Arg.Any<string>());
            _logger.Received().Information(Arg.Is(LogMessageTemplates.SuccessMessage), Arg.Any<long>());
        }
    }

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class CustomInterceptor : PipelineComponentInterceptor
    {
        public CustomInterceptor(ILogger logger)
            : base(logger)
        {
        }

        protected override ILogger EnrichLogger(IInvocationInfo invocationInfo)
        {
            var logger = base.EnrichLogger(invocationInfo);

            return logger.ForContext("Test", "me");
        }
    }

}
