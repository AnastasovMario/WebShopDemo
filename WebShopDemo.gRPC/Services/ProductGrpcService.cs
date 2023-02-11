﻿using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using WebShopDemo.Core.Contracts;
using WebShopDemo.Grpc;

namespace WebShopDemo.gRPC.Services
{
    public class ProductGrpcService : Product.ProductBase
    {
        private readonly IProductService productService;
        public ProductGrpcService(IProductService _productService)
        {
            productService = _productService;
        }

        public int DaysMissing { get; set; }

        public override async Task<ProductList> GetAll(Empty request, ServerCallContext context)
        {
            ProductList result = new ProductList();
            var products = await productService.GetAll();

            result.Items.AddRange(products.Select(p => new ProductItem()
            {
                Name = p.Name,
                Id = p.Id.ToString(),
                Price = (double)p.Price,
                Quantity = p.Quantity
            }));

            return result;
        }
    }
}
