using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Proge2._1.Data;
using Proge2._1.Services.Interfaces;

namespace Proge2._1.Controllers
{
    public class ServicesController : Controller
    {
        private readonly IServiceService _serviceService;

        public ServicesController(IServiceService serviceService)
        {
            _serviceService = serviceService;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            return View(await _serviceService.GetPagedServices(page, pageSize));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _serviceService.GetServiceById(id.Value);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ServiceId,transportation,PanelProduction,montage")] Servicess service)
        {
            if (ModelState.IsValid)
            {
                await _serviceService.AddService(service);
                return RedirectToAction(nameof(Index));
            }
            return View(service);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _serviceService.GetServiceById(id.Value);
            if (service == null)
            {
                return NotFound();
            }
            return View(service);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ServiceId,transportation,PanelProduction,montage")] Servicess service)
        {
            if (id != service.ServiceId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _serviceService.UpdateService(service);
                }
                catch
                {
                    if (!await _serviceService.ServiceExists(service.ServiceId))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(service);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var service = await _serviceService.GetServiceById(id.Value);
            if (service == null)
            {
                return NotFound();
            }

            return View(service);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _serviceService.DeleteService(id);
            return RedirectToAction(nameof(Index));
        }
    }

    public class Servicess
    {
        internal int ServiceId;
    }
}