<%@ Page Title="" Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="TestOpen.aspx.cs" Inherits="Gestion_Web.Formularios.Facturas.TestOpen" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Button ID="btnTest" runat="server" OnClick="btnTest_Click"  Text="a"/>
        </ContentTemplate>
    </asp:UpdatePanel>


    <input id="Button1" type="button" value="button" onclick="test();" />


    <script type="text/javascript">
        function test() {
            window.open('ImpresionPresupuesto?Presupuesto=16668', '_blank');
            window.open('ImpresionPresupuesto?a=3&Presupuesto=37', '_blank');
        }
    </script>
</asp:Content>

