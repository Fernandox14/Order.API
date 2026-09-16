using MediatR;
using Order.Aplicacao.Dto;
using Order.Aplicacao.DTO;

namespace Order.Aplicacao.Handles.Products;

public class GetProductsQuery : IRequest<BaseResponseDto<IEnumerable<ProductDto>>>
{
}