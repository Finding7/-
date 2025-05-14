<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Customer/CustomerMaster.Master" AutoEventWireup="true" CodeBehind="Billing.aspx.cs" Inherits="OnlineSupermarketTuto.Views.Customer.Billing" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        /* 打印账单功能 */
        function PrintBill() {
            // 获取购物车列表元素
            var PGrid = document.getElementById('<%=ShoppingCartList.ClientID%>');
            PGrid.bordr = 0;
            // 打开新窗口用于打印
            var PWin = window.open('', 'PrintGrid', 'left=100, top=100, width=1024, height=768, tollbar=0, scrollbars = 1, status=0, resizable=1');
            // 检查弹窗是否被阻止
            if (!PWin || PWin.closed) {
                alert("请允许弹窗以完成打印操作！");
                return;
            }
            // 写入内容并触发打印
            PWin.document.write(PGrid.outerHTML);
            PWin.document.close();
            PWin.onload = function () {
                PWin.focus();
                PWin.print();
                PWin.close();
            };
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MyContent" runat="server">
    <!-- 主容器 开始 -->
    <div class="container-fluid">
        <!-- 顶部间隔 -->
        <div class="row mb-2"></div>
        
        <!-- 主要内容区域 开始 -->
        <div class="row">
            <!-- 左侧面板: 书本选择区域 -->
            <div class="col-md-5">
                <!-- 标题区 -->
                <div class="bg-light p-2 rounded">
                    <h3 class="text-center" style="color:#008B8B;">书本选购区</h3>
                </div>
                
                <!-- 输入表单区域 -->
                <div class="row shadow-sm p-2 mb-3 bg-white rounded">
                    <!-- 书本名称输入 -->
                    <div class="col">
                        <div class="mt-3">
                          <label for="" class="form-label text-success fw-bold">书本名称</label>
                          <input type="text" placeholder="" autocomplete="off" class="form-control border-success" runat="server" id="PnameTb" />
                        </div>
                    </div>
                    
                    <!-- 价格输入 -->
                    <div class="col">
                        <div class="mt-3">
                          <label for="" class="form-label text-success fw-bold">价格</label>
                          <input type="text" placeholder="" autocomplete="off" class="form-control border-success" runat="server" id="PriceTb" />
                        </div>
                    </div>
                    
                    <!-- 数量输入 -->
                    <div class="col">
                        <div class="mt-3">
                          <label for="" class="form-label text-success fw-bold">数量</label>
                          <input type="text" placeholder="" autocomplete="off" class="form-control border-success" runat="server" id="QtyTb" />
                        </div>
                    </div>
                    
                    <!-- 添加按钮 -->
                    <div class="row mt-3 mb-3">
                        <div class="col d-grid">
                            <asp:Button Text="add to cart" runat="server" id="AddToBillBtn" class="btn-warning btn-block btn shadow" OnClick="AddToBillBtn_Click"/>
                        </div>
                    </div>
                </div>
                
                <!-- 书本列表区域 -->
                <div class="row mt-4 shadow-sm p-2 bg-white rounded">
                    <!-- 列表标题 -->
                    <div class="bg-light p-1 mb-2 rounded">
                        <h4 class="text-center" style="color:#008B8B;">书本列表</h4>
                    </div>
                    
                    <!-- 列表内容 -->
                    <div class="col table-responsive">
                        <asp:GridView ID="ProductList" runat="server" class="table table-hover" CellPadding="4" ForeColor="#333333" GridLines="None" AutoGenerateSelectButton="True" OnSelectedIndexChanged="ProductList_SelectedIndexChanged" >
                            <AlternatingRowStyle BackColor="White" />
                            <EditRowStyle BackColor="#7C6F57" />
                            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="teal" Font-Bold="false" ForeColor="White" />
                            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#E3EAEB" />
                            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#F8FAFA" />
                            <SortedAscendingHeaderStyle BackColor="#246B61" />
                            <SortedDescendingCellStyle BackColor="#D4DFE1" />
                            <SortedDescendingHeaderStyle BackColor="#15524A" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
            
            <!-- 右侧面板: 购物车 -->
            <div class="col-md-7">
                <!-- 购物车标题 -->
                <div class="bg-light p-2 rounded">
                    <h4 class="text-center" style="color:#B22222;">购物车详情</h4>
                </div>
                
                <!-- 购物车列表 -->
                <div class="shadow-sm p-3 mb-3 bg-white rounded">
                    <div class="table-responsive">
                        <asp:GridView ID="ShoppingCartList" runat="server" class="table table-striped" CellPadding="4" ForeColor="#333333" GridLines="None" OnSelectedIndexChanged="ProductList_SelectedIndexChanged" >
                            <AlternatingRowStyle BackColor="White" />
                            <EditRowStyle BackColor="#7C6F57" />
                            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="SlateBlue" Font-Bold="false" ForeColor="White" />
                            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#E3EAEB" />
                            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#F8FAFA" />
                            <SortedAscendingHeaderStyle BackColor="#246B61" />
                            <SortedDescendingCellStyle BackColor="#D4DFE1" />
                            <SortedDescendingHeaderStyle BackColor="#15524A" />
                        </asp:GridView>
                    </div>

                    <!-- 总计金额区域 -->
                    <div class="row mt-2">
                        <div class="offset-md-6 col-md-6">
                            <div class="d-flex justify-content-end">
                                <asp:Label runat="server" ID="RMBLabel" class="text-danger fw-bold me-2"></asp:Label>
                                <asp:Label runat="server" ID="GrdTotalTb" class="text-danger fw-bold"></asp:Label>
                            </div>
                        </div>
                    </div>
                    
                    <!-- 结算按钮区域 -->
                    <div class="row mt-3">
                        <div class="col-md-4 offset-md-4">
                            <asp:Button Text="结算" runat="server" id="PrintBtn" OnClientClick="PrintBill()" class="btn-warning btn-block btn w-100 shadow" OnClick="PrintBtn_Click"/>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- 主要内容区域 结束 -->
    </div>
    <!-- 主容器 结束 -->
</asp:Content>
