// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;

namespace FreeCourse.IdentityServer4
{
    public static class Config
    {
        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                new ApiResource("resource_catalog") {Scopes = { "catalog_full_permission" } },
                new ApiResource("resource_photo_stock") {Scopes = { "photo_stock_full_permission" } },
                new ApiResource("resource_basket") {Scopes = { "basket_full_permission" } },
                new ApiResource("resource_discount") {Scopes = { "discount_full_permission" } },
                new ApiResource("resource_order") {Scopes = { "order_full_permission" } },
                new ApiResource("resource_payment") {Scopes = { "payment_full_permission" } },
                new ApiResource(IdentityServerConstants.LocalApi.ScopeName)
            };

        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile(),
                new IdentityResource() {Name = "roles", DisplayName = "roles", Description = "roles", UserClaims = new [] { "role" } },
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("catalog_full_permission", "Catalog Api için full erişim"),
                new ApiScope("photo_stock_full_permission", "PhotoStock Api için full erişim"),
                new ApiScope("basket_full_permission", "Basket Api için full erişim"),
                new ApiScope("discount_full_permission", "Discount Api için full erişim"),
                new ApiScope("order_full_permission", "Order Api için full erişim"),
                new ApiScope("payment_full_permission", "FakePayment Api için full erişim"),
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName),
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client()
                {
                    ClientName = "AspNetCoreMvc",
                    ClientId = "WebMvcClient",
                    ClientSecrets = {new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "catalog_full_permission", "photo_stock_full_permission", IdentityServerConstants.LocalApi.ScopeName }
                },
                new Client()
                {
                    ClientName = "AspNetCoreMvcForUser",
                    ClientId = "WebMvcClientForUser",
                    ClientSecrets = {new Secret("secret".Sha256())},
                    AllowOfflineAccess = true,
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    AllowedScopes = { "payment_full_permission","order_full_permission", "discount_full_permission", "basket_full_permission", IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Email, IdentityServerConstants.StandardScopes.Profile, IdentityServerConstants.StandardScopes.OfflineAccess, IdentityServerConstants.LocalApi.ScopeName,"roles" },
                    AccessTokenLifetime = 1 * 60 * 60,
                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime = (int)(DateTime.Now.AddDays(60) - DateTime.Now).TotalSeconds,
                    RefreshTokenUsage = TokenUsage.ReUse,
                }
            };
    }
}