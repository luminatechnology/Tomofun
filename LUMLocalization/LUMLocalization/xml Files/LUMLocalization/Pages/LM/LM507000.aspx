<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="LM507000.aspx.cs" Inherits="Page_LM507000" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="LUMLocalization.Graph.RefreshCustomsDuty"
        PrimaryView="Filter"
        >
		<CallbackCommands>

		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" Style="z-index: 100" Width="100%" DataMember="Filter" AllowAutoHide="false">
        <Template>
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="SM" ControlSize="S" />
            <px:PXDateTimeEdit ID="edCuryEffDate" runat="server" DataField="CuryEffDate" CommitChanges="True" />
            <px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="SM" ControlSize="S" />
            <px:PXSelector ID="edBuyingCuryRateTypeID" runat="server" DataField="BuyingCuryRateTypeID" AllowEdit="True" CommitChanges="True" />
			<px:PXSelector ID="edSellingCuryRateTypeID" runat="server" DataField="SellingCuryRateTypeID" AllowEdit="True" CommitChanges="True" />
        </Template>
    </px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
    <px:PXGrid ID="grid" runat="server" Height="150px" Style="z-index: 100;" Width="100%" ActionsPosition="Top" Caption="Rates" SkinID="PrimaryInquire">
        <Levels>
            <px:PXGridLevel DataMember="CurrencyRateList">
                <Columns>
                    <px:PXGridColumn DataField="Selected" TextAlign="Center" Type="CheckBox" AllowCheckAll="True" AllowMove="False" AllowSort="False" />
                    <px:PXGridColumn DataField="FromCuryID"/>
                    <px:PXGridColumn DataField="BuyingCuryRateType" />
                    <px:PXGridColumn DataField="BuyingCuryRate" TextAlign="Right" />
                    <px:PXGridColumn DataField="SellingCuryRateType" />
                    <px:PXGridColumn DataField="SellingCuryRate" TextAlign="Right" />
                </Columns>
            </px:PXGridLevel>
        </Levels>
        <AutoSize Container="Window" Enabled="True" MinHeight="400" />
    </px:PXGrid>
</asp:Content>