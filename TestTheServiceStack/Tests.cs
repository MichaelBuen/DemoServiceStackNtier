using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheServiceStack.DbMappings;
using TheEntities.Poco;
using System.Reflection;


using Ienablemuch.DitTO.ToTheEfnhX.StubAssigner;
using Ienablemuch.DitTO;
using TheEntities.Dto;

namespace TestTheServiceStack
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        [ExpectedException(typeof(System.Data.Entity.Infrastructure.DbUpdateException))]
        public void TestNullOnEF()
        {

            string s = "";
            Assert.IsTrue(s.GetType().IsClass);

            var db = new EfDbMapper("Data Source=localhost; Initial Catalog=ServiceStackNtierDemo; User id=sa; Password=P@$$w0rd");

            Order o = db.Set<Order>().Single(x => x.OrderId == 1);

            // what a work-around! 
            // http://www.codetuning.net/blog/post/Understanding-Entity-Framework-Associations.aspx


            var dummy = o.Customer;
            o.Customer = null;

            db.SaveChanges();                       
        }

        [TestMethod]
        [ExpectedException(typeof(System.Data.Entity.Infrastructure.DbUpdateException))]
        public void Test_null_on_EF_Repo()
        {
            var db = new EfDbMapper("Data Source=localhost; Initial Catalog=ServiceStackNtierDemo; User id=sa; Password=P@$$w0rd");

            var repo = new Ienablemuch.ToTheEfnhX.EntityFramework.Repository<Order>(db);

            // Order o = db.Set<Order>().Single(x => x.OrderId == 1);
            Order o = repo.GetCascade(1);

            // what a work-around! 
            // http://www.codetuning.net/blog/post/Understanding-Entity-Framework-Associations.aspx


            // var dummy = o.Customer;

            // we fix the null-assign problem on Get. we dummy-read all the possible Independent Associations, i.e. all reference types
            o.Customer = null;
           
            Assert.IsNull(o.Customer); // without the: var dummy = o.Customer, IsNull fails


            repo.Save(o);
        }

        [TestMethod]
        [ExpectedException(typeof(System.Data.Entity.Infrastructure.DbUpdateException))]
        public void Test_null_on_EF_Repo_via_dto()
        {
            var db = new EfDbMapper("Data Source=localhost; Initial Catalog=ServiceStackNtierDemo; User id=sa; Password=P@$$w0rd");

            var repo = new Ienablemuch.ToTheEfnhX.EntityFramework.Repository<Order>(db);

            // Order o = db.Set<Order>().Single(x => x.OrderId == 1);
            Order o = repo.Get(1);

            var dto = Mapper.ToDto<Order, OrderDto>(o);
            
            // what a work-around! 
            // http://www.codetuning.net/blog/post/Understanding-Entity-Framework-Associations.aspx

            dto.CustomerId = 0;

            var newPoco = Mapper.ToPoco<OrderDto, Order>(dto);
            repo.AssignStub(newPoco);

            repo.Save(newPoco);
        }

        [TestMethod]
        public void Test_Delete()
        {
            var db = new EfDbMapper("Data Source=localhost; Initial Catalog=ServiceStackNtierDemo; User id=sa; Password=P@$$w0rd");

            var repo = new Ienablemuch.ToTheEfnhX.EntityFramework.Repository<Order>(db);

            // Order o = db.Set<Order>().Single(x => x.OrderId == 1);
            Order o = repo.Get(1);

            repo.DeleteCascade(5, o.RowVersion);
        }
    }


}
