<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FamiliaF.aspx.cs" Inherits="Gestion_Web.Formularios.Clientes.FamiliaF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <div class="main">
        <div class="col-md-12 col-xs-12">
            <div class="widget stacked">
                <div class="stat">
                    <h5><i class="icon-map-marker"></i> Clientes > Familia</h5>
                </div>
                <div class="widget-header">
                    <i class="icon-wrench"></i>
                    <h3>Busqueda</h3>
                </div>
                <!-- /widget-header -->

                <div class="widget-content">

                    <div class="input-group">
                        <span class="input-group-btn">
                            <asp:LinkButton ID="lbtnBuscarPadre" runat="server" Text="Buscar" class="btn btn-primary" OnClick="lbtnBuscarPadre_Click">
                                                <i class="shortcut-icon icon-search"></i></asp:LinkButton>
                        </span>
                        <asp:TextBox ID="txtBusqueda" runat="server" class="form-control" placeholder="Buscar Padre"></asp:TextBox>

                    </div>

                </div>
                <!-- /widget-content -->

            </div>


            <div class="widget stacked">

                <div class="widget-header">
                    <i class="icon-wrench"></i>
                    <h3>Familia</h3>
                </div>
                <!-- /widget-header -->

                <div class="widget-content">

                    <div class="form-horizontal">

                        <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Always" runat="server">
                            <ContentTemplate>

                                <div class="col-md-4">

                                    <label style="font-size: 20px;" for="name">Padre </label>

                                    <asp:ListBox runat="server" ID="ListPadre" Style="height: 60vh" SelectionMode="Single" OnSelectedIndexChanged="ListPadre_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control"></asp:ListBox>

                                </div>

                                <div class="col-md-4">
                                    <label style="font-size: 20px;" for="name">Hijo</label>
                                    <asp:ListBox runat="server" ID="ListHijo" Style="height: 60vh" SelectionMode="Single" CssClass="form-control" OnSelectedIndexChanged="ListHijo_SelectedIndexChanged" AutoPostBack="true"></asp:ListBox>
                                </div>

                                <div class="col-md-4">
                                    <label style="font-size: 20px;" for="name">Nieto</label>
                                    <asp:ListBox runat="server" ID="ListNieto" Style="height: 60vh" SelectionMode="Single" CssClass="form-control"></asp:ListBox>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
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
