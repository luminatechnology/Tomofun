<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="LM501022.aspx.cs" Inherits="Pages_LM501022" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" TypeName="LumTomofunCustomization.Graph.LUMAmazonSettlementTransactionProcess" PrimaryView="Filter">
        <CallbackCommands>
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" DataMember="Filter" Width="100%" Height="150px" AllowAutoHide="false">
        <Template>
            <px:PXDateTimeEdit runat="server" ID="edFromDate" DataField="FromDate" Width="180px"></px:PXDateTimeEdit>
            <px:PXDateTimeEdit runat="server" ID="edToDate" DataField="ToDate" Width="180px"></px:PXDateTimeEdit>
            <px:PXDropDown runat="server" ID="edProcessType" DataField="ProcessType" Width="200px"></px:PXDropDown>
        </Template>
    </px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
    <px:PXGrid AllowPaging="True" AdjustPageSize="Auto" SyncPosition="True" ID="grid" runat="server" DataSourceID="ds" Width="100%" Height="150px" SkinID="Primary" AllowAutoHide="false">
        <Levels>
            <px:PXGridLevel DataMember="SettlementTransaction">
                <Columns>
                    <px:PXGridColumn AllowCheckAll="True" DataField="Selected" Width="40" Type="CheckBox" TextAlign="Center" CommitChanges="True"></px:PXGridColumn>
                    <px:PXGridColumn DataField="BranchID" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Marketplace" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="SettlementID" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="SettlementStartDate" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="SettlementEndDate" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="DepositDate" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="TotalAmount" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="DepositCurrency" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="OrderID" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="TransactionType" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="AmountType" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="AmountDescription" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Amount" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="PostedDate" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="MarketPlaceName" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="MerchantOrderID" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="MerchantOrderItemID" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Sku" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="QuantityPurchased" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="IsProcessed" Width="120" Type="CheckBox"></px:PXGridColumn>
                    <px:PXGridColumn DataField="ErrorMessage" Width="200"></px:PXGridColumn>
                    <px:PXGridColumn DataField="CurrentMarketplace" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="CreatedDateTime" Width="130" DisplayFormat="g"></px:PXGridColumn>
                </Columns>
                <RowTemplate>
                    <px:PXSegmentMask AllowEdit="True" runat="server" ID="CstPXSegmentMask4" DataField="BranchID"></px:PXSegmentMask>
                </RowTemplate>
            </px:PXGridLevel>
        </Levels>
        <AutoSize Container="Window" Enabled="True" MinHeight="150"></AutoSize>
        <ActionBar>
        </ActionBar>
        <Mode AllowDelete="True"  />
    </px:PXGrid>
</asp:Content>
