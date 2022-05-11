<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="LM606500.aspx.cs" Inherits="Page_LM606500" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="LUMLocalization.Graph.LumINItemCostHistMaint"
        PrimaryView="MasterViewFilter"
        >
		<CallbackCommands>

		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
	<px:PXFormView ID="form" runat="server" DataSourceID="ds" DataMember="MasterViewFilter" Width="100%" Height="45px" AllowAutoHide="false">
		<Template>
			<px:PXLayoutRule ID="PXLayoutRule1" runat="server" StartRow="True"></px:PXLayoutRule>
			<px:PXSelector CommitChanges="True" runat="server" ID="CstPXSelector1" DataField="FinPeriodID" ></px:PXSelector></Template>
	</px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" Runat="Server">
	<px:PXGrid ID="grid" runat="server" DataSourceID="ds" Width="100%" Height="150px" SkinID="Details" AllowAutoHide="false">
		<Levels>
			<px:PXGridLevel DataMember="DetailsView">
			    <Columns>
				<px:PXGridColumn DataField="InventoryID" Width="70" ></px:PXGridColumn>
				<px:PXGridColumn DataField="LastSalesDate" Width="90" ></px:PXGridColumn>
				<px:PXGridColumn DataField="LastSalesDoc" Width="280" ></px:PXGridColumn>
				<px:PXGridColumn DataField="LastReceiptDate" Width="90" ></px:PXGridColumn>
				<px:PXGridColumn DataField="LastReceiptDoc" Width="280" ></px:PXGridColumn>
				<px:PXGridColumn DataField="EndingQty_FinYtdQty" Width="100" ></px:PXGridColumn>
				<px:PXGridColumn TextField="" DataField="PeriodQtyWithin30D" Width="100" ></px:PXGridColumn>
				<px:PXGridColumn DataField="PeriodQtyFrom30Dto60D" Width="100" ></px:PXGridColumn>
				<px:PXGridColumn DataField="PeriodQtyFrom60Dto90D" Width="100" ></px:PXGridColumn>
				<px:PXGridColumn DataField="PeriodQtyFrom4Mto6M" Width="100" ></px:PXGridColumn>
				<px:PXGridColumn DataField="PeriodQtyFrom7Mto12M" Width="100" ></px:PXGridColumn>
				<px:PXGridColumn DataField="PeriodQtyOver1Y" Width="100" ></px:PXGridColumn>
				<px:PXGridColumn DataField="EndingCost_FinYtdCost" Width="100" ></px:PXGridColumn>
				<px:PXGridColumn DataField="PeriodCostWithin30D" Width="100" ></px:PXGridColumn>
				<px:PXGridColumn DataField="PeriodCostFrom30Dto60D" Width="100" ></px:PXGridColumn>
				<px:PXGridColumn DataField="PeriodCostFrom60Dto90D" Width="100" ></px:PXGridColumn>
				<px:PXGridColumn DataField="PeriodCostFrom4Mto6M" Width="100" ></px:PXGridColumn>
				<px:PXGridColumn DataField="PeriodCostFrom7Mto12M" Width="100" ></px:PXGridColumn>
				<px:PXGridColumn DataField="PeriodCostOver1Y" Width="100" ></px:PXGridColumn>
				<px:PXGridColumn DataField="ItemDescr" Width="280" ></px:PXGridColumn>
				<px:PXGridColumn DataField="ItemClassCD" Width="140" ></px:PXGridColumn>
				<px:PXGridColumn DataField="ItemClassDescr" Width="280" ></px:PXGridColumn>
				<px:PXGridColumn DataField="LastActivityPeriod" Width="70" ></px:PXGridColumn></Columns>
			</px:PXGridLevel>
		</Levels>
		<AutoSize Container="Window" Enabled="True" MinHeight="150" />
		<ActionBar >
		</ActionBar>
	</px:PXGrid>
</asp:Content>