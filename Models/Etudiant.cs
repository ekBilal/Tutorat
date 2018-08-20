using Newtonsoft.Json;
using System.Collections.Generic;
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
		[JsonIgnore]
		public virtual IList<Cours> Tuteurs { get; set; }
		public virtual IList<Cours> Cours { get; set; }
	}
}
