using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Microsoft.Azure.ActiveDirectory.GraphClient;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using Web.Models;
using Newtonsoft.Json;
using Models;

namespace Web.Controllers
{
    [Authorize]
    public class UserProfileController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
        private string appKey = ConfigurationManager.AppSettings["ida:ClientSecret"];
        private string aadInstance = ConfigurationManager.AppSettings["ida:AADInstance"];
        private string graphResourceID = "https://graph.windows.net/";

		private ServiceApi api = ServiceApi.API;

        // OBTENIR : UserProfile
        public async Task<ActionResult> Index()
        {
            string tenantID = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
            string userObjectID = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;
            try
            {
                Uri servicePointUri = new Uri(graphResourceID);
                Uri serviceRoot = new Uri(servicePointUri, tenantID);
                ActiveDirectoryClient activeDirectoryClient = new ActiveDirectoryClient(serviceRoot,
                      async () => await GetTokenForApplication());

                // Utiliser le jeton pour interroger l'API Graph et obtenir les détails relatifs à l'utilisateur

                var result = await activeDirectoryClient.Users
                    .Where(u => u.ObjectId.Equals(userObjectID))
                    .ExecuteAsync();
                IUser user = result.CurrentPage.ToList().First();

				Etudiant etudiant;
				var rep = await api.Get("Etudiants/");
				if (rep.Contains(user.MailNickname))
				{
					var etudiants = JsonConvert.DeserializeObject<List<Etudiant>>(rep);
					etudiant = etudiants.Find(e => e.PSR == user.MailNickname);
					return RedirectToAction("Details/" + etudiant.IdEtudiant, "Etudiants");
				}
				else
				{
					etudiant = new Etudiant { PSR = user.MailNickname ,Nom=user.Surname, Prenom=user.GivenName};
				}
				Session["etudiant"] = etudiant;
				return View(etudiant);
            }
            catch (AdalException)
            {
                // Retourner à la page d'erreur.
                return View("Error");
            }
            // En cas d'échec de l'opération ci-dessus, l'utilisateur doit se réauthentifier explicitement pour que l'application obtienne le jeton nécessaire
            catch (Exception)
            {
                return View("Relogin");
            }
        }


		public async Task<ActionResult> Inscription(Etudiant etudiant)
		{
			var result = await api.Post("Etudiants/", etudiant);
			return RedirectToAction("Index");
		}

		public void RefreshSession()
        {
            HttpContext.GetOwinContext().Authentication.Challenge(
                new AuthenticationProperties { RedirectUri = "/UserProfile" },
                OpenIdConnectAuthenticationDefaults.AuthenticationType);
        }

        public async Task<string> GetTokenForApplication()
        {
            string signedInUserID = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;
            string tenantID = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
            string userObjectID = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

            // Obtenir un jeton pour l'API Graph sans intervention de l'utilisateur (à partir du cache, via un jeton d'actualisation multiressource, etc.)
            ClientCredential clientcred = new ClientCredential(clientId, appKey);
            // Initialiser AuthenticationContext avec le cache de jetons de l'utilisateur connecté, tel qu'il figure dans la base de données de l'application
            AuthenticationContext authenticationContext = new AuthenticationContext(aadInstance + tenantID, new ADALTokenCache(signedInUserID));
            AuthenticationResult authenticationResult = await authenticationContext.AcquireTokenSilentAsync(graphResourceID, clientcred, new UserIdentifier(userObjectID, UserIdentifierType.UniqueId));
            return authenticationResult.AccessToken;
        }
    }
}
