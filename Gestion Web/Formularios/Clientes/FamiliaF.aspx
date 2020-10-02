<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FamiliaF.aspx.cs" Inherits="Gestion_Web.Formularios.Clientes.FamiliaF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <div class="main">
    <%--<div>--%>

        <div class="col-md-12 col-xs-12">
            <div class="widget stacked">

                <div class="stat">
                    <h5><i class="icon-map-marker"></i>Clientes > Familia</h5>
                </div>

                <div class="widget-header">
                    <i class="icon-wrench"></i>
                    <h3>Familia</h3>
                </div>
                <!-- /widget-header -->

                <div class="widget-content">

                    <div class="form-horizontal">

                        <div class="col-md-4">
                            <label for="name">Padre</label>
                            <asp:ListBox runat="server" ID="ListPadre" style="height:100vh" SelectionMode="Single" OnSelectedIndexChanged="ListPadre_SelectedIndexChanged" CssClass="form-control"></asp:ListBox>
                        </div>

                        <div class="col-md-4">
                            <label for="name">Padre</label>
                            <asp:ListBox runat="server" ID="ListHijo" style="height:100vh" SelectionMode="Single" CssClass="form-control"></asp:ListBox>
                        </div>

                        <div class="col-md-4">
                            <label for="name">Padre</label>
                            <asp:ListBox runat="server" ID="ListNieto" style="height:100vh" SelectionMode="Single" CssClass="form-control"></asp:ListBox>
                        </div>

                    </div>

                </div>
                <!-- /widget-content -->

            </div>
            <!-- /widget -->
        </div>

        <script src="../../Scripts/JSFunciones1.js"></script>
        <link href="../../css/pages/reports.css" rel="stylesheet">
        <!-- Core Scripts - Include with every page -->
        <script src="../../Scripts/jquery-1.10.2.js"></script>
        <script src="../../Scripts/bootstrap.min.js"></script>
        <script src="../../Scripts/libs/jquery-1.9.1.min.js"></script>
        <script src="../../Scripts/libs/jquery-ui-1.10.0.custom.min.js"></script>
        <script src="../../Scripts/libs/bootstrap.min.js"></script>
        <script src="../../Scripts/plugins/hoverIntent/jquery.hoverIntent.minified.js"></script>
        <script src="../../Scripts/Application.js"></script>
        <script src="../../Scripts/demo/gallery.js"></script>
        <script src="../../Scripts/plugins/msgGrowl/js/msgGrowl.js"></script>
        <script src="../../Scripts/plugins/lightbox/jquery.lightbox.min.js"></script>
        <script src="../../Scripts/plugins/msgbox/jquery.msgbox.min.js"></script>
        <script src="../../Scripts/demo/notifications.js"></script>
        <script src="//code.jquery.com/ui/1.10.4/jquery-ui.js"></script>

    </div>
</asp:Content>
