// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace IdentityServer
{
    public static class Config
    {
        public static IConfigurationRoot ConfigurationRoot { get; set; }

        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            { 
                new IdentityResources.OpenId()
            };

        public static IEnumerable<ApiScope> ApiScopes() 
        {
            string scope = ConfigurationRoot.GetValue<string>("User:Scope");

            return  new ApiScope[]
            {
               new ApiScope(scope,"")
            };
        } 

        public static IEnumerable<Client> Clients ()
        {
            string scope = ConfigurationRoot.GetValue<string>("User:Scope");
            string login = ConfigurationRoot.GetValue<string>("User:Login");
            string password = ConfigurationRoot.GetValue<string>("User:Password");
            
            return new Client[] 
            { 
                new Client()
                {
                    ClientId = login,
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets =
                    {
                    new Secret(password.Sha256())
                    },
                    AllowedScopes = { scope },
                }
            };
        }
           
    }
}