using Feeder.Services;
using Microsoft.AspNetCore.Mvc;

namespace Feeder.Controllers
{
    public class FeederController : Controller
    {
        private readonly IFeederService _service;

        public FeederController(IFeederService service) => _service = service;

        public ActionResult Tapes()
        {
            var response =  _service?.GetTapes();
            return PartialView(response);
        }

        [HttpPost]
        public async Task<ActionResult> GetPosts(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                return Content("Пустая адресная строка");

            var response = await _service?.GetItems(address);

            if(!response.Success)
                return PartialView("Error", response);

            return PartialView(response);
        }

        public async Task<ActionResult> GetAllItems(int page = 0)
        {
            var response = await _service?.GetAllItems();
            int pageSize = 10;

            if(!response.Success)
                return PartialView("Error", response);

            response.Data = response.Data.Skip((page-1) * pageSize).Take(pageSize).ToList();
            if (response.Data.Count < 1)
                return null;

            if(page != 0)
                return PartialView("GetPosts", response);

            return PartialView(response);
        }
    }
}
