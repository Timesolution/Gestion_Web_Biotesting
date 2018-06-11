<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ComprasF.aspx.cs" Inherits="Gestion_Web.Formularios.Compras.ComprasF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">

        <div >

            <div class="col-md-12 col-xs-12">
                <div class="widget stacked">
                    <div class="stat">
                        <h5><i class="icon-map-marker"></i> Compras > Compras</h5>
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
                                    <div class="btn-group">
                                        <button type="button" class="btn btn-success dropdown-toggle" data-toggle="dropdown" id="btnAccion" runat="server">Accion    <span class="caret"></span></button>
                                        <ul class="dropdown-menu">
                                            <li>
                                                <asp:LinkButton ID="btnAnular" runat="server" Visible="false" OnClick="btnAnular_Click">Anular Compra</asp:LinkButton>
                                            </li>                                            
                                            <li>
                                                <asp:LinkButton ID="btnCitiCompras" runat="server" OnClick="btnCitiCompras_Click">Citi Compras</asp:LinkButton>
                                            </li>
                                            <li class="dropdown-submenu dropdown-menu-right"><a tabindex="-1" href="#">IVA Compras</a>  
                                                <ul class="dropdown-menu" >
                                                    <li>
                                                        <asp:LinkButton ID="lbtnImprimirDetalle" runat="server" OnClick="lbtnImprimirDetalle_Click">
                                                            <i class="fa fa-file-pdf-o" aria-hidden="true"></i>
                                                            &nbsp Imprimir
                                                        </asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lbtnExportarDetalle" runat="server" OnClick="lbtnExportarDetalle_Click">
                                                            <i class="fa fa-file-excel-o" aria-hidden="true"></i>
                                                            &nbsp Exportar
                                                        </asp:LinkButton>
                                                    </li>
                                                </ul>                                                                                                                                                                      
                                            </li>
                                            <li class="dropdown-submenu dropdown-menu-right"><a tabindex="-1" href="#">Detalle Compras</a>  
                                                <ul class="dropdown-menu" >
                                                    <li>
                                                        <asp:LinkButton ID="btnExportarExcel" runat="server" OnClick="btnExportarExcel_Click">
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
                                    <!-- /btn-group -->
                                </td>
                                <td style="width: 65%">
                                    <h5>
                                        <asp:Label runat="server" ID="lblParametros" Text="" ForeColor="#cccccc"></asp:Label>
                                    </h5>
                                </td>
                                <td style="width: 5%"></td>

                                <td style="width: 5%">
                                    <div class="shortcuts" style="height: 100%">
                                        <a class="btn btn-primary" data-toggle="modal" href="#modalBusqueda" style="width: 100%">
                                            <i class="shortcut-icon icon-filter"></i>
                                        </a>
                                    </div>
                                </td>
                                <td style="width: 5%">
                                    <div class="shortcuts" style="height: 100%">



                                        <a href="ComprasABM.aspx?a=1" class="btn btn-primary ui-tooltip" data-toggle="tooltip" title data-original-title="Agregar" style="width: 100%">
                                            <i class="shortcut-icon icon-plus"></i>
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
                                <h4>Saldo</h4>
                                <asp:Label ID="labelSaldo" runat="server" Text="" class="value" Visible="false"></asp:Label>
                            </div>
                            <!-- .stat -->
                        </div>

                    </div>


                    <!-- /widget-content -->

                </div>
                <!-- /widget -->

            </div>

            <div class="col-md-12 col-xs-12">
                <div class="widget widget-table">
                    <div class="widget-header">
                        <i class="icon-th-list" style="width: 2%"></i>
                        <h3 style="width: 75%">Compras
                        </h3>
                        <h3>
                            <asp:Label ID="lblSaldo" runat="server" Style="text-align: right" Text="" ForeColor="#666666" Font-Bold="true"></asp:Label>
                        </h3>
                    </div>
                    <div class="widget-content">
                        <div class="panel-body">


                            <div class="table-responsive">
                                <a class="btn btn-info" style="display: none" data-toggle="modal" id="abreDialog" href="#modalFacturaDetalle">Agregar Tipo Cliente</a>
                                <table class="table table-striped table-bordered table-hover" id="dataTables-example">
                                    <thead>
                                        <tr>
                                            <th>Fecha</th>
                                            <th>Fecha Imputacion</th>
                                            <th>Tipo</th>
                                            <th>Numero</th>
                                            <th>Proveedor</th>
                                            <th>Total</th>
                                            <th></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:PlaceHolder ID="phCompra" runat="server"></asp:PlaceHolder>
                                    </tbody>
                                </table>

                            </div>
                        </div>


                        <!-- /.content -->

                    </div>

                </div>
            </div>

        </div>


        <%--Fin modalGrupo--%>

        <div id="modalBusqueda" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">

                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Busqueda</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <asp:UpdatePanel ID="UpdatePanel4" UpdateMode="Always" runat="server">
                                <ContentTemplate>
                                    <%--<div class="form-group">
                                        <label class="col-md-4">Desde</label>
                                        <div class="col-md-4">

                                            <asp:TextBox ID="txtFechaDesde" runat="server" class="form-control"></asp:TextBox>

                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-4">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaDesde" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Hasta</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtFechaHasta" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaHasta" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                        <!-- /input-group -->

                                    </div>--%>
                                    <div class="form-group">
                                        <label class="col-md-4">
                                            Fecha Compra
                                        </label>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtFechaDesde" placeholder="Desde" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtFechaHasta" placeholder="Hasta" runat="server" class="form-control"></asp:TextBox>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RadioButton ID="RadioFechaCompra" Checked="true" runat="server" GroupName="fecha" />
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaDesde" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaHasta" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">
                                            Fecha Imputacion
                                        </label>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtFechaDesdeImp" placeholder="Desde" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtFechaHastaImp" placeholder="Hasta" runat="server" class="form-control"></asp:TextBox>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RadioButton ID="RadioFechaImputacion" runat="server" GroupName="fecha" />
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaDesdeImp" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaHastaImp" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Buscar Proveedor</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtCodProveedor" class="form-control" runat="server"></asp:TextBox>
                                        </div>

                                        <div class="col-md-2">
                                            <asp:LinkButton ID="btnBuscarProveedor" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" OnClick="btnBuscarProveedor_Click" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Proveedor</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListProveedor" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Sucursal</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListSucursal" runat="server" disabled="true" class="form-control" AutoPostBack="True" OnSelectedIndexChanged="DropListSucursal_SelectedIndexChanged"></asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-2">
                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListSucursal" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>--%>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label for="name" class="col-md-4">Punto de Venta</label>

                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ListPuntoVenta" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ControlToValidate="ListPuntoVenta" ID="RequiredFieldValidator2" runat="server" ErrorMessage="Seleccione PtoVenta" InitialValue="-1" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>

                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Tipo</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListTipo" runat="server" class="form-control">
                                                <asp:ListItem Value="0">Todos</asp:ListItem>
                                                <asp:ListItem Value="1">FC</asp:ListItem>
                                                <asp:ListItem Value="2">PRP</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListTipo" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>

                        </div>
                    </div>


                    <div class="modal-footer">
                        <asp:LinkButton ID="lbtnBuscar" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnBuscar_Click" ValidationGroup="BusquedaGroup" />
                    </div>
                </div>

            </div>
        </div>

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
                                        <asp:Label runat="server" ID="lblMensaje" Text="Esta seguro que desea eliminar las Ventas Seleccionadas?" Style="text-align: center"></asp:Label>
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

        <script>
            function abrirConfirmacion(valor) {
                document.getElementById('<%= txtMovimiento.ClientID %>').value = valor;
            }
        </script>

        <script>

            function abrirdialog() {
                document.getElementById('abreDialog').click();
            }

        </script>
        <link href="../../css/pages/reports.css" rel="stylesheet">
        <script src="../../Scripts/plugins/dataTables/jquery-2.0.0.js"></script>
        <script src="../../Scripts/plugins/dataTables/jquery.dataTables.min.js"></script>
        <script src="../../Scripts/plugins/dataTables/custom.tables.js"></script>


        <!-- Core Scripts - Include with every page -->
        <script src="../../Scripts/jquery-1.10.2.js"></script>
        <script src="../../Scripts/bootstrap.min.js"></script>
        <script src="../Scripts/plugins/metisMenu/jquery.metisMenu.js"></script>

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


            $(function () {
                $("#<%= txtFechaDesde.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            });

            $(function () {
                $("#<%= txtFechaHasta.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            });

        </script>

        <!-- Page-Level Plugin Scripts - Tables -->
        <script src="//cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>
        <script src="../../Scripts/plugins/dataTables/custom.tables.js"></script>
        <link href="//cdn.datatables.net/1.10.2/css/jquery.dataTables.css" rel="stylesheet" />


        <%--        <script>
            $(document).ready(function () {
                $('#dataTables-example').dataTable({ "bFilter": false, "bInfo": false, "bPaginate": false });
            });
        </script>

        <script type="text/javascript">
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endReq);
            function endReq(sender, args) {
                $('#dataTables-example').dataTable({ destroy: true, "bFilter": false, "bInfo": false, "bPaginate": false });
            }

        </script>--%>
    </div>
</asp:Content>
