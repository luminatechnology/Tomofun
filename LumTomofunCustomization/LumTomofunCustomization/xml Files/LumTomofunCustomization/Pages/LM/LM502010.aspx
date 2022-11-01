<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormTab.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="LM502010.aspx.cs" Inherits="Page_LM502010" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormTab.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" TypeName="LUMTomofunCustomization.Graph.LUM3PLINReconciliationAPIProc" PrimaryView="TopestReconciliation">
		<CallbackCommands>
		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
	<%--<px:PXFormView ID="form" runat="server" DataSourceID="ds" DataMember="Filter" Width="100%" Height="50px" AllowAutoHide="false">
		<Template>
			<px:PXLayoutRule ID="PXLayoutRule1" runat="server" StartRow="True"></px:PXLayoutRule>
			<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit1" DataField="FromDate" CommitChanges="true"></px:PXDateTimeEdit>
		</Template>
	</px:PXFormView>--%>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" Runat="Server">
	<px:PXTab ID="tab" runat="server" Width="100%" Height="150px" DataSourceID="ds" AllowAutoHide="false">
		<Items>
			<px:PXTabItem Text="Topest">
				<Template>
					<px:PXGrid ID="grid1" runat="server" DataSourceID="ds" Width="100%" Height="150px" SkinID="Inquire" AllowAutoHide="false" NoteIndicator="false" FilesIndicator="false">
						<Levels>
							<px:PXGridLevel DataMember="TopestReconciliation">
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
								<px:PXGridColumn DataField="IsProcessed" Width="60" Type="CheckBox" ></px:PXGridColumn></Columns>
							</px:PXGridLevel>
						</Levels>
						<AutoSize Container="Window" Enabled="True" MinHeight="150" />
						<ActionBar PagerVisible="Bottom" >
							<PagerSettings Mode="NumericCompact" />
						</ActionBar>
					</px:PXGrid>
				</Template>
			</px:PXTabItem>
			<px:PXTabItem Text="Return Helper">
				<Template>
					<px:PXGrid ID="grid2" runat="server" DataSourceID="ds" Width="100%" Height="150px" SkinID="Inquire" AllowAutoHide="false" NoteIndicator="false" FilesIndicator="false">
						<Levels>
							<px:PXGridLevel DataMember="RHReconciliation">
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
								<px:PXGridColumn DataField="RMACode" Width="120" ></px:PXGridColumn>
								<px:PXGridColumn DataField="WHRemarks" Width="120" ></px:PXGridColumn>
								<px:PXGridColumn DataField="AssignSku" Width="70" ></px:PXGridColumn>
								<px:PXGridColumn DataField="CreatedByID" Width="80" ></px:PXGridColumn>
								<px:PXGridColumn DataField="IsProcessed" Width="60" Type="CheckBox" ></px:PXGridColumn></Columns>
							</px:PXGridLevel>
						</Levels>
						<AutoSize Container="Window" Enabled="True" MinHeight="150" />
						<ActionBar PagerVisible="Bottom" >
							<PagerSettings Mode="NumericCompact" />
						</ActionBar>
					</px:PXGrid>
				</Template>
			</px:PXTabItem>
			<px:PXTabItem Text="FedEx">
				<Template>
					<px:PXGrid ID="grid3" runat="server" DataSourceID="ds" Width="100%" Height="150px" SkinID="Inquire" AllowAutoHide="false" NoteIndicator="false" FilesIndicator="false">
						<Levels>
							<px:PXGridLevel DataMember="FedExReconciliation">
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
								<px:PXGridColumn DataField="IsProcessed" Width="60" Type="CheckBox" ></px:PXGridColumn></Columns>
							</px:PXGridLevel>
						</Levels>
						<AutoSize Container="Window" Enabled="True" MinHeight="150" />
						<ActionBar PagerVisible="Bottom" >
							<PagerSettings Mode="NumericCompact" />
						</ActionBar>
					</px:PXGrid>
				</Template>
			</px:PXTabItem>
		</Items>
		<AutoSize Container="Window" Enabled="True" MinHeight="150" />
	</px:PXTab>
</asp:Content>