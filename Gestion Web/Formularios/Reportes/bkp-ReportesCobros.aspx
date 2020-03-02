<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportesCobros.aspx.cs" Inherits="Gestion_Web.Formularios.Reportes.ReportesCobros" %>


<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="main">

        <div >

            <div >
                <div class="col-md-12 col-xs-12">
                    <div class="widget stacked">
                        <div class="stat">                        
                            <h5><i class="icon-map-marker"></i> Reportes > Impagas > Clientes</h5>
                        </div>
                        <div class="widget-header">
                            <i class="icon-wrench"></i>
                            <h3>Herramientas</h3>
                        </div>
                        <div class="widget-content">
                            <table style="width: 100%">
                                <tr>

                                    <td style="width: 20%">
                                        <asp:PlaceHolder ID="phAcciones" runat="server">
                                            <div class="btn-group">
                                                <button type="button" class="btn btn-success dropdown-toggle" data-toggle="dropdown" id="btnAccion" runat="server">Accion    <span class="caret"></span></button>
                                                <ul class="dropdown-menu">
                                                    <li>
                                                        <asp:LinkButton ID="lbtnImprimir" OnClick="btnImprimir_Click" runat="server">Reporte impagas clientes</asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lbtnExportar2" OnClick="lbtnExportar2_Click" runat="server">Exportar reporte impagas</asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lbtnImprimirDetalle" OnClick="lbtnImprimirDetalle_Click" runat="server">Reporte impagas detallado</asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lbtnExportar" OnClick="lbtnExportar_Click" runat="server">Exportar reporte detallado</asp:LinkButton>
                                                    </li>
                                                </ul>
                                            </div>
                                        </asp:PlaceHolder>
                                        <!-- /btn-group -->

                                    </td>

                                    <td style="width: 95%">
                                        <h5>
                                            <asp:Label runat="server" ID="lblParametros" Text="" ForeColor="#cccccc"></asp:Label>
                                        </h5>
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
            </div>

            <div >

                <div class="col-md-12">

                    <div class="widget big-stats-container stacked">

                        <div class="widget-content">

                            <div id="big_stats" class="cf">
                                <%--<div class="stat">
                                    <h4>Documentos Impagos</h4>
                                    <asp:Label ID="lblDocumentosImpagos" runat="server" Text="" class="value" ></asp:Label>
                                </div>--%>
                                <!-- .stat -->

                                <div class="stat">
                                    <h4>Documentos Impagos ($)</h4>
                                    <asp:Label ID="lblDocumentosImpagosPesos" runat="server" Text="" class="value"></asp:Label>
                                </div>
                                <!-- .stat -->

                                <%--<div class="stat">
                                        <h4>Clientes Atrasados</h4>
                                        <asp:Label ID="lblClientesAtrasados" runat="server" Text="" class="value"></asp:Label>
                                    </div>--%>
                                <!-- .stat -->

                                <%-- <div class="stat">
                                    <h4>Promedio Impagas por Clientes</h4>
                                    <asp:Label ID="lblPromedioImpagas" runat="server" Text="" class="value"></asp:Label>
                                </div>--%>
                                <!-- .stat -->
                            </div>

                        </div>
                        <!-- /widget-content -->

                    </div>
                    <!-- /widget -->

                </div>
                <!-- /span12 -->

            </div>
            <!-- /row -->


            <%-- <div class="row">

                <div class="col-md-6">

                    <div class="widget stacked">

                        <div class="widget-header">
                            <i class="icon-star"></i>
                            <h3>Top Clientes Impagas</h3>
                        </div>
                        <!-- /widget-header -->

                        <div class="widget-content">

                            <div id="pie-chart" class="chart-holder"></div>

                        </div>
                        <!-- /widget-content -->

                    </div>
                    <!-- /widget -->




                </div>
                <!-- /span6 -->


                <div class="col-md-6">

                    <div class="widget stacked">

                        <div class="widget-header">
                            <i class="icon-list-alt"></i>
                            <h3>Cobros - Ultimos 12 Meses</h3>
                        </div>
                        <!-- /widget-header -->

                        <div class="widget-content">

                            <div id="bar-chart" class="chart-holder">
                            </div>
                        </div>
                        <!-- /widget-content -->

                    </div>
                    <!-- /widget -->

                </div>
                <!-- /span6 -->

            </div>--%>


            <%--  <div class="row">

                <div class="col-md-4">

                    <div class="widget stacked widget-table">

                        <div class="widget-header">
                            <span class="icon-list-alt"></span>
                            <h3>Top Articulos (Cantidad) </h3>
                        </div>
                        <!-- .widget-header -->

                        <div class="widget-content">
                            <table class="table table-bordered table-striped">

                                <thead>
                                    <tr>
                                        <th>Codigo</th>
                                        <th>Descripcion</th>
                                        <th>Cantidad</th>
                                    </tr>

                                </thead>

                                <tbody>
                                    <asp:PlaceHolder ID="phTopArticulos" runat="server"></asp:PlaceHolder>
                                </tbody>

                            </table>

                        </div>
                        <!-- .widget-content -->

                    </div>
                    <!-- /widget -->

                </div>
                <!-- /span4 -->



                <div class="col-md-4">

                    <div class="widget stacked widget-table">

                        <div class="widget-header">
                            <span class="icon-file"></span>
                            <h3>Top Clientes ($) </h3>
                        </div>
                        <!-- .widget-header -->

                        <div class="widget-content">
                            <table class="table table-bordered table-striped">

                                <thead>
                                    <tr>
                                        <th>Cliente</th>
                                        <th>Total</th>
                                    </tr>
                                </thead>

                                <tbody>
                                    <asp:PlaceHolder ID="phTopClientes" runat="server"></asp:PlaceHolder>
                                </tbody>

                            </table>

                        </div>
                        <!-- .widget-content -->

                    </div>

                </div>
                <!-- /span4 -->



                <div class="col-md-4">

                    <div class="widget stacked widget-table">

                        <div class="widget-header">
                            <span class="icon-external-link"></span>
                            <h3>Top Vendedores ($) </h3>
                        </div>
                        <!-- .widget-header -->

                        <div class="widget-content">
                            <table class="table table-bordered table-striped">

                                <thead>
                                    <tr>
                                        <th>Vendedor</th>
                                        <th>Total</th>
                                    </tr>
                                </thead>

                                <tbody>
                                    <asp:PlaceHolder ID="phTopVendedores" runat="server"></asp:PlaceHolder>

                                </tbody>

                            </table>

                        </div>
                        <!-- .widget-content -->

                    </div>

                </div>
                <!-- /span4 -->

            </div>--%>
            <!-- /row -->




            <div >
                <div class="col-md-12 col-xs-12">
                    <div class="widget stacked">

                        <div class="widget-header">
                            <i class="icon-list"></i>
                            <h3>Ranking Impagas</h3>
                        </div>
                        <div class="widget-content">
                            <div class="bs-example">
                                <ul id="myTab" class="nav nav-tabs">
                                    <li class="active"><a href="#home" data-toggle="tab">Clientes</a></li>
                                    <li class=""><a href="#profile" data-toggle="tab">Vendedores</a></li>
                                </ul>
                                <div class="tab-content">
                                    <div class="tab-pane fade active in" id="home">
                                        <div class="col-md-8">

                                            <div class="widget stacked widget-table">

                                                <div class="widget-header">
                                                    <span class="icon-list-alt"></span>
                                                    <h3>Top Clientes</h3>
                                                </div>
                                                <!-- .widget-header -->

                                                <div class="widget-content">
                                                    <table class="table table-bordered table-striped">

                                                        <thead>
                                                            <tr>
                                                                <th style="text-align: left; width: 60%">Cliente</th>
                                                                <th style="text-align: left; width: 35%">Importe</th>
                                                                <th style="text-align: right; width: 5%"></th>
                                                            </tr>

                                                        </thead>

                                                        <tbody>
                                                            <asp:PlaceHolder ID="phTopClientes" runat="server"></asp:PlaceHolder>
                                                        </tbody>

                                                    </table>

                                                </div>
                                                <!-- .widget-content -->

                                            </div>
                                            <!-- /widget -->

                                        </div>
                                        <!-- /span4 -->



                                        <%-- <div class="col-md-4">

                                            <div class="widget stacked widget-table">

                                                <div class="widget-header">
                                                    <span class="icon-file"></span>
                                                    <h3>Top Clientes </h3>
                                                </div>
                                                <!-- .widget-header -->

                                                <div class="widget-content">
                                                    <table class="table table-bordered table-striped">

                                                        <thead>
                                                            <tr>
                                                                <th style="text-align: left; width: 70%">Cliente</th>
                                                                <th style="text-align: right; width: 30%">Cantidad</th>
                                                            </tr>
                                                        </thead>

                                                        <tbody>
                                                            <asp:PlaceHolder ID="phTopClientesCantidad" runat="server"></asp:PlaceHolder>
                                                        </tbody>

                                                    </table>

                                                </div>
                                                <!-- .widget-content -->

                                            </div>

                                        </div>
                                        <!-- /span4 -->



                                        <div class="col-md-4">

                                            <div class="widget stacked widget-table">

                                                <div class="widget-header">
                                                    <span class="icon-external-link"></span>
                                                    <h3>Top Vendedores</h3>
                                                </div>
                                                <!-- .widget-header -->

                                                <div class="widget-content">
                                                    <table class="table table-bordered table-striped">

                                                        <thead>
                                                            <tr>
                                                                <th style="text-align: left; width: 70%">Vendedor</th>
                                                                <th style="text-align: right; width: 30%">Cantidad</th>
                                                            </tr>
                                                        </thead>

                                                        <tbody>
                                                            <asp:PlaceHolder ID="phTopVendedoresCantidad" runat="server"></asp:PlaceHolder>

                                                        </tbody>

                                                    </table>

                                                </div>
                                                <!-- .widget-content -->

                                            </div>

                                        </div>--%>
                                    </div>
                                    <div class="tab-pane fade" id="profile">
                                        <div class="col-md-8">

                                            <div class="widget stacked widget-table">

                                                <div class="widget-header">
                                                    <span class="icon-list-alt"></span>
                                                    <h3>Top Vendedores</h3>
                                                </div>
                                                <!-- .widget-header -->

                                                <div class="widget-content">
                                                    <table class="table table-bordered table-striped">

                                                        <thead>
                                                            <tr>
                                                                <th style="text-align: left; width: 70%">Vendedor</th>
                                                                <th style="text-align: left; width: 30%">Importe</th>
                                                            </tr>

                                                        </thead>

                                                        <tbody>
                                                            <asp:PlaceHolder ID="phTopVendedores" runat="server"></asp:PlaceHolder>
                                                        </tbody>

                                                    </table>

                                                </div>
                                                <!-- .widget-content -->

                                            </div>
                                            <!-- /widget -->

                                        </div>
                                        <!-- /span4 -->



                                        <%--                                        <div class="col-md-4">

                                            <div class="widget stacked widget-table">

                                                <div class="widget-header">
                                                    <span class="icon-file"></span>
                                                    <h3>Top Clientes </h3>
                                                </div>
                                                <!-- .widget-header -->

                                                <div class="widget-content">
                                                    <table class="table table-bordered table-striped">

                                                        <thead>
                                                            <tr>
                                                                <th style="text-align: left; width: 70%">Cliente</th>
                                                                <th style="text-align: right; width: 30%">Total</th>
                                                            </tr>
                                                        </thead>

                                                        <tbody>
                                                            <asp:PlaceHolder ID="phTopClientesImporte" runat="server"></asp:PlaceHolder>
                                                        </tbody>

                                                    </table>

                                                </div>
                                                <!-- .widget-content -->

                                            </div>

                                        </div>
                                        <!-- /span4 -->



                                        <div class="col-md-4">

                                            <div class="widget stacked widget-table">

                                                <div class="widget-header">
                                                    <span class="icon-external-link"></span>
                                                    <h3>Top Vendedores</h3>
                                                </div>
                                                <!-- .widget-header -->

                                                <div class="widget-content">
                                                    <table class="table table-bordered table-striped">

                                                        <thead>
                                                            <tr>
                                                                <th style="text-align: left; width: 70%">Vendedor</th>
                                                                <th style="text-align: right; width: 30%">Total</th>
                                                            </tr>
                                                        </thead>

                                                        <tbody>
                                                            <asp:PlaceHolder ID="phTopVendedorImporte" runat="server"></asp:PlaceHolder>

                                                        </tbody>

                                                    </table>

                                                </div>
                                                <!-- .widget-content -->

                                            </div>

                                        </div>--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- /widget-content -->

                    </div>
                    <!-- /widget -->
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
                                    <div class="form-group" style="display: none">
                                        <label class="col-md-4" style="display: none">Desde</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtFechaDesde" runat="server" Text="01/01/2000" Style="display: none" class="form-control"></asp:TextBox>
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
                                        <label class="col-md-4">Sucursal</label>
                                        <div class="col-md-4">
                                            <asp:DropDownList ID="DropListSucursal" runat="server" class="form-control" disabled></asp:DropDownList>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaHasta" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                        <!-- /input-group -->

                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Buscar Cliente</label>
                                        <div class="col-md-4">
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
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListClientes" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Vendedor</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListVendedores" runat="server" class="form-control"></asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListVendedores" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Tipo</label>
                                        <div class="col-md-6">
       