using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Claims_Api.Exceptions;
using Claims_Api.Models;
using Claims_Api.Services.Claim;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Claims_Api.Controllers
{
    [Produces("application/json")]
    [Route("claims")]
    [ApiController]
    public class ClaimController : ControllerBase
    {
        private readonly IClaimService _claimService;

        public ClaimController(IServiceProvider serviceProvider)
        {
            _claimService = serviceProvider.GetService<IClaimService>();
        }
        
        /// <summary>
        /// Get a Claim by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>If successful: Claim</returns>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(200, Type = typeof(Claim))]
        [ProducesResponseType(404, Type = typeof(ErrorResponse))]
        [ProducesResponseType(500, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> Get(Guid id)
        {
            const string errorMsg = "Failed to get Claim";

            try
            {
                var claim = await _claimService.GetClaimById(id);
                return Ok(claim);
            }
            catch (ClaimObjectNotFoundException ex)
            {
                Log.Information(ex, errorMsg);

                return StatusCode(StatusCodes.Status404NotFound, new ErrorResponse {Message = $"{errorMsg} : {ex.Message}" });
            }
            catch (Exception ex)
            {
                Log.Information(ex, errorMsg);

                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse {Message = $"{errorMsg} : {ex.Message}" });
            }
        }
        
        /// <summary>
        /// Get al Claims
        /// </summary>
        /// <returns>If successful list of  Claims</returns>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(200, Type = typeof(List<Claim>))]
        [ProducesResponseType(500, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> GetAll()
        {
            const string errorMsg = "Failed to get Claims";

            try
            {
                var claims = await _claimService.GetAllClaims();
                return Ok(claims);
            }
            catch (Exception ex)
            {
                Log.Information(ex, errorMsg);

                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse {Message = $"{errorMsg} : {ex.Message}" });
            }
        }
        
        /// <summary>
        /// Post a Claim
        /// </summary>
        /// <returns>If successful list of  Claims</returns>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(200, Type = typeof(Claim))]
        [ProducesResponseType(412, Type = typeof(ErrorResponse))]
        [ProducesResponseType(500, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> SaveClaim(Claim claim)
        {
            const string errorMsg = "Failed to save Claim";

            try
            {
                var claims = await _claimService.SaveClaim(claim);
                return Ok(claims);
            }
            catch (ArgumentException ex)
            {
                Log.Information(ex, errorMsg);

                return StatusCode(StatusCodes.Status400BadRequest,
                    new ErrorResponse {Message = ex.Message});
            }
            catch (Exception ex)
            {
                Log.Information(ex, errorMsg);

                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse {Message = $"{errorMsg} : {ex.Message}" });
            }
        }
        
        
        /// <summary>
        /// Put a Claim
        /// </summary>
        /// <param name="id">Claim Id</param>
        /// <param name="claim">Payload data</param>
        /// <returns>If successful list of  Claims</returns>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(200, Type = typeof(Claim))]
        [ProducesResponseType(404, Type = typeof(ErrorResponse))]
        [ProducesResponseType(412, Type = typeof(ErrorResponse))]
        [ProducesResponseType(500, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> UpdateClaim(Guid id , Claim claim)
        {
            const string errorMsg = "Failed to update Claim";

            try
            {
                claim = await _claimService.UpdateClaim(id, claim);
                return Ok(claim);
            }
            catch (ClaimObjectNotFoundException ex)
            {
                Log.Information(ex, errorMsg);

                return StatusCode(StatusCodes.Status404NotFound,
                    new ErrorResponse {Message = ex.Message});
            }
            catch (ArgumentException ex)
            {
                Log.Information(ex, errorMsg);

                return StatusCode(StatusCodes.Status400BadRequest,
                    new ErrorResponse {Message = ex.Message});
            }
            catch (Exception ex)
            {
                Log.Information(ex, errorMsg);

                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse {Message = $"{errorMsg} : {ex.Message}" });
            }
        }
        
        /// <summary>
        /// Delete a Claim
        /// </summary>
        /// <param name="id">claim Id</param>
        /// <returns>If successful: return OK</returns>
        [HttpDelete]
        [Route("")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404, Type = typeof(ErrorResponse))]
        [ProducesResponseType(500, Type = typeof(ErrorResponse))]
        public async Task<IActionResult> DeleteClaim(Guid id)
        {
            const string errorMsg = "Failed to delete Claim";

            try
            { 
                await _claimService.DeleteClaim(id);
                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (ClaimObjectNotFoundException ex)
            {
                Log.Information(ex, errorMsg);

                return StatusCode(StatusCodes.Status404NotFound, new ErrorResponse {Message = $"{errorMsg} : {ex.Message}" });
            }
            catch (Exception ex)
            {
                Log.Information(ex, errorMsg);

                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse {Message = $"{errorMsg} : {ex.Message}" });
            }
        }
    }
}