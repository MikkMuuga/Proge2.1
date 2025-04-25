using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Proge2._1.Data;
using Proge2._1.Services.Interfaces;

namespace Proge2._1.Controllers
{
    public class MaterialsController : Controller
    {
        private readonly IMaterialService _materialService;

        public MaterialsController(IMaterialService materialService)
        {
            _materialService = materialService;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            return View(await _materialService.GetPagedMaterials(page, pageSize));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var material = await _materialService.GetMaterialById(id.Value);
            if (material == null)
            {
                return NotFound();
            }

            return View(material);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Unit,Price,Seller")] Materials material)
        {
            if (ModelState.IsValid)
            {
                await _materialService.AddMaterial(material);
                return RedirectToAction(nameof(Index));
            }
            return View(material);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var material = await _materialService.GetMaterialById(id.Value);
            if (material == null)
            {
                return NotFound();
            }
            return View(material);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Unit,Price,Seller")] Materials material)
        {
            if (id != material.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _materialService.UpdateMaterial(material);
                }
                catch
                {
                    if (!await _materialService.MaterialExists(material.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(material);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var material = await _materialService.GetMaterialById(id.Value);
            if (material == null)
            {
                return NotFound();
            }

            return View(material);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _materialService.DeleteMaterial(id);
            return RedirectToAction(nameof(Index));
        }
    }
}