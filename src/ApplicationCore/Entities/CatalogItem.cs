﻿using System;
using Ardalis.GuardClauses;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using System.Collections.Generic;

namespace Microsoft.eShopWeb.ApplicationCore.Entities
{


    public class CatalogItem : BaseEntity, IAggregateRoot
    {
        public string Name { get; private set; }
        public int CatalogColorId { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public string PictureUri { get; private set; }
        public int CatalogTypeId { get; private set; }
        public CatalogType CatalogType { get; private set; }
        public int CatalogBrandId { get; private set; }
        public CatalogBrand CatalogBrand { get; private set; }
        public CatalogColor CatalogColor { get; private set; }

        public CatalogItem(int catalogTypeId, 
            int catalogBrandId, 
            int catalogColorId,
            string description,
            string name, 
            decimal price, 
            string pictureUri)
        {
            CatalogTypeId = catalogTypeId;
            CatalogBrandId = catalogBrandId;
            CatalogColorId = catalogColorId;
            Description = description;
            Name = name;
            Price = price;
            PictureUri = pictureUri;
        }

        public void UpdateDetails(string name, string description, decimal price)
        {
            Guard.Against.NullOrEmpty(name, nameof(name));
            Guard.Against.NullOrEmpty(description, nameof(description));
            Guard.Against.NegativeOrZero(price, nameof(price));

            Name = name;
            Description = description;
            Price = price;
        }

        public void UpdateBrand(int catalogBrandId)
        {
            Guard.Against.Zero(catalogBrandId, nameof(catalogBrandId));
            CatalogBrandId = catalogBrandId;
        }

        public void UpdateType(int catalogTypeId)
        {
            Guard.Against.Zero(catalogTypeId, nameof(catalogTypeId));
            CatalogTypeId = catalogTypeId;
        }

        public void UpdateColor(int catalogColorId)
        {
            Guard.Against.Zero(catalogColorId, nameof(catalogColorId));
            CatalogTypeId = catalogColorId;
        }

        public void UpdatePictureUri(string pictureName)
        {
            if (string.IsNullOrEmpty(pictureName))
            {
                PictureUri = string.Empty;
                return;
            }
            PictureUri = $"images\\products\\{pictureName}?{new DateTime().Ticks}";
        }
    }
}