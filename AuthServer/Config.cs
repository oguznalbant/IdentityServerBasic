using IdentityServer4.Models;
using System.Collections.Generic;

namespace AuthServer
{
    public static class Config
    {
        #region Scopes
        //API'larda kullanılacak izinleri tanımlar.
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
        {
            new ApiScope("Garanti.Write","Garanti bankası yazma izni"),
            new ApiScope("Garanti.Read","Garanti bankası okuma izni"),
            new ApiScope("HalkBank.Write","HalkBank bankası yazma izni"),
            new ApiScope("HalkBank.Read","HalkBank bankası okuma izni"),
        };
        }
        #endregion


        #region Resources
        //API'lar tanımlanır.
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
        {
            new ApiResource("Garanti"){ Scopes = { "Garanti.Write", "Garanti.Read" } },
            new ApiResource("HalkBank"){ Scopes = { "HalkBank.Write", "HalkBank.Read" } }
        };
        }
        #endregion


        #region Clients
        //API'ları kullanacak client'lar tanımlanır.
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
        {
            new Client
                    {
                        ClientId = "GarantiBankasi",
                        ClientName = "GarantiBankasi",
                        ClientSecrets = { new Secret("garanti".Sha256()) },
                        AllowedGrantTypes = { GrantType.ClientCredentials }, // clientların yetki tipi
                        AllowedScopes = { "Garanti.Write", "Garanti.Read" }
                    },
            new Client
                    {
                        ClientId = "HalkBankasi",
                        ClientName = "HalkBankasi",
                        ClientSecrets = { new Secret("halkbank".Sha256()) },
                        AllowedGrantTypes = { GrantType.ClientCredentials },
                        AllowedScopes = { "HalkBank.Write", "HalkBank.Read" }
                    }
        };
        }
        #endregion
    }
}
