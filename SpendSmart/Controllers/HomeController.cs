using Microsoft.AspNetCore.Mvc;
using SpendSmart.Models;
using SpendSmart.Services;
using System.Diagnostics;

namespace SpendSmart.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IGenericService<Expense> _service;

        public HomeController(ILogger<HomeController> logger, IGenericService<Expense> service)
        {
            _logger = logger;
            _service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Expenses()
        {
            var allExpenses = _service.GetAll();

            var totalExpenses = allExpenses.Sum(expense => expense.Value);
            
            ViewBag.Expenses = totalExpenses;
            return View(allExpenses);
        }

        public IActionResult CreateEditExpense(int? id)
        {
            var expenseInDb = _service.GetById(id ?? 0);
            if (expenseInDb != null)
            {
                return View(expenseInDb);
            }
            return View();
        }

        public IActionResult DeleteExpense(int id)
        {
            _service.Delete(id);
            _service.SaveChanges();
            return RedirectToAction("Expenses");
        }

        public IActionResult CreateEditExpenseForm(Expense model)
        {
            if (model.Id != 0)
            {
                _service.Update(model);
            } else
            {
                _service.Add(model);
            }

            _service.SaveChanges();

            return RedirectToAction("Expenses");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
