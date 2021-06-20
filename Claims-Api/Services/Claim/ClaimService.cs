using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Claims_Api.Exceptions;
using Claims_Api.Models;
using Claims_Api.Repositories.UnitOfWork;
using Claims_Api.Services.Queues;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Serilog;

namespace Claims_Api.Services.Claim
{
    public class ClaimService : IClaimService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IQueueService _queueService;

        public ClaimService(IServiceProvider serviceProvider)
        {
            _unitOfWork = serviceProvider.GetService<IUnitOfWork>();
            _queueService = serviceProvider.GetService<IQueueService>();
        }
        public async Task<List<Models.Claim>> GetAllClaims(CancellationToken cancellationToken = default)
        {
            return await _unitOfWork.ClaimRepository.GetAllClaims(cancellationToken);
        }

        public async Task<Models.Claim> GetClaimById(Guid claimId, CancellationToken cancellationToken = default)
        {
            const string exceptionMessage = "Failed to get claim";
            try
            {
                var claim = await _unitOfWork.ClaimRepository.GetClaimById(claimId,cancellationToken);
                if (claim == null) throw new ClaimObjectNotFoundException(claimId);
                
                return claim;
            }
            catch (ClaimObjectNotFoundException ex)
            {
                Log.Error(ex, exceptionMessage);
                _unitOfWork.Dispose();
                throw ;
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex, exceptionMessage);
                _unitOfWork.Dispose();
                throw new ClaimObjectNotFoundException(claimId);
            }
            catch (Exception ex)
            {
                Log.Error(ex, exceptionMessage);
                throw;
            }

        }

        public async Task<Models.Claim> SaveClaim(Models.Claim claim, CancellationToken cancellationToken = default)
        {
            const string exceptionMessage = "Failed to update claim";

            try
            {
                ValidateClaim(claim);
                var dbClaim = new Models.Claim(Guid.NewGuid(), claim.Name, claim.Year, claim.Type, claim.DamageCost,
                    DateTime.UtcNow, DateTime.UtcNow);
                await _unitOfWork.ClaimRepository.SaveClaim(dbClaim,cancellationToken);
                _unitOfWork.Commit();
                var message = await Task.Run( () => JsonConvert.SerializeObject(dbClaim), cancellationToken);
                await _queueService.PublishMessage(message);
                return dbClaim;
            }
            catch (ArgumentException ex)
            {
                Log.Error(ex, exceptionMessage);
                throw;
            }
            catch (Exception ex)
            {
                Log.Error(ex, exceptionMessage);
                throw;
            }
        }

        private static void ValidateClaim(Models.Claim claim)
        {
            var currentYear = DateTime.Now.Year;
            if (claim.Year > currentYear) throw new ArgumentException("Claim cannot be in the future");
            if (claim.Year < currentYear - ClaimServiceConstant.MinYearDeviation) throw new ArgumentException($"Claim cannot be more than {ClaimServiceConstant.MinYearDeviation} years in the past");
            if (claim.DamageCost > ClaimServiceConstant.MaxDamageCost) throw new ArgumentException($"Claim Damage cost cant be more than {ClaimServiceConstant.MaxDamageCost} ");
        }

        public async Task<Models.Claim> UpdateClaim(Guid id, Models.Claim claim,
            CancellationToken cancellationToken = default)
        {
            ValidateClaim(claim);
            var dbClaim = new Models.Claim(id, claim.Name, claim.Year, claim.Type, claim.DamageCost,
                claim.Created, DateTime.UtcNow);
            await _unitOfWork.ClaimRepository.UpdateClaim(dbClaim,cancellationToken);
            _unitOfWork.Commit();
            var message = await Task.Run( () => JsonConvert.SerializeObject(dbClaim), cancellationToken);
            await _queueService.PublishMessage(message);
            return dbClaim;
        }

        public async Task<bool> DeleteClaim(Guid claimId, CancellationToken cancellationToken = default)
        {
            const string exceptionMessage = "Failed to delete claim";
            try
            {
                await _unitOfWork.ClaimRepository.DeleteClaim(claimId,cancellationToken);
                _unitOfWork.Commit();
            
                return true;
            }
            catch (InvalidOperationException ex)
            {
                Log.Error(ex, exceptionMessage);
                _unitOfWork.Dispose();
                throw new ClaimObjectNotFoundException(claimId);
            }
            catch (Exception ex)
            {
                Log.Error(ex, exceptionMessage);
                throw;
            }
        }
    }
}