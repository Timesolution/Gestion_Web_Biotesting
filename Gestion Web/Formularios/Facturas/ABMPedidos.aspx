<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ABMPedidos.aspx.cs" Inherits="Gestion_Web.Formularios.Facturas.ABMPedidos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">

        <div class="container">

            <div class="row">
                <div class="col-md-12 col-xs-12">

                    <div class="widget stacked widget-table action-table">
                        <div class="widget-content">
                            <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Always" runat="server">
                                <ContentTemplate>
                                    <div class="col-md-12" style="padding:0px;">
                                        <table class="table table-striped table-bordered" style="width:100%;">
                                            <thead>
                                                <tr>
                                                    <th>Datos Cliente</th>
                                                    <th>Datos Pedido</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr>
                                                    <td style="width: 40%">
                                                        <div class="form-group">
                                                            <div class="col-md-8">
                                                                <asp:DropDownList ID="DropListClientes" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="DropListClientes_SelectedIndexChanged"></asp:DropDownList>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <div class="col-md-6">
                                                                    <asp:Panel ID="PanelBuscar" runat="server">
                                                                        <a class="btn btn-info" onclick="createC();">
                                                                            <i class="shortcut-icon icon-search"></i>
                                                                        </a>                                                                    
                                                                    </asp:Panel>
                                                                </div>
                                                                <div class="col-md-6">
                                                                    <a class="btn btn-info dropdown-toggle ui-tooltip" title data-original-title="Importar" onclick="validarCliente();">
                                                                        <i class="shortcut-icon icon-arrow-up"></i>
                                                                    </a>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </td>
                                                    <td style="width: 60%">

                                                        <div class="form-group">

                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtFecha" runat="server" class="form-control" disabled="" Style="text-align: center"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-8" style="text-align: right">
                                                                <h3>
                                                                    <asp:Label ID="labelNroPedido" runat="server" Text=""></asp:Label>
                                                                </h3>
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>

                                                <tr>
                                                    <td style="width: 40%">
                                                        <div role="form" class="form-horizontal col-md-12">
                                                            <div class="form-group">
                                                                <%--<label class="col-md-4">Cliente: </label>--%>
                                                                <div class="col-md-12">
                                                                    <h5>
                                                                        <asp:Label ID="labelCliente" runat="server" Text="" Font-Bold="true"></asp:Label>
                                                                    </h5>
                                                                </div>
                                                            </div>
                                                        </form>
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
                                                                <div class="col-md-4">
                                                                    <asp:DropDownList ID="ListPuntoVenta" runat="server" class="form-control" AutoPostBack="True" OnSelectedIndexChanged="ListPuntoVenta_SelectedIndexChanged"></asp:DropDownList>
                                                                </div>
                                                            </div>

                                                            <div class="form-inline">
                                                                <div class="col-md-2">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="" ControlToValidate="ListEmpresa" InitialValue="-1" ValidationGroup="PedidosGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                </div>

                                                                <div class="col-md-2">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="" ControlToValidate="ListSucursal" InitialValue="-1" ValidationGroup="PedidosGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                </div>

                                                                <div class="col-md-2">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="" ControlToValidate="ListPuntoVenta" InitialValue="-1" ValidationGroup="PedidosGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
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
                                                    <th style="width: 30%">
                                                        <label class="col-md-5">Formas Pago</label>
                                                    </th>
                                                    <th style="width: 30%">
                                                        <label class="col-md-4">Lista</label>
                                                    </th>
                                                    <th style="width: 5%">
                                                        <label class="col-md-4">Forma</label>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th style="width: 30%">
                                                        <div class="col-md-8">
                                                            <asp:DropDownList ID="DropListVendedor" runat="server" class="form-control"></asp:DropDownList>
                                                        </div>
                                                        <div class="col-md-2">
                                                            <%--<a class="btn btn-info" data-toggle="modal" href="#modalVendedor">
                                                                <i class="shortcut-icon icon-plus"></i>
                                                            </a>--%>
                                                        </div>

                                                        <div class="col-md-2">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListVendedor" InitialValue="-1" ValidationGroup="PedidosGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </th>
                                                    <th style="width: 30%">
                                                        <div class="col-md-8">
                                                            <asp:DropDownList ID="DropListFormaPago" runat="server" class="form-control"></asp:DropDownList>
                                                        </div>
                                                        <div class="col-md-2">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListFormaPago" InitialValue="-1" ValidationGroup="PedidosGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </th>
                                                    <th style="width: 30%">
                                                        <div class="col-md-8">
                                                            <asp:DropDownList ID="DropListLista" runat="server" class="form-control"></asp:DropDownList>
                                                        </div>
                                                        <div class="col-md-2">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListLista" InitialValue="-1" ValidationGroup="PedidosGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </th>
                                                    <th style="width: 10%">
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
                            <h3>Pedido

                            </h3>
                        </div>
                        <!-- /widget-header -->

                        <div class="widget-content">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>

                                     <div class="col-md-12">
                                        <asp:Label ID="lblDatosReferenciaArt" runat="server" class="alert alert-info alert-dismissable" ForeColor="Black" Width="100%" style="text-align:center;" Height="75%" Visible="false"/>
                                        <asp:Label ID="lblDescuentoCantidad" runat="server" class="alert alert-info alert-dismissable" ForeColor="Black" Width="100%" style="text-align:center;" Height="75%" Visible="false"/>
                                         <asp:Label ID="LabelPendientes" runat="server" class="alert alert-danger alert-dismissable" ForeColor="Black" Width="100%" style="text-align:center;" Height="75%" Visible="false"/>
                                    </div>                                    
                                    <table class="table table-striped table-bordered">
                                        <thead>
                                            <tr>
                                                <th>#</th>
                                                <th>Codigo</th>
                                                <th>Cantidad</th>
                                                <th>Descripcion</th>
                                                <th>IVA</th>
                                                <th>Des.%</th>
                                                <th>P. Unitario</th>
                                                <th>Total</th>
                                                <th></th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td style="width: 7%">
                                                    <asp:TextBox ID="txtRenglon" runat="server" class="form-control" Text="0" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                </td>
                                                <td style="width: 18%">
                                                    <div class="form-group">
                                                        <div class="col-md-12">
                                                            <div class="input-group">
                                                                <asp:TextBox ID="txtCodigo" runat="server" class="form-control" ></asp:TextBox>

                                                                <span class="input-group-btn">
                                                                    <a class="btn btn-info" onclick="createA();">
                                                                        <i class="shortcut-icon icon-search"></i>

                                                                    </a>
                                                                    <button runat="server" id="btnRun" style="display: none" onserverclick="btnBuscarProducto_Click" onclick="foco();" class="btn btn-info" title="Search">
                                                                        <i class="btn-icon-only icon-check-sign"></i>
                                                                    </button>
                                                                </span>

                                                            </div>

                                                        </div>
                                                    </div>
                                                </td>
                                                <td style="width: 8%">
                                                    <asp:TextBox ID="txtCantidad" runat="server" class="form-control" AutoPostBack="True" onkeypress="javascript:return validarNro(event)" onfocus="foco();" OnTextChanged="txtCantidad_TextChanged" Style="text-align: right"></asp:TextBox>
                                                    <asp:CheckBox ID="checkMarcar" runat="server" />
                                                    <asp:LinkButton ID="lbtnStockProd" runat="server" class="badge pull-right" Text="0" OnClick="lbtnStockProd_Click" ></asp:LinkButton>
                                                </td>
                                                <td style="width: 25%">

                                                    <asp:TextBox ID="txtDescripcion" runat="server" class="form-control" disabled TextMode="MultiLine" Rows="3"></asp:TextBox>
                                                </td>
                                                <td style="width: 9%">

                                                    <asp:TextBox ID="txtIva" runat="server" class="form-control" disabled></asp:TextBox>
                                                </td>
                                                <td style="width: 6%">

                                                    <asp:TextBox ID="TxtDescuentoArri" runat="server" class="form-control" Text="0" MaxLength="5" OnTextChanged="TxtDescuentoArri_TextChanged" onkeypress="javascript:return validarNro(event)" AutoPostBack="True"></asp:TextBox>

                                                </td>
                                                <td style="width: 14%">

                                                    <asp:TextBox ID="txtPUnitario" runat="server" class="form-control" disabled AutoPostBack="true" OnTextChanged="txtPUnitario_TextChanged"></asp:TextBox>
                                                </td>
                                                <td style="width: 15%">

                                                    <asp:TextBox ID="txtTotalArri" runat="server" class="form-control" disabled></asp:TextBox>
                                                </td>
                                                <td style="width: 10%">
                                                    <asp:LinkButton ID="lbtnAgregarArticuloASP" class="btn btn-info" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" OnClick="btnAgregarArt_Click" Visible="true" />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>                                   

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
                                                                        <label for="name" class="col-md-4">Tipo Entrega</label>
                                                                        <div class="col-md-8">
                                                                            <asp:DropDownList ID="ListTipoEntrega" runat="server" class="form-control"></asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <label for="name" class="col-md-4">Domicilio Entrega</label>

                                                                        <div class="col-md-8">
                                                                            <asp:TextBox ID="txtDomicilioEntrega" runat="server" class="form-control"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <label for="name" class="col-md-4">Fecha Entrega</label>

                                                                        <div class="col-md-8">
                                                                            <asp:TextBox ID="txtFechaEntrega" runat="server" class="form-control"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <label for="name" class="col-md-4">Horario Entrega</label>
                                                                        <div class="col-md-8">                                                                            
                                                                            <asp:TextBox ID="txtHorarioEntrega" runat="server" class="form-control"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <label for="name" class="col-md-4">Zona Entrega</label>
                                                                        <div class="col-md-4">                                                                            
                                                                            <asp:DropDownList ID="DropListZonaEntrega" runat="server" class="form-control"></asp:DropDownList>
                                                                        </div>
                                                                    </div>
                                                                    <div class="form-group">                                                                        
                                                                        <label for="name" class="col-md-4">Seña</label>
                                                                        <div class="col-md-4">
                                                                            <div class="input-group">
                                                                                <span class="input-group-addon">$</span>
                                                                                <asp:TextBox ID="txtSenia" runat="server" Text="0" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                                            </div>
                                                                        </div>   
                                                                        <div class="col-md-2">
                                                                            <asp:Button ID="btnAgregar_senia" runat="server" Text="$" class="btn btn-success" OnClick="btnAgregar_senia_Click" ValidationGroup="PedidosGroup" />
                                                                        </div>                                                                     
                                                                    </div>
                                                                    <div class="form-group">
                                                                        <div class="col-md-12">
                                                                            <asp:TextBox placeholder="AGREGUE AQUI OBSERVACIONES" ID="txtComentarios" runat="server" class="form-control" TextMode="MultiLine" Rows="5"></asp:TextBox>
                                                                        </div>
                                                                    </div>
                                                                                                                                        
                                                                </asp:PlaceHolder>
                                                                
                                                                <div class="col-md-12">
                                                                    <asp:CheckBox ID="CheckBox1" TextAlign="Left" CssClass="pull-right" Text="Más Datos&nbsp&nbsp" runat="server" AutoPostBack="True" OnCheckedChanged="CheckBox1_CheckedChanged" />
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
                                                                    <label class="col-md-3">Percepción: </label>
                                                                    <div class="col-md-4">
                                                                        <asp:TextBox ID="txtPorcRetencion" Style="text-align: right" runat="server" class="form-control" AutoPostBack="True" OnTextChanged="txtPorcRetencion_TextChanged1" TextMode="Number" Text="0"></asp:TextBox>

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
                                    <%-- <asp:AsyncPostBackTrigger ControlID="btnAgregar" EventName="Click" />--%>
                                </Triggers>
                            </asp:UpdatePanel>
                            <div class="row">
                                <div role="form" class="form-horizontal col-md-12">
                                    <div class="col-md-12">
                                        <asp:Button ID="btnAgregar" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnAgregar_Click" ValidationGroup="PedidosGroup" />
                                        <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" class="btn btn-success" OnClick="btnNuevo_Click" />

                                        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" class="btn btn-default" PostBackUrl="~/Formularios/Facturas/ABMPedidos.aspx" />

                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>

                </div>
            </div>
        </div>
        <div id="modalImportar" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">

                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Importar Pedido</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                                <ContentTemplate>                                
                                    <div class="form-group">
                                        <label for="validateSelect" class="col-md-4">Archivo:</label>
                                        <div class="col-md-6">
                                            <asp:FileUpload ID="FileUpload1" runat="server" />
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RegularExpressionValidator ID="uplValidator" runat="server" ControlToValidate="FileUpload1" ValidationGroup="ImportarGroup" ForeColor="Red" ErrorMessage="* .csv" 
                                            ValidationExpression="(.+\.([Cc][Ss][Vv]))" ></asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                </ContentTemplate>
                        </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnImportarPedido" runat="server" Text="Importar" class="btn btn-success" OnClick="btnImportarPedido_Click" ValidationGroup="ImportarGroup" />
                    </div>

                </div>
            </div>
        </div>
        <%--Fin modalImportar--%>
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
                                <div class="col-md-4 pull-right">
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
                            <div class="form-group">
                                <label for="validateSelect" class="col-md-4">Descuento</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtDtoLista" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtDtoLista" ValidationGroup="ListaGroup"></asp:RequiredFieldValidator>
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


        <!-- /row -->

    </div>
    <!-- /container -->

    <%-- </div>--%>
    <!-- /main -->



    <%-- <!-- Core Scripts - Include with every page -->
  
    <script src="../../Scripts/libs/jquery-1.9.1.min.js"></script>
    <script src="../../Scripts/libs/jquery-ui-1.10.0.custom.min.js"></script>
    <script src="../../Scripts/libs/bootstrap.min.js"></script>

    <script src="../../Scripts/plugins/msgGrowl/js/msgGrowl.js"></script>
    <script src="../../Scripts/plugins/lightbox/jquery.lightbox.min.js"></script>
    <script src="../../Scripts/plugins/msgbox/jquery.msgbox.min.js"></script>

    <script src="../../Scripts/Application.js"></script>

    <%--<script src="../../Scripts/plugins/hoverIntent/jquery.hoverIntent.minified.js"></script>--%>



    <%-- <script src="../../Scripts/demo/gallery.js"></script>

      <script src="../../Scripts/jquery-1.10.2.js"></script>
    <script src="../../Scripts/bootstrap.min.js"></script>--%>
    <%--<script src="../Scripts/plugins/metisMenu/jquery.metisMenu.js"></script>--%>

    

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

        $(function () {
            $("#<%= txtFechaEntrega.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

    </script>

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endReq);
        function endReq(sender, args) {
            $(function () {
                $("#<%= txtFechaEntrega.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            });


        }
    </script>

    <%--    <script>

        $(function () {
            $("#<%= txtFecha.ClientID %>").datepicker('option', { dateFormat: 'dd/mm/yy' });
        });

    </script>--%>

    <script type="text/javascript">

        function darclick() {

            document.getElementById("<%= this.lbtnAgregarArticuloASP.ClientID %>").click();

            document.getElementById("<%= this.txtCodigo.ClientID %>").focus();
        }

        function foco() {
            document.getElementById("<%= this.txtCantidad.ClientID %>").focus();
            var note = document.getElementById("<%= this.txtCantidad.ClientID %>");
            var screenPosition = note.getBoundingClientRect();

            //window.scrollTo(0, screenPosition.bottom / 2);
        }

        function focoDesc() {
            document.getElementById("<%= this.TxtDescuentoArri.ClientID %>").focus();
        }

        function validarCliente() {
            var c = document.getElementById("<%= this.DropListClientes.ClientID %>");
            if (c.value > 0) {
                $('#modalImportar').modal('show');
            }
        }

    </script>

    <script src="../../js/daypilot-modal-2.0.js"></script>

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
            modal.showUrl("BuscarCliente.aspx?accion=4");
        }

        function createA() {
            //var d = document.getElementById("TheBody_txtDescripcion").value;
            //              var resource = d.options[d.selectedIndex].value;

            var ddlSucursal = document.getElementById("MainContent_ListSucursal");
            var idSucursal = ddlSucursal.selectedOptions[0].value;

            var modal = new DayPilot.Modal();
            modal.closed = function () {
                if (this.result == "OK") {
                    __doPostBack("UpdateButton", "");
                }
            };
            //modal.showUrl("ModalCreate.aspx?start=" + start + "&resource=" + resource);
            modal.showUrl("BuscarArticulos.aspx?accion=4&suc="+idSucursal);
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
        function createImpresion(value) {
            //var d = document.getElementById("TheBody_txtDescripcion").value;
            //              var resource = d.options[d.selectedIndex].value;

            var modal = new DayPilot.Modal();
            modal.closed = function () {
                if (this.result == "OK") {
                    __doPostBack("UpdateButton", "");
                }
            };
            //modal.showUrl("ModalCreate.aspx?start=" + start + "&resource=" + resource);
            modal.showUrl("PedidoImpresion.aspx?Pedido=" + value);
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

    <script>
        function imprimirCantidadPendientesCliente(value) {
            window.open(value, '_blank');
        }
    </script>

</asp:Content>
