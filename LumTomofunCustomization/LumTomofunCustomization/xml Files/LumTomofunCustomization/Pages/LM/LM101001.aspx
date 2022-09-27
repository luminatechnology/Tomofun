<%@ Page Language="C#" MasterPageFile="~/MasterPages/TabView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="LM101001.aspx.cs" Inherits="Pages_LM101001" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/TabView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="LumTomofunCustomization.Graph.LUMMWSSetup"
        PrimaryView="Setup">
        <CallbackCommands>
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXTab DataMember="Setup" ID="tab" runat="server" DataSourceID="ds" Height="150px" Style="z-index: 100" Width="100%" AllowAutoHide="false">
        <Items>
            <px:PXTabItem Text="MWS">
                <Template>
                    <px:PXLayoutRule runat="server" StartColumn="true"></px:PXLayoutRule>
                    <px:PXLayoutRule ControlSize="L" runat="server" ID="CstPXLayoutRule4" StartGroup="True" GroupCaption="MWS API Settings"></px:PXLayoutRule>
                    <px:PXTextEdit runat="server" ID="CstPXTextEdit1" DataField="AccessKey"></px:PXTextEdit>
                    <px:PXTextEdit runat="server" ID="CstPXTextEdit2" DataField="SecretKey"></px:PXTextEdit>
                    <px:PXTextEdit runat="server" ID="CstPXTextEdit3" DataField="RoleArn"></px:PXTextEdit>
                    <px:PXTextEdit runat="server" ID="CstPXTextEdit5" DataField="ClientID"></px:PXTextEdit>
                    <px:PXTextEdit runat="server" ID="CstPXTextEdit6" DataField="ClientSecret"></px:PXTextEdit>
                    <px:PXLayoutRule ControlSize="L" GroupCaption="US Marketplace" runat="server" ID="CstPXLayoutRule6" StartGroup="True"></px:PXLayoutRule>
                    <px:PXTextEdit runat="server" ID="CstPXTextEdit7" DataField="USMarketplaceID" />
                    <px:PXTextEdit runat="server" ID="CstPXTextEdit8" DataField="USRefreshToken" />
                    <px:PXLayoutRule ControlSize="L" GroupCaption="EU Marketplace" runat="server" ID="PXLayoutRule1" StartGroup="True"></px:PXLayoutRule>
                    <px:PXTextEdit runat="server" ID="PXTextEdit1" DataField="EUMarketplaceID" />
                    <px:PXTextEdit runat="server" ID="PXTextEdit2" DataField="EURefreshToken" />
                    <px:PXLayoutRule ControlSize="L" GroupCaption="JP Marketplace" runat="server" ID="PXLayoutRule2" StartGroup="True"></px:PXLayoutRule>
                    <px:PXTextEdit runat="server" ID="PXTextEdit3" DataField="JPMarketplaceID" />
                    <px:PXTextEdit runat="server" ID="PXTextEdit4" DataField="JPRefreshToken" />
                    <px:PXLayoutRule ControlSize="L" GroupCaption="AU Marketplace" runat="server" ID="PXLayoutRule3" StartGroup="True"></px:PXLayoutRule>
                    <px:PXTextEdit runat="server" ID="PXTextEdit5" DataField="AUMarketplaceID" />
                    <px:PXTextEdit runat="server" ID="PXTextEdit6" DataField="AURefreshToken" />
                    <px:PXLayoutRule ControlSize="L" GroupCaption="MX Marketplace" runat="server" ID="PXLayoutRule4" StartGroup="True"></px:PXLayoutRule>
                    <px:PXTextEdit runat="server" ID="PXTextEdit7" DataField="MXMarketplaceID" />
                    <px:PXTextEdit runat="server" ID="PXTextEdit8" DataField="MXRefreshToken" />
                    <px:PXTextEdit runat="server" ID="PXTextEdit16" DataField="MXClientID"></px:PXTextEdit>
                    <px:PXTextEdit runat="server" ID="PXTextEdit17" DataField="MXClientSecret"></px:PXTextEdit>
                    <px:PXLayoutRule runat="server" StartColumn="true"></px:PXLayoutRule>
                    <px:PXLayoutRule ControlSize="L" runat="server" ID="PXLayoutRule5" StartGroup="True" GroupCaption="MWS SG API Settings"></px:PXLayoutRule>
                    <px:PXTextEdit runat="server" ID="PXTextEdit9" DataField="SGAccessKey"></px:PXTextEdit>
                    <px:PXTextEdit runat="server" ID="PXTextEdit10" DataField="SGSecretKey"></px:PXTextEdit>
                    <px:PXTextEdit runat="server" ID="PXTextEdit11" DataField="SGRoleArn"></px:PXTextEdit>
                    <px:PXTextEdit runat="server" ID="PXTextEdit12" DataField="SGClientID"></px:PXTextEdit>
                    <px:PXTextEdit runat="server" ID="PXTextEdit13" DataField="SGClientSecret"></px:PXTextEdit>
                    <px:PXLayoutRule ControlSize="L" GroupCaption="SG Marketplace" runat="server" ID="PXLayoutRule6" StartGroup="True"></px:PXLayoutRule>
                    <px:PXTextEdit runat="server" ID="PXTextEdit14" DataField="SGMarketplaceID" />
                    <px:PXTextEdit runat="server" ID="PXTextEdit15" DataField="SGRefreshToken" />
                </Template>
            </px:PXTabItem>
        </Items>
        <AutoSize Container="Window" Enabled="True" MinHeight="200"></AutoSize>
    </px:PXTab>
</asp:Content>
