using Newtonsoft.Json;
using System.Collections.Generic;
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
		[JsonIgnore]
		public virtual IList<Etudiant> Etudiants { get; set; }
		public virtual IList<Etudiant> Tuteurs { get; set; }

	}
}