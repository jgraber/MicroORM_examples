using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Quotations.DataAccess;

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
            return View();
        }

        // GET: Quote/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Quote/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

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
            return View();
        }

        // POST: Quote/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
