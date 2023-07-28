﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CobranzaF.aspx.cs" Inherits="Gestion_Web.Formularios.Facturas.CobranzaF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">

        <%--<div class="container">--%><div>

            <div class="col-md-12 col-xs-12">
                <div class="widget stacked">
                    <div class="stat">
                        <h5><i class="icon-map-marker"></i>Ventas > Cobros</h5>
                    </div>
                    <div class="widget-header">
                        <i class="icon-wrench"></i>
                        <h3>Herramientas</h3>
                    </div>
                    <!-- /widget-header -->

                    <div class="widget-content">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                            <ContentTemplate>

                                <table style="width: 100%">
                                    <tr>
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

                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <!-- /widget-content -->

                </div>
                <!-- /widget -->
            </div>
            <div class="col-md-12">
                <div class="widget big-stats-container stacked">
                    <div class="widget-content">
                        <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <div id="big_stats" class="cf">
                                    <div class="stat">
                                        <h4>Saldo</h4>
                                        <asp:Label ID="labelSaldo" runat="server" Text="" class="value"></asp:Label>
                                    </div>
                                    <div class="stat">
                                        <h4>Documentos Seleccionados:</h4>
                                        <asp:Label ID="lblsimbolo" runat="server" Text="$" class="value"></asp:Label>
                                        <asp:Label ID="labelSeleccion" runat="server" Text="0" class="value"></asp:Label>
                                    </div>
                                    <!-- .stat -->
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <!-- /widget-content -->
                </div>
                <!-- /widget -->
            </div>
            <div class="col-md-12 col-xs-12">
                <div class="widget widget-table">
                    <div class="widget-header">
                        <i class="icon-th-list" style="width: 2%"></i>
                        <h3 style="width: 75%">Documentos Impagos
                        </h3>
                        <h3>
                            <asp:Label ID="lblSaldo" runat="server" Style="text-align: end" Text="" ForeColor="#0099ff" Font-Bold="true"></asp:Label>
                        </h3>
                    </div>
                    <div class="widget-content">
                        <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Always" runat="server">
                            <ContentTemplate>
                                <a class="btn btn-info" style="display: none" data-toggle="modal" id="abreDialog" href="#modalFacturaDetalle">Agregar Tipo Cliente</a>
                                <table class="table table-bordered table-striped" id="dataTables-example">
                                    <thead>
                                        <tr>
                                            <th style="text-align: left">Fecha</th>
                                            <th style="text-align: left">Fecha vencimiento</th>
                                            <th style="text-align: left">Numero</th>
                                            <th style="text-align: right">Debe</th>
                                            <th style="text-align: right">Haber</th>
                                            <th style="text-align: right">Saldo</th>
                                            <th>Seleccionar</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:PlaceHolder ID="phCobranzas" runat="server"></asp:PlaceHolder>
                                        <th></th>
                                        <th></th>
                                        <th></th>
                                        <th></th>
                                        <th style="width: 20%">
                                            <div class="col-lg-8">
                                                <div class="input-group">
                                                    <span class="input-group-addon">$</span>
                                                    <asp:TextBox ID="txtImputar" runat="server" class="form-control" Text="0" Style="text-align: right"></asp:TextBox>
                                                </div>
                                            </div>
                                        </th>
                                        <th></th>
                                    </tbody>
                                </table>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>

            <div class="row" style="margin: 0 10px;">
                <table>
                    <tbody>
                        <tr>
                            <td>
                                <asp:Button ID="btnSiguiente" runat="server" Text="Siguiente" class="btn btn-success" OnClick="btnSiguiente_Click" />
                            </td>
                            <td>&nbsp
                            </td>
                            <td>
                                <asp:Button ID="btnPagoCuenta" runat="server" Text="Pago a Cuenta" class="btn btn-info" Visible="true" OnClick="btnPagoCuenta_Click" />
                            </td>
                            <td>&nbsp
                            </td>
                            <td>
                                <asp:Button ID="btnGenerarDto" runat="server" Text="Generar Dto" class="btn btn-info" data-toggle="modal" href="#modalDescuento" />
                            </td>
                            <td>&nbsp
                            </td>
                            <td>
                                <asp:Button ID="btnGenerarNCND" runat="server" Text="Generar NC/ND" class="btn btn-info" data-toggle="modal" href="#modalNCND" />
                            </td>
                            <td>&nbsp
                            </td>
                            <td>
                                <asp:Button ID="btnImputar" runat="server" Text="Imputar doc. a favor" class="btn btn-info" OnClick="btnImputar_Click" />
                            </td>
                            <td>
                            &nbsp
                            <td>
                                <asp:LinkButton ID="BtnImputarModal" runat="server" Text="Autoimputar Documento" class="btn btn-info"
                                    OnClientClick="AbrirModalImputar(); return false;"></asp:LinkButton>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>

            <%--<div class="col-lg-1">
                <asp:Button ID="btnSiguiente" runat="server" Text="Siguiente" class="btn btn-success" OnClick="btnSiguiente_Click" />
            </div>
            <div class="col-lg-2" style="width: 10%;">
                <asp:Button ID="btnPagoCuenta" runat="server" Text="Pago a Cuenta" class="btn btn-info" Visible="true" OnClick="btnPagoCuenta_Click" />
            </div>
            <div class="col-lg-2" style="width: 10%;">
                <asp:Button ID="btnGenerarDto" runat="server" Text="Generar Dto" class="btn btn-info" data-toggle="modal" href="#modalDescuento" />
            </div>
            <div class="col-lg-2" style="width: 10%;">
                <asp:Button ID="btnGenerarNCND" runat="server" Text="Generar NC/ND" class="btn btn-info" data-toggle="modal" href="#modalNCND" />
            </div>
            <div class="col-lg-2" style="width: 10%;">
                <asp:Button ID="btnImputar" runat="server" Text="Imputar" class="btn btn-info" OnClick="btnImputar_Click" />
            </div>--%>
        </div>
        <%--Fin modalGrupo--%>
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
                                    <label class="col-md-4">Buscar Cliente</label>
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
                                        <asp:DropDownList ID="DropListClientes" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="DropListClientes_SelectedIndexChanged"></asp:DropDownList>

                                        <!-- /input-group -->
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListClientes" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Razon Social</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="ListRazonSocial" runat="server" AutoPostBack="true" class="form-control" OnSelectedIndexChanged="ListRazonSocial_SelectedIndexChanged"></asp:DropDownList>
                                    </div>

                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="ListRazonSocial" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Empresa</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="DropListEmpresa" runat="server" class="form-control" OnSelectedIndexChanged="DropListEmpresa_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    </div>

                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListEmpresa" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                    <!-- /input-group -->
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Sucursal</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="DropListSucursal" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="DropListSucursal_SelectedIndexChanged"></asp:DropDownList>
                                        <!-- /input-group -->
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListSucursal" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Punto de Venta</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="DropListPuntoVta" runat="server" class="form-control"></asp:DropDownList>

                                        <!-- /input-group -->
                                    </div>

                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListPuntoVta" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Tipo</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="DropListTipo" runat="server" class="form-control">
                                            <asp:ListItem Value="1">FC</asp:ListItem>
                                            <asp:ListItem Value="2">PRP</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListTipo" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="modal-footer">
                        <asp:LinkButton ID="lbBuscar" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnBuscar_Click" ValidationGroup="BusquedaGroup" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="modalImputacion" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog" style="width: 80%;">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Imputar</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <div class="widget stacked">
                            <div class="widget-content">
                                <div class="bs-example">
                                    <ul id="myTab" class="nav nav-tabs">
                                        <li class="active"><a href="#home" data-toggle="tab" visible="true">Credito</a></li>
                                        <li class=""><a href="#DocsSaldoPositivo" data-toggle="tab" runat="server" id="linkArt" visible="true">Pestaña2</a></li>
                                    </ul>
                                    <div id="tabContent" class="tab-content">

                                        <div class="tab-pane fade active in" id="home">
                                            <contenttemplate>
                                                <div id="validation-form" role="form" class="form-horizontal col-md-10">
                                                    <fieldset>
                                                        <div class="col-md-12">
                                                            <div class="widget big-stats-container stacked">
                                                                <div class="widget-content">
                                                                    <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Always">
                                                                        <ContentTemplate>
                                                                            <div id="big_stats" class="cf">
                                                                                <div class="stat">
                                                                                    <h4>Saldo</h4>
                                                                                    <asp:Label ID="label1" runat="server" Text="" class="value"></asp:Label>
                                                                                </div>
                                                                                <div class="stat">
                                                                                    <h4>Documentos Seleccionados:</h4>
                                                                                    <asp:Label ID="labelPesos" runat="server" Text="$" class="value"></asp:Label>
                                                                                    <asp:Label ID="lblDocumentosSaldoNegativo" runat="server" Text="0" class="value"></asp:Label>
                                                                                </div>
                                                                                <!-- .stat -->
                                                                            </div>
                                                                        </ContentTemplate>
                                                                    </asp:UpdatePanel>
                                                                </div>
                                                                <!-- /widget-content -->
                                                            </div>
                                                            <!-- /widget -->
                                                        </div>
                                                        <div class="table-responsive d-flex align-items-center justify-content-center" style="height: 100%;">
                                                            <table class="table table-bordered table-striped" id="dataTables-example" style="margin: 0 auto;">
                                                                <thead>
                                                                    <tr>
                                                                        <th style="text-align: left">Fecha</th>
                                                                        <th style="text-align: left">Fecha vencimiento</th>
                                                                        <th style="text-align: left">Numero</th>
                                                                        <th style="text-align: right">Debe</th>
                                                                        <th style="text-align: right">Haber</th>
                                                                        <th style="text-align: right">Saldo</th>
                                                                        <th>Seleccionar</th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                    <asp:PlaceHolder ID="phDocSaldoMenorACero" runat="server"></asp:PlaceHolder>
                                                                    <th></th>
                                                                    <th></th>
                                                                    <th></th>
                                                                    <th></th>
                                                                    <th style="width: 20%">
                                                                        <div class="col-lg-8">
                                                                            <div class="input-group">
                                                                                <span class="input-group-addon">$</span>
                                                                                <asp:TextBox ID="TextBox1" runat="server" class="form-control" Text="0" Style="text-align: right"></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                    </th>
                                                                    <th></th>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </fieldset>
                                                </div>
                                            </contenttemplate>
                                        </div>

                                        <div class="tab-pane fade" id="DocsSaldoPositivo">
                                            <contenttemplate>
                                                <div class="widget-content">
                                                    <div id="validation-form2" role="form" class="form-horizontal col-md-10">
                                                        <fieldset>
                                                            <div class="table-responsive d-flex align-items-center justify-content-center" style="height: 100%;">
                                                                <table class="table table-bordered table-striped" id="dataTables-example2" style="margin: 0 auto;">
                                                                    <thead>
                                                                        <tr>
                                                                            <th style="text-align: left">Fecha</th>
                                                                            <th style="text-align: left">Fecha vencimiento</th>
                                                                            <th style="text-align: left">Numero</th>
                                                                            <th style="text-align: right">Debe</th>
                                                                            <th style="text-align: right">Haber</th>
                                                                            <th style="text-align: right">Saldo</th>
                                                                            <th>Seleccionar</th>
                                                                        </tr>
                                                                    </thead>
                                                                    <tbody>
                                                                        <asp:PlaceHolder ID="PlaceHolderSaldoPositivo" runat="server"></asp:PlaceHolder>
                                                                        <th></th>
                                                                        <th></th>
                                                                        <th></th>
                                                                        <th></th>
                                                                        <th style="width: 20%">
                                                                            <div class="col-lg-8">
                                                                                <div class="input-group">
                                                                                    <span class="input-group-addon">$</span>
                                                                                    <asp:TextBox ID="TextBox2" runat="server" class="form-control" Text="0" Style="text-align: right"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                        </th>
                                                                        <th></th>
                                                                    </tbody>
                                                                </table>
                                                            </div>
                                                            <%-- Aca comienza la segunda tabla --%>
                                                            <div class="table-responsive d-flex align-items-center justify-content-center" style="height: 100%; margin-top: 20px;">
                                                                <table class="table table-bordered table-striped" id="dataTables-example3" style="margin: 0 auto;">
                                                                    <thead>
                                                                        <tr>
                                                                            <th style="text-align: left">Fecha</th>
                                                                            <th style="text-align: left">Fecha vencimiento</th>
                                                                            <th style="text-align: left">Numero</th>
                                                                            <th style="text-align: right">Debe</th>
                                                                            <th style="text-align: right">Haber</th>
                                                                            <th style="text-align: right">Saldo</th>
                                                                            <th>Seleccionar</th>
                                                                        </tr>
                                                                    </thead>
                                                                    <tbody>
                                                                        <%--<asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>--%>
                                                                        <th></th>
                                                                        <th></th>
                                                                        <th></th>
                                                                        <th></th>
                                                                        <th style="width: 20%">
                                                                            <div class="col-lg-8">
                                                                                <div class="input-group">
                                                                                    <span class="input-group-addon">$</span>
                                                                                    <asp:TextBox ID="TextBox3" runat="server" class="form-control" Text="0" Style="text-align: right"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                        </th>
                                                                        <th></th>
                                                                    </tbody>
                                                                </table>
                                                            </div>
                                                        </fieldset>
                                                    </div>
                                                </div>
                                            </contenttemplate>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:LinkButton ID="LinkButtonImputar" runat="server" Text="Siguiente" class="btn btn-success" />
                    </div>
                </div>
            </div>
        </div>
    </div>


    <div id="modalDescuento" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Generar Descuento</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Always" runat="server">
                            <ContentTemplate>
                                <div class="form-group">
                                    <label class="col-md-4">Total:</label>
                                    <div class="col-md-4">
                                        <div class="input-group">
                                            <span class="input-group-addon">$</span>
                                            <asp:TextBox ID="txtTotalFC" class="form-control" disabled runat="server" Text="0.00" Style="text-align: right;"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Debe ser mayor a cero." ControlToValidate="txtTotalFC" InitialValue="0.00" ValidationGroup="DtoGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Dto 1 :</label>
                                    <div class="col-md-4">
                                        <div class="input-group">
                                            <span class="input-group-addon">%</span>
                                            <asp:TextBox ID="txtDescuento1" class="form-control" TextMode="Number" Text="0" runat="server" onchange="javascript:TotalDescuento()" onkeypress="javascript:return validarNro(event)" Style="text-align: right;"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ErrorMessage="*" ControlToValidate="txtDescuento1" InitialValue="0" ValidationGroup="DtoGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Dto 2 :</label>
                                    <div class="col-md-4">
                                        <div class="input-group">
                                            <span class="input-group-addon">%</span>
                                            <asp:TextBox ID="txtDescuento2" class="form-control" TextMode="Number" Text="0" runat="server" onchange="javascript:TotalDescuento()" onkeypress="javascript:return validarNro(event)" Style="text-align: right;"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" ControlToValidate="txtDescuento2" ValidationGroup="DtoGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Nota de Credito :</label>
                                    <div class="col-md-4">
                                        <div class="input-group">
                                            <span class="input-group-addon">$</span>
                                            <asp:TextBox ID="txtDescuentoFinal" class="form-control" runat="server" Text="0.00" Style="text-align: right;" ReadOnly="True"></asp:TextBox>
                                            <asp:TextBox ID="txtFinal" class="form-control" runat="server" Style="display: none;"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="*" ControlToValidate="txtDescuentoFinal" InitialValue="0.00" ValidationGroup="DtoGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>

                    </div>
                    <div class="modal-footer">
                        <asp:LinkButton ID="lbtnGenerarNC" runat="server" Text="Generar" class="btn btn-success" ValidationGroup="DtoGroup" OnClick="lbtnGenerarNC_Click" />
                    </div>
                </div>

            </div>
        </div>
    </div>


    <div id="modalNCND" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Generar</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <asp:UpdatePanel ID="UpdatePanel7" UpdateMode="Always" runat="server">
                            <ContentTemplate>
                                <div class="form-group">
                                    <label class="col-md-4">Importe:</label>
                                    <div class="col-md-4">
                                        <div class="input-group">
                                            <span class="input-group-addon">$</span>
                                            <asp:TextBox ID="txtImporte" class="form-control" runat="server" Text="0.00" Style="text-align: right;"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="Debe ser mayor a cero." ControlToValidate="txtImporte" InitialValue="0.00" ValidationGroup="NCNDGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Tipo:</label>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="drpNCND" runat="server" class="form-control">
                                            <asp:ListItem Text="NC" Value="1" />
                                            <asp:ListItem Text="ND" Value="2" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="name" class="col-md-4">Detalle</label>
                                    <div class="col-md-7" style="height: inherit">
                                        <asp:TextBox ID="txtDescripcionArt" runat="server" EnableViewState="false" TextMode="MultiLine" Rows="4" BackColor="LightYellow" class="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="*" ControlToValidate="txtDescripcionArt" InitialValue="0.00" ValidationGroup="NCNDGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="modal-footer">
                        <asp:LinkButton ID="lbtnGenerarNCND" runat="server" Text="Generar" class="btn btn-success" ValidationGroup="NCNDGroup" OnClick="lbtnGenerarNCND_Click" />
                    </div>
                </div>

            </div>
        </div>
    </div>

    <div id="modalComentarios" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode="Always">
                    <ContentTemplate>

                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h4 class="modal-title">Observaciones</h4>
                        </div>
                        <div class="modal-body">
                            <div role="form" class="form-horizontal col-md-12">
                                <div class="form-group">
                                    <div class="col-md-12">
                                        <asp:TextBox ID="txtComentario" runat="server" class="form-control" disabled TextMode="MultiLine" Rows="4" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

        </div>
    </div>


    <script src="//code.jquery.com/ui/1.11.2/jquery-ui.js"></script>

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
                else { return false; }
            }
            return true;
        }
    </script>
    <script>

        function updatebox(valor, id) {
            var textbox = document.getElementById("<%=txtTotalFC.ClientID%>");
            var lblSeleccion = document.getElementById("<%= labelSeleccion.ClientID %>");

            var chk1 = document.getElementById("cbSeleccion_" + id);

            if (chk1.checked) {
                textbox.value = parseFloat(parseFloat(textbox.value) + parseFloat(valor)).toFixed(2);
                lblSeleccion.textContent = parseFloat(parseFloat(lblSeleccion.textContent) + parseFloat(valor)).toFixed(2).toLocaleString("es-ES");
            }
            else {
                textbox.value = parseFloat(parseFloat(textbox.value) - parseFloat(valor)).toFixed(2)
                lblSeleccion.textContent = parseFloat(parseFloat(lblSeleccion.textContent) - parseFloat(valor)).toFixed(2).toLocaleString("es-ES");
            }

        }

        function AbrirModalImputar() {
            $('#modalImputacion').modal('show');
            return false;
            // $('#modalComentarios').modal('show');
        }

        function updateboxSaldoMenorACero(valor, id) {
            //let textbox = document.getElementById("<%//=txtTotalFC.ClientID%>");
            let lblSeleccion = document.getElementById("<%= lblDocumentosSaldoNegativo.ClientID %>");

            let chk1 = document.getElementById("cbSeleccionSaldoMenorACero_" + id);

            if (chk1.checked) {
                //textbox.value = parseFloat(parseFloat(textbox.value) + parseFloat(valor)).toFixed(2);
                lblSeleccion.textContent = parseFloat(parseFloat(lblSeleccion.textContent) + parseFloat(valor)).toFixed(2).toLocaleString("es-ES");
            }
            else {
                //textbox.value = parseFloat(parseFloat(textbox.value) - parseFloat(valor)).toFixed(2)
                lblSeleccion.textContent = parseFloat(parseFloat(lblSeleccion.textContent) - parseFloat(valor)).toFixed(2).toLocaleString("es-ES");
            }

        }

        function updateboxSaldoMayorACero(valor, id) {
            //let textbox = document.getElementById("<%//=txtTotalFC.ClientID%>");
            let lblSeleccion = document.getElementById("<%= lblDocumentosSaldoNegativo.ClientID %>");
            let chk2 = document.getElementById("cbSeleccionSaldoMayorACero_" + id);

            if (chk2.checked == true) {
                //textbox.value = parseFloat(parseFloat(textbox.value) + parseFloat(valor)).toFixed(2);
                lblSeleccion.textContent = parseFloat(parseFloat(lblSeleccion.textContent) + parseFloat(valor)).toFixed(2).toLocaleString("es-ES");
                let tabla = document.getElementById("dataTables-example2");

                //El codigo de la segunda tabla iria aca, creo
                //CargarSegundaTabla(tabla)
                //let Segundatabla = document.getElementById("dataTables-example2");
                //if (Segundatabla != null) {
                //    let Segundafilas = Segundatabla.getElementsByTagName("tr");
                //    for (let i = 1; i <= Segundafilas.length; i++) {
                //        let SegundaFilas = Segundafilas[i];
                //        let checkbox = SegundaFilas.getElementsByTagName("td")[6].querySelector("input[type='checkbox']");

                //        if (checkbox) {
                //            // El checkbox existe, ahora puedes continuar con tu código

                //        } else {
                //            console.log("El checkbox no fue encontrado en la fila " + i);
                //        }



                //        //Si la checkBox esta checkeada, quiere decir que tiene que agregar esa fila
                //        //A la segunda tabla 
                //        /* if (checkbox.checked) {*/
                //        //let Fecha = Filas.getElementsByTagName("td")[0].textContent;
                //        //let FechaVencimiento = Filas.getElementsByTagName("td")[1].textContent;
                //        //let Numero = Filas.getElementsByTagName("td")[2].textContent;
                //        //let Debe = Filas.getElementsByTagName("td")[3].textContent;
                //        //let Haber = Filas.getElementsByTagName("td")[4].textContent;
                //        //let Saldo = Filas.getElementsByTagName("td")[5].textContent;

                //        //console.log(Fecha)
                //        //console.log(FechaVencimiento)
                //        //console.log(Numero)
                //        //console.log(Debe)
                //        //console.log(Haber)
                //        //console.log(Saldo)

                //        //    let FechaSegundaTabla = "<td> " + Fecha + "</td>";
                //        //    let FechaVencimientoSegundaTabla = "<td> " + FechaVencimiento + "</td>";
                //        //    let NumeroSegundaTabla = "<td> " + Numero + "</td>";
                //        //    let DebeSegundaTabla = "<td style=\" text-align: right\"> " + Debe + "</td>";
                //        //    let HaberSegundaTabla = "<td style=\" text-align: right\"> " + Haber + "</td>";
                //        //    let SaldoSegundaTabla = "<td style=\" text-align: right\"> " + Saldo + "</td>";


                //        //    let appendfinal = "<tr>" +
                //        //        FechaSegundaTabla +
                //        //        FechaVencimientoSegundaTabla +
                //        //        NumeroSegundaTabla +
                //        //        DebeSegundaTabla +
                //        //        HaberSegundaTabla +
                //        //        SaldoSegundaTabla +                        
                //        //        "</tr>"
                //        //$('#dataTables-example3').append(appendfinal);
                //        //}
                //    }

                //}

                //A partir de aca es mi codigo
                // Obtener el texto de la etiqueta lblSeleccion
                let textoEtiqueta = lblSeleccion.textContent;
                // Convertir el texto a un número
                let numeroEtiqueta = parseFloat(textoEtiqueta);
                if (numeroEtiqueta >= 0) {
                    // Obtener la tabla dentro del PlaceHolder por su ID
                    if (tabla != null) {
                        // Obtener todas las filas de la tabla
                        let filas = tabla.getElementsByTagName("tr");

                        // Iterar sobre las filas y hacer lo que necesites con ellas
                        for (let i = 1; i <= filas.length; i++) {
                            let Filas = filas[i];
                            let checkbox = Filas.getElementsByTagName("td")[6].querySelector("input[type='checkbox']");

                            if (!checkbox.checked) {
                                checkbox.disabled = true;
                            }
                        }
                    }
                    else {
                        console.log("No se encontró la tabla con ID 'miTabla' dentro del PlaceHolder.");
                    }
                }
            }
            else {
                //textbox.value = parseFloat(parseFloat(textbox.value) - parseFloat(valor)).toFixed(2)
                lblSeleccion.textContent = parseFloat(parseFloat(lblSeleccion.textContent) - parseFloat(valor)).toFixed(2).toLocaleString("es-ES");
                //Se llama Menos porque el numero disminuye cuando el usario quitar el checkmark en la checkbox

                let textoEtiquetaMenos = lblSeleccion.textContent;
                let numeroEtiquetaMenos = parseFloat(textoEtiquetaMenos);

                console.log(numeroEtiquetaMenos)
                let tablaMenos = document.getElementById("dataTables-example2");
                let filasMenos = tablaMenos.getElementsByTagName("tr");
                if (numeroEtiquetaMenos < 0) {
                    for (let i = 0; i < filasMenos.length; i++) {
                        let Filas = filasMenos[i + 1];
                        let checkbox = Filas.getElementsByTagName("td")[6].querySelector("input[type='checkbox']");

                        if (!checkbox.checked) {
                            console.log("Hola");
                            checkbox.disabled = false;
                        }
                    }
                }
            }
        }


        function CargarSegundaTabla(tabla) {
            if (tabla != null) {
                // Obtener todas las filas de la tabla
                let filas = tabla.getElementsByTagName("tr");

                // Iterar sobre las filas y hacer lo que necesites con ellas
                for (let i = 1; i <= filas.length; i++) {
                    let Filas = filas[i];
                    let checkbox = Filas.getElementsByTagName("td")[6].querySelector("input[type='checkbox']");

                    if (checkbox) {
                        console.log("hola")

                    } else {
                        console.log("El checkbox no fue encontrado en la fila " + i);
                    }



                    //Si la checkBox esta checkeada, quiere decir que tiene que agregar esa fila
                    //A la segunda tabla 
                    if (checkbox.checked) {
                        let Fecha = Filas.getElementsByTagName("td")[0].textContent;
                        let FechaVencimiento = Filas.getElementsByTagName("td")[1].textContent;
                        let Numero = Filas.getElementsByTagName("td")[2].textContent;
                        let Debe = Filas.getElementsByTagName("td")[3].textContent;
                        let Haber = Filas.getElementsByTagName("td")[4].textContent;
                        let Saldo = Filas.getElementsByTagName("td")[5].textContent;

                        console.log(Fecha)
                        console.log(FechaVencimiento)
                        console.log(Numero)
                        console.log(Debe)
                        console.log(Haber)
                        console.log(Saldo)

                        let FechaSegundaTabla = "<td> " + Fecha + "</td>";
                        let FechaVencimientoSegundaTabla = "<td> " + FechaVencimiento + "</td>";
                        let NumeroSegundaTabla = "<td> " + Numero + "</td>";
                        let DebeSegundaTabla = "<td style=\" text-align: right\"> " + Debe + "</td>";
                        let HaberSegundaTabla = "<td style=\" text-align: right\"> " + Haber + "</td>";
                        let SaldoSegundaTabla = "<td style=\" text-align: right\"> " + Saldo + "</td>";


                        let appendfinal = "<tr>" +
                            FechaSegundaTabla +
                            FechaVencimientoSegundaTabla +
                            NumeroSegundaTabla +
                            DebeSegundaTabla +
                            HaberSegundaTabla +
                            SaldoSegundaTabla +
                            "</tr>"
                        $('#dataTables-example3').append(appendfinal);

                        let textoEtiqueta = lblSeleccion.textContent;
                        // Convertir el texto a un número
                        let numeroEtiqueta = parseFloat(textoEtiqueta);
                        if (numeroEtiqueta >= 0) {
                            // Obtener la tabla dentro del PlaceHolder por su ID
                            if (tabla != null) {
                                // Obtener todas las filas de la tabla
                                let filas = tabla.getElementsByTagName("tr");

                                // Iterar sobre las filas y hacer lo que necesites con ellas
                                for (let i = 1; i <= filas.length; i++) {
                                    let Filas = filas[i];
                                    let checkbox = Filas.getElementsByTagName("td")[6].querySelector("input[type='checkbox']");

                                    if (!checkbox.checked) {
                                        checkbox.disabled = true;
                                    }
                                }
                            }
                            else {
                                console.log("No se encontró la tabla con ID 'miTabla' dentro del PlaceHolder.");
                            }
                        }
                    }
                }
            }
            else {
                console.log("No se encontró la tabla con ID 'miTabla' dentro del PlaceHolder.");
            }
        }



        function openModalComentario() {
            $('#modalComentarios').modal('show');
        }


    </script>
    <script>
        function TotalDescuento() {

            if (document.getElementById("<%=txtDescuento1.ClientID%>").value != "" && document.getElementById("<%=txtDescuento2.ClientID%>").value != "") {
                var totalFC = parseFloat(document.getElementById("<%=txtTotalFC.ClientID%>").value);
                var dto1 = parseFloat(document.getElementById("<%=txtDescuento1.ClientID%>").value);
                var dto2 = parseFloat(document.getElementById("<%=txtDescuento2.ClientID%>").value);


                //var dtoFinal = dto1 + (dto2 * (1 - (dto2 / 100)));
                var dtoFinal = parseFloat(100 * (1 - (dto1 / 100))).toFixed(2);
                dtoFinal = parseFloat(dtoFinal * (1 - (dto2 / 100))).toFixed(2);

                var totalDto = parseFloat(totalFC * (dtoFinal / 100)).toFixed(2);
                var final = totalFC - totalDto;

                document.getElementById('<%= txtDescuentoFinal.ClientID %>').value = parseFloat(final).toFixed(2);
                document.getElementById('<%= txtFinal.ClientID %>').value = parseFloat(final).toFixed(2);
            }
        }
    </script>


    <link href="../../css/pages/reports.css" rel="stylesheet">
</asp:Content>
