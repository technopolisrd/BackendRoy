using Backend.Entities.Tables;
using Backend.Entities.Tables.DTO;
using Backend.Repository.Sample.Contracts;
using Backend.Repository.Sample.Services.Contracts;
using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Repository.Sample.Services
{
    public class CustomerService : iCustomerService
    {
        private readonly iCustomerRepository _repo;

        public CustomerService(iCustomerRepository repo)
        {
            _repo = repo;
        }

        public async Task<long> AddAsync(CustomerDTO entity, string user)
        {
            Customer table = new Customer();

            table.FirstName = entity.FirstName;
            table.LastName = entity.LastName;
            table.DocumentId = entity.DocumentId;

            FillAudit(ref table, 0, user);

            Customer result;

            try
            {
                result = await _repo.Add(table);
            }
            catch (Exception)
            {
                return 0;
            }

            if (result == null)
            {
                return 0;
            }

            return result.Id;
        }

        public bool ExistOrUsed(long? id)
        {
            if (id != null)
            {
                return _repo.Exists((long)id);
            }

            return false;
        }

        public async Task<List<CustomerDTO>> GetAll()
        {
            var entity = await _repo.GetAll(cus => cus.Select(cust => new CustomerDTO
            {
                Id = cust.Id,
                DocumentId = cust.DocumentId,
                FirstName = cust.FirstName,
                LastName = cust.LastName
            }));

            if (entity.Count() == 0)
            {
                return null;
            }

            return entity.ToList();
        }

        public async Task<List<CustomerDTO>> GetAllBySearch(string searchString)
        {
            var predicate = PredicateBuilder.New<Customer>();

            string[] searchStr = searchString.Split("|");

            foreach (var search in searchStr)
            {
                if (!String.IsNullOrEmpty(search))
                {
                    predicate = predicate.Or(x => x.FirstName.Contains(search));
                    predicate = predicate.Or(x => x.LastName.Contains(search));
                    predicate = predicate.Or(x => x.DocumentId.Equals(search));
                }
            }

            var entity = await _repo.GetAll(cus => cus.Select(cust => new CustomerDTO
            {
                Id = cust.Id,
                DocumentId = cust.DocumentId,
                FirstName = cust.FirstName,
                LastName = cust.LastName
            }), predicate);

            if (entity.Count() == 0)
            {
                return null;
            }

            return entity.ToList();
        }

        public async Task<CustomerDTO> GetDataById(long id)
        {
            var table = await _repo.Get(id);

            if (table == null)
            {
                return null;
            }

            CustomerDTO entity = new CustomerDTO();

            entity.Id = table.Id;
            entity.FirstName = table.FirstName;
            entity.LastName = table.LastName;
            entity.DocumentId = table.DocumentId;

            return entity;
        }

        public async Task<CustomerDTO> RemoveAsync(long id, string user)
        {
            Customer table;
            CustomerDTO cus;

            try
            {
                table = await _repo.Get(id);
            }
            catch (Exception)
            {
                cus = new CustomerDTO();
                cus.Id = 0;
                return cus;
            }

            if (table == null)
            {
                cus = new CustomerDTO();
                cus.Id = 0;
                return cus;
            }

            table.Deferred = true;

            FillAudit(ref table, 2, user);

            Customer result;

            try
            {
                result = await _repo.Update(table);
            }
            catch (Exception)
            {
                return null;
            }

            cus = new CustomerDTO();

            cus.Id = result.Id;
            cus.FirstName = result.FirstName;
            cus.LastName = result.LastName;
            cus.DocumentId = result.DocumentId;

            return cus;
        }

        public async Task<CustomerDTO> UpdateAsync(CustomerDTO entity, string user)
        {
            Customer table;

            try
            {
                table = await _repo.Get(entity.Id);
            }
            catch (Exception)
            {
                CustomerDTO cus = new CustomerDTO();
                cus.Id = 0;
                return cus;
            }

            if (table == null)
            {
                CustomerDTO cus = new CustomerDTO();
                cus.Id = 0;
                return cus;
            }

            table.FirstName = entity.FirstName;
            table.LastName = entity.LastName;
            table.DocumentId = entity.DocumentId;

            FillAudit(ref table, 1, user);

            Customer result;

            try
            {
                result = await _repo.Update(table);
            }
            catch (Exception)
            {
                return null;
            }

            return entity;
        }

        private void FillAudit(ref Customer entity, int state, string user)
        {
            if (state == 0) // si esta insertando
            {
                entity.Deferred = false;
                entity.CreatedDate = DateTime.Now;
                entity.UpdatedDate = DateTime.Now;
                entity.CreatedById = user;
                entity.UpdatedById = user;
            }
            else if (state == 1) // si esta editando
            {
                entity.Deferred = false;
                entity.UpdatedDate = DateTime.Now;
                entity.UpdatedById = user;
            }
            else // si esta borrando
            {
                entity.Deferred = true;
                entity.UpdatedDate = DateTime.Now;
                entity.UpdatedById = user;
            }
        }
    }
}
