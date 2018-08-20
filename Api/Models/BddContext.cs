using System.Data.Entity;
using Models;
namespace Tutorat
{
	public class BddContext : DbContext
	{
		public DbSet<Cours> Cours { get; set; }
		public DbSet<Demande> Demande { get; set; }
		public DbSet<Etudiant> Etudiant { get; set; }
	}
}
