<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PagoRemuneracionABM.aspx.cs" Inherits="Gestion_Web.Formularios.Personal.PagoRemuneracionABM" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">

        <div class="row">
            <div class="col-md-12 col-xs-12 ">
                <div class="widget widget-table">
                    <div class="widget-header">
                        <i class="icon-list"></i>
                        <h3>Documentos impagos</h3>
                    </div>
                    <div class="widget-content">
                        <asp:UpdatePanel ID="UpdatePanel8" UpdateMode="Always" runat="server">
                            <ContentTemplate>
                                <table class="table table-bordered table-striped" id="tbDocumentos">
                                    <thead>
                                        <tr>
                                            <th style="width: 70%">Numero</th>
                                            <th style="width: 15%">Saldo</th>
                                            <th style="width: 15%">Imputar</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:PlaceHolder ID="phDocumentos" runat="server"></asp:PlaceHolder>
                                        <tr>
                                            <th>Total</th>
                                            <th>
                                                <div class="input-group">
                                                    <span class="input-group-addon">$</span>
                                                    <asp:TextBox ID="txtSaldoDoc" runat="server" class="form-control" Text="" Style="text-align: right" disabled=""></asp:TextBox>
                                                </div>
                                            </th>
                                            <th>
                                                <div class="input-group">
                                                    <span class="input-group-addon">$</span>
                                                    <input id="TotalDoc" runat="server" class="form-control" disabled="" text="0" style="text-align: right" />
                                                </div>
                                            </th>
                                        </tr>
                                    </tbody>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-12 col-xs-12 ">
                <%--<div class="widget stacked">--%>
                <div class="widget stacked">

                    <div class="widget-header">
                        <i class="icon-pencil"></i>
                        <h3>Pagos</h3>
                    </div>

                    <div class="widget-content">
                        <section id="accordions">
                            <div class="panel-group accordion" id="acordions">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 95%">
                                                    <h4 class="panel-title">
                                                        <a class="accordion-toggle" data-toggle="collapse" data-parent=".accordion" href="#collapseOne">Efectivo
                                                        </a>
                                                    </h4>
                                                </td>
                                                <td style="width: 5%">

                                                    <asp:LinkButton ID="lbtnEfectivo" runat="server" Text="<span class='shortcut-icon icon-plus'></span>" class="accordion-toggle" data-toggle="collapse" data-parent=".accordion" href="#collapseOne" Style="color: black" />

                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="collapseOne" class="panel-collapse collapse">
                                        <div class="panel-body">
                                            <asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Always" runat="server">
                                                <ContentTemplate>
                                                    <div role="form" class="form-horizontal col-md-10">
                                                        <fieldset>
                                                            <div class="form-group">
                                                                <label for="validateSelect" class="col-md-4">Tipo</label>
                                                                <div class="col-md-4">
                                                                    <asp:DropDownList ID="DropListTipo" runat="server" class="form-control" AutoPostBack="True" OnSelectedIndexChanged="DropListTipo_SelectedIndexChanged" ></asp:DropDownList>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="Seleccione un tipo de Moneda" ControlToValidate="DropListTipo" InitialValue="Seleccione..." ValidationGroup="MonedaGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <label for="name" class="col-md-4">Monto</label>
                                                                <div class="col-md-4">

                                                                    <div class="input-group">
                                                                        <span class="input-group-addon">$</span>
                                                                        <asp:TextBox ID="txtMonto" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)" Style="text-align: right"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtMonto" ValidationGroup="MonedaGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <label for="name" class="col-md-4">Cotizacion</label>
                                                                <div class="col-md-4">

                                                                    <div class="input-group">
                                                                        <span class="input-group-addon">$</span>
                                                                        <asp:TextBox ID="txtCotizacion" runat="server" class="form-control" disabled Style="text-align: right"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtCotizacion" ValidationGroup="MonedaGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <div class="col-md-4">
                                                                    <asp:Button ID="btnAgregarPagoM" runat="server" Text="Agregar" class="btn btn-success" ValidationGroup="MonedaGroup" OnClick="btnAgregarPagoM_Click" />
                                                                </div>
                                                            </div>
                                                        </fieldset>
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
                                                        <a class="accordion-toggle" data-toggle="collapse" data-parent=".accordion" href="#collapseTwo">Cheques Propios
                                                        </a>
                                                    </h4>
                                                </td>
                                                <td style="width: 5%">
                                                    <asp:LinkButton ID="lbtnCheque" runat="server" Text="<span class='shortcut-icon icon-plus'></span>" class="accordion-toggle" data-toggle="collapse" data-parent=".accordion" href="#collapseTwo" Style="color: black" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="collapseTwo" class="panel-collapse collapse">
                                        <div class="panel-body">
                                            <asp:UpdatePanel ID="UpdatePanel4" UpdateMode="Always" runat="server">
                                                <ContentTemplate>
                                                    <div class="widget-content">
                                                        <div role="form" class="form-horizontal col-md-10">
                                                            <br />
                                                            <fieldset>
                                                                <div class="form-group">
                                                                    <label for="validateSelect" class="col-md-4">Cuenta</label>
                                                                    <div class="col-md-8">
                                                                        <asp:DropDownList ID="ListCuenta" runat="server" class="form-control" AutoPostBack="True" OnSelectedIndexChanged="ListCuenta_SelectedIndexChanged">
                                                                        </asp:DropDownList>
                                                                    </div>
                                                                    <div class="col-md-4">
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ErrorMessage="Seleccione un banco" ControlToValidate="ListCuenta" InitialValue="-1" ValidationGroup="ChequeGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group">
                                                                    <label for="validateSelect" class="col-md-4">Banco</label>
                                                                    <div class="col-md-4">
                                                                        <asp:TextBox ID="txtBanco" runat="server" class="form-control" disabled></asp:TextBox>
                                                                    </div>
                                                                    <div class="col-md-4">
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Seleccione un banco" ControlToValidate="txtBanco" InitialValue="-1" ValidationGroup="ChequeGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group">
                                                                    <label for="validateSelect" class="col-md-4">Tipo Cuenta</label>
                                                                    <div class="col-md-4">
                                                                        <asp:TextBox ID="txtTipoCuenta" runat="server" class="form-control" disabled></asp:TextBox>
                                                                    </div>
                                                                    <div class="col-md-4">
                                                                        <asp:RequiredFieldValidator ControlToValidate="txtTipoCuenta" ID="RequiredFieldValidator20" runat="server" ErrorMessage="El campo es obligatorio" ValidationGroup="ChequeGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group">
                                                                    <label for="validateSelect" class="col-md-4">Numero Cuenta</label>
                                                                    <div class="col-md-4">
                                                                        <asp:TextBox ID="txtNroCuenta" runat="server" class="form-control" disabled></asp:TextBox>
                                                                    </div>
                                                                    <div class="col-md-4">
                                                                        <asp:RequiredFieldValidator ControlToValidate="txtNroCuenta" ID="RequiredFieldValidator3" runat="server" ErrorMessage="El campo es obligatorio" ValidationGroup="ChequeGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group">
                                                                    <label for="validateSelect" class="col-md-4">Cuit</label>
                                                                    <div class="col-md-4">
                                                                        <asp:TextBox ID="txtCuitCh" runat="server" class="form-control" disabled></asp:TextBox>
                                                                    </div>
                                                                    <div class="col-md-4">
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtCuitCh" ValidationGroup="ChequeGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                    </div>

                                                                    <div class="col-md-4">
                                                                        <asp:Label runat="server" ID="lblErrorCuit" ForeColor="Red" Font-Bold="true" Text="El CUIT ingresado es incorrecto" Visible="false"></asp:Label>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group">
                                                                    <label for="validateSelect" class="col-md-4">Librador</label>
                                                                    <div class="col-md-4">
                                                                        <asp:TextBox ID="txtLibradorCh" runat="server" class="form-control" disabled></asp:TextBox>
                                                                    </div>
                                                                    <div class="col-md-4">
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtLibradorCh" ValidationGroup="ChequeGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group">
                                                                    <label for="name" class="col-md-4">Fecha </label>

                                                                    <div class="col-md-4">
                                                                        <asp:TextBox ID="txtFechaCh" runat="server" class="form-control"></asp:TextBox>
                                                                    </div>
                                                                    <div class="col-md-4">
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtFechaCh" ValidationGroup="ChequeGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group">
                                                                    <label for="name" class="col-md-4">Importe</label>

                                                                    <div class="col-md-4">

                                                                        <div class="input-group">
                                                                            <span class="input-group-addon">$</span>
                                                                            <asp:TextBox ID="txtImporteCh" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)" Style="text-align: right"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="col-md-4">
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtImporteCh" ValidationGroup="ChequeGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group">
                                                                    <label for="name" class="col-md-4">Numero</label>

                                                                    <div class="col-md-4">
                                                                        <asp:TextBox ID="txtNumeroCh" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                                    </div>
                                                                    <div class="col-md-4">
                                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtNumeroCh" ValidationGroup="ChequeGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                    </div>
                                                                </div>
                                                                <div class="form-group">
                                                                    <div class="col-md-4">
                                                                        <asp:Button ID="btnAgregarPagoCh" runat="server" Text="Agregar" class="btn btn-success" ValidationGroup="ChequeGroup" OnClick="btnAgregarPagoCh_Click" />
                                                                    </div>
                                                                </div>

                                                            </fieldset>
                                                        </div>
                                                    </div>

                                                    <div class="widget-content">
                                                        <div class="col-md-12">
                                                            <div class="widget stacked widget-table">

                                                                <div class="widget-header">
                                                                    <span class="icon-external-link"></span>
                                                                    <h3>Cheques</h3>
                                                                </div>
                                                                <!-- .widget-header -->

                                                                <div class="widget-content">
                                                                    <table class="table table-bordered table-striped">                                                                        
                                                                        <thead>
                                                                            <tr>
                                                                                <th>Fecha</th>
                                                                                <th>Importe</th>
                                                                                <th>Numero</th>
                                                                                <th>Banco</th>
                                                                                <th>Cuenta</th>
                                                                                <th class="td-actions"></th>
                                                                            </tr>
                                                                        </thead>
                                                                        <tbody>
                                                                            <asp:PlaceHolder ID="phCheques" runat="server"></asp:PlaceHolder>
                                                                        </tbody>
                                                                    </table>

                                                                </div>
                                                                <!-- .widget-content -->

                                                            </div>

                                                        </div>
                                                    </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                    <%-- <asp:PostBackTrigger ControlID="ListCuenta" />--%>
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
                                                        <a class="accordion-toggle" data-toggle="collapse" data-parent=".accordion" href="#collapse3">Cheques Terceros
                                                        </a>
                                                    </h4>
                                                </td>
                                                <td style="width: 5%">
                                                    <asp:LinkButton ID="btnAgregarPago" runat="server" Text="<span class='shortcut-icon icon-plus'></span>" class="accordion-toggle" data-toggle="collapse" data-parent=".accordion" href="#collapse3" Style="color: black" />

                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="collapse3" class="panel-collapse collapse">
                                        <div class="panel-body">
                                            <asp:UpdatePanel ID="UpdatePanel7" UpdateMode="Always" runat="server">
                                                <ContentTemplate>
                                                    <div class="widget stacked widget-table">

                                                        <div class="widget-header">
                                                            <span class="icon-external-link"></span>
                                                            <h3>Cheques En Cartera</h3>
                                                        </div>
                                                        <!-- .widget-header -->

                                                        <div class="widget-content">
                                                            <table class="table table-bordered table-striped"  id="dataTables-example">
                                                                <thead>
                                                                    <tr>
                                                                        <th>Fecha</th>
                                                                        <th>Importe</th>
                                                                        <th>Numero</th>
                                                                        <th>Banco</th>
                                                                        <th>Cuenta</th>
                                                                        <th class="td-actions">
                                                                            <asp:LinkButton ID="LinkButton3" ValidationGroup="ChequeTercero" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnAgregar_Click1"></asp:LinkButton>
                                                                        </th>
                                                                    </tr>
                                                                </thead>
                                                                <tbody>
                                                                    <asp:PlaceHolder ID="phChequeCartera" runat="server"></asp:PlaceHolder>
                                                                </tbody>
                                                            </table>

                                                        </div>
                                                        <!-- .widget-content -->

                                                    </div>

                                                    <div class="widget-content">
                                                        <div class="col-md-12">
                                                            <div class="widget stacked widget-table">

                                                                <div class="widget-header">
                                                                    <span class="icon-external-link"></span>
                                                                    <h3>Cheques</h3>
                                                                </div>
                                                                <!-- .widget-header -->

                                                                <div class="widget-content">
                                                                    <table class="table table-bordered table-striped">

                                                                        <thead>
                                                                            <tr>
                                                                                <th>Fecha</th>
                                                                                <th>Importe</th>
                                                                                <th>Numero</th>
                                                                                <th>Banco</th>
                                                                                <th>Cuenta</th>
                                                                                <th class="td-actions"></th>
                                                                            </tr>
                                                                        </thead>
                                                                        <tbody>
                                                                            <asp:PlaceHolder ID="phChequesTerceros" runat="server"></asp:PlaceHolder>
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
                                    </div>
                                </div>

                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 95%">
                                                    <h4 class="panel-title">
                                                        <a class="accordion-toggle" data-toggle="collapse" data-parent=".accordion" href="#collapseThree">Transferencias
                                                        </a>
                                                    </h4>
                                                </td>
                                                <td style="width: 5%">
                                                    <asp:LinkButton ID="lbtnTransferencia" runat="server" Text="<span class='shortcut-icon icon-plus'></span>" class="accordion-toggle" data-toggle="collapse" data-parent=".accordion" href="#collapseThree" Style="color: black" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="collapseThree" class="panel-collapse collapse">
                                        <div class="panel-body">
                                            <asp:UpdatePanel ID="UpdatePanel5" UpdateMode="Always" runat="server">
                                                <ContentTemplate>
                                                    <div role="form" class="form-horizontal col-md-10">
                                                        <br />
                                                        <fieldset>
                                                            <div class="form-group">
                                                                <label for="name" class="col-md-4">Fecha Cobro</label>

                                                                <div class="col-md-4">
                                                                    <asp:TextBox ID="txtFechaTransf" runat="server" class="form-control"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <label for="name" class="col-md-4">Importe</label>

                                                                <div class="col-md-4">

                                                                    <div class="input-group">
                                                                        <span class="input-group-addon">$</span>
                                                                        <asp:TextBox ID="txtImporteTransf" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)" Style="text-align: right"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtImporteTransf" ValidationGroup="TransferGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <label for="validateSelect" class="col-md-4">Banco</label>
                                                                <div class="col-md-4">
                                                                    <asp:DropDownList ID="DropListBancoTransf" runat="server" class="form-control">
                                                                    </asp:DropDownList>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="Seleccione un banco" ControlToValidate="DropListBancoTransf" InitialValue="0" ValidationGroup="TransferGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <label for="validateSelect" class="col-md-4">Cuenta</label>
                                                                <div class="col-md-4">
                                                                    <asp:TextBox ID="txtCuentaTransf" runat="server" class="form-control"></asp:TextBox>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtCuentaTransf" ValidationGroup="TransferGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>

                                                            <div class="form-group">
                                                                <div class="col-md-4">
                                                                    <asp:Button ID="btn_AgregarPagoTrans" runat="server" Text="Agregar" class="btn btn-success" ValidationGroup="TransferGroup" OnClick="btn_AgregarPagoTrans_Click" />
                                                                </div>
                                                            </div>
                                                        </fieldset>
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
                                                        <a class="accordion-toggle" data-toggle="collapse" data-parent=".accordion" href="#collapseFour">Tarjetas
                                                        </a>
                                                    </h4>
                                                </td>
                                                <td style="width: 5%">
                                                    <h4 class="panel-title">
                                                        <asp:LinkButton ID="LinkButton1" runat="server" Text="<span class='shortcut-icon icon-plus'></span>" class="accordion-toggle" data-toggle="collapse" data-parent=".accordion" href="#collapseFour" Style="color: black" />
                                                    </h4>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="collapseFour" class="panel-collapse collapse">
                                        <div class="panel-body">
                                            <asp:UpdatePanel ID="UpdatePanel6" UpdateMode="Always" runat="server">
                                                <ContentTemplate>
                                                    <div role="form" class="form-horizontal col-md-10">
                                                        <br />
                                                        <fieldset>
                                                            <div class="form-group">
                                                                <label for="validateSelect" class="col-md-4">Tipo</label>
                                                                <div class="col-md-4">
                                                                    <asp:DropDownList ID="ListTarjetas" runat="server" class="form-control"></asp:DropDownList>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="Seleccione una Tarjeta" ControlToValidate="ListTarjetas" InitialValue="Seleccione..." ValidationGroup="TarjetaGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <label for="name" class="col-md-4">Monto</label>
                                                                <div class="col-md-4">

                                                                    <div class="input-group">
                                                                        <span class="input-group-addon">$</span>
                                                                        <asp:TextBox ID="txtMontoTarjeta" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)" Style="text-align: right"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtMontoTarjeta" ValidationGroup="TarjetaGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>

                                                            <div class="form-group">
                                                                <div class="col-md-4">
                                                                    <asp:Button ID="btnAgregarPagoTarjeta" runat="server" Text="Agregar" class="btn btn-success" ValidationGroup="TarjetaGroup" OnClick="btnAgregarPagoTarjeta_Click" />
                                                                </div>
                                                            </div>
                                                        </fieldset>
                                                    </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                                <p style="height: 1px"></p>                                

                            </div>

                        </section>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-md-12 col-xs-12 ">

                <div class="widget stacked">

                    <div class="widget-header">
                        <i class="icon-list"></i>
                        <h3>Detalle Pagos</h3>
                    </div>
                    <div class="widget-content">

                        <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Always" runat="server">
                            <ContentTemplate>

                                <table class="table table-striped table-bordered">
                                    <thead>
                                        <tr>
                                            <th style="width: 20%">Tipo Pago</th>
                                            <th>Monto</th>
                                            <th>Cotizacion</th>
                                            <th style="width: 20%">Total</th>
                                            <th></th>

                                        </tr>

                                    </thead>
                                    <tbody>
                                        <asp:PlaceHolder ID="phEfectivo" runat="server"></asp:PlaceHolder>
                                        <tr>
                                            <th></th>
                                            <th></th>
                                            <th style="width: 20%">Total a Ingresar</th>
                                            <th>
                                                <div class="input-group">
                                                    <span class="input-group-addon">$</span>
                                                    <asp:TextBox ID="txtTotalIngresar" runat="server" class="form-control" disabled="" Text="0" Style="text-align: right"></asp:TextBox>
                                                </div>
                                            </th>
                                            <th></th>
                                        </tr>
                                        <tr>
                                            <th></th>
                                            <th></th>
                                            <th style="width: 20%">Total Ingresado</th>
                                            <th>
                                                <div class="input-group">
                                                    <span class="input-group-addon">$</span>
                                                    <asp:TextBox ID="txtTotalIngresado" runat="server" class="form-control" disabled="" Text="0" Style="text-align: right"></asp:TextBox>
                                                </div>
                                            </th>
                                            <th></th>
                                        </tr>
                                        <tr>
                                            <th></th>
                                            <th></th>
                                            <th>
                                                <asp:Label runat="server" ID="lbRestan" Text="Restan"></asp:Label>
                                            </th>
                                            <th>
                                                <div class="input-group">
                                                    <span class="input-group-addon">$</span>
                                                    <asp:TextBox ID="txtRestan" runat="server" class="form-control" disabled="" Text="0" Style="text-align: right"></asp:TextBox>
                                                </div>
                                            </th>
                                            <th></th>
                                        </tr>
                                    </tbody>
                                </table>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                            <div class="form-group">
                            <label class="col-md-2">Observaciones:</label>
                            <div class="col-md-12">
                                <asp:TextBox ID="txtObservaciones" runat="server" class="form-control" PlaceHolder="Escriba aqui comentarios..." TextMode="MultiLine" Rows="6"></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <div class="col-md-4">
                                <a class="btn btn-success" data-toggle="modal" onclick="javascript:total();" href="#modalAgregar" id="btnFinalizarPago" runat="server">Finalizar Pago</a>
                                <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" class="btn btn-success" />
                                <asp:Button runat="server" ID="btnAtras" Text="Atras" class="btn btn-default" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </div>

        <div id="modalAgregar" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Recibo</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <div class="form-group">
                                <label class="col-md-4">Fecha</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtFechaPago" runat="server" class="form-control"></asp:TextBox>
                                </div>

                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Numero</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtNumeroCobro" runat="server" class="form-control" placeholder="Numero de Recibo del Proveedor"></asp:TextBox>
                                    <asp:RequiredFieldValidator ControlToValidate="txtNumeroCobro" SetFocusOnError="true" ID="RequiredFieldValidator21" runat="server" ErrorMessage="*" ValidationGroup="AgregarPagogroup"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Total Cobro</label>
                                <div class="col-md-6">

                                    <div class="input-group">
                                        <span class="input-group-addon">$</span>
                                        <asp:TextBox ID="txtTotalCobro" runat="server" class="form-control" Style="text-align: right" Disabled=""></asp:TextBox>
                                    </div>

                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Total Imputado</label>
                                <div class="col-md-6">

                                    <div class="input-group">
                                        <span class="input-group-addon">$</span>
                                        <asp:TextBox ID="txtTotalImputadoCobro" runat="server" class="form-control" Style="text-align: right" Disabled=""></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">                        
                        <asp:LinkButton ID="lbtnAgregarPago" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnAgregarPago_Click" ValidationGroup="AgregarPagogroup" />
                    </div>

                </div>
            </div>
        </div>


        <script src="../../Scripts/libs/jquery-1.9.1.min.js"></script>
        <script src="//code.jquery.com/ui/1.11.2/jquery-ui.js"></script>

        <!-- Page-Level Plugin Scripts - Tables -->
        <script src="//cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>
        <script src="../../Scripts/plugins/dataTables/custom.tables.js"></script>
        <link href="//cdn.datatables.net/1.10.2/css/jquery.dataTables.css" rel="stylesheet" />

        <script>
            $(document).ready(function () {
                $('#dataTables-example').dataTable({
                    "bLengthChange": false,
                    "pageLength": 15,
                    "bFilter": true,
                    "bInfo": false,
                    "bAutoWidth": false,
                    "language": {
                        "search" : "Buscar:"
                    }
                });
            });
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
            $(function () {
                $("#<%= txtFechaCh.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            });

            $(function () {
                $("#<%= txtFechaTransf.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            });
            $(function () {
                $("#<%= txtFechaPago.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            });
            
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


        <script>
            //window.onload = TotalImputado;
            //actualiza el textbox
            function total() {
                var valorN = document.getElementById('<%= txtTotalIngresado.ClientID %>').value;
                var valor = valorN.replace(',', '.');
                var total = parseFloat(valor);
                document.getElementById('<%= txtTotalCobro.ClientID %>').value = total.toFixed(2);
            }

            function TotalImputado() {
                var total = 0;
                for (var i = 1; i < document.getElementById('tbDocumentos').rows.length - 1; i++) {
                    var txtValor = document.getElementById('tbDocumentos').rows[i].cells[2].childNodes[0].value;
                    var valor = txtValor.replace(',', '.');
                    //if (valor > 0) {
                    if (valor != 0) {
                        total = total + parseFloat(valor);
                    }
                    else {
                        total = total;
                        //}
                    }
                }
                var ValorN = total.toFixed(2);
                document.getElementById('<%= TotalDoc.ClientID %>').value = ValorN;
                document.getElementById('<%= txtTotalImputadoCobro.ClientID %>').value = ValorN;
                document.getElementById('<%= txtTotalIngresar.ClientID %>').value = ValorN;
            }

        </script>

        <script>
            $(document).ready(
                function () {
                    var total = 0;
                    for (var i = 1; i < document.getElementById('tbDocumentos').rows.length - 1; i++) {
                        var txtValor = document.getElementById('tbDocumentos').rows[i].cells[2].childNodes[0].value;
                        var valor = txtValor.replace(',', '.');
                        //if (valor > 0) {
                        if (valor != 0) {
                            total = total + parseFloat(valor);
                        }
                        else {
                            total = total;
                        }
                        //}
                    }
                    var ValorN = total.toFixed(2);
                    document.getElementById('<%= TotalDoc.ClientID %>').value = ValorN;
                    document.getElementById('<%= txtTotalImputadoCobro.ClientID %>').value = ValorN;
                    document.getElementById('<%= txtTotalIngresar.ClientID %>').value = ValorN;
                    var restan = document.getElementById('<%= txtRestan.ClientID %>').value;
                    if (document.getElementById('MainContent_txtRestan').value == '0') {
                        document.getElementById('<%= txtRestan.ClientID %>').value = ValorN;
                    }
                }

        );
        </script>

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

        <script>
            function validaCuit(sCUIT) {

                var aMult = '5432765432';

                var aMult = aMult.split('');



                if (sCUIT && sCUIT.length == 11) {

                    aCUIT = sCUIT.split('');

                    var iResult = 0;

                    for (i = 0; i <= 9; i++) {

                        iResult += aCUIT[i] * aMult[i];

                    }

                    iResult = (iResult % 11);

                    iResult = 11 - iResult;


                    if (iResult == 11) iResult = 0;

                    if (iResult == 10) iResult = 9;

                    if (iResult == aCUIT[10]) {

                        return true;
                    }
                }
                return false;
            }
        </script>

        <script src="../../Scripts/libs/jquery-ui-1.10.0.custom.min.js"></script>

        <script src="../../Scripts/plugins/msgGrowl/js/msgGrowl.js"></script>
        <script src="../../Scripts/plugins/lightbox/jquery.lightbox.min.js"></script>
        <script src="../../Scripts/plugins/msgbox/jquery.msgbox.min.js"></script>

        <script src="../../Scripts/Application.js"></script>

        <script src="../../Scripts/demo/notifications.js"></script>

    </div>
</asp:Content>
