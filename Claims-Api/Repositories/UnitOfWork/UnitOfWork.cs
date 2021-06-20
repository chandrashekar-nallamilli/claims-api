using System;
using System.Data;
using System.Data.SqlClient;
using Claims_Api.Models;
using Microsoft.Extensions.Options;

namespace Claims_Api.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private bool _disposed;
        private IClaimRepository _claimRepository;
        private readonly IOptions<AppSettings> _appSettings;

        public UnitOfWork(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings;
            _connection = new SqlConnection(appSettings.Value.db_connectionstring);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }
        
        public IClaimRepository ClaimRepository
        {
            get { return _claimRepository ??= new ClaimRepository(_appSettings, _transaction); }
        }
        

        public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            catch
            {
                _transaction.Rollback();
                throw;
            }
            finally
            {
                _transaction.Dispose();
                _transaction = _connection.BeginTransaction();
                ResetRepositories();
            }
        }

        private void ResetRepositories()
        {
            _claimRepository = null;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if(disposing)
                {
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }
                    if(_connection != null)
                    {
                        _connection.Dispose();
                        _connection = null;
                    }
                }
                _disposed = true;
            }
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }
    }
}