using CRM.DTOs.CustomerDTOs;

namespace CRM.AppWebBlazor.Data
{
    public class CustomerService
    {
        private readonly HttpClient _httpClientCRMAPI;

        public CustomerService(IHttpClientFactory httpClientFactory)
        {
            _httpClientCRMAPI = httpClientFactory.CreateClient("CRMAPI");
        }

        public async Task<SearchResultCustomerDTO> Search(SearchQueryCustomerDTO searchQueryCustomerDTO)
        {
            var result = new SearchResultCustomerDTO();
            try
            {
                var response = await _httpClientCRMAPI.PostAsJsonAsync("/customer/search", searchQueryCustomerDTO);
                response.EnsureSuccessStatusCode();

                result = await response.Content.ReadFromJsonAsync<SearchResultCustomerDTO>();

                return result ?? new SearchResultCustomerDTO();
            }
            catch (Exception)
            {
                return new SearchResultCustomerDTO(); ;
            }

        }

        public async Task<GetIdResultCustomerDTO> GetById(int id)
        {
            var response = await _httpClientCRMAPI.GetAsync($"/customer/{id}");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<GetIdResultCustomerDTO>();

                return result ?? new GetIdResultCustomerDTO();
            }

            return new GetIdResultCustomerDTO();
        }

        public async Task<int> Create(CreateCustomerDTO createCustomerDTO)
        {
            var result = 0;

            try
            {
                var response = await _httpClientCRMAPI.PostAsJsonAsync("/customer", createCustomerDTO);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();

                int.TryParse(responseBody, out result);

                return result;
            }
            catch (Exception)
            {
                return result;
            }
        }

        public async Task<int> Edit(EditCustomerDTO editCustomerDTO)
        {
            var result = 0;

            try
            {
                var response = await _httpClientCRMAPI.PutAsJsonAsync("/customer", editCustomerDTO);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();

                int.TryParse(responseBody, out result);

                return result;
            }
            catch (Exception)
            {
                return result;
            }
        }

        public async Task<int> Delete(int id)
        {
            var result = 0;

            try
            {
                var response = await _httpClientCRMAPI.DeleteAsync($"/customer/{id}");
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();

                int.TryParse(responseBody, out result);

                return result;
            }
            catch (Exception)
            {
                return result;
            }
        }

    }
}
