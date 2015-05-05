using System.Web.Mvc;
using Quotations.DataAccess;

namespace Quotations.Controllers
{
    public class HomeController : Controller
    {
        private DapperRepository _repository = new DapperRepository();

        public ActionResult Index()
        {
            var quote = _repository.GetRandomQuote();
            return View(quote);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}