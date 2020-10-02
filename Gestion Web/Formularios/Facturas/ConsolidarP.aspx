<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ConsolidarP.aspx.cs" Inherits="Gestion_Web.Formularios.Facturas.ConsolidarP" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <%--<div class="container">--%>
    <div>

        <div class="col-md-12 col-xs-12">
            <div class="widget stacked">

                <div class="stat">
                    <h5><i class="icon-map-marker"></i>Ventas > CRM</h5>
                </div>

                <div class="widget-header">
                    <i class="icon-wrench"></i>
                    <h3>Pedidos Consolidados</h3>
                </div>
                <!-- /widget-header -->

                <div class="widget-content">

                    <div class="panel-body">

                        <div class="col-md-12 col-xs-12">
                            <div class="table-responsive">

                                <table class="table table-striped table-bordered table-hover" id="dataTables-example2">
                                    <thead>
                                        <tr>
                                            <th>Codigo Articulo</th>
                                            <th>Descripcion</th>
                                            <th>Cantidad a entregar</th>
                                            <th>Ubicacion</th>

                                            <th class="td-actions" style="width: 10%"></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:PlaceHolder ID="phPedidos" runat="server"></asp:PlaceHolder>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                        <!-- /.content -->
                    </div>
                </div>
                <!-- /widget-content -->

            </div>
            <!-- /widget -->
        </div>

        <div class="col-md-12">
            <div class="widget big-stats-container stacked">
                <div  class ="widget-content">
                    <asp:PlaceHolder ID="phSaldo" runat="server" Visible="true">
                        <div id="big_stats" class="form-horizontal">
                            <div class="stat col-md-4 col-sm-4" style="text-align: center">
                                <h5 style="text-align:center">Codigo</h5>
                                <asp:Label ID="lblCodigoArticulo" runat="server" Text="" class="value" Style="color:black" Font-Size="30px" Visible="true"></asp:Label>
                            </div>
                            <div class=" col-md-8 col-sm-4" style="text-align: center;">
                                <h5 style="text-align:center">Descripcion</></h5>
                                <strong><asp:Label ID="lblDescripcionArticulo" runat="server" Text=""  Style="color:black" Font-Size="35px" class="value" Visible="true"></asp:Label></strong>
                            </div>
                        </div>
                        <!-- .stat -->
                    </asp:PlaceHolder>
                </div>
                <!-- /widget-content -->
            </div>
            <!-- /widget -->
        </div>

        <!-- /span12 -->
        <div class="col-md-12 col-xs-12">
            <div class="widget stacked">
                <div class="widget-header">
                    <i class="icon-th-list" style="width: 2%"></i>
                    <h3 style="width: 75%">Articulos por pedidos
                    </h3>
                    <h3>
                        <asp:Label ID="lblSaldo" runat="server" Style="text-align: right" Text="" ForeColor="#666666" Font-Bold="true" Visible="true"></asp:Label>
                    </h3>
                </div>
                <div class="widget-content">
                    <div class="panel-body">

                        <div class="col-md-12 col-xs-12">
                            <div class="table-responsive">
                                <table  class="table table-striped table-bordered table-hover" id="dataTables-example">
                                    <thead>
                                        <tr>
                                            <th>Pedido</th>
                                            <th>Cantidad a entregar</th>
                                            <th>Cantidad Entregada</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:PlaceHolder ID="phArticulos" runat="server"></asp:PlaceHolder>
                                    </tbody>
                                </table>

                            </div>
                            <asp:Button ID="btnAgregar" Style="margin-top: 20px" runat="server" Text="Guardar" OnClick="btnAgregar_Click" class="btn btn-success " />
                        </div>
                        <!-- /.content -->
                    </div>
                </div>
            </div>
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

        <%--<script src="//code.jquery.com/jquery-1.9.1.js"></script>--%>
        <script src="//code.jquery.com/ui/1.10.4/jquery-ui.js"></script>
        <script>
            function CantidadesEditadas() {
                $.msgGrowl({
                    type: 'success'
                    , title: 'Editado'
                    , text: 'Cantidades editadas con exito!.'
                });
            }

        </script>



    </div>
</asp:Content>
