﻿using FluentAssertions;
using MrCMS.EcommerceApp.Tests.Services.ImportExport.ImportProductsServiceTests;
using MrCMS.Services;
using MrCMS.Settings;
using MrCMS.Web.Apps.Ecommerce.Services.Products;
using Xunit;

namespace MrCMS.EcommerceApp.Tests.Services
{
    public class ProductOrCategoryDeletedTest : InMemoryDatabaseTest
    {
        private readonly SiteSettings _siteSettings;
        private readonly IDocumentService _documentService;
        private readonly ProductService _productService;

        public ProductOrCategoryDeletedTest()
        {
            _siteSettings = new SiteSettings();
            _documentService = new DocumentService(Session, _siteSettings, CurrentSite);
            _productService = new ProductService(Session, _documentService, _siteSettings, null);

        }

        [Fact]
        public void DeletingCategoryShouldDeleteProductMapping()
        {
            //build
            var category = new CategoryBuilder().Build();
            _documentService.AddDocument(category);
            var product = new ProductBuilder().Build();
            _documentService.AddDocument(product);

            //test
            _productService.AddCategory(product, category.Id);
            product.Categories.Count.Should().Be(1);
            _documentService.DeleteDocument(category);
            product.Categories.Count.Should().Be(0);
        }

        [Fact]
        public void DeletingProductShouldDeleteCategoryMapping()
        {
            //build
            var category = new CategoryBuilder().Build();
            _documentService.AddDocument(category);
            var product = new ProductBuilder().Build();
            _documentService.AddDocument(product);

            //test
            _productService.AddCategory(product, category.Id);
            category.Products.Count.Should().Be(1);
            _documentService.DeleteDocument(product);
            category.Products.Count.Should().Be(0);
        }

        [Fact]
        public void ProductOnDeletingRemovesCategory()
        {
            //arrange
            var category = new CategoryBuilder().Build();
            var product = new ProductBuilder().Build();
            product.Categories.Add(category);
            category.Products.Add(product);
            product.Categories.Count.Should().Be(1);
            category.Products.Count.Should().Be(1);

            //act
            product.OnDeleting(Session);
            
            //assert
            product.Categories.Count.Should().Be(0);
            category.Products.Count.Should().Be(0);
        }
    }
}