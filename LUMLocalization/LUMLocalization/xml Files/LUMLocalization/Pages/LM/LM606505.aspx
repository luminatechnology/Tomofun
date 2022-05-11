<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="LM606505.aspx.cs" Inherits="Page_LM606505" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="LUMLocalization.Graph.LCMValuationMaint"
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
				<px:PXGridColumn DataField="FinPeriodID" Width="72" ></px:PXGridColumn>
				<px:PXGridColumn DataField="ConditionPeriod" Width="72" />
				<px:PXGridColumn DataField="InventoryID" Width="70" ></px:PXGridColumn>
				<px:PXGridColumn DataField="UnitCost" Width="100" ></px:PXGridColumn>
				<px:PXGridColumn DataField="ValuationLoss" Width="100" ></px:PXGridColumn>
				<px:PXGridColumn DataField="LastSalesPrice" Width="100" ></px:PXGridColumn>
				<px:PXGridColumn DataField="LastSalesDate" Width="90" ></px:PXGridColumn>
				<px:PXGridColumn DataField="LastReceiptPrice" Width="100" ></px:PXGridColumn>
				<px:PXGridColumn DataField="LastReceiptDate" Width="90" ></px:PXGridColumn>
				<px:PXGridColumn DataField="IsValuationLoss" Width="70" ></px:PXGridColumn>
				<px:PXGridColumn DataField="FinYtdQty" Width="100" ></px:PXGridColumn>
				<px:PXGridColumn DataField="FinYtdCost" Width="100" ></px:PXGridColumn></Columns>
			</px:PXGridLevel>
		</Levels>
		<AutoSize Container="Window" Enabled="True" MinHeight="150" />
		<ActionBar >
		</ActionBar>
	</px:PXGrid>
</asp:Content>