﻿using System.Diagnostics.CodeAnalysis;

namespace PipelineFramework.Core.Tests.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public class TestPayload
    {
        public int Count { get; set; }
        public string FooStatus { get; set; }
        public bool FooWasCalled { get; set; }
        public string BarStatus { get; set; }
        public bool BarWasCalled { get; set; }
    }
}
