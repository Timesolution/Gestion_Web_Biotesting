<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CajaCierreABM.aspx.cs" Inherits="Gestion_Web.Formularios.Valores.CajaCierreABM" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">

            <div class="col-md-12">

                <div class="widget stacked">

                    <div class="widget-header">
                        <i class="icon-pencil"></i>
                        <h3>Cierre Caja</h3>

                    </div>
                    <!-- /.widget-header -->
                    <asp:UpdatePanel ID="UpdatePanelSaldo" runat="server">
                        <ContentTemplate>
                            <div class="widget big-stats-container stacked ">
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
                        </ContentTemplate>

                    </asp:UpdatePanel>


                    <div class="widget-content">
                        <div class="bs-example">
                            <ul id="myTab" class="nav nav-tabs">
                                <li class="active"><a href="#home" data-toggle="tab">Cierre</a></li>
                                <li><a href="#mercaderia" data-toggle="tab">Mercaderia</a></li>
                                <li><a href="#turnos" data-toggle="tab">Cierre Turno</a></li>
                            </ul>
                            <div class="tab-content">
                                <div class="tab-pane fade active in" id="home">
                                    <div id="validation-form" role="form" class="form-horizontal col-md-7">
                                        <fieldset>
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                                <ContentTemplate>
                                                    <div class="form-group">
                                                        <label for="name" class="col-md-3">Sucursal</label>

                                                        <div class="col-md-6">
                                                            <asp:TextBox ID="txtSucursal" class="form-control" runat="server" disabled></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <asp:RequiredFieldValidator ControlToValidate="txtSucursal" ID="RequiredFieldValidator18" runat="server" ErrorMessage="Campo Obligatorio" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label for="name" class="col-md-3">Punto Venta</label>

                                                        <div class="col-md-6">
                                                            <asp:TextBox ID="txtPuntoVenta" class="form-control" runat="server" disabled></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <asp:RequiredFieldValidator ControlToValidate="txtPuntoVenta" ID="RequiredFieldValidator1" runat="server" ErrorMessage="Campo Obligatorio" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>

                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label for="name" class="col-md-3">Numero Cierre</label>

                                                        <div class="col-md-6">
                                                            <asp:TextBox ID="txtNumeroCierre" class="form-control" runat="server" disabled></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label for="name" class="col-md-3">Fecha Cierre</label>

                                                        <div class="col-md-6">
                                                            <div class="input-group">
                                                                <span class="input-group-addon"><i class="icon-calendar"></i></span>
                                                                <asp:TextBox ID="txtFecha" runat="server" class="form-control"></asp:TextBox>

                                                            </div>

                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RequiredFieldValidator ControlToValidate="txtFecha" ID="RequiredFieldValidator29" runat="server" ErrorMessage="Seleccione Fecha" InitialValue="-1" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>

                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label for="name" class="col-md-3">Fecha Apertura</label>

                                                        <div class="col-md-6">
                                                            <div class="input-group">
                                                                <span class="input-group-addon"><i class="icon-calendar"></i></span>
                                                                <asp:TextBox ID="txtFechaApertura" runat="server" class="form-control" AutoPostBack="true" OnTextChanged="txtFechaApertura_TextChanged"></asp:TextBox>
                                                            </div>

                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RequiredFieldValidator ControlToValidate="txtFecha" ID="RequiredFieldValidator2" runat="server" ErrorMessage="Seleccione Fecha" InitialValue="-1" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>

                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label for="name" class="col-md-3">Billetes 1000</label>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <span class="input-group-addon">N°</span>
                                                                <asp:TextBox ID="txt1000Cant" Text="0" runat="server" Style="text-align: right;" class="form-control" onkeypress="javascript:return validarNro(event)" AutoPostBack="True" OnTextChanged="txt1000Cant_TextChanged"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <span class="input-group-addon">$</span>
                                                                <asp:TextBox ID="txt1000Total" Text="0" runat="server" Style="text-align: right;" class="form-control" disabled></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txt100Cant" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txt200Cant" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                                        </div>

                                                    </div>
                                                    <div class="form-group">
                                                        <label for="name" class="col-md-3">Billetes 500</label>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <span class="input-group-addon">N°</span>
                                                                <asp:TextBox ID="txt500Cant" Text="0" runat="server" Style="text-align: right;" class="form-control" onkeypress="javascript:return validarNro(event)" AutoPostBack="True" OnTextChanged="txt500Cant_TextChanged"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <span class="input-group-addon">$</span>
                                                                <asp:TextBox ID="txt500Total" Text="0" runat="server" Style="text-align: right;" class="form-control" disabled></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txt100Cant" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txt200Cant" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                                        </div>

                                                    </div>
                                                    <div class="form-group">
                                                        <label for="name" class="col-md-3">Billetes 200</label>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <span class="input-group-addon">N°</span>
                                                                <asp:TextBox ID="txt200Cant" Text="0" runat="server" Style="text-align: right;" class="form-control" onkeypress="javascript:return validarNro(event)" AutoPostBack="True" OnTextChanged="txt200Cant_TextChanged"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <span class="input-group-addon">$</span>
                                                                <asp:TextBox ID="txt200Total" Text="0" runat="server" Style="text-align: right;" class="form-control" disabled></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txt100Cant" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txt200Cant" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                                        </div>

                                                    </div>
                                                    <div class="form-group">
                                                        <label for="name" class="col-md-3">Billetes 100</label>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <span class="input-group-addon">N°</span>
                                                                <asp:TextBox ID="txt100Cant" Text="0" runat="server" Style="text-align: right;" class="form-control" onkeypress="javascript:return validarNro(event)" AutoPostBack="True" OnTextChanged="txt100Cant_TextChanged"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <span class="input-group-addon">$</span>
                                                                <asp:TextBox ID="txt100Total" Text="0" runat="server" Style="text-align: right;" class="form-control" disabled></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator35" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txt100Cant" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator11" runat="server" ControlToValidate="txt100Cant" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                                        </div>

                                                    </div>
                                                    <div class="form-group">
                                                        <label for="name" class="col-md-3">Billetes 50</label>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <span class="input-group-addon">$</span>
                                                                <asp:TextBox ID="txt50Cant" Text="0" runat="server" Style="text-align: right;" class="form-control" onkeypress="javascript:return validarNro(event)" AutoPostBack="True" OnTextChanged="txt50Cant_TextChanged"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <span class="input-group-addon">$</span>
                                                                <asp:TextBox ID="txt50Total" Text="0" runat="server" Style="text-align: right;" class="form-control" disabled></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator36" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txt50Cant" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator12" runat="server" ControlToValidate="txt50Cant" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                                        </div>

                                                    </div>

                                                    <div class="form-group">
                                                        <label for="name" class="col-md-3">Billetes 20</label>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <span class="input-group-addon">$</span>
                                                                <asp:TextBox ID="txt20Cant" Text="0" runat="server" Style="text-align: right;" class="form-control" AutoPostBack="true" onkeypress="javascript:return validarNro(event)" OnTextChanged="txt20Cant_TextChanged"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <span class="input-group-addon">$</span>
                                                                <asp:TextBox ID="txt20Total" Text="0" runat="server" Style="text-align: right;" class="form-control" disabled></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator37" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txt20Cant" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator13" runat="server" ControlToValidate="txt20Cant" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label for="name" class="col-md-3">Billetes 10</label>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <span class="input-group-addon">$</span>
                                                                <asp:TextBox ID="txt10Cant" Text="0" runat="server" Style="text-align: right;" class="form-control" AutoPostBack="true" onkeypress="javascript:return validarNro(event)" OnTextChanged="txt10Cant_TextChanged"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <span class="input-group-addon">$</span>
                                                                <asp:TextBox ID="txt10Total" Text="0" runat="server" Style="text-align: right;" class="form-control" disabled></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txt10Cant" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-4">
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label for="name" class="col-md-3">Billetes 5</label>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <span class="input-group-addon">$</span>
                                                                <asp:TextBox ID="txt5Cant" Text="0" runat="server" Style="text-align: right;" class="form-control" AutoPostBack="true" onkeypress="javascript:return validarNro(event)" OnTextChanged="txt5Cant_TextChanged"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <span class="input-group-addon">$</span>
                                                                <asp:TextBox ID="txt5Total" Text="0" runat="server" Style="text-align: right;" class="form-control" disabled></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txt5Total" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-4">
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label for="name" class="col-md-3">Billetes 2</label>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <span class="input-group-addon">$</span>
                                                                <asp:TextBox ID="txt2Cant" Text="0" runat="server" Style="text-align: right;" class="form-control" AutoPostBack="true" onkeypress="javascript:return validarNro(event)" OnTextChanged="txt2Cant_TextChanged"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <span class="input-group-addon">$</span>
                                                                <asp:TextBox ID="txt2Total" Text="0" runat="server" Style="text-align: right;" class="form-control" disabled></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txt2Cant" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-4">
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label for="name" class="col-md-3">Monedas 2</label>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <span class="input-group-addon">$</span>
                                                                <asp:TextBox ID="txt2MCant" Text="0" runat="server" Style="text-align: right;" class="form-control" AutoPostBack="true" onkeypress="javascript:return validarNro(event)" OnTextChanged="txt2MCant_TextChanged"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <span class="input-group-addon">$</span>
                                                                <asp:TextBox ID="txt2MTotal" Text="0" runat="server" Style="text-align: right;" class="form-control" disabled></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txt2MCant" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-4">
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label for="name" class="col-md-3">Monedas 1</label>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <span class="input-group-addon">$</span>
                                                                <asp:TextBox ID="txt1Cant" Text="0" runat="server" Style="text-align: right;" class="form-control" AutoPostBack="true" onkeypress="javascript:return validarNro(event)" OnTextChanged="txt1Cant_TextChanged"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <span class="input-group-addon">$</span>
                                                                <asp:TextBox ID="txt1Total" Text="0" runat="server" Style="text-align: right;" class="form-control" disabled></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RequiredFieldValidator ControlToValidate="txt1Cant" ID="RequiredFieldValidator9" runat="server" ErrorMessage="El campo es obligatorio" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-4">
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label for="name" class="col-md-3">Monedas 50</label>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <span class="input-group-addon">$</span>
                                                                <asp:TextBox ID="txt05Cant" Text="0" runat="server" Style="text-align: right;" class="form-control" AutoPostBack="true" onkeypress="javascript:return validarNro(event)" OnTextChanged="txt05Cant_TextChanged"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <span class="input-group-addon">$</span>
                                                                <asp:TextBox ID="txt05Total" Text="0" runat="server" Style="text-align: right;" class="form-control" disabled></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RequiredFieldValidator ControlToValidate="txt05Cant" ID="RequiredFieldValidator10" runat="server" ErrorMessage="El campo es obligatorio" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-4">
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label for="name" class="col-md-3">Monedas 25</label>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <span class="input-group-addon">$</span>
                                                                <asp:TextBox ID="txt025Cant" Text="0" runat="server" Style="text-align: right;" class="form-control" AutoPostBack="true" onkeypress="javascript:return validarNro(event)" OnTextChanged="txt025Cant_TextChanged"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <span class="input-group-addon">$</span>
                                                                <asp:TextBox ID="txt025Total" Text="0" runat="server" Style="text-align: right;" class="form-control" disabled></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RequiredFieldValidator ControlToValidate="txt025Cant" ID="RequiredFieldValidator11" runat="server" ErrorMessage="El campo es obligatorio" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-4">
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label for="name" class="col-md-3">Monedas 10</label>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <span class="input-group-addon">$</span>
                                                                <asp:TextBox ID="txt010Cant" Text="0" runat="server" Style="text-align: right;" class="form-control" AutoPostBack="true" onkeypress="javascript:return validarNro(event)" OnTextChanged="txt010Cant_TextChanged"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <span class="input-group-addon">$</span>
                                                                <asp:TextBox ID="txt010Total" Text="0" runat="server" Style="text-align: right;" class="form-control" disabled></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RequiredFieldValidator ControlToValidate="txt010Cant" ID="RequiredFieldValidator12" runat="server" ErrorMessage="El campo es obligatorio" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-4">
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label for="name" class="col-md-3">Cambio</label>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <span class="input-group-addon">$</span>
                                                                <asp:TextBox ID="txt005Cant" Text="0" runat="server" Style="text-align: right;" class="form-control" AutoPostBack="true" onkeypress="javascript:return validarNro(event)" OnTextChanged="txt005Cant_TextChanged"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <span class="input-group-addon">$</span>
                                                                <asp:TextBox ID="txt005Total" Text="0" runat="server" Style="text-align: right;" class="form-control" disabled></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RequiredFieldValidator ControlToValidate="txt005Cant" ID="RequiredFieldValidator13" runat="server" ErrorMessage="El campo es obligatorio" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-4">
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label for="name" class="col-md-3">Retiro</label>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <span class="input-group-addon">$</span>
                                                                <asp:TextBox ID="txt001Cant" Text="0" runat="server" Style="text-align: right;" class="form-control" AutoPostBack="true" onkeypress="javascript:return validarNro(event)" OnTextChanged="txt001Cant_TextChanged"></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <span class="input-group-addon">$</span>
                                                                <asp:TextBox ID="txt001Total" Text="0" runat="server" Style="text-align: right;" class="form-control" disabled></asp:TextBox>

                                                            </div>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RequiredFieldValidator ControlToValidate="txt001Cant" ID="RequiredFieldValidator14" runat="server" ErrorMessage="El campo es obligatorio" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-4">
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label for="name" class="col-md-3">Total Cargado</label>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <span class="input-group-addon">$</span>
                                                                <asp:TextBox ID="txtTotal" Text="0" runat="server" Style="text-align: right;" class="form-control" disabled></asp:TextBox>
                                                                <%--<input type="text" class="form-control" name="name" id="name">--%>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Debe cargar billetes." InitialValue="0.00" ControlToValidate="txtTotal" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-4">
                                                        </div>
                                                    </div>
                                                    <asp:PlaceHolder ID="phDiferencia" Visible="true" runat="server">
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-3">Diferencia</label>
                                                            <div class="col-md-3">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">$</span>
                                                                    <asp:TextBox ID="txtDiferencia" Text="0" runat="server" Style="text-align: right;" class="form-control" disabled></asp:TextBox>

                                                                </div>
                                                            </div>

                                                            <div class="col-md-4">
                                                                <asp:Button ID="btnMovDiferencia" runat="server" Text="Generar Diferencia" class="btn btn-primary" OnClick="btnMovDiferencia_Click" />
                                                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtDiferencia" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="txtDiferencia" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-3">Total Caja</label>
                                                            <div class="col-md-3">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">$</span>
                                                                    <asp:TextBox ID="txtTotalCaja" Text="0" runat="server" Style="text-align: right;" class="form-control" disabled></asp:TextBox>

                                                                </div>
                                                            </div>
                                                            <div class="col-md-1">
                                                                <asp:LinkButton ID="btnRecargar" class="btn btn-info" runat="server" Text="<span class='shortcut-icon icon-refresh'></span>" OnClick="btnRecargar_Click" />
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtTotalCaja" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                            <div class="col-md-4">
                                                            </div>
                                                        </div>
                                                    </asp:PlaceHolder>
                                                    <div class="col-md-8">
                                                        <%--<asp:Button ID="btnAgregar" runat="server" Text="Guardar" class="btn btn-success" ValidationGroup="ArticuloGroup" OnClick="btnAgregar_Click" />--%>
                                                        <asp:Button ID="btnAgregar" runat="server" Text="Confirmar" class="btn btn-success" OnClick="btnAgregar_Click" ValidationGroup="ArticuloGroup" />
                                                        <asp:Button ID="btnAbrirModal" runat="server" Style="display: none;" Text="Confirmar" class="btn btn-success" data-toggle="modal" href="#modalConfirmacion" />
                                                        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" class="btn btn-default" PostBackUrl="../Valores/CajaF.aspx" />
                                                    </div>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </fieldset>
                                    </div>
                                </div>
                                <div class="tab-pane fade" id="mercaderia">
                                    <div id="validation-form2" role="form" class="form-horizontal col-md-12">
                                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                                            <ContentTemplate>
                                                <table class="table table-striped table-bordered table-hover" id="dataTables-example">
                                                    <thead>
                                                        <tr>
                                                            <th>Fecha</th>
                                                            <th>Tipo</th>
                                                            <th>Numero</th>
                                                            <th>Suc. Origen</th>
                                                            <th>Suc. Destino</th>
                                                            <th style="text-align: right;">Total</th>
                                                            <th></th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <asp:PlaceHolder ID="phFacturas" runat="server"></asp:PlaceHolder>
                                                    </tbody>
                                                </table>
                                                <div class="form-group">
                                                    <asp:LinkButton ID="lbtnAceptarMercaderia" runat="server" class="btn btn-success" Text="Aceptar" OnClick="lbtnAceptarMercaderia_Click" OnClientClick="grisarClick();" />
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                                <div class="tab-pane fade" id="turnos">
                                    <div id="validation-form3" role="form" class="form-horizontal col-md-12">
                                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always">
                                            <ContentTemplate>
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 95%"></td>
                                                        <td style="width: 2%">

                                                            <div class="btn-group pull-right" style="width: 100%">
                                                                <button type="button" class="btn btn-primary dropdown-toggle ui-tooltip" title data-original-title="Listados" data-toggle="dropdown">
                                                                    <i class="shortcut-icon icon-print"></i>&nbsp
                                                                        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                                                                    <span class="caret"></span>
                                                                </button>
                                                                <ul class="dropdown-menu">
                                                                    <li>
                                                                        <asp:LinkButton ID="btnExportarExcel" runat="server" OnClick="btnExportarExcel_Click">
                                                                            <i class="fa fa-file-excel-o" aria-hidden="true"></i>&nbsp Exportar
                                                                        </asp:LinkButton>
                                                                    </li>
                                                                    <li>
                                                                        <asp:LinkButton ID="btnImprimirPdf" runat="server" OnClick="btnImprimirPdf_Click">
                                                                            <i class="fa fa-file-pdf-o" aria-hidden="true"></i>&nbsp Imprimir
                                                                        </asp:LinkButton>
                                                                    </li>
                                                                </ul>
                                                            </div>

                                                        </td>
                                                    </tr>
                                                </table>
                                                &nbsp
                                                <table class="table table-striped table-bordered table-hover" id="dataTables-example2">
                                                    <thead>
                                                        <tr>
                                                            <th>Fecha</th>
                                                            <th>Movimiento</th>
                                                            <th style="text-align: right;">Total</th>
                                                            <th>Tipo</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <asp:PlaceHolder ID="phCajaTurno" runat="server"></asp:PlaceHolder>
                                                    </tbody>
                                                </table>
                                                <div class="col-md-4">
                                                    <asp:Button ID="btnCierreTurno" runat="server" Text="Confirmar" class="btn btn-success" data-toggle="modal" href="#modalCierreTurno" />
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </div>

                    </div>


                </div>
            </div>
        </div>

        <div id="modalConfirmacion" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                                <h4 class="modal-title">Confirmacion de Cierre Caja</h4>
                            </div>
                            <div class="modal-body">
                                <div role="form" class="form-horizontal col-md-12">
                                    <div class="form-group">
                                        <label class="col-md-4">Fecha Cierre:</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtFechaCierreConfirmacion" runat="server" class="form-control" disabled Style="text-align: right;"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Fecha Apertura:</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtFechaAperturaConfirmacion" runat="server" class="form-control" disabled Style="text-align: right;"></asp:TextBox>
                                        </div>
                                    </div>
                                    <asp:PlaceHolder ID="phDiferenciaModal" Visible="false" runat="server">
                                        <div class="form-group">
                                            <label for="name" class="col-md-4">Diferencia</label>
                                            <div class="col-md-4">
                                                <div class="input-group">
                                                    <span class="input-group-addon">$</span>
                                                    <asp:TextBox ID="txtDiferenciaModal" Text="0" runat="server" Style="text-align: right;" class="form-control" disabled></asp:TextBox>

                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <asp:Button ID="Button1" runat="server" Text="Generar Diferencia" class="btn btn-primary" OnClick="btnMovDiferencia_Click" />
                                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtDiferencia" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtDiferencia" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                            </div>
                                        </div>

                                    </asp:PlaceHolder>


                                    <div class="form-group">
                                        <label class="col-md-4">Total cierre $:</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtCajaConfirmacion" runat="server" class="form-control" disabled Style="text-align: right;"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="alert alert-danger alert-dismissable">
                                            <asp:Label ID="lblAlerta" runat="server" Font-Bold="True"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:Button runat="server" ID="btnConfirmarCierre" Text="Aceptar" class="btn btn-success" OnClick="btnConfirmarCierre_Click" ValidationGroup="ArticuloGroup" />
                                <asp:Label runat="server" ID="lblCierre"></asp:Label>
                                <asp:UpdateProgress ID="UpdateProgress2" AssociatedUpdatePanelID="UpdatePanel2" runat="server">
                                    <ProgressTemplate>
                                        <div class="col-md-6">
                                            <h3>Procesando <i class="fa fa-spinner fa-spin"></i>
                                            </h3>
                                        </div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <%--<asp:UpdateProgress ID="UpdateProgress2" AssociatedUpdatePanelID="UpdatePanel2" runat="server">
                        <ProgressTemplate>
                            <div class="col-md-4">
                                Procesando...
                                <i class="fa fa-spinner fa-spin"></i>
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>--%>
                </div>
            </div>
        </div>

        <div id="modalCierreTurno" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <div class="modal-header">
                                <button id="btnCerrarModalTurnos" type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                                <%--<button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>--%>
                                <h4 class="modal-title">Confirmacion de Cierre Turno</h4>
                            </div>
                            <div class="modal-body">
                                <div role="form" class="form-horizontal col-md-12">
                                    <div class="form-group">
                                        <label class="col-md-3">Turno:</label>
                                        <div class="col-md-4">
                                            <asp:DropDownList ID="ListTurnos" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                        <div>
                                            <asp:RequiredFieldValidator ErrorMessage="*" Font-Bold="true" ForeColor="Red" ControlToValidate="ListTurnos" InitialValue="-1" runat="server" ValidationGroup="TurnosGroup" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-3">Total Cierre:</label>
                                        <div class="col-md-4">
                                            <div class="input-group">
                                                <span class="input-group-addon">$</span>
                                                <asp:TextBox ID="txtTotalCierreTurno" runat="server" class="form-control" Text="0" Style="text-align: right;" disabled />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-3">Total Efectivo:</label>
                                        <div class="col-md-4">
                                            <div class="input-group">
                                                <span class="input-group-addon">$</span>
                                                <asp:TextBox ID="txtTotalEfectivo" runat="server" class="form-control" Text="0" Style="text-align: right;" disabled />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-3">Total Tarjeta:</label>
                                        <div class="col-md-4">
                                            <div class="input-group">
                                                <span class="input-group-addon">$</span>
                                                <asp:TextBox ID="txtTotalTarjeta" runat="server" class="form-control" Text="0" Style="text-align: right;" disabled />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-3">Total Traspasos:</label>
                                        <div class="col-md-4">
                                            <div class="input-group">
                                                <span class="input-group-addon">$</span>
                                                <asp:TextBox ID="txtTotalTraspasos" runat="server" class="form-control" Text="0" Style="text-align: right;" disabled />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:LinkButton runat="server" ID="btnConfirmarCierreTurno" Text="Aceptar" class="btn btn-success" OnClick="btnConfirmarCierreTurno_Click" ValidationGroup="TurnosGroup" OnClientClick="grisarClick2();" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <%--roww--%>

    <link href="../../css/pages/reports.css" rel="stylesheet">
    <!-- Core Scripts - Include with every page -->
    <script src="../../Scripts/jquery-1.10.2.js"></script>
    <script src="../../Scripts/bootstrap.min.js"></script>
    <%--<script src="../Scripts/plugins/metisMenu/jquery.metisMenu.js"></script>--%>

    <script src="../../Scripts/libs/jquery-1.9.1.min.js"></script>
    <script src="../../Scripts/libs/jquery-ui-1.10.0.custom.min.js"></script>
    <script src="../../Scripts/libs/bootstrap.min.js"></script>

    <script src="../../Scripts/plugins/hoverIntent/jquery.hoverIntent.minified.js"></script>
    <script src="../../Scripts/plugins/lightbox/jquery.lightbox.min.js"></script>

    <script src="../../Scripts/Application.js"></script>
    <script src="../../Scripts/demo/gallery.js"></script>

    <script src="../../Scripts/plugins/msgGrowl/js/msgGrowl.js"></script>
    <script src="../../Scripts/plugins/lightbox/jquery.lightbox.min.js"></script>
    <script src="../../Scripts/plugins/msgbox/jquery.msgbox.min.js"></script>
    <script src="../../Scripts/demo/notifications.js"></script>


    <script>


        $(function () {
            $("#<%= txtFecha.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

        $(function () {
            $("#<%= txtFechaApertura.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

    </script>

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endReq);
        function endReq(sender, args) {
            $(function () {
                $("#<%= txtFecha.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            });

            $(function () {
                $("#<%= txtFechaApertura.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            });
        }
    </script>

    <script>

        function abrirdialog() {
            document.getElementById('<%= btnAbrirModal.ClientID %>').click();
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
                if (key == 8)// Detectar . (punto) y backspace (retroceso) y , (coma)
                { return true; }
                else
                { return false; }
            }
            return true;
        }
    </script>
    <script>
        function grisarClick() {
            var btn = document.getElementById("<%= this.lbtnAceptarMercaderia.ClientID %>");
            btn.setAttribute("disabled", "disabled");
            //btn.firstChild.className = "fa fa-spinner fa-spin";            
        }
        function grisarClick2() {
            if (Page_ClientValidate("TurnosGroup")) {
                var btn = document.getElementById("<%= this.btnConfirmarCierreTurno.ClientID %>");
                btn.setAttribute("disabled", "disabled");
                //btn.firstChild.className = "fa fa-spinner fa-spin";            
            }
        }
        function cerrarModalCierreTurno() {
            document.getElementById('btnCerrarModalTurnos').click();
        }
    </script>

    <script type="text/javascript">
        function updateProgress(percentage) {
            document.getElementById('ProgressBar').style.width = percentage + "%";
        }
    </script>

</asp:Content>
