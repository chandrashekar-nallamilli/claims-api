using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Claims_Api.Models;

namespace Claims_Api.Repositories
{
    public interface IClaimRepository
    {
        Task<List<Claim>> GetAllClaims(CancellationToken cancellationToken = default);
        Task<Claim> GetClaimById(Guid claimId, CancellationToken cancellationToken = default);
        Task SaveClaim(Claim claim, CancellationToken cancellationToken = default);
        Task UpdateClaim(Claim claim, CancellationToken cancellationToken = default);
        Task DeleteClaim(Guid claimId, CancellationToken cancellationToken = default);
        
    }
}