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
		public virtual ObservableCollection<Etudiant> Inscrits { get; set; }
		public virtual ObservableCollection<Etudiant> Tuteurs { get; set; }
	}
}