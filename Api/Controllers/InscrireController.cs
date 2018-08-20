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
using System;

namespace API.Controllers
{
	public class InscrireController : ApiController
	{
		private BddContext db = new BddContext();



		[ResponseType(typeof(Etudiant))]
		public async Task<IHttpActionResult> Post()
		{
			if (!Request.Headers.Contains("idEtudiant")) return BadRequest();
			if (!Request.Headers.Contains("idCours")) return BadRequest();

			Etudiant etudiant;
			Cours cours;
			try
			{
				int idEtudiant = int.Parse(Request.Headers.GetValues("idEtudiant").FirstOrDefault());
				int idCours = int.Parse(Request.Headers.GetValues("idCours").FirstOrDefault());

				etudiant = db.Etudiant.Find(idEtudiant);
				cours = db.Cours.Find(idCours);

				etudiant.Cours.Add(cours);
				//				cours.Etudiants.Add(etudiant);

				db.Entry(etudiant).State = EntityState.Modified;
				//				db.Entry(cours).State = EntityState.Modified;

				await db.SaveChangesAsync();
			}
			catch (Exception)
			{
				return BadRequest();
			}
			return Ok(etudiant);
		}
	}
}
