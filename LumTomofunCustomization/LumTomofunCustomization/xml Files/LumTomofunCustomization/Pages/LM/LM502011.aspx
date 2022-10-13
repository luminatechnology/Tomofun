<%@ Page Language="C#" MasterPageFile="~/MasterPages/ListView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="LM502011.aspx.cs" Inherits="Page_LM502011" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/ListView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" TypeName="LumTomofunCustomization.Graph.LUM3PLINReconciliationGSProc" PrimaryView="GSheetsReconciliation">
		<CallbackCommands>
		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phL" runat="Server">
	<px:PXGrid ID="grid" runat="server" DataSourceID="ds" Width="100%" Height="150px" SkinID="Inquire" AllowAutoHide="false">
		<Levels>
			<px:PXGridLevel DataMember="GSheetsReconciliation">
			    <Columns>
			        <px:PXGridColumn DataField="CreatedDateTime" Width="90" ></px:PXGridColumn>
					<px:PXGridColumn DataField="TranDate" Width="120" DisplayFormat="g" ></px:PXGridColumn>
					<px:PXGridColumn DataField="Sku" Width="70" ></px:PXGridColumn>
					<px:PXGridColumn DataField="ProductName" Width="280" ></px:PXGridColumn>
					<px:PXGridColumn DataField="Qty" Width="100" ></px:PXGridColumn>
					<px:PXGridColumn DataField="FBACenterID" Width="120" ></px:PXGridColumn>
					<px:PXGridColumn DataField="DetailedDesc" Width="220" ></px:PXGridColumn>
					<px:PXGridColumn DataField="CountryID" Width="70" ></px:PXGridColumn>
					<px:PXGridColumn DataField="Warehouse" Width="70" ></px:PXGridColumn>
					<px:PXGridColumn DataField="Location" Width="70" ></px:PXGridColumn>
					<px:PXGridColumn DataField="ERPSku" Width="180" ></px:PXGridColumn>
					<px:PXGridColumn DataField="INDate" Width="90" ></px:PXGridColumn>
					<px:PXGridColumn DataField="CreatedByID" Width="80" ></px:PXGridColumn>
					<px:PXGridColumn DataField="IsProcessed" Width="60" Type="CheckBox" ></px:PXGridColumn>
			    </Columns>
			</px:PXGridLevel>
		</Levels>
		<AutoSize Container="Window" Enabled="True" MinHeight="150" />
		<ActionBar PagerVisible="Bottom" >
			<PagerSettings Mode="NumericCompact" />
		</ActionBar>
	</px:PXGrid>
</asp:Content>