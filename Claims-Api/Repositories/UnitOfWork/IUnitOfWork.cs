using System;

namespace Claims_Api.Repositories.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IClaimRepository ClaimRepository { get; }
        void Commit();
    }


}
