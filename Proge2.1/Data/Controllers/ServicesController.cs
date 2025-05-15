 
using System.Threading.Tasks;  
using Microsoft.AspNetCore.Mvc;  
using Proge2._1.Data;  
using Proge2._1.Services.Interfaces;  

namespace Proge2._1.Controllers  
{  
    public class ServicesController : Controller  
    {  
        private readonly IServicessService _servicesService; 

        public ServicesController(IServicessService servicesService)  
        {  
            _servicesService = servicesService;  
        }  

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)  
        {  
            var pagedServices = await _servicesService.GetPagedServices(page, pageSize);  
            return View(pagedServices);  
        }  

        public async Task<IActionResult> Details(int? id)  
        {  
            if (id == null)  
            {  
                return NotFound();  
            }  

            var service = await _servicesService.GetServiceById(id.Value);  
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
                await _servicesService.AddService(service);  
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

            var service = await _servicesService.GetServiceById(id.Value);  
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
                    await _servicesService.UpdateService(service);  
                }  
                catch  
                {  
                    if (!await _servicesService.ServiceExists(service.ServiceId))  
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

            var service = await _servicesService.GetServiceById(id.Value);  
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
            await _servicesService.DeleteService(id);  
            return RedirectToAction(nameof(Index));  
        }  
    }  
}  
