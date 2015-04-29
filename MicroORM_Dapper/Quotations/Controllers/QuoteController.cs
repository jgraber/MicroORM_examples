using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Quotations.DataAccess;
using Quotations.Models;

namespace Quotations.Controllers
{
    public class QuoteController : Controller
    {
        private DapperRepository _repository = new DapperRepository();

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
            return View();
        }

        // POST: Quote/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

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
