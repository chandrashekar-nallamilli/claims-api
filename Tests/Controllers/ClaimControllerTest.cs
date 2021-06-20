using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Claims_Api.Controllers;
using Claims_Api.Exceptions;
using Claims_Api.Models;
using Claims_Api.Services.Claim;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Tests.Helpers;

namespace Tests.Controllers
{
    public class ClaimControllerTest
    {
        private readonly Mock<IClaimService> _claimService = new();
        private readonly Mock<IServiceProvider> _serviceProvider = new();
        [Test]
        public async Task TestGetAllClaims()
        {
            var jsonRequest = ReadJson.Getfile(@"Payload/getClaim.json");

            //try parsing it
            var claim = jsonRequest.ToObject<Claim>();
            var claims = new List<Claim> {claim, claim};
            _claimService.Setup(_ => _.GetAllClaims(It.IsAny<CancellationToken>())).ReturnsAsync(claims);
            _serviceProvider
                .Setup(x => x.GetService(typeof(IClaimService)))
                .Returns(_claimService.Object);
            var controller = new ClaimController(_serviceProvider.Object);
            var result = await controller.GetAll();
            
            Assert.IsInstanceOf<IActionResult>(result);
            // Assert
            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);
            var claimsReturned = okObjectResult.Value as IEnumerable<Claim>;
            Assert.IsNotNull(claimsReturned);
        }
        [Test]
        public async Task TestGetAllClaims500()
        {
            _claimService.Setup(_ => _.GetAllClaims(It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
            _serviceProvider
                .Setup(x => x.GetService(typeof(IClaimService)))
                .Returns(_claimService.Object);
            var controller = new ClaimController(_serviceProvider.Object);
            var result = await controller.GetAll();
            
            Assert.IsInstanceOf<IActionResult>(result);
            Assert.IsInstanceOf<IActionResult>(result);
            var statusCodeResult = result as ObjectResult;
            Assert.That(statusCodeResult?.StatusCode == 500);
        }
        [Test]
        public async Task TestGetClaimById()
        {
            var jsonRequest = ReadJson.Getfile(@"Payload/getClaim.json");

            //try parsing it
            var claim = jsonRequest.ToObject<Claim>();
            _claimService.Setup(_ => _.GetClaimById(claim.Id,It.IsAny<CancellationToken>())).ReturnsAsync(claim);
            _serviceProvider
                .Setup(x => x.GetService(typeof(IClaimService)))
                .Returns(_claimService.Object);
            var controller = new ClaimController(_serviceProvider.Object);
            var result = await controller.Get(claim.Id);
            
            Assert.IsInstanceOf<IActionResult>(result);
            // Assert
            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);
            var returned = okObjectResult.Value as Claim;
            Assert.IsNotNull(returned);
        }
        
        [Test]
        public async Task TestGetClaimById404()
        {
            var jsonRequest = ReadJson.Getfile(@"Payload/getClaim.json");

            //try parsing it
            var claim = jsonRequest.ToObject<Claim>();
            _claimService.Setup(_ => _.GetClaimById(claim.Id,It.IsAny<CancellationToken>())).ThrowsAsync(new ClaimObjectNotFoundException(claim.Id));
            _serviceProvider
                .Setup(x => x.GetService(typeof(IClaimService)))
                .Returns(_claimService.Object);
            var controller = new ClaimController(_serviceProvider.Object);
            var result = await controller.Get(claim.Id);
            
            Assert.IsInstanceOf<IActionResult>(result);
            var statusCodeResult = result as ObjectResult;
            Assert.That(statusCodeResult?.StatusCode == 404);

        }
        
        [Test]
        public async Task TestGetClaimById500()
        {
            var jsonRequest = ReadJson.Getfile(@"Payload/getClaim.json");

            //try parsing it
            var claim = jsonRequest.ToObject<Claim>();
            _claimService.Setup(_ => _.GetClaimById(claim.Id,It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
            _serviceProvider
                .Setup(x => x.GetService(typeof(IClaimService)))
                .Returns(_claimService.Object);
            var controller = new ClaimController(_serviceProvider.Object);
            var result = await controller.Get(claim.Id);
            
            Assert.IsInstanceOf<IActionResult>(result);
            var statusCodeResult = result as ObjectResult;
            Assert.That(statusCodeResult?.StatusCode == 500);

        }
        
        [Test]
        public async Task TestSaveClaim()
        {
            var jsonRequest = ReadJson.Getfile(@"Payload/getClaim.json");

            //try parsing it
            var claim = jsonRequest.ToObject<Claim>();
            _claimService.Setup(_ => _.SaveClaim(claim,It.IsAny<CancellationToken>())).ReturnsAsync(claim);
            _serviceProvider
                .Setup(x => x.GetService(typeof(IClaimService)))
                .Returns(_claimService.Object);
            var controller = new ClaimController(_serviceProvider.Object);
            var result = await controller.SaveClaim(claim);
            
            Assert.IsInstanceOf<IActionResult>(result);
            var statusCodeResult = result as ObjectResult;
            Assert.That(statusCodeResult?.StatusCode == 200);

        }
        
        [Test]
        public async Task TestSaveClaimThrow400()
        {
            var jsonRequest = ReadJson.Getfile(@"Payload/getClaim.json");

            //try parsing it
            var claim = jsonRequest.ToObject<Claim>();
            _claimService.Setup(_ => _.SaveClaim(claim,It.IsAny<CancellationToken>())).Throws(new ArgumentException());
            _serviceProvider
                .Setup(x => x.GetService(typeof(IClaimService)))
                .Returns(_claimService.Object);
            var controller = new ClaimController(_serviceProvider.Object);
            var result = await controller.SaveClaim(claim);
            
            Assert.IsInstanceOf<IActionResult>(result);
            var statusCodeResult = result as ObjectResult;
            Assert.That(statusCodeResult?.StatusCode == 400);

        }
        
        [Test]
        public async Task TestSaveClaimThrow500()
        {
            var jsonRequest = ReadJson.Getfile(@"Payload/getClaim.json");

            //try parsing it
            var claim = jsonRequest.ToObject<Claim>();
            _claimService.Setup(_ => _.SaveClaim(claim,It.IsAny<CancellationToken>())).Throws(new Exception());
            _serviceProvider
                .Setup(x => x.GetService(typeof(IClaimService)))
                .Returns(_claimService.Object);
            var controller = new ClaimController(_serviceProvider.Object);
            var result = await controller.SaveClaim(claim);
            
            Assert.IsInstanceOf<IActionResult>(result);
            var statusCodeResult = result as ObjectResult;
            Assert.That(statusCodeResult?.StatusCode == 500);

        }
        
                [Test]
        public async Task TestUpdateClaim()
        {
            var jsonRequest = ReadJson.Getfile(@"Payload/getClaim.json");

            //try parsing it
            var claim = jsonRequest.ToObject<Claim>();
            _claimService.Setup(_ => _.UpdateClaim(claim.Id,claim,It.IsAny<CancellationToken>())).ReturnsAsync(claim);
            _serviceProvider
                .Setup(x => x.GetService(typeof(IClaimService)))
                .Returns(_claimService.Object);
            var controller = new ClaimController(_serviceProvider.Object);
            var result = await controller.UpdateClaim(claim.Id,claim);
            
            Assert.IsInstanceOf<IActionResult>(result);
            var statusCodeResult = result as ObjectResult;
            Assert.That(statusCodeResult?.StatusCode == 200);

        }
        
        [Test]
        public async Task TestUpdateClaimThrow400()
        {
            var jsonRequest = ReadJson.Getfile(@"Payload/getClaim.json");

            //try parsing it
            var claim = jsonRequest.ToObject<Claim>();
            _claimService.Setup(_ => _.UpdateClaim(claim.Id,claim,It.IsAny<CancellationToken>())).Throws(new ArgumentException());
            _serviceProvider
                .Setup(x => x.GetService(typeof(IClaimService)))
                .Returns(_claimService.Object);
            var controller = new ClaimController(_serviceProvider.Object);
            var result = await controller.UpdateClaim(claim.Id,claim);
            
            Assert.IsInstanceOf<IActionResult>(result);
            var statusCodeResult = result as ObjectResult;
            Assert.That(statusCodeResult?.StatusCode == 400);

        }
        
        [Test]
        public async Task TestUpdateClaimThrow500()
        {
            var jsonRequest = ReadJson.Getfile(@"Payload/getClaim.json");

            //try parsing it
            var claim = jsonRequest.ToObject<Claim>();
            _claimService.Setup(_ => _.UpdateClaim(claim.Id,claim,It.IsAny<CancellationToken>())).Throws(new Exception());
            _serviceProvider
                .Setup(x => x.GetService(typeof(IClaimService)))
                .Returns(_claimService.Object);
            var controller = new ClaimController(_serviceProvider.Object);
            var result = await controller.UpdateClaim(claim.Id,claim);
            
            Assert.IsInstanceOf<IActionResult>(result);
            var statusCodeResult = result as ObjectResult;
            Assert.That(statusCodeResult?.StatusCode == 500);
        }
        [Test]
        public async Task TestUpdateClaimThrow404()
        {
            var jsonRequest = ReadJson.Getfile(@"Payload/getClaim.json");

            //try parsing it
            var claim = jsonRequest.ToObject<Claim>();
            _claimService.Setup(_ => _.UpdateClaim(claim.Id,claim,It.IsAny<CancellationToken>())).Throws(new ClaimObjectNotFoundException(claim.Id));
            _serviceProvider
                .Setup(x => x.GetService(typeof(IClaimService)))
                .Returns(_claimService.Object);
            var controller = new ClaimController(_serviceProvider.Object);
            var result = await controller.UpdateClaim(claim.Id,claim);
            
            Assert.IsInstanceOf<IActionResult>(result);
            var statusCodeResult = result as ObjectResult;
            Assert.That(statusCodeResult?.StatusCode == 404);
        }
        
        [Test]
        public async Task TestDeleteClaim()
        {
            var jsonRequest = ReadJson.Getfile(@"Payload/getClaim.json");

            //try parsing it
            var claim = jsonRequest.ToObject<Claim>();
            _claimService.Setup(_ => _.DeleteClaim(claim.Id,It.IsAny<CancellationToken>())).ReturnsAsync(true);
            _serviceProvider
                .Setup(x => x.GetService(typeof(IClaimService)))
                .Returns(_claimService.Object);
            var controller = new ClaimController(_serviceProvider.Object);
            var result = await controller.DeleteClaim(claim.Id);
            
            Assert.IsInstanceOf<IActionResult>(result);
            var statusCodeResult = result as StatusCodeResult;
            Assert.That(statusCodeResult?.StatusCode == 204);

        }
        
        [Test]
        public async Task TestDeleteClaim404()
        {
            var jsonRequest = ReadJson.Getfile(@"Payload/getClaim.json");

            //try parsing it
            var claim = jsonRequest.ToObject<Claim>();
            _claimService.Setup(_ => _.DeleteClaim(claim.Id,It.IsAny<CancellationToken>())).ThrowsAsync(new ClaimObjectNotFoundException(claim.Id));
            _serviceProvider
                .Setup(x => x.GetService(typeof(IClaimService)))
                .Returns(_claimService.Object);
            var controller = new ClaimController(_serviceProvider.Object);
            var result = await controller.DeleteClaim(claim.Id);
            
            Assert.IsInstanceOf<IActionResult>(result);
            var statusCodeResult = result as ObjectResult;
            Assert.That(statusCodeResult?.StatusCode == 404);
        }
        
        [Test]
        public async Task TestDeleteClaim500()
        {
            var jsonRequest = ReadJson.Getfile(@"Payload/getClaim.json");

            //try parsing it
            var claim = jsonRequest.ToObject<Claim>();
            _claimService.Setup(_ => _.DeleteClaim(claim.Id,It.IsAny<CancellationToken>())).ThrowsAsync(new Exception());
            _serviceProvider
                .Setup(x => x.GetService(typeof(IClaimService)))
                .Returns(_claimService.Object);
            var controller = new ClaimController(_serviceProvider.Object);
            var result = await controller.DeleteClaim(claim.Id);
            
            Assert.IsInstanceOf<IActionResult>(result);
            var statusCodeResult = result as ObjectResult;
            Assert.That(statusCodeResult?.StatusCode == 500);
        }
    }
}