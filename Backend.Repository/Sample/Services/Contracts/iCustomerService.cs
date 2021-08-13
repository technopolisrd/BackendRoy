using Backend.Entities.Tables.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Repository.Sample.Services.Contracts
{
    public interface iCustomerService
    {
        public bool ExistOrUsed(long? id);

        public Task<List<CustomerDTO>> GetAll();

        public Task<List<CustomerDTO>> GetAllBySearch(string searchString);

        public Task<CustomerDTO> GetDataById(long id);

        public Task<CustomerDTO> AddAsync(CustomerDTO entity, string user);

        public Task<CustomerDTO> UpdateAsync(CustomerDTO entity, string user);

        public Task<CustomerDTO> RemoveAsync(long id, string user);
    }
}
