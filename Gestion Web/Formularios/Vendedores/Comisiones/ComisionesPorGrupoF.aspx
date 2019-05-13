﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ComisionesPorGrupoF.aspx.cs" Inherits="Gestion_Web.Formularios.Vendedores.Comisiones.ComisionesPorGrupoF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <div class="col-md-12 col-xs-12">
            <div class="widget stacked">
                <div class="stat">
                    <h5><i class="icon-map-marker"></i>Maestros > Comisiones > Comisiones Por Vendedores</h5>
                </div>
                <div class="widget-header">
                    <i class="icon-wrench"></i>
                    <h3>Herramientas</h3>
                </div>
                <div class="widget-content">
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 10%">
                                <div class="btn-group">
                                    <%--<button type="button" class="btn btn-success dropdown-toggle" data-toggle="dropdown" id="btnAccion" runat="server">Accion<span class="caret"></span></button>--%>
                                    <ul class="dropdown-menu">
                                        <%--<li>
                                            <asp:LinkButton ID="lbtnRemesa" Visible="false" runat="server" OnClick="lbtnRemesa_Click">Nota de Remesa</asp:LinkButton>
                                        </li>--%>
                                    </ul>
                                </div>
                            </td>
                            <td style="width: 65%">
                                <h5>
                                    <asp:Label runat="server" ID="lblParametros" Text="" ForeColor="#cccccc"></asp:Label>
                                </h5>
                            </td>
                            <td style="width: 5%">
                                <div class="shortcuts" style="height: 100%">
                                    <a class="btn btn-primary" data-toggle="modal" data-placement="top" title="Tooltip on top" href="#modalBusqueda" style="width: 100%">
                                        <i class="shortcut-icon icon-filter"></i>
                                    </a>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="widget big-stats-container stacked">
                <div class="widget-content">
                    <div id="big_stats" class="cf">
                        <div class="stat">
                            <h4>Saldo</h4>
                            <asp:Label ID="lblSaldo" runat="server" Text="" class="value"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-12 col-xs-12">
            <div class="widget stacked widget-table action-table">
                <div class="widget-header">
                    <i class="icon-money" style="width: 2%"></i>
                    <h3 style="width: 75%">Comisiones</h3>
                </div>
                <div class="widget-content">
                    <div class="panel-body">
                        <div class="table-responsive">
                            <%--<a class="btn btn-info" style="display: none" data-toggle="modal" id="abreDialog" href="#Comentario">Agregar Tipo Cliente</a>--%>
                            <table class="table table-striped table-bordered table-hover" id="dataTables-example">
                                <thead>
                                    <tr>
                                        <th style="width: 10%">Fecha</th>
                                        <th style="width: 15%">Documento</th>
                                        <th style="width: 10%">Cod. Articulo</th>
                                        <th style="width: 15%">Descripcion</th>
                                        <th style="width: 10%">Vendedor</th>
                                        <th style="width: 10%">Neto Item</th>
                                        <th style="width: 5%">Comision</th>
                                        <th style="width: 15%">Total</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:PlaceHolder ID="phComisiones" runat="server"></asp:PlaceHolder>
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
                        <div class="form-group">
                            <label class="col-md-4">Desde</label>
                            <div class="col-md-6">
                                <asp:TextBox ID="txtFechaDesde"  runat="server" class="form-control"></asp:TextBox> <%--onKeyPress="javascript:FechaDesdeMenorAFechaHasta();"--%>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-4">Hasta</label>
                            <div class="col-md-6">
                                <asp:TextBox ID="txtFechaHasta" runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-4">Empresa</label>
                            <div class="col-md-6">
                                <asp:DropDownList ID="DropListEmpresa" runat="server" class="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-4">Sucursal</label>
                            <div class="col-md-6">
                                <asp:DropDownList ID="DropListSucursal" runat="server" class="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-4">Punto Venta</label>
                            <div class="col-md-6">
                                <asp:DropDownList ID="ListPuntoVenta" runat="server" class="form-control"></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbtnBuscar" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" />
                </div>
            </div>
        </div>
    </div>

    <%--<link href="../../css/pages/reports.css" rel="stylesheet">--%>
    <%--<script src="../../Scripts/plugins/dataTables/jquery-2.0.0.js"></script>
    <script src="../../Scripts/plugins/dataTables/jquery.dataTables.min.js"></script>
    <script src="../../Scripts/plugins/dataTables/custom.tables.js"></script>
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
    <script src="../../Scripts/plugins/msgbox/jquery.msgbox.min.js"></script>--%>
    <script src="Comisiones.js" type="text/javascript"></script>
    <script src="//code.jquery.com/ui/1.10.4/jquery-ui.js"></script>

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

            controlFechaDesde.addEventListener("change", ComprobacionFechaDesde);
            controlFechaHasta.addEventListener("change", ComprobacionFechaHasta);
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
            var desde = new Date(controlFechaDesde.value);
            var hasta = new Date(controlFechaHasta.value);

            if (desde.value > hasta.value)
            {
                controlFechaHasta.value = fechaActual;
                return false;
            }
            //si hasta es mas grande que hoy
            if (controlFechaHasta.value > fechaActual)
            {
                controlFechaHasta.value = fechaActual;
                return false;
            }
        };

        function ComprobacionFechaDesde(controlFechaDesde, controlFechaHasta)
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
            //si es mas grande que la fecha hasta
            if (controlFechaDesde.value > controlFechaHasta.value)
            {
                controlFechaDesde.value = fechaActual;
                return false;
            }
        };

    </script>

</asp:Content>
