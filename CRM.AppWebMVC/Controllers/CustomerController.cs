using CRM.DTOs.CustomerDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CRM.AppWebMVC.Controllers
{
    public class CustomerController : Controller
    {

        private readonly HttpClient _httpClientCRMAPI;

        public CustomerController(IHttpClientFactory httpClientFactory)
        {
            httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _httpClientCRMAPI = httpClientFactory.CreateClient("CRMAPI");

        }

        // GET: CustomerController
        public async Task<IActionResult> Index(SearchQueryCustomerDTO searchQueryCustomerDTO, int CountRow = 0)
        {
            // Varlores por defecto para la busqueda
            if (searchQueryCustomerDTO.SendRowCount == 0)
                searchQueryCustomerDTO.SendRowCount = 2;

            if (searchQueryCustomerDTO.Take == 0)
                searchQueryCustomerDTO.Take = 10;

            var result = new SearchResultCustomerDTO();

            try
            {
                var response = await _httpClientCRMAPI.PostAsJsonAsync("/customer/search", searchQueryCustomerDTO);

                if (response.IsSuccessStatusCode)
                    result = await response.Content.ReadFromJsonAsync<SearchResultCustomerDTO>();

                result = result != null ? result : new SearchResultCustomerDTO();

                // Configuracion de valores para la lista
                if (result.CountRow == 0 && searchQueryCustomerDTO.SendRowCount == 1)
                    result.CountRow = CountRow;

                ViewBag.CountRow = result.CountRow;
                searchQueryCustomerDTO.SendRowCount = 0;
                ViewBag.SearchQuery = searchQueryCustomerDTO;

                return View(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine("rrrrrrrrrrrrrrrrrr" + ex.Message);
                return View(new SearchResultCustomerDTO());
                throw;
            }
        }

        // GET: CustomerController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var result = new GetIdResultCustomerDTO();

            var response = await _httpClientCRMAPI.GetAsync($"/customer/{id}");

            if (response.IsSuccessStatusCode)
                result = await response.Content.ReadFromJsonAsync<GetIdResultCustomerDTO>();


            return View(result ?? new GetIdResultCustomerDTO());
        }

        // GET: CustomerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCustomerDTO createCustomerDTO)
        {
            try
            {
                var response = await _httpClientCRMAPI.PostAsJsonAsync<CreateCustomerDTO>("customer", createCustomerDTO);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));

                ViewBag.Error = "Error al intertar guardar el registro";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        // GET: CustomerController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var result = new GetIdResultCustomerDTO();

            var respose = await _httpClientCRMAPI.GetAsync($"/customer/{id}");

            if (respose.IsSuccessStatusCode)
                result = await respose.Content.ReadFromJsonAsync<GetIdResultCustomerDTO>();

            return View(new EditCustomerDTO(
                result ?? new GetIdResultCustomerDTO()
            ));
        }

        // POST: CustomerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, EditCustomerDTO editCustomerDTO)
        {
            try
            {
                var response = await _httpClientCRMAPI.PutAsJsonAsync("/customer", editCustomerDTO);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));

                ViewBag.Error = "Error al intentar editar el registro";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        // GET: CustomerController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var result = new GetIdResultCustomerDTO();

            var respose = await _httpClientCRMAPI.GetAsync($"/customer/{id}");

            if (respose.IsSuccessStatusCode)
                result = await respose.Content.ReadFromJsonAsync<GetIdResultCustomerDTO>();

            return View(result ?? new GetIdResultCustomerDTO());
        }

        // POST: CustomerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, GetIdResultCustomerDTO getIdResultCustomerDTO)
        {
            try
            {
                var response = await _httpClientCRMAPI.DeleteAsync($"/customer/{id}");

                if (response.IsSuccessStatusCode)
                    return RedirectToAction(nameof(Index));

                ViewBag.Error = "Error la intertar eliminar el registro";
                return View(getIdResultCustomerDTO);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View(getIdResultCustomerDTO);
            }
        }
    }
}
