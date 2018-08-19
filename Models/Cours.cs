using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
	[Table("Cours")]
	public class Cours
	{
		[Key]
		public int IdCours { get; set; }
		[Required]
		public string Nom { get; set; }
		[Required]
		public bool Annulee { get; set; }
		public virtual ICollection<Etudiant> Etudiants { get; set; }
		public virtual ICollection<Etudiant> Tuteurs { get; set; }


		public Cours()
		{
			this.Etudiants = new HashSet<Etudiant>();
			this.Tuteurs = new HashSet<Etudiant>();
		}
	}
}