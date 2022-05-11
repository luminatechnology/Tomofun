<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="LM306405.aspx.cs" Inherits="Page_LM306405" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="LUMLocalization.Graph.LMICVendorMaint"
        PrimaryView="DetailsView"
        >
		<CallbackCommands>

		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phG" runat="Server">
	<px:PXGrid ID="grid" runat="server" DataSourceID="ds" Width="100%" Height="150px" SkinID="Primary" AllowAutoHide="false">
		<Levels>
			<px:PXGridLevel DataMember="DetailsView">
			    <Columns>
				<px:PXGridColumn DataField="BranchID" Width="140" ></px:PXGridColumn>
				<px:PXGridColumn CommitChanges="True" DataField="VendorID" Width="140" ></px:PXGridColumn>
				<px:PXGridColumn DataField="VendorName" Width="280" ></px:PXGridColumn>
				<px:PXGridColumn CommitChanges="True" DataField="TenantID" Width="70" ></px:PXGridColumn>
				<px:PXGridColumn DataField="LoginName" Width="250" ></px:PXGridColumn></Columns>
			</px:PXGridLevel>
		</Levels>
		<AutoSize Container="Window" Enabled="True" MinHeight="150" />
		<ActionBar >
		</ActionBar>
	</px:PXGrid>
</asp:Content>