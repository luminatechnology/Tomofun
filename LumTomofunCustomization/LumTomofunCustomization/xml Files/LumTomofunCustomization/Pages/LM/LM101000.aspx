<%@ Page Language="C#" MasterPageFile="~/MasterPages/ListView.master" AutoEventWireup="true" CodeFile="LM101000.aspx.cs" Inherits="Pages_LM101000" %>

<%@ MasterType VirtualPath="~/MasterPages/ListView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="LumTomofunCustomization.Graph.LUMMRPPreferenceMaint"
        PrimaryView="Preference">
        <CallbackCommands>
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>

<asp:Content ID="cont3" ContentPlaceHolderID="phL" runat="Server">
    <px:PXGrid SyncPosition="True" ID="grid" runat="server" DataSourceID="ds" Width="100%" Height="150px" SkinID="Primary" AllowAutoHide="false">
        <Levels>
            <px:PXGridLevel DataMember="Preference">
                <Columns>
                    <px:PXGridColumn DataField="AllocationType"></px:PXGridColumn>
                    <px:PXGridColumn DataField="PlanType"></px:PXGridColumn>
                    <px:PXGridColumn DataField="Mrptype"></px:PXGridColumn>
                    <px:PXGridColumn DataField="GroupingType"></px:PXGridColumn>
                </Columns>
                <RowTemplate>
                    <px:PXDropDown DataField="Mrptype" ID="edMrptype" runat="server"></px:PXDropDown>
                </RowTemplate>
            </px:PXGridLevel>
        </Levels>
        <AutoSize Container="Window" Enabled="True" MinHeight="150" />
        <ActionBar>
        </ActionBar>
        <Mode AllowUpdate="True" AllowAddNew="True" />
    </px:PXGrid>

</asp:Content>
