using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Tutorat;
using Models;
using Newtonsoft.Json;

namespace API.Controllers
{
	public class CoursController : ApiController
	{
		private BddContext db = new BddContext();

		// GET: api/Cours
		[ResponseType(typeof(List<Cours>))]
		public IHttpActionResult GetCours()
		{
			var cours = db.Cours.Where(c => c.Annulee == false).ToList();
			return Ok(cours);
		}

		// GET: api/Cours/5
		[ResponseType(typeof(Cours))]
		public async Task<IHttpActionResult> GetCours(int id)
		{
			var cours = await db.Cours.FindAsync(id);
			if (cours == null) return NotFound();
			return Ok(cours);
		}

		// PUT: api/Cours/5
		[ResponseType(typeof(void))]
		public async Task<IHttpActionResult> PutCours(int id, Cours cours)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);
			if (id != cours.IdCours) return BadRequest();
			db.Entry(cours).State = EntityState.Modified;

			try
			{
				await db.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!CoursExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return Ok(cours);
		}

		// POST: api/Cours
		[ResponseType(typeof(Cours))]
		public async Task<IHttpActionResult> PostCours(Cours cours)
		{
			if (!ModelState.IsValid) return BadRequest(ModelState);
			db.Cours.Add(cours);
			await db.SaveChangesAsync();

			return CreatedAtRoute("DefaultApi", new { id = cours.IdCours }, cours);
		}

		// DELETE: api/Cours/5
		[ResponseType(typeof(Cours))]
		public async Task<IHttpActionResult> DeleteCours(int id)
		{
			Cours cours = await db.Cours.FindAsync(id);
			if (cours == null) return NotFound();
			cours.Annulee = true;
			db.Entry(cours).State = EntityState.Modified;
			await db.SaveChangesAsync();
			return Ok(cours);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}

		private bool CoursExists(int id)
		{
			return db.Cours.Count(e => e.IdCours == id) > 0;
		}
	}
}