<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="LM501002.aspx.cs" Inherits="Pages_LM501002" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="LumTomofunCustomization.Graph.LUMMRPProcess" PrimaryView="Filter">
        <CallbackCommands>
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phF" runat="Server">
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" DataMember="Filter" Width="100%" Height="150px" AllowAutoHide="false">
        <Template>
            <px:PXDateTimeEdit runat="server" ID="edDate" DataField="Date" CommitChanges="True" />
            <px:PXSelector runat="server" ID="edSku" DataField="Sku" Size="M"></px:PXSelector>
            <px:PXSelector runat="server" ID="edWarehouse" DataField="Warehouse" Size="M"></px:PXSelector>
            <px:PXSelector runat="server" ID="edItemClassID" DataField="ItemClassID" Size="M"></px:PXSelector>
            <px:PXSelector runat="server" ID="edRevision" DataField="Revision" Size="M"></px:PXSelector>
        </Template>
    </px:PXFormView>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phG" runat="Server">
    <px:PXGrid SyncPosition="True" ID="grid" runat="server" DataSourceID="ds" Width="100%" Height="150px" SkinID="PrimaryInquire" AllowAutoHide="false">
        <Levels>
            <px:PXGridLevel DataMember="Transaction">
                <Columns>
                    <px:PXGridColumn DataField="Sku"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Warehouse" />
                    <px:PXGridColumn DataField="Date"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Revision"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Forecast"></px:PXGridColumn>
                    <px:PXGridColumn DataField="ForecastBase"></px:PXGridColumn>
                    <px:PXGridColumn DataField="OpenSo"></px:PXGridColumn>
                    <px:PXGridColumn DataField="PastOpenSo" />
                    <px:PXGridColumn DataField="ForecastIntial" />
                    <px:PXGridColumn DataField="ForecastComsumption" />
                    <px:PXGridColumn DataField="ForecastRemains" />
                    <px:PXGridColumn DataField="DemandAdj" />
                    <px:PXGridColumn DataField="NetDemand" />
                    <px:PXGridColumn DataField="StockInitial" />
                    <px:PXGridColumn DataField="Supply" />
                    <px:PXGridColumn DataField="StockAva" />
                    <px:PXGridColumn DataField="SafetyStock" />
                    <px:PXGridColumn DataField="CreatedByID" />
                    <px:PXGridColumn DataField="CreatedDateTime" />
                    <px:PXGridColumn DataField="LastModifiedByID" />
                    <px:PXGridColumn DataField="LastModifiedDateTime" />
                </Columns>
            </px:PXGridLevel>
        </Levels>
        <AutoSize Container="Window" Enabled="True" MinHeight="150"></AutoSize>
        <ActionBar>
        </ActionBar>

        <Mode InitNewRow="True"></Mode>
    </px:PXGrid>
</asp:Content>
