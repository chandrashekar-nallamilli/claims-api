using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Claims_Api.Models;
using Dapper;
using Microsoft.Extensions.Options;

namespace Claims_Api.Repositories
{
    public class ClaimRepository : RepositoryBase, IClaimRepository
    {
        private readonly string _schema;

        public ClaimRepository(IOptions<AppSettings>  appSettings,IDbTransaction transaction): base(transaction)
        {
            _schema = appSettings.Value.schema_name;
        }

        public async Task<List<Claim>> GetAllClaims(CancellationToken cancellationToken = default)
        {
            var query = $@"SELECT * FROM {_schema}.Claim";
            var command = new CommandDefinition(query,null, Transaction, QueryMaxTimeOutInSeconds.Sixty, cancellationToken: cancellationToken);
            var result = await Connection.QueryAsync<Claim>(command);
            return result == null ? new List<Claim>() : result.ToList();
        }

        public async Task<Claim> GetClaimById(Guid claimId, CancellationToken cancellationToken = default)
        {
            var parameters = new {Id = claimId.ToString().ToUpper()};
            var query = $@"SELECT * FROM {_schema}.Claim c where  c.Id = @Id ";
            var command = new CommandDefinition(query, parameters, Transaction,QueryMaxTimeOutInSeconds.Sixty, cancellationToken: cancellationToken);
            var result = await Connection.QueryFirstAsync<Claim>(command);
            
            return result;
        }

        public async Task SaveClaim(Claim claim, CancellationToken cancellationToken = default)
        {
            var parameters = CreateClaimParameters(claim);
            var query = $@"
                INSERT INTO {_schema}.Claim (
                    Id,
                    Name,
                    Year,
                    Type,
                    DamageCost,
                    Created,
                    LastModified
                ) VALUES (
                    @Id,
                    @Name,
                    @Year,
                    @Type,
                    @DamageCost,
                    @Created,
                    @LastModified
                );";
            var command = new CommandDefinition(query, parameters, Transaction, QueryMaxTimeOutInSeconds.Sixty, cancellationToken: cancellationToken);
            await Connection.ExecuteAsync(command);
        }

        private static object CreateClaimParameters(Claim claim)
        {
            var parameters = new
            {
                Id = claim.Id,
                Name = claim.Name,
                Year = claim.Year,
                Type = claim.Type.ToString(),
                DamageCost = claim.DamageCost,
                Created = claim.Created,
                LastModified = claim.LastModified
            };

            return parameters;
        }

        public async Task UpdateClaim(Claim claim, CancellationToken cancellationToken = default)
        {
            var parameters = CreateClaimParameters(claim);
            var query = $@"
                UPDATE {_schema}.Claim
                SET Name = @Name,
                    Year = @Year,
                    Type = @Type,
                    DamageCost = @DamageCost,
                    Created = @Created,
                    LastModified = @LastModified
                WHERE Id = '{claim.Id.ToString().ToUpper()}';";
            var command = new CommandDefinition(query, parameters, Transaction, QueryMaxTimeOutInSeconds.Sixty, cancellationToken: cancellationToken);

            await Connection.ExecuteAsync(command);
        }

        public async Task DeleteClaim(Guid claimId, CancellationToken cancellationToken = default)
        {
            var parameters = new {Id = claimId.ToString().ToUpper()};
            var query = $@"DELETE FROM {_schema}.Claim c where  c.Id = @Id ";
            var command = new CommandDefinition(query, parameters, commandTimeout: QueryMaxTimeOutInSeconds.Sixty, cancellationToken: cancellationToken);
            await Connection.QueryFirstAsync<Claim>(command);
        }
    }
}