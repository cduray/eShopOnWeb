﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Specifications;
using Microsoft.eShopWeb.Web.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.eShopWeb.Web.Services
{
    /// <summary>
    /// This is a UI-specific service so belongs in UI project. It does not contain any business logic and works
    /// with UI-specific types (view models and SelectListItem types).
    /// </summary>
    public class CatalogViewModelService : ICatalogViewModelService
    {
        private readonly ILogger<CatalogViewModelService> _logger;
        private readonly IAsyncRepository<CatalogItem> _itemRepository;
        private readonly IAsyncRepository<CatalogBrand> _brandRepository;
        private readonly IAsyncRepository<CatalogType> _typeRepository;
        private readonly IAsyncRepository<CatalogColor> _colorRepository;
        private readonly IUriComposer _uriComposer;

        public CatalogViewModelService(
            ILoggerFactory loggerFactory,
            IAsyncRepository<CatalogItem> itemRepository,
            IAsyncRepository<CatalogBrand> brandRepository,
            IAsyncRepository<CatalogType> typeRepository,
            IAsyncRepository<CatalogColor> colorRepository,
            IUriComposer uriComposer)
        {
            _logger = loggerFactory.CreateLogger<CatalogViewModelService>();
            _itemRepository = itemRepository;
            _brandRepository = brandRepository;
            _typeRepository = typeRepository;
            _colorRepository = colorRepository;
            _uriComposer = uriComposer;
        }

        public async Task<CatalogIndexViewModel> GetCatalogItems(int pageIndex, int itemsPage, int? brandId, int? typeId, int? colorId)
        {
            _logger.LogInformation("GetCatalogItems called.");

            var filterSpecification = new CatalogFilterSpecification(brandId, typeId, colorId);
            var filterPaginatedSpecification =
                new CatalogFilterPaginatedSpecification(itemsPage * pageIndex, itemsPage, brandId, typeId, colorId);

            // the implementation below using ForEach and Count. We need a List.
            var itemsOnPage = await _itemRepository.ListAsync(filterPaginatedSpecification);
            var totalItems = await _itemRepository.CountAsync(filterSpecification);

            var vm = new CatalogIndexViewModel()
            {
                CatalogItems = itemsOnPage.Select(i => new CatalogItemViewModel()
                {
                    Id = i.Id,
                    Name = i.Name,
                    PictureUri = _uriComposer.ComposePicUri(i.PictureUri),
                    Price = i.Price
                }).ToList(),
                Brands = (await GetBrands()).ToList(),
                Types = (await GetTypes()).ToList(),
                Colors = (await GetColors()).ToList(),
                BrandFilterApplied = brandId ?? 0,
                TypesFilterApplied = typeId ?? 0,
                ColorsFilterApplied = colorId ?? 0,
                PaginationInfo = new PaginationInfoViewModel()
                {
                    ActualPage = pageIndex,
                    ItemsPerPage = itemsOnPage.Count,
                    TotalItems = totalItems,
                    TotalPages = int.Parse(Math.Ceiling(((decimal)totalItems / itemsPage)).ToString())
                }
            };

            vm.PaginationInfo.Next = (vm.PaginationInfo.ActualPage == vm.PaginationInfo.TotalPages - 1) ? "is-disabled" : "";
            vm.PaginationInfo.Previous = (vm.PaginationInfo.ActualPage == 0) ? "is-disabled" : "";

            return vm;
        }

        public async Task<IEnumerable<SelectListItem>> GetBrands()
        {
            _logger.LogInformation("GetBrands called.");
            var brands = await _brandRepository.ListAllAsync();

            var items = brands
                .Select(brand => new SelectListItem() { Value = brand.Id.ToString(), Text = brand.Brand })
                .OrderBy(b => b.Text)
                .ToList();

            var allItem = new SelectListItem() { Value = null, Text = "All", Selected = true };
            items.Insert(0, allItem);

            return items;
        }

        public async Task<IEnumerable<SelectListItem>> GetTypes()
        {
            _logger.LogInformation("GetTypes called.");
            var types = await _typeRepository.ListAllAsync();

            var items = types
                .Select(type => new SelectListItem() { Value = type.Id.ToString(), Text = type.Type })
                .OrderBy(t => t.Text)
                .ToList();

            var allItem = new SelectListItem() { Value = null, Text = "All", Selected = true };
            items.Insert(0, allItem);

            return items;
        }

        public async Task<IEnumerable<SelectListItem>> GetColors()
        {
            _logger.LogInformation("GetColors called.");
            var colors = await _colorRepository.ListAllAsync();

            var items = colors
                .Select(colors => new SelectListItem() { Value = colors.Id.ToString(), Text = colors.Color })
                .OrderBy(c => c.Text)
                .ToList();

            var allItem = new SelectListItem() { Value = null, Text = "All", Selected = true };
            items.Insert(0, allItem);

            return items;
        }
    }
}
