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
                // 新增：检查来自Billing页面的商品添加请求
                if (Request.QueryString["add"] == "true")
                {
                    AddProductToCart(
                        Request.QueryString["name"],
                        Request.QueryString["price"],
                        Request.QueryString["qty"]);
                }

                InitializeShoppingCart();
            }
        }
        private void AddProductToCart(string name, string price, string qty)
        {
            DataTable dt = (DataTable)Session["ShoppingCart"] ?? CreateCartTable();

            // 添加商品逻辑（原Billing页面的AddToBillBtn_Click内容转移至此）
            int newQty = Convert.ToInt32(qty);
            int total = newQty * Convert.ToInt32(price);

            // 检查商品是否存在
            DataRow[] existingRows = dt.Select($"书本名称 = '{name}'");
            if (existingRows.Length > 0)
            {
                existingRows[0]["数量"] = Convert.ToInt32(existingRows[0]["数量"]) + newQty;
                existingRows[0]["总计"] = Convert.ToInt32(existingRows[0]["总计"]) + total;
            }
            else
            {
                dt.Rows.Add(dt.Rows.Count + 1, name, price, qty, total);
            }

            Session["ShoppingCart"] = dt;
        }

        private void InitializeShoppingCart()
        {
            DataTable dt = (DataTable)Session["ShoppingCart"] ?? CreateCartTable();
            ViewState["账单"] = dt;
            BindGrid();
        }

        private DataTable CreateCartTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[5] {
                new DataColumn("序号"),
                new DataColumn("书本名称"),
                new DataColumn("价格"),
                new DataColumn("数量"),
                new DataColumn("总计"),
            });
            return dt;
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
            // +++ 新增校验逻辑 +++
            if (ViewState["账单"] == null || ((DataTable)ViewState["账单"]).Rows.Count == 0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert",
                    "alert('购物车为空，请先添加商品！');", true);
                return;
            }
            // 1. 首先插入订单信息
            string Query = "insert into BillTb1 values('{0}',{1},{2})";
            Query = string.Format(Query, DateTime.Today.Date.ToString(), customer, Convert.ToInt32(GrdTotalTb.Text));
            Con.SetData(Query);

            // 2. 获取总金额并验证买家余额
            decimal totalAmount = Convert.ToDecimal(GrdTotalTb.Text);
            DataTable buyerDt = Con.GetData($"SELECT Balance FROM CustomerTb1 WHERE CustId = {customer}");
            decimal buyerBalance = Convert.ToDecimal(buyerDt.Rows[0]["Balance"]);

            if (buyerBalance < totalAmount)
            {
                throw new Exception("余额不足，无法完成支付！");
            }

            // 3. 扣除买家余额
            Con.SetData($"UPDATE CustomerTb1 SET Balance = Balance - {totalAmount} WHERE CustId = {customer}");

            // 2. 更新库存
            if (ViewState["账单"] != null)
            {
                DataTable dt = (DataTable)ViewState["账单"];
                foreach (DataRow row in dt.Rows)
                {
                    string productName = row["书本名称"].ToString();
                    int quantity = Convert.ToInt32(row["数量"]);

                    // 获取商品ID和当前库存
                    string getProductQuery = "SELECT PId, PQty, PSellerId FROM ProductTb1 WHERE PName = '{0}'";
                    getProductQuery = string.Format(getProductQuery, productName);
                    DataTable productDt = Con.GetData(getProductQuery);

                    if (productDt.Rows.Count == 0)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('商品 {productName} 不存在！');", true);
                        return;
                    }

                    int pId = Convert.ToInt32(productDt.Rows[0]["PId"]);
                    int currentQty = Convert.ToInt32(productDt.Rows[0]["PQty"]);
                    int sellerId = Convert.ToInt32(productDt.Rows[0]["PSellerId"]);

                    if (currentQty < quantity)
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", $"alert('商品 {productName} 库存不足！当前库存：{currentQty}');", true);
                        return;
                    }

                    // 更新库存
                    int newQty = currentQty - quantity;
                    string updateStockQuery = "UPDATE ProductTb1 SET PQty = {0} WHERE PId = {1}";
                    updateStockQuery = string.Format(updateStockQuery, newQty, pId);
                    Con.SetData(updateStockQuery);

                    Con.SetData($"UPDATE CustomerTb1 SET Balance = Balance + {totalAmount} WHERE CustId = {sellerId}");
                }
            }

            // 3. 获取购物车中数量最多的商品类别并更新顾客喜好
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
            // 4. 清空购物车
            Session["ShoppingCart"] = null;
            BindGrid();
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('结算成功！');", true);
        }

        protected void ShoppingCartList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DataTable dt = (DataTable)ViewState["账单"];
            dt.Rows.RemoveAt(e.RowIndex);
            ViewState["账单"] = dt;
            Session["ShoppingCart"] = dt;
            BindGrid();
        }
    }
}