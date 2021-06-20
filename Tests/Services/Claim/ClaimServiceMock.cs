using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Claims_Api.Models;
using Claims_Api.Services.Claim;
using Microsoft.Extensions.Options;
using Moq;
using Tests.Helpers;

namespace Tests.Services.Claim
{
    public class ClaimServiceMock : IClaimService
    {
        private readonly OptionsWrapper<AppSettings> _appSettings;

        public ClaimServiceMock()
        {
            _appSettings = new OptionsWrapper<AppSettings>(new AppSettings()
            {
                db_connectionstring = "test",
            });
            
            MockRepositories.Setup();
        }
        public async Task<List<Claims_Api.Models.Claim>> GetAllClaims(CancellationToken cancellationToken = default)
        {
            var jsonRequest = ReadJson.Getfile(@"Payload/getClaim.json");

            //try parsing it
            var claim = jsonRequest.ToObject<Claims_Api.Models.Claim>();
            var claims = new List<Claims_Api.Models.Claim> {claim, claim};
            ServiceProviderHelper.ClaimRepository.Setup(x => x.GetAllClaims(cancellationToken))
                .ReturnsAsync(claims);
            var serviceProvider = ServiceProviderHelper.MockServiceProvider();
            var service = new ClaimService(serviceProvider);
            return  await service.GetAllClaims(cancellationToken);
        }

        public async Task<Claims_Api.Models.Claim> GetClaimById(Guid claimId, CancellationToken cancellationToken = default)
        {
            var jsonRequest = ReadJson.Getfile(@"Payload/getClaim.json");

            //try parsing it
            var claim = jsonRequest.ToObject<Claims_Api.Models.Claim>();
            ServiceProviderHelper.ClaimRepository.Setup(x => x.GetClaimById(claim.Id, cancellationToken))
                .ReturnsAsync(claim);

            var serviceProvider = ServiceProviderHelper.MockServiceProvider();
            var service = new ClaimService(serviceProvider);
            return  await service.GetClaimById(claimId, cancellationToken);
        }

        public async Task<Claims_Api.Models.Claim> SaveClaim(Claims_Api.Models.Claim claim, CancellationToken cancellationToken = default)
        {
            ServiceProviderHelper.ClaimRepository.Setup(x => x.SaveClaim(claim, cancellationToken))
                .Returns(Task.CompletedTask);

            var serviceProvider = ServiceProviderHelper.MockServiceProvider();
            var service = new ClaimService(serviceProvider);
            return  await service.SaveClaim(claim, cancellationToken);
        }

        public async Task<Claims_Api.Models.Claim> UpdateClaim(Guid id, Claims_Api.Models.Claim claim, CancellationToken cancellationToken = default)
        {
            ServiceProviderHelper.ClaimRepository.Setup(x => x.UpdateClaim(claim, cancellationToken))
                .Returns(Task.CompletedTask);

            var serviceProvider = ServiceProviderHelper.MockServiceProvider();
            var service = new ClaimService(serviceProvider);
            return  await service.UpdateClaim(id, claim, cancellationToken);
        }

        public async Task<bool> DeleteClaim(Guid claimId, CancellationToken cancellationToken = default)
        {
            ServiceProviderHelper.ClaimRepository.Setup(x => x.DeleteClaim(claimId, cancellationToken))
                .Returns(Task.CompletedTask);

            var serviceProvider = ServiceProviderHelper.MockServiceProvider();
            var service = new ClaimService(serviceProvider);
            return  await service.DeleteClaim(claimId, cancellationToken);
        }
    }
}