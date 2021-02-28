using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Library.FunctionalTests.Infrastructure
{
    public abstract class TestBase : IClassFixture<TestApplicationFactory<TestStartup>>
    {
        protected WebApplicationFactory<TestStartup> Factory { get; }

        protected TestBase(TestApplicationFactory<TestStartup> factory)
        {
            Factory = factory;            
        }

    }
}