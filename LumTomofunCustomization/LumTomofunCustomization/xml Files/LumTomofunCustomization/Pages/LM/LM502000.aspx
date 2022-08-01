<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="LM502000.aspx.cs" Inherits="Page_LM502000" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" TypeName="LUMTomofunCustomization.Graph.LUMAmzINReconciliationProc" PrimaryView="Filter" >
		<CallbackCommands>
		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
	<px:PXFormView ID="form" runat="server" DataSourceID="ds" DataMember="Filter" Width="100%" Height="50px" AllowAutoHide="false">
		<Template>
			<px:PXLayoutRule ID="PXLayoutRule1" runat="server" StartRow="True"></px:PXLayoutRule>
			<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit1" DataField="FromDate" CommitChanges="true" /></Template>
	</px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" Runat="Server">
	<px:PXGrid ID="grid" runat="server" DataSourceID="ds" Width="100%" Height="150px" SkinID="Inquire" AllowAutoHide="false" NoteIndicator="false" FilesIndicator="false">
		<Levels>
			<px:PXGridLevel DataMember="Reconcilition">
			    <Columns>
				<px:PXGridColumn DataField="Selected" Width="40" Type="CheckBox" TextAlign="Center" AllowCheckAll="True" CommitChanges="True"></px:PXGridColumn>
				<px:PXGridColumn DataField="CreatedDateTime" Width="90" ></px:PXGridColumn>
				<px:PXGridColumn DataField="SnapshotDate" Width="90" DisplayFormat="g" ></px:PXGridColumn>
				<px:PXGridColumn DataField="INDate" Width="90" />
				<px:PXGridColumn DataField="Fnsku" Width="70" ></px:PXGridColumn>
				<px:PXGridColumn DataField="Sku" Width="70" ></px:PXGridColumn>
				<px:PXGridColumn DataField="ProductName" Width="280" ></px:PXGridColumn>
				<px:PXGridColumn DataField="Qty" Width="100" ></px:PXGridColumn>
				<px:PXGridColumn DataField="Fbacenterid" Width="120" ></px:PXGridColumn>
				<px:PXGridColumn DataField="DetailedDesc" Width="220" ></px:PXGridColumn>
				<px:PXGridColumn DataField="CountryID" Width="70" ></px:PXGridColumn>
				<px:PXGridColumn DataField="Warehouse" Width="70" ></px:PXGridColumn>
				<px:PXGridColumn DataField="Location" Width="70" ></px:PXGridColumn>
				<px:PXGridColumn DataField="ERPSku" Width="180" ></px:PXGridColumn>
				<px:PXGridColumn DataField="CreatedByID" Width="280" ></px:PXGridColumn>
				<px:PXGridColumn DataField="IsProcesses" Width="60" Type="CheckBox" ></px:PXGridColumn></Columns>
			</px:PXGridLevel>
		</Levels>
		<AutoSize Container="Window" Enabled="True" MinHeight="150" />
		<ActionBar PagerVisible="Bottom" >
			<PagerSettings Mode="NumericCompact" />
		</ActionBar>
	</px:PXGrid>
</asp:Content>