using Microsoft.AspNetCore.Mvc;
using Okane.Application;
using Okane.Domain;

namespace Okane.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ExpensesController : ControllerBase
{
    private readonly IExpenseService _expensesService;

    public ExpensesController(IExpenseService expensesService) => 
        _expensesService = expensesService;
    
    // POST /expenses
    [HttpPost]
    public ExpenseResponse Post(CreateExpenseRequest request) => 
        _expensesService.RegisterExpense(request);
    
    // GET /expenses
    [HttpGet]
    public IEnumerable<ExpenseResponse> Get(string? category) => 
        _expensesService.Search(category);

    // GET /expenses/:id
    [HttpGet("{id}")]
    public ActionResult<ExpenseResponse> Get(int id)
    {
        var response = _expensesService.ById(id);
        if (response == null)
            return NotFound();
        
        return Ok(response);
    }

    // DELETE /expenses/:id
    [HttpDelete("{id}")]
    public bool Delete(int id) => 
        _expensesService.Delete(id);
}