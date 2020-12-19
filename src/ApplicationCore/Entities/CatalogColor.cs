using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.eShopWeb.ApplicationCore.Entities
{
    public class CatalogColor : BaseEntity, IAggregateRoot
    {
        public string Color { get; private set; }
        public CatalogColor(string color)
        {
            Color = color;
        }
    }
}
