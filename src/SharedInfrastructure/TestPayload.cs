﻿using System.Diagnostics.CodeAnalysis;

namespace PipelineFramework.Tests.SharedInfrastructure
{
    [ExcludeFromCodeCoverage]
    public class TestPayload
    {
        public int Count { get; set; }
        public string FooStatus { get; set; }
        public bool FooWasCalled => !string.IsNullOrWhiteSpace(FooStatus);
        public string BarStatus { get; set; }
        public bool BarWasCalled => !string.IsNullOrWhiteSpace(BarStatus);
    }

}
