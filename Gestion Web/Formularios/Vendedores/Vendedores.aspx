<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Vendedores.aspx.cs" Inherits="Gestion_Web.Formularios.Vendedores.Vendedores" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">

        <%--<div class="container">--%><div>

                        <div class="col-md-12 col-xs-12">
                <div class="widget stacked">

                    <div class="widget-header">
                        <i class="icon-wrench"></i>
                        <h3>Herramientas</h3>
                    </div>
                    <!-- /widget-header -->

                    <div class="widget-content">


                        <table style="width: 100%">
                            <tr>
                                <td style="width: 30%">
                                    <div class="col-lg-12">
                                        <div class="input-group">
                                            <asp:TextBox ID="txtBusqueda" runat="server" class="form-control" placeholder="Buscar Vendedor"></asp:TextBox>
                                            <span class="input-group-btn">
                                                <asp:LinkButton ID="lbBuscar" runat="server" Text="Buscar" class="btn btn-primary" OnClick="btnBuscar_Click">
                                                    <i class="shortcut-icon icon-search"></i></asp:LinkButton>
                                            </span>

                                        </div>
                                        <!-- /input-group -->
                                    </div>
                                </td>
                                <td style="width: 50%"></td>
                                <td style="width: 10%">
                                    <div class="btn-group pull-right" style="width: 100%">
                                    <button type="button" class="btn btn-primary dropdown-toggle ui-tooltip" title data-original-title="Listados" data-toggle="dropdown">
                                        <i class="shortcut-icon icon-print"></i>&nbsp
                                        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                                        <span class="caret"></span>
                                    </button>
                                    <ul class="dropdown-menu dropdown-menu-right" role="menu" aria-labelledby="dropdownMenu">
                                        <li class="dropdown-submenu dropdown-menu-right"><a tabindex="-1" href="#">Imprimir</a>
                                            <ul class="dropdown-menu">
                                                <li>
                                                    <asp:LinkButton ID="btnExportar" runat="server" OnClick="btnExportar_Click">
                                                        <i class="fa fa-file-excel-o" aria-hidden="true"></i>
                                                        &nbsp Exportar
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="btnImprimir" runat="server" OnClick="btnImprimir_Click">
                                                        <i class="fa fa-file-pdf-o" aria-hidden="true"></i>
                                                        &nbsp Imprimir
                                                    </asp:LinkButton>
                                                </li>
                                            </ul>
                                        </li>
                                    </ul>
                                </div>
                                </td>
                                <td style="width: 5%">
                                    <%--<div class="shortcuts" style="height:50%">--%>

                                    <a href="VendedoresABM.aspx?accion=1" class="btn btn-primary ui-tooltip" data-toggle="tooltip" title data-original-title="Agregar" style="width: 100%">
                                        <i class="shortcut-icon icon-plus"></i>
                                        <%--<span class="shortcut-label">Users</span>--%>
                                    </a>

                                    <%--</div>--%>

                                </td>
                            </tr>
                        </table>
                    </div>
                    <!-- /widget-content -->

                </div>
                <!-- /widget -->
            </div>
                    <div class="col-md-12 col-xs-12">
                        <div class="widget stacked widget-table action-table">

                        <div class="widget-header">
                            <i class="icon-briefcase"></i>
                            <h3>Vendedores</h3>
                        </div>
                            <div class="widget-content">
                                <div class="panel-body">

                                    <%--<div class="col-md-12 col-xs-12">--%>
                                            <div class="table-responsive">
                                                <table class="table table-striped table-bordered table-hover" id="dataTables-example">
                                <thead>
                                    <tr>
                                        <th style="width: 20%" >Legajo</th>
                                        <th style="width: 30%">Nombre</th>
                                        <th style="width: 30%">Apellido</th>
                                        <th style="width: 10%">Comision</th>
                                        <th class="td-actions" style="width: 10%"></th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:PlaceHolder ID="phVendedores" runat="server"></asp:PlaceHolder>
                                </tbody>
                                                </table>
                                            </div>


                                </div>


                                <!-- /.content -->

                            </div>

                        </div>
                    </div>
            <!-- /container -->
            <!-- /container -->

        </div>
        <!-- /main -->
    </div>

       <%--<script src="//code.jquery.com/jquery-1.9.1.js"></script>
    <script src="//code.jquery.com/ui/1.10.4/jquery-ui.js"></--%>script>
    
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


    <!-- Page-Level Plugin Scripts - Tables -->
    <script src="//cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>
    <script src="../../Scripts/plugins/dataTables/custom.tables.js"></script>
    <link href="//cdn.datatables.net/1.10.2/css/jquery.dataTables.css" rel="stylesheet"/>

    <script>
        $(document).ready(function () {
            $('#dataTables-example').dataTable({
                "bLengthChange": false,
                "bFilter": false,
                "bInfo": false,
                "bAutoWidth": false
            });
        });
    </script>

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endReq);
        function endReq(sender, args) {
            $('#dataTables-example').dataTable({
                "bLengthChange": false,
                "bFilter": false,
                "bInfo": false,
                "bAutoWidth": false
            });
        }
    </script>

  

    </asp:Content>