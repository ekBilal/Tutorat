using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Models;
using Web.Models;
using Newtonsoft.Json;

using System;

namespace Web.Controllers
{
	[Authorize]
	public class DemandesController : Controller
	{
		private ServiceApi api = ServiceApi.API;
		private readonly string uri = "Demandes/";

		// GET: Demandes
		public async Task<ActionResult> Index()
		{
			var result = await api.Get(uri);
			var demandes = JsonConvert.DeserializeObject<List<Demande>>(result);
			return View(demandes);
		}

		// GET: Demandes/Details/5
		public async Task<ActionResult> Details(int? id)
		{
			if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			var result = await api.Get(uri + id);
			var demandes = JsonConvert.DeserializeObject<Demande>(result);
			if (demandes == null) return HttpNotFound();
			return View(demandes);
		}

		// GET: Demandes/Create
		public async Task<ActionResult> Create()
		{
			var etudiant = (Etudiant)Session["etudiant"];
			var demande = new Demande { Etudiant = etudiant };
			return View(demande);
		}

		// POST: Demandes/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		public async Task<ActionResult> Create(string commentaire, int IdCours)
		{

			try
			{
				var etudiant = (Etudiant)Session["etudiant"];
				var result = await api.Get("Cours/" + IdCours);
				var cours = JsonConvert.DeserializeObject<Cours>(result);
				var demande = new Demande { Commentaire = commentaire, Etudiant = etudiant, Cours = cours };
				var x = await api.Post(uri, demande);

				return RedirectToAction("Details/" + etudiant.IdEtudiant, "Etudiants");

			}
			catch (Exception)
			{
				return View();
			}

		}

		// GET: Demandes/Edit/5
		public async Task<ActionResult> Edit(int? id)
		{
			if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			var result = await api.Get(uri + id);
			var demande = JsonConvert.DeserializeObject<Demande>(result);
			if (demande == null) return HttpNotFound();
			return View(demande);
		}

		// POST: Demandes/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		public async Task<ActionResult> Edit([Bind(Include = "Id,Commentaire,Etat")] Demande demande)
		{
			if (!ModelState.IsValid) return View(demande);
			var result = await api.Put(uri + demande.IdDemande, demande);
			return RedirectToAction("Index");
		}
	}
}
