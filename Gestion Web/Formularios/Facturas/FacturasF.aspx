<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FacturasF.aspx.cs" Inherits="Gestion_Web.Formularios.Facturas.FacturasF" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <%--<div class="container">--%>
    <div>

        <div class="col-md-12 col-xs-12">
            <div class="widget stacked">

                <div class="stat">
                    <h5><i class="icon-map-marker"></i>Ventas > Ventas</h5>
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
                                            <asp:LinkButton ID="lbtnAnular" runat="server" Visible="False" Enabled="false" data-toggle="modal" href="#modalConfirmacion">Anular</asp:LinkButton>
                                        </li>
                                        <li>
                                            <asp:LinkButton ID="lbtnRemitir" runat="server" Visible="false" OnClick="lbtnRemitir_Click">Remitir</asp:LinkButton>
                                        </li>
                                        <li>
                                            <asp:LinkButton ID="lbtnNotaCredito" runat="server" OnClick="lbtnNotaCredito_Click">Nota de Credito</asp:LinkButton>
                                        </li>
                                        <li>
                                            <asp:LinkButton ID="lbtnGuiaDespacho" runat="server" OnClick="lbtnGuiaDespacho_Click">Guia de Despacho</asp:LinkButton>
                                        </li>
                                        <li>
                                            <asp:LinkButton ID="lbtnRefacturar" runat="server" Visible="false" OnClick="lbtnRefacturar_Click">Facturar PRP</asp:LinkButton>
                                        </li>
                                        <li>
                                            <asp:LinkButton ID="lbtnEnviar" runat="server" OnClick="lbtnEnviar_Click">Enviar FC Por Mail</asp:LinkButton>
                                        </li>
                                        <li>
                                            <asp:LinkButton ID="lbtnEnviarFacturaMailPorCliente" runat="server" Visible="true" OnClick="lbtnEnviarFacturaMailPorCliente_Click" ToolTip="Envia cada factura al cliente correspondiente">Enviar FC Mail por Cliente</asp:LinkButton>
                                        </li>
                                        <li>
                                            <asp:LinkButton ID="lbtnOrdenReparacion" runat="server" Visible="false" OnClick="lbtnOrdenReparacion_Click">Orden de Reparacion</asp:LinkButton>
                                        </li>
                                        <li>
                                            <asp:LinkButton ID="lbtnReimprimirFiscal" runat="server" Visible="false" OnClick="lbtnReimprimirFiscal_Click">Reimprimir Fiscal</asp:LinkButton>
                                        </li>
                                        <li>
                                            <asp:LinkButton ID="lbtnPatentamiento" runat="server" Visible="false" data-toggle="modal" href="#modalPatentamiento">Patentamiento</asp:LinkButton>
                                        </li>
                                        <li>
                                            <asp:LinkButton ID="lbtnPagosProgramados" runat="server" OnClick="lbtnPagosProgramados_Click">Cronograma de Pagos</asp:LinkButton>
                                        </li>
                                        <asp:PlaceHolder runat="server" ID="phEditarFC" Visible="false">
                                            <li>
                                                <asp:LinkButton ID="btnEditarFC" runat="server" OnClick="btnEditarDatosFC_Click">Editar FC</asp:LinkButton>
                                            </li>
                                        </asp:PlaceHolder>
                                        <asp:PlaceHolder runat="server" ID="phAgregarFC" Visible="false">
                                            <li>
                                                <asp:LinkButton ID="lbtnAgregarFC" runat="server" OnClick="lbtnAgregarFC_Click">Agregar FC</asp:LinkButton>
                                            </li>
                                        </asp:PlaceHolder>
                                        <asp:PlaceHolder runat="server" ID="phFormulario12" Visible="true">
                                            <li>
                                                <asp:LinkButton ID="btnForm12" runat="server" OnClick="btnForm12_Click">Formulario 12</asp:LinkButton>
                                            </li>
                                        </asp:PlaceHolder>
                                        <asp:PlaceHolder runat="server" ID="phNotaDebitoCreditoDiferenciaCambio" Visible="false">
                                            <li>
                                                <asp:LinkButton runat="server" ID="lbtnDebitoCreditoDiferenciaCambio" OnClick="lbtnDebitoCreditoDiferenciaCambio_Click">ND/C Diferencia Cambio</asp:LinkButton>
                                            </li>
                                        </asp:PlaceHolder>
                                        <asp:PlaceHolder runat="server" ID="phImprimirTicketDeCambio" Visible="true">
                                            <li>
                                                <asp:LinkButton ID="lbtnImprimirTicketDeCambio" runat="server" OnClick="lbtnImprimirTicketDeCambio_Click">Imprimir Ticket De Cambio</asp:LinkButton>
                                            </li>
                                        </asp:PlaceHolder>
                                        <li>
                                            <asp:LinkButton ID="lbtnImprimirFC_En_Otra_Divisa" runat="server" OnClick="lbtnImprimirFC_En_Otra_Divisa_Click">Imprimir FC en otra divisa</asp:LinkButton>
                                        </li>
                                    </ul>
                                </div>
                                <!-- /btn-group -->
                            </td>
                            <td style="width: 60%">
                                <h5>
                                    <asp:Label runat="server" ID="lblParametros" Text="" ForeColor="#cccccc"></asp:Label>
                                </h5>
                            </td>

                            <td style="width: 5%">
                                <div class="shortcuts" style="height: 100%">
                                    <asp:LinkButton ID="lbImpresion" class="btn btn-primary" runat="server" Text="<span class='shortcut-icon icon-print'></span>" Visible="false" Style="width: 100%" OnClick="lbImpresion_Click" />
                                </div>
                            </td>
                            <td style="width: 5%">
                                <div class="shortcuts" style="height: 100%">
                                    <a class="btn btn-primary" data-toggle="modal" href="#modalNro" style="width: 100%" data-toggle="tooltip" title="Busqueda de facturas">
                                        <i class="shortcut-icon icon-search"></i>
                                    </a>
                                </div>
                            </td>
                            <td style="width: 2%">
                                <div class="btn-group pull-right" style="width: 100%">
                                    <button type="button" class="btn btn-primary dropdown-toggle ui-tooltip" title data-original-title="Listados" data-toggle="dropdown">
                                        <i class="shortcut-icon icon-print"></i>&nbsp
                                        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                                        <span class="caret"></span>
                                    </button>
                                    <ul class="dropdown-menu dropdown-menu-right" role="menu" aria-labelledby="dropdownMenu">
                                        <li class="dropdown-submenu dropdown-menu-right"><a tabindex="-1" href="#">IVA Ventas</a>
                                            <ul class="dropdown-menu">
                                                <li>
                                                    <asp:LinkButton ID="btnExportarExcel" runat="server" OnClick="btnExportarExcel_Click">
                                                        <i class="fa fa-file-excel-o" aria-hidden="true"></i>
                                                        &nbsp Exportar
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="btnImprimirIvaVentas" runat="server" OnClick="btnImprimirIvaVentas_Click">
                                                        <i class="fa fa-file-pdf-o" aria-hidden="true"></i>
                                                        &nbsp Imprimir
                                                    </asp:LinkButton>
                                                </li>
                                            </ul>
                                        </li>
                                        <li class="dropdown-submenu dropdown-menu-right"><a tabindex="-1" href="#">Detalle Facturas</a>
                                            <ul class="dropdown-menu">
                                                <li>
                                                    <asp:LinkButton ID="btnExportarDetalle2" runat="server" OnClick="btnExportarDetalle2_Click">
                                                        <i class="fa fa-file-excel-o" aria-hidden="true"></i>
                                                        &nbsp Exportar
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="btnImprimirDetalle2" runat="server" OnClick="btnImprimirDetalle2_Click">
                                                        <i class="fa fa-file-pdf-o" aria-hidden="true"></i>
                                                        &nbsp Imprimir                                                        
                                                    </asp:LinkButton>
                                                </li>
                                            </ul>
                                        </li>
                                        <li class="dropdown-submenu dropdown-menu-right"><a tabindex="-1" href="#">Reporte IIBB</a>
                                            <ul class="dropdown-menu">
                                                <li>
                                                    <asp:LinkButton ID="btnExportarIIBB" runat="server" OnClick="btnExportarIIBB_Click">
                                                        <i class="fa fa-file-excel-o" aria-hidden="true"></i>
                                                        &nbsp Exportar
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="btnImprimirIIBB" runat="server" OnClick="btnImprimirIIBB_Click">
                                                        <i class="fa fa-file-pdf-o" aria-hidden="true"></i>
                                                        &nbsp Imprimir
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="btnSolicitarInformeIIBB" runat="server" OnClick="btnSolicitarInformeIIBB_Click">
                                                        <i class="fa fa-file-text-o" aria-hidden="true"></i>
                                                        &nbsp Solicitar Informe
                                                    </asp:LinkButton>
                                                </li>
                                            </ul>
                                        </li>
                                        <li class="dropdown-submenu dropdown-menu-right"><a tabindex="-1" href="#">Detalle Ventas</a>
                                            <ul class="dropdown-menu">
                                                <li>
                                                    <asp:LinkButton ID="btnExportarVentas" runat="server" OnClick="btnExportarVentas_Click">
                                                        <i class="fa fa-file-excel-o" aria-hidden="true"></i>
                                                        &nbsp Exportar
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="btnImprimirVentas" runat="server" OnClick="btnImprimirVentas_Click">
                                                        <i class="fa fa-file-pdf-o" aria-hidden="true"></i>
                                                        &nbsp Imprimir
                                                    </asp:LinkButton>
                                                </li>
                                            </ul>
                                        </li>

                                        <li class="dropdown-submenu dropdown-menu-right"><a tabindex="-1" href="#">Detalle Ventas Con Solicitudes</a>
                                            <ul class="dropdown-menu">
                                                <li>
                                                    <asp:LinkButton ID="btnExportarVentasConSolicitudes" runat="server" OnClick="btnExportarVentasConSolicitudes_Click">
                                                        <i class="fa fa-file-excel-o" aria-hidden="true"></i>
                                                        &nbsp Exportar
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="btnImprimirVentasConSolicitudes" runat="server" OnClick="btnImprimirVentasConSolicitudes_Click">
                                                        <i class="fa fa-file-pdf-o" aria-hidden="true"></i>
                                                        &nbsp Imprimir
                                                    </asp:LinkButton>
                                                </li>
                                            </ul>
                                        </li>

                                        <li class="dropdown-submenu">
                                            <a tabindex="-1" data-toggle="modal" href="#modalDescuentos">Detalle descuentos</a>
                                        </li>
                                        <li class="dropdown-submenu dropdown-menu-right"><a tabindex="-1" href="#">Detalle Ventas x Vendedor</a>
                                            <ul class="dropdown-menu">
                                                <li>
                                                    <asp:LinkButton ID="btnExportarVentasVendedor" runat="server" OnClick="btnExportarVentasVendedor_Click">
                                                        <i class="fa fa-file-excel-o" aria-hidden="true"></i>
                                                        &nbsp Exportar
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="btnImprimirVentasVendedor" runat="server" OnClick="btnImprimirVentasVendedor_Click">
                                                        <i class="fa fa-file-pdf-o" aria-hidden="true"></i>
                                                        &nbsp Imprimir
                                                    </asp:LinkButton>
                                                </li>
                                            </ul>
                                        </li>

                                        <li class="dropdown-submenu dropdown-menu-right"><a tabindex="-1" href="#">Reporte Ventas x Vendedor x Categoria</a>
                                            <ul class="dropdown-menu">
                                                <li>
                                                    <asp:LinkButton ID="btnExportarReporteVentasVendedor" runat="server" OnClick="btnExportarReporteVentasVendedor_Click">
                                                        <i class="fa fa-file-excel-o" aria-hidden="true"></i>
                                                        &nbsp Exportar
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="btnImprimirReporteVentasVendedor" runat="server" OnClick="btnImprimirReporteVentasVendedor_Click">
                                                        <i class="fa fa-file-pdf-o" aria-hidden="true"></i>
                                                        &nbsp Imprimir
                                                    </asp:LinkButton>
                                                </li>
                                            </ul>
                                        </li>

                                        <li class="dropdown-submenu dropdown-menu-right"><a tabindex="-1" href="#">Total Ventas x Sucursal</a>
                                            <ul class="dropdown-menu">
                                                <li>
                                                    <asp:LinkButton ID="btnExportarVentasSucursal" runat="server" OnClick="btnExportarVentasSucursal_Click">
                                                       <i class="fa fa-file-excel-o" aria-hidden="true"></i>
                                                        &nbsp Exportar
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="btnImprimirVentasSucursal" runat="server" OnClick="btnImprimirVentasSucursal_Click">
                                                        <i class="fa fa-file-pdf-o" aria-hidden="true"></i>
                                                        &nbsp Imprimir
                                                    </asp:LinkButton>
                                                </li>
                                            </ul>
                                        </li>

                                        <li class="dropdown-submenu dropdown-menu-right"><a tabindex="-1" href="#">Ventas x Lista Precio</a>
                                            <ul class="dropdown-menu">
                                                <li>
                                                    <asp:LinkButton ID="btnExportarVentasListaP" runat="server" OnClick="btnExportarVentasListaP_Click">
                                                       <i class="fa fa-file-excel-o" aria-hidden="true"></i>
                                                        &nbsp Exportar
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="btnImprimirVentasListaP" runat="server" OnClick="btnImprimirVentasListaP_Click">
                                                        <i class="fa fa-file-pdf-o" aria-hidden="true"></i>
                                                        &nbsp Imprimir
                                                    </asp:LinkButton>
                                                </li>
                                            </ul>
                                        </li>
                                        <li class="dropdown-submenu dropdown-menu-right"><a tabindex="-1" href="#">Ventas x Forma Pago</a>
                                            <ul class="dropdown-menu">
                                                <li>
                                                    <asp:LinkButton ID="btnExportarPantalla" runat="server" OnClick="btnExportarPantalla_Click">
                                                        <i class="fa fa-file-excel-o" aria-hidden="true"></i>
                                                        &nbsp Exportar
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="btnImprimirPantalla" runat="server" OnClick="btnImprimirPantalla_Click">
                                                        <i class="fa fa-file-pdf-o" aria-hidden="true"></i>
                                                        &nbsp Imprimir
                                                    </asp:LinkButton>
                                                </li>
                                            </ul>
                                        </li>
                                        <li class="dropdown-submenu dropdown-menu-right"><a tabindex="-1" href="#">Citi ventas</a>
                                            <ul class="dropdown-menu">
                                                <li>
                                                    <asp:LinkButton ID="btnCitiVentas" runat="server" OnClick="btnCitiVentas_Click">
                                                        <i class="fa fa-file-excel-o" aria-hidden="true"></i>
                                                        &nbsp Exportar
                                                    </asp:LinkButton>
                                                </li>
                                            </ul>
                                        </li>
                                        <li class="dropdown-submenu dropdown-menu-right"><a tabindex="-1" href="#">Percepciones</a>
                                            <ul class="dropdown-menu">
                                                <li>
                                                    <asp:LinkButton ID="btnPercepciones" runat="server" OnClick="btnPercepciones_Click">
                                                        <i class="fa fa-file-excel-o" aria-hidden="true"></i>
                                                        &nbsp Exportar
                                                    </asp:LinkButton>
                                                </li>
                                            </ul>
                                        </li>
                                        <li class="dropdown-submenu dropdown-menu-right"><a tabindex="-1" href="#">Todo</a>
                                            <ul class="dropdown-menu">
                                                <li>
                                                    <asp:LinkButton ID="lbtnImprimirTodo" runat="server" OnClick="lbtnImprimirTodo_Click">
                                                        <i class="fa fa-file-excel-o" aria-hidden="true"></i>
                                                        &nbsp Imprimir
                                                    </asp:LinkButton>
                                                </li>
                                            </ul>
                                        </li>
                                    </ul>
                                </div>

                            </td>

                            <td style="width: 5%">
                                <div class="shortcuts" style="height: 100%">



                                    <a class="btn btn-primary" data-toggle="modal" href="#modalBusqueda" style="width: 100%">
                                        <i class="shortcut-icon icon-filter"></i>
                                    </a>
                                </div>
                            </td>
                            <td style="width: 5%">
                                <div class="shortcuts" style="height: 100%">

                                    <a href="ABMFacturas.aspx" class="btn btn-primary ui-tooltip" data-toggle="tooltip" title data-original-title="Agregar" style="width: 100%">
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
        <!-- /span12 -->
        <div class="col-md-12 col-xs-12">
            <div class="widget widget-table">
                <div class="widget-header">
                    <i class="icon-th-list" style="width: 2%"></i>
                    <h3 style="width: 75%">Facturas
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
                                            <th>Fecha</th>
                                            <th>Tipo</th>
                                            <th>Numero</th>
                                            <th>Razon Social</th>
                                            <th style="text-align: right;">IIBB</th>
                                            <th style="text-align: right;">Neto</th>
                                            <th style="text-align: right;">IVA</th>
                                            <th style="text-align: right;">P. IIBB</th>
                                            <th style="text-align: right;">P. IVA CF</th>
                                            <th style="text-align: right;">Total</th>
                                            <th></th>
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

        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Visible="false" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="80%">
        </rsweb:ReportViewer>

        <div id="modalBusqueda" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">

                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Busqueda</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <div class="form-group">
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

                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Empresa</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListEmpresa" runat="server" disabled class="form-control" AutoPostBack="true" OnSelectedIndexChanged="DropListEmpresa_SelectedIndexChanged"></asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListEmpresa" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Sucursal</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListSucursal" runat="server" disabled class="form-control"></asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListSucursal" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Cod Cliente</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtCodCliente" class="form-control" runat="server"></asp:TextBox>
                                        </div>

                                        <div class="col-md-2">
                                            <asp:LinkButton ID="btnBuscarCod" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" OnClick="btnBuscarCod_Click" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Cliente</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListClientes" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Tipo Cliente</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListTipoCliente" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Tipo</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListTipo" runat="server" class="form-control">
                                                <asp:ListItem Value="0">Ambos</asp:ListItem>
                                                <asp:ListItem Value="1">FC</asp:ListItem>
                                                <asp:ListItem Value="2">PRP</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListTipo" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Documento</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListDocumento" runat="server" class="form-control">
                                                <asp:ListItem Value="0">Todos</asp:ListItem>
                                                <asp:ListItem Value="1">Factura A</asp:ListItem>
                                                <asp:ListItem Value="2">Factura B</asp:ListItem>
                                                <asp:ListItem Value="24">Factura E</asp:ListItem>
                                                <asp:ListItem Value="4">Nota de Debito A</asp:ListItem>
                                                <asp:ListItem Value="5">Nota de Debito B</asp:ListItem>
                                                <asp:ListItem Value="9">Nota de Credito A</asp:ListItem>
                                                <asp:ListItem Value="8">Nota de Credito B</asp:ListItem>
                                                <asp:ListItem Value="11">Nota de Credito PRP</asp:ListItem>
                                                <asp:ListItem Value="12">Nota de Debito PRP</asp:ListItem>
                                                <asp:ListItem Value="99">NOTAS DE CREDITO</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListDocumento" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Lista Precio</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListListas" runat="server" class="form-control">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Formas Pago</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListFormasPago" runat="server" class="form-control">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Vendedor</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListVendedor" runat="server" class="form-control">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Mostrar anuladas </label>
                                        <div class="col-md-6">
                                            <asp:CheckBox ID="chkAnuladas" runat="server" />
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

        <div id="modalPagosProgramados" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">

                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Pagos</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                <ContentTemplate>

                                    <div class="form-group">

                                        <label class="col-md-4">Total</label>
                                        <div class="col-md-4">
                                            <div class="input-group">
                                                <span class="input-group-addon">$</span>
                                                <asp:TextBox ID="txtTotalPago" runat="server" class="form-control" disabled></asp:TextBox>
                                            </div>

                                            <!-- /input-group -->
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-md-4">Restan</label>
                                        <div class="col-md-4">
                                            <div class="input-group">
                                                <span class="input-group-addon">$</span>
                                                <asp:TextBox ID="txtRestaPago" runat="server" class="form-control" disabled></asp:TextBox>
                                            </div>
                                            <!-- /input-group -->
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-md-4">Fecha</label>
                                        <div class="col-md-4">
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="shortcut-icon icon-calendar"></i></span>
                                                <asp:TextBox ID="txtFechaPago" runat="server" class="form-control"></asp:TextBox>
                                            </div>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-4">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaPago" ValidationGroup="BusquedaGroup1" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-md-4">Importe</label>
                                        <div class="col-md-4">
                                            <div class="input-group">
                                                <span class="input-group-addon">$</span>
                                                <asp:TextBox ID="txtImportePago" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                            </div>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-1">
                                            <asp:LinkButton ID="lbtnAgregarImporte" class="btn btn-info" ValidationGroup="BusquedaGroup1" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" Visible="true" OnClick="lbtnAgregarImporte_Click" />
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator29" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtImportePago" ValidationGroup="BusquedaGroup1" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>

                                    </div>

                                    <div class="form-group">
                                        <label class="col-md-4">Observacion</label>
                                        <div class="col-md-8" style="height: inherit">
                                            <asp:TextBox ID="txtObservacionPago" runat="server" EnableViewState="false" TextMode="MultiLine" Rows="10" BackColor="LightYellow" class="form-control"></asp:TextBox>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <asp:Label runat="server" Text="" Visible="false" ID="lblAvisoImporte" ForeColor="Red" Font-Bold="true"></asp:Label>
                                    </div>
                                    <%--<div class="form-group">
                                        <label class="col-md-4">Formas Pago</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropDownList8" runat="server" class="form-control">
                                            </asp:DropDownList>
                                        </div>
                                    </div>--%>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <div role="form" class="form-horizontal col-md-12">
                                <asp:UpdatePanel ID="UpdatePanel7" UpdateMode="Always" runat="server">
                                    <ContentTemplate>
                                        <div class="form-group">
                                            <table class="table table-striped table-bordered">
                                                <thead>
                                                    <tr>
                                                        <th>Fecha</th>
                                                        <th>Importe</th>
                                                        <th>Observacion</th>
                                                        <th></th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <asp:PlaceHolder ID="phPagosProgramables" runat="server"></asp:PlaceHolder>
                                                </tbody>
                                            </table>
                                        </div>
                                        <%--<div class="form-group">
                                        <div class="col-md-4">
                                            <asp:LinkButton ID="lbtnConfirmarPago" class="btn btn-success" runat="server" Visible="false" Text="Confirmar pagos" OnClick="lbtnConfirmarPago_Click" />
                                        </div>
                                        <div class="col-md-4">
                                            <asp:LinkButton ID="lbtnCancelarPago" class="btn btn-default" runat="server" Visible="false" Text="Limpiar pagos" OnClick="lbtnCancelarPago_Click" />
                                        </div>
                                    </div>--%>
                                    </ContentTemplate>
                                    <Triggers>
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>


                        </div>
                    </div>


                    <div class="modal-footer">
                        <asp:HiddenField ID="idFactura" runat="server" />
                        <asp:HiddenField ID="hiddenTotalPago" runat="server" />
                        <asp:HiddenField ID="hiddenEditarPago" runat="server" />
                        <asp:LinkButton ID="btnAgregarPago" runat="server" Text="Generar" class="btn btn-success" OnClick="btnAgregarPago_Click" />
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
                        </div>


                        <div class="modal-footer">
                            <asp:Button runat="server" ID="btnSi" Text="Eliminar" class="btn btn-danger" OnClick="btnSi_Click" />
                            <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>

                        </div>
                    </div>
                </div>
            </div>
        </div>


        <div id="modalConfirmacionEnvioMailPorCliente" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Confirmacion de Envio de E-Mails</h4>
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
                                        <asp:Label runat="server" ID="Label1" Text="Esta seguro que desea enviar los mails a cada cliente ?" Style="text-align: center"></asp:Label>
                                    </h5>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button runat="server" ID="btnSiEnviarMailPorCliente" Text="Enviar" class="btn btn-success" OnClick="btnSiEnviarMailPorCliente_Click" OnClientClick="this.disabled = true; this.value = 'Enviando...';" UseSubmitBehavior="false" />
                            <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div id="modalEnvioMail" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Datos de envio correo</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <div class="form-group">
                                <label class="col-md-3">Destinatario</label>
                                <div class="col-md-8">
                                    <div class="input-group">
                                        <span class="input-group-addon">@</span>
                                        <asp:TextBox ID="txtEnvioMail" runat="server" class="form-control" />
                                    </div>
                                </div>
                                <div class="col-md-1">
                                    <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="txtEnvioMail" runat="server" ForeColor="Red" Font-Bold="true" SetFocusOnError="true" ValidationGroup="EnvioGroup" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-3">Destinatario 2</label>
                                <div class="col-md-8">
                                    <div class="input-group">
                                        <span class="input-group-addon">@</span>
                                        <asp:TextBox ID="txtEnvioMail2" runat="server" class="form-control" />
                                    </div>
                                </div>
                            </div>
                            <asp:TextBox runat="server" ID="txtIdEnvioFC" Text="0" Style="display: none"></asp:TextBox>
                        </div>
                        <div class="modal-footer">
                            <asp:LinkButton ID="lbtnEnviarMail" runat="server" Text="<span class='shortcut-icon fa fa-paper-plane'></span>" class="btn btn-success" OnClick="lbtnEnviarMail_Click" ValidationGroup="EnvioGroup"></asp:LinkButton>
                        </div>
                    </div>

                </div>
            </div>
        </div>

        <div id="modalNotaDebitoCreditoDiferenciaCambio" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Generar Nota Debito/Credito por Diferencia de Cambio</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <asp:UpdatePanel runat="server" ID="UpdatePanelNotaDebitoCreditoDiferenciaCambio" UpdateMode="Always">
                                <ContentTemplate>
                                    <asp:Label runat="server" ID="lblIdFacturaNotaDebitoCreditoDiferenciaCambio" Visible="false"></asp:Label>
                                    <div class="form-group">
                                        <label class="col-md-6">Tipo de Cambio original</label>
                                        <asp:Label runat="server" class="col-md-6" ID="lblTipoCambioOriginalNotaDebitoCreditoDiferenciaCambio" Style="text-align: right;"></asp:Label>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-6">Tipo de Cambio nuevo</label>
                                        <div class="input-group col-md-6">
                                            <span class="input-group-addon">$</span>
                                            <asp:TextBox ID="txtNuevoCambioNotaDebitoCreditoDiferenciaCambio" Text="0" runat="server" Style="text-align: right;" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" ID="lblMensajeDiferenciaCambio"></asp:Label>
                                        <asp:Label runat="server" ID="lblDiferenciaCambio"></asp:Label>
                                    </div>

                                    <div class="modal-footer">
                                        <asp:Button runat="server" ID="btnCalcularDiferenciaCambio" class="btn btn-primary" Text="Calcular" OnClick="lbtnCalcularDiferenciaCambio_Click" />
                                        <asp:Button runat="server" ID="btnGenerarNotaDebitoCreditoDiferenciaCambio" class="btn btn-success" Text="Generar" disabled OnClick="lbtnGenerarNotaDebitoCreditoDiferenciaCambio_Click" />
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div id="modalPatentamiento" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Generar Patentamiento</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <div class="form-group">
                                <label class="col-md-4">Proveedor</label>
                                <div class="col-md-6">
                                    <h5>
                                        <asp:DropDownList ID="dropDownListProveedoresPatentamiento" runat="server" InitialValue="-1" class="form-control"></asp:DropDownList>
                                    </h5>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Total Patentamiento</label>
                                <div class="input-group col-md-6">
                                    <span class="input-group-addon">$</span>
                                    <asp:TextBox ID="txtTotalPatentamiento" Text="0" runat="server" Style="text-align: right;" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnGenerarPatentamiento" runat="server" Text="Generar" class="btn btn-success" OnClick="btnGenerarPatentamiento_Click" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div id="modalFacturar" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Confirmar facturacion</h4>
                    </div>
                    <div class="modal-body">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <div role="form" class="form-horizontal col-md-12">
                                    <div class="form-group">
                                        <div class="col-md-12">
                                            <h4>
                                                <asp:Label runat="server" ID="lblNroFact" Style="text-align: center"></asp:Label>
                                                <asp:Label runat="server" ID="lblIdFact" Style="text-align: center; display: none;"></asp:Label>
                                            </h4>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Cod Cliente</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtClienteFacturarPRP" class="form-control" runat="server"></asp:TextBox>
                                        </div>

                                        <div class="col-md-2">
                                            <asp:LinkButton ID="btnClienteFacturarPRP" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" OnClick="btnClienteFacturarPRP_Click" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Cliente</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="listClienteFacturarPRP" runat="server" AutoPostBack="true" class="form-control" OnSelectedIndexChanged="listClienteFacturarPRP_SelectedIndexChanged"></asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-3">
                                            <h4>Cliente:
                                            </h4>
                                        </div>
                                        <div class="col-md-5">
                                            <h5>
                                                <asp:Label runat="server" ID="lblCliente" Style="text-align: center"></asp:Label>
                                                <asp:Label runat="server" ID="lblIdCliente" Style="text-align: center; display: none;"></asp:Label>
                                            </h5>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-3">
                                            <h4>CUIT: 
                                            </h4>
                                        </div>
                                        <div class="col-md-6">
                                            <h5>
                                                <asp:TextBox ID="txtCUITfact" runat="server" class="form-control" />

                                            </h5>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtCUITfact" ValidationGroup="RefactGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-3">
                                            <h4>Iva: 
                                            </h4>
                                        </div>
                                        <div class="col-md-6">
                                            <h5>
                                                <asp:DropDownList ID="ListIvaFact" runat="server" class="form-control"></asp:DropDownList>
                                            </h5>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="ListIvaFact" InitialValue="-1" ValidationGroup="RefactGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-3">
                                            <h4>Direccion: 
                                            </h4>
                                        </div>
                                        <div class="col-md-6">
                                            <h5>
                                                <asp:TextBox ID="txtDirFact" runat="server" class="form-control" />
                                                <asp:Label ID="lblIdDirFact" runat="server" Text="0" Style="display: none;" />
                                            </h5>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtDirFact" ValidationGroup="RefactGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-3">
                                            <h4>Provincia: 
                                            </h4>
                                        </div>
                                        <div class="col-md-6">
                                            <h5>
                                                <asp:DropDownList ID="ListProvinciaFact" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ListProvinciaFact_SelectedIndexChanged" />
                                            </h5>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-3">
                                            <h4>Localidad: 
                                            </h4>
                                        </div>
                                        <div class="col-md-6">
                                            <h5>
                                                <asp:DropDownList ID="ListLocalidadFact" runat="server" class="form-control" />
                                            </h5>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="col-md-3">
                                            <h4>Neto: 
                                            </h4>
                                        </div>
                                        <div class="col-md-6" style="text-align: right">
                                            <h5>
                                                <asp:Label runat="server" ID="lblNetoFact" Style="text-align: center"></asp:Label>
                                            </h5>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-3">
                                            <h4>Iva: 
                                            </h4>
                                        </div>
                                        <div class="col-md-6" style="text-align: right">
                                            <h5>
                                                <asp:Label runat="server" ID="lblIvaFact" Style="text-align: center"></asp:Label>
                                            </h5>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-3">
                                            <h4>Total: 
                                            </h4>
                                        </div>
                                        <div class="col-md-6" style="text-align: right">
                                            <h5>
                                                <asp:Label runat="server" ID="lblTotalFact" Style="text-align: center"></asp:Label>
                                            </h5>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="modal-footer">
                        <asp:Button runat="server" ID="btnSiFacturar" Text="Facturar" class="btn btn-success" ValidationGroup="RefactGroup" OnClick="btnSiFacturar_Click" />
                        <asp:Button runat="server" ID="btnSiEditarPRP" Text="Editar PRP" class="btn btn-success" ValidationGroup="RefactGroup" OnClick="btnSiEditarPRP_Click" />
                        <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                    </div>

                </div>
            </div>
        </div>

        <div id="modalDescuentos" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Confirmar datos</h4>
                    </div>
                    <div class="modal-body">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <div role="form" class="form-horizontal col-md-12">
                                    <div class="form-group">
                                        <label class="col-md-4">Desde</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtFechaDesdeDto" runat="server" class="form-control" />
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaDesdeDto" ValidationGroup="DescuentosGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Hasta</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtFechaHastaDto" runat="server" class="form-control" />
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaHastaDto" ValidationGroup="DescuentosGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Empresa</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ListEmpresaDto" runat="server" disabled class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ListEmpresaDto_SelectedIndexChanged" />
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="<h3>*</h3>" InitialValue="-1" ControlToValidate="ListEmpresaDto" ValidationGroup="DescuentosGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Sucursal</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ListSucursalesDto" runat="server" disabled class="form-control" />
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="<h3>*</h3>" InitialValue="-1" ControlToValidate="ListSucursalesDto" ValidationGroup="DescuentosGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Listas de precio</label>
                                        <asp:CheckBoxList ID="chkListListas" runat="server" RepeatLayout="table" RepeatColumns="2" RepeatDirection="vertical">
                                        </asp:CheckBoxList>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="modal-footer">
                        <asp:LinkButton ID="btnExportarDescuentos" runat="server" class="btn btn-success" OnClick="btnExportarDescuentos_Click" ValidationGroup="DescuentosGroup">
                            <i class="fa fa-file-excel-o" aria-hidden="true"></i>
                            &nbsp Exportar
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnImprimirDescuentos" runat="server" class="btn btn-success" OnClick="btnImprimirDescuentos_Click" ValidationGroup="DescuentosGroup">
                            <i class="fa fa-file-pdf-o" aria-hidden="true"></i>
                            &nbsp Imprimir
                        </asp:LinkButton>
                        <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                    </div>

                </div>
            </div>
        </div>

        <div id="modalEditarFactura" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Editar Factura</h4>
                    </div>
                    <div class="modal-body">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <div role="form" class="form-horizontal col-md-12">
                                    <div class="form-group">
                                        <div class="col-md-3">
                                            <label>
                                                Numero:
                                           
                                            </label>
                                        </div>
                                        <div class="col-md-5">
                                            <asp:Label runat="server" ID="lblNroFactura" Style="text-align: center"></asp:Label>
                                            <asp:Label runat="server" ID="lblIdFactura" Style="text-align: center; display: none;"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-3">
                                            <label>
                                                Fecha:
                                           
                                            </label>
                                        </div>
                                        <div class="col-md-5">
                                            <asp:Label runat="server" ID="lblFechaFactura" Style="text-align: center"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="form-group">

                                        <div class="col-md-3">
                                            <label>
                                                Cod. Cliente:
                                           
                                            </label>
                                        </div>

                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtCodClienteEditar" class="form-control" runat="server"></asp:TextBox>
                                        </div>

                                        <div class="col-md-2">
                                            <asp:LinkButton ID="btnBuscarClienteEditar" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" OnClick="btnBuscarClienteEditar_Click" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-3">
                                            <label>
                                                Cliente:
                                           
                                            </label>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ListClientesEditar" runat="server" class="form-control"></asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-3">
                                            <label>
                                                Neto:
                                           
                                            </label>
                                        </div>
                                        <div class="col-md-5">
                                            <asp:Label runat="server" ID="lblNetoFactura" Style="text-align: center"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-3">
                                            <label>
                                                Iva:
                                           
                                            </label>
                                        </div>
                                        <div class="col-md-5">
                                            <asp:Label runat="server" ID="lblIvaFactura" Style="text-align: center"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-3">
                                            <label>
                                                Total:
                                           
                                            </label>
                                        </div>
                                        <div class="col-md-5">
                                            <asp:Label runat="server" ID="lblTotalFactura" Style="text-align: center"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-3">
                                            <label>
                                                Nuevo punto de venta: 
                                           
                                            </label>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtNuevoPuntoVenta" runat="server" class="form-control" MaxLength="4" onchange="completar4Ceros(this, this.value)" />
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ErrorMessage="<h3>*</h3>" onkeypress="javascript:return validarNro(event)" ControlToValidate="txtNuevoPuntoVenta" ValidationGroup="EditFcGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-3">
                                            <label>
                                                Nuevo numero: 
                                           
                                            </label>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtNuevoNumeroFactura" runat="server" class="form-control" MaxLength="8" onkeypress="javascript:return validarNro(event)" onchange="completar8Ceros(this, this.value)" />
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtNuevoNumeroFactura" ValidationGroup="EditFcGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-3">
                                            <label>
                                                Nuevo neto: 
                                           
                                            </label>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtNuevoNetoFactura" runat="server" class="form-control" MaxLength="8" onkeypress="javascript:return validarNro(event)" />
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtNuevoNetoFactura" ValidationGroup="EditFcGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-3">
                                            <label>
                                                Nuevo Iva: 
                                           
                                            </label>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtNuevoIvaFactura" runat="server" class="form-control" MaxLength="8" onkeypress="javascript:return validarNro(event)" />
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtNuevoIvaFactura" ValidationGroup="EditFcGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-3">
                                            <label>
                                                Nuevo total: 
                                           
                                            </label>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtNuevoTotalFactura" runat="server" class="form-control" MaxLength="8" onkeypress="javascript:return validarNro(event)" />
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtNuevoTotalFactura" ValidationGroup="EditFcGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-3">
                                            <label>
                                                Nueva fecha: 
                                           
                                            </label>
                                        </div>
                                        <div class="col-md-6">
                                            <label class="input-group">
                                                <span class="input-group-addon"><i class="icon-calendar"></i></span>
                                                <asp:TextBox ID="txtNuevaFecha" runat="server" class="form-control" />
                                            </label>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtNuevaFecha" InitialValue="-1" ValidationGroup="EditFcGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="modal-footer">
                        <asp:Button runat="server" ID="btnEditarFactura" Text="Modificar" class="btn btn-success" ValidationGroup="EditFcGroup" OnClick="btnEditarFactura_Click" />
                        <asp:Button runat="server" ID="btnAnularFactura" Text="Anular" class="btn btn-danger" ValidationGroup="EditFcGroup" OnClick="btnAnularFactura_Click" />
                        <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                    </div>

                </div>
            </div>
        </div>

        <%-- MODAL BUSQUEDA --%>
        <div id="modalNro" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Busqueda</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">

                            <div class="form-group">
                                <label class="col-md-4">N° Factura</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtNumeroFactura" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label class="col-md-4">Razon Social</label>
                                <div class="col-md-6">
                                    <asp:TextBox ID="txtRazonSocial" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label class="col-md-4">Observacion</label>
                                <div class="col-md-6">
                                    <asp:TextBox ID="txtObservacion" runat="server" class="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                </div>
                            </div>

                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnBuscarNumerosFacturas" runat="server" Text="Buscar" class="btn btn-success" OnClick="btnBuscarNumerosFacturas_Click" OnClientClick="this.disabled = true; this.value = 'Buscando...';" UseSubmitBehavior="false" />
                    </div>
                </div>
            </div>
        </div>

        <%-- Modal Imprimir FC en Divisa Elegida --%>
        <div id="modalImprimirFC_EnOtraDivisa" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Imprimir FC en Otra Divisa</h4>
                    </div>
                    <div class="modal-body">

                        <asp:UpdatePanel ID="UpdatePanel8" UpdateMode="Always" runat="server">
                            <ContentTemplate>
                                <div role="form" class="form-horizontal col-md-12">
                                    <div class="row">
                                        <div class="col-md-7">
                                            <div class="form-group">
                                                <label class="col-md-4">Numero del Doc.</label>
                                                <div class="col-md-8">
                                                    <asp:Label ID="lblNumeroFC" runat="server"></asp:Label>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="form-group">
                                                <label class="col-md-4">Divisa</label>
                                                <div class="col-md-8">
                                                    <asp:DropDownList ID="DropListDivisa" runat="server" class="form-control" AutoPostBack="True" OnSelectedIndexChanged="DropListDivisa_SelectedIndexChanged"></asp:DropDownList>
                                                </div>
                                                <%--<div class="col-md-2">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server" ErrorMessage="Seleccione un tipo de Moneda" ControlToValidate="DropListTipo" InitialValue="Seleccione..." ValidationGroup="MonedaGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>--%>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-4">Cotizacion</label>
                                                <div class="col-md-8">
                                                    <div class="input-group">
                                                        <span class="input-group-addon">$</span>
                                                        <asp:TextBox ID="txtCotizacion" runat="server" class="form-control" disabled Style="text-align: right"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-5">
                                            <div class="col-md-12">
                                                <div class="widget stacked widget-box">
                                                    <div class="widget-header">
                                                        <h3>Información</h3>
                                                    </div>
                                                    <!-- /widget-header -->
                                                    <div class="widget-content">
                                                        <p>La divisa elegida en la lista, traera el valor de la divisa al momento en que se realizo la FC o NT/ND.</p>
                                                        <p>Si no se guardo con ninguna divisa, el valor a tomar sera el valor actual de la divisa seleccionada.</p>
                                                        <strong>Divisa guardada</strong><br />
                                                        <asp:Label runat="server" ID="lblFacturaMonedaGuardada" ForeColor="DarkGreen"></asp:Label></p>
                                                    </div>
                                                    <!-- /widget-content -->
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                        <div class="modal-footer">
                            <asp:Button ID="btnImprimirFCDivisa" runat="server" Text="Imprimir" class="btn btn-success" ValidationGroup="MonedaGroup" OnClick="btnImprimirFCDivisa_Click" />
                        </div>
                        <%--<div class="modal-footer">
                            <asp:LinkButton ID="LinkButton1" runat="server" Text="<span class='shortcut-icon fa fa-paper-plane'></span>" class="btn btn-success" OnClick="" ValidationGroup="EnvioGroup"></asp:LinkButton>
                        </div>--%>
                    </div>

                </div>
            </div>
        </div>
        <%-- Fin Modal Imprimir FC en Divisa Elegida --%>

        <div id="modalAgregarFactura" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Agregar Factura</h4>
                    </div>
                    <div class="modal-body">
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <div role="form" class="form-horizontal col-md-12">
                                    <div class="form-group">
                                        <div class="col-md-3">
                                            <label>
                                                Numero Desde:
                                           
                                            </label>
                                        </div>
                                        <div class="col-md-7">
                                            <asp:TextBox ID="txtNumeroDesdeAgregarFC" runat="server" class="form-control" MaxLength="8" onkeypress="javascript:return validarNro(event)" onchange="completar8Ceros(this, this.value)" />
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server" ErrorMessage="<h3>*</h3>" onkeypress="javascript:return validarNro(event)" ControlToValidate="txtNumeroDesdeAgregarFC" ValidationGroup="AgregarFcGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-3">
                                            <label>
                                                Numero Hasta:
                                           
                                            </label>
                                        </div>
                                        <div class="col-md-7">
                                            <asp:TextBox ID="txtNumeroHastaAgregarFC" runat="server" class="form-control" MaxLength="8" onkeypress="javascript:return validarNro(event)" onchange="completar8Ceros(this, this.value)" />
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator27" runat="server" ErrorMessage="<h3>*</h3>" onkeypress="javascript:return validarNro(event)" ControlToValidate="txtNumeroHastaAgregarFC" ValidationGroup="AgregarFcGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-3">Documento:</label>
                                        <div class="col-md-7">
                                            <asp:DropDownList ID="DropListDocumentoAgregarFC" runat="server" class="form-control">
                                                <asp:ListItem Value="0">Todos</asp:ListItem>
                                                <asp:ListItem Value="1">Factura A</asp:ListItem>
                                                <asp:ListItem Value="2">Factura B</asp:ListItem>
                                                <asp:ListItem Value="4">Nota de Debito A</asp:ListItem>
                                                <asp:ListItem Value="5">Nota de Debito B</asp:ListItem>
                                                <asp:ListItem Value="9">Nota de Credito A</asp:ListItem>
                                                <asp:ListItem Value="8">Nota de Credito B</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator19" InitialValue="-1" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListDocumentoAgregarFC" ValidationGroup="AgregarFcGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-3">
                                            <label>
                                                Articulo:
                                           
                                            </label>
                                        </div>
                                        <div class="col-md-7">
                                            <asp:Label runat="server" ID="lblItemAgregarFC" Style="text-align: center"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-3">
                                            <label>
                                                Cod. Cliente:
                                           
                                            </label>
                                        </div>
                                        <div class="col-md-7">
                                            <asp:TextBox ID="txtBuscarClienteAgregarFC" AutoPostBack="true" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:LinkButton ID="lbtnBuscarClienteAgregarFC" runat="server" Text="<span class='shortcut-icon icon-search'></span>" OnClick="lbtnBuscarClienteAgregarFC_Click" class="btn btn-info" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-3">
                                            <label>
                                                Cliente:
                                           
                                            </label>
                                        </div>
                                        <div class="col-md-7">
                                            <asp:DropDownList ID="DropListClienteAgregarFC" AutoPostBack="true" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-3">
                                            <label>
                                                Lista de Precios:
                                           
                                            </label>
                                        </div>
                                        <div class="col-md-7">
                                            <asp:Label runat="server" ID="lblListaPrecioAgregarFC" Style="text-align: center"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-3">
                                            <label>
                                                Empresa:
                                           
                                            </label>
                                        </div>
                                        <div class="col-md-7">
                                            <asp:DropDownList ID="DropListEmpresaAgregarFC" AutoPostBack="true" OnSelectedIndexChanged="DropListEmpresaAgregarFC_SelectedIndexChanged" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator28" InitialValue="-1" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListEmpresaAgregarFC" ValidationGroup="AgregarFcGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-3">
                                            <label>
                                                Sucursal:
                                           
                                            </label>
                                        </div>
                                        <div class="col-md-7">
                                            <asp:DropDownList ID="DropListSucursalAgregarFC" AutoPostBack="true" OnSelectedIndexChanged="DropListSucursalAgregarFC_SelectedIndexChanged" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListSucursalAgregarFC" ValidationGroup="AgregarFcGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-3">
                                            <label>
                                                Punto de Venta: 
                                           
                                            </label>
                                        </div>
                                        <div class="col-md-7">
                                            <asp:DropDownList ID="DropListPuntoVentaAgregarFC" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator18" InitialValue="-1" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListPuntoVentaAgregarFC" ValidationGroup="AgregarFcGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-3">
                                            <label>
                                                Vendedor: 
                                           
                                            </label>
                                        </div>
                                        <div class="col-md-7">
                                            <asp:DropDownList ID="DropListVendedorAgregarFC" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator23" InitialValue="-1" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListVendedorAgregarFC" ValidationGroup="AgregarFcGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="modal-footer">
                        <asp:Button runat="server" ID="btnAgregarFC" Text="Agregar" class="btn btn-success" ValidationGroup="AgregarFcGroup" OnClick="btnAgregarFC_Click" />
                        <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                    </div>
                </div>
            </div>
        </div>

        <script>
            function abrirConfirmacion(valor) {
                document.getElementById('<%= txtMovimiento.ClientID %>').value = valor;
            }
        </script>

        <script src="../../Scripts/JSFunciones1.js"></script>

        <script>

            function abrirdialog() {
                document.getElementById('abreDialog').click();
            }

        </script>
        <script type="text/javascript">
            function openModalMail() {
                $('#modalEnvioMail').modal('show');
            }
        </script>
        <script type="text/javascript">
            function openModalMailPorCliente() {
                $('#modalConfirmacionEnvioMailPorCliente').modal('show');
            }
        </script>
        <script type="text/javascript">
            function openModalImprimirFC_EnOtraDivisa() {
                $('#modalImprimirFC_EnOtraDivisa').modal('show');
            }
        </script>
        <script type="text/javascript">
            function openModal() {
                $('#modalFacturar').modal('show');
            }
        </script>
        <script type="text/javascript">
            function openModalEditarFactura() {
                $('#modalEditarFactura').modal('show');
            }
        </script>
        <script type="text/javascript">
            function openModalAgregarFactura() {
                $('#modalAgregarFactura').modal('show');
            }
        </script>
        <script type="text/javascript">
            function openModalNotaDebitoCreditoDiferenciaCambio() {
                $('#modalNotaDebitoCreditoDiferenciaCambio').modal('show');
            }
        </script>


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
            function pageLoad() {
                $("#<%= txtFechaDesde.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
                $("#<%= txtFechaHasta.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
                $("#<%= txtFechaDesdeDto.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
                $("#<%= txtFechaHastaDto.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
                $("#<%= txtFechaPago.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            }
        </script>

        <script>
            function showModalPagos() {
                $("#modalPagosProgramados").modal('show');
            }
        </script>

        <script>
            function Hidebutton() {
                document.getElementById('<%= btnAgregarPago.ClientID %>').setAttribute('Disabled', 'Disabled');
            }
        </script>

        <script>
            function Showbutton() {
                document.getElementById('<%= btnAgregarPago.ClientID %>').removeAttribute('Disabled');
            }
        </script>

        <script>


            $(function () {
                $("#<%= txtFechaDesde.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
                $("#<%= txtFechaPago.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            });

            $(function () {
                $("#<%= txtFechaHasta.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            });

        </script>

        <script>

            $(function () {
                $("#<%= txtNuevaFecha.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            });

        </script>

        <script>
            function validarNro(e) {
                var key;
                if (window.event) // IE
                {
                    key = e.keyCode;
                }
                else if (e.which) // Netscape/Firefox/Opera
                {
                    key = e.which;
                }

                if (key < 48 || key > 57) {
                    if (key == 46 || key == 8 || key == 44)// Detectar . (punto) y backspace (retroceso) y , (coma)
                    { return true; }
                    else { return false; }
                }
                return true;
            }
        </script>


    </div>
</asp:Content>
