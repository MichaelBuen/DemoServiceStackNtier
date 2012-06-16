using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheEntities.Dto;
using TheEntities.Poco;

namespace TheServiceStack.App_Start
{
    public class CustomerRequest
    {
        public int Id { get; set; }
        public CustomerDto CustomerDto { get; set; }
    }
    
    class CustomerRequestResponse
    {
        public CustomerDto CustomerDto { get; set; }
        public IEnumerable<CustomerDto> CustomerDtos { get; set; }

        public ServiceStack.ServiceInterface.ServiceModel.ResponseStatus ResponseStatus { get; set; } //Where Exceptions get auto-serialized
    }

    class CustomerRequestService : ServiceStack.ServiceInterface.RestServiceBase<CustomerRequest>
    {

        Ienablemuch.ToTheEfnhX.IRepository<Customer> Repository { get; set; }
        public CustomerRequestService(Ienablemuch.ToTheEfnhX.IRepository<Customer> x)
        {
            Repository = x;
        }

        public override object OnGet(CustomerRequest request)
        {
            if (request.Id != 0)
            {
                return new CustomerRequestResponse
                {
                    CustomerDto = Ienablemuch.DitTO.Mapper.ToDto<Customer, CustomerDto>(Repository.Get(request.Id))
                };
            }
            else
            {
                if (request.CustomerDto == null)
                    return new CustomerRequestResponse
                    {
                        CustomerDtos = Repository.All.ToList().Select(x => Ienablemuch.DitTO.Mapper.ToDto<Customer, CustomerDto>(x))
                    };
                else
                {
                    IQueryable<Customer> qry = Repository.All;

                    if (request.CustomerDto.CountryName != null)
                        qry = qry.Where(x => x.Country.CountryName.StartsWith(request.CustomerDto.CountryName));
                    
                    if (request.CustomerDto.CustomerName != null)
                        qry = qry.Where(x => x.CustomerName.StartsWith(request.CustomerDto.CustomerName));

                    if (request.CustomerDto.MemberYear != 0)
                        qry = qry.Where(x => x.MemberYear == request.CustomerDto.MemberYear);

                    if (request.CustomerDto.Address1 != null)
                        qry = qry.Where(x => x.Address1.StartsWith(request.CustomerDto.Address1));

                    return new CustomerRequestResponse
                    {
                        CustomerDtos = qry.ToList().Select(x => Ienablemuch.DitTO.Mapper.ToDto<Customer, CustomerDto>(x))
                    };
                }
            }            
        }
    }
}