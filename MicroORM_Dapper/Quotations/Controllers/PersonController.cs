using System.Web.Mvc;
using Quotations.DataAccess;
using Quotations.Models;

namespace Quotations.Controllers
{
    public class PersonController : Controller
    {
        private IRepository _repository = new DapperRepository();

        // GET: Person
        public ActionResult Index()
        {
            var allPersons = _repository.GetAllPersons();
            return View(allPersons);
        }

        // GET: Person/Details/5
        public ActionResult Details(int id)
        {
            var person = _repository.FindPerson(id);
            return View(person);
        }

        // GET: Person/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Person/Create
        [HttpPost]
        public ActionResult Create(Person person)
        {
            try
            {
                _repository.Add(person);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(person);
            }
        }

        // GET: Person/Edit/5
        public ActionResult Edit(int id)
        {
            var person = _repository.FindPerson(id);
            return View(person);
        }

        // POST: Person/Edit/5
        [HttpPost]
        public ActionResult Edit(Person person)
        {
            try
            {
                _repository.Update(person);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(person);
            }
        }

        // GET: Person/Delete/5
        public ActionResult Delete(int id)
        {
            var person = _repository.FindPerson(id);
            return View(person);
        }

        // POST: Person/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                _repository.DeletePerson(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
