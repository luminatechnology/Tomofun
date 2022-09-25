<%@ Page Language="C#" MasterPageFile="~/MasterPages/TabView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="LM101002.aspx.cs" Inherits="Page_LM101002" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/TabView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" TypeName="LUMTomofunCustomization.Graph.LUM3PLSetupMaint" PrimaryView="Setup">
		<CallbackCommands>
		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
	<px:PXTab ID="tab" runat="server" DataSourceID="ds" Height="150px" Style="z-index: 100" Width="100%" AllowAutoHide="false" DataMember="Setup">
		<Items>
			<px:PXTabItem Text="API Info">
				<Template>
					<px:PXLayoutRule runat="server" StartColumn="True" LabelsWidth="M" ControlSize="XM" />
					<px:PXPanel ID="CstPXPanel1" runat="server" Caption="Topest" RenderStyle="Fieldset">
						<px:PXLayoutRule ID="PXLayoutRule1" runat="server" StartColumn="True" LabelsWidth="M" ControlSize="XM" />
							<px:PXTextEdit runat="server" ID="CstPXTextEdit1" DataField="TopestToken" TextMode="Password"></px:PXTextEdit>
							<px:PXTextEdit runat="server" ID="CstPXTextEdit2" DataField="TopestTokenCA" TextMode="Password"></px:PXTextEdit>
					</px:PXPanel>
					<px:PXPanel ID="CstPXPanel2" runat="server" Caption="Return Helper" RenderStyle="Fieldset">
						<px:PXLayoutRule ID="PXLayoutRule2" runat="server" StartColumn="True" LabelsWidth="M" ControlSize="XM" />
							<px:PXTextEdit runat="server" ID="CstPXTextEdit3" DataField="RHAuthzToken" TextMode="Password"></px:PXTextEdit>
							<px:PXTextEdit runat="server" ID="CstPXTextEdit4" DataField="RHApiKey" TextMode="Password"></px:PXTextEdit>
							<px:PXTextEdit runat="server" ID="CstPXTextEdit5" DataField="RHApiToken" TextMode="Password"></px:PXTextEdit>
						</px:PXPanel>
					<px:PXPanel ID="CstPXPanel3" runat="server" Caption="FedEx" RenderStyle="Fieldset">
						<px:PXLayoutRule ID="PXLayoutRule3" runat="server" StartColumn="True" LabelsWidth="M" ControlSize="XM" />
							<px:PXTextEdit runat="server" ID="CstPXTextEdit8" DataField="FedExClientID" TextMode="Password"></px:PXTextEdit>
							<px:PXTextEdit runat="server" ID="CstPXTextEdit9" DataField="FedExClientSecret" TextMode="Password"></px:PXTextEdit>
					</px:PXPanel>
					<px:PXPanel ID="CstPXPanel4" runat="server" Caption="Google Sheets" RenderStyle="Fieldset">
						<px:PXLayoutRule ID="PXLayoutRule4" runat="server" StartColumn="True" LabelsWidth="M" ControlSize="XM" />
							<px:PXTextEdit runat="server" ID="CstPXTextEdit10" DataField="GoogleSheetsURL"></px:PXTextEdit>
							<px:PXTextEdit runat="server" ID="CstPXTextEdit11" DataField="GoogleSheetName"></px:PXTextEdit>
					</px:PXPanel>
				</Template>
			</px:PXTabItem>
		</Items>
		<AutoSize Container="Window" Enabled="True" MinHeight="200" />
	</px:PXTab>
</asp:Content>