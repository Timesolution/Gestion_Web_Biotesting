<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportesVentas.aspx.cs" Inherits="Gestion_Web.Formularios.Reportes.ReportesVentas" %>

<%--<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="main">
        <div>
            <div class="col-md-12">

                <div class="row">
                    <div class="col-md-12 col-xs-12">
                        <div class="widget stacked">

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
                                                            <asp:LinkButton ID="lbtnImprimir" OnClick="lbtnImprimir_Click" runat="server">Imprimir Ranking ventas Cantidad</asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lbtnImprimir2" OnClick="lbtnImprimir2_Click" runat="server">Imprimir Ranking ventas Importe</asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lbtnImprimir3" OnClick="lbtnImprimir3_Click" runat="server">Imprimir Ranking ventas clientes</asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lbtnImprimir4" OnClick="lbtnImprimir4_Click" runat="server">Imprimir Ranking ventas Sucursales</asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lbtnExportar" OnClick="lbtnExportar_Click" runat="server">Exportar reporte Cantidad</asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lbtnExportar2" OnClick="lbtnExportar2_Click" runat="server">Exportar reporte Importe</asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lbtnExportar3" OnClick="lbtnExportar3_Click" runat="server">Exportar Ranking ventas clientes</asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lbtnExportar4" OnClick="lbtnExportar4_Click" runat="server">Exportar Ranking ventas sucursales</asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lbtnExportar5" OnClick="lbtnExportar5_Click" runat="server">Exportar cantidad articulos x cliente</asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lbtnExportar6" OnClick="lbtnExportar6_Click" runat="server">Exportar Importe vendedores</asp:LinkButton>
                                                        </li>
                                                        <li class="dropdown-submenu dropdown-menu-right"><a tabindex="-1" href="#">Articulos por grupo</a>
                                                            <ul class="dropdown-menu">
                                                                <li>
                                                                    <asp:LinkButton ID="lbtnReporteArticulosPorGrupo" runat="server" OnClick="lbtnReporteArticulosPorGrupo_Click">
                                                                    <i class="fa fa-file-excel-o" aria-hidden="true"></i>
                                                                    &nbsp Exportar
                                                                    </asp:LinkButton>
                                                                </li>
                                                                <li>
                                                                    <asp:LinkButton ID="lbtnReporteArticulosPorGrupoPDF" runat="server" OnClick="lbtnReporteArticulosPorGrupoPDF_Click">
                                                                    <i class="fa fa-file-pdf-o" aria-hidden="true"></i>
                                                                    &nbsp Imprimir
                                                                    </asp:LinkButton>
                                                                </li>
                                                            </ul>
                                                        </li>
                                                        <li class="dropdown-submenu dropdown-menu-right"><a tabindex="-1" href="#">Articulos por categoria</a>
                                                            <ul class="dropdown-menu">
                                                                <li>
                                                                    <asp:LinkButton ID="lbtnReporteArticulosPorCategoriaAndProveedorXLS" runat="server" OnClick="lbtnReporteArticulosPorCategoriaAndProveedorXLS_Click">
                                                                    <i class="fa fa-file-excel-o" aria-hidden="true"></i>
                                                                    &nbsp Exportar
                                                                    </asp:LinkButton>
                                                                </li>
                                                                <li>
                                                                    <asp:LinkButton ID="lbtnReporteArticulosPorCategoriaAndProveedorPDF" runat="server" OnClick="lbtnReporteArticulosPorCategoriaAndProveedorPDF_Click">
                                                                    <i class="fa fa-file-pdf-o" aria-hidden="true"></i>
                                                                    &nbsp Imprimir
                                                                    </asp:LinkButton>
                                                                </li>
                                                            </ul>
                                                        </li>
                                                        <li class="dropdown-submenu dropdown-menu-right"><a tabindex="-1" href="#">Ventas por Punto de venta</a>
                                                            <ul class="dropdown-menu">
                                                                <li>
                                                                    <asp:LinkButton ID="lbtnReporteVentasPorPuntoDeVentaExcel" runat="server" OnClick="lbtnReporteVentasPorPuntoDeVentaExcel_Click">
                                                                    <i class="fa fa-file-excel-o" aria-hidden="true"></i>
                                                                    &nbsp Exportar
                                                                    </asp:LinkButton>
                                                                </li>
                                                                <li>
                                                                    <asp:LinkButton ID="lbtnReporteVentasPorPuntoDeVentaPDF" runat="server" OnClick="lbtnReporteVentasPorPuntoDeVentaPDF_Click">
                                                                    <i class="fa fa-file-pdf-o" aria-hidden="true"></i>
                                                                    &nbsp Imprimir
                                                                    </asp:LinkButton>
                                                                </li>
                                                            </ul>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </asp:PlaceHolder>
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

                <div class="row">

                    <div class="col-md-12">

                        <div class="widget big-stats-container stacked">

                            <div class="widget-content">

                                <div id="big_stats" class="cf">
                                    <div class="stat" style="width:10%">
                                        <h4>Pedidos Pendientes</h4>
                                        <asp:Label ID="lblPedidosPendientes" runat="server" Text="" class="value"></asp:Label>
                                    </div>
                                    <!-- .stat -->

                                    <div class="stat" style="width:10%">
                                        <h4>Ventas Realizadas</h4>
                                        <asp:Label ID="lblVentasRealizadas" runat="server" Text="" class="value"></asp:Label>
                                    </div>
                                    <!-- .stat -->

                                    <div class="stat" style="width:10%">
                                        <h4>Productos Vendidos</h4>
                                        <asp:Label ID="lblProductosVendidos" runat="server" Text="" class="value"></asp:Label>
                                    </div>
                                    <!-- .stat -->

                                    <div class="stat" style="width:8%">
                                        <h4>Devoluciones</h4>
                                        <asp:Label ID="lbDevoluciones" runat="server" Text="" class="value"></asp:Label>
                                    </div>
                                    <!-- .stat -->

                                     <div class="stat" style="width:15%">
                                        <h4>Venta Promedio</h4>
                                        <asp:Label ID="lbVentaPromedio" runat="server" Text="" class="value"></asp:Label>
                                    </div>
                                    <!-- .stat -->

                                    <div class="stat" style="width:10%">
                                        <h4>Articulos Por Venta</h4>
                                        <asp:Label ID="lbArticulosXVenta" runat="server" Text="" class="value"></asp:Label>
                                    </div>
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

                <div class="row">
                    <div class="col-md-12 col-xs-12">
                        <div class="widget stacked">

                            <div class="widget-header">
                                <i class="icon-list"></i>
                                <h3>Top´s</h3>
                                <asp:Label ID="lblParametrosUrl" runat="server" Style="display: none;" />
                            </div>
                            <div class="widget-content">
                                <div class="bs-example">
                                    <ul id="myTab" class="nav nav-tabs">
                                        <li class="active"><a href="#home" data-toggle="tab">Cantidad</a></li>
                                        <li class=""><a href="#profile" data-toggle="tab">Importe</a></li>
                                        <li class=""><a href="#grafico" data-toggle="tab">Estadistica</a></li>
                                    </ul>
                                    <div class="tab-content">
                                        <div class="tab-pane fade active in" id="home">
                                            <div class="col-md-5">

                                                <div class="widget stacked widget-table">

                                                    <div class="widget-header">
                                                        <span class="icon-list-alt"></span>
                                                        <h3>Top Articulos</h3>
                                                    </div>
                                                    <!-- .widget-header -->

                                                    <div class="widget-content">
                                                        <table class="table table-bordered table-striped">

                                                            <thead>
                                                                <tr>
                                                                    <th style="text-align: left; width: 30%">Codigo</th>
                                                                    <th style="text-align: left;">Descripcion</th>
                                                                    <th style="text-align: right; width: 30%">Cantidad</th>
                                                                </tr>

                                                            </thead>

                                                            <tbody>
                                                                <asp:PlaceHolder ID="phTopArticulosCantidad" runat="server"></asp:PlaceHolder>
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



                                            <div class="col-md-3">

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

                                            </div>
                                        </div>
                                        <div class="tab-pane fade" id="profile">
                                            <div class="col-md-5">

                                                <div class="widget stacked widget-table">

                                                    <div class="widget-header">
                                                        <span class="icon-list-alt"></span>
                                                        <h3>Top Articulos</h3>
                                                    </div>
                                                    <!-- .widget-header -->

                                                    <div class="widget-content">
                                                        <table class="table table-bordered table-striped">

                                                            <thead>
                                                                <tr>
                                                                    <th style="text-align: left; width: 30%">Codigo</th>
                                                                    <th style="text-align: left; width: 40%">Descripcion</th>
                                                                    <th style="text-align: right; width: 30%">Total</th>
                                                                </tr>

                                                            </thead>

                                                            <tbody>
                                                                <asp:PlaceHolder ID="phTopArticulosImporte" runat="server"></asp:PlaceHolder>
                                                            </tbody>

                                                        </table>

                                                    </div>
                                                    <!-- .widget-content -->

                                                </div>
                                                <!-- /widget -->

                                            </div>
                                            <!-- /span4 -->



                                            <div class="col-md-3">

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
                                                                    <th style="text-align: left; width: 50%">Vendedor</th>
                                                                    <th style="text-align: right; width: 20%">Importe</th>
                                                                    <th style="text-align: right; width: 10%">Comisión</th>
                                                                    <th style="text-align: right; width: 20%">Total</th>
                                                                </tr>
                                                            </thead>

                                                            <tbody>
                                                                <asp:PlaceHolder ID="phTopVendedorImporte" runat="server"></asp:PlaceHolder>

                                                            </tbody>

                                                        </table>

                                                    </div>
                                                    <!-- .widget-content -->

                                                </div>

                                            </div>
                                        </div>
                                        <div class="tab-pane fade" id="grafico">
                                            <div class="form-group" style="margin-bottom: 0px;">
                                                <div id="bar-chart" class="chart-holder"></div>
                                                <!-- /area-chart -->
                                                <br />
                                            </div>
                                            <a class="btn btn-info" id="btnGraficar" onclick=" obtenerGrafico();">Graficar por Bultos</a>
                                            <a class="btn btn-info" id="btnGraficarImportes" onclick="obtenerGraficoImportes();">Graficar por Ventas</a>
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
            <!-- / col-md-12 -->
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
                                        <label class="col-md-4">Sucursal</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListSucursal" runat="server" class="form-control"></asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListSucursal" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Proveedor</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListProveedores" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="DropListGrupo_SelectedIndexChanged"></asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListProveedores" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Grupo</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListGrupo" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="DropListGrupo_SelectedIndexChanged"></asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListGrupo" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">SubGrupo</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListSubGrupo" runat="server" class="form-control"></asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListSubGrupo" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Desc Articulo</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtDescArticulo" class="form-control" runat="server"></asp:TextBox>
                                        </div>

                                        <div class="col-md-2">
                                            <asp:LinkButton ID="btnBuscarCod" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" OnClick="btnBuscarCod_Click" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Articulo</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListArticulos" runat="server" class="form-control"></asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListArticulos" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
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
                                            <asp:DropDownList ID="ListTipo" runat="server" class="form-control">
                                                <asp:ListItem Text="Ambos" Value="-1" />
                                                <asp:ListItem Text="FC" Value="0" />
                                                <asp:ListItem Text="PRP" Value="1" />
                                            </asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Listas de precio</label>
                                    </div>
                                    <div class="form-group">
                                        <%--<label class="col-md-4">Listas de precio</label>--%>
                                        <div class="col-md-10">
                                            <asp:CheckBoxList ID="chkListListas" runat="server" RepeatLayout="table" RepeatColumns="2" RepeatDirection="horizontal">
                                            </asp:CheckBoxList>
                                        </div>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>


                    <div class="modal-footer">
                        <asp:LinkButton ID="lbtnBuscar" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnBuscar_Click" ValidationGroup="BusquedaGroup" />
                    </div>
                </div>

            </div>
        </div>
        <!-- /container -->

    </div>
    <!-- /main -->



    <link href="../../css/pages/reports.css" rel="stylesheet">
    <script src="../../Scripts/jquery-1.10.2.js"></script>
    <script src="../../Scripts/bootstrap.min.js"></script>

    <script src="../../Scripts/libs/jquery-1.9.1.min.js"></script>
    <script src="../../Scripts/libs/jquery-ui-1.10.0.custom.min.js"></script>
    <script src="../../Scripts/libs/bootstrap.min.js"></script>

    <script src="//code.jquery.com/ui/1.11.2/jquery-ui.js"></script>


    <script src="../../Scripts/Application.js"></script>

    <script src="../../Scripts/plugins/msgGrowl/js/msgGrowl.js"></script>
    <script src="../../Scripts/plugins/lightbox/jquery.lightbox.min.js"></script>
    <script src="../../Scripts/plugins/msgbox/jquery.msgbox.min.js"></script>
    <script src="../../Scripts/demo/notifications.js"></script>

    <script src="../../js/plugins/flot/jquery.flot.js"></script>
    <script src="../../js/plugins/flot/jquery.flot.pie.js"></script>
    <script src="../../js/plugins/flot/jquery.flot.resize.js"></script>

    <script>
        function pageLoad() {
            $("#<%= txtFechaDesde.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });

            $("#<%= txtFechaHasta.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        }
        function obtenerGrafico() {

            var btn = document.getElementById("btnGraficar");
            btn.setAttribute("disabled", "disabled");

            var parametros = document.getElementById("<%=lblParametrosUrl.ClientID %>");
            var valor = parametros.textContent;

            $.ajax({
                type: "POST",
                url: "ReportesVentas.aspx/cargarDatosChartVentas",
                data: '{parametros: "' + valor + '"  }',
                contentType: "application/json",
                dataType: 'json',
                error: function () {
                    btn.removeAttribute("disabled");
                    alert("No se pudo cargar grafico.");
                },
                success: OnSuccess
            });

            function OnSuccess(response) {

                //parse la info que traigo del server, la cargo en una variable y despues la seteo en el chart            
                var data = response.d;
                obj = JSON.parse(data);
                var infoChart = [];
                var ticksData = [];
                for (i = 0; i < obj.length; i++) {
                    ticksData.push([i + 1, obj[i].mes]);
                    //infoChart.push([new Date(obj[i].mes), obj[i].cantidad]);                    
                    infoChart.push([i + 1, obj[i].cantidad]);
                }
                var options = {
                    grid: { clickable: true, hoverable: true },
                    points: { show: true },
                    xaxis: { ticks: ticksData, min: 1, max: 15 },
                    series: {
                        bars: {
                            show: true
                        },
                        lines: {
                            show: true
                        }
                    },
                    bars: {
                        align: "center",
                        barWidth: 0.5
                    }
                };
                $.plot($("#bar-chart"), [{ data: infoChart, label: "Articulos" }], options);

                function showTooltip(x, y, contents) {
                    $('<div id="tooltip">' + contents + '</div>').css({
                        position: 'absolute',
                        display: 'none',
                        top: y - 25,
                        left: x + 15,
                        border: '1px solid #000000',
                        padding: '2px',
                        'background-color': '#42c8f4',
                        'color': '#000000',
                        opacity: 0.90
                    }).appendTo("body").fadeIn(200);
                }
                var previousPoint = null;

                $("#bar-chart").bind("plothover", function (event, pos, item) {
                    $("#x").text(pos.x.toFixed(2));
                    $("#y").text(pos.y.toFixed(2));

                    if (item) {
                        if (previousPoint != item.dataIndex) {
                            previousPoint = item.dataIndex;

                            $("#tooltip").remove();
                            var x = item.datapoint[0],
                                y = item.datapoint[1].toFixed(2);

                            showTooltip(item.pageX, item.pageY, "Ventas " + item.series.xaxis.ticks[item.dataIndex].label + ": " + y);
                        }
                    }
                    else {
                        $("#tooltip").remove();
                        previousPoint = null;
                    }
                });
                btn.removeAttribute("disabled");
            }
        }

        function obtenerGraficoImportes() {

            var btn = document.getElementById("btnGraficarImportes");
            btn.setAttribute("disabled", "disabled");

            var parametros = document.getElementById("<%=lblParametrosUrl.ClientID %>");
            var valor = parametros.textContent;

            $.ajax({
                type: "POST",
                url: "ReportesVentas.aspx/cargarDatosChartVentasImportes",
                data: '{parametros: "' + valor + '"  }',
                contentType: "application/json",
                dataType: 'json',
                error: function () {
                    btn.removeAttribute("disabled");
                    alert("No se pudo cargar grafico.");
                },
                success: OnSuccess
            });

            function OnSuccess(response) {

                //parse la info que traigo del server, la cargo en una variable y despues la seteo en el chart            
                var data = response.d;
                obj = JSON.parse(data);
                var infoChart = [];
                var ticksData = [];
                for (i = 0; i < obj.length; i++) {
                    ticksData.push([i + 1, obj[i].mes]);
                    infoChart.push([i + 1, obj[i].importe]);
                }
                var options = {
                    grid: { clickable: true, hoverable: true },
                    points: { show: true },
                    xaxis: { ticks: ticksData, min: 1, max: 15 },
                    series: {
                        bars: {
                            show: true
                        },
                        lines: {
                            show: true
                        }
                    },
                    bars: {
                        align: "center",
                        barWidth: 0.5
                    }
                };
                $.plot($("#bar-chart"), [{ data: infoChart, label: "Articulos" }], options);

                function showTooltip(x, y, contents) {
                    $('<div id="tooltip">' + contents + '</div>').css({
                        position: 'absolute',
                        display: 'none',
                        top: y - 25,
                        left: x + 15,
                        border: '1px solid #000000',
                        padding: '2px',
                        'background-color': '#42c8f4',
                        'color': '#000000',
                        opacity: 0.90
                    }).appendTo("body").fadeIn(200);
                }
                var previousPoint = null;

                $("#bar-chart").bind("plothover", function (event, pos, item) {
                    $("#x").text(pos.x.toFixed(2));
                    $("#y").text(pos.y.toFixed(2));

                    if (item) {
                        if (previousPoint != item.dataIndex) {
                            previousPoint = item.dataIndex;

                            $("#tooltip").remove();
                            var x = item.datapoint[0],
                                y = item.datapoint[1].toFixed(2);

                            showTooltip(item.pageX, item.pageY, "Ventas " + item.series.xaxis.ticks[item.dataIndex].label + ": " + y);
                        }
                    }
                    else {
                        $("#tooltip").remove();
                        previousPoint = null;
                    }
                });
                btn.removeAttribute("disabled");
            }
        }

    </script>


    <script>

        $(function () {
            $("#<%= txtFechaDesde.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

        $(function () {
            $("#<%= txtFechaHasta.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

    </script>

</asp:Content>
