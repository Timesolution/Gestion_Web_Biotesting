<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ABMCotizaciones.aspx.cs" Inherits="Gestion_Web.Formularios.Facturas.ABMCotizaciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">

        <div class="container">
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
                                                    <th>Datos Cotizacion</th>
                                                </tr>

                                            </thead>
                                            <tbody>
                                                <tr>
                                                    <td style="width: 40%">
                                                        <div class="form-group">
                                                            <div class="col-md-8">
                                                                <asp:DropDownList ID="DropListClientes" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="DropListClientes_SelectedIndexChanged" ></asp:DropDownList>
                                                            </div>
                                                            
                                                             
                                                            <div class="col-md-2">
                                                                <a class="btn btn-info" onclick="createC();">
                                                                    <i class="shortcut-icon icon-search"></i>
                                                                </a>
                                                            </div>                                                                           
                                                                <div class="col-md-2">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListClientes" InitialValue="-1" ValidationGroup="CotizacionGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                </div>
                                                        </div>
                                                    </td>
                                                    <td style="width: 60%">
                                                        <div class="form-group">
                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtFecha" runat="server" class="form-control" disabled="" style="text-align:center"></asp:TextBox> 
                                                            </div>
                                                            <div class="col-md-8" style="text-align: right">
                                                                <h3>
                                                                    <asp:Label ID="labelNroCotizacion" runat="server" Text="" ></asp:Label>
                                                                </h3>
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                             <tr>
                                                    <td style="width: 40%">
                                                        <div role="form" class="form-horizontal col-md-12" >                                                          
                                                             <div class="form-group">
                                                                <%--<label class="col-md-4">Cliente: </label>--%>
                                                                <div class="col-md-12">
                                                                    <h5>
                                                                    <asp:Label ID="labelCliente" runat="server" Text="" Font-Bold="true"></asp:Label>
                                                                    </h5>
                                                                </div>
                                                            </div>
                                                            <%--<div class="form-group">
                                                                <label class="col-md-4">Iva: </label>
                                                                <div class="col-md-8">
                                                                    <asp:Label ID="labelSaldo" runat="server" Text=""></asp:Label>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <label class="col-md-4">CUIT: </label>
                                                                <div class="col-md-8">
                                                                    <asp:Label ID="labelCuit" runat="server" Text=""></asp:Label>
                                                                </div>--%>
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
                                                             <div class="col-md-4">
                                                                 <asp:DropDownList ID="ListPuntoVenta" runat="server" class="form-control" AutoPostBack="True" OnSelectedIndexChanged="ListPuntoVenta_SelectedIndexChanged"></asp:DropDownList>
                                                             </div>
                                                         </div>
                                                         
                                                            
                                                            <div class="form-inline">                                                                                   
                                                                <div class="col-md-2">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="" ControlToValidate="ListEmpresa" InitialValue="-1" ValidationGroup="CotizacionGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                </div>
                                                                                                                                                   
                                                                <div class="col-md-2">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="" ControlToValidate="ListSucursal" InitialValue="-1" ValidationGroup="CotizacionGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                </div>
                                                                                                                                                   
                                                                <div class="col-md-2">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="" ControlToValidate="ListPuntoVenta" InitialValue="-1" ValidationGroup="CotizacionGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
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
                                                    <th style="width: 30%">
                                                        <label class="col-md-4">Formas Pago</label>
                                                    </th>
                                                    <th style="width: 30%">
                                                        <label class="col-md-4">Lista</label>
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
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListVendedor" InitialValue="-1" ValidationGroup="CotizacionGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </th>                                        
                                                    <th style="width: 30%">
                                                        <div class="col-md-8">
                                                            <asp:DropDownList ID="DropListFormaPago" runat="server" class="form-control"></asp:DropDownList>
                                                        </div>
                                                        <div class="col-md-2">
                                                            <a class="btn btn-info" data-toggle="modal" href="#modalFormaPago">
                                                                <i class="shortcut-icon icon-plus"></i>
                                                            </a>
                                                        </div>
                                                        
                                                                                                                                                                
                                                        <div class="col-md-2">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListFormaPago" InitialValue="-1" ValidationGroup="CotizacionGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </th>
                                                    <th style="width: 30%">  
                                                        <div class="col-md-8">
                                                            <asp:DropDownList ID="DropListLista" runat="server" class="form-control"></asp:DropDownList>
                                                        </div>
                                                        <div class="col-md-2">
                                                            <a class="btn btn-info" data-toggle="modal" href="#modalLista">
                                                                <i class="shortcut-icon icon-plus"></i>
                                                            </a>
                                                        </div>
                                                        
                                                                                                                                                                
                                                        <div class="col-md-2">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListLista" InitialValue="-1" ValidationGroup="CotizacionGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
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
                            <h3>Cotizacion

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
                                                                    <button runat="server" style="display:none" id="btnRun" onserverclick="btnBuscarProducto_Click" class="btn btn-info" title="Search">
                                                                        <i class="btn-icon-only icon-check-sign"></i>
                                                                    </button>
<%--                                                                    <a href="../../formularios/articulos/articulos.aspx?accion=3" class="btn btn-info">--%>
                                                                    <a class="btn btn-info" onclick="createA();">
                                                                        <i class="shortcut-icon icon-search"></i>
                                                                        <%--<span class="shortcut-label">Users</span>--%>
                                                                    </a>
                                                                </span>

                                                            </div>
                                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCodigo" ErrorMessage="El campo es obligatorio" SetFocusOnError="True"></asp:RequiredFieldValidator>--%>

                                                        </div>
                                                    </div>
                                                </td>
                                                <td style="width: 8%">
                                                
                                                    <asp:TextBox ID="txtCantidad" runat="server" class="form-control" Text="0" AutoPostBack="True" OnTextChanged="txtCantidad_TextChanged"></asp:TextBox>
                                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="El campo es obligatorio" SetFocusOnError="True" ControlToValidate="txtCantidad"></asp:RequiredFieldValidator>--%>
                                                </td>
                                                <td style="width: 25%">
                                                
                                                    <asp:TextBox ID="txtDescripcion" runat="server" class="form-control" disabled TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                                <td style="width: 10%">
                                                
                                                    <asp:TextBox ID="txtIva" runat="server" class="form-control" disabled></asp:TextBox>
                                                </td>
                                                <td style="width: 10%">
                                                
                                                    <asp:TextBox ID="TxtDescuentoArri" runat="server" class="form-control" Text="0" OnTextChanged="TxtDescuentoArri_TextChanged" AutoPostBack="True"></asp:TextBox>
                                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="El campo es obligatorio" SetFocusOnError="True" ControlToValidate="TxtDescuentoArri"></asp:RequiredFieldValidator>--%>
                                                </td>
                                                <td style="width: 15%">
                                                
                                                    <asp:TextBox ID="txtPUnitario" runat="server" class="form-control" disabled></asp:TextBox>
                                                </td>
                                                <td style="width: 15%">
                                                
                                                    <asp:TextBox ID="txtTotalArri" runat="server" class="form-control" disabled></asp:TextBox>
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
                                    <div class="col-md-4 pull-right">
                                        <%--<table class="table table-striped table-bordered col-md-4">--%>
                                        <table class="table table-striped table-bordered">
                                            <%-- <thead>
                                            <tr>
                                                <th>Sub-Total $</th>
                                                <th>IVA 21%</th>
                                                <th>Desc</th>

                                            </tr>
                                        </thead>--%>
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
                                                                    <label class="col-md-3">Percepcíon: </label>
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

                                                    <%--<td>
                                                        <div  role="form" class="form-horizontal col-md-12">
                                                            <div class="form-group" style="text-align: left">
                                                                <label class="col-md-3">Neto: </label>
                                                                <div class="col-md-9">
                                                                    <asp:TextBox ID="txtNeto" style="text-align: right" runat="server" class="form-control col-md-4" Text="0.00" disabled></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            
                                                            <div class="form-group" style="text-align: left">
                                                                <label class="col-md-3">Descuento %: </label>
                                                                <div class="col-md-4">
                                                                        <asp:TextBox ID="txtPorcDescuento" style="text-align: right" runat="server" class="form-control" Text="0" AutoPostBack="True" OnTextChanged="txtDescuento_TextChanged" TextMode="Number"></asp:TextBox>
                                                                     
                                                                </div>
                                                                <div class="col-md-5">
                                                                    <asp:TextBox ID="txtDescuento" style="text-align: right" runat="server" class="form-control" Text="0.00" AutoPostBack="True" OnTextChanged="txtDescuento_TextChanged" disabled></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="form-group" style="text-align: left">
                                                                <label class="col-md-3">SubTotal: </label>
                                                                <div class="col-md-9">
                                                                    <asp:TextBox ID="txtsubTotal" style="text-align: right" runat="server" class="form-control col-md-4" Text="0.00" disabled></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="form-group" style="text-align: left">
                                                                <label class="col-md-3">Iva: </label>
                                                                <div class="col-md-9">
                                                                    <asp:TextBox ID="txtIvaTotal" style="text-align: right" runat="server" class="form-control" Text="0.00" disabled></asp:TextBox>
                                                                </div>
                                                            </div>

                                                            <div class="form-group" style="text-align: left">
                                                                <label class="col-md-3">Percepcíon: </label>
                                                                <div class="col-md-4">
                                                                 
                                                                        <asp:TextBox ID="txtPorcRetencion" Style="text-align: right" runat="server" class="form-control" AutoPostBack="True" OnTextChanged="txtPorcRetencion_TextChanged" TextMode="Number" Text="0"></asp:TextBox>
                                                          
                                                                </div>
                                                                <div class="col-md-5">
                                                                    <asp:TextBox ID="txtRetencion" style="text-align: right" runat="server" class="form-control" Text="0.00" disabled></asp:TextBox>
                                                                </div>
                                                            </div>

                                                            <div class="form-group" style="text-align: left">
                                                                <strong>
                                                                    <label class="col-md-3">Total:</label></strong>
                                                                <div class="col-md-9">
                                                                    <asp:TextBox ID="txtTotal" style="text-align: right" runat="server" class="form-control" Text="0.00" disabled Font-Bold="True"></asp:TextBox>
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
                            <div class="row">
                                        <div role="form" class="form-horizontal col-md-12">
                                            <div class="col-md-12">
                                                <asp:Button ID="btnAgregar" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnAgregar_Click" ValidationGroup="CotizacionGroup"/>
                                                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" class="btn btn-default" PostBackUrl="~/Formularios/Facturas/ABMCotizaciones.aspx" />
                                            </div>
                                        </div>
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
                               <asp:DropDownList ID="ListEmpleados" runat="server" class="form-control" ></asp:DropDownList>
                        </div>
                        <div class="col-md-4">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Seleccione un empleado" ControlToValidate="ListEmpleados" InitialValue="-1" ValidationGroup="VendedorGroup"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                     <div class="form-group">
                        <label for="validateSelect" class="col-md-4">Comision</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtComision" runat="server" class="form-control"  onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                        </div>
                         <div class="col-md-4">
                              <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtComision" ValidationGroup="VendedorGroup"></asp:RequiredFieldValidator>
                          </div>
                    </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnAgregarVendedor" runat="server" Text="Guardar" class="btn btn-success"  OnClick="btnAgregarVendedor_Click" ValidationGroup="VendedorGroup"/>
                </div>

            </div>
        </div>
    </div>  <%--Fin modalGrupo--%>

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
                            <asp:TextBox ID="txtDtoLista" runat="server" class="form-control"  onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                        </div>
                        <div class="col-md-4">
                              <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtDtoLista" ValidationGroup="ListaGroup"></asp:RequiredFieldValidator>
                         </div>
                    </div>
                </div>
                    </div>
                <div class="modal-footer">
                    <asp:Button ID="btnAgregarLista" runat="server" Text="Guardar" class="btn btn-success"  OnClick="btnAgregarLista_Click" ValidationGroup="ListaGroup"  />
                </div>

            </div>
        </div>
    </div>  <%--Fin modalGrupo--%>

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
                    <asp:Button ID="btnAgregarFP" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnAgregarFP_Click"  ValidationGroup="FPGroup"  />
                </div>

            </div>
        </div>
    </div>  <%--Fin modalGrupo--%>



        <!-- /row -->

    </div>
    <!-- /container -->

    <%-- </div>--%>
    <!-- /main -->

    <%--<link rel="stylesheet" href="//code.jquery.com/ui/1.11.1/themes/smoothness/jquery-ui.css">--%>
  <script src="//code.jquery.com/jquery-1.10.2.js"></script>
  <script src="//code.jquery.com/ui/1.11.1/jquery-ui.js"></script>

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
    <%--<script src="../../Scripts/demo/notifications.js"></script>--%>


    <script>

        $(function () {
            $("#<%= txtFecha.ClientID %>").datepicker('option', { dateFormat: 'dd/mm/yyyy' });
        });

    </script>

    <script type="text/javascript">

        function darclick() {

            document.getElementById("<%= this.lbtnAgregarArticuloASP.ClientID %>").click();


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
            modal.showUrl("BuscarCliente.aspx?accion=2");
        }

        function createA() {
            //var d = document.getElementById("TheBody_txtDescripcion").value;
            //              var resource = d.options[d.selectedIndex].value;

            var modal = new DayPilot.Modal();
            modal.closed = function () {
                if (this.result == "OK") {
                    __doPostBack("UpdateButton", "");
                }
            };
            //modal.showUrl("ModalCreate.aspx?start=" + start + "&resource=" + resource);
            modal.showUrl("BuscarArticulos.aspx?accion=2");
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
