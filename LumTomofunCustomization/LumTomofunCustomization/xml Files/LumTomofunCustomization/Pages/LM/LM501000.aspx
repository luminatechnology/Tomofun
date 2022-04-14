<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" AutoEventWireup="true" CodeFile="LM501000.aspx.cs" Inherits="Pages_LM501000" %>

<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="LumTomofunCustomization.Graph.LUMForecastUploadProcess"
        PrimaryView="Filter">
        <CallbackCommands>
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" DataMember="Filter" Width="100%" Height="50px" AllowAutoHide="false">
        <Template>
            <px:PXCheckBox runat="server" ID="edWithAttachment" DataField="WithAttachment" CommitChanges="true"></px:PXCheckBox>
        </Template>
    </px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
    <px:PXGrid SyncPosition="True" ID="grid" runat="server" DataSourceID="ds" Width="100%" Height="150px" SkinID="Primary" AllowAutoHide="false">
        <Levels>
            <px:PXGridLevel DataMember="Transaction">
                <Columns>
                    <px:PXGridColumn DataField="Mrptype"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Revision"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Sku"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Company"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Warehouse" />
                    <px:PXGridColumn DataField="Date" />
                    <px:PXGridColumn DataField="Qty" />
                    <px:PXGridColumn DataField="Week" />
                    <px:PXGridColumn DataField="Qoh" />
                    <px:PXGridColumn DataField="CreatedDateTime" Width="130" DisplayFormat="g"></px:PXGridColumn>
                    <px:PXGridColumn DataField="CreatedByID" Width="130" DisplayFormat="g"></px:PXGridColumn>
                    <px:PXGridColumn DataField="LastModifiedDateTime" Width="130" DisplayFormat="g"></px:PXGridColumn>
                </Columns>
                <RowTemplate>
                    <px:PXSelector runat="server" ID="edSku" DataField="Sku"></px:PXSelector>
                    <px:PXSelector runat="server" ID="edWarehouse" DataField="Warehouse"></px:PXSelector>
                </RowTemplate>
            </px:PXGridLevel>
        </Levels>
        <AutoSize Container="Window" Enabled="True" MinHeight="150" />
        <ActionBar>
        </ActionBar>
        <Mode AllowUpdate="True" AllowUpload="True" AllowDelete="True" />
    </px:PXGrid>

</asp:Content>
