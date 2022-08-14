using Feeder.Models;
using Feeder.Services;
using Microsoft.AspNetCore.Mvc;

namespace Feeder.Controllers
{
    public class SettingController : Controller
    {
        private ISettingService _service;

        public SettingController(ISettingService service) => _service = service;

        public ActionResult Index()
        {
            var response = _service.GetSetting();
            if(!response.Success)
                return PartialView("Error", response.Message);

            return PartialView(response);
        }

        public JsonResult RemoveChannel(string name)
        {
            var response = _service.RemoveChannel(name);
            return Json(response);
        }

        [HttpPost]
        public JsonResult AddChannel(Tape tape)
        {
            if (!ModelState.IsValid)
                return Json(new ResponseSetting { Success=false, Message = "Заполните поля"});

            var response = _service.AddChannel(tape);
            return Json(response);
        }

        public JsonResult UpdateTime(int time)
        {
            var response = _service.ChangeUpdateTime(time);
            return Json(response);
        }

        [HttpPost]
        public JsonResult EnabledChannel(Tape tape)
        {
            var response = _service.EnabledChannel(tape);
            return Json(response);
        }

        public JsonResult ChangeFormatByTags(bool isFormat)
        {
            var response = _service.ChangeFormatByTags(isFormat);
            return Json(response);
        }

        [HttpPost]
        public JsonResult ChangeProxyData(Proxy proxy)
        {
            if (!ModelState.IsValid)
                return Json(new ResponseSetting { Success = false, Message = "Заполните поля" });

            var response = _service.ChangeProxyData(proxy);
            return Json(response);
        }

        public JsonResult EnableProxy(bool isEnabled)
        {
            var response = _service.EnabledProxy(isEnabled);
            return Json(response);
        }
    }
}
