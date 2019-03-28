<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProveedoresF.aspx.cs" Inherits="Gestion_Web.Formularios.Proveedores.ProveedoresF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="main">

        <%--<div class="container">--%><div>

            <div class="col-md-12 col-xs-12">
                <div class="widget stacked">
                    <div class="stat">                        
                        <h5><i class="icon-map-marker"></i> Maestro > Proveedores</h5>
                    </div>
                    <div class="widget-header">
                        <i class="icon-wrench"></i>
                        <h3>Herramientas</h3>
                    </div>
                    <!-- /widget-header -->

                    <div class="widget-content">


                        <table style="width: 100%">
                            <tr>
                                <td style="width: 30%">
                                    <div class="col-lg-8">
                                        <div class="input-group">                                            
                                            <span class="input-group-btn">
                                                <asp:LinkButton ID="lbBuscar" runat="server" Text="Buscar" class="btn btn-primary" OnClick="btnBuscar_Click" OnClientClick="obtenerProveedores();">
                                                    <i class="shortcut-icon icon-search"></i>
                                                </asp:LinkButton>
                                            </span>
                                            <asp:TextBox ID="txtBusqueda" runat="server" class="form-control" placeholder="Buscar Proveedor"></asp:TextBox>
                                        </div>
                                        <!-- /input-group -->
                                    </div>
                                </td>

                                <td style="width: 60%">
                                    <asp:TextBox runat="server" ID="txtProveedores" Text="0" Style="display: none"></asp:TextBox>
                                </td>

                                <td style="width: 5%">
                                    <div class="btn-group" style="width: 100%">
                                        <button type="button" class="btn btn-primary dropdown-toggle ui-tooltip" title data-original-title="Listados" data-toggle="dropdown">
                                            <i class="shortcut-icon icon-print"></i>&nbsp
                                            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                                            <span class="caret"></span>
                                        </button>
                                        <ul class="dropdown-menu" role="menu">
                                            <li>
                                                <asp:LinkButton ID="lbtnImpresion" runat="server" Text="Exportar Listado" OnClick="lbtnImpresion_Click" style="width: 90%"/>
                                            </li>
                                        </ul>
                                    </div>
                                </td>

                                <td style="width: 5%">
                                    <a href="ClientesABM.aspx?accion=3" class="btn btn-primary ui-tooltip" data-toggle="tooltip" title data-original-title="Agregar" style="width: 100%">
                                        <i class="shortcut-icon icon-plus"></i>                                    
                                    </a>
                                </td>

                                <%--<td style="width: 5%">
                                    <div class="shortcuts">
                                        <%--<asp:LinkButton ID="LinkButton1" runat="server" class="shortcut" OnClick="LinkButton1_Click"></asp:LinkButton>

                                        <a href="/" id="linkEditar" runat="server" class="btn btn-default ui-tooltip" data-toggle="tooltip" title data-original-title="Editar" style="width: 100%">

                                            <i class="shortcut-icon icon-pencil"></i>
                                            <%--<span class="shortcut-label">Users</span>
                                        </a>
                                    </div>
                                </td>--%>


                                

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
                        <i class="icon-group"></i>
                        <h3>Proveedores</h3>
                    </div>
                    <div class="widget-content">
                        <div class="panel-body">

                            <%--<div class="col-md-12 col-xs-12">--%>
                            <div class="table-responsive">
                                <table class="table table-bordered table-striped" id="dataTables-example">
                                    <thead>
                                        <tr>
                                          <%--  <th style="width: 5%">Codigo</th>
                                            <th style="width: 25%">Razon Social</th>
                                            <th style="width: 25%">Alias</th>
                                            <th style="width: 20%">Tipo</th>
                                            <th style="width: 15%">CUIT</th>
                                            <th class="td-actions" style="width: 10%"></th>--%>
                                            <th style="width: 10%">Codigo</th>
                                            <th style="width: 25%">Razon Social</th>
                                            <th style="width: 25%">Alias</th>
                                            <th style="width: 15%">Tipo</th>
                                            <th style="width: 15%">CUIT</th>
                                            <th class="td-actions" style="width: 10%"></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:PlaceHolder ID="phProveedores" runat="server"></asp:PlaceHolder>
                                    </tbody>
                                </table>
                            </div>


                        </div>


                        <!-- /.content -->

                    </div>

                </div>
            </div>
            <!-- /row -->
        </div>
        <!-- /container -->
    </div>
    <!-- /main -->

    <div id="modalConfirmacion" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Confirmacion de Eliminacion</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <div class="form-group">
                            <div class="col-md-2">
                                <h1>
                                    <i class="icon-warning-sign" style="color: orange"></i>
                                </h1>
                            </div>
                            <div class="col-md-7">
                                <h5>
                                    <asp:Label runat="server" ID="lblMensaje" Text="Esta seguro que desea eliminar el Proveedor?" Style="text-align: center"></asp:Label>
                                </h5>
                            </div>

                            <div class="col-md-3">
                                <asp:TextBox runat="server" ID="txtMovimiento" Text="0" Style="display: none"></asp:TextBox>
                            </div>
                        </div>
                        <%--                                <div class="form-group">
                                    
                                </div>--%>
                    </div>


                    <div class="modal-footer">
                        <asp:Button runat="server" ID="btnSi" Text="Eliminar" class="btn btn-danger" OnClick="btnSi_Click" />
                        <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>

                        <%--                        <asp:LinkButton ID="LinkButton1" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnBuscar_Click" ValidationGroup="BusquedaGroup" />--%>
                    </div>
                </div>

            </div>
        </div>
    </div>
    <%--Fin modalGrupo--%>

    <script>
        function abrirdialog(valor) {
            document.getElementById('<%= txtMovimiento.ClientID %>').value = valor;
            }
    </script>


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
    <link href="//cdn.datatables.net/1.10.2/css/jquery.dataTables.css" rel="stylesheet" />

    <script>
        //$(document).ready(function () {
        //    $('#dataTables-example').dataTable({
        //        "bLengthChange": false,
        //        "bFilter": false,
        //        "bInfo": false,
        //        "bAutoWidth": false
        //    });
        //});
        function obtenerProveedores() {
            var total = "";
            for (var i = 1; i < document.getElementById('dataTables-example').rows.length - 1; i++) {
                total += document.getElementById('dataTables-example').rows[i].cells[0].value + ";";
            }
            document.getElementById('<%= txtProveedores.ClientID %>').value = total;
        }

    </script>

    <script>
        $(document).ready(
            function () {
                var total = "";
                for (var i = 1; i < document.getElementById('dataTables-example').rows.length - 1; i++) {
                    total += document.getElementById('dataTables-example').rows[i].cells[0].value + ";";
                }
                document.getElementById('<%= txtProveedores.ClientID %>').value = total;
                }

        );
    </script>
    <script>
        $(document).ready(function () {
            $('#dataTables-example').dataTable({
                "bLengthChange": false,
                "bFilter": false,
                "bInfo": false,
                "bAutoWidth": false,
                "bStateSave": true
            });
        });
    </script>

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endReq);
        function endReq(sender, args) {
            $('#dataTables-example').dataTable();
        }
    </script>

</asp:Content>
