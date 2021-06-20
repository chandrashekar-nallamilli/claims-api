using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Claims_Api.Exceptions;
using NUnit.Framework;
using Tests.Helpers;

namespace Tests.Services.Claim
{
    public class ClaimServiceTester
    {
        [Test]
        public async Task TestGetClaim()
        {
            var service = new ClaimServiceMock();
            var claim = await service.GetClaimById(Guid.Parse("f2a6261a-a1bb-424a-ba85-d136fc0e8d5b"));
            
            Assert.That(claim.Id == Guid.Parse("f2a6261a-a1bb-424a-ba85-d136fc0e8d5b"));
        }
        
        [Test]
        public async Task TestGetAllClaim()
        {
            var service = new ClaimServiceMock();
            var claims = await service.GetAllClaims();
            
            Assert.That(claims.Count == 2);
        }
        
        [Test]
        public async Task GetClaimNotFoundTest()
        {
            try
            {
                var service = new ClaimServiceMock();
                await service.GetClaimById(Guid.Parse("f2a6261a-a1bb-424a-ba85-d136fc0e8d5c"));
            }
            catch (ClaimObjectNotFoundException)
            {
                Assert.Pass();
            }
        }
        
        [Test]
        public async Task TestSaveClaim()
        {
            var jsonRequest = ReadJson.Getfile(@"Payload/getClaim.json");

            //try parsing it
            var claim = jsonRequest.ToObject<Claims_Api.Models.Claim>();
            var service = new ClaimServiceMock();
            claim = await service.SaveClaim(claim);
            Assert.That(claim.Id != null);
        }
        
        [Test]
        public async Task TestSaveClaimDamageCostValidationTrue()
        {
            var jsonRequest = ReadJson.Getfile(@"Payload/getClaim.json");

            //try parsing it
            var claim = jsonRequest.ToObject<Claims_Api.Models.Claim>();
            var service = new ClaimServiceMock();
            claim = await service.SaveClaim(claim);
            Assert.That(claim.DamageCost <= 100000);
        }
        [Test]
        public async Task TestSaveClaimDamageCostValidationFalse()
        {
            try
            {
                var jsonRequest = ReadJson.Getfile(@"Payload/getClaim.json");

                //try parsing it
                var request = jsonRequest.ToObject<Claims_Api.Models.Claim>();
                var claim = new Claims_Api.Models.Claim(request.Id, request.Name, request.Year, request.Type,
                    100000000, request.Created, request.LastModified);
                var service = new ClaimServiceMock();
                await service.SaveClaim(claim);
            }
            catch (ArgumentException e)
            { 
                Assert.Pass();
            }
        }
        [Test]
        public async Task TestSaveClaimYearValidationTrue()
        {
            var jsonRequest = ReadJson.Getfile(@"Payload/getClaim.json");
            var currentYear = DateTime.Now.Year;
            //try parsing it
            var claim = jsonRequest.ToObject<Claims_Api.Models.Claim>();
            var service = new ClaimServiceMock();
            claim = await service.SaveClaim(claim);
            Assert.That(claim.Year >= currentYear - 10 && claim.Year <= currentYear);
        }
        [Test]
        public async Task TestSaveClaimYearValidationFalse()
        {
            try
            {
                var jsonRequest = ReadJson.Getfile(@"Payload/getClaim.json");

                //try parsing it
                var request = jsonRequest.ToObject<Claims_Api.Models.Claim>();
                var claim = new Claims_Api.Models.Claim(request.Id, request.Name, 1996, request.Type,
                    request.DamageCost, request.Created, request.LastModified);
                var service = new ClaimServiceMock();
                await service.SaveClaim(claim);
            }
            catch (ArgumentException e)
            { 
                Assert.Pass();
            }
        }
        
                [Test]
        public async Task TestUpdateClaim()
        {
            var jsonRequest = ReadJson.Getfile(@"Payload/getClaim.json");

            //try parsing it
            var claim = jsonRequest.ToObject<Claims_Api.Models.Claim>();
            var service = new ClaimServiceMock();
            claim = await service.UpdateClaim(claim.Id,claim);
            Assert.That(claim.Id != null);
        }
        
        [Test]
        public async Task TestUpdateClaimDamageCostValidationTrue()
        {
            var jsonRequest = ReadJson.Getfile(@"Payload/getClaim.json");

            //try parsing it
            var claim = jsonRequest.ToObject<Claims_Api.Models.Claim>();
            var service = new ClaimServiceMock();
            claim = await service.UpdateClaim(claim.Id,claim);
            Assert.That(claim.DamageCost <= 100000);
        }
        [Test]
        public async Task TestUpdateClaimDamageCostValidationFalse()
        {
            try
            {
                var jsonRequest = ReadJson.Getfile(@"Payload/getClaim.json");

                //try parsing it
                var request = jsonRequest.ToObject<Claims_Api.Models.Claim>();
                var claim = new Claims_Api.Models.Claim(request.Id, request.Name, request.Year, request.Type,
                    100000000, request.Created, request.LastModified);
                var service = new ClaimServiceMock();
                await service.UpdateClaim(claim.Id,claim);
            }
            catch (ArgumentException e)
            { 
                Assert.Pass();
            }
        }
        [Test]
        public async Task TestUpdateClaimYearValidationTrue()
        {
            var jsonRequest = ReadJson.Getfile(@"Payload/getClaim.json");
            var currentYear = DateTime.Now.Year;
            //try parsing it
            var claim = jsonRequest.ToObject<Claims_Api.Models.Claim>();
            var service = new ClaimServiceMock();
            claim = await service.UpdateClaim(claim.Id,claim);
            Assert.That(claim.Year >= currentYear - 10 && claim.Year <= currentYear);
        }
        [Test]
        public async Task TestUpdateClaimYearValidationFalse()
        {
            try
            {
                var jsonRequest = ReadJson.Getfile(@"Payload/getClaim.json");

                //try parsing it
                var request = jsonRequest.ToObject<Claims_Api.Models.Claim>();
                var claim = new Claims_Api.Models.Claim(request.Id, request.Name, 1996, request.Type,
                    request.DamageCost, request.Created, request.LastModified);
                var service = new ClaimServiceMock();
                await service.UpdateClaim(claim.Id,claim);
            }
            catch (ArgumentException e)
            { 
                Assert.Pass();
            }
        }
        
        [Test]
        public async Task TestDeleteClaim()
        {
            var jsonRequest = ReadJson.Getfile(@"Payload/getClaim.json");

            //try parsing it
            var request = jsonRequest.ToObject<Claims_Api.Models.Claim>();
            var claim = new Claims_Api.Models.Claim(request.Id, request.Name, 1996, request.Type,
                request.DamageCost, request.Created, request.LastModified);
            var service = new ClaimServiceMock();
            Assert.That(await service.DeleteClaim(claim.Id));
        }
    }
}