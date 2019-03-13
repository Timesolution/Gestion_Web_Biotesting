<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OrdenesCompraF.aspx.cs" Inherits="Gestion_Web.Formularios.Compras.OrdenesCompraF" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">

        <div>

            <div class="col-md-12 col-xs-12">
                <div class="widget stacked">
                    <div class="stat">
                        <h5><i class="icon-map-marker"></i>Compras > Ordenes Compra</h5>
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
                                        <button type="button" class="btn btn-success dropdown-toggle" data-toggle="dropdown" id="btnAccion" runat="server">Accion<span class="caret"></span></button>
                                        <ul class="dropdown-menu">
                                            <li>
                                                <asp:LinkButton ID="lbtnAnular" runat="server" Enabled="false" data-toggle="modal" href="#modalConfirmacion">Anular</asp:LinkButton>
                                            </li>
                                            <asp:PlaceHolder runat="server" ID="phCambiarEstadoOC" Visible="false">
                                                <li>
                                                    <asp:LinkButton ID="ltbnCambiarEstado" runat="server" Visible="false" Enabled="false" data-toggle="modal" href="#modalCambiarEstado">Cambiar estado</asp:LinkButton>
                                                </li>
                                            </asp:PlaceHolder>
                                            <asp:PlaceHolder runat="server" ID="lbtnEntregasPH" Visible="false">
                                                <li class="dropdown-submenu">
                                                    <a tabindex="" href="#">Entregas</a>
                                                    <ul class="dropdown-menu">
                                                        <li>
                                                            <asp:LinkButton ID="lbtnAceptarEntregas" runat="server" OnClick="lbtnProcesarEntrega_Click">Entrega</asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lbtnRechazarEntregas" runat="server" data-toggle="modal" href="#modalConfirmacionRechazarEntrega">Rechazar Entrega</asp:LinkButton>
                                                        </li>
                                                    </ul>
                                                </li>
                                            </asp:PlaceHolder>
                                        </ul>
                                    </div>
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

                                        <a href="OrdenesCompraABM.aspx?a=1" class="btn btn-primary ui-tooltip" data-toggle="tooltip" title data-original-title="Agregar" style="width: 100%">
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
                                            <th style="width: 5%">Fecha</th>
                                            <th style="width: 10%">Fecha Entrega</th>
                                            <th style="width: 10%">Numero</th>
                                            <th style="width: 15%">Proveedor</th>
                                            <th style="width: 15%">Sucursal</th>
                                            <th style="width: 10%">Estado</th>
                                            <th style="width: 10%">Estado General</th>
                                            <th style="width: 15%"></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:PlaceHolder ID="phOrdenes" runat="server"></asp:PlaceHolder>
                                    </tbody>
                                </table>

                            </div>


                            <%--                                    </ContentTemplate>
                                    <Triggers>
                                    </Triggers>
                                </asp:UpdatePanel>--%>
                        </div>


                        <!-- /.content -->

                    </div>

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
                                    <asp:Label runat="server" ID="lblMensaje" Text="Esta seguro que desea eliminar las Ordenes de Compra seleccionadas?" Style="text-align: center"></asp:Label>
                                </h5>
                            </div>

                            <div class="col-md-3">
                                <asp:TextBox runat="server" ID="txtMovimiento" Text="0" Style="display: none"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button runat="server" ID="btnSi" Text="Eliminar" class="btn btn-danger" OnClick="btnSi_Click" />
                        <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="modalConfirmacionRechazarEntrega" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Confirmar Rechazo</h4>
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
                                    <asp:Label runat="server" ID="Label1" Text="Esta seguro que desea rechazar la Orden de Compra seleccionada?" Style="text-align: center"></asp:Label>
                                </h5>
                            </div>
                            <div class="col-md-3">
                                <asp:TextBox runat="server" ID="TextBox1" Text="0" Style="display: none"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button runat="server" ID="ConfirmarRechazarEntrega" Text="Rechazar" class="btn btn-danger" OnClick="ConfirmarRechazarEntrega_Click" />
                        <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="modalBusqueda" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">

                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Busqueda</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <fieldset>
                                <div class="form-group">
                                    <label class="col-md-3">Fecha Orden Compra</label>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtFechaDesde" placeholder="Desde" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtFechaHasta" placeholder="Hasta" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-md-1">
                                        <asp:RadioButton ID="RadioFechaOrdenCompra" Checked="true" runat="server" GroupName="fecha" />
                                    </div>
                                    <div class="col-md-1">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaDesde" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-1">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaHasta" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-3">Fecha Entrega</label>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtFechaEntregaDesde" placeholder="Desde" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtFechaEntregaHasta" placeholder="Hasta" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-md-1">
                                        <asp:RadioButton ID="RadioFechaEntrega" runat="server" GroupName="fecha" />
                                    </div>
                                    <div class="col-md-1">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaEntregaDesde" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-1">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaEntregaHasta" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Sucursal</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="DropListSucursal" disabled runat="server" class="form-control"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Proveedor</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="DropListProveedor" runat="server" class="form-control"></asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListProveedor" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Estado</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="DropListEstadoFiltro" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="DropListEstadoFiltro_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListEstadoFiltro" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Estado General</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="DropListEstadoGeneralFiltro" runat="server" class="form-control"></asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListEstadoGeneralFiltro" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <asp:PlaceHolder runat="server" ID="phDropListEstadosItemOC" Visible="false">
                                    <div class="form-group">
                                        <label class="col-md-4">Estado Item OC</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListOC_ItemEstados" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListOC_ItemEstados" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </asp:PlaceHolder>
                            </fieldset>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:LinkButton ID="lbtnBuscar" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" ValidationGroup="BusquedaGroup" OnClick="lbtnBuscar_Click" />
                    </div>
                </div>
            </div>
    </div>

    <div id="modalCambiarEstado" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Cambiar estado</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <div class="form-group">
                            <label class="col-md-3">Estados</label>
                            <div class="col-md-6">
                                <asp:DropDownList ID="DropListEstados" runat="server" class="form-control"></asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListEstados" InitialValue="-1" ValidationGroup="ImputarGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-3">Observaciones</label>
                            <div class="col-md-6">
                                <asp:TextBox ID="txtObservaciones" runat="server" class="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="modal-footer">
                    <asp:LinkButton ID="btnCambiarEstado" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" OnClick="btnCambiarEstado_Click" class="btn btn-success" ValidationGroup="ImputarGroup" />
                </div>
            </div>

        </div>
    </div>

    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Visible="false" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="80%">
    </rsweb:ReportViewer>

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
    <script src="../../Scripts/plugins/dataTables/jquery.dataTables.min.js"></script>
    <script src="../../Scripts/plugins/dataTables/custom.tables.js"></script>
    <link href="../../Scripts/plugins/dataTables/dataTables.css" rel="stylesheet" />

    <%--<script src="//code.jquery.com/jquery-1.9.1.js"></script>--%>
    <script src="//code.jquery.com/ui/1.10.4/jquery-ui.js"></script>

    <script>
        function pageLoad() {

            $("#<%= txtFechaDesde.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });

            $("#<%= txtFechaHasta.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });

            $("#<%= txtFechaEntregaDesde.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });

            $("#<%= txtFechaEntregaHasta.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        }
    </script>

    <script>
        $(function () {
            $("#<%= txtFechaDesde.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

        $(function () {
            $("#<%= txtFechaHasta.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

        $(function () {
            $("#<%= txtFechaEntregaDesde.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

        $(function () {
            $("#<%= txtFechaEntregaHasta.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

    </script>

</asp:Content>
