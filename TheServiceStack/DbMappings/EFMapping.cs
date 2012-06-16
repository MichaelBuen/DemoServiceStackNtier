using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using TheEntities.Poco;

namespace TheServiceStack.DbMappings
{
    public class EfDbMapper : DbContext
    {
        public EfDbMapper(string connectionString) : base(connectionString) 
        {
            this.Configuration.ProxyCreationEnabled = true;
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            
            
            modelBuilder.Entity<Country>().HasMany(x => x.Languages).WithMany(x => x.Countries)
                .Map(m =>
                {
                    m.ToTable("LanguageAssocCountry");
                    m.MapLeftKey("AssocCountryId");
                    m.MapRightKey("AssocLanguageId");
                });



            modelBuilder.Entity<Order>().HasMany(x => x.OrderLines).WithRequired(y => y.Order).Map(x => x.MapKey("Order_OrderId"));
            modelBuilder.Entity<Order>().Property(x => x.RowVersion).IsRowVersion();
            

            modelBuilder.Entity<OrderLine>().HasMany(x => x.Comments).WithRequired(y => y.OrderLine).Map(x => x.MapKey("OrderLine_OrderLineId"));
            
            

        }

    }
}