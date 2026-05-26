using Microsoft.AspNetCore.Mvc;
using Proge2._1.Data;
using Proge2._1.Services.Interfaces;

namespace Proge2._1.Controllers
{
    [Route("api/Budgets")]
    [ApiController]
    public class BudgetsApiController : ControllerBase
    {
        private readonly IBudgetService _service;

        public BudgetsApiController(IBudgetService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<Budget>> Get()
        {
            var result = await _service.ListAsync(1, 10000, new Search.BudgetSearch());
            return result.Results;
        }

        [HttpGet("{id}")]
        public async Task<object> Get(int id)
        {
            var budget = await _service.GetBudgetByIdAsync(id);
            if (budget == null)
                return NotFound();
            return budget;
        }

        [HttpPost]
        public async Task<object> Post([FromBody] Budget budget)
        {
            await _service.Save(budget);
            return Ok(budget);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Budget budget)
        {
            if (id != budget.Id)
                return BadRequest();
            var existing = await _service.GetBudgetByIdAsync(id);
            if (existing == null)
                return NotFound();
            await _service.Save(budget);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var budget = await _service.GetBudgetByIdAsync(id);
            if (budget == null)
                return NotFound();
            await _service.DeleteBudgetAsync(id);
            return Ok();
        }
    }
} 