using System.Linq.Expressions;
using AutoMapper;
using Ecommerce.Application.Features.Products.Queries.Vms;
using Ecommerce.Application.Persistence;
using Ecommerce.Domain;
using MediatR;

namespace Ecommerce.Application.Features.Products.Queries.GetProductById;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductVm>
{
    private IUnitOfWork unitOfWork;
    private IMapper mapper;

    public GetProductByIdQueryHandler(IMapper mapper, IUnitOfWork unitOfWork)
    {
        this.mapper = mapper;
        this.unitOfWork = unitOfWork;
    }

    public async Task<ProductVm> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var includes = new List<Expression<Func<Product, object>>>();
        includes.Add(p => p.Images!);
        includes.Add(p => p.Reviews!.OrderByDescending(x => x.CreatedDate));

        var product = await this.unitOfWork.Repository<Product>().GetEntityAsync(
            x => x.Id == request.ProductId,
            includes,
            true
        );

        return this.mapper.Map<ProductVm>(product);

    }
}