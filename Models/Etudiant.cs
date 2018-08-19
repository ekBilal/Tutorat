using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
	public class Etudiant
	{
		[Key]
		public int IdEtudiant { get; set; }
		[Required, MinLength(8), MaxLength(8), Index(IsUnique = true)]
		public string PSR { get; set; }
		[Required]
		public string Nom { get; set; }
		[Required]
		public string Prenom { get; set; }
		[Required]
		public bool Desinscrit { get; set; }

		public virtual ICollection<Cours> Cours { get; set; }
		public virtual ICollection<Cours> Tuteurs { get; set; }



		public Etudiant()
		{
			this.Cours = new HashSet<Cours>();
			this.Tuteurs = new HashSet<Cours>();
		}
	}
}
