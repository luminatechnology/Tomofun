<%@ Page Language="C#" MasterPageFile="~/MasterPages/ListView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="LM501020.aspx.cs" Inherits="Pages_LM501020" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/ListView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" TypeName="LumTomofunCustomization.Graph.LUMAmazonTransactionProcess" PrimaryView="AmazonTransaction">
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
            <px:PXGridLevel DataMember="AmazonTransaction">
                <Columns>
                    <px:PXGridColumn AllowCheckAll="True" DataField="Selected" Width="40" Type="CheckBox" TextAlign="Center" CommitChanges="True"></px:PXGridColumn>
                    <px:PXGridColumn DataField="BranchID" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="APIType" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="TransactionType" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Marketplace" Width="120"></px:PXGridColumn>
                    <px:PXGridColumn DataField="OrderID" Width="150"></px:PXGridColumn>
                    <px:PXGridColumn DataField="SalesChannel" Width="150"></px:PXGridColumn>
                    <px:PXGridColumn DataField="OrderStatus" Width="150"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Amount" Width="150"></px:PXGridColumn>
                    <px:PXGridColumn DataField="OrderStatus" Width="150"></px:PXGridColumn><px:PXGridColumn DataField="IsProcessed" Width="150" Type="CheckBox"></px:PXGridColumn>
                    <px:PXGridColumn DataField="ErrorMessage" Width="150"></px:PXGridColumn>
                    <px:PXGridColumn DataField="TransJson" Width="400"></px:PXGridColumn>
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
</asp:Content>
