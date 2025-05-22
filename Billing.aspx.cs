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

        // 使用ViewState保存选中的商品ID，确保回发后仍能保持
        protected int SelectedProductId
        {
            get { return (ViewState["SelectedProductId"] != null) ? (int)ViewState["SelectedProductId"] : 0; }
            set { ViewState["SelectedProductId"] = value; }
        }

        // 在构造函数中初始化 Con 对象
        public Billing()
        {
            Con = new Models.Functions();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["SelectedProductId"] = 0; // 初始化会话变量
                ShowProducts();
                GetCategories();
                GetManufactors();
            }

            // 确保GridView的选择事件被正确处理
            if (ProductList.SelectedIndex >= 0)
            {
                HighlightSelectedRow();
            }
        }


        // 其他方法保持不变
        private void HighlightSelectedRow()
        {
            foreach (GridViewRow row in ProductList.Rows)
            {
                if (row.RowIndex == ProductList.SelectedIndex)
                {
                    row.BackColor = System.Drawing.ColorTranslator.FromHtml("#C5BBAF");
                    row.Font.Bold = true;
                }
                else
                {
                    row.BackColor = System.Drawing.ColorTranslator.FromHtml("#E3EAEB");
                    row.Font.Bold = false;
                }
            }
        }

        protected void ProductList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ProductList.PageIndex = e.NewPageIndex;
            ShowProducts();

            // 恢复选中状态
            if (SelectedProductId > 0)
            {
                SelectProductById(SelectedProductId);
            }
        }

        private void SelectProductById(int productId)
        {
            try
            {
                for (int i = 0; i < ProductList.Rows.Count; i++)
                {
                    if (Convert.ToInt32(ProductList.DataKeys[i].Value) == productId)
                    {
                        ProductList.SelectedIndex = i;
                        ProductList_SelectedIndexChanged(null, EventArgs.Empty);
                        HighlightSelectedRow();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrMsgNew.Text = "恢复选择状态时出错: " + ex.Message;
            }
        }

        private void ShowProducts()
        {
            string Query = @"SELECT P.PId, P.PName, P.PPrice, P.PQty, CU.CustName AS SellerName
                    FROM ProductTb1 P
                    INNER JOIN CustomerTb1 CU ON P.PSellerId = CU.CustId";
            ProductList.DataSource = Con.GetData(Query);
            ProductList.DataBind();

            // 设置表头文本
            if (ProductList.HeaderRow != null)
            {
                ProductList.HeaderRow.Cells[1].Text = "序号";
                ProductList.HeaderRow.Cells[2].Text = "书本名称";
                ProductList.HeaderRow.Cells[4].Text = "库存总量";
                ProductList.HeaderRow.Cells[3].Text = "书本价格";
                ProductList.HeaderRow.Cells[5].Text = "出售者";

                // 隐藏PId列
                ProductList.HeaderRow.Cells[1].Visible = false;
            }

            // 隐藏商品ID列
            foreach (GridViewRow row in ProductList.Rows)
            {
                row.Cells[1].Visible = false;
            }
        }

        protected void ProductList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ProductList.SelectedRow != null)
                {
                    // 使用DataKeys获取商品ID
                    SelectedProductId = Convert.ToInt32(ProductList.DataKeys[ProductList.SelectedIndex].Value);
                    System.Diagnostics.Debug.WriteLine($"Selected Product ID: {SelectedProductId}");

                    // 填充表单
                    PnameTb.Value = ProductList.SelectedRow.Cells[2].Text;
                    PriceTb.Value = ProductList.SelectedRow.Cells[3].Text;
                    string sellerName = ProductList.SelectedRow.Cells[5].Text;

                    // 检查是否为当前用户的商品
                    string currentCustomerName = UserProfile.GetCurrentCustomerName();
                    if (!string.IsNullOrEmpty(currentCustomerName) &&
                        sellerName.Trim().ToLower() == currentCustomerName.Trim().ToLower())
                    {
                        // 允许编辑
                        EditBtnNew.Enabled = true;
                        PNameTbNew.Value = PnameTb.Value;
                        PriceTbNew.Value = PriceTb.Value;

                        // 获取并填充其他商品信息
                        FillProductDetails(SelectedProductId);
                        ErrMsgNew.Text = "选中商品: " + PnameTb.Value;
                    }
                    else
                    {
                        // 禁用编辑
                        DisableEditControls();
                        ErrMsgNew.Text = "您只能编辑自己出售的商品。";
                    }

                    // 立即高亮显示当前选中的行
                    HighlightSelectedRow();
                }
                else
                {
                    DisableEditControls();
                    ErrMsgNew.Text = "未选择商品。";
                }
            }
            catch (Exception ex)
            {
                ErrMsgNew.Text = "选择商品时出错: " + ex.Message;
                DisableEditControls();
            }
        }

        private void FillProductDetails(int productId)
        {
            try
            {
                // 查询商品详细信息
                string query = "SELECT * FROM ProductTb1 WHERE PId = " + productId;
                DataTable dt = Con.GetData(query);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    QtyTbNew.Value = row["PQty"].ToString();
                    AuthorTbNew.Value = row["PAuthor"].ToString();

                    // 设置下拉列表
                    PManufactCbNew.SelectedValue = row["PManufact"].ToString();
                    PCatCbNew.SelectedValue = row["PCategory"].ToString();
                }
            }
            catch (Exception ex)
            {
                ErrMsgNew.Text = "加载商品详情时出错: " + ex.Message;
            }
        }

        private void DisableEditControls()
        {
            EditBtnNew.Enabled = false;
            PNameTbNew.Value = "";
            PriceTbNew.Value = "";
            QtyTbNew.Value = "";
            AuthorTbNew.Value = "";
            PManufactCbNew.SelectedIndex = -1;
            PCatCbNew.SelectedIndex = -1;
            SelectedProductId = 0;
        }

        protected void AddToCartRedirect_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(PnameTb.Value) &&
                !string.IsNullOrEmpty(QtyTb.Value))
            {
                string redirectUrl = "~/Views/Customer/ShoppingCart.aspx?" +
                    $"add=true&name={HttpUtility.UrlEncode(PnameTb.Value)}" +
                    $"&price={HttpUtility.UrlEncode(PriceTb.Value)}" +
                    $"&qty={HttpUtility.UrlEncode(QtyTb.Value)}";

                Response.Redirect(redirectUrl);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert",
                    "alert('请先选择商品！')", true);
            }
        }

        private void GetCategories()
        {
            string Query = "select * from CategoryTb1";
            PCatCbNew.DataTextField = Con.GetData(Query).Columns["CatName"].ToString();
            PCatCbNew.DataValueField = Con.GetData(Query).Columns["CatId"].ToString();
            PCatCbNew.DataSource = Con.GetData(Query);
            PCatCbNew.DataBind();
            PCatCbNew.Items.Insert(0, new ListItem("-- 选择种类 --", "-1"));
        }

        private void GetManufactors()
        {
            string Query = "select * from ManufactorTb1";
            PManufactCbNew.DataTextField = Con.GetData(Query).Columns["ManufactName"].ToString();
            PManufactCbNew.DataValueField = Con.GetData(Query).Columns["ManufactId"].ToString();
            PManufactCbNew.DataSource = Con.GetData(Query);
            PManufactCbNew.DataBind();
            PManufactCbNew.Items.Insert(0, new ListItem("-- 选择出版社 --", "-1"));
        }

        protected void SaveBtnNew_Click(object sender, EventArgs e)
        {
            try
            {
                if (PNameTbNew.Value == "" || PManufactCbNew.SelectedIndex == -1 || PCatCbNew.SelectedIndex == -1 || PriceTbNew.Value == "" || QtyTbNew.Value == "" || AuthorTbNew.Value == "")
                {
                    ErrMsgNew.Text = "信息缺失！";
                    return;
                }

                // 验证所选分类是否有效
                int selectedCategoryId;
                if (!int.TryParse(PCatCbNew.SelectedValue, out selectedCategoryId))
                {
                    ErrMsgNew.Text = "请选择有效的商品分类！";
                    return;
                }

                // 检查所选分类是否存在于 CategoryTb1 表中
                string categoryCheckQuery = "SELECT COUNT(*) FROM CategoryTb1 WHERE CatId = " + selectedCategoryId;
                DataTable categoryCheckDt = Con.GetData(categoryCheckQuery);
                if (Convert.ToInt32(categoryCheckDt.Rows[0][0]) == 0)
                {
                    ErrMsgNew.Text = "所选商品分类不存在，请重新选择！";
                    return;
                }

                // 构建 SQL 查询
                string pName = SqlEscape(PNameTbNew.Value);
                string pAuthor = SqlEscape(AuthorTbNew.Value);
                string pManufact = PManufactCbNew.SelectedValue;
                string pCategory = PCatCbNew.SelectedValue;
                int pQty = Convert.ToInt32(QtyTbNew.Value);
                int pPrice = Convert.ToInt32(PriceTbNew.Value);
                int pSellerId = customer;

                string query = string.Format(
                    "INSERT INTO ProductTb1 (PName, PAuthor, PManufact, PCategory, PQty, PPrice, PSellerId) " +
                    "VALUES ('{0}', '{1}', {2}, {3}, {4}, {5}, {6})",
                    pName, pAuthor, pManufact, pCategory, pQty, pPrice, pSellerId);

                int rowsAffected = Con.SetData(query);
                if (rowsAffected > 0)
                {
                    ShowProducts();
                    ErrMsgNew.Text = "商品信息已添加！";
                    ClearEditForm();

                    // 添加成功后刷新商品列表并高亮显示新添加的商品
                    ClientScript.RegisterStartupScript(this.GetType(), "refresh", "alert('商品添加成功！');", true);
                }
                else
                {
                    ErrMsgNew.Text = "添加商品失败，请重试。";
                }
            }
            catch (Exception ex)
            {
                ErrMsgNew.Text = "添加商品时出错: " + ex.Message;
            }
        }

        protected void EditBtnNew_Click(object sender, EventArgs e)
        {
            try
            {
                if (SelectedProductId <= 0)
                {
                    ErrMsgNew.Text = "未选择有效的商品！";
                    return;
                }

                if (PNameTbNew.Value == "" || PManufactCbNew.SelectedIndex == -1 || PCatCbNew.SelectedIndex == -1 || PriceTbNew.Value == "" || QtyTbNew.Value == "" || AuthorTbNew.Value == "")
                {
                    ErrMsgNew.Text = "信息缺失！";
                    return;
                }

                // 构建SQL查询
                string pName = SqlEscape(PNameTbNew.Value);
                string pAuthor = SqlEscape(AuthorTbNew.Value);
                string pManufact = PManufactCbNew.SelectedValue;
                string pCategory = PCatCbNew.SelectedValue;
                int pQty = Convert.ToInt32(QtyTbNew.Value);
                int pPrice = Convert.ToInt32(PriceTbNew.Value);
                int pSellerId = customer;

                string query = string.Format(
                    "UPDATE ProductTb1 " +
                    "SET PName = '{0}', PAuthor = '{1}', PManufact = {2}, " +
                    "PCategory = {3}, PQty = {4}, PPrice = {5}, PSellerId = {6} " +
                    "WHERE PId = {7}",
                    pName, pAuthor, pManufact, pCategory, pQty, pPrice, pSellerId, SelectedProductId);

                System.Diagnostics.Debug.WriteLine("Update Query: " + query);

                int rowsAffected = Con.SetData(query);

                if (rowsAffected > 0)
                {
                    ShowProducts(); // 重新绑定数据

                    // 重新选择刚刚编辑的商品
                    SelectProductById(SelectedProductId);

                    ErrMsgNew.Text = "商品信息已更新！";
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('商品信息已成功更新！');", true);
                }
                else
                {
                    ErrMsgNew.Text = "未找到要更新的商品，请刷新页面后重试。";
                }
            }
            catch (Exception ex)
            {
                ErrMsgNew.Text = "更新商品时出错: " + ex.Message;
            }
        }

        // 辅助方法：转义SQL字符串
        private string SqlEscape(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";
            return value.Replace("'", "''");
        }

        private void ClearEditForm()
        {
            PNameTbNew.Value = "";
            PriceTbNew.Value = "";
            QtyTbNew.Value = "";
            AuthorTbNew.Value = "";
            PManufactCbNew.SelectedIndex = -1;
            PCatCbNew.SelectedIndex = -1;
            SelectedProductId = 0;
        }

        protected void DeleteBtnNew_Click(object sender, EventArgs e)
        {
            try
            {
                if (SelectedProductId <= 0)
                {
                    ErrMsgNew.Text = "未选择有效的商品！";
                    return;
                }

                string query = "DELETE FROM ProductTb1 WHERE PId = " + SelectedProductId;
                int rowsAffected = Con.SetData(query);

                if (rowsAffected > 0)
                {
                    ShowProducts();
                    ErrMsgNew.Text = "商品信息已删除！";
                    ClearEditForm();
                }
                else
                {
                    ErrMsgNew.Text = "未找到要删除的商品，请刷新页面后重试。";
                }
            }
            catch (Exception ex)
            {
                ErrMsgNew.Text = "删除商品时出错: " + ex.Message;
            }
        }
    }
}