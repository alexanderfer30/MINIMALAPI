using Microsoft.EntityFrameworkCore;
using CRM.API.Models.EN;

namespace CRM.API.Models.DAL
{
    public class CustomerDAL
    {
         readonly CRMContext _contex;

        public CustomerDAL(CRMContext contex)
        {
            _contex = contex;
        }

        public async Task<int> Create(Customer customer)
        {
            await _contex.AddAsync(customer);
            return await _contex.SaveChangesAsync();
        }

        public async Task<Customer> GetById(int id)
        {
            var customer = await _contex.Customers.FirstOrDefaultAsync(s => s.Id == id);

            return customer != null ? customer : new Customer();
        }

        public async Task<int> Update(Customer customer)
        {
            int result = 0;

            var customerUpdate = await GetById(customer.Id);

            if (customerUpdate.Id == default) return result;

            customerUpdate.Name = customer?.Name;
            customerUpdate.LastName = customer?.LastName;
            customerUpdate.Address = customer?.Address;

            result = await _contex.SaveChangesAsync();

            return result;
        }

        public async Task<int> Delete(int id)
        {
            int result = 0;

            var custumerDelete = await GetById(id);

            if (custumerDelete.Id == default) return result;

            _contex.Customers.Remove(custumerDelete);
            result = await _contex.SaveChangesAsync();

            return result;
        }

        private IQueryable<Customer> Query(Customer customer)
        {
            var query = _contex.Customers.AsQueryable();

            if (!string.IsNullOrWhiteSpace(customer.Name))
                query = query.Where(s => s.Name.Contains(customer.Name));

            if (!string.IsNullOrWhiteSpace(customer.LastName))
                query = query.Where(s => s.LastName.Contains(customer.LastName));

            return query;
        }

        public async Task<int> CountSearch(Customer customer)
        {
            return await Query(customer).CountAsync();
        }

        public async Task<List<Customer>> Search(Customer customer, int take = 10, int skip = 0)
        {
            take = take == 0 ? 10 : take;

            var query = Query(customer);
            query.OrderByDescending(s => s.Id).Skip(skip).Take(take);

            return await query.ToListAsync();
        }

    }
}
