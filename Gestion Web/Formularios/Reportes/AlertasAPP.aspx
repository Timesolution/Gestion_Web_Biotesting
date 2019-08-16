<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AlertasAPP.aspx.cs" Inherits="Gestion_Web.Formularios.Reportes.AlertasAPP" %>

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
                    <div class="widget-content">
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 20%">
                                    <div class="btn-group">
                                        <button type="button" class="btn btn-success dropdown-toggle" data-toggle="dropdown" id="btnAccion" runat="server">Accion    <span class="caret"></span></button>
                                        <ul class="dropdown-menu">
                                            <li>
                                                <asp:LinkButton ID="btnCambiarEstado" runat="server">Cambiar estado</asp:LinkButton>
                                            </li>
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
                                        <a href="ComprasABM.aspx?a=1" class="btn btn-primary ui-tooltip" data-toggle="tooltip" title data-original-title="Agregar" style="width: 100%">
                                            <i class="shortcut-icon icon-plus"></i>
                                        </a>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
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
                            <asp:UpdatePanel ID="UpdatePanel4" UpdateMode="Always" runat="server">
                                <ContentTemplate>
                                    <div class="form-group">
                                        <label class="col-md-4">Fecha Alerta</label>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtFechaDesde" onchange="javascript:return ComprobacionFechaDesde()" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtFechaHasta" onchange="javascript:return ComprobacionFechaHasta()" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Buscar Cliente</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtCodCliente" class="form-control" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:LinkButton ID="btnBuscarCliente" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Cliente</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListCliente" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Buscar Vendedor/Distribuidor</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtVendedor" class="form-control" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:LinkButton ID="lbtnBuscarVendedor" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Vendedor/Distribuidor</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListVendedor" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Tipo Alerta</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListTipoAlerta" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:LinkButton ID="lbtnBuscar" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" ValidationGroup="BusquedaGroup" />
                    </div>
                </div>
            </div>
        </div>

        <div id="modalConfirmacion" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Confirmacion Cambiar estado</h4>
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
                                        <asp:Label runat="server" ID="lblMensaje" Text="Esta seguro que desea cambiar el estado de la alerta?" Style="text-align: center"></asp:Label>
                                    </h5>
                                </div>
                                <div class="col-md-3">
                                    <asp:TextBox runat="server" ID="txtMovimiento" Text="0" Style="display: none"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button runat="server" ID="btnSi" Text="Eliminar" class="btn btn-danger" />
                            <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <script src="../../../Scripts/libs/jquery-1.9.1.min.js"></script>
        <script src="../../../Scripts/libs/jquery-ui-1.10.0.custom.min.js"></script>
        <link href="../../../css/pages/reports.css" rel="stylesheet">
        <script src="//cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>
        <script src="//cdn.datatables.net/plug-ins/1.10.9/sorting/date-eu.js"></script>
        <script src="../../../Scripts/plugins/dataTables/custom.tables.js"></script>
        <link href="//cdn.datatables.net/1.10.2/css/jquery.dataTables.css" rel="stylesheet" />
        <script src="../../../Scripts/plugins/lightbox/jquery.lightbox.min.js"></script>
        <script src="../../../Scripts/plugins/msgbox/jquery.msgbox.min.js"></script>
        <script src="../../../scripts/demo/notifications.js"></script>
        <script src="../Vendedores/Comisiones/Comisiones.js" type="text/javascript"></script>

        <script>
            $(function ()
            {
                var fechaActual = ObtenerFechaActual();

                var controlFechaHasta = document.getElementById('<%= txtFechaHasta.ClientID %>');
                var controlFechaDesde = document.getElementById('<%= txtFechaDesde.ClientID %>');

                controlFechaDesde.value = fechaActual;
                controlFechaHasta.value = fechaActual;

                $("#<%= txtFechaDesde.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
                $("#<%= txtFechaHasta.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            });
        </script>

        <script type="text/javascript">
        function ComprobacionFechaHasta()
        {
            var fechaActual = ObtenerFechaActual();

            var controlFechaHasta = document.getElementById('<%= txtFechaHasta.ClientID %>');
            var controlFechaDesde = document.getElementById('<%= txtFechaDesde.ClientID %>');

            var pattern = /^([0-9]{2})\/([0-9]{2})\/([0-9]{4})$/;

            //si es invalida
            if (!pattern.test(controlFechaHasta.value))
            {
                controlFechaHasta.value = fechaActual;
                return false;
            }

            //si hasta es mas chico que desde
            var desde = InvertirDiaPorMes(controlFechaDesde.value);
            var hasta = InvertirDiaPorMes(controlFechaHasta.value);
            var hoy = InvertirDiaPorMes(fechaActual);
            if (desde > hasta)
            {
                controlFechaHasta.value = fechaActual;
                controlFechaDesde.value = fechaActual;
                return false;
            }

            //si hasta es mas grande que hoy
            if (hasta > hoy)
            {
                controlFechaHasta.value = fechaActual;
                return false;
            }
        };        

        function ComprobacionFechaDesde()
        {
            var fechaActual = ObtenerFechaActual();

            var controlFechaHasta = document.getElementById('<%= txtFechaHasta.ClientID %>');
            var controlFechaDesde = document.getElementById('<%= txtFechaDesde.ClientID %>');

            var pattern = /^([0-9]{2})\/([0-9]{2})\/([0-9]{4})$/;

            //si es invalida
            if (!pattern.test(controlFechaDesde.value))
            {
                controlFechaDesde.value = fechaActual;
                return false;
            }

            var desde = InvertirDiaPorMes(controlFechaDesde.value);
            var hasta = InvertirDiaPorMes(controlFechaHasta.value);
            //si es mas grande que la fecha hasta
            if (desde > hasta)
            {
                controlFechaDesde.value = fechaActual;
                controlFechaHasta.value = fechaActual;
                return false;
            }
        };
        </script>
    </div>
</asp:Content>
