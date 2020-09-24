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
                    <h3>Herramientas</h3>
                </div>
                <!-- /widget-header -->

                <div class="widget-content">


                    <table style="width: 100%">
                        <tr>
                            <td style="width: 20%">
                                <!-- /btn-group -->
                            </td>
                            <td style="width: 63%">
                                <h5>
                                    <asp:Label runat="server" ID="lblParametros" Text="" ForeColor="#cccccc"></asp:Label>
                                </h5>
                            </td>
                            <td style="width: 5%">
                     <%--           <div class="shortcuts" style="height: 100%">

                                    <div class="shortcuts" style="height: 100%">
                                        <asp:LinkButton ID="lbImpresion" class="btn btn-primary" runat="server" Text="<span class='shortcut-icon icon-print'></span>" Visible="false" Style="width: 100%" />
                                    </div>
                                </div>--%>
                            </td>
                            <td style="width: 2%">

                              <%--  <div class="btn-group pull-right" style="width: 100%">
                                    <button type="button" class="btn btn-primary dropdown-toggle ui-tooltip" title data-original-title="Listados" data-toggle="dropdown">
                                        <i class="shortcut-icon icon-print"></i>&nbsp
                                       
                                        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                                        <span class="caret"></span>
                                    </button>

                                </div>--%>

                            </td>

                            
                            <td style="width: 5%">
                               <%-- <div class="shortcuts" style="height: 100%">

                                    <a class="btn btn-primary ui-tooltip" data-toggle="tooltip" title data-original-title="Agregar" style="width: 100%">
                                        <i class="shortcut-icon icon-plus"></i>
                                    </a>
                                </div>--%>
                            </td>
                            <td style="width: 5%">
                                <div class="shortcuts" style="height: 100%">



                                    <a class="btn btn-primary" data-toggle="modal" href="#modalBusqueda" style="width: 100%">
                                        <i class="shortcut-icon icon-filter"></i>
                                    </a>
                                </div>
                            </td>
                        </tr>

                    </table>
                </div>
                <!-- /widget-content -->

            </div>
            <!-- /widget -->
        </div>

        <div class="col-md-12">

            <div class="widget big-stats-container stacked">
                <div class="widget-content">

                    <div id="big_stats" class="cf">
                        <div class="stat">
                            <h4>Total</h4>
                            <asp:Label ID="labelSaldo" runat="server" Text="" class="value"></asp:Label>
                        </div>
                        <!-- .stat -->
                    </div>

                </div>


                <!-- /widget-content -->

            </div>
            <!-- /widget -->

        </div>
        <!-- /span12 -->
        <div class="col-md-12 col-xs-12">
            <div class="widget widget-table">
                <div class="widget-header">
                    <i class="icon-th-list" style="width: 2%"></i>
                    <h3 style="width: 75%">CRM
                    </h3>
                    <h3>
                        <asp:Label ID="lblSaldo" runat="server" Style="text-align: right" Text="" ForeColor="#666666" Font-Bold="true" Visible="true"></asp:Label>
                    </h3>
                </div>
                <div class="widget-content">
                    <div class="panel-body">

                        <div class="col-md-12 col-xs-12">
                            <div class="table-responsive">
                                <a class="btn btn-info" style="display: none" data-toggle="modal" id="abreDialog" href="#modalFacturaDetalle">Agregar Tipo Cliente</a>
                                <table class="table table-striped table-bordered table-hover" id="dataTables-example">
                                    <thead>
                                         <tr>
                                            <th>Codigo</th>
                                            <th>Cliente</th>
                                            <th>E-Mail</th>
                                            <th>Fecha</th>
                                            <th>Detalle</th>
                                            <th>Tarea</th>
                                            <th style="text-align: right;">Vencimiento</th>
                                            <th style="text-align: right;">Estado</th>
                                            <th style="text-align: right;">Usuario</th>
                                            <th class="td-actions" style="width: 10%"></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:PlaceHolder ID="phFacturas" runat="server"></asp:PlaceHolder>
                                    </tbody>
                                </table>

                            </div>
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

    


    </div>
</asp:Content>
