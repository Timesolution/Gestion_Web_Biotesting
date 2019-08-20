<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ABMFacturasLargo.aspx.cs" Inherits="Gestion_Web.Formularios.Facturas.ABMFacturasLargo" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">

        <div class="container">

            <div class="row">
                <div class="col-md-12 col-xs-12">

                    <div class="widget stacked widget-table action-table">
                        <div class="widget-content">
                            <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Always" runat="server">
                                <ContentTemplate>
                                    <div class="col-md-12" style="padding: 0px;">
                                        <table class="table table-striped table-bordered">
                                            <thead>
                                                <tr>
                                                    <th>Datos Cliente</th>
                                                    <th>Datos Factura</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr>
                                                    <td style="width: 40%">
                                                        <div class="form-group">
                                                            <div class="col-md-8">
                                                                <asp:TextBox ID="txtCodigoCliente" runat="server" class="form-control"></asp:TextBox>
                                                                <label class="col-md-4"></label>
                                                            </div>
                                                            <div class="col-md-2">
                                                                <asp:LinkButton runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" OnClick="btnBuscarCod_Click"/> 
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <div class="col-md-8">
                                                                <asp:DropDownList ID="DropListClientes" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="DropListClientes_SelectedIndexChanged"></asp:DropDownList>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:Panel ID="panelBusquedaCliente" runat="server">
                                                                    <%--<asp:LinkButton ID="lbtnBuscarCliente" runat="server" Text="<span class='shortcut-icon icon-search'></span>" title data-original-title="Buscar cliente" data-toggle="modal" class="btn btn-info ui-tooltip" href="#modalBuscarClienteDescripcion" OnClientClick="CargarClientes()"/>--%>
                                                                    <%--<a class="btn btn-info ui-tooltip" data-toggle="tooltip" title data-original-title="Buscar cliente" onclick="createC();">
                                                                        <i class="shortcut-icon icon-search"></i>
                                                                    </a>--%>
                                                                    <asp:LinkButton ID="lbtnVerCtaCte" runat="server" OnClick="lbtnVerCtaCte_Click" class="btn btn-info ui-tooltip" data-toggle="tooltip" title data-original-title="Ver cta cte">
                                                                        <i class="shortcut-icon icon-th-list"></i>
                                                                    </asp:LinkButton>
                                                                </asp:Panel>
                                                            </div>
                                                        </div>
                                                    </td>
                                                    <td style="width: 60%">
                                                        <div class="form-group">
                                                            <div class="col-md-3">

                                                                <asp:TextBox ID="txtFecha" runat="server" class="form-control" disabled="" Style="text-align: center"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-9" style="text-align: right">
                                                                <h3>
                                                                    <asp:Label ID="labelNroFactura" runat="server" Text=""></asp:Label>
                                                                </h3>
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="width: 40%">
                                                        <div role="form" class="form-horizontal col-md-12">
                                                            <div class="form-inline">
                                                                <label class="col-md-12">
                                                                    <asp:Label ID="labelCliente" runat="server" Text="" Font-Bold="true"></asp:Label></label>

                                                            </div>

                                                            <div class="form-inline">
                                                                <div class="col-md-11">
                                                                    <asp:DropDownList ID="ListSucursalCliente" Visible="false" runat="server" class="form-control"></asp:DropDownList>
                                                                </div>
                                                                <div class="col-md-1">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="ListSucursalCliente" InitialValue="-1" ValidationGroup="FacturaGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>
                                                    </td>
                                                    <td style="width: 60%">
                                                        <div role="form" class="form-horizontal col-md-12" style="text-align: left">
                                                            <div class="form-inline">
                                                                <label class="col-md-4">Empresa</label>
                                                                <label class="col-md-4">Sucursal</label>
                                                                <label class="col-md-4">Punto de Venta</label>
                                                            </div>

                                                            <div class="form-inline">
                                                                <div class="col-md-4">
                                                                    <asp:DropDownList ID="ListEmpresa" runat="server" class="form-control" disabled AutoPostBack="True" OnSelectedIndexChanged="ListEmpresa_SelectedIndexChanged"></asp:DropDownList>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <asp:DropDownList ID="ListSucursal" runat="server" class="form-control" disabled AutoPostBack="True" OnSelectedIndexChanged="ListSucursal_SelectedIndexChanged"></asp:DropDownList>
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <asp:DropDownList ID="ListPuntoVenta" runat="server" class="form-control" AutoPostBack="True" OnSelectedIndexChanged="ListPuntoVenta_SelectedIndexChanged"></asp:DropDownList>
                                                                </div>
                                                                <div class="col-md-1">
                                                                    <asp:LinkButton ID="btnCierreZ" class="btn btn-info ui-tooltip" data-toggle="tooltip" title data-original-title="Cierre Z" runat="server" Text="Z" Visible="false" OnClick="btnCierreZ_Click" />
                                                                </div>
                                                            </div>
                                                            <div class="form-inline">
                                                                <div class="col-md-2">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="" ControlToValidate="ListEmpresa" InitialValue="-1" ValidationGroup="FacturaGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                </div>

                                                                <div class="col-md-2">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="" ControlToValidate="ListSucursal" InitialValue="-1" ValidationGroup="FacturaGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                </div>

                                                                <div class="col-md-2">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="" ControlToValidate="ListPuntoVenta" InitialValue="-1" ValidationGroup="FacturaGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </td>

                                                </tr>

                                            </tbody>
                                        </table>
                                    </div>

                                    <div class="col-md-12" style="padding: 0px;">
                                        <table class="table table-striped table-bordered">
                                            <thead>
                                                <tr>
                                                    <th style="width: 30%">
                                                        <label class="col-md-4">Vendedor</label>
                                                    </th>
                                                    <th style="width: 35%">
                                                        <label class="col-md-4">Formas Pago</label>
                                                    </th>
                                                    <th style="width: 25%">
                                                        <label class="col-md-4">Lista</label>
                                                    </th>
                                                    <th style="width: 5%">
                                                        <label class="col-md-4">Tipo</label>
                                                    </th>
                                                    <th style="width: 5%">
                                                        <label class="col-md-4">Forma</label>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th style="width: 30%">
                                                        <div class="col-md-10">
                                                            <asp:DropDownList ID="DropListVendedor" runat="server" class="form-control"></asp:DropDownList>
                                                        </div>
                                                        <%--<div class="col-md-2">
                                                            <a class="btn btn-info" data-toggle="modal" href="#modalVendedor">
                                                                <i class="shortcut-icon icon-plus"></i>
                                                            </a>
                                                        </div>--%>

                                                        <%--<div class="col-md-2">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListVendedor" InitialValue="-1" ValidationGroup="FacturaGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>--%>
                                                    </th>
                                                    <th style="width: 35%">
                                                        <div class="col-md-8">
                                                            <asp:DropDownList ID="DropListFormaPago" runat="server" disabled class="form-control" AutoPostBack="true" OnSelectedIndexChanged="DropListFormaPago_SelectedIndexChanged"></asp:DropDownList>
                                                        </div>
                                                        <%--<div class="col-md-2">
                                                            <a class="btn btn-info" data-toggle="modal" href="#modalFormaPago">
                                                                <i class="shortcut-icon icon-plus"></i>
                                                            </a>
                                                        </div>--%>
                                                        <div class="col-md-2">
                                                            <a class="btn btn-info" data-toggle="modal" href="#modalTarjeta" runat="server" id="btnTarjeta" visible="false">
                                                                <i class="shortcut-icon icon-credit-card"></i>
                                                            </a>
                                                            <a class="btn btn-danger ui-tooltip" title data-original-title="Credito" data-toggle="modal" data-backdrop="static" data-keyboard="false" href="#modalCreditos" runat="server" id="btnCredito" visible="false">
                                                                <i class="shortcut-icon icon-usd"></i>
                                                            </a>
                                                            <a class="btn btn-info ui-tooltip" title data-original-title="Mutual" data-toggle="modal" href="#modalMutuales" runat="server" id="btnMutual" visible="false">
                                                                <i class="shortcut-icon icon-money"></i>
                                                            </a>
                                                        </div>
                                                        <%--<div class="col-md-2">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListFormaPago" InitialValue="-1" ValidationGroup="FacturaGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>--%>
                                                    </th>
                                                    <th style="width: 25%">
                                                        <div class="col-md-10">
                                                            <asp:DropDownList ID="DropListLista" runat="server" class="form-control"></asp:DropDownList>
                                                        </div>
                                                        <%--<div class="col-md-2">
                                                            <a class="btn btn-info" data-toggle="modal" href="#modalLista">
                                                                <i class="shortcut-icon icon-plus"></i>
                                                            </a>
                                                        </div>--%>

                                                        <%--<div class="col-md-2">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListLista" InitialValue="-1" ValidationGroup="FacturaGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>--%>
                                                    </th>

                                                    <th style="width: 5%">
                                                        <div class="btn-group">
                                                            <%--                                                <button type="button" class="btn btn-info dropdown-toggle" data-toggle="dropdown" id="btnAccion" runat="server">FC<span class="caret"></span></button>--%>
                                                            <asp:LinkButton ID="lbtnAccion" runat="server" class="btn btn-info dropdown-toggle" data-toggle="dropdown" Text="FC <span class='caret'></span>" />
                                                            <ul class="dropdown-menu">

                                                                <li>
                                                                    <asp:LinkButton ID="lbtnFC" runat="server" OnClick="lbtnFC_Click">FC</asp:LinkButton>

                                                                </li>

                                                                <li>
                                                                    <asp:LinkButton ID="lbtnNC" runat="server" Visible="false" OnClick="lbtnNC_Click">NC</asp:LinkButton>

                                                                </li>

                                                                <li>
                                                                    <asp:LinkButton ID="lbtnND" runat="server" OnClick="lbtnND_Click">ND</asp:LinkButton>

                                                                </li>
                                                                <li class="divider"></li>

                                                                <li>
                                                                    <asp:LinkButton ID="lbtnPRP" runat="server" OnClick="lbtnPRP_Click">PRP</asp:LinkButton>

                                                                </li>
                                                                <li>
                                                                    <asp:LinkButton ID="lbNC" runat="server" Visible="false" OnClick="lbNC_Click">NC PRP</asp:LinkButton>

                                                                </li>

                                                                <li>
                                                                    <asp:LinkButton ID="lbND" runat="server" OnClick="lbND_Click">ND PRP</asp:LinkButton>

                                                                </li>

                                                            </ul>
                                                        </div>
                                                    </th>
                                                    <th style="width: 25%">
                                                        <div class="col-md-12" style="padding-left: 0%; padding-right: 0%;">
                                                            <asp:DropDownList ID="ListFormaVenta" runat="server" class="form-control" Style="width: 100%;"></asp:DropDownList>
                                                        </div>
                                                    </th>
                                                </tr>

                                            </thead>

                                        </table>



                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>


                    </div>
                    <!-- /widget-content -->
                </div>
            </div>

            <div class="row">

                <!-- /widget -->

                <div class="col-md-12">

                    <div class="widget stacked">

                        <div class="widget-header">
                            <i class="icon-wrench"></i>
                            <h3>Factura

                            </h3>
                        </div>
                        <!-- /widget-header -->

                        <div class="widget-content">
                            <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Always" runat="server">
                                <ContentTemplate>
                                    <table class="table table-striped table-bordered" onload="AddRequestHandler()">
                                        <asp:PlaceHolder ID="phAgregarItems" runat="server" Visible="true">
                                        <thead>
                                            <tr>
                                                <th>#</th>
                                                <th>Codigo</th>
                                                <th>Cantidad</th>
                                                <th>Descripcion</th>
                                                <th>IVA</th>
                                                <th>Des. %</th>
                                                <th>P. Unitario</th>
                                                <th>Total</th>
                                                <th></th>
                                            </tr>
                                        </thead>                                        
                                        <tbody>
                                            <tr>
                                                <%-- <td style="width: 25%">--%>
                                                <td style="width: 7%">
                                                    <asp:TextBox ID="txtRenglon" runat="server" class="form-control" Text="0" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                </td>
                                                <td style="width: 18%">
                                                    <div class="form-group">
                                                        <div class="col-md-12">
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtCodigo" runat="server" class="form-control"></asp:TextBox>

                                                                <span class="input-group-btn">
                                                                    <%--<asp:LinkButton ID="lbtnBuscarArticulo" runat="server" Text="<span class='shortcut-icon icon-search'></span>" data-toggle="modal" class="btn btn-info" href="#modalBuscarArticuloDescripcion" OnClientClick="CargarArticulos()"/> --%>
                                                                    <a class="btn btn-info" onclick="createA();">
                                                                        <i class="shortcut-icon icon-search"></i>
                                                                    </a>
                                                                    <asp:Button runat="server" Style="display: none" OnClick="btnBuscarProducto_Click" OnClientClick="foco();" class="btn btn-info" title="Search" /> 
                                                                    <%--<button runat="server" style="display: none" id="btnRun" onserverclick="btnBuscarProducto_Click" onclick="foco();" class="btn btn-info" title="Search">
                                                                        <%--<i class="btn-icon-only icon-check-sign"></i>--%>
                                                                </span>
                                                            </div>
                                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCodigo" ErrorMessage="El campo es obligatorio" SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                                                        </div>
                                                    </div>
                                                </td>
                                                <td style="width: 8%">
                                                    <asp:TextBox ID="txtCantidad" runat="server" class="form-control" AutoPostBack="True" OnTextChanged="txtCantidad_TextChanged" Style="text-align: right;"></asp:TextBox>
                                                    <div class="col-md-3" style="padding-left: 0px">
                                                        <asp:LinkButton ID="lbtnStockProd" runat="server" class="badge" Text="0" OnClick="lbtnStockProd_Click"></asp:LinkButton>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:LinkButton ID="lbtnStockDestinoProd" runat="server" class="badge" Text="0" Visible="false"></asp:LinkButton>
                                                    </div>
                                                    <div class="col-md-1">
                                                        <asp:CheckBox ID="chkVentaMedidaVenta" runat="server" Visible="false" />
                                                    </div>
                                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="El campo es obligatorio" SetFocusOnError="True" ControlToValidate="txtCantidad"></asp:RequiredFieldValidator>--%>
                                                </td>
                                                <td style="width: 25%">

                                                    <asp:TextBox ID="txtDescripcion" runat="server" class="form-control" disabled TextMode="MultiLine" MaxLength="200"></asp:TextBox>
                                                </td>
                                                <td style="width: 9%">
                                                    <asp:TextBox ID="txtIva" runat="server" class="form-control" disabled></asp:TextBox>
                                                </td>
                                                <td style="width: 6%">

                                                    <asp:TextBox ID="TxtDescuentoArri" runat="server" class="form-control" Text="0" OnTextChanged="TxtDescuentoArri_TextChanged" AutoPostBack="True" Style="text-align: right"></asp:TextBox>
                                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="El campo es obligatorio" SetFocusOnError="True" ControlToValidate="TxtDescuentoArri"></asp:RequiredFieldValidator>--%>
                                                </td>
                                                <td style="width: 14%">

                                                    <asp:TextBox ID="txtPUnitario" runat="server" class="form-control" disabled Style="text-align: right" AutoPostBack="true" OnTextChanged="txtPUnitario_TextChanged" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                </td>
                                                <td style="width: 15%">

                                                    <asp:TextBox ID="txtTotalArri" runat="server" class="form-control" disabled Style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td style="width: 10%">

                                                    <%--                                                    <button runat="server" id="btnAgregarArticulo" onclick="darclick()" class="btn btn-info">
                                                        <i class="btn-icon-only icon-plus"></i>
                                                    </button>--%>
                                                    <asp:LinkButton ID="lbtnAgregarArticuloASP" class="btn btn-info" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" OnClick="btnAgregarArt_Click" Visible="true" />
                                                </td>
                                            </tr>
                                        </tbody>
                                        </asp:PlaceHolder>
                                    </table>

                                    <br />
                                    <a class="btn btn-info" style="display: none" data-toggle="modal" id="abreDialog" href="#modalTrazabilidad"></a>
                                    <a class="btn btn-info" style="display: none" data-toggle="modal" id="abreDialog2" href="#modalDatosExtra"></a>
                                    <a class="btn btn-info" style="display: none" data-toggle="modal" id="abreDialog3" href="#modalEditarDesc"></a>
                                    <table class="table table-striped table-bordered">
                                        <thead>
                                            <tr>
                                                <th>Codigo</th>
                                                <th><a data-toggle="modal" href="#modalPorcentajeVenta">Cantidad</a></th>
                                                <th>Descripcion</th>
                                                <th>P. Unitario</th>
                                                <th>Des.</th>
                                                <th>Total</th>
                                                <th></th>
                                            </tr>
                                        </thead>
                                        <tbody>


                                            <asp:PlaceHolder ID="phArticulos" runat="server"></asp:PlaceHolder>
                                        </tbody>
                                    </table>

                                    <br />
                                    <div class="row">

                                        <div class="col-md-8">
                                            <div role="form" class="form-horizontal col-md-12">
                                                <ul id="myTab" class="nav nav-tabs">
                                                    <li class="active"><a href="#Comentarios" data-toggle="tab">Comentarios</a></li>
                                                    <li class=""><a href="#Combustible" runat="server" id="linkCombustible" visible="false" data-toggle="tab">Datos Combustible</a></li>
                                                </ul>
                                                <div class="tab-content">
                                                    <div class="tab-pane fade active in" id="Comentarios">
                                                        <table class="table table-striped table-bordered">
                                                            <tbody>
                                                                <tr>
                                                                    <td>
                                                                        <asp:PlaceHolder ID="phDatosEntrega" Visible="false" runat="server">
                                                                            <div class="form-group">
                                                                                <label class="col-md-3">Ticket de cambio:</label>

                                                                                <div class="col-md-1">
                                                                                    <asp:CheckBox ID="chkImprimirTicketDeCambio" runat="server" />
                                                                                </div>
                                                                            </div>
                                                                            <div class="form-group">
                                                                                <label class="col-md-3">Fecha Entrega: </label>

                                                                                <div class="col-md-4">
                                                                                    <asp:TextBox ID="txtFechaEntrega" runat="server" class="form-control"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                            <div class="form-group">
                                                                                <label class="col-md-3">Hora Entrega: </label>
                                                                                <div class="col-md-4">
                                                                                    <asp:TextBox ID="txtHorarioEntrega" runat="server" class="form-control"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                            <div class="form-group">
                                                                                <label class="col-md-3">Bultos Entrega: </label>
                                                                                <div class="col-md-4">
                                                                                    <asp:TextBox ID="txtBultosEntrega" runat="server" class="form-control"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                            <div class="form-group">
                                                                                <label class="col-md-3">Mail Entrega: </label>
                                                                                <div class="col-md-4">
                                                                                    <div class="input-group">
                                                                                        <span class="input-group-addon">@</span>
                                                                                        <asp:TextBox ID="txtMailEntrega" runat="server" class="form-control ui-popover" data-container="body" data-toggle="popover" data-trigger="hover" data-placement="right" data-content="Puede enviar a varios destinatarios separando las casillas de correo por punto y coma (;). Ej.: Mail1@gmail.com;Mail2@yahoo.com;..." title="" data-original-title="Ayuda"></asp:TextBox>
                                                                                    </div>
                                                                                </div>
                                                                                <div class="col-md-2">
                                                                                    <asp:CheckBox ID="chkEnviarMail" runat="server" AutoPostBack="true" Text="Enviar mail" OnCheckedChanged="chkEnviarMail_CheckedChanged" />
                                                                                </div>
                                                                            </div>
                                                                            <div class="form-group">
                                                                                <div class="col-md-12">
                                                                                    <asp:TextBox placeholder="AGREGUE AQUI OBSERVACIONES" ID="txtComentarios" runat="server" class="form-control" TextMode="MultiLine" Rows="5"></asp:TextBox>
                                                                                </div>
                                                                            </div>
                                                                        </asp:PlaceHolder>

                                                                        <div class="col-md-12">
                                                                            <asp:CheckBox ID="checkDatos" TextAlign="Left" CssClass="pull-right" Text="Comentarios&nbsp&nbsp" runat="server" AutoPostBack="True" OnCheckedChanged="checkDatos_CheckedChanged" />
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                    <div class="tab-pane fade" id="Combustible">
                                                        <table class="table table-striped table-bordered">
                                                            <tbody>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Panel ID="panelDatosCombustibles" runat="server" Visible="true">
                                                                            <div class="form-group">
                                                                                <label class="col-md-3">Proveedor: </label>
                                                                                <div class="col-md-6">
                                                                                    <asp:DropDownList ID="ListProveedorCombustible" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ListProveedorCombustible_SelectedIndexChanged" />
                                                                                </div>
                                                                            </div>
                                                                            <div class="form-group">
                                                                                <label class="col-md-3">ICL: </label>
                                                                                <%--<div class="col-md-3">
                                                                                    <div class="input-group">
                                                                                        <span class="input-group-addon">$</span>
                                                                                        <asp:TextBox ID="txtValorITC" runat="server" disabled class="form-control"></asp:TextBox>
                                                                                    </div>
                                                                                </div>--%>
                                                                                <div class="col-md-3">
                                                                                    <div class="input-group">
                                                                                        <span class="input-group-addon">$</span>
                                                                                        <asp:TextBox ID="txtTotalITC" runat="server" Style="text-align: right;" disabled class="form-control"></asp:TextBox>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="form-group">
                                                                                <label class="col-md-3">IDC: </label>
                                                                                <%--<div class="col-md-3">
                                                                                    <div class="input-group">
                                                                                        <span class="input-group-addon">$</span>
                                                                                        <asp:TextBox ID="txtValorTasaHidrica" runat="server" disabled class="form-control"></asp:TextBox>
                                                                                    </div>
                                                                                </div>--%>
                                                                                <div class="col-md-3">
                                                                                    <div class="input-group">
                                                                                        <span class="input-group-addon">$</span>
                                                                                        <asp:TextBox ID="txtTotalTasaHidrica" runat="server" Style="text-align: right;" disabled class="form-control"></asp:TextBox>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="form-group">
                                                                                <label class="col-md-3">Tasa Vial: </label>
                                                                                <%--<div class="col-md-3">
                                                                                    <div class="input-group">
                                                                                        <span class="input-group-addon">$</span>
                                                                                        <asp:TextBox ID="txtValorTasaVial" runat="server" disabled class="form-control"></asp:TextBox>
                                                                                    </div>
                                                                                </div>--%>
                                                                                <div class="col-md-3">
                                                                                    <div class="input-group">
                                                                                        <span class="input-group-addon">$</span>
                                                                                        <asp:TextBox ID="txtTotalTasaVial" runat="server" Style="text-align: right;" disabled class="form-control"></asp:TextBox>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="form-group">
                                                                                <label class="col-md-3">Tasa Municipal: </label>
                                                                                <%--<div class="col-md-3">
                                                                                    <div class="input-group">
                                                                                        <span class="input-group-addon">$</span>
                                                                                        <asp:TextBox ID="txtValorTasaMunicipal" runat="server" disabled class="form-control"></asp:TextBox>
                                                                                    </div>
                                                                                </div>--%>
                                                                                <div class="col-md-3">
                                                                                    <div class="input-group">
                                                                                        <span class="input-group-addon">$</span>
                                                                                        <asp:TextBox ID="txtTotalTasaMunicipal" runat="server" Style="text-align: right;" disabled class="form-control"></asp:TextBox>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                            <div class="form-group">
                                                                                <label class="col-md-3">Total Impuestos: </label>
                                                                                <div class="col-md-3">
                                                                                    <div class="input-group">
                                                                                        <span class="input-group-addon">$</span>
                                                                                        <asp:TextBox ID="txtTotalImpuestos" runat="server" Style="text-align: right;" disabled class="form-control"></asp:TextBox>
                                                                                    </div>
                                                                                </div>
                                                                            </div>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-md-4 pull-right">

                                            <table class="table table-striped table-bordered">

                                                <tbody>
                                                    <tr>
                                                        <td>
                                                            <div role="form" class="form-horizontal col-md-12">
                                                                <div class="form-group" style="text-align: left">
                                                                    <label class="col-md-3">Neto: </label>
                                                                    <div class="col-md-9">
                                                                        <div class="input-group">
                                                                            <span class="input-group-addon">$</span>
                                                                            <asp:TextBox ID="txtNeto" Style="text-align: right" runat="server" class="form-control col-md-4" Text="0.00" disabled></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="form-group" style="text-align: left">
                                                                    <label class="col-md-3">Descuento %: </label>
                                                                    <div class="col-md-9">
                                                                        <asp:TextBox ID="txtPorcDescuento" Style="text-align: right" runat="server" class="form-control" Text="0" AutoPostBack="True" OnTextChanged="txtDescuento_TextChanged" TextMode="Number"></asp:TextBox>
                                                                    </div>
                                                                </div>

                                                                <asp:PlaceHolder ID="phDescuentoSobreElTotal" runat="server" Visible="true">
                                                                    <div class="form-group" style="text-align: left">
                                                                        <label class="col-md-3">Descuento $</label>
                                                                        <div class="col-md-9">
                                                                            <div class="input-group">
                                                                                <a class="btn btn-info ui-tooltip input-group-addon" title data-original-title="Descuento" data-toggle="modal" href="#modalCalcularDescuentoConUnMonto" runat="server" id="A2" visible="true">
                                                                                    <i class="shortcut-icon icon-money"></i>
                                                                                </a>
                                                                                <asp:TextBox ID="txtDescuento" Style="text-align: right" runat="server" class="form-control" Text="0.00" AutoPostBack="True" OnTextChanged="txtDescuento_TextChanged" disabled></asp:TextBox>
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </asp:PlaceHolder>

                                                                <div class="form-group" style="text-align: left">
                                                                    <label class="col-md-3">SubTotal: </label>
                                                                    <div class="col-md-9">
                                                                        <div class="input-group">
                                                                            <span class="input-group-addon">$</span>
                                                                            <asp:TextBox ID="txtsubTotal" Style="text-align: right" runat="server" class="form-control col-md-4" Text="0.00" disabled></asp:TextBox>
                                                                        </div>
                                                                    </div>

                                                                </div>
                                                                <div class="form-group" style="text-align: left">
                                                                    <label class="col-md-3">Iva: </label>
                                                                    <div class="col-md-9">
                                                                        <div class="input-group">
                                                                            <span class="input-group-addon">$</span>
                                                                            <asp:TextBox ID="txtIvaTotal" Style="text-align: right" runat="server" class="form-control" Text="0.00" disabled></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="form-group" style="text-align: left">
                                                                    <div class="col-md-7">
                                                                        <label id="lblPercepcionCF" runat="server" visible="false">Percepcion IVA CF:</label>
                                                                        &nbsp
                                                                        <asp:CheckBox ID="chkIvaNoInformado" runat="server" Visible="false" OnCheckedChanged="chkIvaNoInformado_CheckedChanged" AutoPostBack="true" />
                                                                    </div>
                                                                    <div class="col-md-5">
                                                                        <div class="input-group">
                                                                            <%--<span class="input-group-addon" id="spanPerpCF">$</span>--%>
                                                                            <asp:TextBox ID="txtPercepcionCF" Style="text-align: right" runat="server" Visible="false" class="form-control" Text="0.00" disabled></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="form-group" style="text-align: left">
                                                                    <label class="col-md-3">Percepción: </label>
                                                                    <div class="col-md-4">
                                                                        <asp:TextBox ID="txtPorcRetencion" Style="text-align: right" runat="server" class="form-control" AutoPostBack="True" OnTextChanged="txtRetencion_TextChanged" TextMode="Number" Text="0"></asp:TextBox>

                                                                    </div>
                                                                    <div class="col-md-5">
                                                                        <div class="input-group">
                                                                            <span class="input-group-addon">$</span>

                                                                            <asp:TextBox ID="txtRetencion" Style="text-align: right" runat="server" class="form-control" Text="0.00" disabled></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="form-group" style="text-align: left">
                                                                    <strong>
                                                                        <label class="col-md-3">Total:</label></strong>
                                                                    <div class="col-md-9">
                                                                        <div class="input-group">
                                                                            <span class="input-group-addon">$</span>
                                                                            <asp:TextBox ID="txtTotal" Style="text-align: right" runat="server" class="form-control" Text="0.00" disabled Font-Bold="True"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </td>
                                                        <%--<td>
                                                            <div role="form" class="form-horizontal col-md-12">
                                                                <div class="form-group" style="text-align: left">
                                                                    <label class="col-md-3">Neto: </label>
                                                                    <div class="col-md-9">
                                                                        <div class="input-group">
                                                                            <span class="input-group-addon">$</span>
                                                                            <asp:TextBox ID="txtNeto" Style="text-align: right" runat="server" class="form-control col-md-4" Text="0.00" disabled></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                 <div class="form-group" style="text-align: left">
                                                                    <label class="col-md-3">Iva: </label>
                                                                    <div class="col-md-9">
                                                                        <div class="input-group">
                                                                            <span class="input-group-addon">$</span>
                                                                            <asp:TextBox ID="txtIvaTotal" Style="text-align: right" runat="server" class="form-control" Text="0.00" disabled></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                 <div class="form-group" style="text-align: left">
                                                                    <label class="col-md-3">Percepcíon: </label>
                                                                    <div class="col-md-4">
                                                                        <asp:TextBox ID="txtPorcRetencion" Style="text-align: right" runat="server" class="form-control" AutoPostBack="True" OnTextChanged="txtRetencion_TextChanged" TextMode="Number" Text="0"></asp:TextBox>

                                                                    </div>
                                                                    <div class="col-md-5">
                                                                        <div class="input-group">
                                                                            <span class="input-group-addon">$</span>

                                                                            <asp:TextBox ID="txtRetencion" Style="text-align: right" runat="server" class="form-control" Text="0.00" disabled></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="form-group" style="text-align: left">
                                                                    <label class="col-md-3">SubTotal: </label>
                                                                    <div class="col-md-9">
                                                                        <div class="input-group">
                                                                            <span class="input-group-addon">$</span>
                                                                            <asp:TextBox ID="txtsubTotal" Style="text-align: right" runat="server" class="form-control col-md-4" Text="0.00" disabled></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>

                                                                <div class="form-group" style="text-align: left">
                                                                    <label class="col-md-3">Descuento %: </label>
                                                                    <div class="col-md-4">

                                                                        <asp:TextBox ID="txtPorcDescuento" Style="text-align: right" runat="server" class="form-control" Text="0" AutoPostBack="True" OnTextChanged="txtDescuento_TextChanged" TextMode="Number"></asp:TextBox>


                                                                    </div>
                                                                    <div class="col-md-5">
                                                                        <div class="input-group">
                                                                            <span class="input-group-addon">$</span>
                                                                            <asp:TextBox ID="txtDescuento" Style="text-align: right" runat="server" class="form-control" Text="0.00" AutoPostBack="True" OnTextChanged="txtDescuento_TextChanged" disabled></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                                
                                                               

                                                               

                                                                <div class="form-group" style="text-align: left">
                                                                    <strong>
                                                                        <label class="col-md-3">Total:</label></strong>
                                                                    <div class="col-md-9">
                                                                        <div class="input-group">
                                                                            <span class="input-group-addon">$</span>
                                                                            <asp:TextBox ID="txtTotal" Style="text-align: right" runat="server" class="form-control" Text="0.00" disabled Font-Bold="True"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </td>--%>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>

                                </ContentTemplate>
                                <%--<Triggers>
                                    <asp:PostBackTrigger ControlID="btnRun" />
                                </Triggers>--%>
                            </asp:UpdatePanel>
                            <div class="row">
                                <asp:UpdatePanel ID="UpdatePanel5" UpdateMode="Always" runat="server">
                                    <ContentTemplate>
                                        <div role="form" class="form-horizontal col-md-12">
                                            <div class="col-md-12">
                                                <asp:Button ID="btnAgregar" runat="server" Text="Facturar" class="btn btn-success" OnClick="btnAgregar_Click" ValidationGroup="FacturaGroup"/>                                                
                                                <a class="btn btn-success" data-toggle="modal" href="#modalFacturaE" runat="server" id="btnFacturaE" visible="false">Siguiente</a>
                                                <asp:Button ID="btnAgregarRemitir" runat="server" Text="Facturar y Remitir" class="btn btn-success" ValidationGroup="FacturaGroup" OnClick="btnAgregarRemitir_Click"/>
                                                <asp:Button ID="btnNueva" Visible="false" runat="server" Text="Nueva Factura" class="btn btn-success" OnClick="btnNueva_Click" />
                                                <asp:Button ID="btnRefacturar" runat="server" Visible="false" Text="Refacturar" CssClass="btn btn-success" OnClick="btnRefacturar_Click" />
                                                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" class="btn btn-default" PostBackUrl="~/Default.aspx" />
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>

                        </div>
                    </div>

                </div>
            </div>
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Visible="false" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="80%">
            </rsweb:ReportViewer>
        </div>


        <div id="modalVendedor" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">

                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Agregar Vendedor</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <div class="form-group">
                                <label for="validateSelect" class="col-md-4">Vendedor</label>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="ListEmpleados" runat="server" class="form-control"></asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Seleccione un empleado" ControlToValidate="ListEmpleados" InitialValue="-1" ValidationGroup="VendedorGroup"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="validateSelect" class="col-md-4">Comision</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtComision" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtComision" ValidationGroup="VendedorGroup"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnAgregarVendedor" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnAgregarVendedor_Click" ValidationGroup="VendedorGroup" />
                    </div>

                </div>
            </div>
        </div>
        <%--Fin modalGrupo--%>

        <div id="modalLista" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">

                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Agregar Lista</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <div class="form-group">
                                <label for="validateSelect" class="col-md-4">Nombre</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtNombreLista" runat="server" class="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtNombreLista" ValidationGroup="ListaGroup"></asp:RequiredFieldValidator>
                                </div>
                            </div>

                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnAgregarLista" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnAgregarLista_Click" ValidationGroup="ListaGroup" />
                    </div>

                </div>
            </div>
        </div>
        <%--Fin modalGrupo--%>

        <div id="modalFormaPago" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">

                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Agregar Forma de Pago</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <label for="validateSelect" class="col-md-4">Forma de Pago</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtFormaPago" runat="server" class="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtFormaPago" ValidationGroup="FPGroup"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnAgregarFP" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnAgregarFP_Click" ValidationGroup="FPGroup" />
                    </div>

                </div>
            </div>
        </div>
        <%--Fin modalGrupo--%>

        <div id="modalBuscarCliente" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">

                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Buscar Cliente</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <div class="form-group">
                                <div class="col-md-8">
                                    <asp:TextBox ID="txtBuscarCliente" runat="server" class="form-control" Text=""></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <asp:Button ID="btnBuscarCliente" runat="server" class="btn btn-success" Text="Buscar" OnClick="btnBuscarCliente_Click" ValidationGroup="BCGroup" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%--Fin modalGrupo--%>

        <div id="modalTarjeta" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button id="btnCerrarTarjetas" type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Pagos con Tarjeta</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <asp:UpdatePanel ID="UpdatePanel4" UpdateMode="Always" runat="server">
                                <ContentTemplate>
                                    <div class="form-group">
                                        <div class="col-md-5">
                                            <asp:DropDownList ID="ListOperadores" runat="server" class="form-control" OnSelectedIndexChanged="ListOperadores_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="<h3>*</h3>" InitialValue="-1" ControlToValidate="ListOperadores" ValidationGroup="TarjetaGroup"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-5">
                                            <asp:DropDownList ID="ListTarjetas" runat="server" class="form-control" OnSelectedIndexChanged="ListTarjetas_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="<h3>*</h3>" InitialValue="-1" ControlToValidate="ListTarjetas" ValidationGroup="TarjetaGroup"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="input-group">
                                                <span class="input-group-addon">$</span>
                                                <asp:TextBox ID="txtImporteT" runat="server" class="form-control" Text="0" AutoPostBack="true" OnTextChanged="txtImporteT_TextChanged" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtImporteT" ValidationGroup="TarjetaGroup"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:LinkButton ID="lbtnAgregarPago" class="btn btn-info" ValidationGroup="TarjetaGroup" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" Visible="true" OnClick="lbtnAgregarPago_Click" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ListMonedas" runat="server" class="form-control"></asp:DropDownList>
                                            <label>Restan / Efectivo</label>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="input-group">
                                                <span class="input-group-addon">$</span>
                                                <asp:TextBox ID="txtImporteEfectivo" runat="server" class="form-control" ViewStateMode="Disabled" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtImporteEfectivo" ValidationGroup="EfectivoGroup"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:LinkButton ID="lbtnAgregarEfectivo" class="btn btn-info" ValidationGroup="EfectivoGroup" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" Visible="true" OnClick="lbtnAgregarEfectivo_Click" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label runat="server" Text="" Visible="false" ID="lblAvisoTarjeta" ForeColor="Red" Font-Bold="true"></asp:Label>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label ID="lblMontoCuotas" runat="server" class="alert alert-info alert-dismissable"></asp:Label>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label ID="lblAvisoPromocion" runat="server" class="alert alert-warning alert-dismissable" Visible="false"></asp:Label>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div role="form" class="form-horizontal col-md-12">
                            <asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Always" runat="server">
                                <ContentTemplate>
                                    <div class="form-group">
                                        <table class="table table-striped table-bordered">
                                            <thead>
                                                <tr>
                                                    <th>Tipo</th>
                                                    <th>Importe</th>
                                                    <th></th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:PlaceHolder ID="phPagosTarjeta" runat="server"></asp:PlaceHolder>
                                            </tbody>
                                        </table>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-4">
                                            <asp:LinkButton ID="lbtnConfirmarPago" class="btn btn-success" runat="server" Visible="false" Text="Confirmar pagos" OnClick="lbtnConfirmarPago_Click" />
                                        </div>
                                        <div class="col-md-4">
                                            <asp:LinkButton ID="lbtnCancelarPago" class="btn btn-default" runat="server" Visible="false" Text="Limpiar pagos" OnClick="lbtnCancelarPago_Click" />
                                        </div>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:UpdatePanel ID="UpdatePanelTarjeta" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="Label2" runat="server" class="col-md-3" ForeColor="Gray" Font-Bold="true" Style="padding-right: 0%;">Sin Recargo: $</asp:Label>
                                <div class="col-md-3" style="padding-left: 0%; text-align: left;">
                                    <asp:Label ID="lblMontoOriginal" runat="server" ForeColor="Gray" Font-Bold="true"></asp:Label>
                                </div>
                                <asp:Label ID="Label1" runat="server" class="col-md-3" ForeColor="Blue" Font-Bold="true" Style="padding-right: 0%;">Con Recargo: $</asp:Label>
                                <div class="col-md-3" style="padding-left: 0%; text-align: left;">
                                    <asp:Label ID="lblSaldoTarjeta" runat="server" ForeColor="Blue" Font-Bold="true"></asp:Label>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                </div>
            </div>
        </div>

        <div id="modalBuscarArticulo" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">

                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Agregar Forma de Pago</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <label for="validateSelect" class="col-md-4">Forma de Pago</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="TextBox2" runat="server" class="form-control"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtFormaPago" ValidationGroup="FPGroup"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="Button2" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnAgregarFP_Click" ValidationGroup="FPGroup" />
                    </div>

                </div>
            </div>
        </div>

        <div id="modalFacturaE" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">

                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Detalles Exportacion</h4>
                    </div>
                    <div class="modal-body">
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 20%">
                                    <asp:Label ID="lblPaisDestino" runat="server" Text="Pais de destino:"></asp:Label>
                                </td>
                                <td style="width: 80%">
                                    <div class="col-md-8">
                                        <div class="imput-group">
                                            <asp:DropDownList ID="DropListPais" runat="server" class="form-control">
                                                <asp:ListItem Text="Seleccione..." Value="-1"></asp:ListItem>
                                            </asp:DropDownList>
                                            <%--<asp:TextBox ID="txtPaisDestino" runat="server" class="form-control" Text=""></asp:TextBox>--%>
                                        </div>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListPais" InitialValue="-1" ValidationGroup="FEgroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                    </div>
                                    <!-- /input-group -->
                                </td>
                            </tr>

                            <tr>
                                <td style="width: 20%">
                                    <asp:Label ID="lblIncoterms" runat="server" Text="Incoterms:"></asp:Label>
                                </td>
                                <td style="width: 50%">
                                    <div class="col-md-8">
                                        <div class="imput-group">
                                            <asp:DropDownList ID="DropListIncoterms" runat="server" class="form-control">
                                                <asp:ListItem Text="EXW" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="FCA" Value="2"></asp:ListItem>
                                                <asp:ListItem Text="FAS" Value="3"></asp:ListItem>
                                                <asp:ListItem Text="FOB" Value="4"></asp:ListItem>
                                                <asp:ListItem Text="CFR" Value="5"></asp:ListItem>
                                                <asp:ListItem Text="CIF" Value="6"></asp:ListItem>
                                                <asp:ListItem Text="CPT" Value="7"></asp:ListItem>
                                                <asp:ListItem Text="CIP" Value="8"></asp:ListItem>
                                                <asp:ListItem Text="DAF" Value="9"></asp:ListItem>
                                                <asp:ListItem Text="DES" Value="10"></asp:ListItem>
                                                <asp:ListItem Text="DEQ" Value="11"></asp:ListItem>
                                                <asp:ListItem Text="DDU" Value="12"></asp:ListItem>
                                                <asp:ListItem Text="DDP" Value="13"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <!-- /input-group -->
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 40%">
                                    <asp:Label ID="lblPermiso" runat="server" Text="Permiso de Embarque:"></asp:Label>
                                </td>
                                <td style="width: 60%">
                                    <div class="col-md-8">
                                        <div class="imput-group">
                                            <asp:TextBox ID="txtPermisoEmbarque" runat="server" class="form-control" Text=""></asp:TextBox>
                                        </div>
                                    </div>
                                    <!-- /input-group -->
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnAgregarFactE" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnAgregarFactE_Click" ValidationGroup="FEgroup" />
                    </div>

                </div>
            </div>
        </div>
        <%--Fin modalGrupo--%>

        <div id="modalTrazabilidad" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog" style="width: 80%;">
                <div class="modal-content">
                    <div class="modal-header">
                        <button id="btnCerrarTraza" type="button" class="close" data-dismiss="modal" aria-hidden="true" style="display: none;">×</button>
                        <h4 class="modal-title">Trazabilidad</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <asp:UpdatePanel ID="UpdatePanel6" UpdateMode="Always" runat="server">
                                <ContentTemplate>
                                    <div class="widget big-stats-container stacked">

                                        <div class="widget-content">

                                            <div id="big_stats" class="cf">
                                                <div class="stat">
                                                    <h4>Cantidad</h4>
                                                    <asp:Label ID="lblTrazaActual" runat="server" Text="0" class="value"></asp:Label>
                                                    <label class="value">/</label>
                                                    <asp:Label ID="lblTrazaTotal" runat="server" Text="0" class="value"></asp:Label>
                                                </div>
                                                <!-- .stat -->
                                            </div>

                                        </div>
                                        <!-- /widget-content -->

                                    </div>
                                    <!-- /widget -->
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div role="form" class="form-horizontal col-md-12">
                            <div class="form-group">
                                <asp:UpdatePanel ID="UpdatePanel7" UpdateMode="Always" runat="server">
                                    <ContentTemplate>
                                        <table class="table table-striped table-bordered">
                                            <thead>
                                                <tr>
                                                    <th>#</th>
                                                    <asp:PlaceHolder ID="phCamposTrazabilidad" runat="server"></asp:PlaceHolder>
                                                    <th></th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:PlaceHolder ID="phStockTrazabilidad" runat="server"></asp:PlaceHolder>
                                            </tbody>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lblMovTraza" runat="server" Style="display: none;"></asp:Label>
                                <asp:LinkButton ID="AgregarTraza" runat="server" class="btn btn-success" Text="<span class='shortcut-icon icon-ok'></span>" OnClick="AgregarTraza_Click"></asp:LinkButton>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                </div>
            </div>
        </div>

        <div id="modalCreditos" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <asp:UpdatePanel ID="UpdatePanelCerrarCred" runat="server">
                            <ContentTemplate>
                                <button id="btnCerrarCreditos2" type="button" class="close" data-dismiss="modal" aria-hidden="true" style="display: none;">×</button>
                                <asp:Button ID="btnCerrarCreditos" Text="x" type="button" class="btn close" data-dismiss="modal" runat="server" OnClick="btnCerrarCreditos_Click" aria-hidden="true" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <h4 class="modal-title">Creditos</h4>
                    </div>
                    <div class="modal-body" style="padding-bottom: 0px;">
                        <div role="form" class="form-horizontal col-md-12">
                            <asp:UpdatePanel ID="UpdatePanelCreditos1" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <div class="form-group">
                                        <label class="col-md-2">Importe:</label>
                                        <div class="col-md-4">
                                            <div class="input-group">
                                                <span class="input-group-addon">$</span>
                                                <asp:TextBox ID="txtImporteFinanciar" runat="server" class="form-control" disabled Style="text-align: right;" />
                                            </div>
                                        </div>
                                        <label class="col-md-2">Financiado:</label>
                                        <div class="col-md-4">
                                            <div class="input-group">
                                                <span class="input-group-addon">$</span>
                                                <asp:TextBox ID="txtFinanciado" runat="server" class="form-control" disabled Style="text-align: right;" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-2">Anticipo:</label>
                                        <div class="col-md-4">
                                            <div class="input-group">
                                                <span class="input-group-addon">$</span>
                                                <asp:TextBox ID="txtAnticipo" runat="server" class="form-control" Text="0" OnTextChanged="txtAnticipo_TextChanged" AutoPostBack="true" onkeypress="javascript:return validarNro(event)" Style="text-align: right;" />
                                            </div>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RadioButton ID="rbtnAnticipoCredito" runat="server" Checked="true" AutoPostBack="true" GroupName="PagosCtaCreditoGroup" OnCheckedChanged="rbtnAnticipoCredito_CheckedChanged" />
                                        </div>
                                        <div class="col-md-2">
                                            <asp:LinkButton ID="lbtnAnticipo" runat="server" class="btn btn-success ui-tooltip" data-toggle="tooltip" title data-original-title="Cobrar Anticipo" Text="$" OnClick="lbtnAnticipo_Click" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-6">Pagos a cta:</label>
                                        <div class="col-md-1">
                                            <asp:RadioButton ID="rbtnPagoCuentaCredito" runat="server" GroupName="PagosCtaCreditoGroup" AutoPostBack="true" OnCheckedChanged="rbtnPagoCuentaCredito_CheckedChanged" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Panel ID="panelPagosCtaCredito" runat="server" Visible="false">
                                            <div class="col-md-12">
                                                <table class="table table-striped table-bordered">
                                                    <thead>
                                                        <tr>
                                                            <th>Fecha</th>
                                                            <th>Numero</th>
                                                            <th>Saldo</th>
                                                            <th></th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <asp:PlaceHolder ID="phPagosCuentaCredito" runat="server" Visible="true"></asp:PlaceHolder>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="modal-footer" style="padding-bottom: 0px; padding-top: 0px;">
                    </div>

                    <asp:PlaceHolder runat="server" Visible="true" ID="phCreditoModoAvanzado">
                        <div class="modal-body" style="padding-bottom: 0px;">
                            <div role="form" class="form-horizontal col-md-12">
                                <asp:UpdatePanel ID="UpdatePanelSms" runat="server" UpdateMode="Always">
                                    <ContentTemplate>
                                        <div class="form-group">
                                            <label class="col-md-2">Telefono:</label>
                                            <div class="col-md-3">
                                                <div class="input-group">
                                                    <span class="input-group-addon">0</span>
                                                    <asp:TextBox ID="txtCodAreaCredito" runat="server" class="form-control" placeholder="Ej:3735" MaxLength="4" onkeypress="javascript:return validarNroSinComa(event)" />
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <asp:TextBox ID="txtNroCelularCredito" runat="server" class="form-control" placeholder="565123" MaxLength="10" onkeypress="javascript:return validarNroSinComa(event)" />
                                            </div>
                                            <div class="col-md-1">
                                                <asp:LinkButton ID="btnEnviarCodigoCredito" runat="server" class="btn btn-info ui-tooltip" data-toggle="tooltip" title data-original-title="Enviar codigo" Text="<i class='fa fa-phone' aria-hidden='true'></i>" ValidationGroup="TelefonoGroup" OnClientClick="bloquear();" OnClick="btnEnviarCodigoCredito_Click" />
                                            </div>
                                            <div class="col-md-1">
                                                <asp:LinkButton ID="lbtnVolverValidar" runat="server" class="btn btn-success ui-tooltip" data-toggle="tooltip" title data-original-title="Volver a validar" Text="<i class='icon icon-refresh' aria-hidden='true'></i>" OnClick="lbtnVolverValidar_Click" Visible="false" />
                                                <asp:LinkButton ID="lbtnNoValidar" runat="server" class="btn btn-success ui-tooltip" data-toggle="tooltip" title data-original-title="Validar luego" Text="<i class='icon fa fa-times' aria-hidden='true'></i>" OnClick="lbtnNoValidar_Click" />
                                            </div>
                                            <div class="col-md-2">
                                                <asp:RegularExpressionValidator ErrorMessage="*" Font-Bold="true" ForeColor="Red" SetFocusOnError="true" ControlToValidate="txtCodAreaCredito" runat="server" ValidationGroup="TelefonoGroup" ValidationExpression="^[1-9][0-9]{1,3}$" />
                                                <asp:RequiredFieldValidator ErrorMessage="*" Font-Bold="true" ForeColor="Red" SetFocusOnError="true" ControlToValidate="txtNroCelularCredito" runat="server" ValidationGroup="TelefonoGroup" />
                                                <asp:RequiredFieldValidator ErrorMessage="*" Font-Bold="true" ForeColor="Red" SetFocusOnError="true" ControlToValidate="txtCodAreaCredito" runat="server" ValidationGroup="TelefonoGroup" />
                                            </div>

                                        </div>
                                        <div class="form-group">
                                            <asp:Panel ID="panelCodigoSMS" runat="server">
                                                <label class="col-md-2">Codigo:</label>
                                                <div class="col-md-3">
                                                    <asp:TextBox ID="txtCodigoVerif" runat="server" class="form-control" MaxLength="4" onkeypress="javascript:return validarNroSinComa(event)" />
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:RequiredFieldValidator ErrorMessage="*" Font-Bold="true" ForeColor="Red" SetFocusOnError="true" ControlToValidate="txtCodigoVerif" runat="server" ValidationGroup="CodigoGroup" />
                                                    <asp:Label ID="lblIdRegistro" Text="0" runat="server" Visible="false" />
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:LinkButton ID="btnVerificarCodigo" runat="server" class="btn btn-info" Text="Validar" ValidationGroup="CodigoGroup" OnClick="btnVerificarCodigo_Click" />
                                                </div>
                                                <div class="col-md-1">
                                                    <asp:LinkButton ID="btnLimpiarProcesoCredito" runat="server" class="btn btn-success ui-tooltip" data-toggle="tooltip" title data-original-title="Limpiar campos" Text="<i class='icon icon-trash' aria-hidden='true'></i>" OnClick="btnLimpiarProcesoCredito_Click" />
                                                </div>
                                            </asp:Panel>
                                            <asp:Panel ID="panelMotivoCodigoSMS" runat="server" Visible="false">
                                                <label class="col-md-2">Motivo:</label>
                                                <div class="col-md-6">
                                                    <asp:TextBox ID="txtMotivoCredito" runat="server" class="form-control" />
                                                </div>
                                                <asp:Label ID="lblOmitioCodigoCredito" runat="server" Style="display: none;" Text="0" />
                                            </asp:Panel>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <div class="modal-footer" style="padding-bottom: 0px; padding-top: 0px;">
                        </div>
                        <div class="modal-body">
                            <div role="form" class="form-horizontal col-md-12">
                                <asp:UpdatePanel ID="UpdatePanelCreditos" runat="server" UpdateMode="Always">
                                    <ContentTemplate>
                                        <div class="form-group">
                                            <label class="col-md-2">Fecha Nacimiento:</label>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtFechaNacimientoCredito" runat="server" class="form-control" />
                                            </div>
                                            <div class="col-md-1">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaNacimientoCredito" ValidationGroup="CreditosGroup"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-2">DNI:</label>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtDniCredito" runat="server" class="form-control" MaxLength="8" onkeypress="javascript:return validarNro(event)" />
                                            </div>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnBuscarSolicitudes" runat="server" class="btn btn-info ui-tooltip" data-toggle="tooltip" title data-original-title="Buscar Solicitudes" Text="<span class='shortcut-icon icon-refresh'></span>" Visible="false" OnClick="lbtnBuscarSolicitudes_Click" />
                                            </div>
                                            <div class="col-md-1">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtDniCredito" ValidationGroup="CreditosGroup"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4">Solicitud:</label>
                                            <asp:UpdateProgress ID="UpdateProgress2" AssociatedUpdatePanelID="UpdatePanelCreditos" runat="server">
                                                <ProgressTemplate>
                                                    <div class="col-md-6">
                                                        <h3>Procesando <i class="fa fa-spinner fa-spin"></i>
                                                        </h3>
                                                    </div>
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </div>
                                        <asp:Panel ID="panelSolicitudes" runat="server" Visible="false">
                                            <div class="form-group">
                                                <div class="col-md-12">
                                                    <table class="table table-striped table-bordered">
                                                        <thead>
                                                            <tr>
                                                                <th>#</th>
                                                                <th>Fecha</th>
                                                                <th>Solicitud</th>
                                                                <th>Capital</th>
                                                                <th>Anticipo</th>
                                                                <th></th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <asp:PlaceHolder ID="phSolicitud" runat="server" Visible="true"></asp:PlaceHolder>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-4">Nro Solicitud:</label>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="txtNroSolicitud" runat="server" disabled class="form-control" />
                                                </div>
                                            </div>
                                        </asp:Panel>
                                        <asp:Panel ID="panelCreditoManual" runat="server" Visible="false">
                                            <div class="form-group">
                                                <label class="col-md-4">Fecha solicitud:</label>
                                                <div class="col-md-4">
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="icon-calendar"></i></span>
                                                        <asp:TextBox ID="txtFechaSolicitudManual" runat="server" class="form-control" placeholder="DD/MM/YYYY" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-4">Capital a cancelar:</label>
                                                <div class="col-md-4">
                                                    <div class="input-group">
                                                        <span class="input-group-addon">$</span>
                                                        <asp:TextBox ID="txtCapitalSolicitudManual" runat="server" class="form-control" data-toggle="popover" data-trigger="hover" data-placement="right" data-html="true" data-content="<p> Ejemplo: Factura:$10000</p><p> Capital a cancelar:$8000 </p> <p> Anticipo:$2000 </p>" title="" data-original-title="Ayuda" onkeypress="javascript:return validarNro(event)" Style="z-index: 100000;" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-4">Anticipo:</label>
                                                <div class="col-md-4">
                                                    <div class="input-group">
                                                        <span class="input-group-addon">$</span>
                                                        <asp:TextBox ID="txtAnticipoSolicitudManual" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)" />
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-4">Nro Solicitud:</label>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="txtNroSolicitudManual" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)" />
                                                </div>
                                            </div>
                                        </asp:Panel>
                                        <div class="form-group" id="divCargaManual" runat="server" visible="false">
                                            <label class="col-md-4">Carga manual:</label>
                                            <asp:CheckBox ID="chkCreditoManual" runat="server" AutoPostBack="true" OnCheckedChanged="chkCreditoManual_CheckedChanged" />
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </asp:PlaceHolder>
                    <div class="modal-footer">
                        <asp:UpdatePanel ID="UpdatePanelCreditos2" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lblMovSolicitud" runat="server" Style="display: none;"></asp:Label>
                                <asp:LinkButton ID="lbtnAgregarSolicitud" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnAgregarSolicitud_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                </div>
            </div>
        </div>
        <!-- /row -->
        <div id="modalMutuales" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button id="btnCerrarMutuales" type="button" class="close" data-dismiss="modal" aria-hidden="true" style="display: none;">×</button>
                        <h4 class="modal-title">Mutuales</h4>
                    </div>
                    <div class="modal-body" style="padding-bottom: 0px;">
                        <div role="form" class="form-horizontal col-md-12">
                            <asp:UpdatePanel ID="UpdatePanelMutuales" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <div class="form-group">
                                        <label class="col-md-4">Mutual:</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ListMutuales" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ListMutuales_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="ListMutuales" InitialValue="-1" runat="server" ForeColor="Red" Font-Bold="true" SetFocusOnError="true" ValidationGroup="MutualGroup" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Pagos:</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ListMutualesPagos" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ListMutualesPagos_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="ListMutualesPagos" InitialValue="-1" runat="server" ForeColor="Red" Font-Bold="true" SetFocusOnError="true" ValidationGroup="MutualGroup" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Fecha:</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtFechaPagareMutual" runat="server" class="form-control" />
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="txtFechaPagareMutual" runat="server" ForeColor="Red" Font-Bold="true" SetFocusOnError="true" ValidationGroup="MutualGroup" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Nro Socio:</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtNroSocioMutual" runat="server" class="form-control" />
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="txtNroSocioMutual" runat="server" ForeColor="Red" Font-Bold="true" SetFocusOnError="true" ValidationGroup="MutualGroup" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Cant. Cuotas:</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtCantCuotasMutual" runat="server" class="form-control" TextMode="Number" disabled />
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="txtCantCuotasMutual" runat="server" ForeColor="Red" Font-Bold="true" SetFocusOnError="true" ValidationGroup="MutualGroup" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Fecha vto 1ra Cuota:</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtFechaVtoCuotaMutual" runat="server" class="form-control" />
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="txtFechaVtoCuotaMutual" runat="server" ForeColor="Red" Font-Bold="true" SetFocusOnError="true" ValidationGroup="MutualGroup" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Nro Pagare:</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtNroPagareMutual" runat="server" class="form-control" />
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="txtNroPagareMutual" runat="server" ForeColor="Red" Font-Bold="true" SetFocusOnError="true" ValidationGroup="MutualGroup" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Nro Autorizacion:</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtNroAutorizacionMutual" runat="server" class="form-control" />
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="txtNroAutorizacionMutual" runat="server" ForeColor="Red" Font-Bold="true" SetFocusOnError="true" ValidationGroup="MutualGroup" />
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-md-4">Anticipo:</label>
                                        <div class="col-md-4">
                                            <div class="input-group">
                                                <span class="input-group-addon">$</span>
                                                <asp:TextBox ID="txtAnticipoMutual" runat="server" class="form-control" Text="0" OnTextChanged="txtAnticipoMutual_TextChanged" AutoPostBack="true" onkeypress="javascript:return validarNro(event)" Style="text-align: right;" />
                                            </div>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RadioButton ID="rbtnAnticipoMutual" runat="server" AutoPostBack="true" GroupName="PagosCtaMutualGroup" Checked="true" OnCheckedChanged="rbtnAnticipoMutual_CheckedChanged" />
                                        </div>
                                        <div class="col-md-2">
                                            <asp:LinkButton ID="lbtnAnticipoMutual" runat="server" class="btn btn-success ui-tooltip" data-toggle="tooltip" title data-original-title="Cobrar Anticipo" Text="$" OnClick="lbtnAnticipoMutual_Click" Visible="true" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-8">Pagos a Cuenta:</label>
                                        <div class="col-md-1">
                                            <asp:RadioButton ID="rbtnPagoCuentaMutual" runat="server" GroupName="PagosCtaMutualGroup" AutoPostBack="true" OnCheckedChanged="rbtnPagoCuentaMutual_CheckedChanged" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Panel ID="panelPagosCuentaMutual" runat="server" Visible="false">
                                            <div class="col-md-12">
                                                <table class="table table-striped table-bordered">
                                                    <thead>
                                                        <tr>
                                                            <th>Fecha</th>
                                                            <th>Numero</th>
                                                            <th>Saldo</th>
                                                            <th></th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <asp:PlaceHolder ID="phPagosCuentaMutual" runat="server" Visible="true"></asp:PlaceHolder>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </asp:Panel>
                                    </div>


                                    <div class="form-group">
                                        <asp:LinkButton runat="server" ID="lbtnCalcularMontosMutual" Text="Calcular Montos" CssClass="btn btn-info btn-lg btn-block" OnClick="lbtnCalcularMontosMutual_Click"></asp:LinkButton>
                                    </div>

                                    <div class="form-group alert alert-info alert-dismissable">
                                        <div class="col-md-6">
                                            <label>Total Factura Original: $</label>
                                            <asp:Label ID="lblTotalOriginalMutuales" runat="server" Font-Bold="true" Text="0.00"></asp:Label>
                                        </div>
                                        <div class="col-md-6">
                                            <%--<label>Total Recargo: $</label>--%>
                                            <asp:Label ID="lblTotalRecargoOriginalMutuales" runat="server" Font-Bold="true" Text="0.00" Visible="false"></asp:Label>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:Label ID="lblTotalRecargoFinalOriginalMutuales" runat="server" Font-Bold="true" Text="0.00" Visible="false"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="form-group alert alert-danger alert-dismissable">
                                        <div class="col-md-6">
                                            <label>Total Anticipo: $</label>
                                            <asp:Label ID="lblTotalAnticipoMutual" runat="server" Font-Bold="true" Text="0.00"></asp:Label>
                                        </div>
                                        <div class="col-md-6">
                                            <label>Total Anticipo Financiado: $</label>
                                            <asp:Label ID="lblTotalAnticipoMutualFinanciado" runat="server" Font-Bold="true" Text="0.00"></asp:Label>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:Label ID="lblTotalAnticipoMutualFinal" runat="server" Font-Bold="true" Text="0.00" Visible="false"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="form-group alert alert-danger alert-dismissable">
                                        <div class="col-md-6">
                                            <label>Total Pagos a Cuenta: $</label>
                                            <asp:Label ID="lblTotalAnticipoPagoCuentaMutual" runat="server" Font-Bold="true" Text="0.00"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="form-group alert alert-info alert-dismissable">
                                        <div class="col-md-6">
                                            <label>Total Financiado: $</label>
                                            <asp:Label ID="lblTotalMutuales" runat="server" Font-Bold="true" Text="0.00"></asp:Label>
                                        </div>
                                        <div class="col-md-6">
                                            <label>Total Recargo: $</label>
                                            <asp:Label ID="lblTotalRecargoMutuales" runat="server" Font-Bold="true" Text="0.00"></asp:Label>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:Label ID="lblTotalRecargoMutualFinal" runat="server" Font-Bold="true" Text="0.00" Visible="false"></asp:Label>
                                        </div>
                                    </div>

                                    <div class="form-group alert alert-info alert-dismissable">
                                        <div class="col-md-6">
                                            <label>Total Factura Final: $</label>
                                            <asp:Label ID="lblTotalFacturaFinal" runat="server" Font-Bold="true" Text="0.00"></asp:Label>
                                        </div>
                                    </div>

                                    <asp:Label ID="lblFlagCalcularMontosMutual" runat="server" Text="0" Visible="false"></asp:Label>

                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanelMutuales">
                                <ProgressTemplate>
                                    <i class="fa fa-spinner fa-spin fa-3x fa-fw"></i>
                                    Procesando...
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="Label3" runat="server" Style="display: none;"></asp:Label>
                                <asp:LinkButton ID="lbtnAgregarPagoMutual" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success ui-tooltip" data-toggle="tooltip" title data-original-title="Agregar pago" ValidationGroup="MutualGroup" OnClick="lbtnAgregarPagoMutual_Click" />
                                <asp:LinkButton ID="lbtnQuitarPagoMutual" runat="server" Text="Quitar" class="btn btn-danger ui-tooltip" data-toggle="tooltip" title data-original-title="Quitar pago" OnClick="lbtnQuitarPagoMutual_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <%--<asp:UpdateProgress ID="UpdateProgress1" runat="server"></asp:UpdateProgress>--%>
                    </div>

                </div>
            </div>
        </div>

        <div id="modalDatosExtra" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button id="btnCerrarDatosExtra" type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Datos Extra</h4>
                    </div>
                    <div class="modal-body" style="padding-bottom: 0px;">
                        <div role="form" class="form-horizontal col-md-12">
                            <asp:UpdatePanel ID="UpdatePanel9" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <div class="form-group">
                                        <label class="col-md-4">Dato extra:</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtDatoExtra" runat="server" class="form-control" TextMode="MultiLine" Rows="4" />
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="txtDatoExtra" runat="server" ForeColor="Red" Font-Bold="true" SetFocusOnError="true" ValidationGroup="DatosExtraGroup" />
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:UpdatePanel ID="UpdatePanel10" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lblDatosExtraItem" runat="server" Style="display: none;"></asp:Label>
                                <asp:LinkButton ID="btnDatosExtra" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" ValidationGroup="DatosExtraGroup" OnClick="btnDatosExtra_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                </div>
            </div>
        </div>

        <div id="modalEnvioSMS" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">¿Desea avisar por sms?</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <div class="form-group">
                                <div class="col-md-2">
                                    <h1>
                                        <i class="icon-warning-sign" style="color: orange"></i>
                                    </h1>
                                </div>
                                <div class="col-md-10">
                                    <h4>
                                        <asp:Label ID="lblAvisoSMSSaldoMax" runat="server" Text="Cliente con saldo max. superado" />
                                    </h4>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-2">Telefono</label>
                                <div class="col-md-3">
                                    <div class="input-group">
                                        <span class="input-group-addon">0</span>
                                        <asp:TextBox ID="txtCodArea" runat="server" class="form-control" MaxLength="4" onkeypress="javascript:return validarNroSinComa(event)" />
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <asp:TextBox ID="txtTelefono" runat="server" class="form-control" MaxLength="8" onkeypress="javascript:return validarNroSinComa(event)" />
                                </div>
                                <div class="col-md-2">
                                    <asp:RegularExpressionValidator ErrorMessage="*" Font-Bold="true" ForeColor="Red" SetFocusOnError="true" ControlToValidate="txtCodArea" runat="server" ValidationGroup="AlertaSMSGroup" ValidationExpression="^[1-9][0-9]{1,3}$" />
                                    <asp:RequiredFieldValidator ErrorMessage="*" Font-Bold="true" ForeColor="Red" SetFocusOnError="true" ControlToValidate="txtCodArea" runat="server" ValidationGroup="AlertaSMSGroup" />
                                    <asp:RequiredFieldValidator ErrorMessage="*" Font-Bold="true" ForeColor="Red" SetFocusOnError="true" ControlToValidate="txtTelefono" runat="server" ValidationGroup="AlertaSMSGroup" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-2">Mensaje</label>
                                <div class="col-md-10">
                                    <asp:TextBox ID="txtMensajeSMS" runat="server" class="form-control" TextMode="MultiLine" Rows="4" onkeypress="javascript:return validarEnter(event)" />
                                </div>
                            </div>
                            <asp:TextBox runat="server" ID="txtIdEnvioSMS" Text="0" Style="display: none;"></asp:TextBox>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                            <ContentTemplate>
                                <asp:LinkButton ID="lbtnEnviarSMSAviso" runat="server" Text="<span class='shortcut-icon fa fa-paper-plane'></span>" class="btn btn-success" OnClick="lbtnEnviarSMSAviso_Click" ValidationGroup="AlertaSMSGroup"></asp:LinkButton>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                </div>
            </div>
        </div>

        <div id="modalCargaTrazabilidad" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog" style="width: 60%;">
                <div class="modal-content">
                    <asp:UpdatePanel ID="UpdatePanelCargaTraza" runat="server">
                        <ContentTemplate>
                            <div class="modal-header">
                                <button id="btnCerrarTraza2" type="button" class="close" data-dismiss="modal" aria-hidden="true" style="display: none;">×</button>
                                <h4 class="modal-title">Trazabilidad</h4>
                            </div>
                            <div class="modal-body">
                                <div id="validation-form" role="form" class="form-horizontal col-md-12">
                                    <fieldset>
                                        <table class="table table-striped table-bordered">
                                            <tbody>
                                                <asp:PlaceHolder ID="phCampos" runat="server"></asp:PlaceHolder>
                                            </tbody>
                                        </table>
                                        <div class="form-group">
                                            <asp:Button ID="btnCargarTraza" runat="server" Text="Agregar" class="btn btn-success" OnClick="btnCargarTraza_Click" />
                                            <asp:Button ID="btnLimpiarTrazaNueva" runat="server" Text="Limpiar" class="btn btn-danger" OnClick="btnLimpiarTrazaNueva_Click" />
                                        </div>
                                    </fieldset>
                                </div>
                                <div class="form-group">
                                    <table class="table table-striped table-bordered">
                                        <thead>
                                            <tr>
                                                <asp:PlaceHolder ID="phCamposTrazaNueva" runat="server"></asp:PlaceHolder>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:PlaceHolder ID="phTrazabilidadNueva" runat="server"></asp:PlaceHolder>
                                        </tbody>
                                    </table>

                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:Label ID="lbMovTrazaNueva" runat="server" Visible="false"></asp:Label>
                                <asp:LinkButton ID="lbtnConfirmarTrazas" Text="<i class='shortcut-icon icon-ok'> Aceptar</i>" runat="server" class="btn btn-success" OnClick="lbtnConfirmarTrazas_Click" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>

        <div id="modalEditarDesc" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button id="btnCerrarEditarDesc" type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Editar Descripcion</h4>
                    </div>
                    <div class="modal-body" style="padding-bottom: 0px;">
                        <div role="form" class="form-horizontal col-md-12">
                            <asp:UpdatePanel ID="UpdatePanel13" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <div class="form-group">
                                        <label class="col-md-4">Descripcion item:</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtDescripcionItemRefact" runat="server" class="form-control" TextMode="MultiLine" Rows="4" />
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="txtDescripcionItemRefact" runat="server" ForeColor="Red" Font-Bold="true" SetFocusOnError="true" ValidationGroup="EditarDescGroup" />
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:UpdatePanel ID="UpdatePanel14" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="lblEditarDescRefacturar" runat="server" Text="0"></asp:Label>
                                <asp:LinkButton ID="lbtnEditarDescRefacturar" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" ValidationGroup="EditarDescGroup" OnClick="lbtnEditarDescRefacturar_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>

        <div id="modalPorcentajeVenta" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">

                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Porcentaje cantidades a facturar</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <div class="form-group">
                                <label for="validateSelect" class="col-md-3">Porcentaje</label>
                                <div class="col-md-5">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtPorcentajeCantFacturar" runat="server" class="form-control" TextMode="Number" Text="100" />
                                        <span class="input-group-addon">%</span>
                                    </div>
                                </div>
                                <div class="col-md-1">
                                    <asp:RequiredFieldValidator ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="PorcentajeCantGroup" ControlToValidate="txtPorcentajeCantFacturar" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button Text="Aceptar" ID="btnCambiarPorcentajeCantidadFacturar" runat="server" class="btn btn-success" ValidationGroup="PorcentajeCantGroup" OnClick="btnCambiarPorcentajeCantidadFacturar_Click" />
                    </div>

                </div>
            </div>
        </div>

        <div id="modalCalcularDescuentoConUnMonto" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button id="btnCerrarModalDescuentoMonto" type="button" class="close" data-dismiss="modal" aria-hidden="true" style="display: none;">×</button>
                        <h4 class="modal-title">Aplicar descuento</h4>
                    </div>
                    <div class="modal-body" style="padding-bottom: 0px;">

                        <div role="form" class="form-horizontal col-md-12">
                            <div class="form-group">
                                <label for="validateSelect" class="col-md-3">Monto de descuento</label>
                                <div class="col-md-5">
                                    <div class="input-group">
                                        <asp:TextBox ID="txtMontoParaAplicarDescuentoAlTotal" runat="server" class="form-control" TextMode="Number" Text="100" ValidationGroup="MontoDescuentoGroup" />
                                        <span class="input-group-addon">$</span>
                                    </div>
                                </div>
                                <div class="col-md-1">
                                    <asp:RequiredFieldValidator ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="PorcentajeCantGroup" ControlToValidate="txtPorcentajeCantFacturar" runat="server" />
                                </div>
                            </div>
                        </div>

                    </div>
                    <div class="modal-footer">
                        <asp:UpdatePanel ID="updatePanelAgregarMontoParaCalcularPorcentajeDescuento" runat="server">
                            <ContentTemplate>
                                <asp:Label ID="Label16" runat="server" Style="display: none;"></asp:Label>
                                <asp:LinkButton ID="lbtnAgregarMontoParaCalcularPorcentajeDescuento" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success ui-tooltip" data-toggle="tooltip" title data-original-title="Agregar descuento" ValidationGroup="MontoDescuentoGroup" OnClick="lbtnAgregarMontoParaCalcularPorcentajeDescuento_Click" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                </div>
            </div>
        </div>

        <div id="modalBuscarArticuloDescripcion" onkeypress="javascript:return validarEnter(event)" class="modal fade" tabindex="-1" role="dialog">
            <asp:Panel ID="Panel2" runat="server">                
                <div class="modal-dialog" style="width: 60%;">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" id="btnCerrarModalBuscarArticulo" onclick="CerrarModalBuscarArticulo()" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h4 class="modal-title">Busqueda de Articulos</h4>
                        </div>
                        <div class="modal-body">
                            <div role="form" class="form-horizontal col-md-12">
                                <div class="form-group">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label for="name" class="col-md-4">Buscar Articulo</label>
                                            <div class="col-md-3">
                                                <asp:TextBox ID="txtDescripcionArticulo" class="form-control" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-md-1">
                                                <button ID="btnBuscarArticuloDescripcion" type="button" onclick="CargarArticulos()" class="btn btn-info"><span class='shortcut-icon icon-search'></span></button>
                                            </div>
                                            <asp:UpdateProgress ID="UpdateProgress3" runat="server">
                                                <ProgressTemplate>
                                                    <div class="col-md-4">
                                                            <i class="fa fa-spinner fa-spin" id="spinnerCargandoArticulos"></i>
                                                        <label id="lblCargandoArticulo" class="col-md-10">Cargando articulo por favor aguarde.</label>
                                                    </div>
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </div>
                                    </div>
                                </div>
                                <table class="table table-striped table-bordered" id="articulosTabla">
                                    <thead>
                                        <tr>
                                            <th style="width: 10%">Codigo</th>
                                            <th style="width: 20%">Descripcion</th>
                                            <th style="width: 10%">Stock</th>
                                            <th style="width: 10%">Moneda</th>
                                            <th style="width: 10%">P.Venta</th>
                                            <th style="width: 20%"></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:PlaceHolder ID="phBuscarArticulo" runat="server"></asp:PlaceHolder>
                                    </tbody>
                                </table>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnAgregarArticulosBuscadosATablaItems" UseSubmitBehavior="false" OnClientClick="AgregarArticulosMultiples()" Text="Agregar" runat="server" class="btn btn-success"/>
                                <button type="button" onclick="CerrarModalBuscarArticulo()" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>

        <div id="modalBuscarClienteDescripcion" onkeypress="javascript:return validarEnter(event)" class="modal fade" tabindex="-1" role="dialog">
            <asp:Panel ID="Panel1" runat="server">
                <div class="modal-dialog" style="width: 60%;">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" id="btnCerrarModalBuscarCliente" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h4 class="modal-title">Busqueda de Clientes</h4>
                        </div>
                        <div class="modal-body">
                            <div role="form" class="form-horizontal col-md-12">
                                <div class="form-group">
                                    <div class="col-md-12">
                                        <div class="form-group">
                                            <label for="name" class="col-md-4">Buscar Cliente</label>
                                            <div class="col-md-3">
                                                <asp:TextBox ID="txtDescripcionCliente" class="form-control" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-md-1">
                                                <button ID="btnBuscarClienteDescripcion" type="button" onclick="BuscarClientes()" class="btn btn-info"><span class='shortcut-icon icon-search'></span></button>
                                            </div>
                                            <asp:UpdateProgress ID="UpdateProgress4" runat="server">
                                                <ProgressTemplate>
                                                    <div class="col-md-4">
                                                            <i class="fa fa-spinner fa-spin" id="spinnerCargandoClientes"></i>
                                                        <label id="lblCargandoCliente" class="col-md-10">Cargando cliente por favor aguarde.</label>
                                                    </div>
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </div>
                                    </div>
                                </div>
                                <table class="table table-striped table-bordered" id="clientesTabla">
                                    <thead>
                                        <tr>
                                            <th style="width: 10%">Codigo</th>
                                            <th style="width: 20%">Razon Social</th>
                                            <th style="width: 20%">Alias</th>
                                            <th style="width: 5%"></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:PlaceHolder ID="phBuscarCliente" runat="server"></asp:PlaceHolder>
                                    </tbody>
                                </table>
                            </div>
                            <div class="modal-footer">
                                <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <%--Fin modalGrupo--%>
    </div>
    <!-- /main -->

    <link href="../../css/pages/reports.css" rel="stylesheet">
    <!-- Core Scripts - Include with every page -->
    <script src="../../Scripts/jquery-1.10.2.js"></script>

    <script src="../../Scripts/libs/jquery-1.9.1.min.js"></script>
    <script src="../../Scripts/libs/jquery-ui-1.10.0.custom.min.js"></script>
    <script src="//cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>
    <script src="//cdn.datatables.net/plug-ins/1.10.9/sorting/date-eu.js"></script>
    <script src="../../../Scripts/plugins/dataTables/custom.tables.js"></script>
    <link href="//cdn.datatables.net/1.10.2/css/jquery.dataTables.css" rel="stylesheet" />
    <script src="../../Scripts/libs/bootstrap.min.js"></script>

    <script src="../../Scripts/plugins/hoverIntent/jquery.hoverIntent.minified.js"></script>

    <script src="../../Scripts/Application.js"></script>

    <script src="../../Scripts/demo/gallery.js"></script>

    <script src="../../Scripts/plugins/msgGrowl/js/msgGrowl.js"></script>
    <script src="../../Scripts/plugins/lightbox/jquery.lightbox.min.js"></script>
    <script src="../../Scripts/plugins/msgbox/jquery.msgbox.min.js"></script>
    <script src="../../Scripts/demo/notifications.js"></script>

    <script src="../../Scripts/bootstrap.min.js"></script>    

    <script>
        function pageLoad() {
            $("#<%= txtFecha.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%= txtFechaEntrega.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%= txtFechaSolicitudManual.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%= txtFechaNacimientoCredito.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%= txtFechaVtoCuotaMutual.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%= txtFechaPagareMutual.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%= lbtnAnticipo.ClientID %>").tooltip();
            $("#<%= lbtnAnticipoMutual.ClientID %>").tooltip();
            $("#<%= btnEnviarCodigoCredito.ClientID %>").tooltip();
            $("#<%= lbtnNoValidar.ClientID %>").tooltip();
            $("#<%= lblMovSolicitud.ClientID %>").tooltip();
            $("#<%= lbtnVolverValidar.ClientID %>").tooltip();
            $("#<%= btnLimpiarProcesoCredito.ClientID %>").tooltip();
            $("#<%= lbtnBuscarSolicitudes.ClientID %>").tooltip();
            $("#<%= btnCredito.ClientID %>").tooltip();
            $("#<%= btnMutual.ClientID %>").tooltip();
            $("#<%= lbtnVerCtaCte.ClientID %>").tooltip();
            $("#<%= btnCierreZ.ClientID %>").tooltip();
            $("#<%= txtMailEntrega.ClientID %>").popover();
            $("#<%= txtCapitalSolicitudManual.ClientID %>").popover();

            $(window).on('shown.bs.modal', function ()
            {
                var ddlSucursal = document.getElementById("MainContent_ListSucursal");
                var idSucursal = ddlSucursal.selectedOptions[0].value;

                if (idSucursal <= 0)
                {
                    document.getElementById('btnCerrarModalBuscarArticulo').click();
                    CerrarModalBuscarArticulo();
                }
            });

            var updateProgress3 = $get('<%= UpdateProgress3.ClientID %>');
            var dynamicLayout3 = '<%= UpdateProgress3.DynamicLayout.ToString().ToLower() %>';

            if (dynamicLayout3)
            {
                updateProgress3.style.display = "block";
            }
            else
            {
                updateProgress3.style.visibility = "visible";
            }

            var updateProgress4 = $get('<%= UpdateProgress4.ClientID %>');
            var dynamicLayout4 = '<%= UpdateProgress4.DynamicLayout.ToString().ToLower() %>';

            if (dynamicLayout4)
            {
                updateProgress4.style.display = "block";
            }
            else
            {
                updateProgress4.style.visibility = "visible";
            }
        }
    </script>

    <script>
        $(function () {
            $("#<%= txtFechaEntrega.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%= txtFechaSolicitudManual.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });
    </script>
    <script>

        $(function () {
            $("#<%= txtFecha.ClientID %>").datepicker('option', { dateFormat: 'dd/mm/yy' });
        });        

    </script>    
    <script>
        function BuscarArticulo(descripcion,idSucursal)
        {
            if (idSucursal <= 0)
            {
                $.msgbox("Debe seleccionar una sucursal!", { type: "alert" });
            }
            else
            {
                var btnBuscarArticulosDescripcion = document.getElementById("btnBuscarArticuloDescripcion");
                btnBuscarArticulosDescripcion.disabled = true;

                $.ajax({
                    type: "POST",
                    url: "ABMFacturasLargo.aspx/BuscarArticulosPorDescripcion",
                    data: '{codigoArticulo: "' + descripcion + '", idSucursal: "' + idSucursal + '"}',
                    contentType: "application/json",
                    dataType: 'json',
                    error: function () {
                        $.msgbox("No se pudo buscar el articulo!", { type: "error" });
                        BuscarArticulo("", idSucursal);
                    },
                    success: OnSuccessBuscarArticulo
                });
            }            
        }

        function CerrarModalBuscarArticulo()
        {
            $.ajax({
                type: "POST",
                url: "ABMFacturasLargo.aspx/CerrarModalBuscarArticulosPorDescripcion",
                contentType: "application/json",
                dataType: 'json',
                error: function ()
                {
                    $.msgbox("No se pudo cerrar el modal!", { type: "error" });
                }
            });
        }

        function OnSuccessBuscarArticulo(response)
        {
            var btnBuscarArticulosDescripcion = document.getElementById("btnBuscarArticuloDescripcion");
            btnBuscarArticulosDescripcion.disabled = false;            

            var data = response.d;
            var obj = JSON.parse(data);            

            $("#articulosTabla").dataTable().fnDestroy();
            $('#articulosTabla').find("tr:gt(0)").remove();

            for (var i = 0; i < obj.length; i++)
            {
                $('#articulosTabla').append(
                    "<tr> " +
                    "<td> " + obj[i].codigo + "</td>" +
                    "<td> " + obj[i].descripcion + "</td>" +
                    "<td> " + obj[i].stock + "</td>" +
                    "<td> " + obj[i].moneda + "</td>" +
                    '<td style="text-align:right"> ' + obj[i].precioVenta + "</td>" +
                    "<td> " + CrearBotonesAccion(obj[i].codigo) + "</td>" +
                    "</tr> ");
            };

            $('#articulosTabla').on("click", "button[name=\"btnAgregarArticulo\"]", function (button)
            {
                AgregarArticuloBuscadoPorDescripcion(button);
            });

            document.getElementById("MainContent_txtDescripcionArticulo").value = "";

            var lblCargandoArticulo = document.getElementById("lblCargandoArticulo");
            lblCargandoArticulo.innerHTML = "";

            $("#spinnerCargandoArticulos").hide();
        }

        function CrearBotonesAccion(codigo)
        {
            var accion = "";

            accion += "<button id='btn_" + codigo + "' name='btnAgregarArticulo' class='btn btn-info' > <span class='shortcut-icon icon-ok'></span></button > ";
            accion += "<span class=\"btn btn-info\" style=\"font-size:7pt;\"><input id='input_" + codigo + "' type=\"checkbox\"></span> "

            return accion;
        }

        function OnSuccessCargarClientes(response)
        {
            var btnBuscarClienteDescripcion = document.getElementById("btnBuscarClienteDescripcion");
            btnBuscarClienteDescripcion.disabled = false;

            var data = response.d;
            var obj = JSON.parse(data);

            $("#clientesTabla").dataTable().fnDestroy();
            $('#clientesTabla').find("tr:gt(0)").remove();

            for (var i = 0; i < obj.length; i++)
            {
                $('#clientesTabla').append(
                    "<tr> " +
                    "<td> " + obj[i].codigo + "</td>" +
                    "<td> " + obj[i].razonSocial + "</td>" +
                    "<td> " + obj[i].alias + "</td>" +
                    "<td> " + CrearBotonesAccionCliente(obj[i].id) + "</td>" +
                    "</tr> ");
            };

            $('#clientesTabla').on("click", "button[name=\"btnAgregarCliente\"]", function (button)
            {
                AgregarCliente(button);
            });
                        
            document.getElementById("MainContent_txtDescripcionCliente").value = "";

            var lblCargandoCliente = document.getElementById("lblCargandoCliente");
            lblCargandoCliente.innerHTML = "";

            $("#spinnerCargandoClientes").hide();
        }

        function CrearBotonesAccionCliente(id)
        {
            var accion = "";

            accion += "<button id='btn_" + id + "' name='btnAgregarCliente' class='btn btn-info' > <span class='shortcut-icon icon-ok'></span></button > ";

            return accion;
        }
    </script>

    <script type="text/javascript">
        function CargarArticulos()
        {
            var ddlSucursal = document.getElementById("MainContent_ListSucursal");
            var idSucursal = ddlSucursal.selectedOptions[0].value;

            var lblCargandoArticulo = document.getElementById("lblCargandoArticulo");
            lblCargandoArticulo.innerHTML = "Cargando articulos por favor aguarde.";

            $("#spinnerCargandoArticulos").show();
            
            var descripcionArticulo = document.getElementById("MainContent_txtDescripcionArticulo");

            BuscarArticulo(descripcionArticulo.value, idSucursal);
        }

        function AgregarArticuloBuscadoPorDescripcion(button)
        {
            var descripcionArticulo = document.getElementById("MainContent_txtCodigo");            

            descripcionArticulo.value = button.currentTarget.id.replace("btn_","");

            $.ajax({
                type: "POST",
                url: "ABMFacturasLargo.aspx/AgregarArticulosPorDescripcion",
                data: '{codigoArticulo: "' + descripcionArticulo.value + '"}',
                contentType: "application/json",
                dataType: 'json',
                error: function ()
                {
                    $.msgbox("No se pudo agregar el articulo!", {type: "error"});
                }
            });
        }

        function AgregarArticulosMultiples()
        {
            var btnAgregarArticulosMultiple = document.getElementById("MainContent_btnAgregarArticulosBuscadosATablaItems");
            btnAgregarArticulosMultiple.disabled = true;
            btnAgregarArticulosMultiple.value = "Aguarde...";

            var ddlSucursal = document.getElementById("MainContent_ListSucursal");
            var idSucursal = ddlSucursal.selectedOptions[0].value;

            var checkedNodes = $('#articulosTabla').find('input[type="checkbox"]:checked');

            var codigosArticulos = "";

            for (var i = 0; i < checkedNodes.length; i++)
            {
                codigosArticulos += checkedNodes[i].id.replace("input_","") + ";";
            }

            $.ajax({
                type: "POST",
                url: "ABMFacturasLargo.aspx/AgregarMultiplesArticulosPorDescripcion",
                data: '{codigosArticulos: "' + codigosArticulos + '"}',
                contentType: "application/json",
                dataType: 'json',
                error: function ()
                {
                    $.msgbox("No se pudieron agregar los articulos!", { type: "error" });
                }
            });
        }

        function AgregarCliente(button)
        {
            var idCliente = button.currentTarget.id.replace("btn_","");

            $.ajax({
                type: "POST",
                url: "ABMFacturasLargo.aspx/AgregarCliente",
                data: '{idCliente: "' + idCliente + '"}',
                contentType: "application/json",
                dataType: 'json',
                error: function ()
                {
                    $.msgbox("No se pudo agregar el cliente!", {type: "error"});
                }
            });
        }

        function CargarClientes()
        {
            $.ajax({
                type: "POST",
                url: "ABMFacturasLargo.aspx/CargarClientes",
                contentType: "application/json",
                dataType: 'json',
                error: function ()
                {
                    $.msgbox("No se pudo cargar el cliente!", { type: "error" });
                },
                success: OnSuccessCargarClientes
            });
        }

        function BuscarClientes()
        {
            var btnBuscarClienteDescripcion = document.getElementById("btnBuscarClienteDescripcion");
            btnBuscarClienteDescripcion.disabled = true;

            var lblCargandoCliente = document.getElementById("lblCargandoCliente");
            lblCargandoCliente.innerHTML = "Cargando cliente por favor aguarde.";

            $("#spinnerCargandoClientes").show();

            var descripcionCliente = document.getElementById("MainContent_txtDescripcionCliente").value;

            $.ajax({
                type: "POST",
                url: "ABMFacturasLargo.aspx/BuscarCliente",
                data: '{razonSocial: "' + descripcionCliente + '"}',
                contentType: "application/json",
                dataType: 'json',
                error: function ()
                {
                    $.msgbox("No se pudo buscar el cliente!", { type: "error" });
                },
                success: OnSuccessCargarClientes
            });
            }

        function BuscarClienteDefaultButton()
        {
            $("#btnBuscarClienteDescripcion").click();
        }
        function BuscarArticuloDefaultButton()
        {
            $("#btnBuscarArticuloDescripcion").click();
        }
    </script>
    <script>

        $(function ()
        {
            var modal = document.getElementById('modalBuscarArticuloDescripcion');
            // When the user clicks anywhere outside of the modal, close it
            window.onclick = function (event)
            {
                if (event.target == modal)
                {
                    modal.style.display = "none";
                    CerrarModalBuscarArticulo();
                }
            }
        });

    </script>

    <script type="text/javascript">

        function darclick() {
            document.getElementById("<%= this.lbtnAgregarArticuloASP.ClientID %>").click();
        }

        function bloquear() {
            if (Page_ClientValidate("TelefonoGroup")) {
                document.getElementById("<%= this.btnEnviarCodigoCredito.ClientID %>").setAttribute("disabled", "disabled");
            }
        }
        function desbloquearEnvioCod() {
            document.getElementById("<%= this.btnEnviarCodigoCredito.ClientID %>").removeAttribute("disabled");
        }

        function desbloquear() {
            if (!Page_ClientValidate("FacturaGroup")) {
                document.getElementById("<%= this.btnAgregarRemitir.ClientID %>").removeAttribute("disabled");
                document.getElementById("<%= this.btnAgregarRemitir.ClientID %>").removeAttribute("style");
                document.getElementById("<%= this.btnAgregar.ClientID %>").removeAttribute("disabled");
                document.getElementById("<%= this.btnAgregar.ClientID %>").removeAttribute("style");
            }
        }

        function foco()
        {
            var modalArticulosVisible = $('#modalBuscarArticuloDescripcion').is(':visible');
            var modalClientesVisible = $('#modalBuscarClienteDescripcion').is(':visible');

            if (!modalArticulosVisible && !modalClientesVisible)
            {
                document.getElementById("<%= this.txtCantidad.ClientID %>").focus();
                var note = document.getElementById("<%= this.txtCantidad.ClientID %>");
                var screenPosition = note.getBoundingClientRect();

                window.scrollTo(0, screenPosition.bottom / 2);
            }            
        }

        function focoDesc() {
            <%--document.getElementById("<%= this.TxtDescuentoArri.ClientID %>").focus();--%>
            document.getElementById("<%= this.txtPUnitario.ClientID %>").focus();
        }

    </script>

    <script>

        function updateboxCredito(valor, id) {
            var textboxAnticipo = document.getElementById("<%=txtAnticipo.ClientID%>");
            var textboxAnticipoManual = document.getElementById("<%=txtAnticipoSolicitudManual.ClientID%>");
            var chk1 = document.getElementById("cbSeleccion_" + id);

            if (chk1.checked) {
                textboxAnticipo.value = parseFloat(parseFloat(textboxAnticipo.value) + Math.abs(parseFloat(valor))).toFixed(2);
                textboxAnticipoManual.value = parseFloat(parseFloat(textboxAnticipoManual.value) + Math.abs(parseFloat(valor))).toFixed(2);
            }
            else {
                textboxAnticipo.value = parseFloat(parseFloat(textboxAnticipo.value) - Math.abs(parseFloat(valor))).toFixed(2)
                textboxAnticipoManual.value = parseFloat(parseFloat(textboxAnticipoManual.value) - Math.abs(parseFloat(valor))).toFixed(2)
            }

        }

        function updateboxMutual(valor, id) {
            var textboxAnticipoMutual = document.getElementById("<%=txtAnticipoMutual.ClientID%>");
            var chk1 = document.getElementById("cbSeleccionMutual_" + id);

            if (chk1.checked) {
                textboxAnticipoMutual.value = parseFloat(parseFloat(textboxAnticipoMutual.value) + Math.abs(parseFloat(valor))).toFixed(2);
            }
            else {
                textboxAnticipoMutual.value = parseFloat(parseFloat(textboxAnticipoMutual.value) - Math.abs(parseFloat(valor))).toFixed(2)
            }

            __doPostBack("txtAnticipoMutual", "TextChanged");

        }

        function updatebox(valor, id) {
            var cantActual = document.getElementById("<%=lblTrazaActual.ClientID%>").textContent;
            var cantTotal = document.getElementById("<%=lblTrazaTotal.ClientID%>").textContent;

            var chk1 = document.getElementById(id);
            if (cantActual == cantTotal) {
                if (chk1.checked == false) {
                    cantActual = parseInt(parseInt(cantActual) - 1);
                    document.getElementById('<%= lblTrazaActual.ClientID %>').textContent = cantActual;
                }

                document.getElementById(id).checked = false;
            }
            else {
                if (chk1.checked) {
                    cantActual = parseInt(parseInt(cantActual) + 1);
                }
                else {
                    cantActual = parseInt(parseInt(cantActual) - 1);
                }
                document.getElementById('<%= lblTrazaActual.ClientID %>').textContent = cantActual;
            }

        }
        function updateNroSolicitud(valor, id, anticipo) {


            var chk1 = document.getElementById(id);
            if (chk1.checked) {
                document.getElementById('<%=txtNroSolicitud.ClientID%>').value = valor;
                document.getElementById('<%=txtAnticipo.ClientID%>').value = anticipo;
                document.getElementById('<%=txtAnticipoSolicitudManual.ClientID%>').value = anticipo;
            }
            else {
                document.getElementById('<%= txtNroSolicitud.ClientID %>').value = "";
                document.getElementById('<%=txtAnticipo.ClientID%>').value = "";
                <%--document.getElementById('<%=txtAnticipoSolicitudManual.ClientID%>').value = ""--%>;
            }

        }
    </script>

    <script src="../../js/daypilot-modal-2.0.js"></script>
    <script>
        function abrirdialog() {
            document.getElementById('abreDialog').click();
        }
        function abrirdialog2() {
            document.getElementById('abreDialog2').click();
        }
        function abrirdialog3() {
            document.getElementById('abreDialog3').click();
        }
        function abrirCargaTraza() {
            $('#modalCargaTrazabilidad').modal('show');
        }

    </script>

    <script>
        function abrirModalMutuales() {
            $('#modalMutuales').modal('show');
        }
    </script>
    <script>
        function abrirModalBuscarArticulo() {
            $('#modalBuscarArticuloDescripcion').modal('show');
        }
        function cerrarModalBuscarArticulo() {
            $('#modalBuscarArticuloDescripcion').modal('hide');
        }
    </script>
    <script>
        function modalCalcularDescuentoConUnMonto() {
            $('#modal').modal('show');
        }
    </script>

    <script>
        function cerrarModal() {
            document.getElementById('btnCerrarTraza').click();
        }
        function cerrarModal2() {
            document.getElementById('btnCerrarTraza2').click();
        }
        function cerrarModalTarjeta() {
            document.getElementById('btnCerrarTarjetas').click();
        }
        function cerrarModalCredito() {
            document.getElementById('btnCerrarCreditos2').click();
        }
        function cerrarModalDatosExtra() {
            document.getElementById('btnCerrarDatosExtra').click();
        }
        function cerrarModalMutuales() {
            document.getElementById('btnCerrarMutuales').click();
        }
        function cerrarModalEditarDesc() {
            document.getElementById('btnCerrarEditarDesc').click();
        }
        function clickTab() {
            document.getElementById("<%= this.linkCombustible.ClientID %>").click();
        }
        function cerrarModalDescuentoMonto() {
            document.getElementById('btnCerrarModalDescuentoMonto').click();
        }
    </script>

    <script>
        function openModal(cod, nro, text) {
            $('#modalEnvioSMS').modal('show');
            document.getElementById("<%= this.txtCodArea.ClientID %>").value = cod;
            document.getElementById("<%= this.txtTelefono.ClientID %>").value = nro;
            document.getElementById("<%= this.txtMensajeSMS.ClientID %>").value = text;
        }
        function alertaSMSSaldoMax(text) {

            document.getElementById("<%= this.lblAvisoSMSSaldoMax.ClientID %>").value = text;
        }
    </script>
    <script>

        function createC() {
            //var d = document.getElementById("TheBody_txtDescripcion").value;
            //              var resource = d.options[d.selectedIndex].value;

            var modal = new DayPilot.Modal();
            modal.closed = function () {
                if (this.result == "OK") {
                    __doPostBack("UpdateButton", "");
                }
            };
            //modal.showUrl("ModalCreate.aspx?start=" + start + "&resource=" + resource);
            modal.showUrl("BuscarCliente.aspx?accion=1");
        }

        function createA() {
            //var d = document.getElementById("TheBody_txtDescripcion").value;
            //              var resource = d.options[d.selectedIndex].value;
            document.getElementById("<%= this.txtCantidad.ClientID %>").focus();

            var ddlSucursal = document.getElementById("MainContent_ListSucursal");
            var idSucursal = ddlSucursal.selectedOptions[0].value;

            //ListSucursalCliente
            var modal = new DayPilot.Modal();
            modal.closed = function () {
                if (this.result == "OK") {
                    __doPostBack("UpdateButton", "");
                }
            };
            //modal.showUrl("ModalCreate.aspx?start=" + start + "&resource=" + resource);
            modal.showUrl("BuscarArticulos.aspx?accion=1&suc=" + idSucursal);
        }

        function edit(id) {
            var modal = new DayPilot.Modal();
            modal.closed = function () {
                if (this.result == "OK") {
                    __doPostBack("UpdateButton", "");
                }
            };
            modal.showUrl("ModalEdit.aspx?id=" + id);
        }




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
                if (key == 46 || key == 8)// || key == 44) // Detectar . (punto) , backspace (retroceso) y , (coma)
                { return true; }
                else { return false; }
            }
            return true;
        }
        //valida los campos solo numeros
        function validarNroSinComa(e) {
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
                if (key == 8)// || key == 44) // Detectar . (punto) , backspace (retroceso) y , (coma)
                { return true; }
                else { return false; }
            }
            return true;
        }
        function validarEnter(e) {
            var key;
            if (window.event) // IE
            {
                key = e.keyCode;
            }
            else if (e.which) // Netscape/Firefox/Opera
            {
                key = e.which;
            }

            if (key == 13) {
                return false;
            }
            return true;
        }
    </script>


</asp:Content>
