using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OnlineSupermarketTuto.Views.Customer
{
    public partial class UserProfile : System.Web.UI.Page
    {
        Models.Functions Con;
        int customerId = Login.User;
        string customerName = Login.UName;

        // 在构造函数中初始化 Con 对象
        public UserProfile()
        {
            Con = new Models.Functions();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUserInfo();
                LoadOrderHistory();
            }
        }

        // 添加静态属性以获取当前客户名称
        public static string ct;

        private void LoadUserInfo()
        {
            try
            {
                // 查询用户信息，包括余额
                string query = "SELECT CustName as CustName, RegisterDate as RegisterDate, ISNULL(Balance, 0) as Balance FROM CustomerTb1 WHERE CustId = {0}";
                query = string.Format(query, customerId);
                DataTable dt = Con.GetData(query);
                ct = dt.Rows[0]["CustName"].ToString();

                if (dt.Rows.Count > 0)
                {
                    UserNameLabel.Text = dt.Rows[0]["CustName"].ToString();
                    RegisterDateLabel.Text = Convert.ToDateTime(dt.Rows[0]["RegisterDate"]).ToString("yyyy-MM-dd  HH:mm:ss");
                    BalanceLabel.Text = string.Format("{0:N2}", Convert.ToDecimal(dt.Rows[0]["Balance"]));
                }
            }
            catch (Exception ex)
            {
                // 处理异常
                Response.Write("<script>alert('加载用户信息时出错: " + ex.Message + "');</script>");
            }
        }
        protected void OrderHistoryGV_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                OrderHistoryGV.PageIndex = e.NewPageIndex;
                LoadOrderHistory(); // 重新加载数据
            }
            catch (Exception ex)
            {
                Response.Write($"<script>alert('分页错误: {ex.Message}');</script>");
            }
        }
        private void LoadOrderHistory()
        {
            try
            {
                // 查询用户订单历史
                string query = "SELECT BillId, PDate, Amount FROM BillTb1 WHERE Customer = {0} ORDER BY PDate DESC";
                query = string.Format(query, customerId);
                DataTable dt = Con.GetData(query);
 
                OrderHistoryGV.DataSource = dt;
                OrderHistoryGV.DataBind();
                // 添加分页提示
                if (dt.Rows.Count > 0)
                {
                    int startRecord = OrderHistoryGV.PageIndex * OrderHistoryGV.PageSize + 1;
                    int endRecord = Math.Min((OrderHistoryGV.PageIndex + 1) * OrderHistoryGV.PageSize, dt.Rows.Count);
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "updatePagerInfo",
                        $"document.getElementById('pageInfo').innerHTML = '显示 {startRecord}-{endRecord} 条，共 {dt.Rows.Count} 条记录';", true);
                }
            }
            catch (Exception ex)
            {
                // 处理异常
                Console.Write("<script>alert('加载订单历史时出错: " + ex.Message + "');</script>");
            }
        }

        protected void RechargeBtn_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取充值金额
                if (string.IsNullOrEmpty(RechargeAmountTb.Value))
                {
                    Response.Write("<script>alert('请输入充值金额!');</script>");
                    return;
                }

                decimal rechargeAmount = Convert.ToDecimal(RechargeAmountTb.Value);
                if (rechargeAmount <= 0)
                {
                    Response.Write("<script>alert('请输入有效的充值金额!');</script>");
                    return;
                }

                // 更新用户余额
                string query = "UPDATE CustomerTb1 SET Balance = Balance + {0} WHERE CustId = {1}";
                query = string.Format(query, rechargeAmount, customerId);
                Con.SetData(query);

                // 刷新用户信息
                LoadUserInfo();
                RechargeAmountTb.Value = "";

                Response.Write("<script>alert('充值成功!');</script>");
            }
            catch (Exception ex)
            {
                // 处理异常
                Response.Write("<script>alert('充值过程中出错: " + ex.Message + "');</script>");
            }
        }

        // 添加静态方法获取当前用户姓名
        public static string GetCurrentCustomerName()
        {
            if (string.IsNullOrEmpty(ct))
            {
                Models.Functions Con = new Models.Functions();
                int customerId = Login.User;
                string query = "SELECT CustName as CustName FROM CustomerTb1 WHERE CustId = {0}";
                query = string.Format(query, customerId);
                DataTable dt = Con.GetData(query);
                if (dt.Rows.Count > 0)
                {
                    ct = dt.Rows[0]["CustName"].ToString();
                }
            }
            return ct;
        }
    }
}