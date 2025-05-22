<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Customer/CustomerMaster.Master" AutoEventWireup="true" CodeBehind="Billing.aspx.cs" Inherits="OnlineSupermarketTuto.Views.Customer.Billing" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
                            <asp:Button Text="添加到购物车" runat="server" id="AddToCartRedirect" class="btn-warning btn-block btn shadow" OnClick="AddToCartRedirect_Click"/>
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
                        <!-- 添加DataKeyNames属性 -->
                        <asp:GridView ID="ProductList" runat="server" AllowPaging="True" PageSize="5" 
                            OnPageIndexChanging="ProductList_PageIndexChanging" class="table table-hover" 
                            CellPadding="4" ForeColor="#333333" GridLines="None" 
                            AutoGenerateSelectButton="True" OnSelectedIndexChanged="ProductList_SelectedIndexChanged"
                            DataKeyNames="PId">
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
                            <PagerSettings Mode="NumericFirstLast" Position="Bottom" />
                            <PagerStyle CssClass="gridview-pager" HorizontalAlign="Center" BackColor="Teal" ForeColor="White" />
                        </asp:GridView>
                    </div>
                </div>
            </div>

            <!-- 右侧面板: 上架书本区域 -->
            <div class="col-md-7">
                <div class="bg-light p-2 rounded">
                    <h3 class="text-center" style="color:#008B8B;">上架书本</h3>
                </div>
                <div class="row shadow-sm p-2 mb-3 bg-white rounded">
                    <div class="col-md-12">
                        <div class="mb-3">
                            <label for="" class="form-label text-success">书本名称</label>
                            <input type="text" placeholder="" autocomplete="off" runat="server" id="PNameTbNew" class ="form-control" />
                        </div>
                        <div class="mb-3">
                            <label for="" class="form-label text-success">出版社名称</label>
                            <asp:DropDownList ID="PManufactCbNew" runat="server" class="form-control"></asp:DropDownList>
                        </div>
                        <div class="mb-3">
                            <label for="" class="form-label text-success">书本类目</label>
                            <asp:DropDownList ID="PCatCbNew" runat="server" class="form-control"></asp:DropDownList>     
                        </div>
                        <div class="mb-3">
                            <label for="" class="form-label text-success">价格</label>
                            <input type="text" placeholder="" autocomplete="off" runat="server" id="PriceTbNew" class ="form-control" />
                        </div>
                        <div class="mb-3">
                            <label for="" class="form-label text-success">数量</label>
                            <input type="text" placeholder="" autocomplete="off" runat="server" id="QtyTbNew" class ="form-control" />
                        </div>
                        <div class="mb-3">
                            <label for="" class="form-label text-success">作者</label>
                            <input type="text" placeholder="" autocomplete="off" runat="server" id="AuthorTbNew" class="form-control" />
                        </div>
                        <div class="row">
                            <asp:Label runat="server" ID="ErrMsgNew" class="text-danger text-center"></asp:Label>
                            <div class="col-md-4"><asp:Button Text="编辑" runat="server" id="EditBtnNew" class="btn-warning btn-block btn" Width="100px" OnClick="EditBtnNew_Click"/></div>
                            <div class="col-md-4"><asp:Button Text="保存" runat="server" id="SaveBtnNew" class="btn-success btn-block btn" Width="100px" OnClick="SaveBtnNew_Click"/></div>
                            <div class="col-md-4"><asp:Button Text="删除" runat="server" id="DeleteBtnNew" class="btn-danger btn-block btn" Width="100px" OnClick="DeleteBtnNew_Click"/></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <!-- 主要内容区域 结束 -->
    </div>
    <!-- 主容器 结束 -->
</asp:Content>