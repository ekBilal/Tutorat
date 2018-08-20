using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace Web.Models
{
	public class ServiceApi
	{
		private string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
		private string appKey = ConfigurationManager.AppSettings["ida:ClientSecret"];
		private string aadInstance = ConfigurationManager.AppSettings["ida:AADInstance"];
		private string apiID = null;

		private static ServiceApi api=null;

		private HttpClient client=null;
		private Uri address = new Uri("http://localhost:49909/api/");

		public static ServiceApi API {
			get
			{
				if (api == null)
					API = new ServiceApi();
				return api;
			}
			private set
			{
				if (api == null)
					api = value;
			}
		}
		private ServiceApi()
		{
			client = new HttpClient
			{
				BaseAddress = address
			};
		}

		internal async Task<string> Get(string uri)
		{
			var result = await client.GetStringAsync(uri);
			return result;
		}


		internal async Task<HttpResponseMessage> Post(string requestUri, Object objet)
		{
			var content = new StringContent(JsonConvert.SerializeObject(objet), Encoding.UTF8, "application/json");
			var result = await client.PostAsync(requestUri, content);
			return result;
		}

		internal async Task<HttpResponseMessage> Post(string requestUri, Dictionary<string, string> headers)
		{
			foreach (var item in headers)
			{
				client.DefaultRequestHeaders.Add(item.Key, "" + item.Value);
			}
			var rep = await client.PostAsync(requestUri, null);
			foreach (var item in headers)
			{
				client.DefaultRequestHeaders.Remove(item.Key);
			}
			return rep;
		}


		internal async Task<HttpResponseMessage> Put(string requestUri, Object objet)
		{
			var content = new StringContent(JsonConvert.SerializeObject(objet), Encoding.UTF8, "application/json");
			var result = await client.PutAsync(requestUri, content);
			return result;
		}

		internal async Task<HttpResponseMessage> Delete(string requestUri)
		{
			var result = await client.DeleteAsync(requestUri);
			return result;
		}


/*		private async Task<string> GetTokenForApplication()
		{
			string signedInUserID = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;
			string tenantID = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid").Value;
			string userObjectID = ClaimsPrincipal.Current.FindFirst("http://schemas.microsoft.com/identity/claims/objectidentifier").Value;

			// Obtenir un jeton pour l'API Graph sans intervention de l'utilisateur (à partir du cache, via un jeton d'actualisation multiressource, etc.)
			ClientCredential clientcred = new ClientCredential(clientId, appKey);
			// Initialiser AuthenticationContext avec le cache de jetons de l'utilisateur connecté, tel qu'il figure dans la base de données de l'application
			AuthenticationContext authenticationContext = new AuthenticationContext(aadInstance + tenantID, new ADALTokenCache(signedInUserID));
			AuthenticationResult authenticationResult = await authenticationContext.AcquireTokenSilentAsync(apiID, clientcred, new UserIdentifier(userObjectID, UserIdentifierType.UniqueId));
			return authenticationResult.AccessToken;
		}
		*/
	}
}