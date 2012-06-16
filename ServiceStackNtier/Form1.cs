using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TheEntities.Dto;

namespace ServiceStackNtier
{
    public partial class Form1 : Form
    {
        

        // http://localhost:6428/api
        string address = "http://localhost/TheServiceSparta/api/";
        // string address = "http://localhost:6428/api/";

        ServiceStack.ServiceClient.Web.JsonServiceClient Client = new ServiceStack.ServiceClient.Web.JsonServiceClient();

        public Form1()
        {
            InitializeComponent();

            grdResults.AutoGenerateColumns = false;


            

            WireEvents();

            

            bdsOrder.DataSource = GetOrderTemplate();
        }

        private void WireEvents()
        {
            uxPageNumber.Value = 1;

            {
                CustomerRequestResponse r = Client.Get<CustomerRequestResponse>(address + "customer_request");
                bdsCustomer.DataSource = new[] { new CustomerDto { CustomerId = 0, CustomerName = "--SELECT Customer--" } }.Union(r.CustomerDtos);
            }


            {
                ProductRequestResponse r = Client.Get<ProductRequestResponse>(address + "product_request");
                IEnumerable<ProductDto> products = new[] { new ProductDto { ProductId = 0, ProductName = "--SELECT Product--" } }.Union(r.ProductDtos);
                bdsProduct.DataSource = products;
                bdsProduct.PositionChanged += (s, e) =>
                {
                    OrderLineDto ol = (OrderLineDto)bdsOrderLine.Current;

                    ProductDto px = (ProductDto)bdsProduct.Current;


                    ol.ProductoId = px.ProductId;
                    ol.ProductDescription = px.ProductDescription;

                    bdsOrderLine.EndEdit();
                    grd.Refresh();
                };

                bdsOrder.CurrentItemChanged += (s, e) =>
                    {
                        OrderDto x = (OrderDto)bdsOrder.Current;
                        uxOrderEntryTab.Text = "Order # " + x.OrderId;
                    };

                bdsFreebie.DataSource = products;
            }

        }

        


        bool isLaunched = false;
        private void Form1_Activated(object sender, EventArgs e)
        {
            if (isLaunched) return;

            //bdsOrder.DataSource = new OrderDto();

            isLaunched = true;
        }



        private void bdsOrderLine_CurrentChanged(object sender, EventArgs e)
        {
            var ol = (OrderLineDto)bdsOrderLine.Current;

            uxComment.Enabled = ol != null;

            if (ol == null)
            {
                
            }
            else
            {                
                if (ol.Koments == null)
                {
                    ol.Koments = new List<CommentDto>();                   
                }
            }

        }

        private void uxSave_Click(object sender, EventArgs e)
        {

            OrderRequestResponse r = null;
            try
            {
                OrderDto dto = (OrderDto)bdsOrder.Current;                               
                r = Client.Post<OrderRequestResponse>(address + "/order_request", new OrderRequest { OrderDto = dto });

                dto.OrderId = r.OrderDto.OrderId;
                dto.RowVersion = r.OrderDto.RowVersion;

                bdsOrder.ResetItem(0);
                MessageBox.Show("Saved.");

            }            
            catch (Exception ex)
            {
                if (r != null)
                {
                    MessageBox.Show("Hei");
                    MessageBox.Show(r.ResponseStatus.StackTrace);
                }
                MessageBox.Show(ex.Message);
            }
        }

        private void uxOpen_Click(object sender, EventArgs e)
        {
            try
            {

                int id = int.Parse(uxLoadId.Text);

                OpenOrder(id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OpenOrder(int id)
        {
            //var r = Client.Send<OrderRequestResponse>("GET", address + "order_request/" + int.Parse(uxLoadId.Text), null);
            //var r = Client.Send<OrderRequestResponse>("GET", address + "order_request", new OrderRequest { Id = int.Parse(uxLoadId.Text) });

            var r = Client.Get<OrderRequestResponse>(address + "order_request/" + id);
            bdsOrder.DataSource = r.OrderDto;
        }

        private void uxNew_Click(object sender, EventArgs e)
        {
            bdsOrder.DataSource = GetOrderTemplate();
        }


        OrderDto GetOrderTemplate()
        {
            return new OrderDto { OrderLines = new List<OrderLineDto>() };
        }

        private void uxDelete_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Delete?", this.Text, MessageBoxButtons.YesNo) != System.Windows.Forms.DialogResult.Yes)
            {
                return;
            }


            try
            {
                OrderDto dto = (OrderDto)bdsOrder.Current;
                string x = Uri.EscapeDataString(Convert.ToBase64String(dto.RowVersion));
                OrderRequestResponse r =
                     Client.Delete<OrderRequestResponse>(address + "order_request/" + dto.OrderId.ToString() + "/" + x);
                bdsOrder.DataSource = GetOrderTemplate();
                MessageBox.Show("Deleted");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }



        private void uxTheTab_Selected(object sender, TabControlEventArgs e)
        {

            
            if (e.TabPage == uxOrderSearchTab)
            {
                uxPageNumber.Value = 1;
                DoSearch();
            }            
        }

        void DoSearch()
        {
            OrderDto o = (OrderDto)bdsOrder.Current;
            if (o.OrderId != 0)
            {
                MessageBox.Show("Can not use search if the input is from an existing record.\n\nClick New", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                uxTheTab.SelectedTab = uxOrderEntryTab;
            }


            OrderDto dto = (OrderDto)bdsOrder.Current;
            int pageNumber = (int)uxPageNumber.Value;
            var r = Client.Post<OrderRequestResponse>(address + "/order_request", new OrderRequest { OrderDto = dto, IsSearch = true, PageNumber = pageNumber });

            bdsSearchResults.DataSource = r;

            uxPageCount.Text = string.Format("of {0}", r.TotalPage);
            uxPageCount.Tag = r.TotalPage;

        }

        private void grdResults_DoubleClick(object sender, EventArgs e)
        {
            SearchResult r = (SearchResult)bdsSearchResults.Current;

            uxTheTab.SelectedTab = uxOrderEntryTab;
            OpenOrder(r.OrderId);



        }

        private void uxNext_Click(object sender, EventArgs e)
        {
            uxPageNumber.UpButton();
            DoSearch();
        }

        private void uxPrev_Click(object sender, EventArgs e)
        {
            uxPageNumber.DownButton();
            DoSearch();
        }

        private void uxFirst_Click(object sender, EventArgs e)
        {
            uxPageNumber.Value = 1;
            DoSearch();
        }

        private void uxLast_Click(object sender, EventArgs e)
        {
            uxPageNumber.Value = (int)uxPageCount.Tag;
            DoSearch();
        }

        private void uxPageNumber_Validated(object sender, EventArgs e)
        {
            DoSearch();
        }


        

    }//Form1


}


