<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="LM501028.aspx.cs" Inherits="Pages_LM501028" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" TypeName="LumTomofunCustomization.Graph.LUMAmazonFulfillmentProcess" PrimaryView="Filter">
        <CallbackCommands>
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" DataMember="Filter" Width="100%" Height="50px" AllowAutoHide="false">
        <Template>
             <px:PXDateTimeEdit runat="server" ID="edFromDate" DataField="StartDate" Width="180px"></px:PXDateTimeEdit>
            <px:PXDateTimeEdit runat="server" ID="edToDate" DataField="EndDate" Width="180px"></px:PXDateTimeEdit>
        </Template>
    </px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
    <px:PXGrid AllowPaging="True" AdjustPageSize="Auto" SyncPosition="True" ID="grid" runat="server" DataSourceID="ds" Width="100%" Height="150px" SkinID="Primary" AllowAutoHide="false">
        <Levels>
            <px:PXGridLevel DataMember="FulfillmentTransactions">
                <Columns>
                    <px:PXGridColumn AllowCheckAll="True" DataField="Selected" Width="40" Type="CheckBox" TextAlign="Center" CommitChanges="True"></px:PXGridColumn>
                    <px:PXGridColumn DataField="AmazonOrderID" Width="150"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Marketplace" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="ShipmentDate" Width="180"></px:PXGridColumn>
                    <px:PXGridColumn DataField="ReportID" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="IsProcessed" Width="120" Type="CheckBox"></px:PXGridColumn>
                    <px:PXGridColumn DataField="CreatedDateTime" Width="130" DisplayFormat="g"></px:PXGridColumn>
                </Columns>
                <RowTemplate>
                    <px:PXDateTimeEdit runat="server" DataField="ShipmentDate" TimeMode="true"></px:PXDateTimeEdit>
                </RowTemplate>
            </px:PXGridLevel>
        </Levels>
        <AutoSize Container="Window" Enabled="True" MinHeight="150"></AutoSize>
        <ActionBar>
        </ActionBar>
        <Mode AllowUpdate="True" AllowUpload="True" AllowAddNew="True" AllowDelete="True" />
    </px:PXGrid>
</asp:Content>
