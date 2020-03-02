<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ABMFacturas.aspx.cs" Inherits="Gestion_Web.Formularios.Facturas.ABMFacturas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">

        <div class="container">

            <div class="row">
                <div class="col-md-12 col-xs-12">
                    <div class="widget-content">
                        <section id="accordions">
                            <div class="panel-group accordion" id="acordions">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 95%">
                                                    <h4 class="panel-title">
                                                        <a class="accordion-toggle" data-toggle="collapse" data-parent=".accordion" href="#collapseOne">
                                                            <asp:UpdatePanel runat="server" UpdateMode="Always">
                                                                <ContentTemplate>
                                                                    <label id="lblAcordeon1" runat="server">Datos</label>
                                                                    |
                                                                    <label id="lblAcordeonFPLista" runat="server">Datos</label>
                                                                    |
                                                                    <label id="lblAcordeonNumero" runat="server"></label>
                                                                    |
                                                                    <label id="lblAcordeonSuc" runat="server"></label>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </a>
                                                    </h4>
                                                </td>
                                                <td style="width: 5%">

                                                    <asp:LinkButton ID="LinkButton2" runat="server" Text="<span class='shortcut-icon icon-plus'></span>" class="accordion-toggle" data-toggle="collapse" data-parent=".accordion" href="#collapseOne" Style="color: black" />

                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="collapseOne" class="panel-collapse collapse">
                                        <div class="panel-body">
                                            <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Always" runat="server">
                                                <ContentTemplate>
                                                    <div class="col-md-12" style="padding:0px;">
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
                                                                                <asp:DropDownList ID="DropListClientes" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="DropListClientes_SelectedIndexChanged"></asp:DropDownList>
                                                                            </div>
                                                                            <div class="col-md-2">
                                                                                <asp:Panel ID="panelBusquedaCliente" runat="server">
                                                                                    <a class="btn btn-info" onclick="createC();">
                                                                                        <i class="shortcut-icon icon-search"></i>
                                                                                    </a>
                                                                                </asp:Panel>
                                                                            </div>
                                                                            <div class="col-md-2">
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
                                                                                <div class="col-md-12">
                                                                                    <asp:DropDownList ID="ListSucursalCliente" Visible="false" runat="server" class="form-control"></asp:DropDownList>
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
                                                                                    <asp:LinkButton ID="btnCierreZ" class="btn btn-info" runat="server" Text="Z" Visible="false" OnClick="btnCierreZ_Click" />
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

                                                    <div class="col-md-12" style="padding:0px;">
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

                                                                        <div class="col-md-2">
                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListVendedor" InitialValue="-1" ValidationGroup="FacturaGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                        </div>
                                                                    </th>
                                                                    <th style="width: 35%">
                                                                        <div class="col-md-8">
                                                                            <asp:DropDownList ID="DropListFormaPago" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="DropListFormaPago_SelectedIndexChanged"></asp:DropDownList>
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
                                                                        </div>
                                                                        <div class="col-md-2">
                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListFormaPago" InitialValue="-1" ValidationGroup="FacturaGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                        </div>
                                                                    </th>
                                                                    <th style="width: 25%">
                                                                        <div class="col-md-9">
                                                                            <asp:DropDownList ID="DropListLista" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="DropListLista_SelectedIndexChanged"></asp:DropDownList>
                                                                        </div>
                                                                        <%--<div class="col-md-2">
                                                            <a class="btn btn-info" data-toggle="modal" href="#modalLista">
                                                                <i class="shortcut-icon icon-plus"></i>
                                                            </a>
                                                        </div>--%>

                                                                        <div class="col-md-1">
                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListLista" InitialValue="-1" ValidationGroup="FacturaGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                        </div>
                                                                    </th>

                                                                    <th style="width: 5%">
                                                                        <asp:DropDownList ID="ListDocumentos" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ListDocumentos_SelectedIndexChanged">
                                                                            <asp:ListItem>FC</asp:ListItem>
                                                                            <asp:ListItem>NC</asp:ListItem>
                                                                            <asp:ListItem>ND</asp:ListItem>
                                                                            <asp:ListItem>PRP</asp:ListItem>
                                                                            <asp:ListItem>NC PRP</asp:ListItem>
                                                                            <asp:ListItem>ND PRP</asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <div class="btn-group" style="display: none;">
                                                                            <asp:LinkButton ID="lbtnAccion" runat="server" class="btn btn-info dropdown-toggle" data-toggle="dropdown" Text="FC <span class='caret'></span>" />
                                                                            <ul class="dropdown-menu">

                                                                                <li>
                                                                                    <asp:LinkButton ID="lbtnFC" runat="server" OnClick="lbtnFC_Click" Style="height: 50%;">FC</asp:LinkButton>

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
                                </div>
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 95%">
                                                    <h4 class="panel-title">
                                                        <a class="accordion-toggle" data-toggle="collapse" data-parent=".accordion" href="#collapseTwo">
                                                            <asp:UpdatePanel runat="server">
                                                                <ContentTemplate>
                                                                    <label id="lblAcordeon2" runat="server">Detalle</label>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>

                                                        </a>
                                                    </h4>
                                                </td>
                                                <td style="width: 5%">

                                                    <asp:LinkButton ID="LinkButton3" runat="server" Text="<span class='shortcut-icon icon-plus'></span>" class="accordion-toggle" data-toggle="collapse" data-parent=".accordion" href="#collapseTwo" Style="color: black" />

                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="collapseTwo" class="panel-collapse in">
                                        <div class="panel-body">
                                            <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                                                <ContentTemplate>
                                                    <table class="table table-striped table-bordered">
                                                        <thead>
                                                            <tr>
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
                                                                <td style="width: 18%">
                                                                    <div class="form-group">
                                                                        <div class="col-md-12">
                                                                            <div class="input-group">
                                                                                <asp:TextBox ID="txtCodigo" runat="server" class="form-control"></asp:TextBox>

                                                                                <span class="input-group-btn">
                                                                                    <a class="btn btn-info" onclick="createA();">
                                                                                        <i class="shortcut-icon icon-search"></i>
                                                                                    </a>
                                                                                    <button runat="server" style="display: none;" id="btnRun" onserverclick="btnBuscarProducto_Click" onclick="foco();" class="btn btn-info" title="Search">
                                                                                        <i class="btn-icon-only icon-check-sign"></i>
                                                                                    </button>
                                                                                </span>
                                                                            </div>
                                                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCodigo" ErrorMessage="El campo es obligatorio" SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                                                                        </div>
                                                                    </div>
                                                                </td>
                                                                <td style="width: 8%">
                                                                    <asp:TextBox ID="txtCantidad" runat="server" class="form-control" AutoPostBack="True" OnTextChanged="txtCantidad_TextChanged" Style="text-align: right;"></asp:TextBox>
                                                                    <asp:LinkButton ID="lbtnStockProd" runat="server" class="badge pull-right" Text="0" OnClick="lbtnStockProd_Click"></asp:LinkButton>
                                                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="El campo es obligatorio" SetFocusOnError="True" ControlToValidate="txtCantidad"></asp:RequiredFieldValidator>--%>
                                                                </td>
                                                                <td style="width: 25%">

                                                                    <asp:TextBox ID="txtDescripcion" runat="server" class="form-control" disabled TextMode="MultiLine" MaxLength="200"></asp:TextBox>
                                                                </td>
                                                                <td style="width: 10%">
                                                                    <asp:TextBox ID="txtIva" runat="server" class="form-control" disabled></asp:TextBox>
                                                                </td>
                                                                <td style="width: 10%">

                                                                    <asp:TextBox ID="TxtDescuentoArri" runat="server" class="form-control" Text="0" OnTextChanged="TxtDescuentoArri_TextChanged" AutoPostBack="True" Style="text-align: right"></asp:TextBox>
                                                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="El campo es obligatorio" SetFocusOnError="True" ControlToValidate="TxtDescuentoArri"></asp:RequiredFieldValidator>--%>
                                                                </td>
                                                                <td style="width: 15%">

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
                                                    </table>


                                                    <a class="btn btn-info" style="display: none" data-toggle="modal" id="abreDialog" href="#modalTrazabilidad"></a>
                                                    <table class="table table-striped table-bordered">
                                                        <thead>
                                                            <tr>
                                                                <th>Codigo</th>
                                                                <th>Cantidad</th>
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
                                                </ContentTemplate>
                                                <Triggers>
                                                    <asp:PostBackTrigger ControlID="btnRun" />
                                                </Triggers>
                                            </asp:UpdatePanel>
                                            <div class="row">
                                                <asp:UpdatePanel ID="UpdatePanel5" UpdateMode="Always" runat="server">
                                                    <ContentTemplate>
                                                        <div role="form" class="form-horizontal col-md-12">
                                                            <div class="col-md-9">
                                                                <asp:Button ID="btnAgregar" runat="server" Text="Facturar" class="btn btn-success" OnClick="btnAgregar_Click" ValidationGroup="FacturaGroup" OnClientClick="desbloquear();" />
                                                                <a class="btn btn-success" data-toggle="modal" href="#modalFacturaE" runat="server" id="btnFacturaE" visible="false">Siguiente</a>
                                                                <asp:Button ID="btnAgregarRemitir" runat="server" Text="Facturar y Remitir" class="btn btn-success" ValidationGroup="FacturaGroup" OnClick="btnAgregarRemitir_Click" OnClientClick="desbloquear();" />
                                                                <asp:Button ID="btnNueva" Visible="false" runat="server" Text="Nueva Factura" class="btn btn-success" OnClick="btnNueva_Click" />
                                                                <asp:Button ID="btnRefacturar" runat="server" Visible="false" Text="Refacturar" CssClass="btn btn-success" OnClick="btnRefacturar_Click" />
                                                                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" class="btn btn-default" PostBackUrl="~/Default.aspx" />
                                                            </div>
                                                            <div class="col-md-3" style="height: 50%;">
                                                                <asp:Label ID="lblCartelTotal" runat="server" class="alert alert-success" Width="100%" Style="text-align: right; padding-top: 0px; padding-bottom: 0px;"></asp:Label>
                                                            </div>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 95%">
                                                    <h4 class="panel-title">
                                                        <a class="accordion-toggle" data-toggle="collapse" data-parent=".accordion" href="#collapseThree">
                                                            <asp:UpdatePanel runat="server" UpdateMode="Always">
                                                                <ContentTemplate>
                                                                    <label id="lblAcordeon3" runat="server">Totales - </label>
                                                                    <label id="lblAcordeonNeto" runat="server"></label>
                                                                    |
                                                                    <label id="lblAcordeonIva" runat="server"></label>
                                                                    |
                                                                    <label id="lblAcordeonSub" runat="server"></label>
                                                                    |
                                                                    <label id="lblAcordeonTotal" runat="server"></label>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </a>
                                                    </h4>
                                                </td>
                                                <td style="width: 5%">
                                                    <asp:LinkButton ID="LinkButton1" runat="server" Text="<span class='shortcut-icon icon-plus'></span>" class="accordion-toggle" data-toggle="collapse" data-parent=".accordion" href="#collapseThree" Style="color: black" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="collapseThree" class="panel-collapse collapse">
                                        <div class="panel-body">
                                            <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                                                <ContentTemplate>

                                                    <div class="row">

                                                        <div class="col-md-8">
                                                            <div role="form" class="form-horizontal col-md-12">
                                                                <table class="table table-striped table-bordered">
                                                                    <tbody>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:PlaceHolder ID="phDatosEntrega" Visible="false" runat="server">
                                                                                    <div class="form-group">
                                                                                        <label class="col-md-4">Fecha Entrega: </label>

                                                                                        <div class="col-md-8">
                                                                                            <asp:TextBox ID="txtFechaEntrega" runat="server" class="form-control"></asp:TextBox>
                                                                                        </div>
                                                                                    </div>
                                                                                    <div class="form-group">
                                                                                        <label class="col-md-4">Hora Entrega: </label>
                                                                                        <div class="col-md-8">
                                                                                            <asp:TextBox ID="txtHorarioEntrega" runat="server" class="form-control"></asp:TextBox>
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
                                                <Triggers>
                                                    <%-- <asp:AsyncPostBackTrigger ControlID="btnAgregar" EventName="Click" />--%>
                                                </Triggers>
                                            </asp:UpdatePanel>

                                        </div>
                                    </div>
                                </div>
                            </div>
                        </section>
                    </div>
                    <!-- /widget-content -->
                </div>
            </div>

            <div class="row">
                <div class="col-md-12 col-xs-12">
                    <div class="widget stacked widget-table action-table">
                        <div class="widget-content">
                        </div>


                    </div>
                    <!-- /widget-content -->
                </div>
            </div>

            <div class="row">

                <!-- /widget -->

                <div class="col-md-12">

                    <%--<div class="widget stacked">

                        <div class="widget-header">
                            <i class="icon-wrench"></i>
                            <h3>Factura

                            </h3>
                        </div>
                        <!-- /widget-header -->

                        <div class="widget-content">
                            

                        </div>
                    </div>--%>
                </div>
            </div>
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
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
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
                                                <asp:TextBox ID="txtImporteT" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)" OnTextChanged="txtImporteT_TextChanged" AutoPostBack="true"></asp:TextBox>
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
                                        <label class="col-md-6">Restan / Efectivo</label>
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
                                            <asp:LinkButton ID="lbtnConfirmarPago" class="btn btn-success" runat="server" Visible="false" Text="Confirmar pagos" />
                                        </div>
                                        <div class="col-md-4">
                                            <asp:LinkButton ID="lbtnCancelarPago" class="btn btn-default" runat="server" Visible="false" Text="Limpiar pagos" />
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
                                                <asp:ListItem Text="Brasil" Value="1"></asp:ListItem>
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

        <!-- /row -->

    </div>
    <!-- /container -->

    <%-- </div>--%>
    <!-- /main -->

    <script src="../../Scripts/libs/jquery-1.9.1.min.js"></script>
    <script src="//code.jquery.com/ui/1.11.2/jquery-ui.js"></script>
    <script>
        $(function () {

            $('.accordion').on('show', function (e) {
                $(e.target).prev('.accordion-heading').addClass('accordion-opened');
            });

            $('.accordion').on('hide', function (e) {
                $(this).find('.accordion-heading').not($(e.target)).removeClass('accordion-opened');
            });

        });
    </script>


    <%--<link href="../../css/pages/reports.css" rel="stylesheet">--%>
    <!-- Core Scripts - Include with every page -->
    <%--<script src="../../Scripts/jquery-1.10.2.js"></script>--%>
    <%--<script src="../../Scripts/bootstrap.min.js"></script>--%>
    <%--<script src="../Scripts/plugins/metisMenu/jquery.metisMenu.js"></script>--%>



    <%--<script src="../../Scripts/libs/bootstrap.min.js"></script>

    <script src="../../Scripts/plugins/hoverIntent/jquery.hoverIntent.minified.js"></script>--%>


    <%--<script src="../../Scripts/demo/gallery.js"></script>--%>
    <script src="../../Scripts/libs/jquery-ui-1.10.0.custom.min.js"></script>

    <script src="../../Scripts/plugins/msgGrowl/js/msgGrowl.js"></script>
    <script src="../../Scripts/plugins/lightbox/jquery.lightbox.min.js"></script>
    <script src="../../Scripts/plugins/msgbox/jquery.msgbox.min.js"></script>

    <%--<script src="../../Scripts/Application.js"></script>--%>

    <script src="../../Scripts/demo/notifications.js"></script>

    <script>
        function pageLoad() {
            $("#<%= txtFecha.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%= txtFechaEntrega.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        }
    </script>
    <script>
        $(function () {
            $("#<%= txtFechaEntrega.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });
    </script>
    <script>

        $(function () {
            $("#<%= txtFecha.ClientID %>").datepicker('option', { dateFormat: 'dd/mm/yy' });
        });

    </script>

    <script type="text/javascript">

        function darclick() {

            document.getElementById("<%= this.lbtnAgregarArticuloASP.ClientID %>").click();


        }

        function desbloquear() {
            if (!Page_ClientValidate("FacturaGroup")) {
                document.getElementById("<%= this.btnAgregarRemitir.ClientID %>").removeAttr("disabled");
                document.getElementById("<%= this.btnAgregarRemitir.ClientID %>").removeAttr("style");
                document.getElementById("<%= this.btnAgregar.ClientID %>").removeAttr("disabled");
                document.getElementById("<%= this.btnAgregar.ClientID %>").removeAttr("style");
            }
        }

        function foco() {
            document.getElementById("<%= this.txtCantidad.ClientID %>").focus();
            var note = document.getElementById("<%= this.txtCantidad.ClientID %>");
            var screenPosition = note.getBoundingClientRect();

            window.scrollTo(0, screenPosition.bottom / 2);
        }

        function focoDesc() {
            document.getElementById("<%= this.TxtDescuentoArri.ClientID %>").focus();
        }

    </script>

    <script>

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
    </script>
    <script src="../../js/daypilot-modal-2.0.js"></script>
    <script>
        function abrirdialog() {
            document.getElementById('abreDialog').click();
        }

    </script>
    <script>
        function cerrarModal() {
            document.getElementById('btnCerrarTraza').click();
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
            var modal = new DayPilot.Modal();
            modal.closed = function () {
                if (this.result == "OK") {
                    __doPostBack("UpdateButton", "");
                }
            };
            //modal.showUrl("ModalCreate.aspx?start=" + start + "&resource=" + resource);
            modal.showUrl("BuscarArticulos.aspx?accion=1");
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
                if (key == 46 || key == 8)//|| key == 44) // Detectar . (punto) , backspace (retroceso) y , (coma)
                { return true; }
                else
                { return false; }
            }
            return true;
        }
    </script>


</asp:Content>
