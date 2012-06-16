using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheEntities.Dto;

// using Ienablemuch.DitTO;

using System.Configuration;
using TheEntities.Poco;


using Ienablemuch.DitTO.ToTheEfnhX.StubAssigner;



namespace TheServiceStack.App_Start
{
    public class OrderRequest
    {
        public int Id { get; set; }
        public byte[] RowVersion { get; set; }
        public OrderDto OrderDto { get; set; } // other request filter goes here
        
        public bool IsSearch { get; set; }
        public int PageNumber { get; set; }

    }

    public class OrderRequestResponse
    {
        public OrderDto OrderDto { get; set; }        

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

    public class OrderRequestService : ServiceStack.ServiceInterface.RestServiceBase<OrderRequest>
    {

        public Ienablemuch.ToTheEfnhX.IRepository<Order> Repository { get; set; }
        

        public override object OnGet(OrderRequest request)
        {
            
            if (request.Id != 0)
            {
                Order poco = Repository.Get(request.Id);
                OrderDto dto = Ienablemuch.DitTO.Mapper.ToDto<Order, OrderDto>(poco);

                return new OrderRequestResponse
                {
                    OrderDto = dto
                };
            }
            else
                return null;
        }

        public override object OnPost(OrderRequest request)
        {
            if (request.IsSearch)
            {
                return Search(request);

            }
            else // Save
            {
                return Save(request);
            }
        }

        OrderRequestResponse Save(OrderRequest request)
        {
            Order poco = Ienablemuch.DitTO.Mapper.ToPoco<OrderDto, Order>(request.OrderDto);
            // NHibernate don't have problem with stub references. Entity Framework stub object needed be wired to an object already in identity maps
            // http://en.wikipedia.org/wiki/Identity_map_pattern            
            Repository.AssignStub(poco);

            Repository.SaveGraph(poco);
            return new OrderRequestResponse { OrderDto = new OrderDto { OrderId = poco.OrderId, RowVersion = poco.RowVersion } };
        }


        OrderRequestResponse Search(OrderRequest request)
        {
            var query = Repository.All.Where(x =>
                    (request.OrderDto.CustomerId == 0 || x.Customer.CustomerId == request.OrderDto.CustomerId)
                    &&
                    (request.OrderDto.OrderDate == null || x.OrderDate >= request.OrderDto.OrderDate)
                    &&
                    (string.IsNullOrEmpty(request.OrderDto.OrderDescription) || x.OrderDescription.Contains(request.OrderDto.OrderDescription))
                );


            if (request.OrderDto.OrderLines.Any())
            {
                foreach (OrderLineDto o in request.OrderDto.OrderLines)
                {

                    var z = o;
                    query = query.Where(x => x.OrderLines.Any(y =>

                            (z.ProductoId == 0 || y.Product.ProductId == z.ProductoId)

                            &&

                            (z.Quantity == 0 || y.Quantity == z.Quantity)
                        ));


                }
            }


            int rowsPerPage = 10;

            if (request.PageNumber == 0) ++request.PageNumber;
            

            return new OrderRequestResponse
            {
                SearchResults =
                    query
                    .OrderBy(o => o.OrderId)
                    .Skip((request.PageNumber-1) * rowsPerPage)
                    .Take(rowsPerPage)
                    .ToList()
                    .Select(x => new SearchResult
                    {
                        OrderId = x.OrderId,
                        OrderDate = x.OrderDate,
                        CustomerName = x.Customer.CustomerName,
                        CountryName = (x.Customer.Country ?? new Country()).CountryName,
                        OrderDescription = x.OrderDescription
                    }).ToList()

                ,TotalPage = (int) Math.Ceiling((decimal)query.Count() / rowsPerPage),


            };
        }




        public override object OnDelete(OrderRequest request)
        {
            
            // throw new Exception((request.OrderDto != null) + " " + (request.OrderDto != null ? request.OrderDto.OrderId : -1));

            // throw new Exception("ID " + request.Id.ToString() + "x" + Convert.ToBase64String(request.RowVersion));
            // Repository.DeleteCascade(request.OrderDto.OrderId, request.OrderDto.RowVersion);
            Repository.DeleteCascade(request.Id, request.RowVersion);
            
            return new OrderRequestResponse();
        }


    }
}