<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ABMFacturasRapido.aspx.cs" Inherits="Gestion_Web.Formularios.Facturas.ABMFacturasRapido" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">

        <div class="container">
            <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Always">
                <ContentTemplate>
                    <div class="row">
                        <div class="col-md-12 col-xs-12">
                            <asp:LinkButton ID="lbtnAvanzada" runat="server" Text="Facturacion avanzada" OnClick="lbtnAvanzada_Click"></asp:LinkButton>
                            <div class="widget stacked widget-table action-table">
                                <div class="widget-content">
                                    <div class="shortcuts">
                                        <div class="col-md-4" style="padding-top: 2%; padding-bottom: 2%;">
                                            <asp:Label ID="lbNaftaNumeroFc" runat="server" Text="Factura B Nº" class="h4 text-muted"></asp:Label>
                                        </div>
                                        <div class="col-md-5" style="padding-top: 2%; padding-bottom: 2%;">
                                            <asp:Label ID="lbNaftaCliente" runat="server" Text="" class="h4 text-muted"></asp:Label>
                                        </div>
                                        <div class="col-md-1" style="padding-top: 1%;">
                                            <asp:LinkButton ID="btnNaftaTipoFc" runat="server" class="shortcut" Visible="false" OnClick="btnNaftaTipoFc_Click" Style="width: 100%;">
                                                <asp:Label ID="lbNaftaTipoFc" runat="server" Text="FC" class="h4 text-info"></asp:Label>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnNaftaTipoPRP" runat="server" class="shortcut" Visible="true" OnClick="btnNaftaTipoPRP_Click" Style="width: 100%;">
                                                <asp:Label ID="lbNaftaTipoPRP" runat="server" Text="PRP" class="h4 text-muted"></asp:Label>
                                            </asp:LinkButton>
                                        </div>
                                        <div class="col-md-1" style="padding-top: 1%;">
                                            <a class="shortcut" onclick="createC();" style="width: 100%;">
                                                <i class="h4 icon-search"></i>
                                            </a>
                                        </div>
                                        <div class="col-md-1" style="padding-top: 1%;">
                                            <a href="#modalAltaRapida" class="shortcut" data-toggle="modal" data-toggle="tooltip" title data-original-title="Alta rapida" style="width: 100%;">
                                                <i class="h4 icon-bolt"></i>
                                            </a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12 col-xs-12">
                            <div class="widget stacked widget-table action-table">
                                <div class="widget-content">
                                    <div class="shortcuts">
                                        <div class="col-md-3" style="padding-top: 1%;">
                                            <asp:LinkButton ID="lbtnNafta1" runat="server" class="shortcut" Width="100%" OnClick="lbtnNafta1_Click" >
                                                <asp:Label ID="Label3" runat="server" Text="SUPER" class="h2" ForeColor="Red"></asp:Label>
                                            </asp:LinkButton>
                                        </div>
                                        <div class="col-md-3" style="padding-top: 1%;">
                                            <asp:LinkButton ID="lbtnNafta2" runat="server" class="shortcut" Width="100%" OnClick="lbtnNafta2_Click" >
                                                <asp:Label ID="Label4" runat="server" Text="ULTRA" class="h2" ForeColor="Green"></asp:Label>
                                            </asp:LinkButton>
                                        </div>
                                        <div class="col-md-3" style="padding-top: 1%;">
                                            <asp:LinkButton ID="lbtnNafta3" runat="server" class="shortcut" Width="100%" OnClick="lbtnNafta3_Click">
                                                <asp:Label ID="Label5" runat="server" Text="DIESEL" class="h2" ForeColor="#3399ff"></asp:Label>
                                            </asp:LinkButton>                                            
                                        </div>
                                         <div class="col-md-3" style="padding-top: 1%;">
                                            <asp:LinkButton ID="lbtnNafta4" runat="server" class="shortcut" Width="100%" OnClick="lbtnNafta4_Click" >
                                                <asp:Label ID="Label6" runat="server" Text="EURO" class="h2" ForeColor="Orange"></asp:Label>
                                            </asp:LinkButton>                                            
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row" style="display:none;">
                        <div class="col-md-12 col-xs-12">
                            <div class="widget stacked widget-table action-table">
                                <div class="widget-content">
                                    <div class="shortcuts">
                                        <div class="col-md-2" style="padding-top: 1%;">
                                            <asp:Label Text="Articulo:" runat="server" class="h4 text-muted" />
                                        </div>
                                        <div class="col-md-4" style="padding-top: 1%;padding-bottom:1%;">
                                            <asp:DropDownList ID="ListArticulosCombustibles" runat="server" class="form-control" Font-Bold="true" AutoPostBack="true" OnSelectedIndexChanged="ListArticulosCombustibles_SelectedIndexChanged" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12 col-xs-12">
                            <div class="widget stacked widget-table action-table">
                                <div class="widget-content">
                                    <div class="shortcuts">
                                        <div class="col-md-3" style="padding-top: 1%;">
                                            <asp:Label ID="lbNaftaSeleccion" runat="server" class="h1 text-muted" Text="Seleccione..."></asp:Label>
                                        </div>
                                        <div class="col-md-4" style="padding-top: 1%; padding-bottom: 1%;">
                                            <div class="col-md-4">
                                                <asp:Label ID="lblLitros" runat="server" class="h1" Text="Litros: "></asp:Label>
                                            </div>
                                            <div class="col-md-5">
                                                <asp:TextBox ID="txtLitros" runat="server" class="form-control" AutoPostBack="true"  Style="text-align: right;" ReadOnly="true" TextMode="Number" Font-Size="X-Large" Rows="4" Height="50" OnTextChanged="txtLitros_TextChanged"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="*" InitialValue="" ControlToValidate="txtLitros" ValidationGroup="NaftaGroup" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <asp:Label ID="lblRedondeoLitrosPesos" runat="server" Visible="false" Text="0" />
                                        <div class="col-md-3" style="padding-top: 1%; padding-bottom: 1%;">
                                            <div class="col-md-2">
                                                <asp:Label ID="lblPesos" runat="server" class="h1" Text="$: "></asp:Label>
                                            </div>
                                            <div class="col-md-7">
                                                <asp:TextBox ID="txtPesos" runat="server" class="form-control" AutoPostBack="true"  Style="text-align: right;" ReadOnly="true" TextMode="Number" Font-Size="X-Large" Rows="4" Height="50" OnTextChanged="txtPesos_TextChanged"></asp:TextBox>
                                            </div>
                                            <div class="col-md-3">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server" ErrorMessage="*" InitialValue="" ControlToValidate="txtPesos" ValidationGroup="NaftaGroup" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="col-md-2" style="padding-top: 1%;">
                                            <asp:Label ID="lbNaftaPrecio" runat="server" class="h1" Text="$0"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12 col-xs-12">
                            <div class="widget stacked widget-table action-table">
                                <div class="widget-content">
                                    <div class="shortcuts">
                                        <div class="col-md-6" style="padding-top: 1%;">
                                            <asp:LinkButton ID="btnNaftaContado" runat="server" class="shortcut" Style="width: 45%" OnClick="btnNaftaContado_Click" ValidationGroup="NaftaGroup">
                                                <asp:Label ID="lbContado" runat="server" Text="Contado" class="h2" ForeColor="Black"></asp:Label>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="btnNaftaCtaCte" runat="server" class="shortcut" Style="width: 45%" OnClick="btnNaftaCtaCte_Click" ValidationGroup="NaftaGroup" Visible="false">
                                                <asp:Label ID="lbCtaCte" runat="server" Text="Cta. Cte." class="h2" ForeColor="Black"></asp:Label>
                                            </asp:LinkButton>
                                        </div>
                                        <div class="col-md-6" style="padding-top: 1%;">
                                            <asp:LinkButton ID="btnNaftaTarjetaDebito" runat="server" class="shortcut" Style="width: 45%" ValidationGroup="NaftaGroup" OnClick="btnNaftaTarjetaDebito_Click">
                                                <asp:Label ID="lbTarjeta" runat="server" Text="Tarjeta" class="h2" ForeColor="Black"></asp:Label>
                                            </asp:LinkButton>                                            
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12 col-xs-12">
                            <div class="widget stacked widget-table action-table">
                                <div class="widget-content ">
                                    <div class="shortcuts col-md-6">
                                        <div class="col-md-12" style="padding-top: 10%; padding-bottom: 10%;">
                                            <label class="h1">Total: $</label>
                                            <asp:Label ID="lbNaftaImporte" runat="server" Text="0" class="h1"></asp:Label>
                                            <asp:Label ID="lbIDProvNafta" runat="server" Text="0" style="display:none;" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12 col-xs-12">
                            <div class="widget stacked widget-table action-table">
                                <div class="widget-content">
                                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                                        <ContentTemplate>
                                            <asp:Button ID="btnNaftaAceptar" runat="server" class="btn btn-info" Text="Aceptar" Style="width: 100%;" Font-Size="XX-Large" OnClick="btnNaftaAceptar_Click"></asp:Button>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>

                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>

        <div class="container" style="display: none;">

            <div class="row">
                <div class="col-md-12 col-xs-12">

                    <div class="widget stacked widget-table action-table">
                        <div class="widget-content">
                            <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Always" runat="server">
                                <ContentTemplate>
                                    <div class="col-md-12">
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
                                                                <a class="btn btn-info" onclick="createC();">
                                                                    <i class="shortcut-icon icon-search"></i>
                                                                </a>
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
                                                                    <asp:DropDownList ID="ListEmpresa" runat="server" class="form-control" AutoPostBack="True" OnSelectedIndexChanged="ListEmpresa_SelectedIndexChanged"></asp:DropDownList>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <asp:DropDownList ID="ListSucursal" runat="server" class="form-control" AutoPostBack="True" OnSelectedIndexChanged="ListSucursal_SelectedIndexChanged"></asp:DropDownList>
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

                                    <div class="col-md-12">
                                        <table class="table table-striped table-bordered">
                                            <thead>
                                                <tr>
                                                    <th style="width: 30%">
                                                        <label class="col-md-4">Vendedor</label>
                                                    </th>
                                                    <th style="width: 35%">
                                                        <label class="col-md-4">Formas Pago</label>
                                                    </th>
                                                    <th style="width: 30%">
                                                        <label class="col-md-4">Lista</label>
                                                    </th>
                                                    <th style="width: 5%">
                                                        <label class="col-md-4">Tipo</label>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th style="width: 30%">
                                                        <div class="col-md-8">
                                                            <asp:DropDownList ID="DropListVendedor" runat="server" class="form-control"></asp:DropDownList>
                                                        </div>
                                                        <div class="col-md-2">
                                                            <a class="btn btn-info" data-toggle="modal" href="#modalVendedor">
                                                                <i class="shortcut-icon icon-plus"></i>
                                                            </a>
                                                        </div>

                                                        <div class="col-md-2">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListVendedor" InitialValue="-1" ValidationGroup="FacturaGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </th>
                                                    <th style="width: 35%">
                                                        <div class="col-md-6">
                                                            <asp:DropDownList ID="DropListFormaPago" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="DropListFormaPago_SelectedIndexChanged"></asp:DropDownList>
                                                        </div>
                                                        <div class="col-md-2">
                                                            <a class="btn btn-info" data-toggle="modal" href="#modalFormaPago">
                                                                <i class="shortcut-icon icon-plus"></i>
                                                            </a>
                                                        </div>
                                                        <div class="col-md-2">
                                                            <a class="btn btn-info" data-toggle="modal" href="#modalTarjeta" runat="server" id="btnTarjeta" visible="false">
                                                                <i class="shortcut-icon icon-credit-card"></i>
                                                            </a>
                                                        </div>
                                                        <div class="col-md-2">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListFormaPago" InitialValue="-1" ValidationGroup="FacturaGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </th>
                                                    <th style="width: 30%">
                                                        <div class="col-md-6">
                                                            <asp:DropDownList ID="DropListLista" runat="server" class="form-control"></asp:DropDownList>
                                                        </div>
                                                        <div class="col-md-2">
                                                            <a class="btn btn-info" data-toggle="modal" href="#modalLista">
                                                                <i class="shortcut-icon icon-plus"></i>
                                                            </a>
                                                        </div>

                                                        <div class="col-md-2">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListLista" InitialValue="-1" ValidationGroup="FacturaGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
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
                                                                    <asp:LinkButton ID="lbtnNC" runat="server" OnClick="lbtnNC_Click">NC</asp:LinkButton>

                                                                </li>

                                                                <li>
                                                                    <asp:LinkButton ID="lbtnND" runat="server" OnClick="lbtnND_Click">ND</asp:LinkButton>

                                                                </li>
                                                                <li class="divider"></li>

                                                                <li>
                                                                    <asp:LinkButton ID="lbtnPRP" runat="server" OnClick="lbtnPRP_Click">PRP</asp:LinkButton>

                                                                </li>
                                                                <li>
                                                                    <asp:LinkButton ID="lbNC" runat="server" OnClick="lbNC_Click">NC PRP</asp:LinkButton>

                                                                </li>

                                                                <li>
                                                                    <asp:LinkButton ID="lbND" runat="server" OnClick="lbND_Click">ND PRP</asp:LinkButton>

                                                                </li>

                                                            </ul>
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
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
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
                                                                    <button runat="server" style="display: none" id="btnRun" onserverclick="btnBuscarProducto_Click" onclick="foco();" class="btn btn-info" title="Search">
                                                                        <i class="btn-icon-only icon-check-sign"></i>
                                                                    </button>
                                                                </span>
                                                            </div>
                                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCodigo" ErrorMessage="El campo es obligatorio" SetFocusOnError="True"></asp:RequiredFieldValidator>--%>
                                                        </div>
                                                    </div>
                                                </td>
                                                <td style="width: 8%">

                                                    <asp:TextBox ID="txtCantidad" runat="server" class="form-control" AutoPostBack="True" OnTextChanged="txtCantidad_TextChanged" Style="text-align: right"></asp:TextBox>
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

                                                    <asp:TextBox ID="txtPUnitario" runat="server" class="form-control" disabled Style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td style="width: 15%">

                                                    <asp:TextBox ID="txtTotalArri" runat="server" class="form-control" disabled Style="text-align: right"></asp:TextBox>
                                                </td>
                                                <td style="width: 10%">
                                                    <asp:LinkButton ID="lbtnAgregarArticuloASP" class="btn btn-info" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" OnClick="btnAgregarArt_Click" Visible="true" />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>

                                    <br />

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

                                    <br />
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
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                </Triggers>
                            </asp:UpdatePanel>
                            <div class="row">
                                <asp:UpdatePanel ID="UpdatePanel5" UpdateMode="Always" runat="server">
                                    <ContentTemplate>
                                        <div role="form" class="form-horizontal col-md-12">
                                            <div class="col-md-12">
                                                <asp:Button ID="btnAgregar" runat="server" Text="Facturar" class="btn btn-success" OnClick="btnAgregar_Click" ValidationGroup="FacturaGroup" />
                                                <a class="btn btn-success" data-toggle="modal" href="#modalFacturaE" runat="server" id="btnFacturaE" visible="false">Siguiente
                                                </a>
                                                <asp:Button ID="btnAgregarRemitir" runat="server" Text="Facturar y Remitir" class="btn btn-success" ValidationGroup="FacturaGroup" />
                                                <asp:Button ID="btnNueva" Visible="false" runat="server" Text="Nueva Factura" class="btn btn-success" OnClick="btnNueva_Click" />
                                                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" class="btn btn-default" PostBackUrl="~/Formularios/Facturas/ABMFacturas.aspx" />
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>

                        </div>
                    </div>

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
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<h3>*</h3>" InitialValue="-1" ControlToValidate="ListOperadores" ValidationGroup="TarjetaGroup"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-5">
                                            <asp:DropDownList ID="ListTarjetas" runat="server" class="form-control" OnSelectedIndexChanged="ListTarjetas_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="<h3>*</h3>" InitialValue="-1" ControlToValidate="ListTarjetas" ValidationGroup="TarjetaGroup"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-4">
                                            <div class="input-group">
                                                <span class="input-group-addon">$</span>
                                                <asp:TextBox ID="txtImporteT" runat="server" class="form-control" Text="0" AutoPostBack="true" OnTextChanged="txtImporteT_TextChanged" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtImporteT" ValidationGroup="TarjetaGroup"></asp:RequiredFieldValidator>
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
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtImporteEfectivo" ValidationGroup="EfectivoGroup"></asp:RequiredFieldValidator>
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
                        <asp:UpdatePanel ID="UpdatePanelTarjeta" runat="server" UpdateMode="Always">
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

        <div id="modalAltaRapida" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">

                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Alta Rapida</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <asp:UpdatePanel ID="UpdatePanel7" UpdateMode="Always" runat="server">
                                <ContentTemplate>
                                    <div class="form-group">
                                        <label class="col-md-4">Codigo</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtCodigoAR" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="*" ControlToValidate="txtCodigoAR" ValidationGroup="ClienteARGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Razon Social</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtRazonAR" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ErrorMessage="*" ControlToValidate="txtRazonAR" ValidationGroup="ClienteARGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Tipo</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListTipoAR" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ErrorMessage="*" ControlToValidate="DropListTipoAR" InitialValue="-1" ValidationGroup="ClienteARGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">IVA</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListIvaAR" runat="server" class="form-control">
                                                <asp:ListItem Text="Seleccione..." Value="-1"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ErrorMessage="*" ControlToValidate="DropListIvaAR" InitialValue="-1" ValidationGroup="ClienteARGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Grupo</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListGrupoAR" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ErrorMessage="*" ControlToValidate="DropListGrupoAR" InitialValue="-1" ValidationGroup="ClienteARGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">CUIT</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtCuitAR" runat="server" class="form-control" MaxLength="11" onkeypress="javascript:return validarSoloNro(event)"></asp:TextBox>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ErrorMessage="*" ControlToValidate="txtCuitAR" ValidationGroup="ClienteARGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Lista Precio</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ListListaPreciosAR" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ErrorMessage="*" ControlToValidate="ListListaPreciosAR" InitialValue="-1" ValidationGroup="ClienteARGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Forma Pago</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListFormaPagoAR" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ErrorMessage="*" ControlToValidate="DropListFormaPagoAR" InitialValue="-1" ValidationGroup="ClienteARGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Vendedor</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ListVendedoresAR" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ErrorMessage="*" ControlToValidate="ListVendedoresAR" InitialValue="-1" ValidationGroup="ClienteARGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                </Triggers>
                            </asp:UpdatePanel>

                        </div>

                        <div class="modal-footer">
                            <asp:LinkButton ID="btnAltaRapida" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnAltaRapida_Click" ValidationGroup="ClienteARGroup" />
                        </div>
                    </div>

                </div>
            </div>
        </div>

        <!-- /row -->

    </div>
    <!-- /container -->

    <%-- </div>--%>
    <!-- /main -->


    <link href="../../css/pages/reports.css" rel="stylesheet">
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

        function foco() {
            document.getElementById("<%= this.txtCantidad.ClientID %>").focus();
        }

        function focoDesc() {
            document.getElementById("<%= this.TxtDescuentoArri.ClientID %>").focus();
        }

    </script>
    <script src="../../js/daypilot-modal-2.0.js"></script>

    <script>
        function openModal() {
            $('#modalTarjeta').modal('show');
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
                if (key == 46 || key == 8 || key == 44) // Detectar . (punto) , backspace (retroceso) y , (coma)
                { return true; }
                else
                { return false; }
            }
            return true;
        }
    </script>

</asp:Content>
