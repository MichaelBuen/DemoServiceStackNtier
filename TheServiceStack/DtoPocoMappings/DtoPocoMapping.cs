using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ienablemuch.DitTO;
using TheEntities.Dto;
using TheEntities.Poco;

namespace TheServiceStack.DtoPocoMappings
{
    class OrderMapping : DtoPocoMap<OrderDto, Order>
    {
        public override void Mapping()
        {
            Map(x => x.CustomerName, y => y.Customer.CustomerName);
            MapKey(x => x.CustomerId, y => y.Customer.CustomerId);
            Map(x => x.Address1, y => y.Customer.Address1);
            Map(x => x.CountryName, y => y.Customer.Country.CountryName);
                        
            Map(x => x.MemberYear, y => y.OrderDate.Year);            

            MapList(x => x.OrderLines, x => x.OrderLines, z => z.Order);

            MapList(x => x.PossibleLanguages, x => x.Customer.Country.Languages);

        }
    }

    class OrderLineMapping : DtoPocoMap<OrderLineDto, OrderLine>
    {
        public override void Mapping()
        {
            MapKey(d => d.ProductoId, s => s.Product.ProductId);
            Map(d => d.ProductDescription, s => s.Product.ProductDescription);
            MapKey(d => d.FreebieId, s => s.Freebie.ProductId);

            MapList(d => d.Koments, s => s.Comments, r => r.OrderLine);
        }
    }

    class CustomerMapping : DtoPocoMap<CustomerDto, Customer>
    {
        public override void Mapping()
        {
            Map(x => x.CountryName, y => y.Country.CountryName);
        }
    }

}