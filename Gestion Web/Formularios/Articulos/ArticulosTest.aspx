<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ArticulosTest.aspx.cs" Inherits="Gestion_Web.Formularios.Articulos.ArticulosTest" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="main">

        <div class="container">

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
                                                    <asp:TextBox ID="txtBusqueda" runat="server" class="form-control" placeholder="Buscar Articulo"></asp:TextBox>
                                                    <span class="input-group-btn">
                                                            <asp:LinkButton ID="lbBuscar" runat="server" Text="Buscar" class="btn btn-primary" OnClick="btnBuscar_Click">
                                                    <i class="shortcut-icon icon-search"></i></asp:LinkButton>
                                                    </span>

                                                </div>
                                                <!-- /input-group -->
                                            </div>
                                        </td>

                                        <td style="width: 45%"></td>

                                        <td style="width: 5%">
                                            <%--<div class="shortcuts" style="height:50%">--%>



                                            <a href="ArticulosABM.aspx?accion=1" class="btn btn-default ui-tooltip" data-toggle="tooltip" title data-original-title="Agregar" style="width: 100%">
                                                <i class="shortcut-icon icon-plus"></i>
                                                <%--<span class="shortcut-label">Users</span>--%>
                                            </a>

                                            <%--</div>--%>

                                        </td>

                                        <td style="width: 5%">
                                            <div class="shortcuts">
                                                <%--<asp:LinkButton ID="LinkButton1" runat="server" class="shortcut" OnClick="LinkButton1_Click"></asp:LinkButton>--%>

                                                <a href="/" id="linkEditar" runat="server" class="btn btn-default ui-tooltip" data-toggle="tooltip" title data-original-title="Editar" style="width: 100%">

                                                    <i class="shortcut-icon icon-pencil"></i>
                                                    <%--<span class="shortcut-label">Users</span>--%>
                                                </a>
                                            </div>
                                        </td>


                                        <td style="width: 5%">
                                            <div class="shortcuts" style="height: 100%">
                                                <%--<asp:LinkButton ID="LinkButton1" runat="server" class="shortcut" OnClick="LinkButton1_Click"></asp:LinkButton>--%>

                                                <a href="/" id="A1" runat="server" class="btn btn-default ui-tooltip" data-toggle="tooltip" title data-original-title="Imprimir" style="width: 100%">

                                                    <i class="shortcut-icon icon-print"></i>
                                                    <%--<span class="shortcut-label">Users</span>--%>
                                                </a>
                                            </div>
                                        </td>

                                        <td style="width: 5%">
                                            <div class="shortcuts" style="height: 100%">
                                                <%--<asp:LinkButton ID="LinkButton1" runat="server" class="shortcut" OnClick="LinkButton1_Click"></asp:LinkButton>--%>

                                                <a href="/" id="linkStock" runat="server" class="btn btn-default ui-tooltip" data-toggle="tooltip" title data-original-title="Stock" style="width: 100%">
                                                    <i class="shortcut-icon icon-edit"></i>

                                                </a>
                                            </div>
                                        </td>

                                        <td style="width: 5%">
                                            <div class="shortcuts">
                                                <%--<asp:LinkButton ID="LinkButton1" runat="server" class="shortcut" OnClick="LinkButton1_Click"></asp:LinkButton>--%>

                                                <a href="/" id="A2" runat="server" class="btn btn-default ui-tooltip" data-toggle="tooltip" title data-original-title="Eliminar" style="width: 100%">

                                                    <i class="shortcut-icon icon-trash"></i>
                                                    <%--<span class="shortcut-label">Users</span>--%>
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
                    <div class="col-md-12 col-xs-12">
                        <div class="widget stacked widget-table action-table">

                        <div class="widget-header">
                            <i class="icon-th-list"></i>
                            <h3>Articulos</h3>
                        </div>
                            <div class="widget-content">
                                <div class="panel-body">

                                    <%--<div class="col-md-12 col-xs-12">--%>
                                            <div class="table-responsive">
                                                <table class="table table-striped table-bordered table-hover" id="dataTables-example">
                                                    <thead>
                                                        <tr>
                                                            <th style="width: 5%">Codigo</th>
                                                            <th style="width: 40%">Descripcion</th>
                                                            <th style="width: 5%">IVA</th>
                                                            <th style="width: 15%">Grupo</th>
                                                            <th style="width: 15%">Sub Grupo</th>
                                                            <th style="width: 5%">Moneda</th>
                                                            <th style="width: 10%">Precio Venta</th>
                                                            <th class="td-actions" style="width: 5%"></th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <asp:PlaceHolder ID="phArticulos" runat="server"></asp:PlaceHolder>
                                                    </tbody>
                                                </table>
                                            </div>


                                </div>


                                <!-- /.content -->

                            </div>

                        </div>
                    </div>
            <!-- /container -->

        </div>
        <!-- /main -->

    </div>

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
