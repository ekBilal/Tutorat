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
		[Required, MinLength(5), MaxLength(5), Index(IsUnique = true)]
		public string PSR { get; set; }
		[Required]
		public string Nom { get; set; }
		[Required]
		public string Prenom { get; set; }
		[Required]
		public bool Desinscrit { get; set; }

		public virtual ObservableCollection<Cours> Cours { get; set; }
		public virtual ObservableCollection<Cours> Tuteurs { get; set; }
	}
}
