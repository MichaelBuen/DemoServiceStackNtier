using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TheEntities.Dto
{
    // Why DTO? https://github.com/ServiceStack/ServiceStack/wiki/Why-Servicestack
    public class OrderDto
    {
        public int OrderId { get; set; }


        public int CustomerId { get; set; }

        public List<LanguageDto> PossibleLanguages { get; set; }


        public string CustomerName { get; set; }



        public string Address1 { get; set; }


        public int MemberYear { get; set; }

        public string CountryName { get; set; }

        public string OrderDescription { get; set; }
        public DateTime? OrderDate { get; set; }


        public List<OrderLineDto> OrderLines { get; set; }

        public byte[] RowVersion { get; set; }
    }

    public class LanguageDto
    {
        public int LanguageId { get; set; }
        public string LanguageName { get; set; }
    }

    public class OrderLineDto
    {
        public int OrderLineId { get; set; }


        public int ProductoId { get; set; }
        public string ProductDescription { get; set; }

        public int FreebieId { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }


        public List<CommentDto> Koments { get; set; }
    }


    public class CommentDto
    {
        public int CommentId { get; set; }
        public string TheComment { get; set; }
    }


    public class ProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription{ get; set; }
    }

    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CountryName { get; set; }
        public string Address1 { get; set; }
        public int MemberYear { get; set; }
    }


}