using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Models;
using Web.Models;
using Newtonsoft.Json;

namespace Web.Controllers
{
	public class EtudiantsController : Controller
	{
		private ServiceApi api = ServiceApi.API;
		private readonly string uri = "Etudiants/";

		// GET: Etudiants
		public async Task<ActionResult> Index()
		{
			var result = await api.Get(uri);
			var etudiants = JsonConvert.DeserializeObject<List<Etudiant>>(result);
			return View(etudiants);
		}

		// GET: Etudiants/Details/5
		public async Task<ActionResult> Details(int? id)
		{
			if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			var result = await api.Get(uri + id);
			var etudiant = JsonConvert.DeserializeObject<Etudiant>(result);
			if (etudiant == null) return HttpNotFound();
			Session["etudiant"] = etudiant;
			return View(etudiant);
		}

		// GET: Etudiants/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Etudiants/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		public async Task<ActionResult> Create([Bind(Include = "PSR,Nom,Prenom")] Etudiant etudiant)
		{
			if (!ModelState.IsValid) return View(etudiant);
			var result = await api.Post(uri, etudiant);
			return RedirectToAction("Index");
		}

		// GET: Etudiants/Edit/5
		public async Task<ActionResult> Edit(int? id)
		{
			if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			var result = await api.Get(uri + id);
			var etudiant = JsonConvert.DeserializeObject<Etudiant>(result);
			if (etudiant == null) return HttpNotFound();
			return View(etudiant);
		}

		// POST: Etudiants/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		public async Task<ActionResult> Edit([Bind(Include = "IdEtudiant,PSR,Nom,Prenom")] Etudiant etudiant)
		{
			if (!ModelState.IsValid) return View(etudiant);
			var result = await api.Put(uri + etudiant.IdEtudiant, etudiant);
			return RedirectToAction("Index");
		}

		// GET: Etudiants/Delete/5
		public async Task<ActionResult> Delete(int? id)
		{
			if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			var result = await api.Get(uri + id);
			var etudiant = JsonConvert.DeserializeObject<Etudiant>(result);
			if (etudiant == null) return HttpNotFound();
			return View(etudiant);
		}

		// POST: Etudiants/Delete/5
		[HttpPost, ActionName("Delete")]
		public async Task<ActionResult> DeleteConfirmed(int id)
		{
			await api.Delete(uri + id);
			return RedirectToAction("Index");
		}
	}
}