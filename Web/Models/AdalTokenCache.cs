using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Security;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace Web.Models
{
    public class ADALTokenCache : TokenCache
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private string userId;
        private UserTokenCache Cache;

        public ADALTokenCache(string signedInUserId)
        {
            // Associer le cache à l'utilisateur actuel de l'application web
            userId = signedInUserId;
            this.AfterAccess = AfterAccessNotification;
            this.BeforeAccess = BeforeAccessNotification;
            this.BeforeWrite = BeforeWriteNotification;
            // Rechercher l'entrée dans la base de données
            Cache = db.UserTokenCacheList.FirstOrDefault(c => c.webUserUniqueId == userId);
            // Mettre l'entrée en mémoire
            this.Deserialize((Cache == null) ? null : MachineKey.Unprotect(Cache.cacheBits,"ADALCache"));
        }

        // Nettoyer la base de données
        public override void Clear()
        {
            base.Clear();
            var cacheEntry = db.UserTokenCacheList.FirstOrDefault(c => c.webUserUniqueId == userId);
            db.UserTokenCacheList.Remove(cacheEntry);
            db.SaveChanges();
        }

        // Notification générée avant qu'ADAL n'accède au cache.
        // Vous pouvez mettre à jour la copie en mémoire de la base de données si la version en mémoire contient des données périmées
        void BeforeAccessNotification(TokenCacheNotificationArgs args)
        {
            if (Cache == null)
            {
                // Premier accès
                Cache = db.UserTokenCacheList.FirstOrDefault(c => c.webUserUniqueId == userId);
            }
            else
            {
                // Récupérer la dernière écriture à partir de la base de données
                var status = from e in db.UserTokenCacheList
                             where (e.webUserUniqueId == userId)
                select new
                {
                    LastWrite = e.LastWrite
                };

                // Si la copie en mémoire est plus ancienne que la copie persistante
                if (status.First().LastWrite > Cache.LastWrite)
                {
                    // Lire à partir du stockage, mettre à jour la copie en mémoire
                    Cache = db.UserTokenCacheList.FirstOrDefault(c => c.webUserUniqueId == userId);
                }
            }
            this.Deserialize((Cache == null) ? null : MachineKey.Unprotect(Cache.cacheBits, "ADALCache"));
        }

        // Notification générée une fois qu'ADAL a accédé au cache.
        // Si l'indicateur HasStateChanged est défini, ADAL a changé le contenu du cache
        void AfterAccessNotification(TokenCacheNotificationArgs args)
        {
            // Si l'état a changé
            if (this.HasStateChanged)
            {
                Cache = new UserTokenCache
                {
                    webUserUniqueId = userId,
                    cacheBits = MachineKey.Protect(this.Serialize(), "ADALCache"),
                    LastWrite = DateTime.Now
                };
                // Mettre à jour la base de données et la dernière écriture
                db.Entry(Cache).State = Cache.UserTokenCacheId == 0 ? EntityState.Added : EntityState.Modified;
                db.SaveChanges();
                this.HasStateChanged = false;
            }
        }

        void BeforeWriteNotification(TokenCacheNotificationArgs args)
        {
            // Pour garantir l'absence d'accès en écriture simultané, utilisez cette notification afin de verrouiller l'entrée
        }

    public override void DeleteItem(TokenCacheItem item)
        {
            base.DeleteItem(item);
        }
    }
}
