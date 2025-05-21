using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Proge2._1.Data;
using Proge2._1.Services.Interfaces;

namespace Proge2._1.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ICommentService _commentService;

        // 1. Changed constructor to inject ICommentService
        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        // GET: Comments
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)

        {
            // 2. Replaced _context with service call
            return View(await _commentService.GetPagedComments(page, pageSize));
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // 3. Replaced _context with service call
            var comment = await _commentService.GetCommentById(id.Value);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // GET: Comments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Comments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Comment comment)
        {
            if (ModelState.IsValid)
            {
                // 4. Replaced _context with service call
                await _commentService.AddComment(comment);
                return RedirectToAction(nameof(Index));
            }
            return View(comment);
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // 5. Replaced _context with service call
            var comment = await _commentService.GetCommentById(id.Value);
            if (comment == null)
            {
                return NotFound();
            }
            return View(comment);
        }

        // POST: Comments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Comment comment)
        {
            if (id != comment.CommentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // 6. Replaced _context with service call
                    await _commentService.UpdateComment(comment);
                }
                catch
                {
                    if (!await _commentService.CommentExists(comment.CommentId))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(comment);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // 7. Replaced _context with service call
            var comment = await _commentService.GetCommentById(id.Value);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // 8. Replaced _context with service call
            await _commentService.DeleteComment(id);
            return RedirectToAction(nameof(Index));
        }
    }
}