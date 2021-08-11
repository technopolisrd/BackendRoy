using Common.Core.Base;
using Backend.Entities.Tables;
using Backend.Repository.Sample.Contracts;
using Backend.Context.Data.API;

namespace Backend.Repository.Sample.Repository
{
    public class CustomerRepository : RepositoryBase<Customer, BackendContext>, iCustomerRepository
    {
        public CustomerRepository(BackendContext context) : base(context) { }
    }
}
