using Okane.Domain;
using System.Text.RegularExpressions;

namespace Okane.Application;

public class ExpenseService : IExpenseService
{
    private readonly IExpensesRepository _expensesRepository;
    private readonly Func<DateTime> _getCurrentTime;

    public ExpenseService(IExpensesRepository expensesRepository, Func<DateTime> getCurrentTime)
    {
        _expensesRepository = expensesRepository;
        _getCurrentTime = getCurrentTime;
    }

    
    public ExpenseResponse Register(CreateExpenseRequest request)
    {

        /*if (!isInvalidUrl(request.InvoiceUrl))
        {
            Console.WriteLine("Factura no valida.");
            ModelState.AddModelError("InvoiceUrl", "La url que has enviado no es una valida, por favor, verifica.");
            return BadReqeust(ModelState);
        
        }*/
        var expense = new Expense
        {
            Amount = request.Amount,
            Description = request.Description,
            Category = request.Category,
            InvoiceUrl = request.InvoiceUrl,
            CreatedAt = _getCurrentTime()
        };
        
        _expensesRepository.Add(expense);
        
        return CreateExpenseResponse(expense);
    }

    public ExpenseResponse Update(int id, UpdateExpenseRequest request)
    {
        var expense = _expensesRepository.Update(id, request);
        return CreateExpenseResponse(expense);
    }

    public ExpenseResponse? ById(int id)
    {
        var expense = _expensesRepository.ById(id);

        return expense == null ? null : CreateExpenseResponse(expense);
    }

    public IEnumerable<ExpenseResponse> Search(string? category = null) => 
        _expensesRepository
            .Search(category)
            .Select(CreateExpenseResponse);

    public bool Delete(int id)
    {
        var expenseToDelete = _expensesRepository.ById(id);

        if (expenseToDelete == null)
            return false;
        
        _expensesRepository.Delete(id);
        return true;
    }

    private static ExpenseResponse CreateExpenseResponse(Expense expense) =>
        new()
        {
            Id = expense.Id,
            Category = expense.Category,
            Description = expense.Description,
            Amount = expense.Amount,
            InvoiceUrl = expense.InvoiceUrl,
            CreatedAt = expense.CreatedAt
        };



  
}