<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Clientesaspx.aspx.cs" Inherits="Gestion_Web.Formularios.Clientes.Clientesaspx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="main">
        <%--<div class="container">--%>
        <div>
            <div class="col-md-12 col-xs-12">
                <div class="widget stacked">
                    <div class="stat">
                        <h5><i class="icon-map-marker"></i>Maestro > Clientes > Clientes</h5>
                    </div>
                    <div class="widget-header">
                        <i class="icon-wrench"></i>
                        <h3>Herramientas</h3>
                    </div>
                    <!-- /widget-header -->

                    <div class="widget-content">

                        <table style="width: 100%">
                            <tr>
                                <td style="width: 15%">
                                    <div class="btn-group">
                                        <button type="button" class="btn btn-success dropdown-toggle" data-toggle="dropdown" id="btnAccion" runat="server">Accion    <span class="caret"></span></button>
                                        <ul class="dropdown-menu">
                                            <li>
                                                <asp:LinkButton ID="lbtnImprimir" runat="server" OnClick="lbtnImprimir_Click">Imprimir</asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="lbtnExportar" runat="server" OnClick="lbtnExportar_Click">Exportar Clientes</asp:LinkButton>
                                            </li>
                                            <li>
                                                <a href="#modalReporteCliente" data-toggle="modal" style="width: 90%">Exportar Clientes x Vendedor</a>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="lbtnExportarProvincia" runat="server" OnClick="lbtnExportarProvincia_Click">Exportar Clientes x Provincia</asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="lbtnExportarZona" runat="server" OnClick="lbtnExportarZona_Click">Exportar Clientes x Zona</asp:LinkButton>
                                            </li>
                                            <li>
                                                <a href="#modalEnviarSMS_A_Clientes" data-toggle="modal" style="width: 90%">Enviar SMS a los clientes</a>
                                            </li>
                                        </ul>
                                    </div>
                                </td>
                                <td style="width: 30%">
                                    <div class="col-lg-8">
                                        <div class="input-group">
                                            <asp:TextBox ID="txtBusqueda" runat="server" class="form-control" placeholder="Buscar Cliente"></asp:TextBox>
                                            <span class="input-group-btn">
                                                <asp:LinkButton ID="lbBuscar" runat="server" Text="Buscar" class="btn btn-primary" OnClick="btnBuscar_Click">
                                                    <i class="shortcut-icon icon-search"></i></asp:LinkButton>
                                            </span>
                                        </div>
                                    </div>
                                </td>

                                <td style="width: 35%"></td>

                                <td style="width: 5%">
                                    <a class="btn btn-primary ui-tooltip" data-toggle="modal" title data-original-title="Filtrar Clientes" href="#modalBusqueda" style="width: 100%">
                                        <i class="shortcut-icon icon-filter"></i>
                                    </a>
                                </td>
                                <td style="width: 5%">

                                    <a href="ClientesABM.aspx?accion=1" class="btn btn-primary ui-tooltip" data-toggle="tooltip" title data-original-title="Agregar" style="width: 100%">
                                        <i class="shortcut-icon icon-plus"></i>
                                    </a>

                                </td>
                                <td style="width: 5%">

                                    <a href="#modalAltaRapida" class="btn btn-primary ui-tooltip" data-toggle="modal" data-toggle="tooltip" title data-original-title="Alta rapida" style="width: 100%">
                                        <i class="shortcut-icon icon-bolt"></i>
                                    </a>

                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
            <div class="col-md-12 col-xs-12">
                <div class="widget stacked widget-table action-table">

                    <div class="widget-header">
                        <i class="icon-group"></i>
                        <h3>Clientes</h3>
                    </div>
                    <div class="widget-content">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                            <ContentTemplate>
                                <div class="panel-body">
                                    <div class="table-responsive">
                                        <a class="btn btn-info" style="display: none" data-toggle="modal" id="abreMillas" href="#modalMillas"></a>
                                        <table class="table table-striped table-bordered table-hover" id="dataTables-example">
                                            <thead>
                                                <tr>
                                                    <th>Codigo</th>
                                                    <th>Razon Social</th>
                                                    <th>Alias</th>
                                                    <th>Mail</th>
                                                    <th>Telefono</th>
                                                    <th>CUIT</th>
                                                    <th class="td-actions"></th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:PlaceHolder ID="phClientes" runat="server"></asp:PlaceHolder>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
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
                                    <asp:Label runat="server" ID="lblMensaje" Text="Esta seguro que desea eliminar el Cliente?" Style="text-align: center"></asp:Label>
                                </h5>
                            </div>

                            <div class="col-md-3">
                                <asp:TextBox runat="server" ID="txtMovimiento" Text="0" Style="display: none"></asp:TextBox>
                            </div>
                        </div>
                        <%--                                <div class="form-group">
                                    
                                </div>--%>
                    </div>


                    <div class="modal-footer">
                        <asp:Button runat="server" ID="btnSi" Text="Eliminar" class="btn btn-danger" OnClick="btnSi_Click" />
                        <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>

                        <%--                        <asp:LinkButton ID="LinkButton1" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnBuscar_Click" ValidationGroup="BusquedaGroup" />--%>
                    </div>
                </div>

            </div>
        </div>
    </div>
    <%--Fin modalGrupo--%>
    <div id="modalReporteCliente" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Seleccionar Sucursal</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Always" runat="server">
                            <ContentTemplate>
                                <div class="form-group">
                                    <label class="col-md-4">Vendedor</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="DropListVendedores" runat="server" class="form-control"></asp:DropDownList>
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>

                    </div>

                    <div class="modal-footer">
                        <asp:LinkButton ID="btnReporteClientes" runat="server" Text="Exportar" class="btn btn-success" OnClick="btnReporteClientes_Click" />
                    </div>
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
                        <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Always" runat="server">
                            <ContentTemplate>
                                <div class="form-group">
                                    <label class="col-md-4">Codigo</label>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtCodigoAR" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-md-1">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="*" ControlToValidate="txtCodigoAR" ValidationGroup="ClienteARGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Razon Social</label>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtRazonAR" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-md-1">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="txtRazonAR" ValidationGroup="ClienteARGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Tipo</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="DropListTipoAR" runat="server" class="form-control" OnSelectedIndexChanged="DropListTipoAR_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    </div>
                                    <div class="col-md-1">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" ControlToValidate="DropListTipoAR" InitialValue="-1" ValidationGroup="ClienteARGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <asp:Panel runat="server" ID="PanelFamiliaAR" Visible="false">
                                    <div class="form-group">
                                        <label class="col-md-4">Depende de</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListFamiliaAR" runat="server" class="form-control">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <div class="form-group">
                                    <label class="col-md-4">IVA</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="DropListIvaAR" runat="server" class="form-control">
                                            <asp:ListItem Text="Seleccione..." Value="-1"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-1">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*" ControlToValidate="DropListIvaAR" InitialValue="-1" ValidationGroup="ClienteARGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Grupo</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="DropListGrupoAR" runat="server" class="form-control"></asp:DropDownList>
                                    </div>
                                    <div class="col-md-1">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*" ControlToValidate="DropListGrupoAR" InitialValue="-1" ValidationGroup="ClienteARGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">CUIT</label>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtCuitAR" runat="server" class="form-control" MaxLength="11" onkeypress="javascript:return validarSoloNro(event)"></asp:TextBox>
                                    </div>
                                    <div class="col-md-1">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="*" ControlToValidate="txtCuitAR" ValidationGroup="ClienteARGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Lista Precio</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="ListListaPreciosAR" runat="server" class="form-control"></asp:DropDownList>
                                    </div>
                                    <div class="col-md-1">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="*" ControlToValidate="ListListaPreciosAR" InitialValue="-1" ValidationGroup="ClienteARGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Forma Pago</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="DropListFormaPagoAR" runat="server" class="form-control"></asp:DropDownList>
                                    </div>
                                    <div class="col-md-1">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="*" ControlToValidate="DropListFormaPagoAR" InitialValue="-1" ValidationGroup="ClienteARGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Vendedor</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="ListVendedoresAR" runat="server" class="form-control"></asp:DropDownList>
                                    </div>
                                    <div class="col-md-1">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="*" ControlToValidate="ListVendedoresAR" InitialValue="-1" ValidationGroup="ClienteARGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <%--<div class="form-group">
                                    <label class="col-md-4">Aplica Descuento por Cantidad</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="ListDescuentoPorCantidad" runat="server" class="form-control">
                                            <asp:ListItem Text="Si" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>--%>
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

    <div id="modalMillas" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Adherir sistema millas</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Always" runat="server">
                            <ContentTemplate>
                                <div class="form-group">
                                    <label class="col-md-4">Nombre </label>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtNombreMillas" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-md-1">
                                        <asp:RequiredFieldValidator runat="server" Font-Bold="true" ForeColor="Red" ErrorMessage="*" ControlToValidate="txtNombreMillas" ValidationGroup="MillasGroup" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Apellido </label>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtApellidoMillas" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-md-1">
                                        <asp:RequiredFieldValidator runat="server" Font-Bold="true" ForeColor="Red" ErrorMessage="*" ControlToValidate="txtApellidoMillas" ValidationGroup="MillasGroup" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">DNI </label>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtDNIMillas" runat="server" class="form-control" MaxLength="8" onkeypress="javascript:return validarSoloNro(event)"></asp:TextBox>
                                    </div>
                                    <div class="col-md-1">
                                        <asp:RequiredFieldValidator runat="server" Font-Bold="true" ForeColor="Red" ErrorMessage="*" ControlToValidate="txtDNIMillas" ValidationGroup="MillasGroup" />
                                    </div>
                                    <asp:Label ID="lblIdRegistro" runat="server" Style="display: none;" Text="0" />
                                </div>
                                <div class="form-group" style="margin-bottom: 0px;">
                                    <label class="col-md-4">Celular</label>
                                    <div class="col-md-3">
                                        <div class="input-group">
                                            <span class="input-group-addon">0</span>
                                            <asp:TextBox ID="txtCodAreaMillas" runat="server" class="form-control" MaxLength="4" onkeypress="javascript:return validarSoloNro(event)"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtTelefonoMillas" runat="server" class="form-control" MaxLength="8" onkeypress="javascript:return validarSoloNro(event)"></asp:TextBox>
                                    </div>
                                    <div class="col-md-1">
                                        <asp:LinkButton ID="lbtnEnviarCodigoMillas" runat="server" class="btn btn-info ui-tooltip" data-toggle="tooltip" title data-original-title="Enviar codigo" Text="<i class='fa fa-phone' aria-hidden='true'></i>" OnClientClick="bloquear();" OnClick="lbtnEnviarCodigoMillas_Click" />
                                    </div>
                                    <div class="col-md-1">
                                        <asp:LinkButton ID="lbtnValidarLuegoMillas" runat="server" class="btn btn-success ui-tooltip" data-toggle="tooltip" title data-original-title="Validar luego" Text="<i class='icon fa fa-times' aria-hidden='true'></i>" OnClick="lbtnValidarLuegoMillas_Click" />
                                    </div>
                                    <div class="col-md-2" style="margin-bottom: 0px; padding-bottom: 0px;">
                                        <asp:RegularExpressionValidator ErrorMessage="*Invalido" Font-Bold="true" ForeColor="Red" SetFocusOnError="true" ControlToValidate="txtCodAreaMillas" runat="server" ValidationGroup="MillasGroup" ValidationExpression="^[1-9][0-9]{1,3}$" />
                                        <asp:RequiredFieldValidator runat="server" Font-Bold="true" ForeColor="Red" ErrorMessage="*" ControlToValidate="txtCodAreaMillas" ValidationGroup="MillasGroup" />
                                        <asp:RequiredFieldValidator runat="server" Font-Bold="true" ForeColor="Red" ErrorMessage="*" ControlToValidate="txtTelefonoMillas" ValidationGroup="MillasGroup" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Codigo: </label>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtCodigoMillas" runat="server" class="form-control" MaxLength="4" onkeypress="javascript:return validarSoloNro(event)"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:LinkButton ID="lbtnValidarCodigoMillas" runat="server" class="btn btn-info" Text="Validar" OnClick="lbtnValidarCodigoMillas_Click" />
                                    </div>
                                </div>
                                <asp:Panel ID="panelMillas" runat="server" Visible="false">
                                    <div class="form-group">
                                        <label class="col-md-4">Fecha Nac. </label>
                                        <div class="col-md-6">
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="shortcut-icon icon-calendar"></i></span>
                                                <asp:TextBox ID="txtFechaNacMillas" runat="server" class="form-control"></asp:TextBox>
                                            </div>

                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator runat="server" Font-Bold="true" ForeColor="Red" ErrorMessage="*" ControlToValidate="txtFechaNacMillas" ValidationGroup="MillasGroup" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Mail </label>
                                        <div class="col-md-6">
                                            <div class="input-group">
                                                <span class="input-group-addon">@</span>
                                                <asp:TextBox ID="txtMailMillas" runat="server" class="form-control" TextMode="Email"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="col-md-1">
                                            <%--<asp:RequiredFieldValidator runat="server" Font-Bold="true" ForeColor="Red" ErrorMessage="*" ControlToValidate="txtMailMillas" ValidationGroup="MillasGroup"/>--%>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Sucursal </label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ListSucursalesMillas" runat="server" class="form-control" />
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator runat="server" Font-Bold="true" ForeColor="Red" ErrorMessage="*" InitialValue="-1" ControlToValidate="ListSucursalesMillas" ValidationGroup="MillasGroup" />
                                        </div>
                                    </div>
                                    <asp:Label Text="0" ID="lblIdClienteMillas" runat="server" />
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                    <div class="modal-footer">
                        <asp:LinkButton ID="lbtnAgregarSistMillas" runat="server" Text="Guardar" class="btn btn-success" OnClick="lbtnAgregarSistMillas_Click" ValidationGroup="MillasGroup" />
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
                    <h4 class="modal-title">Filtrar Clientes</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <asp:UpdatePanel ID="UpdatePanel5" UpdateMode="Always" runat="server">
                            <ContentTemplate>

                                <div class="form-group">
                                    <label class="col-md-4">Tipo Cliente</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="DropListTipoCliente" runat="server" class="form-control" AutoPostBack="true"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Grupo</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="ListGruposClientes_ModalBusqueda" runat="server" class="form-control" AutoPostBack="false"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Vendedor</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="ListVendedores_ModalBusqueda" runat="server" class="form-control" AutoPostBack="false"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Provincia</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="ListProvincias_ModalBusqueda" runat="server" class="form-control" AutoPostBack="false"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Estado Cliente</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="ListEstadoCliente_ModalBusqueda" runat="server" class="form-control" AutoPostBack="false"></asp:DropDownList>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                    <div class="modal-footer">
                        <asp:LinkButton ID="btnFiltrar" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnFiltrar_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="modalEnviarSMS_A_Clientes" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Envio de SMS al cliente</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-12">
                                    <h1 style="text-align: center">
                                        <i class="icon-comment" style="color: blue"></i>
                                    </h1>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-8">
                                    <h5>
                                        <asp:Label runat="server" Font-Size="Medium" Text="Introduzca el titulo del mensaje a enviar" Style="text-align: center"></asp:Label>
                                    </h5>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-12">
                                    <h5>
                                        <asp:TextBox runat="server" Style="max-width: unset" ID="txtTituloMensaje" class="form-control"></asp:TextBox>
                                    </h5>
                                </div>
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-10">
                                    <h5>
                                        <asp:Label runat="server" Font-Size="Medium" Text="Introduzca el mensaje a enviar" Style="text-align: center"></asp:Label>
                                    </h5>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-lg-12">
                                    <h5>
                                        <asp:TextBox runat="server" Style="max-width: unset" ID="txtCuerpoMensaje" class="form-control"></asp:TextBox>
                                    </h5>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="modal-footer">
                        <div class="row">
                            <h5>
                                <asp:Label runat="server" ID="lb_cantidadClientesFiltrados" Font-Size="Medium" Text="" Style="text-align: center"></asp:Label>
                                <asp:Label runat="server" ID="lb_cantidadClientesTildados" Font-Size="Medium" Text="" Style="text-align: center"></asp:Label>
                            </h5>
                        </div>
                        <div class="row">
                            <asp:Button runat="server" Text="Enviar a Filtrados" class="btn btn-primary" OnClick="btnEnviarSMS_A_ClientesFiltrados_Click" />
                            <asp:Button runat="server" Text="Enviar a tildados" class="btn btn-info" OnClick="btnEnviarSMS_A_ClientesTildados_Click" />
                            <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script>
        function pageLoad() {
            $("#<%= txtFechaNacMillas.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%= lbtnEnviarCodigoMillas.ClientID %>").tooltip();
            $("#<%= lbtnValidarLuegoMillas.ClientID %>").tooltip();
        }
    </script>
    <script>
        $(function () {
            $("#<%= txtFechaNacMillas.ClientID %>").datepicker('option', { dateFormat: 'dd/mm/yy' });
        });
    </script>
    <script>
        function abrirdialog(valor) {
            document.getElementById('<%= txtMovimiento.ClientID %>').value = valor;
        }

        function ObtenerLosIdDeLosCheckBoxTildados() {
            var place = document.getElementById('<%=phClientes.ClientID%>')
            var botones = document.getElementsByName('CHK');

            for (var i in botones) {
                var boton = botones[i];
            }
        }
    </script>
    <script>
        function abrirModalMillas() {
            document.getElementById('abreMillas').click();
        }
        function bloquear() {
            if (Page_ClientValidate("MillasGroup")) {
                document.getElementById("<%= this.lbtnEnviarCodigoMillas.ClientID %>").setAttribute("disabled", "disabled");
                document.getElementById("<%= this.lbtnValidarCodigoMillas.ClientID %>").setAttribute("disabled", "disabled");
            }
        }
        function desbloquearEnvioCod() {
            document.getElementById("<%= this.lbtnEnviarCodigoMillas.ClientID %>").removeAttribute("disabled");
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
                else { return false; }
            }
            return true;
        }
    </script>

    <script>
        //valida los campos solo numeros
        function validarSoloNro(e) {
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
                return false;
            }
            else { return true; }
        }
    </script>



    <script>
        //valida los campos solo numeros
        function validarNroGuion(e) {
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
                if (key == 45) // Detectar  guion medio (-)
                { return true; }
                else { return false; }
            }
            return true;
        }
    </script>

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
    <script src="//cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>
    <script src="../../Scripts/plugins/dataTables/custom.tables.js"></script>
    <link href="//cdn.datatables.net/1.10.2/css/jquery.dataTables.css" rel="stylesheet" />


    <script>
        $(document).ready(function () {
            $('#dataTables-example').dataTable({
                "bLengthChange": false,
                "bFilter": false,
                "bInfo": false,
                "bAutoWidth": false
            });
        });
    </script>

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endReq);
        function endReq(sender, args) {
            $('#dataTables-example').dataTable({
                "bLengthChange": false,
                "bFilter": false,
                "bInfo": false,
                "bAutoWidth": false
            });
        }
    </script>

</asp:Content>
