<%@ Page Language="C#" MasterPageFile="~/MasterPages/ListView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="LM501010.aspx.cs" Inherits="Pages_LM501010" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/ListView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" TypeName="LumTomofunCustomization.Graph.LUMAmazonInterfaceMaint" PrimaryView="AmazonSourceData">
        <CallbackCommands>
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<%--<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
	<px:PXFormView ID="form" runat="server" DataSourceID="ds" DataMember="Filter" Width="100%" Height="50px" AllowAutoHide="false">
		<Template>
			<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit2" DataField="StartDate" CommitChanges="True" ></px:PXDateTimeEdit>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule3" StartColumn="True" ></px:PXLayoutRule>
			<px:PXDateTimeEdit CommitChanges="True" runat="server" ID="CstPXDateTimeEdit1" DataField="EndDate" ></px:PXDateTimeEdit></Template>
	</px:PXFormView>
</asp:Content>--%>
<asp:Content ID="cont3" ContentPlaceHolderID="phL" runat="Server">
    <px:PXGrid AllowPaging="True" AdjustPageSize="Auto" SyncPosition="True" ID="grid" runat="server" DataSourceID="ds" Width="100%" Height="150px" SkinID="Primary" AllowAutoHide="false">
        <Levels>
            <px:PXGridLevel DataMember="AmazonSourceData">
                <Columns>
                    <px:PXGridColumn AllowCheckAll="True" DataField="Selected" Width="40" Type="CheckBox" TextAlign="Center" CommitChanges="True" ></px:PXGridColumn>
                    <px:PXGridColumn DataField="BranchID" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="APIType" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="TransactionType" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Marketplace" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="IsProcessed" Width="130" Type="CheckBox"></px:PXGridColumn>
                    <px:PXGridColumn DataField="IsSkippedProcess" Width="130" Type="CheckBox"></px:PXGridColumn>
                    <px:PXGridColumn DataField="CreatedDateTime" Width="130" DisplayFormat="g"></px:PXGridColumn>
                </Columns>
                <RowTemplate>
                    <px:PXSegmentMask AllowEdit="True" runat="server" ID="CstPXSegmentMask4" DataField="BranchID"></px:PXSegmentMask>
                </RowTemplate>
            </px:PXGridLevel>
        </Levels>
        <AutoSize Container="Window" Enabled="True" MinHeight="150"></AutoSize>
        <ActionBar>
        </ActionBar>
        <Mode AllowUpload="True" />
    </px:PXGrid>
     <px:PXSmartPanel ID="pnlJsonPanel" runat="server" CaptionVisible="True" Caption="JsonViewer"
        Style="position: static" LoadOnDemand="True" Key="JsonViewer" AutoCallBack-Target="frmMyCommand"
        AutoCallBack-Command="Refresh" DesignView="Content">
        <px:PXFormView ID="frmMyCommand" runat="server" SkinID="Transparent" DataMember="JsonViewer" DataSourceID="ds" EmailingGraph="">
            <Template>
                <px:PXLayoutRule runat="server" ControlSize="L" LabelsWidth="L" StartColumn="True" />
                <px:PXRichTextEdit ID="PXRichTextEdit1" runat="server" DataField="JsonSource" ></px:PXRichTextEdit>
                <px:PXPanel ID="PXPanel1" runat="server" SkinID="Buttons">
                    <px:PXButton ID="btnMyCommandCancel" runat="server" DialogResult="Cancel" Text="Confirm" />
                </px:PXPanel>
            </Template>
        </px:PXFormView>
    </px:PXSmartPanel>
</asp:Content>
