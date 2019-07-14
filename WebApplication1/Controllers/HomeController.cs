using System;
using System.Linq;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Tools;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        CalculationContext db = new CalculationContext();
        CalculationService calculationService = new CalculationService();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult History()
        {
            return View(db.CalculationHistory.OrderByDescending(calc => calc.DateTime));
        }

        [HttpGet]
        public ActionResult Calculate(string expression)
        {
            try
            {
                var res = calculationService.Calculate(expression);
                SaveToBd(expression, res);
                return Json(new CalculationResult { Result = res }, JsonRequestBehavior.AllowGet);
            }
            catch(InvalidExpressionException ex)
            {
                return Json(new CalculationResult { Result = null }, JsonRequestBehavior.AllowGet);
            }
        }

        private void SaveToBd(string expression, double res)
        {
            Calculation calculation = new Calculation
            {
                DateTime = DateTime.Now,
                Expression = expression,
                Result = res
            };
            db.CalculationHistory.Add(calculation);
            db.SaveChanges();
        }
    }
}