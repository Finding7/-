using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineSupermarketTuto.Views.Customer
{
    public partial class ShoppingCart : System.Web.UI.Page
    {
        Models.Functions Con;
        int customer = Login.User;
        string CName = Login.UName;
        protected void Page_Load(object sender, EventArgs e)
        {
            Con = new Models.Functions();
            if (!IsPostBack)
            {
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
            ShoppingCartList.DataSource = ViewState["账单"];
            ShoppingCartList.DataBind();

            // 计算总计价格
            int GrdTotal = 0;
            DataTable dt = (DataTable)ViewState["账单"];
            foreach (DataRow row in dt.Rows)
            {
                GrdTotal += Convert.ToInt32(row["总计"]);
            }
            GrdTotalTb.Text = GrdTotal.ToString();
        }

        int Amount;
        protected void PrintBtn_Click(object sender, EventArgs e)
        {
            InsertBill();
        }

        private void InsertBill()
        {
            // 1. 首先插入订单信息
            string Query = "insert into BillTb1 values('{0}',{1},{2})";
            Query = string.Format(Query, DateTime.Today.Date.ToString(), customer, Convert.ToInt32(GrdTotalTb.Text));
            Con.SetData(Query);

            // 2. 获取购物车中数量最多的商品类别并更新顾客喜好
            if (ViewState["账单"] != null)
            {
                DataTable dt = (DataTable)ViewState["账单"];
                if (dt.Rows.Count > 0)
                {
                    // 创建一个字典来统计每个类别的总数量
                    Dictionary<int, int> categoryQuantities = new Dictionary<int, int>();

                    foreach (DataRow row in dt.Rows)
                    {
                        string productName = row["书本名称"].ToString();
                        int quantity = Convert.ToInt32(row["数量"]);

                        // 获取当前商品的类别
                        string getCategoryQuery = "SELECT PCategory FROM ProductTb1 WHERE PName = '{0}'";
                        getCategoryQuery = string.Format(getCategoryQuery, productName);
                        DataTable categoryDt = Con.GetData(getCategoryQuery);

                        if (categoryDt.Rows.Count > 0)
                        {
                            int categoryId = Convert.ToInt32(categoryDt.Rows[0]["PCategory"]);

                            // 更新类别数量统计
                            if (categoryQuantities.ContainsKey(categoryId))
                            {
                                categoryQuantities[categoryId] += quantity;
                            }
                            else
                            {
                                categoryQuantities.Add(categoryId, quantity);
                            }
                        }
                    }

                    // 找出数量最多的类别
                    if (categoryQuantities.Count > 0)
                    {
                        int favoriteCategory = categoryQuantities.OrderByDescending(x => x.Value).First().Key;

                        // 更新顾客喜好
                        string updateFavoriteQuery = "UPDATE CustomerTb1 SET CustFavorite = {0} WHERE CustId = {1}";
                        updateFavoriteQuery = string.Format(updateFavoriteQuery, favoriteCategory, customer);
                        Con.SetData(updateFavoriteQuery);
                    }
                }
            }
        }
    }
}