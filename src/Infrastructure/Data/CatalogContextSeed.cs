using Microsoft.EntityFrameworkCore;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.Infrastructure.Data
{
    public class CatalogContextSeed
    {
        public static async Task SeedAsync(CatalogContext catalogContext,
            ILoggerFactory loggerFactory, int? retry = 0)
        {
            int retryForAvailability = retry.Value;
            try
            {
                // TODO: Only run this if using a real database
                // context.Database.Migrate();
                if (!await catalogContext.CatalogBrands.AnyAsync())
                {
                    await catalogContext.CatalogBrands.AddRangeAsync(
                        GetPreconfiguredCatalogBrands());

                    await catalogContext.SaveChangesAsync();
                }

                if (!await catalogContext.CatalogTypes.AnyAsync())
                {
                    await catalogContext.CatalogTypes.AddRangeAsync(
                        GetPreconfiguredCatalogTypes());

                    await catalogContext.SaveChangesAsync();
                }

                if (!await catalogContext.CatalogItems.AnyAsync())
                {
                    await catalogContext.CatalogItems.AddRangeAsync(
                        GetPreconfiguredItems());

                    await catalogContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    var log = loggerFactory.CreateLogger<CatalogContextSeed>();
                    log.LogError(ex.Message);
                    await SeedAsync(catalogContext, loggerFactory, retryForAvailability);
                }
                throw;
            }
        }

        static IEnumerable<CatalogBrand> GetPreconfiguredCatalogBrands()
        {
            return new List<CatalogBrand>()
            {
                new CatalogBrand("CSS"),
                new CatalogBrand("Moleskine"),
                new CatalogBrand("Nike"),
                new CatalogBrand("CI Sport"),
                new CatalogBrand("Under Armour")
            };
        }

        static IEnumerable<CatalogType> GetPreconfiguredCatalogTypes()
        {
            return new List<CatalogType>()
            {
                new CatalogType("Face Mask"),
                new CatalogType("Notebook"),
                new CatalogType("Gift Card"),
                new CatalogType("Hoodie"),
                new CatalogType("Sweatshirt"),
                new CatalogType("T-Shirt")
            };
        }

        static IEnumerable<CatalogItem> GetPreconfiguredItems()
        {
            return new List<CatalogItem>()
            {
                new CatalogItem(3, 1, "Blue", "Saints Shop Gift Card", "Saints Shop Gift Card", 20, "https://cdn.shopify.com/s/files/1/0008/6491/1404/products/SS-giftcard_800x.jpg?v=1572293720"),
                new CatalogItem(3, 1, "Blue", "Saints Shop Gift Card", "Saints Shop Gift Card", 15, "https://cdn.shopify.com/s/files/1/0008/6491/1404/products/SS-giftcard_800x.jpg?v=1572293720"),
                new CatalogItem(3, 1, "Blue", "Saints Shop Gift Card", "Saints Shop Gift Card", 10, "https://cdn.shopify.com/s/files/1/0008/6491/1404/products/SS-giftcard_800x.jpg?v=1572293720"),
                new CatalogItem(3, 1, "Blue", "Saints Shop Gift Card", "Saints Shop Gift Card", 5, "https://cdn.shopify.com/s/files/1/0008/6491/1404/products/SS-giftcard_800x.jpg?v=1572293720"),
                new CatalogItem(2, 2, "Cadmium Orange", "Moleskine 12 Month Weekly Notebook", "Moleskine 12 Month Weekly Notebook", 11.48M, "https://cdn.shopify.com/s/files/1/0008/6491/1404/products/dc43e1d1d421c33f01a27e22e2856143_400x.jpg?v=1597860220"),
                new CatalogItem(2, 2, "Bougainvillea Pink", "Moleskine 12 Month Weekly Notebook", "Moleskine 12 Month Weekly Notebook", 11.48M, "https://cdn.shopify.com/s/files/1/0008/6491/1404/products/3aedc8c0d1d154228edfea00ecee4e48_600x.jpg?v=1597860216"),
                new CatalogItem(6, 5, "True Gray Heather", "Under Armour S19 Mens Tech Tee 2.0", "Under Armour S19 Mens Tech Tee 2.0", 29.95M, "https://cdn.shopify.com/s/files/1/0008/6491/1404/products/858e89ef9366782b9fabeb380ade3438_2adf3b1d-1950-4e31-8a3a-ace88c61d6bb_800x.jpg?v=1582926911"),
                new CatalogItem(1, 1, "Solid Black", "Blue 84 Face Mask", "Blue 84 Face Mask", 4.5M, "https://cdn.shopify.com/s/files/1/0008/6491/1404/products/2d7c03d6a84d5f2348cc2b41f845f50f_800x.jpg?v=1603464191"),
                new CatalogItem(1, 1, "Navy", "Spirit St. Scholastica Zuma Face Mask with Filter", "Spirit St. Scholastica Zuma Face Mask with Filter", 6.65M, "https://cdn.shopify.com/s/files/1/0008/6491/1404/products/image_400x.jpg?v=1602515405"),
                new CatalogItem(5, 5, "Midnight Navy", "Under Armour F19 Women's Double Knit 1/4 Snap", "Under Armour F19 Women's Double Knit 1/4 Snap", 57, "https://cdn.shopify.com/s/files/1/0008/6491/1404/products/0ec2f0c4e0959ac595ccefa95608cddf_800x.jpg?v=1582926782"),
                new CatalogItem(5, 4, "Royal", "CI Sport CSS Crewneck", "CI Sport CSS Crewneck", 45, "https://cdn.shopify.com/s/files/1/0008/6491/1404/products/4c91dbb7996cc6e5504e8019bbefbafc_800x.jpg?v=1590080616"),
                new CatalogItem(4, 4, "Blue", "CI Sport Colorblock Classic Hockey Hood", "CI Sport Colorblock Classic Hockey Hood", 45, "https://cdn.shopify.com/s/files/1/0008/6491/1404/products/a596536dbc6db67e05a550e2d362d96f_800x.jpg?v=1596816366"),
                new CatalogItem(4, 3, "DK Heather", "Nike Stadium Club Fleece Hoody", "Nike Stadium Club Fleece Hoody", 50, "https://cdn.shopify.com/s/files/1/0008/6491/1404/products/3ca661855bcb6954c6c847f000de78f3_800x.jpg?v=1550595616")
            };
        }
    }
}

