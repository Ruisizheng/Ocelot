using Xunit;
using Shouldly;
using TestStack.BDDfy;
using Ocelot.Controllers;
using System;
using Moq;
using Ocelot.Cache;
using System.Net.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Ocelot.UnitTests.Controllers
{
    public class OutputCacheControllerTests
    {
        private OutputCacheController _controller;
        private Mock<IOcelotCache<HttpResponseMessage>> _cache;
        private Mock<IRegionsGetter> _getter;
        private IActionResult _result;

        public OutputCacheControllerTests()
        {
            _cache = new Mock<IOcelotCache<HttpResponseMessage>>();
            _getter = new Mock<IRegionsGetter>();
            _controller = new OutputCacheController(_cache.Object, _getter.Object);
        }

        [Fact]
        public void should_get_all_keys_from_server()
        {
            this.Given(_ => GivenTheFollowingKeys(new List<string>{"b", "a"}))
                .When(_ => WhenIGetTheKeys())
                .Then(_ => ThenTheKeysAreReturned())
                .BDDfy();
        }

        [Fact]
        public void should_delete_key()
        {
             this.When(_ => WhenIDeleteTheKey("a"))
                .Then(_ => ThenTheKeyIsDeleted("a"))
                .BDDfy();
        }

        private void ThenTheKeyIsDeleted(string key)
        {
            _result.ShouldBeOfType<NoContentResult>();
            _cache
                .Verify(x => x.ClearRegion(key), Times.Once);
        }

        private void WhenIDeleteTheKey(string key)
        {
            _result = _controller.Delete(key);
        }

        private void GivenTheFollowingKeys(List<string> keys)
        {
            
        }

        private void WhenIGetTheKeys()
        {
            _result = _controller.Get();
        }

        private void ThenTheKeysAreReturned()
        {
            _result.ShouldBeOfType<OkObjectResult>();
        }
    }
}