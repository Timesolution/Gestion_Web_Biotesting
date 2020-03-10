<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClientesABM.aspx.cs" Inherits="Gestion_Web.Formularios.Clientes.ClientesABM"
    EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container">
        <div class="row">
            <div class="col-md-12">
                <div class="widget stacked ">
                    <div class="widget-header">
                        <i class="icon-pencil"></i>
                        <h3>
                            <asp:Label ID="labelNombre" runat="server" Text=""></asp:Label>
                            <asp:Label ID="labelNombreCliente" runat="server" Text=""></asp:Label>
                            <asp:HiddenField ID="hiddenIdCliente" runat="server"></asp:HiddenField>
                            <asp:HiddenField ID="hiddenOrigenCliente" runat="server"></asp:HiddenField>
                        </h3>
                    </div>
                    <div class="widget-content">
                        <div class="bs-example">
                            <ul id="myTab" class="nav nav-tabs">
                                <li class="active"><a href="#home" data-toggle="tab">Datos</a></li>
                                <li><a href="#profile" data-toggle="tab">Direccion</a></li>
                                <li><a href="#Contacto" data-toggle="tab">Contacto</a></li>
                                <li><a href="#Expreso" id="linkSucursales" runat="server" visible="false" data-toggle="tab">Cliente interno</a></li>
                                <li><a href="#Expreso2" id="linkExpreso" runat="server" visible="false" data-toggle="tab">Expreso</a></li>
                                <li><a href="#Entregas" id="linkEntregas" runat="server" visible="false" data-toggle="tab">Entregas</a></li>
                                <li><a href="#Exportacion" id="linkExportacion" runat="server" visible="false" data-toggle="tab">Exportacion</a></li>
                                <li><a href="#Millas" id="linkMillas" runat="server" data-toggle="tab" visible="false">Millas</a></li>
                                <li><a href="#Eventos" id="linkEventos" runat="server" data-toggle="tab" visible="false">Eventos</a></li>
                                <li><a href="#Empleado" id="linkEmpleado" runat="server" visible="false" data-toggle="tab">Cliente Empleado</a></li>
                                <li><a href="#Familia" id="linkFamilia" runat="server" data-toggle="tab" visible="false">Familia</a></li>
                                <li><a href="#Ganancias" id="linkGanancias" runat="server" data-toggle="tab" visible="false">Ganancias</a></li>
                                <li><a href="#OrdenesCompra" id="linkOrdenesCompra" runat="server" data-toggle="tab" visible="false">Orden de Compra</a></li>
                                <li><a href="#CodigoBTB" id="linkCodigoBTB" runat="server" data-toggle="tab" visible="false">Codigo BTB</a></li>
                                <li><a href="#IngresosBrutos" id="linkIngresosBrutos" runat="server" data-toggle="tab" visible="true">IIBB Otras Jurisdicciones</a></li>
                            </ul>
                            <div class="tab-content">
                                <div class="tab-pane fade active in" id="home">
                                    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Always" runat="server">
                                        <ContentTemplate>

                                            <div id="validation-form" role="form" class="form-horizontal col-md-10">
                                                <fieldset>
                                                    <div class="form-group">
                                                        <label for="name" class="col-md-4">Codigo</label>
                                                        <div class="col-md-4">
                                                            <asp:TextBox ID="txtCodCliente" runat="server" class="form-control" onkeypress="javascript:return validarSoloNro(event)"></asp:TextBox>

                                                        </div>
                                                        <div class="col-md-1">
                                                            <asp:LinkButton ID="lbGenerarCodigoCliente" class="btn btn-info" runat="server" Text="<span class='shortcut-icon icon-arrow-right'></span>" OnClick="lbGenerarCodigoCliente_Click" />
                                                        </div>
                                                        <div class="col-md-3">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtCodCliente" ValidationGroup="ClienteGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label for="name" class="col-md-4">Razon Social</label>

                                                        <div class="col-md-4">

                                                            <asp:TextBox ID="txtRazonSocial" runat="server" class="form-control"></asp:TextBox>

                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtRazonSocial" ValidationGroup="ClienteGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>


                                                    <div class="form-group">
                                                        <label for="name" class="col-md-4">Alias</label>

                                                        <div class="col-md-4">

                                                            <asp:TextBox ID="txtAlias" runat="server" class="form-control"></asp:TextBox>
                                                            <%--<input type="text" class="form-control" name="name" id="name">--%>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtAlias" ValidationGroup="ClienteGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>


                                                    <div class="form-group">
                                                        <label for="validateSelect" class="col-md-4">Grupo</label>
                                                        <div class="col-md-4">
                                                            <asp:DropDownList ID="DropListGrupo" runat="server" class="form-control"></asp:DropDownList>

                                                        </div>
                                                        <div class="col-md-4">
                                                            <a class="btn btn-info" data-toggle="modal" href="#modalGrupo">
                                                                <i class="shortcut-icon icon-plus"></i>
                                                            </a>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label for="validateSelect" class="col-md-4">Pais</label>
                                                        <div class="col-md-4">
                                                            <asp:DropDownList ID="DropListPais" runat="server" class="form-control"></asp:DropDownList>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label for="validateSelect" class="col-md-4">Tipo</label>
                                                        <div class="col-md-4">
                                                            <asp:DropDownList ID="DropListTipo" runat="server" class="form-control" OnSelectedIndexChanged="DropListTipo_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <a class="btn btn-info" data-toggle="modal" href="#modalTipoCliente" style="display: none;">
                                                                <i class="shortcut-icon icon-plus"></i>
                                                            </a>
                                                        </div>
                                                    </div>
                                                    <asp:Panel runat="server" ID="PanelFamilia" Visible="false">
                                                        <div class="form-group">
                                                            <label class="col-md-4">Depende de</label>
                                                            <div class="col-md-6">
                                                                <asp:DropDownList ID="DropListFamilia" runat="server" class="form-control">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                    </asp:Panel>
                                                    <div class="form-group">
                                                        <label id="lbCUITDNI" runat="server" for="name" class="col-md-4">Cuit</label>

                                                        <div class="col-md-4">

                                                            <asp:TextBox ID="txtCuit" runat="server" class="form-control" MaxLength="11" onkeypress="javascript:return validarSoloNro(event)"></asp:TextBox>
                                                            <asp:TextBox ID="txtID" runat="server" class="form-control" Visible="False"></asp:TextBox>
                                                            <%--<input type="text" class="form-control" name="name" id="name">--%>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtCuit" ValidationGroup="ClienteGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <%--<asp:RegularExpressionValidator ControlToValidate="txtCuit" ID="RegularExpressionValidator4" runat="server" ErrorMessage="Formato de CUIT Invalido" ValidationExpression="^\d{2}\d{8}\d{1}$" ValidationGroup="ClienteGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RegularExpressionValidator>--%>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label for="validateSelect" class="col-md-4">Iva</label>
                                                        <div class="col-md-4">
                                                            <asp:DropDownList ID="DropListIva" runat="server" class="form-control">
                                                                <asp:ListItem Text="Seleccione..." Value="-1"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator35" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="DropListIva" InitialValue="-1" ValidationGroup="ClienteGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label for="name" class="col-md-4">IIBB Pcia. Bs. As.</label>
                                                        <div class="col-md-4">
                                                            <div class="input-group">
                                                                <span class="input-group-addon">%</span>
                                                                <asp:TextBox ID="txtIngBrutos" runat="server" class="form-control" Style="text-align: right" Text="0"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-1">
                                                            <asp:LinkButton ID="btnActualizarIngrBrutos" class="btn btn-info" runat="server" Text="<span class='shortcut-icon icon-refresh'></span>" OnClick="btnActualizarIngrBrutos_Click" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label for="name" class="col-md-4">Saldo Maximo</label>
                                                        <div class="col-md-4">
                                                            <div class="input-group">
                                                                <span class="input-group-addon">$</span>
                                                                <asp:TextBox ID="txtSaldoMaximo" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)" Text="0" Style="text-align: right"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="El campo es obligatorio" SetFocusOnError="true" ControlToValidate="txtSaldoMaximo" ValidationGroup="ClienteGroup" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtSaldoMaximo" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="ClienteGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                                        </div>
                                                    </div>
                                                    <asp:Panel runat="server" ID="PanelCtaCtble" Visible="false">
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Plan de cuentas:</label>
                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtPlanCtaProv" runat="server" class="form-control" ReadOnly="true" />
                                                            </div>
                                                            <div class="col-md-2">
                                                                <a href="#modalCuentas" data-toggle="modal" class="btn btn-info"><span class='shortcut-icon icon-plus'></span></a>
                                                            </div>
                                                        </div>
                                                    </asp:Panel>

                                                    <div class="form-group">
                                                        <label for="name" class="col-md-4">Dias Vence FC</label>
                                                        <div class="col-md-4">
                                                            <asp:TextBox ID="txtVencFC" runat="server" class="form-control" onkeypress="javascript:return validarSoloNro(event)" MaxLength="2" Style="text-align: right" Text="0"></asp:TextBox>

                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtVencFC" ValidationGroup="ClienteGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>

                                                    </div>
                                                    <div class="form-group">
                                                        <label for="name" class="col-md-4">Descuento Adicional</label>
                                                        <div class="col-md-4">
                                                            <div class="input-group">
                                                                <span class="input-group-addon">%</span>
                                                                <asp:TextBox ID="txtDescFC" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)" Style="text-align: right" Text="0" AutoPostBack="true" OnTextChanged="txtDescFC_TextChanged"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="col-md-1">
                                                            <a class="btn btn-info" data-toggle="modal" href="#modalDescuentos">
                                                                <i class="shortcut-icon icon-plus"></i>
                                                            </a>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtDescFC" ValidationGroup="ClienteGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtDescFC" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="ClienteGroup" ForeColor="Red" Font-Bold="true" />
                                                        </div>
                                                    </div>
                                                    <asp:Panel runat="server" ID="panel1" Visible="true">
                                                        <div class="form-group">

                                                            <div class="col-md-4">
                                                                <asp:Label runat="server" ID="lblVendedor" Text="Vendedor" Font-Bold="true"></asp:Label>
                                                            </div>

                                                            <div class="col-md-4">
                                                                <asp:DropDownList ID="ListVendedores" runat="server" class="form-control">
                                                                </asp:DropDownList>
                                                            </div>

                                                            <div class="col-md-3">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="ListVendedores" InitialValue="-1" ValidationGroup="ClienteGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <div class="col-md-4">
                                                                <asp:Label runat="server" ID="lblFormaPago" Text="Forma de Pago" Font-Bold="true"></asp:Label>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:DropDownList ID="DropListFormaPago" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="DropListFormaPago_SelectedIndexChanged">
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="col-md-1">
                                                                <a class="btn btn-info" data-toggle="modal" href="#modalFormaPago">
                                                                    <i class="shortcut-icon icon-plus"></i>
                                                                </a>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator32" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="DropListFormaPago" InitialValue="-1" ValidationGroup="ClienteGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <div class="col-md-4">
                                                                <asp:Label runat="server" ID="LabelFormasVenta" Visible="false" Text="Forma venta" Font-Bold="true"></asp:Label>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:DropDownList ID="ListFormasVenta" Visible="false" runat="server" class="form-control">
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <div class="col-md-4">
                                                                <asp:Label ID="LitCliente_1" runat="server" Font-Bold="true"></asp:Label>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:DropDownList ID="ListListaPrecios" runat="server" class="form-control">
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="col-md-1">
                                                                <a class="btn btn-info" data-toggle="modal" href="#modalLista">
                                                                    <i class="shortcut-icon icon-plus"></i>
                                                                </a>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="ListListaPrecios" InitialValue="-1" ValidationGroup="ClienteGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label class="col-md-4">Mail envio FC: </label>
                                                            <div class="col-md-4">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">@</span>
                                                                    <asp:TextBox ID="txtMailEntrega" runat="server" class="form-control"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label class="col-md-4">Cel. envio SMS: </label>
                                                            <div class="col-md-2">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">0</span>
                                                                    <asp:TextBox ID="txtCodArea" runat="server" class="form-control" placeholder="Cod. Area" MaxLength="4"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-2">
                                                                <asp:TextBox ID="txtCelularSMS" runat="server" class="form-control" placeholder="Ej.: 1111 2222" MaxLength="8"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label class="col-md-4">Fecha de Nacimiento: </label>
                                                            <div class="col-md-3">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon"><i class="shortcut-icon icon-calendar"></i></span>
                                                                    <asp:TextBox ID="txtFechaNacimientoSMS" runat="server" class="form-control"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </asp:Panel>
                                                    <div class="form-group">
                                                        <label for="validateSelect" class="col-md-4">Estado</label>

                                                        <div class="col-md-4">
                                                            <asp:DropDownList ID="DropListEstado" runat="server" class="form-control">
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <a class="btn btn-info" data-toggle="modal" href="#modalEstado">
                                                                <i class="shortcut-icon icon-plus"></i>
                                                            </a>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label for="message" class="col-md-4">Observaciones</label>

                                                        <div class="col-md-4">

                                                            <asp:TextBox ID="txtObservaciones" runat="server" class="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>

                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label for="message" class="col-md-4">Alerta</label>

                                                        <div class="col-md-4">

                                                            <asp:TextBox ID="txtAlerta" runat="server" class="form-control" TextMode="MultiLine" Rows="4" BackColor="#ffff99"></asp:TextBox>

                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <asp:Label for="validateSelect" runat="server" ID="LabelDescuentoPorCantidad" Visible="false" class="col-md-4">Aplica Descuento por Cantidad</asp:Label>
                                                        <div class="col-md-4">
                                                            <asp:DropDownList ID="ListDescuentoPorCantidad" runat="server" Visible="false" class="form-control">
                                                                <asp:ListItem Text="Si" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <%--<div class="col-md-3">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator39" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="DropListIva" InitialValue="-1" ValidationGroup="ClienteGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>--%>
                                                    </div>


                                                    <%-- <div class="form-group">
                                                        <div class="col-md-4"></div>

                                                        <div class="col-md-4">
                                                    
                                                        </div>
                                                    </div>--%>
                                                </fieldset>
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    <div class="col-md-8">
                                        <asp:Button ID="btnAgregar" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnAgregar_Click" ValidationGroup="ClienteGroup" />
                                        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" class="btn btn-default" PostBackUrl="~/Formularios/Clientes/Clientesaspx.aspx" />
                                    </div>
                                </div>

                                <%--Direccion--%>
                                <div class="tab-pane fade" id="profile">
                                    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Always" runat="server">
                                        <ContentTemplate>
                                            <div class="form-horizontal col-md-12">

                                                <%--                                                <br />
                                                <fieldset>--%>
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <label for="validateSelect" class="col-md-4">Tipo</label>
                                                        <div class="col-md-4">
                                                            <asp:DropDownList ID="ListTipoDireccion" class="form-control" runat="server">
                                                                <asp:ListItem>Legal</asp:ListItem>
                                                                <asp:ListItem>Entrega</asp:ListItem>
                                                                <asp:ListItem>Otro</asp:ListItem>
                                                            </asp:DropDownList>

                                                            <%--<asp:TextBox ID="txtTipo" runat="server" class="form-control"></asp:TextBox>--%>
                                                            <%--<input type="text" class="form-control" name="name" id="name">--%>
                                                        </div>

                                                        <div class="col-md-4">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="ListTipoDireccion" ValidationGroup="DireccionGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label for="name" class="col-md-4">Dirección</label>
                                                        <div class="col-md-4">
                                                            <asp:TextBox ID="txtDirecDireccion" runat="server" class="form-control"></asp:TextBox>
                                                            <%--<input type="text" class="form-control" name="name" id="name">--%>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtDirecDireccion" ValidationGroup="DireccionGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                                        <ContentTemplate>


                                                            <div class="form-group">
                                                                <label for="validateSelect" class="col-md-4">Provincia</label>
                                                                <div class="col-md-4">
                                                                    <asp:DropDownList ID="ListProvincia" class="form-control" runat="server" OnSelectedIndexChanged="ListProvincia_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                                                    <%--<asp:TextBox ID="txtProvincia" runat="server" class="form-control"></asp:TextBox>--%>
                                                                    <%--<input type="text" class="form-control" name="name" id="name">--%>
                                                                </div>

                                                                <div class="col-md-4">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="ListProvincia" ValidationGroup="DireccionGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>

                                                            <div class="form-group">
                                                                <label for="validateSelect" class="col-md-4">Localidad</label>
                                                                <div class="col-md-4">
                                                                    <asp:DropDownList ID="ListLocalidad" class="form-control" runat="server" OnSelectedIndexChanged="ListLocalidad_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                                                    <%--<asp:TextBox ID="txtLocalidad" runat="server" class="form-control"></asp:TextBox>--%>
                                                                    <%--<input type="text" class="form-control" name="name" id="name">--%>
                                                                </div>

                                                                <div class="col-md-4">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="ListLocalidad" ValidationGroup="DireccionGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>

                                                            <div class="form-group">
                                                                <label for="name" class="col-md-4">Codigo Postal</label>

                                                                <div class="col-md-4">
                                                                    <%--<asp:DropDownList ID="ListCodPostal" class="form-control" runat="server"></asp:DropDownList>--%>
                                                                    <asp:TextBox ID="txtCodigoPostal" runat="server" class="form-control" onkeypress="javascript:return validarSoloNro(event)"></asp:TextBox>
                                                                    <%--<input type="text" class="form-control" name="name" id="name">--%>
                                                                </div>

                                                                <div class="col-md-1">
                                                                    <asp:LinkButton ID="lbtnAgregarDireccion" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnAgregarDireccion_Click" ValidationGroup="DireccionGroup" />
                                                                </div>
                                                                <div class="col-md-3">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtCodigoPostal" ValidationGroup="DireccionGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>
                                                        </ContentTemplate>

                                                    </asp:UpdatePanel>
                                                </div>
                                                <%-- </fieldset>--%>

                                                <div class="col-md-12">

                                                    <div class="widget stacked widget-table">

                                                        <div class="widget-header">
                                                            <span class="icon-external-link"></span>
                                                            <h3>Direcciones</h3>
                                                        </div>
                                                        <!-- .widget-header -->

                                                        <div class="widget-content">
                                                            <table class="table table-bordered table-striped">

                                                                <thead>
                                                                    <tr>
                                                                        <th style="width: 15%">Tipo</th>
                                                                        <th style="width: 20%">Direccion</th>
                                                                        <th style="width: 20%">Provincia</th>
                                                                        <th style="width: 20%">Localidad</th>
                                                                        <th style="width: 15%">Codigo Postal</th>
                                                                        <th style="width: 10%"></th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                    <asp:PlaceHolder ID="phDireccion" runat="server"></asp:PlaceHolder>
                                                                </tbody>
                                                            </table>

                                                        </div>
                                                        <!-- .widget-content -->

                                                    </div>

                                                </div>
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                        </Triggers>
                                    </asp:UpdatePanel>

                                </div>
                                <%--Fin direccion--%>

                                <%-- Contactos --%>
                                <div class="tab-pane fade" id="Contacto">
                                    <asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Always" runat="server">
                                        <ContentTemplate>
                                            <div role="form" class="form-horizontal col-md-12">
                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Nombre</label>

                                                    <div class="col-md-4">

                                                        <asp:TextBox ID="txtNombreContacto" runat="server" class="form-control"></asp:TextBox>

                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtNombreContacto" ValidationGroup="ContactoGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Cargo</label>

                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtCargoContacto" runat="server" class="form-control"></asp:TextBox>

                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtCargoContacto" ValidationGroup="ContactoGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>


                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Numero de telefono</label>

                                                    <div class="col-md-4">
                                                        <%--<asp:TextBox ID="txtNumeroContacto" runat="server" class="form-control" onkeypress="javascript:return validarNroGuion(event)"></asp:TextBox>--%>
                                                        <asp:TextBox ID="txtNumeroContacto" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Mail</label>

                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtMailContacto" runat="server" class="form-control"></asp:TextBox>

                                                    </div>
                                                    <div class="col-md-1">
                                                        <asp:LinkButton ID="lbtnAgregarContacto" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnAgregarContacto_Click" ValidationGroup="ContactoGroup" />
                                                    </div>
                                                    <div class="col-md-3">
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Debe tener un formato mail Ej: ejemplo@mail.com" ControlToValidate="txtMailContacto" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="ContactoGroup"></asp:RegularExpressionValidator>
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="col-md-12">

                                                <div class="widget stacked widget-table">

                                                    <div class="widget-header">
                                                        <span class="icon-external-link"></span>
                                                        <h3>Contactos</h3>
                                                    </div>
                                                    <!-- .widget-header -->

                                                    <div class="widget-content">
                                                        <table class="table table-bordered table-striped">

                                                            <thead>
                                                                <tr>
                                                                    <th style="width: 25%">Nombre</th>
                                                                    <th style="width: 20%">Cargo</th>
                                                                    <th style="width: 20%">Numero</th>
                                                                    <th style="width: 25%">Mail</th>
                                                                    <th style="width: 10%"></th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                <asp:PlaceHolder ID="phContactos" runat="server"></asp:PlaceHolder>
                                                            </tbody>
                                                        </table>

                                                    </div>
                                                    <!-- .widget-content -->

                                                </div>

                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <%-- Fin Contactos --%>

                                <%--Sucursales--%>
                                <div class="tab-pane fade" id="Expreso">
                                    <asp:UpdatePanel ID="UpdatePanelSucursales" UpdateMode="Always" runat="server">
                                        <ContentTemplate>
                                            <div role="form" class="form-horizontal col-md-12">
                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Sucursal</label>

                                                    <div class="col-md-4">
                                                        <asp:DropDownList ID="ListSucursales" class="form-control" runat="server"></asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:LinkButton ID="btnAgregarSucursal" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnAgregarSucursal_Click" />
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <label for="name" class="col-md-4"></label>

                                                    <div class="col-md-4">
                                                        <asp:ListBox ID="ListBoxSucursales" runat="server" class="form-control"></asp:ListBox>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:LinkButton ID="btnQuitarSucursal" runat="server" Text="<span class='shortcut-icon icon-trash'></span>" class="btn btn-danger" OnClick="btnQuitarSucursal_Click" />
                                                    </div>
                                                </div>

                                            </div>

                                        </ContentTemplate>
                                        <Triggers>
                                        </Triggers>
                                    </asp:UpdatePanel>

                                </div>
                                <%-- Sucursales --%>

                                <%--Expreso--%>
                                <div class="tab-pane fade" id="Expreso2">
                                    <asp:UpdatePanel ID="UpdatePanel7" UpdateMode="Always" runat="server">
                                        <ContentTemplate>
                                            <div role="form" class="form-horizontal col-md-12">
                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Expreso</label>

                                                    <div class="col-md-4">
                                                        <asp:DropDownList ID="ddlExpresos" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlExpresos_SelectedIndexChanged"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Nombre</label>

                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtNombreExpreso" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">CUIT</label>

                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtCuitExpreso" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Telefono</label>

                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtTelefonoExpreso" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Direccion</label>

                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtDireccionExpreso" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Observaciones</label>

                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtObservacionesExpreso" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <div class="col-md-4">
                                                        <asp:LinkButton ID="AgregarExpreso" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="AgregarExpreso_Click" />
                                                    </div>
                                                </div>

                                            </div>

                                        </ContentTemplate>
                                        <Triggers>
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <%-- Expreso --%>

                                <%--Entrega--%>
                                <div class="tab-pane fade" id="Entregas">
                                    <asp:UpdatePanel ID="UpdatePanel8" UpdateMode="Always" runat="server">
                                        <ContentTemplate>
                                            <div role="form" class="form-horizontal col-md-12">
                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Entregas</label>
                                                    <div class="col-md-4">
                                                        <asp:DropDownList ID="ListTipoEntrega" runat="server" class="form-control"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Horario Entrega</label>

                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtHorarioEntrega" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Zona Entrega</label>

                                                    <div class="col-md-4">
                                                        <asp:DropDownList ID="ListZonaEntrega" runat="server" class="form-control"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <div class="col-md-4">
                                                        <asp:LinkButton ID="btnAgregarEntrega" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnAgregarEntrega_Click" />
                                                    </div>
                                                </div>
                                            </div>

                                        </ContentTemplate>
                                        <Triggers>
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <%-- Expreso --%>

                                <%--Exportacion--%>
                                <div class="tab-pane fade" id="Exportacion">
                                    <asp:UpdatePanel ID="UpdatePanel9" UpdateMode="Always" runat="server">
                                        <ContentTemplate>
                                            <div role="form" class="form-horizontal col-md-12">
                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Cliente</label>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtClienteExportacion" runat="server" class="form-control" disabled></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">ID Impositvo</label>

                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtIdImpositivo" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <div class="col-md-4">
                                                        <asp:LinkButton ID="btnAgregarClienteImposivito" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnAgregarClienteImposivito_Click" />
                                                    </div>
                                                </div>
                                            </div>

                                        </ContentTemplate>
                                        <Triggers>
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <%-- Exportacion / --%>

                                <%--Alerta--%>
                                <div class="tab-pane fade" id="Alerta">
                                    <asp:UpdatePanel ID="UpdatePanel4" UpdateMode="Always" runat="server">
                                        <ContentTemplate>
                                            <div action="/" role="form" class="form-horizontal col-md-8">

                                                <br />
                                                <fieldset>
                                                    <div class="form-group">
                                                        <label for="name" class="col-md-4">Alerta</label>
                                                        <div class="col-md-4">
                                                            <asp:TextBox ID="txtDescAlerta" runat="server" class="form-control"></asp:TextBox>
                                                            <%--<input type="text" class="form-control" name="name" id="name">--%>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="El campo es obligatorio " ControlToValidate="txtDescAlerta" ValidationGroup="AlertaGroup"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <div class="col-md-4">
                                                            <asp:Button ID="btnAgregarAlerta" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnAgregarAlerta_Click" ValidationGroup="AlertaGroup" />
                                                        </div>
                                                    </div>


                                                </fieldset>

                                            </div>
                                            <div class="col-md-4">

                                                <div class="widget stacked widget-table">

                                                    <div class="widget-header">
                                                        <span class="icon-external-link"></span>
                                                        <h3>Alertas</h3>
                                                    </div>
                                                    <!-- .widget-header -->

                                                    <div class="widget-content">
                                                        <table class="table table-bordered table-striped">

                                                            <thead>
                                                                <tr>
                                                                    <th>Alerta</th>
                                                                    <th></th>
                                                                    <th></th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                <asp:PlaceHolder ID="phAlertas" runat="server"></asp:PlaceHolder>
                                                            </tbody>
                                                        </table>

                                                    </div>
                                                    <!-- .widget-content -->

                                                </div>

                                            </div>

                                        </ContentTemplate>
                                        <Triggers>
                                        </Triggers>
                                    </asp:UpdatePanel>

                                </div>
                                <%--Fin alerta--%>

                                <%--Millas--%>
                                <div class="tab-pane fade" id="Millas">
                                    <asp:UpdatePanel ID="UpdatePanel10" UpdateMode="Always" runat="server">
                                        <ContentTemplate>
                                            <div role="form" class="form-horizontal col-md-12">

                                                <div class="form-group">
                                                    <div class="col-md-4">
                                                        <label>¿Sistema de millas?: </label>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <asp:DropDownList ID="ListSiMillas" runat="server" class="form-control">
                                                            <asp:ListItem Text="NO" Value="0" />
                                                            <asp:ListItem Text="SI" Value="1" />
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:LinkButton ID="lbtnAgregarClienteMillas" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnAgregarClienteMillas_Click" />
                                                    </div>
                                                </div>

                                                <asp:Panel ID="PanelMillas" runat="server" Visible="false">
                                                    <div class="form-group">
                                                        <div class="col-md-4">
                                                            <label>Nro de socio: </label>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <asp:TextBox ID="txtNroSocioMillas" runat="server" class="form-control" disabled />
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <div class="col-md-4">
                                                            <label>Nro de tarjeta: </label>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <asp:TextBox ID="txtNroTarjetaMillas" runat="server" class="form-control" disabled />
                                                        </div>
                                                    </div>
                                                </asp:Panel>

                                            </div>

                                        </ContentTemplate>
                                        <Triggers>
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <%-- Millas / --%>

                                <%-- Eventos --%>
                                <div class="tab-pane fade" id="Eventos">
                                    <asp:UpdatePanel ID="UpdatePanel11" UpdateMode="Always" runat="server">
                                        <ContentTemplate>
                                            <div role="form" class="form-horizontal col-md-12">
                                                <div class="form-group">
                                                    <label for="name" class="col-md-1">Fecha</label>
                                                    <div class="col-md-2">
                                                        <div class="input-group">
                                                            <span class="input-group-addon"><i class="shortcut-icon icon-calendar"></i></span>
                                                            <asp:TextBox ID="txtFechaEvento" runat="server" class="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator36" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtFechaEvento" ValidationGroup="EventosGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <label for="name" class="col-md-1">Detalle</label>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtDetalleEvento" runat="server" class="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1">
                                                        <asp:LinkButton ID="lbtnAgregarEventoCliente" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnAgregarEventoCliente_Click" ValidationGroup="EventosGroup" />
                                                    </div>
                                                    <div class="col-md-1">
                                                        <asp:Label ID="lblIdEventoCliente" runat="server" Text="0" Visible="false" />
                                                    </div>
                                                </div>

                                            </div>
                                            <div class="col-md-12">

                                                <div class="widget stacked widget-table">

                                                    <div class="widget-header">
                                                        <span class="icon-external-link"></span>
                                                        <h3>Eventos</h3>
                                                    </div>
                                                    <!-- .widget-header -->

                                                    <div class="widget-content">
                                                        <table class="table table-bordered table-striped">
                                                            <thead>
                                                                <tr>
                                                                    <th>Fecha</th>
                                                                    <th>Detalle</th>
                                                                    <th style="width: 10%"></th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                <asp:PlaceHolder ID="phEventos" runat="server"></asp:PlaceHolder>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                    <!-- .widget-content -->
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </div>
                                <%-- Fin Eventos --%>

                                <%--Empleados--%>
                                <div class="tab-pane fade" id="Empleado">
                                    <asp:UpdatePanel ID="UpdatePanel13" UpdateMode="Always" runat="server">
                                        <ContentTemplate>
                                            <div role="form" class="form-horizontal col-md-12">
                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Empleado</label>

                                                    <div class="col-md-4">
                                                        <asp:DropDownList ID="listEmpleados" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="listEmpleados_SelectedIndexChanged"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Nombre</label>

                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtNombreEepleado" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Apellido</label>

                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtApellidoEmpleado" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">DNI</label>

                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtDNIEmpleado" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <div class="col-md-4">
                                                        <asp:LinkButton ID="btnAgregarEmpleado" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnAgregarEmpleado_Click" />
                                                    </div>
                                                </div>

                                            </div>

                                        </ContentTemplate>
                                        <Triggers>
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <%-- Expreso --%>

                                <%-- Familia --%>
                                <div class="tab-pane fade" id="Familia">
                                    <asp:UpdatePanel ID="UpdatePanel14" UpdateMode="Always" runat="server">
                                        <ContentTemplate>
                                            <div role="form" class="form-horizontal col-md-12">
                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Buscar Cliente</label>

                                                    <div class="col-md-4">
                                                        <asp:TextBox runat="server" class="form-control" ID="txtClienteReferir"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:LinkButton ID="lbtnBuscarClienteReferir" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-success" OnClick="lbtnBuscarClienteReferir_Click" />
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Cliente</label>

                                                    <div class="col-md-4">
                                                        <asp:DropDownList ID="ListClientesReferir" class="form-control" runat="server"></asp:DropDownList>

                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:LinkButton ID="lbtnReferirCliente" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnReferirCliente_Click" />
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <label for="name" class="col-md-4"></label>

                                                    <div class="col-md-4">
                                                        <asp:ListBox ID="ListBoxClientesReferidos" runat="server" class="form-control"></asp:ListBox>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:LinkButton ID="lbtnQuitarReferido" runat="server" Text="<span class='shortcut-icon icon-trash'></span>" class="btn btn-danger" OnClick="lbtnQuitarReferido_Click" />
                                                    </div>
                                                </div>

                                            </div>

                                        </ContentTemplate>
                                        <Triggers>
                                        </Triggers>
                                    </asp:UpdatePanel>

                                </div>
                                <%-- Familia --%>

                                <%-- Ganancias --%>
                                <div class="tab-pane fade" id="Ganancias">
                                    <asp:UpdatePanel ID="UpdatePanel15" UpdateMode="Always" runat="server">
                                        <ContentTemplate>
                                            <div role="form" class="form-horizontal col-md-12">
                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Minimo no imponible</label>
                                                    <div class="col-md-4">
                                                        <asp:TextBox runat="server" class="form-control" ID="txtMinimoNoImponible" MaxLength="15" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator37" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtMinimoNoImponible" ValidationGroup="GananciasGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Retención</label>
                                                    <div class="col-md-3">
                                                        <div class="input-group">
                                                            <span class="input-group-addon">%</span>
                                                            <asp:TextBox ID="txtRetencionGanancias" runat="server" class="form-control" Style="text-align: right" onkeypress="javascript:return validarNro(event)" MaxLength="10"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator38" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtRetencionGanancias" ValidationGroup="GananciasGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>


                                            </div>

                                        </ContentTemplate>
                                        <Triggers>
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    <div class="form-group">
                                        <div class="col-md-4">
                                            <asp:LinkButton ID="lbtnAgregarGanancias" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnAgregarGanancias_Click" ValidationGroup="GananciasGroup" />
                                        </div>
                                    </div>
                                </div>
                                <%-- Ganancias --%>

                                <%-- Ordenes de Compra --%>
                                <div class="tab-pane fade" id="OrdenesCompra">
                                    <asp:UpdatePanel ID="UpdatePanel16" UpdateMode="Always" runat="server">
                                        <ContentTemplate>
                                            <div role="form" class="form-horizontal col-md-12">
                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Mail Envio OC</label>
                                                    <div class="col-md-4">
                                                        <div class="input-group">
                                                            <span class="input-group-addon">@</span>
                                                            <asp:TextBox runat="server" class="form-control" ID="txtMailEnvioOC"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator39" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtMailEnvioOC" ValidationGroup="OrdenesCompraGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Requiere Autorización</label>
                                                    <div class="col-md-4">
                                                        <asp:DropDownList ID="ListRequiereAutorizacionOC" runat="server" class="form-control">
                                                            <asp:ListItem Text="Si" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Monto Autorización</label>
                                                    <div class="col-md-4">
                                                        <div class="input-group">
                                                            <span class="input-group-addon">$</span>
                                                            <asp:TextBox ID="txtMontoAutorizacionOC" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)" Text="0" Style="text-align: right"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator40" runat="server" ErrorMessage="El campo es obligatorio" SetFocusOnError="true" ControlToValidate="txtMontoAutorizacionOC" ValidationGroup="OrdenesCompraGroup" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtSaldoMaximo" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="OrdenesCompraGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Requiere Anticipo</label>
                                                    <div class="col-md-4">
                                                        <asp:DropDownList ID="ListRequiereAnticipoOC" runat="server" class="form-control">
                                                            <asp:ListItem Text="Si" Value="1"></asp:ListItem>
                                                            <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="col-md-4">Forma de Pago</label>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtFormaDePago_OC" runat="server" TextMode="MultiLine" Rows="4" class="form-control"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator41" runat="server" ErrorMessage="El campo es obligatorio" SetFocusOnError="true" ControlToValidate="txtFormaDePago_OC" ValidationGroup="OrdenesCompraGroup" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    <div class="form-group">
                                        <div class="col-md-4">
                                            <asp:LinkButton ID="lbtnAgregarProveedorOC" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnAgregarProveedorOC_Click" ValidationGroup="OrdenesCompraGroup" />
                                        </div>
                                    </div>
                                </div>

                                <div class="tab-pane fade" id="CodigoBTB">
                                    <%-- <asp:UpdatePanel ID="UpdatePanel17" UpdateMode="Always" runat="server">
                                        <ContentTemplate>--%>
                                    <div role="form" class="form-horizontal col-md-12">
                                        <div class="form-group">
                                            <label for="name" class="col-md-4">Codigo BTB</label>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtCodigoBTB1" runat="server" class="form-control" MaxLength="5"></asp:TextBox>
                                            </div>
                                        </div>
                                        <asp:PlaceHolder runat="server" ID="phCodigoBTB2" Visible="false">
                                            <div class="form-group">
                                                <label for="name" class="col-md-4">Codigo BTB</label>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="txtCodigoBTB2" runat="server" class="form-control" MaxLength="5"></asp:TextBox>
                                                </div>
                                            </div>
                                        </asp:PlaceHolder>
                                        <div class="form-group">
                                            <div class="col-md-4">
                                                <asp:LinkButton ID="lbtnCodigoBTB" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnCodigoBTB_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="tab-pane fade" id="IngresosBrutos">
                                    <div role="form" class="form-horizontal col-md-12">
                                        <div class="form-group">
                                            <div class="col-md-12">
                                                <div class="col-md-1">
                                                    <label for="name">Provincia</label>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="IngresosBrutos_DropList_Provincias" runat="server" class="form-control"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <div class="col-md-12">
                                                <div class="col-md-1">
                                                    <label for="name">Modo</label>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="IngresosBrutos_DropList_Modo" runat="server" class="form-control">
                                                        <asp:ListItem Value="0" Text="Siempre"></asp:ListItem>
                                                        <asp:ListItem Value="1" Text="Segun domicilio de entrega"></asp:ListItem>
                                                        <asp:ListItem Value="2" Text="Segun Padron"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <div class="col-md-12">
                                                <div class="col-md-1">
                                                    <asp:Label ID="lbPercepcionORetencion" Font-Bold="true" runat="server"></asp:Label>
                                                </div>
                                                <div class="input-group col-xs-2">
                                                    <span class="input-group-addon">%</span>
                                                    <asp:TextBox ID="IngresosBrutos_TxtPercepcionORetencion" Text="0" runat="server" Style="max-width: 100%; text-align:right" class="form-control" TextMode="Number"></asp:TextBox>
                                                </div>
                                                <div class="col-ms-2">
                                                    <asp:LinkButton ID="LinkButton1" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClientClick="javascript:return AgregarALaTablaLaPercepcion()" />
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-lg-5">
                                            <div class="widget stacked widget-table">
                                                <div class="widget-header">
                                                    <span class="icon-external-link"></span>
                                                    <h3 ID="lbPercepcionORetencionTituloTabla" Font-Bold="true" runat="server"></h3>
                                                </div>
                                                <div class="widget-content">
                                                    <table class="table table-bordered table-striped" id="tabla_IngresosBrutos">
                                                        <thead>
                                                            <tr>
                                                                <th style="width: 50%">Provincia</th>
                                                                <th style="width: 10%">
                                                                    <asp:Label ID="lbColumnaRetencionOPercepcion" runat="server"></asp:Label>
                                                                </th>
                                                                <th>Modo</th>
                                                                <th></th>
                                                            </tr>
                                                        </thead>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="modalTipoCliente" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Agregar Tipo</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label for="validateSelect" class="col-md-4">Tipo</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtTipoCliente2" runat="server" class="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-4">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="El campo es obligatorio " ControlToValidate="txtTipoCliente2" ValidationGroup="TipoClienteGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                        </div>

                    </div>

                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnAgregarTipoCliente" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnAgregarTipoCliente_Click" ValidationGroup="TipoClienteGroup" />
                </div>

            </div>
        </div>
    </div>
    <%--Fin modalGrupo--%>

    <div id="modalGrupo" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Agregar Grupo</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label for="validateSelect" class="col-md-4">Grupo</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtGrupo2" runat="server" class="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-4">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="El campo es obligatorio " ControlToValidate="txtGrupo2" ValidationGroup="GrupoClienteGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                        </div>
                    </div>

                </div>
                <div class="modal-footer">
                    <asp:Button ID="Button1" runat="server" Text="Guardar" class="btn btn-success" OnClick="Button1_Click" ValidationGroup="GrupoClienteGroup" />
                </div>

            </div>
        </div>
    </div>
    <%--Fin modalGrupo--%>

    <div id="modalCategoria" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Agregar Categoria</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label for="validateSelect" class="col-md-4">Categoria</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtCategoria2" runat="server" class="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-4">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="El campo es obligatorio " ControlToValidate="txtCategoria2" ValidationGroup="CategoriaClienteGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                        </div>
                    </div>

                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnAgregarCategoria" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnAgregarCategoria_Click" ValidationGroup="CategoriaClienteGroup" />
                </div>
            </div>
        </div>
    </div>
    <%--Fin modalGrupo--%>

    <div id="modalEstado" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Agregar estado</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label for="validateSelect" class="col-md-4">Estado</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtDescEstado" runat="server" class="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-4">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="El campo es obligatorio " ControlToValidate="txtDescEstado" ValidationGroup="EstadoClienteGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                        </div>
                    </div>

                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnAgregarEstado" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnAgregarEstado_Click" ValidationGroup="EstadoClienteGroup" />
                </div>

            </div>
        </div>
    </div>

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
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtNombreLista" ValidationGroup="ListaGroup"></asp:RequiredFieldValidator>
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
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator33" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtFormaPago" ValidationGroup="FPGroup"></asp:RequiredFieldValidator>
                        </div>
                    </div>

                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnAgregarFP" runat="server" Text="Guardar" class="btn btn-success" ValidationGroup="FPGroup" OnClick="btnAgregarFP_Click" />
                </div>

            </div>
        </div>
    </div>
    <%--Fin modalGrupo--%>
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
                            <label for="name" class="col-md-4">Legajo</label>

                            <div class="col-md-6">

                                <asp:TextBox ID="txtLegajo" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>

                                <%--<input type="text" class="form-control" name="name" id="name">--%>
                            </div>
                            <div class="col-md-2">
                                <asp:RequiredFieldValidator ControlToValidate="txtLegajo" ID="RequiredFieldValidator21" runat="server" ErrorMessage="<h3>*</h3>" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" ValidationGroup="VendedorGroup"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="name" class="col-md-4">Nombre</label>
                            <div class="col-md-6">
                                <asp:TextBox ID="txtNombre" runat="server" class="form-control"></asp:TextBox>
                                <%--<input type="text" class="form-control" name="name" id="name">--%>
                            </div>
                            <div class="col-md-2">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtNombre" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" ValidationGroup="VendedorGroup"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="name" class="col-md-4">Apellido</label>
                            <div class="col-md-6">
                                <asp:TextBox ID="txtApellido" runat="server" class="form-control"></asp:TextBox>
                                <%--<input type="text" class="form-control" name="name" id="name">--%>
                            </div>
                            <div class="col-md-2">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtApellido" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" ValidationGroup="VendedorGroup"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="name" class="col-md-4">DNI</label>
                            <div class="col-md-6">
                                <asp:TextBox ID="txtDni" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                <%--<input type="text" class="form-control" name="name" id="name">--%>
                            </div>
                            <div class="col-md-2">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtDni" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" ValidationGroup="VendedorGroup"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="name" class="col-md-4">Dirección</label>
                            <div class="col-md-6">
                                <asp:TextBox ID="txtDireccion" runat="server" class="form-control"></asp:TextBox>
                                <%--<input type="text" class="form-control" name="name" id="name">--%>
                            </div>
                            <div class="col-md-2">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator27" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtDireccion" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" ValidationGroup="VendedorGroup"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="name" class="col-md-4">Fecha Nacimiento</label>
                            <div class="col-md-6">
                                <asp:TextBox ID="txtFechaNacimiento" runat="server" class="form-control"></asp:TextBox>
                            </div>

                            <div class="col-md-2">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator28" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaNacimiento" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" ValidationGroup="VendedorGroup"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="name" class="col-md-4">Fecha Ingreso</label>
                            <div class="col-md-6">
                                <asp:TextBox ID="txtFechaIngreso" runat="server" name="fecha" class="form-control"></asp:TextBox>
                            </div>

                            <div class="col-md-2">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator29" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaIngreso" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" ValidationGroup="VendedorGroup"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="name" class="col-md-4">Cuit/Cuil</label>
                            <div class="col-md-6">
                                <asp:TextBox ID="txtCuitVendedor" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                <%--<input type="text" class="form-control" name="name" id="name">--%>
                            </div>
                            <div class="col-md-2">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator30" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtCuitVendedor" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" ValidationGroup="VendedorGroup"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-2">
                                <asp:RegularExpressionValidator ControlToValidate="txtCuitVendedor" ID="RegularExpressionValidator5" runat="server" ErrorMessage="<h3>*</h3>" ValidationExpression="^\d{2}\d{8}\d{1}$" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" ValidationGroup="VendedorGroup"></asp:RegularExpressionValidator>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="name" class="col-md-4">Observaciones</label>
                            <div class="col-md-6">
                                <asp:TextBox ID="TextBox2" runat="server" class="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                <%--<input type="text" class="form-control" name="name" id="name">--%>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="name" class="col-md-4">Comisión</label>

                            <div class="col-md-6">
                                <div class="input-group">
                                    <span class="input-group-addon">$</span>
                                    <asp:TextBox ID="txtComision" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)" Style="text-align: right"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator31" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtComision" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" ValidationGroup="VendedorGroup"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-2">
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="txtComision" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="<h3>*</h3>" ValidationGroup="VendedorGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                            </div>
                        </div>

                        <%--<div class="form-group">
                                                <label for="validateSelect" class="col-md-4">Perfil</label>
                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="DropListProveedor" runat="server" class="form-control"></asp:DropDownList>
                                                </div>
                                                <%--<div class="col-md-4">
                                                    <a class="btn btn-info" data-toggle="modal" href="../Proveedores/ProoveedoresABM.aspx">Agregar Proveedor</a>
                                                </div>--%>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnAgregarVendedor" runat="server" Text="Guardar" class="btn btn-success" ValidationGroup="VendedorGroup" OnClick="btnAgregarVendedor_Click" />
                </div>

            </div>
        </div>
    </div>

    <!-- Modal descuentos -->
    <div id="modalDescuentos" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Descuentos</h4>
                </div>

                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                        <ContentTemplate>
                            <div role="form" class="form-horizontal col-md-12">
                                <div class="form-group">
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtDescripcionDescuento" placeholder="Descripcion" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-md-1">
                                        <asp:RequiredFieldValidator ForeColor="Red" ValidationGroup="descuentos" ControlToValidate="txtDescripcionDescuento" ID="RequiredFieldValidator34" runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                    </div>

                                    <div class="col-md-4">
                                        <div class="input-group">
                                            <span class="input-group-addon">%</span>
                                            <asp:TextBox ID="txtPorcentajeDesc" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)" Style="text-align: right" Text="0"></asp:TextBox>

                                        </div>
                                        <asp:RangeValidator ControlToValidate="txtPorcentajeDesc" ValidationGroup="descuentos" ID="RangeValidator1" SetFocusOnError="true" ForeColor="Red" runat="server" ErrorMessage="Valores Permitidos 0 a 100" MaximumValue="100" Type="Double" MinimumValue="0"></asp:RangeValidator>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:LinkButton ID="btnAgregarDescuento" runat="server" ValidationGroup="descuentos" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnAgregarDescuento_Click" />
                                    </div>
                                </div>

                                <div class="form-group">


                                    <div class="col-md-10">
                                        <asp:ListBox ID="ListBoxDescuentos" runat="server" class="form-control"></asp:ListBox>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:LinkButton ID="btnEliminar" runat="server" Text="<span class='shortcut-icon icon-trash'></span>" class="btn btn-danger" OnClick="btnEliminar_Click" />
                                    </div>
                                </div>

                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="btnDescuento" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnDescuento_Click" />

                </div>

            </div>
        </div>
    </div>
    <%--Fin modalGrupo--%>

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
                                    <asp:Label runat="server" ID="lblMensaje" Text="Esta seguro que desea eliminar evento?" Style="text-align: center"></asp:Label>
                                </h5>
                            </div>

                            <div class="col-md-3">
                                <asp:TextBox runat="server" ID="txtMovimientoEventoCliente" Text="0" Style="display: none"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button runat="server" ID="btnSiEventoCliente" Text="Eliminar" class="btn btn-danger" OnClick="btnSiEventoCliente_Click" />
                        <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                    </div>
                </div>

            </div>
        </div>
    </div>
    <%--Fin modalGrupo--%>
    <div id="modalCuentas" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Cuentas contables</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel12" runat="server">
                        <ContentTemplate>
                            <div role="form" class="form-horizontal col-md-12">
                                <div class="form-group">
                                    <div class="col-md-12">
                                        <label class="col-md-3">Nivel 1:</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ListCtaContables1" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ListCtaContables1_SelectedIndexChanged" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-12">
                                        <label class="col-md-3">Nivel 2:</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ListCtaContables2" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ListCtaContables2_SelectedIndexChanged" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-12">
                                        <label class="col-md-3">Nivel 3:</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ListCtaContables3" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ListCtaContables3_SelectedIndexChanged" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-12">
                                        <label class="col-md-3">Nivel 4:</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ListCtaContables" runat="server" class="form-control" />
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="ListCtaContables" InitialValue="-1" ForeColor="Red" Font-Bold="true" runat="server" ValidationGroup="CtaContableGroup" />
                                        </div>
                                    </div>
                                </div>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="modal-footer">
                        <asp:LinkButton ID="lbtnAgregarMovCtaCbe" runat="server" class="btn btn-success" Text="Guardar" OnClick="lbtnAgregarMovCtaCbe_Click" ValidationGroup="CtaContableGroup" />
                        <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script src="../../Scripts/libs/jquery-1.9.1.min.js"></script>
    <script src="//code.jquery.com/ui/1.11.2/jquery-ui.js"></script>

    <script>
        var hiddenOrigenCliente;

        $(function () {
            $("#<%= txtFechaNacimiento.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%= txtFechaNacimientoSMS.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%= txtFechaEvento.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

        $(function () {
            $("#<%= txtFechaIngreso.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });
    </script>
    <script>
        function abrirdialog(valor) {
            document.getElementById('<%= txtMovimientoEventoCliente.ClientID %>').value = valor;
        }
    </script>
    <script>
        function pageLoad() {
            $("#<%= txtFechaEvento.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });

            ObtenerRegistrosYLLenarTablaIIBB();

            hiddenOrigenCliente = this.document.getElementById('<%= hiddenOrigenCliente.ClientID %>');
            var lbPercepcionORetencion = this.document.getElementById('<%= lbPercepcionORetencion.ClientID %>');
            var lbPercepcionORetencionTituloTabla = this.document.getElementById('<%= lbPercepcionORetencionTituloTabla.ClientID %>');
            var lbColumnaRetencionOPercepcion = this.document.getElementById('<%= lbColumnaRetencionOPercepcion.ClientID %>');

            if (hiddenOrigenCliente.value == 1) {//es cliente mostrar solo IIBB percepcion
                lbPercepcionORetencion.textContent = "Percepcion";
                lbColumnaRetencionOPercepcion.textContent = "Percepcion";
                lbPercepcionORetencionTituloTabla.textContent = "Percepciones";
            }
            else {
                lbPercepcionORetencion.textContent = "Retencion";
                lbColumnaRetencionOPercepcion.textContent = "Retencion";
                lbPercepcionORetencionTituloTabla.textContent = "Retenciones";
            }
        };
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

        function AgregarALaTablaLaPercepcion() {
            var controlHiddenIdCliente = document.getElementById('<%= hiddenIdCliente.ClientID %>');
            var controlProvincia = document.getElementById('<%= IngresosBrutos_DropList_Provincias.ClientID %>');
            var controlTxtPercepcionORetencion = document.getElementById('<%= IngresosBrutos_TxtPercepcionORetencion.ClientID %>');
            var controlModo = document.getElementById('<%= IngresosBrutos_DropList_Modo.ClientID %>');
            var modoText = controlModo.options[controlModo.selectedIndex].text;

            if (controlTxtPercepcionORetencion.value == "0") {
                return false;
            }
           
            $.ajax({
                type: "POST",
                url: "ClientesABM.aspx/AgregarIngresosBrutosYObtenerLosRegistros",
                data: '{ idClienteString: "' + controlHiddenIdCliente.value + '", IdProvincia: "' + controlProvincia.value +
                    '", origenCliente: "' + hiddenOrigenCliente.value + '", percepcionORetencion: "' + Math.abs(controlTxtPercepcionORetencion.value) + '", modo: "' + modoText + '"}',
                contentType: "application/json",
                dataType: 'json',
                error: (error) => {
                    console.log(JSON.stringify(error));
                    $.msgbox("No se pudo filtrar!", { type: "error" });
                },
                success: CargarTablaIIBB
            });
            controlTxtPercepcionORetencion.value = "0";
            return false;
        }

        function ObtenerRegistrosYLLenarTablaIIBB() {
            var controlHiddenIdCliente = document.getElementById('<%= hiddenIdCliente.ClientID %>');

            $.ajax({
                type: "POST",
                url: "ClientesABM.aspx/ObtenerRegistrosIIBBProvinciaByCliente",
                data: '{ IdCliente: "' + controlHiddenIdCliente.value + '"}',
                contentType: "application/json",
                dataType: 'json',
                error: (error) => {
                    console.log(JSON.stringify(error));
                    $.msgbox("No se pudo filtrar !", { type: "error" });
                }
                ,
                success: CargarTablaIIBB
            });
            return false;
        }

        function CargarTablaIIBB(response) {
            var data = response.d;
            var obj = JSON.parse(data);

            if (obj == "-2") {
                $.msgbox("Provincia ya existente", { type: "error" });
                return false;
            }

            $('#tabla_IngresosBrutos').find("tr:gt(0)").remove();

            for (var i = 0; i < obj.length; i++) {
                var percepcionORetencion = 0;
                percepcionORetencion = obj[i].Retencion;
                if (hiddenOrigenCliente.value == "1") {
                    percepcionORetencion = obj[i].Percepcion;
                }
                $('#tabla_IngresosBrutos').append(
                    "<tr>" +
                    "<td> " + obj[i].Provincia + "</td>" +
                    '<td style="text-align:right">' + percepcionORetencion + "</td>" +
                    '<td style="text-align:right">' + obj[i].Modo + "</td>" +
                    '<td style="text-align:right"> <a "id = ' + obj[i].Id + ' class= "btn btn-danger" autopostback="false" onclick="javascript: return EliminarRegistroDeTabla(' + obj[i].Id + ',' + obj[i].IdCliente + ')"><span class="shortcut-icon icon-trash"></span></a></td>' +
                    "</tr> ");
            };
        }

        function EliminarRegistroDeTabla(IdIIBBProvincia, IdCliente) {
            $.ajax({
                type: "POST",
                url: "ClientesABM.aspx/EliminarRegistroIIBBProvincia",
                data: '{ IdIIBBProvincia: "' + IdIIBBProvincia + '", IdCliente: "' + IdCliente + '"}',
                contentType: "application/json",
                dataType: 'json',
                error: (error) => {
                    console.log(JSON.stringify(error));
                    $.msgbox("No se pudo filtrar !", { type: "error" });
                }
                ,
                success: CargarTablaIIBB
            });
        }
    </script>


</asp:Content>
