﻿using FluentValidation;
using FluentValidation.AspNetCore;
using Holtz.CQRS.Application.Commands.AddProduct;
using Holtz.CQRS.Application.Interfaces;
using Holtz.CQRS.Application.Queries.GetProducts;
using Holtz.CQRS.Infraestructure.Persistence;
using Holtz.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Reflection;

namespace Holtz.CQRS.Tests
{
    /// <summary>
    /// To make this method works, needs to install XUnit.DependencyInjection package
    /// </summary>
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services) 
        {
            services.AddMediatR(sfg => sfg.RegisterServicesFromAssembly(typeof(GetProductsQueryHandler).GetTypeInfo().Assembly));
            services.AddValidatorsFromAssemblyContaining<AddProductCommandValidator>();

#region Dependency Injection to "Holtz.CQRS.Tests.Api"

            var productsQueryRepositoryMock = new Mock<IProductsQueryRepository>();
            productsQueryRepositoryMock.Setup(x => x.GetProductsAsync()).ReturnsAsync(new List<Product> { new Product("Product 1", "Desc 1", 15), new Product("Product 2", "Desc 2", 12) });
            services.AddTransient<IProductsQueryRepository>(x => productsQueryRepositoryMock.Object);

            var productsCommandRepositoryMock = new Mock<IProductsCommandRepository>();
            productsCommandRepositoryMock.Setup(x => x.AddProductAsync(It.IsAny<Product>())).ReturnsAsync(Guid.NewGuid);
            services.AddTransient<IProductsCommandRepository>(x => productsCommandRepositoryMock.Object);

            #endregion
        }
    }
}
