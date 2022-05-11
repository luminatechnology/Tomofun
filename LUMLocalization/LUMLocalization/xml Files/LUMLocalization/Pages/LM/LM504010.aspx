<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="LM504010.aspx.cs" Inherits="Page_LM504010" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="LUMLocalization.Graph.LumPOCreateIntertenantSO"
        PrimaryView="Filter"
        >
    <CallbackCommands>

    </CallbackCommands>
  </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" Style="z-index: 100"
        Width="100%" DataMember="Filter" Caption="Selection" DefaultControlID="edPODocType">
        <Template>
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="SM" ControlSize="SM" ></px:PXLayoutRule>
            <px:PXDateTimeEdit CommitChanges="True" ID="edDocDate" runat="server" DataField="DocDate" ></px:PXDateTimeEdit>
            <px:PXSegmentMask CommitChanges="True" ID="edSellingCompany" runat="server" DataField="SellingCompany" ></px:PXSegmentMask>
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="SM" ControlSize="M" ></px:PXLayoutRule>
        </Template>
    </px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
    <px:PXGrid ID="grid" runat="server" DataSourceID="ds" Height="150px" Style="z-index: 100"
        Width="100%" ActionsPosition="Top" Caption="Documents" SkinID="PrimaryInquire" SyncPosition="true" FastFilterFields="CustomerID,ShipmentNbr" AllowPaging="True">
        <Levels>
            <px:PXGridLevel DataMember="Documents">
                <RowTemplate>
                </RowTemplate>
                <Columns>
                    <px:PXGridColumn AllowNull="False" DataField="Selected" TextAlign="Center" Type="CheckBox" AllowCheckAll="True" AllowSort="False" AllowMove="False" AllowUpdate="False" AutoCallBack="True" ></px:PXGridColumn>
                    <px:PXGridColumn DataField="Pobranchcd" Width="140" />
                    <px:PXGridColumn DataField="POAcctName" Width="280" />
                    <px:PXGridColumn DataField="POOrderNbr" Width="140" LinkCommand="ViewPOOrder" />
                    <px:PXGridColumn DataField="Sobranchcd" Width="140" />
                    <px:PXGridColumn DataField="ReceiptNbr" Width="140" LinkCommand="ViewPOReceipt" />
                    <px:PXGridColumn DataField="ReceiptDate" Width="140" />
                    <px:PXGridColumn DataField="SOAcctName" Width="280" />
                    <px:PXGridColumn DataField="SOOrderNbr" Width="140" />
                    <px:PXGridColumn DataField="SOShipmentShipmentNbr" Width="140"  />
                    <px:PXGridColumn DataField="SOShipmentStatus" Width="70" />
                    <px:PXGridColumn DataField="SOShipmentShipmentQty" Width="100" />
                    <px:PXGridColumn DataField="SOShipmentShipmentDesc" Width="280" />
                    <px:PXGridColumn DataField="SOShipmentShipDate" Width="90" />
                    <px:PXGridColumn DataField="SOShipmentPackageWeight" Width="100" />
                    <px:PXGridColumn DataField="SOShipmentPackageCount" Width="70" />
                    <px:PXGridColumn DataField="ShipmentWeight" Width="100" />
                    <px:PXGridColumn DataField="ShipmentVolume" Width="100" /></Columns>
            </px:PXGridLevel>
        </Levels>
        <ActionBar DefaultAction="ViewPODocument" ></ActionBar>
        <AutoSize Container="Window" Enabled="True" MinHeight="150" ></AutoSize>
    </px:PXGrid>
</asp:Content>