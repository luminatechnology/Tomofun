<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="LM401000.aspx.cs" Inherits="Pages_LM401000" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" TypeName="LumTomofunCustomization.Graph.LUMDailyInventoryQuery" PrimaryView="Filter">
        <CallbackCommands>
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" DataMember="Filter" Width="100%" Height="50px" AllowAutoHide="false">
        <Template>
            <px:PXLayoutRule runat="server" ID="CstPXLayoutRule1" StartColumn="True" ></px:PXLayoutRule>
            <px:PXDateTimeEdit runat="server" ID="edsDate" DataField="sDate" Width="180px" CommitChanges="True"></px:PXDateTimeEdit>
        </Template>
    </px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
    <px:PXGrid AllowPaging="True" SyncPosition="True" ID="grid" runat="server" DataSourceID="ds" Width="100%" Height="150px" SkinID="PrimaryInquire" AllowAutoHide="false" NoteIndicator="false" FilesIndicator=" false">
        <Levels>
            <px:PXGridLevel DataMember="Transaction">
                <Columns>
                    <px:PXGridColumn DataField="CompanyCD" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="InventoryID" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="InventoryCD" Width="120px"></px:PXGridColumn>
                    <px:PXGridColumn DataField="InventoryITemDescr" Width="200px"></px:PXGridColumn>
                    <px:PXGridColumn DataField="EndQty" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Siteid" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="SiteCD" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="SiteDescr" Width="200px"></px:PXGridColumn>
                    <px:PXGridColumn DataField="LocationID" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="LocationCD" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="LocationDescr" Width="200px"></px:PXGridColumn>
                    <px:PXGridColumn DataField="WarehouseQty" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="VarQty" Width="120"></px:PXGridColumn>
                </Columns>
            </px:PXGridLevel>
        </Levels>
        <AutoSize Container="Window" Enabled="True" MinHeight="150"></AutoSize>
        <ActionBar PagerVisible="Bottom">
            <PagerSettings Mode="NumericCompact" />
        </ActionBar>
        <Mode AllowDelete="False" AllowAddNew="False" AllowUpdate="False" />
    </px:PXGrid>
</asp:Content>
