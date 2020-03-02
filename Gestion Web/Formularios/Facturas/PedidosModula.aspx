<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PedidosModula.aspx.cs" Inherits="Gestion_Web.Formularios.Facturas.PedidosModula" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

        <div class="col-md-12">


                        <div class="widget stacked">

                            <div class="widget-header">
                                <i class="icon-bookmark"></i>
                                <h3>Pedidos</h3>
                            </div>
                            <!-- /widget-header -->

                            <div class="widget-content">

                                <div class="shortcuts">


                                    <%--<a href="Formularios/Clientes/Clientesaspx.aspx" class="shortcut">
                                        <i class="shortcut-icon icon-group"></i>
                                        <span class="shortcut-label">Pedido 0004-000007</span>
                                    </a>--%>

                                    <asp:PlaceHolder ID="phPedidos" runat="server"></asp:PlaceHolder>

                                    <%--<a href="Formularios/Articulos/Articulos.aspx" class="shortcut">
                                        <i class="shortcut-icon icon-bookmark"></i>
                                        <span class="shortcut-label">0004-000007</span>
                                    </a>

                                    <a href="Formularios/Facturas/ABMPedidos.aspx" class="shortcut">
                                        <i class="shortcut-icon icon-shopping-cart"></i>
                                        <span class="shortcut-label">Pedidos</span>
                                    </a>

                                    <a href="Formularios/Facturas/ABMFacturas.aspx" class="shortcut">
                                        <i class="shortcut-icon icon-file-text"></i>
                                        <span class="shortcut-label">Facturación</span>
                                    </a>

                                    <a href="Formularios/Cobros/CobranzaF.aspx" class="shortcut">
                                        <i class="shortcut-icon icon-money"></i>
                                        <span class="shortcut-label">Cobros</span>
                                    </a>

                                    <a href="Formularios/Valores/CajaF.aspx" class="shortcut">
                                        <i class="shortcut-icon icon-inbox"></i>
                                        <span class="shortcut-label">Caja</span>
                                    </a>

                                    <a href="Formularios/Facturas/CuentaCorrienteF.aspx" class="shortcut">
                                        <i class="shortcut-icon icon-th-list"></i>
                                        <span class="shortcut-label">Cta Cte</span>
                                    </a>

                                    <a href="Formularios/Reportes/BalanceF.aspx" class="shortcut">
                                        <i class="shortcut-icon  icon-adjust"></i>
                                        <span class="shortcut-label">Balance</span>
                                    </a>--%>
                                </div>
                                <!-- /shortcuts -->

                            </div>
                            <!-- /widget-content -->

                        </div>

            </div>

    
    <!-- Page-Level Plugin Scripts - Tables -->
    <script src="//cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>
    <script src="../../Scripts/plugins/dataTables/custom.tables.js"></script>
    <link href="//cdn.datatables.net/1.10.2/css/jquery.dataTables.css" rel="stylesheet" />

    <link href="../../css/pages/reports.css" rel="stylesheet">
    <!-- Core Scripts - Include with every page -->
    <script src="../../Scripts/jquery-1.10.2.js"></script>
    <script src="../../Scripts/bootstrap.min.js"></script>
    <%--<script src="../Scripts/plugins/metisMenu/jquery.metisMenu.js"></script>--%>

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


</asp:Content>
