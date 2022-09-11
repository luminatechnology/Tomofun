<%@ Page Language="C#" MasterPageFile="~/MasterPages/ListView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="LM201002.aspx.cs" Inherits="Pages_LM201002" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/ListView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" TypeName="LumTomofunCustomization.Graph.LUMShopifyMarketplacePreferenceMaint" PrimaryView="Transactions">
        <CallbackCommands>
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phL" runat="Server">
    <px:PXGrid AllowPaging="True" AdjustPageSize="Auto" SyncPosition="True" ID="grid" runat="server" DataSourceID="ds" Width="100%" Height="150px" SkinID="Primary" AllowAutoHide="false">
        <Levels>
            <px:PXGridLevel DataMember="Transactions">
                <Columns>
                    <px:PXGridColumn DataField="BAccountID" Width="150px" DisplayMode="Hint" CommitChanges="true"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Marketplace" Width="120px"></px:PXGridColumn>
                    <px:PXGridColumn DataField="IsTaxCalculation" Width="100px" Type="CheckBox"></px:PXGridColumn>
                    <px:PXGridColumn DataField="TimeZone" Width="100px"></px:PXGridColumn>
                </Columns>
            </px:PXGridLevel>
        </Levels>
        <AutoSize Container="Window" Enabled="True" MinHeight="150"></AutoSize>
        <ActionBar>
        </ActionBar>
        <Mode AllowUpload="True" />
    </px:PXGrid>
</asp:Content>
