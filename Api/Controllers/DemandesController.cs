using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Models;
using Tutorat;

namespace API.Controllers
{
	public class DemandesController : ApiController
	{
		private BddContext db = new BddContext();

		// GET: api/Demandes
		public IQueryable<Demande> GetDemande()
		{
			return db.Demande;
		}

		// GET: api/Demandes/5
		[ResponseType(typeof(Demande))]
		public async Task<IHttpActionResult> GetDemande(int id)
		{
			var demande = await db.Demande.FindAsync(id);
			if (demande == null) return NotFound();
			return Ok(demande);
		}

		// PUT: api/Demandes/5
		[ResponseType(typeof(void))]
		public async Task<IHttpActionResult> PutDemande(int id, Demande demande)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);
			if (id != demande.IdDemande) return BadRequest();
			db.Entry(demande).State = EntityState.Modified;

			try
			{
				await db.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!DemandeExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return Ok(demande);
		}

		// POST: api/Demandes
		[ResponseType(typeof(Demande))]
		public async Task<IHttpActionResult> PostDemande(Demande demande)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);
			demande.Etudiant = await db.Etudiant.FindAsync(demande.Etudiant.IdEtudiant);
			demande.Cours = await db.Cours.FindAsync(demande.Cours.IdCours);


			db.Demande.Add(demande);
			await db.SaveChangesAsync();

			return CreatedAtRoute("DefaultApi", new { id = demande.IdDemande }, demande);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}

		private bool DemandeExists(int id)
		{
			return db.Demande.Count(e => e.IdDemande == id) > 0;
		}
	}
}