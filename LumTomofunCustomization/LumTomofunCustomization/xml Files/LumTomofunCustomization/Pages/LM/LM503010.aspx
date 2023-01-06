<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" AutoEventWireup="true" CodeFile="LM503010.aspx.cs" Inherits="Pages_LM503010" %>

<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="LumTomofunCustomization.Graph.LUMAmazon_USPaymentUploadProcess"
        PrimaryView="PaymentTransactions">
        <CallbackCommands>
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" DataMember="Filter" Width="100%" Height="50px" AllowAutoHide="false">
        <Template>
            <px:PXTextEdit ID="edApiTotal" runat="server" DataField="ApiTotal"></px:PXTextEdit>
        </Template>
    </px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
    <px:PXGrid SyncPosition="True" ID="grid" runat="server" DataSourceID="ds" Width="100%" Height="150px" SkinID="Primary" AllowAutoHide="false" PageSize="500">
        <CallbackCommands>
            <Refresh RepaintControlsIDs="form" />
        </CallbackCommands>
        <Levels>
            <px:PXGridLevel DataMember="PaymentTransactions">
                <Columns>
                    <px:PXGridColumn DataField="Selected" Type="CheckBox" AllowCheckAll="true"></px:PXGridColumn>
                    <px:PXGridColumn DataField="IsProcessed" Type="CheckBox"></px:PXGridColumn>
                    <px:PXGridColumn DataField="ErrorMessage" />
                    <px:PXGridColumn DataField="SequenceNumber"></px:PXGridColumn>
                    <px:PXGridColumn DataField="API_MarketPlace"></px:PXGridColumn>
                    <px:PXGridColumn DataField="ReportDateTime"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Settlementid"></px:PXGridColumn>
                    <px:PXGridColumn DataField="ReportType" />
                    <px:PXGridColumn DataField="OrderID" />
                    <px:PXGridColumn DataField="Sku" />
                    <px:PXGridColumn DataField="Description" />
                    <px:PXGridColumn DataField="ProductSales" />
                    <px:PXGridColumn DataField="ProductSalesTax" />
                    <px:PXGridColumn DataField="ShippingCredits" />
                    <px:PXGridColumn DataField="ShippingCreditsTax"></px:PXGridColumn>
                    <px:PXGridColumn DataField="GiftWrapCredits"></px:PXGridColumn>
                    <px:PXGridColumn DataField="GiftWrapCreditsTax"></px:PXGridColumn>
                    <px:PXGridColumn DataField="RegulatoryFee"></px:PXGridColumn>
                    <px:PXGridColumn DataField="TaxOnRegulatoryFee"></px:PXGridColumn>
                    <px:PXGridColumn DataField="PromotionalRebates"></px:PXGridColumn>
                    <px:PXGridColumn DataField="PromotionalRebatesTax"></px:PXGridColumn>
                    <px:PXGridColumn DataField="MarketplaceWithheldTax"></px:PXGridColumn>
                    <px:PXGridColumn DataField="SellingFees"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Fbafees"></px:PXGridColumn>
                    <px:PXGridColumn DataField="OtherTransactionFee"></px:PXGridColumn>
                    <px:PXGridColumn DataField="OtherFee"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Total"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Api_date_1" DisplayFormat="g"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Api_date" DisplayFormat="g"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Api_settlementid"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Api_trantype"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Api_orderid"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Api_sku"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Api_description"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Api_productsales"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Api_producttax"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Api_shipping"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Api_shippingtax"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Api_giftwrap"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Api_giftwraptax"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Api_regulatoryfee"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Api_taxonregulatoryfee"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Api_promotion"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Api_promotiontax"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Api_whtax"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Api_sellingfee"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Api_fbafee"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Api_othertranfee"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Api_otherfee"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Api_total"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Api_cod"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Api_codfee"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Api_coditemcharge"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Api_points"></px:PXGridColumn>
                    <px:PXGridColumn DataField="CreatedDateTime" Width="130" DisplayFormat="g"></px:PXGridColumn>
                    <px:PXGridColumn DataField="CreatedByID" Width="130" DisplayFormat="g"></px:PXGridColumn>
                </Columns>
            </px:PXGridLevel>
        </Levels>
        <AutoSize Container="Window" Enabled="True" MinHeight="150" />
        <ActionBar>
        </ActionBar>
        <Mode AllowUpdate="True" AllowUpload="True" AllowDelete="True" AllowAddNew="True" />
    </px:PXGrid>

</asp:Content>
