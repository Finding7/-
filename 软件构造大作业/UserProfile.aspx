<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Customer/CustomerMaster.Master" AutoEventWireup="true" CodeBehind="UserProfile.aspx.cs" Inherits="OnlineSupermarketTuto.Views.Customer.UserProfile" CodePage="65001" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MyContent" runat="server">
    <div class="container-fluid">
        <!-- 用户信息部分 -->
        <div class="row mb-4">
            <div class="col-md-12">
                <div class="card shadow">
                    <div class="card-header bg-primary text-white">
                        <h4 class="mb-0">个人信息</h4>
                    </div>
                    <div class="card-body">
                        <div class="row">
                            <!-- 左侧用户基本信息 -->
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <label class="form-label text-primary">用户名</label>
                                    <asp:Label ID="UserNameLabel" runat="server" CssClass="form-control"></asp:Label>
                                </div>
                                <div class="mb-3">
                                    <label class="form-label text-primary">注册时间</label>
                                    <asp:Label ID="RegisterDateLabel" runat="server" CssClass="form-control"></asp:Label>
                                </div>
                            </div>
                            
                            <!-- 右侧余额信息 -->
                            <div class="col-md-6">
                                <div class="mb-3">
                                    <label class="form-label text-primary">账户余额</label>
                                    <div class="input-group">
                                        <span class="input-group-text">¥</span>
                                        <asp:Label ID="BalanceLabel" runat="server" CssClass="form-control"></asp:Label>
                                    </div>
                                </div>
                                
                                <!-- 充值功能 -->
                                <div class="mb-3">
                                    <label class="form-label text-primary">充值金额</label>
                                    <div class="input-group">
                                        <span class="input-group-text">¥</span>
                                        <input type="number" id="RechargeAmountTb" runat="server" class="form-control" placeholder="输入充值金额" min="1" />
                                        <asp:Button ID="RechargeBtn" runat="server" Text="充值" CssClass="btn btn-success" OnClick="RechargeBtn_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        
        <!-- 订单历史记录部分 -->
         <div class="row">
            <div class="col-md-12">
                <div class="card shadow">
                    <div class="card-header bg-info text-white">
                        <h4 class="mb-0">订单记录</h4>
                    </div>
                    <div class="card-body">
                        <div class="table-responsive">
                            <asp:GridView ID="OrderHistoryGV" runat="server" CssClass="table table-striped table-hover" 
                                AutoGenerateColumns="false" AllowPaging="true" PageSize="8"  OnPageIndexChanging="OrderHistoryGV_PageIndexChanging"  PagerSettings-Mode="NumericFirstLast"  PagerStyle-CssClass="pagination" CellPadding="4" ForeColor="#333333" GridLines="None">
                                <AlternatingRowStyle BackColor="White" />
                                <EditRowStyle BackColor="#7C6F57" />
                                <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#1C5E55" Font-Bold="true" ForeColor="White" />
                                <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#E3EAEB" />
                                <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                                <SortedAscendingCellStyle BackColor="#F8FAFA" />
                                <SortedAscendingHeaderStyle BackColor="#246B61" />
                                <SortedDescendingCellStyle BackColor="#D4DFE1" />
                                <SortedDescendingHeaderStyle BackColor="#15524A" />
                                <Columns>
                                    <asp:BoundField DataField="BillId" HeaderText="订单号" />
                                    <asp:BoundField DataField="PDate" HeaderText="订单日期" DataFormatString="{0:yyyy-MM-dd}" />
                                    <asp:BoundField DataField="Amount" HeaderText="订单金额" DataFormatString="¥{0:N2}" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        
    </div>

       
    <!-- 添加Bootstrap和jQuery脚本 -->
    <script src="../../Assets/Lib/js/bootstrap.bundle.min.js"></script>
</asp:Content>