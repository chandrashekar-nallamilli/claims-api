using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Claims_Api.Services.Claim
{
    public interface IClaimService
    {
        Task<List<Models.Claim>> GetAllClaims(CancellationToken cancellationToken = default);
        Task<Models.Claim> GetClaimById(Guid claimId, CancellationToken cancellationToken = default);
        Task<Models.Claim> SaveClaim(Models.Claim claim, CancellationToken cancellationToken = default);
        Task<Models.Claim> UpdateClaim(Guid id, Models.Claim claim, CancellationToken cancellationToken = default);
        Task<bool> DeleteClaim(Guid  claimId, CancellationToken cancellationToken = default);
    }
}