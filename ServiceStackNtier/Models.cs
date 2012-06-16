using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheEntities.Dto;

namespace ServiceStackNtier
{
    public class OrderRequest
    {
        public int OrderId { get; set; }
        public OrderDto OrderDto { get; set; }
        public byte[] RowVersion { get; set; }

        public bool IsSearch { get; set; }
        public int PageNumber { get; set; }

    }

    public class OrderRequestResponse
    {
        public OrderDto OrderDto { get; set; }
        public byte[] RowVersion { get; set; }

        public List<SearchResult> SearchResults { get; set; }
        public int TotalPage { get; set; }        

        public ServiceStack.ServiceInterface.ServiceModel.ResponseStatus ResponseStatus { get; set; } //Where Exceptions get auto-serialized
    }

    public class SearchResult
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; }
        public string OrderDescription { get; set; }
        public string CountryName { get; set; }
    }


    class CustomerRequestResponse
    {
        public CustomerDto CustomerDto { get; set; }
        public IEnumerable<CustomerDto> CustomerDtos { get; set; }


        public ServiceStack.ServiceInterface.ServiceModel.ResponseStatus ResponseStatus { get; set; } //Where Exceptions get auto-serialized
    }



    public class ProductRequestResponse
    {
        public ProductDto ProductDto { get; set; }

        public IEnumerable<ProductDto> ProductDtos { get; set; }

        public ServiceStack.ServiceInterface.ServiceModel.ResponseStatus ResponseStatus { get; set; } //Where Exceptions get auto-serialized
    }

    public class ProductRequest
    {
        public int Id { get; set; }
        public ProductDto ProductDto { get; set; }
    }


}
