using Microsoft.AspNetCore.Mvc;
using Moonglade.FriendLink;
using Moonglade.Web.Controllers;
using Moq;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Moonglade.Web.Models;

namespace Moonglade.Web.Tests.Controllers
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class FriendLinkControllerTests
    {
        private MockRepository _mockRepository;

        private Mock<IFriendLinkService> _mockFriendLinkService;

        private static readonly Guid Uid = Guid.Parse("76169567-6ff3-42c0-b163-a883ff2ac4fb");
        private FriendLinkEditModel _friendlinkEditViewModel = new()
        {
            Id = Uid,
            LinkUrl = "https://996.icu",
            Title = "996 ICU"
        };

        [SetUp]
        public void SetUp()
        {
            _mockRepository = new MockRepository(MockBehavior.Default);

            _mockFriendLinkService = _mockRepository.Create<IFriendLinkService>();
        }

        private FriendLinkController CreateFriendLinkController()
        {
            return new(_mockFriendLinkService.Object);
        }

        [Test]
        public async Task Create_InvalidModel()
        {
            var ctl = CreateFriendLinkController();
            ctl.ModelState.AddModelError("Title", "Title is required");

            var result = await ctl.Create(_friendlinkEditViewModel);
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
        }

        [Test]
        public async Task Create_OK()
        {
            var ctl = CreateFriendLinkController();

            var result = await ctl.Create(_friendlinkEditViewModel);

            Assert.IsInstanceOf<OkObjectResult>(result);
            _mockFriendLinkService.Verify(p => p.AddAsync(It.IsAny<string>(), It.IsAny<string>()));
        }
    }
}
