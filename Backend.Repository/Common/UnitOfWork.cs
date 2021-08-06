using System;
using Common.Core.Contracts.Common;
using Backend.Context.Data.API;
using Microsoft.EntityFrameworkCore.Storage;


namespace Backend.Repository.Common
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly BackendContext _DataContext;

        public UnitOfWork(BackendContext dataContext)
        {
            _DataContext = dataContext;
        }

        public IDbContextTransaction CreateTransaction()
        {
            return this._DataContext.Database.BeginTransaction();
        }

        public void Dispose()
        {
            if (_DataContext != null)
            {
                _DataContext.Dispose();
            }
        }

        public int SaveChanges()
        {
            return _DataContext.SaveChanges();
        }
    }
}
