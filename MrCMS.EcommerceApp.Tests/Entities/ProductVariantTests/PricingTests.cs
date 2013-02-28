﻿using System.Collections.Generic;
using FluentAssertions;
using MrCMS.Web.Apps.Ecommerce.Entities;
using Xunit;
using Xunit.Extensions;

namespace MrCMS.EcommerceApp.Tests.Entities.ProductVariantTests
{
    public class PricingTests
    {
        [Fact]
        public void ProductVariant_ReducedBy_IsZeroIfPreviousPriceIsNotSet()
        {
            var variant = new ProductVariant { PricePreTax = 1, PreviousPrice = null };

            var reducedPrice = variant.ReducedBy;

            reducedPrice.Should().Be(0);
        }
        [Fact]
        public void ProductVariant_ReducedBy_ShouldBeTheDifferenceIfThePreviousPriceIsGreaterThanThePrice()
        {
            var variant = new ProductVariant { PricePreTax = 1, PreviousPrice = 2 };

            var reducedPrice = variant.ReducedBy;

            reducedPrice.Should().Be(1);
        }
        [Fact]
        public void ProductVariant_ReducedBy_ShouldBeZeroIfPriceIsGreaterThanPreviousPrice()
        {
            var variant = new ProductVariant { PricePreTax = 2, PreviousPrice = 1 };

            var reducedPrice = variant.ReducedBy;

            reducedPrice.Should().Be(0);
        }

        [Fact]
        public void ProductVariant_ReducedByPercentage_ShouldBeTheReducedByAsAPercentageOfThePreviousPrice()
        {
            var variant = new ProductVariant { PricePreTax = 1, PreviousPrice = 2 };

            var reducedByPercentage = variant.ReducedByPercentage;

            reducedByPercentage.Should().Be(0.5m);
        }

        [Fact]
        public void ProductVariant_ReducedByPercentage_ShouldReturnZeroIfPreviousPriceIsNull()
        {
            var variant = new ProductVariant { PricePreTax = 1 };

            var reducedByPercentage = variant.ReducedByPercentage;

            reducedByPercentage.Should().Be(0);
        }

        [Fact]
        public void ProductVariant_Price_WithNoTaxRateShouldBePricePreTax()
        {
            var variant = new ProductVariant { PricePreTax = 1 };

            var price = variant.Price;

            price.Should().Be(1);
        }

        [Fact]
        public void ProductVariant_Price_WithTaxRateSetShouldBeTheSameAsPricePreTax()
        {
            var variant = new ProductVariant { PricePreTax = 1, TaxRate = new TaxRate { Percentage = 20 } };

            var price = variant.Price;

            price.Should().Be(1.2m);
        }

        [Fact]
        public void ProductVariant_Price_ShouldBeRoundedTo2DecimalPlaces()
        {
            var variant = new ProductVariant { PricePreTax = 1, TaxRate = new TaxRate { Percentage = 17.5m } };

            var price = variant.Price;

            price.Should().Be(1.18m);
        }

        [Fact]
        public void ProductVariant_Tax_ShouldBePriceMinusPricePreTax()
        {
            var variant = new ProductVariant { PricePreTax = 1, TaxRate = new TaxRate { Percentage = 17.5m } };

            var tax = variant.Tax;

            tax.Should().Be(0.18m);
        }

        [Theory]
        [PropertyData("TaxRates")]
        public void ProductVariant_TaxRatePercentage_IsTakenFromTheTaxRatesPercentage(TaxRate rate, decimal expected)
        {
            var productVariant = new ProductVariant { TaxRate = rate };

            productVariant.TaxRatePercentage.Should().Be(expected);
        }

        [Fact]
        public void ProductVariant_TaxRatePercentage_IsZeroWhenTaxRateIsNull()
        {
            var productVariant = new ProductVariant { TaxRate = null };

            productVariant.TaxRatePercentage.Should().Be(0);
        }

        public static IEnumerable<object[]> TaxRates
        {
            get
            {
                yield return new object[] { new TaxRate { Percentage = 10 }, 10m };
                yield return new object[] { new TaxRate { Percentage = 20 }, 20m };
            }
        } 
    }
}