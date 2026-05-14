using Microsoft.AspNetCore.Mvc;
using Proge2._1.Data;
using Proge2._1.Models;
using Proge2._1.Search;
using Proge2._1.Services.Interfaces;
using System.Threading.Tasks;

namespace Proge2._1.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        // ✅ GET: Comments (with search + paging)
        public async Task<IActionResult> Index(int page = 1, CommentIndexModel model = null)
        {
            model ??= new CommentIndexModel();
            model.Data = await _commentService.GetPagedComments(page, 10, model.Search);
            return View(model);
        }


        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var comment = await _commentService.GetCommentById(id.Value);
            if (comment == null)
                return NotFound();

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
        public async Task<IActionResult> Create([Bind("Content,User")] Comment comment)
        {
            if (!ModelState.IsValid)
                return View(comment);

            await _commentService.AddComment(comment);
            return RedirectToAction(nameof(Index));
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var comment = await _commentService.GetCommentById(id.Value);
            if (comment == null)
                return NotFound();

            return View(comment);
        }

        // POST: Comments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Comment comment)
        {
            if (id != comment.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(comment);

            try
            {
                await _commentService.UpdateComment(comment);
            }
            catch
            {
                if (!await _commentService.CommentExists(comment.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var comment = await _commentService.GetCommentById(id.Value);
            if (comment == null)
                return NotFound();

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _commentService.DeleteComment(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
