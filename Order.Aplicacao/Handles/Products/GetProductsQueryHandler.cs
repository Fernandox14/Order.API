using MediatR;
using Order.Aplicacao.Dto;
using Order.Aplicacao.DTO;
using Order.Dominio.Interfaces.Repository;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Order.Aplicacao.Handles.Products;

public class GetProductsQueryHandler(IProductRepository repository) : IRequestHandler<GetProductsQuery, BaseResponseDto<IEnumerable<ProductDto>>>
{
    public async Task<BaseResponseDto<IEnumerable<ProductDto>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var response = new BaseResponseDto<IEnumerable<ProductDto>>();

        try
        {
            var products = await repository.GetAllAsync();
            response.Success = true;
            response.Data = products.Select(product => new ProductDto(product));
        }
        catch (Exception ex)
        {
            response.Errors = new List<string> { ex.Message };
            response.Success = false;
        }

        return response;
    }
}
