<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChequesF_2.aspx.cs" Inherits="Gestion_Web.Formularios.Valores.ChequesF_2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">
        <div>
            <%--<div class="container">--%>
            <div class="col-md-12 col-xs-12">
                <div class="widget stacked">
                    <div class="stat">
                        <h5><i class="icon-map-marker"></i>Valores > Cheques</h5>
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
                                                <asp:LinkButton ID="btnImprimir" runat="server" OnClick="btnImprimir_Click">Imprimir</asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="btnImprimir2" runat="server" OnClick="btnImprimir2_Click">Reporte agrupado por Bco.</asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="btnImputar" runat="server" data-toggle="modal" href="#modalImputar">Imputar a</asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="btnDebitar" runat="server" data-toggle="modal" href="#modalDebitar">Debitar Cheques</asp:LinkButton>
                                            </li>
                                        </ul>
                                    </div>
                                    <!-- /btn-group -->
                                </td>
                                <td style="width: 70%">
                                    <h5>
                                        <asp:Label runat="server" ID="lblParametros" Text="" ForeColor="#cccccc"></asp:Label>
                                    </h5>
                                </td>
                                <td style="width: 5%">
                                    <div class="shortcuts" style="height: 100%">



                                        <a class="btn btn-primary" data-toggle="modal" href="#modalAgregar" style="width: 100%;">
                                            <i class="shortcut-icon icon-plus"></i>
                                        </a>
                                    </div>
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

            <div class="widget big-stats-container stacked">
                <div class="widget-content">

                    <div id="big_stats" class="cf">
                        <div class="stat">
                            <h4>Saldo</h4>
                            <asp:Label ID="lblSaldo" runat="server" Text="" class="value"></asp:Label>
                        </div>
                        <!-- .stat -->
                    </div>

                </div>


                <!-- /widget-content -->

            </div>

            <div class="col-md-12 col-xs-12">
                <div class="widget stacked widget-table action-table">

                    <div class="widget-header">
                        <i class="icon-money" style="width: 2%"></i>
                        <h3 style="width: 75%">Cheques
                                    
                        </h3>
                        <h3>
                            <%--<asp:Label ID="lblSaldo" runat="server" Style="text-align: end" Text="" ForeColor="#0099ff" Font-Bold="true"></asp:Label>--%>
                        </h3>
                    </div>
                    <div class="widget-content">
                        <div class="panel-body">

                            <%--<div class="col-md-12 col-xs-12">--%>
                            <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <div class="table-responsive">
                                        <a class="btn btn-info" style="display: none" data-toggle="modal" id="abreDialog" href="#modalFacturaDetalle">Agregar Tipo Cliente</a>
                                        <table class="table table-striped table-bordered table-hover" style="font-size:12px;"  id="dataTables-example">
                                            <thead>
                                                <tr>
                                                    <th style="font-size:11px;">Fecha Recibido</th>
                                                    <th style="font-size:11px;">Fecha Entregado</th>
                                                    <th style="font-size:11px;">Fecha Acreditacion</th>
                                                    <th style="font-size:11px;">Fecha Imputacion</th>
                                                    <asp:PlaceHolder runat="server" ID="phColumnaReciboCobro" Visible="false">
                                                        <th style="font-size:11px;">Recibo Cobro</th>
                                                    </asp:PlaceHolder>
                                                    <asp:PlaceHolder runat="server" ID="phColumnaReciboPago" Visible="false">
                                                        <th style="font-size:11px;">Recibo Pago</th>
                                                    </asp:PlaceHolder>
                                                    <th style="font-size:11px;">Importe</th>
                                                    <th style="font-size:11px;">Numero</th>
                                                    <th style="font-size:11px;">Banco</th>
                                                    <th style="font-size:11px;">Cuenta</th>
                                                    <th style="font-size:11px;">Cliente</th>
                                                    <th style="font-size:11px;">Proveedor</th>
                                                    <asp:PlaceHolder runat="server" ID="phColumnaSucCobro" Visible="false">
                                                        <th style="font-size:11px;">Suc Cobro</th>
                                                    </asp:PlaceHolder>
                                                    <asp:PlaceHolder runat="server" ID="phColumnaSucPago" Visible="false">
                                                        <th style="font-size:11px;">Suc Pago</th>
                                                    </asp:PlaceHolder>
                                                    <asp:PlaceHolder runat="server" ID="phColumaObservacion" Visible="false">
                                                        <th style="font-size:11px;">Observacion</th>
                                                    </asp:PlaceHolder>
                                                    <th style="font-size:11px;">Estado</th>
                                                    <th style="font-size:11px;">Tipo</th>
                                                    <th style="font-size:11px;"></th>
                                                </tr>

                                            </thead>
                                            <tbody>
                                                <asp:PlaceHolder ID="phCheques" runat="server"></asp:PlaceHolder>
                                            </tbody>
                                        </table>

                                    </div>


                                </ContentTemplate>
                                <Triggers>
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>


                        <!-- /.content -->

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
                            <asp:UpdatePanel ID="UpdatePanelBuscar" UpdateMode="Always" runat="server">
                                <ContentTemplate>
                                    <div class="form-group">
                                        <label class="col-md-3">
                                            Fecha Recibido
                                       
                                        </label>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtFechaDesdeRecibido" placeholder="Desde" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtFechaHastaRecibido" placeholder="Hasta" runat="server" class="form-control"></asp:TextBox>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RadioButton ID="RadioFechaRecibido" Checked="true" runat="server" GroupName="fecha" />
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaDesdeRecibido" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaHastaRecibido" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-3">
                                            Fecha Entregado
                                       
                                        </label>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtFechaDesdeEntregado" placeholder="Desde" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtFechaHastaEntregado" placeholder="Hasta" runat="server" class="form-control"></asp:TextBox>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RadioButton ID="RadioFechaEntregado" runat="server" GroupName="fecha" />
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaDesdeEntregado" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaHastaEntregado" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-3">
                                            Fecha Acreditacion
                                       
                                        </label>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtDesdeC" placeholder="Desde" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtHastaC" placeholder="Hasta" runat="server" class="form-control"></asp:TextBox>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RadioButton ID="RadioFechaCobro" runat="server" AutoPostBack="true" OnCheckedChanged="RadioFechaCobro_CheckedChanged" GroupName="fecha" />
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtDesdeC" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtHastaC" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-3">
                                            Fecha Imputacion
                                       
                                        </label>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtDesdeI" placeholder="Desde" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtHastaI" placeholder="Hasta" runat="server" class="form-control"></asp:TextBox>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RadioButton ID="RadioFechaImp" runat="server" GroupName="fecha" AutoPostBack="true" OnCheckedChanged="RadioFechaImp_CheckedChanged" />
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtDesdeI" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtHastaI" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-3">Sucursal Cobro</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListSucursal" runat="server" disabled class="form-control"></asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListSucursal" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-3">Sucursal Pago</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListSucursalPago" runat="server" disabled class="form-control"></asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListSucursalPago" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label for="name" class="col-md-3">Cod. Cliente</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtCodCliente" class="form-control" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:LinkButton ID="btnBuscarCodigoCliente" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" OnClick="btnBuscarCodigoCliente_Click" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-3">Cliente</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListClientes" runat="server" class="form-control"></asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListClientes" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label for="name" class="col-md-3">Cod. Proveedor</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtCodProveedor" class="form-control" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:LinkButton ID="btnBuscarCodigoProveedor" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" OnClick="btnBuscarCodigoProveedor_Click" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-3">Proveedor</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListProveedores" runat="server" class="form-control"></asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListProveedores" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-3">Origen</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropDownListOrigen" runat="server" class="form-control">
                                                <asp:ListItem Text="Todos" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Propios" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Terceros" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListClientes" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-3">Estado</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListEstado" runat="server" class="form-control">
                                                <asp:ListItem Text="Todos" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="Disponibles" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Depositado" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="Entregado" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="Imputado" Value="5"></asp:ListItem>
                                                <asp:ListItem Text="Debitado" Value="6"></asp:ListItem>
                                            </asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListEstado" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
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

        <div id="modalImputar" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">

                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Imputar a cta</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <div class="form-group">
                                <label class="col-md-3">Fecha:</label>
                                <div class="col-md-6">
                                    <asp:TextBox ID="txtFechaImputar" runat="server" class="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaImputar" ValidationGroup="ImputarGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-3">Cuenta:</label>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="DropListCuentas" runat="server" class="form-control">
                                        <asp:ListItem Text="Seleccione..." Value="-1"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListCuentas" InitialValue="-1" ValidationGroup="ImputarGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                    </div>


                    <div class="modal-footer">
                        <asp:LinkButton ID="btnSiImputar" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnImputar_Click" ValidationGroup="ImputarGroup" />
                    </div>
                </div>

            </div>
        </div>

        <div id="modalDebitar" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">

                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Debitar Cheque</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <div class="form-group">
                                <label class="col-md-3">Fecha:</label>
                                <div class="col-md-6">
                                    <asp:TextBox ID="txtFechaDebitar" runat="server" class="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaDebitar" ValidationGroup="DebitarGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                    </div>


                    <div class="modal-footer">
                        <asp:LinkButton ID="lbtnSiDebitar" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnSiDebitar_Click" ValidationGroup="DebitarGroup" />
                    </div>
                </div>

            </div>
        </div>

        <div id="modalAgregar" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">

                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Agregar cheque</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <div class="form-group">
                                <label class="col-md-3">Fecha recibido:</label>
                                <div class="col-md-6">
                                    <div class="input-group">
                                        <span class="input-group-addon">
                                            <i class="fa fa-calendar" aria-hidden="true"></i>
                                        </span>
                                        <asp:TextBox ID="txtFechaAgregar" runat="server" class="form-control" Style="text-align: right;"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaAgregar" ValidationGroup="AgregarGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-3">Fecha cobro:</label>
                                <div class="col-md-6">
                                    <div class="input-group">
                                        <span class="input-group-addon">
                                            <i class="fa fa-calendar" aria-hidden="true"></i>
                                        </span>
                                        <asp:TextBox ID="txtFechaCAgregar" runat="server" class="form-control" Style="text-align: right;"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaAgregar" ValidationGroup="AgregarGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-3">Numero:</label>
                                <div class="col-md-6">
                                    <asp:TextBox ID="txtNumeroAgregar" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtNumeroAgregar" ValidationGroup="AgregarGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-3">Importe</label>
                                <div class="col-md-6">
                                    <div class="input-group">
                                        <span class="input-group-addon">$</span>
                                        <asp:TextBox ID="txtImporteAgregar" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)" Style="text-align: right"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtImporteAgregar" ValidationGroup="AgregarGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-3">Banco:</label>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="ListBancoAgregar" runat="server" class="form-control" />
                                </div>
                                <div class="col-md-2">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="ListBancoAgregar" InitialValue="-1" ValidationGroup="AgregarGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-3">Cuenta:</label>
                                <div class="col-md-6">
                                    <asp:TextBox ID="txtCuentaAgregar" runat="server" class="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtCuentaAgregar" ValidationGroup="AgregarGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-3">Cuit:</label>
                                <div class="col-md-6">
                                    <asp:TextBox ID="txtCuitAgregar" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)" MaxLength="11"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtCuitAgregar" ValidationGroup="AgregarGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-3">Librador:</label>
                                <div class="col-md-6">
                                    <asp:TextBox ID="txtLibradorAgregar" runat="server" class="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtLibradorAgregar" ValidationGroup="AgregarGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-3">Observacion:</label>
                                <div class="col-md-6">
                                    <asp:TextBox ID="txtObservacionAgregar" runat="server" TextMode="MultiLine" Rows="4" class="form-control"></asp:TextBox>
                                </div>
                            </div>

                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:LinkButton ID="lbtnAgregarCh" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnAgregarCh_Click" ValidationGroup="AgregarGroup" />
                    </div>
                </div>

            </div>
        </div>
    </div>

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


    <!-- Page-Level Plugin Scripts - Tables -->
    <%--<script src="//cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>--%>
    <script src="//cdn.datatables.net/1.10.11/js/jquery.dataTables.min.js"></script>
    <script src="//cdn.datatables.net/plug-ins/1.10.9/sorting/date-eu.js"></script>
    <%--<script src="../../Scripts/plugins/dataTables/custom.tables.js"></script>--%>
    <%--<link href="//cdn.datatables.net/1.10.2/css/jquery.dataTables.css" rel="stylesheet" />--%>
    <link href="//cdn.datatables.net/1.10.11/css/jquery.dataTables.min.css" rel="stylesheet" />


    <script>
        function pageLoad() {
            $("#<%= txtFechaDesdeRecibido.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });

            $("#<%= txtFechaHastaRecibido.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });

            $("#<%= txtFechaDesdeEntregado.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });

            $("#<%= txtFechaHastaEntregado.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });

            $("#<%= txtHastaC.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });

            $("#<%= txtDesdeC.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });

            $("#<%= txtHastaI.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });

            $("#<%= txtDesdeI.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });

            $("#<%= txtFechaAgregar.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });

            $("#<%= txtFechaCAgregar.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });


        }
    </script>

    <script>


        $(function () {
            $("#<%= txtFechaDesdeRecibido.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });
        $(function () {
            $("#<%= txtFechaHastaRecibido.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            });
            $(function () {
                $("#<%= txtFechaDesdeEntregado.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            });
            $(function () {
                $("#<%= txtFechaHastaEntregado.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            });
            $(function () {
                $("#<%= txtHastaC.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            });
            $(function () {
                $("#<%= txtDesdeC.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            });
            $(function () {
                $("#<%= txtHastaI.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            });
            $(function () {
                $("#<%= txtDesdeI.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            });

            $(function () {
                $("#<%= txtFechaImputar.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            });
            $(function () {
                $("#<%= txtFechaDebitar.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            });

            $(function () {
                $("#<%= txtFechaAgregar.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            });
            $(function () {
                $("#<%= txtFechaCAgregar.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            });


    </script>


    <script>
        $(document).ready(function () {
            $('#dataTables-example').dataTable({
                "bLengthChange": false,
                "pageLength": 15,
                "bFilter": true,
                "bInfo": false,
                "bAutoWidth": false,
                "columnDefs": [
                { type: 'date-eu', targets: [0, 1, 2] }
                ],
                "language": {
                    "search": "Buscar:"
                }
            });
        });

        //$(document).ready(function () {
        //    $('#dataTables-example').dataTable().makeEditable();
        //});

    </script>

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endReq);
        function endReq(sender, args) {
            $('#dataTables-example').dataTable();
        }
        //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endReq);
        //function endReq(sender, args) {
        //    $('#dataTables-example').dataTable({
        //        "bLengthChange": false,
        //        "bFilter": false,
        //        "bInfo": false,
        //        "bAutoWidth": false
        //    });
        //}
    </script>
    <script>
        //valida los campos solo numeros
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
                if (key == 46 || key == 8 || key == 44) // Detectar . (punto) , backspace (retroceso) y , (coma)
                { return true; }
                else
                { return false; }
            }
            return true;
        }
    </script>


</asp:Content>
