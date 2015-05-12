using System.Collections.Generic;
using System.Web.Mvc;
using Quotations.DataAccess;
using Quotations.Models;

namespace Quotations.Controllers
{
    public class QuoteController : Controller
    {
        private IRepository _repository = new DapperRepository();

        // GET: Quote
        public ActionResult Index()
        {
            var quotes = _repository.GetAllQuotes();
            return View(quotes);
        }

        // GET: Quote/Details/5
        public ActionResult Details(int id)
        {
            var quote = _repository.FindQuote(id);
            return View(quote);
        }

        // GET: Quote/Create
        public ActionResult Create()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            _repository.GetAllPersons().ForEach(p => items.Add(new SelectListItem(){Text = p.LastName + " " + p.FirstName, Value = p.Id.ToString()}));
            ViewBag.AuthorId = items;

            return View();
        }

        // POST: Quote/Create
        [HttpPost]
        public ActionResult Create(Quote quote)
        {
            try
            {
                _repository.Add(quote);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Quote/Edit/5
        public ActionResult Edit(int id)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            _repository.GetAllPersons().ForEach(p => items.Add(new SelectListItem() { Text = p.LastName + " " + p.FirstName, Value = p.Id.ToString() }));
            ViewBag.AuthorId = items;

            var quote = _repository.FindQuote(id);
            return View(quote);
        }

        // POST: Quote/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Quote quote)
        {
            try
            {
                _repository.Update(quote);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Quote/Delete/5
        public ActionResult Delete(int id)
        {
            var quote = _repository.FindQuote(id);
            return View(quote);
        }

        // POST: Quote/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                _repository.DeleteQuote(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
