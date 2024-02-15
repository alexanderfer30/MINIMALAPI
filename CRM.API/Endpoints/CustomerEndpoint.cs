using CRM.API.Models.DAL;
using CRM.API.Models.EN;
using CRM.DTOs.CustomerDTOs;

namespace CRM.API.Endpoints
{
    public static class CustomerEndpoint
    {
        public static void AddCustomerEndpoints(this WebApplication app)
        {
            app.MapPost("/customer/search", async (SearchQueryCustomerDTO customerDTO, CustomerDAL customerDAL) =>
            {
                var customer = new Customer()
                {
                    Name = customerDTO.Name_Like != null ? customerDTO.Name_Like : string.Empty,
                    LastName = customerDTO.LastName_Like != null ? customerDTO.LastName_Like : string.Empty
                };

                var customers = new List<Customer>();
                int countRow = 0;

                if (customerDTO.SendRowCount == 2)
                {
                    customers = await customerDAL.Search(customer, skip: customerDTO.Skip, take: customerDTO.Take);

                    if (customers.Count > 0)
                        countRow = await customerDAL.CountSearch(customer);
                }
                else
                {
                    customers = await customerDAL.Search(customer, skip: customerDTO.Skip, take: customerDTO.Take);
                }

                var customerResult = new SearchResultCustomerDTO()
                {
                    Data = new List<SearchResultCustomerDTO.CustomerDTO>(),
                    CountRow = countRow,
                };

                customers.ForEach(customer =>
                {
                    customerResult.Data.Add(new SearchResultCustomerDTO.CustomerDTO()
                    {
                        Id = customer.Id,
                        Name = customer.Name,
                        LastName = customer.LastName,
                        Address = customer.Address,
                    });
                });

                return customerResult;
            });

            app.MapGet("/customer/{id:int}", async (int id, CustomerDAL customerDAL) =>
            {
                var customer = await customerDAL.GetById(id);

                var customerResult = new GetIdResultCustomerDTO()
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    LastName = customer.LastName,
                    Address = customer.Address
                };

                if (customerResult.Id > 0) return Results.Ok(customerResult);

                return Results.NotFound(customerResult);
            });

            app.MapPost("/customer", async (CreateCustomerDTO customerDTO, CustomerDAL customerDAL) =>
            {
                var customer = new Customer()
                {
                    Name = customerDTO.Name,
                    LastName = customerDTO.LastName,
                    Address = customerDTO.Address
                };

                int result = await customerDAL.Create(customer);

                if (result != 0) return Results.Ok(result);

                return Results.StatusCode(500);
            });

            app.MapPut("customer", async (EditCustomerDTO customerDTO, CustomerDAL customerDAL) =>
            {
                var customer = new Customer()
                {
                    Id = customerDTO.Id,
                    Name = customerDTO.Name,
                    LastName = customerDTO.LastName,
                    Address = customerDTO.Address
                };

                int result = await customerDAL.Update(customer);

                if (result != 0) return Results.Ok(result);

                return Results.StatusCode(500);
            });

            app.MapDelete("customer/{id:int}", async (int id, CustomerDAL customerDAL) =>
            {
                var result = await customerDAL.Delete(id);
                Console.WriteLine(result);

                if (result != 0) return Results.Ok(result);

                return Results.StatusCode(500);
            });
        }
    }
}
