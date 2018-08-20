using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Models;
using Web.Models;
using Newtonsoft.Json;

namespace Web.Controllers
{
	[System.Web.Mvc.Authorize]
	public class CoursController : Controller
	{
		private ServiceApi api = ServiceApi.API;
		private readonly string uri = "Cours/";

		// GET: Cours
		[AllowAnonymous]
		public async Task<ActionResult> Index()
		{
			var result = await api.Get(uri);
			var cours = JsonConvert.DeserializeObject<List<Cours>>(result);
			return View(cours);
		}

		// GET: Cours/Details/5
		[AllowAnonymous]
		public async Task<ActionResult> Details(int? id)
		{
			if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			var result = await api.Get(uri + id);
			var cours = JsonConvert.DeserializeObject<Cours>(result);
			if (cours == null) return HttpNotFound();
			return View(cours);
		}

		// GET: Cours/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Cours/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		public async Task<ActionResult> Create([Bind(Include = "IdCours,Nom")] Cours cours)
		{
			if (!ModelState.IsValid) return View(cours);
			var result = await api.Post(uri, cours);
			return RedirectToAction("Index");
		}

		// GET: Cours/Edit/5
		public async Task<ActionResult> Edit(int? id)
		{
			if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			var result = await api.Get(uri + id);
			var cours = JsonConvert.DeserializeObject<Cours>(result);
			if (cours == null) return HttpNotFound();
			return View(cours);
		}

		// POST: Cours/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		public async Task<ActionResult> Edit([Bind(Include = "IdCours,Nom")] Cours cours)
		{
			if (!ModelState.IsValid) return View(cours);
			var result = await api.Put(uri + cours.IdCours, cours);
			return RedirectToAction("Index");
		}

		// GET: Cours/Delete/5
		public async Task<ActionResult> Delete(int? id)
		{
			if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			var result = await api.Get(uri + id);
			var cours = JsonConvert.DeserializeObject<Cours>(result);
			if (cours == null) return HttpNotFound();
			return View(cours);
		}

		// POST: Cours/Delete/5
		[HttpPost, ActionName("Delete")]
		public async Task<ActionResult> DeleteConfirmed(int id)
		{
			await api.Delete(uri + id);
			return RedirectToAction("Index");
		}

		// GET: Cours/Edit/5
		public async Task<ActionResult> AddCours()
		{
			var result = await api.Get(uri);
			var cours = JsonConvert.DeserializeObject<List<Cours>>(result);
			if (cours == null) return HttpNotFound();
			return View(cours);
		}

		// POST: Cours/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		public async Task<ActionResult> AddCours(int idCours)
		{
			var idEtudiant = ((Etudiant)Session["etudiant"]).IdEtudiant;
			var dico = new Dictionary<string, string>
			{
				{ "idEtudiant", idEtudiant.ToString() },
				{ "idCours", idCours.ToString() }
			};
			var x = await api.Post("Inscrire", dico);
			return RedirectToAction("Details/" + idEtudiant, "Etudiants");
		}
	}
}