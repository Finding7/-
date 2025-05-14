using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineSupermarketTuto.Views.Customer
{
    public partial class Billing : System.Web.UI.Page
    {
        Models.Functions Con;
        int customer = Login.User;
        string CName = Login.UName;
        protected void Page_Load(object sender, EventArgs e)
        {
            Con = new Models.Functions();
            if (!IsPostBack)
            {
                ShowProducts();
                DataTable dt;
                if (Session["ShoppingCart"] == null)
                {
                    dt = new DataTable();
                    dt.Columns.AddRange(new DataColumn[5]
                    {
                        new DataColumn("序号"),
                        new DataColumn("书本名称"),
                        new DataColumn("价格"),
                        new DataColumn("数量"),
                        new DataColumn("总计"),
                    });
                }
                else
                {
                    dt = (DataTable)Session["ShoppingCart"];
                }
                ViewState["账单"] = dt;
                this.BindGrid();
            }
        }
        protected void BindGrid()
        {
            DataTable dt = (DataTable)Session["ShoppingCart"];
            ShoppingCartList.DataSource = dt;
            ShoppingCartList.DataBind();

            GrdTotal = 0;
            for (int i = 0; i < ShoppingCartList.Rows.Count; i++)
            {
                GrdTotal = GrdTotal + Convert.ToInt32(ShoppingCartList.Rows[i].Cells[4].Text);
            }
            Amount = GrdTotal;
            RMBLabel.Text = "¥";
            GrdTotalTb.Text = Convert.ToString(GrdTotal);
        }
        private void ShowProducts()
        {
            string Query = "Select PId, PName, PPrice,PQty from ProductTb1";
            ProductList.DataSource = Con.GetData(Query);
            ProductList.DataBind();
            ProductList.HeaderRow.Cells[1].Text = "序号";
            ProductList.HeaderRow.Cells[2].Text = "书本名称";
            ProductList.HeaderRow.Cells[4].Text = "库存总量";
            ProductList.HeaderRow.Cells[3].Text = "书本价格";
        }
        int key = 0;
        int stock = 0;
        protected void ProductList_SelectedIndexChanged(object sender, EventArgs e)
        {
            PnameTb.Value = ProductList.SelectedRow.Cells[2].Text;
            PriceTb.Value = ProductList.SelectedRow.Cells[3].Text;
            stock = Convert.ToInt32(ProductList.SelectedRow.Cells[4].Text);

            if (PnameTb.Value == "")
            {
                key = 0;
            }
            else
            {
                key = Convert.ToInt32(ProductList.SelectedRow.Cells[1].Text);
            }
        }

        private void UpdateStock()
        {
            int NewQty;
            NewQty = Convert.ToInt32(ProductList.SelectedRow.Cells[4].Text) - Convert.ToInt32(QtyTb.Value);
            string Query = "Update ProductTb1 set PQty={0} where PId={1}";
            Query = string.Format(Query, NewQty, ProductList.SelectedRow.Cells[1].Text);
            Con.SetData(Query);
            ShowProducts();
        }

        private void InsertBill()
        {
            string Query = "insert into BillTb1 values('{0}',{1},{2})";
            Query = string.Format(Query, DateTime.Today.Date.ToString(), customer, Convert.ToInt32(GrdTotalTb.Text));
            Con.SetData(Query);
        }

        int GrdTotal = 0;
        int Amount;
        protected void AddToBillBtn_Click(object sender, EventArgs e)
        {
            if (PnameTb.Value == "" || QtyTb.Value == "" || PriceTb.Value == "")
            {
                return;
            }
            else
            {
                int newQty = Convert.ToInt32(QtyTb.Value);
                int price = Convert.ToInt32(PriceTb.Value);
                int total = newQty * price;
                DataTable dt;
                if (Session["ShoppingCart"] == null)
                {
                    dt = new DataTable();
                    dt.Columns.AddRange(new DataColumn[5]
                    {
                        new DataColumn("序号"),
                        new DataColumn("书本名称"),
                        new DataColumn("价格"),
                        new DataColumn("数量"),
                        new DataColumn("总计"),
                    });
                }
                else
                {
                    dt = (DataTable)Session["ShoppingCart"];
                }

                // 检查购物车中是否已经存在该商品
                bool itemExists = false;
                foreach (DataRow row in dt.Rows)
                {
                    if (row["书本名称"].ToString() == PnameTb.Value.Trim())
                    {
                        int existingQty = Convert.ToInt32(row["数量"]);
                        int existingTotal = Convert.ToInt32(row["总计"]);
                        row["数量"] = existingQty + newQty;
                        row["总计"] = existingTotal + total;
                        itemExists = true;
                        break;
                    }
                }

                // 如果商品不存在，则添加新行
                if (!itemExists)
                {
                    dt.Rows.Add(ShoppingCartList.Rows.Count + 1,
                        PnameTb.Value.Trim(),
                        PriceTb.Value.Trim(),
                        QtyTb.Value.Trim(),
                        total);
                }

                Session["ShoppingCart"] = dt;

                UpdateStock();
                BindGrid();

                PnameTb.Value = "";
                QtyTb.Value = "";
                PriceTb.Value = "";
            }
        }

        protected void PrintBtn_Click(object sender, EventArgs e)
        {
            InsertBill();
        }
    }
}