using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Models
{
	public class Demande
	{
		[Key]
		public int IdDemande { get; set; }
		[Required]
		public virtual Etudiant Etudiant { get; set; }
		[Required]
		public virtual Cours Cours { get; set; }
		public string Commentaire { get; set; }
		[Required, DefaultValue(Etat.EnAttente)]
		public Etat Etat { get; set; }
	}
}
