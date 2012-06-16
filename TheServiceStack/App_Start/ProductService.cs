using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheEntities.Dto;
using TheEntities.Poco;

namespace TheServiceStack.App_Start
{
    public class ProductRequest
    {
        public int Id { get; set; }
        public ProductDto ProductDto { get; set; }
    }

    public class ProductRequestResponse
    {
        public ProductDto ProductDto { get; set; }        

        public IEnumerable<ProductDto> ProductDtos { get; set; }

        public ServiceStack.ServiceInterface.ServiceModel.ResponseStatus ResponseStatus { get; set; } //Where Exceptions get auto-serialized
    }

    class ProductRequestService : ServiceStack.ServiceInterface.RestServiceBase<ProductRequest>
    {
        Ienablemuch.ToTheEfnhX.IRepository<Product> Repository { get; set; }
        public ProductRequestService(Ienablemuch.ToTheEfnhX.IRepository<TheEntities.Poco.Product> x)
        {
            Repository = x;
        }

        public override object OnGet(ProductRequest request)
        {
            if (request.Id != 0)
            {
                return new ProductRequestResponse
                {
                    ProductDto = Ienablemuch.DitTO.Mapper.ToDto<Product,ProductDto>(Repository.Get(request.Id))
                };
            }
            else
            {
                // http://localhost:6428/api/product_request?ProductDto={ProductName:Michael}
                // if (request.ProductDto != null) throw new Exception(request.ProductDto.ProductName);

                return new ProductRequestResponse
                {
                    ProductDtos = Repository.All.ToList().Select(x => Ienablemuch.DitTO.Mapper.ToDto<Product,ProductDto>(x))
                };
            }

        
        }

        public override object OnDelete(ProductRequest request)
        {
            throw new Exception("Id: " + request.Id.ToString());
        }
    }
}