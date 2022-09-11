<%@ Page Language="C#" MasterPageFile="~/MasterPages/ListView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="LM202000.aspx.cs" Inherits="Page_LM202000" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/ListView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" TypeName="LUMTomofunCustomization.Graph.LUM3PLWHMappingMaint" PrimaryView="ThirdPLWHMap">
		<CallbackCommands>
		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phL" runat="Server">
	<px:PXGrid ID="grid" runat="server" DataSourceID="ds" Width="100%" Height="150px" SkinID="Primary" AllowAutoHide="false">
		<Levels>
			<px:PXGridLevel DataMember="ThirdPLWHMap">
			    <Columns>
			        <px:PXGridColumn DataField="ThirdPLType" Width="150px"></px:PXGridColumn>
					<px:PXGridColumn DataField="CountryID" Width="150px"></px:PXGridColumn>
					<px:PXGridColumn DataField="SourceWH" Width="150px"></px:PXGridColumn>
					<px:PXGridColumn DataField="ERPWH" Width="150px"></px:PXGridColumn>
			    </Columns>
				<RowTemplate>
					<px:PXSelector runat="server" ID="CstPXSelector1" DataField="CountryID" AllowEdit="true"/>
                    <px:PXSelector runat="server" ID="CstPXSelector2" DataField="ERPWH" AllowEdit="true"/>
                </RowTemplate>
			</px:PXGridLevel>
		</Levels>
		<AutoSize Container="Window" Enabled="True" MinHeight="150" />
		<Mode AllowUpload="true" />
		<ActionBar >
		</ActionBar>
	</px:PXGrid>
</asp:Content>