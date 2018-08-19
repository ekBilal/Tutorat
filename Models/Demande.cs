using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
	public class Demande
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public virtual Etudiant Etudiant { get; set; }
		[Required]
		public virtual Cours cours { get; set; }
		public string Commentaire { get; set; }
		[Required, DefaultValue(Etat.EnAttente)]
		public Etat Etat { get; set; }
	}
}
