using Grpc.Core;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities;
using ProductService.Grpc;

namespace ProductService.API.Services;

public class ProductGrpcService : ProductGrpc.ProductGrpcBase
{
    private readonly IProductRepository _repository;

    public ProductGrpcService(IProductRepository repository)
    {
        _repository = repository;
    }

    public override async Task<ProductReply> GetProductById(ProductRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.Id, out var guid))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid GUID format"));
        }

        var product = await _repository.GetByIdAsync(guid, context.CancellationToken);

        if (product == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Product not found"));
        }

        return MapToReply(product);
    }

    private ProductReply MapToReply(Product product)
    {
        return new ProductReply
        {
            Id = product.Id.ToString(),
            Name = product.Name,
            Price = (double)product.Price,
            Stock = product.StockQuantity
        };
    }
}