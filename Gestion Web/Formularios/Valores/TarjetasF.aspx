<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TarjetasF.aspx.cs" Inherits="Gestion_Web.Formularios.Valores.TarjetasF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">
        <div>
            <div class="col-md-12 col-xs-12">
                <div class="widget stacked">
                    <div class="stat">
                        <h5><i class="icon-map-marker"></i> Valores > Tarjetas</h5>
                    </div>
                    <div class="widget-header">
                        <i class="icon-wrench"></i>
                        <h3>Herramientas</h3>
                    </div>
                    <!-- /widget-header -->

                    <div class="widget-content">


                        <table style="width: 100%">
                            <tr>
                                <td style="width: 20%">
                                    <div class="btn-group">
                                        <button type="button" class="btn btn-success dropdown-toggle" data-toggle="dropdown" id="btnAccion" runat="server">Accion    <span class="caret"></span></button>
                                        <ul class="dropdown-menu">
                                            <li>
                                                <asp:LinkButton ID="btnLiquidar" runat="server" data-toggle="modal" href="#modalLiquidar">Liquidar</asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="btnEditar" runat="server" OnClick="btnEditar_Click" >Editar</asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="btnLiquidarArchivo" runat="server" data-toggle="modal" href="#modalLiquidarArchivo" style="display:none;">Liquidar desde archivo</asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="btnExportar" runat="server" OnClick="btnExportar_Click">Exportar</asp:LinkButton>
                                            </li>
                                        </ul>
                                    </div>
                                    <!-- /btn-group -->
                                </td>
                                <td style="width: 70%">
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
                                <td style="width: 5%">
                                    <div class="shortcuts" style="height: 100%">
                                        <a class="btn btn-primary" data-toggle="modal" href="#modalAgregar" style="width: 100%;">
                                            <i class="shortcut-icon icon-plus"></i>
                                        </a>
                                    </div>
                                </td>
                            </tr>

                        </table>
                    </div>
                    <!-- /widget-content -->

                </div>
                <!-- /widget -->
            </div>

            <div class="col-md-12 col-xs-12">
                <div class="widget big-stats-container stacked">
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
                <div class="widget stacked widget-table action-table">

                    <div class="widget-header">
                        <i class="icon-money" style="width: 2%"></i>
                        <h3 style="width: 75%">Tarjetas
                                    
                        </h3>
                    </div>
                    <div class="widget-content">
                        <div class="panel-body">

                            <%--<div class="col-md-12 col-xs-12">--%>
                            <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <div class="table-responsive">
                                        <table class="table table-striped table-bordered table-hover" id="dataTables-example">
                                            <thead>
                                                <tr>
                                                    <th>Fecha</th>
                                                    <th>Recibo</th>
                                                    <th>Sucursal</th>
                                                    <th>Nombre</th>
                                                    <th>Importe</th>
                                                    <th>Fecha Acreditacion</th>
                                                    <th>Liquidacion</th>
                                                    <th>#Lote</th>
                                                    <th>#Cupon</th>
                                                    <th>Ult. 4 digitos</th>                                                    
                                                    <th>Observaciones</th>
                                                    <th>Origen Pago</th>
                                                    <th></th>
                                                </tr>

                                            </thead>
                                            <tbody>
                                                <asp:PlaceHolder ID="phTarjetas" runat="server"></asp:PlaceHolder>
                                            </tbody>
                                        </table>

                                    </div>


                                </ContentTemplate>
                                <Triggers>
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>


                        <!-- /.content -->

                    </div>

                </div>
            </div>





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

                            <div class="form-group">
                                <label class="col-md-3">
                                    Fecha Acreditacion
                                </label>
                                <div class="col-md-3">
                                    <asp:TextBox ID="txtFechaDesde" placeholder="Desde" runat="server" class="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <asp:TextBox ID="txtFechaHasta" placeholder="Hasta" runat="server" class="form-control"></asp:TextBox>
                                    <!-- /input-group -->
                                </div>
                                <div class="col-md-1">
                                    <asp:RadioButton ID="RadioFecha" Checked="true" runat="server" GroupName="fecha" />
                                    <!-- /input-group -->
                                </div>
                                <div class="col-md-1">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaDesde" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-1">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaHasta" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                            </div>

                            <div class="form-group">
                                <label class="col-md-3">
                                    Fecha Venta
                                </label>
                                <div class="col-md-3">
                                    <asp:TextBox ID="txtDesdeV" placeholder="Desde" runat="server" class="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <asp:TextBox ID="txtHastaV" placeholder="Hasta" runat="server" class="form-control"></asp:TextBox>
                                    <!-- /input-group -->
                                </div>
                                <div class="col-md-1">
                                    <asp:RadioButton ID="RadioFechaVenta" Checked="true" runat="server" GroupName="fecha" />
                                    <!-- /input-group -->
                                </div>
                                <div class="col-md-1">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtDesdeV" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-1">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtHastaV" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                            </div>

                            <div class="form-group">
                                <label class="col-md-3">Operador</label>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="ListOperadores" runat="server" class="form-control" />
                                </div>
                            </div>

                            <div class="form-group">
                                <label class="col-md-3">Nombre Tarjeta</label>
                                <div class="col-md-6">
                                    <asp:TextBox ID="txtTarjeta" runat="server" class="form-control"></asp:TextBox>
                                </div>
                                <!-- /input-group -->
                            </div>

                            <div class="form-group">
                                <label class="col-md-3">Sucursal</label>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="DropListSucursal" runat="server" class="form-control"></asp:DropDownList>
                                    <!-- /input-group -->
                                </div>
                                <div class="col-md-2">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListSucursal" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-3">Estado</label>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="DropListEstado" runat="server" class="form-control">
                                        <asp:ListItem Text="Todos" Value="-1" />
                                        <asp:ListItem Text="Sin Liquidar" Value="0" />
                                        <asp:ListItem Text="Liquidado" Value="1" />
                                    </asp:DropDownList>
                                    <!-- /input-group -->
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-3">Origen Pago tarjeta:</label>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="ListOrigenPago" runat="server" class="form-control" >
                                        <asp:ListItem Text="Seleccione..." Value="-1" />
                                        <asp:ListItem Text="Todos" Value="0" />
                                        <asp:ListItem Text="Por Venta" Value="1" />
                                        <asp:ListItem Text="Por Anticipo" Value="2" />
                                        <asp:ListItem Text="Por Otras Cobranzas" Value="3" />
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-1">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" InitialValue="-1" ErrorMessage="<h3>*</h3>" ControlToValidate="ListOrigenPago" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                    </div>


                    <div class="modal-footer">
                        <asp:LinkButton ID="lbtnBuscar" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnBuscar_Click" ValidationGroup="BusquedaGroup" />
                    </div>
                </div>

            </div>
        </div>

        <div id="modalLiquidar" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">

                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Agregar Liquidacion</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <div class="form-group">
                                <label class="col-md-4">Numero Liquidacion:</label>
                                <div class="col-md-6">
                                    <asp:TextBox ID="txtNumeroLiquidacion" runat="server" class="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtNumeroLiquidacion" ValidationGroup="ImputarGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <%--<div class="form-group">
                                <label class="col-md-4">Numero Lote:</label>
                                <div class="col-md-6">
                                    <asp:TextBox ID="txtNumeroLote" runat="server" class="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtNumeroLote" ValidationGroup="ImputarGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Numero Cupon:</label>
                                <div class="col-md-6">
                                    <asp:TextBox ID="txtNumeroCupon" runat="server" class="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtNumeroCupon" ValidationGroup="ImputarGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Ultimos 4 dig. tarjeta:</label>
                                <div class="col-md-6">
                                    <asp:TextBox ID="txtUltimosDigitos" runat="server" class="form-control" maxlength="4"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtUltimosDigitos" ValidationGroup="ImputarGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                            </div>--%>

                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:LinkButton ID="btnSi" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnSi_Click" ValidationGroup="ImputarGroup" />
                    </div>
                </div>

            </div>
        </div>

        <div id="modalAgregar" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">

                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Agregar pago tarjeta</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <div class="form-group">
                                <label class="col-md-4">Fecha:</label>
                                <div class="col-md-5">
                                    <div class="input-group">
                                        <span class="input-group-addon">
                                            <i class="fa fa-calendar" aria-hidden="true"></i>
                                        </span>
                                        <asp:TextBox ID="txtFechaAgregar" runat="server" class="form-control" style="text-align:right;"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-2">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaAgregar" ValidationGroup="AgregarGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Sucursal</label>
                                <div class="col-md-5">
                                    <asp:DropDownList ID="ListSucursalAgregar" runat="server" class="form-control"></asp:DropDownList>
                                    <!-- /input-group -->
                                </div>
                                <div class="col-md-2">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="ListSucursalAgregar" InitialValue="-1" ValidationGroup="AgregarGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            
                            <div class="form-group">
                                <label class="col-md-4">Tarjeta:</label>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="ListAgregarTarjeta" runat="server" class="form-control" />
                                </div>
                                <div class="col-md-2">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" InitialValue="-1" ErrorMessage="<h3>*</h3>" ControlToValidate="ListAgregarTarjeta" ValidationGroup="AgregarGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                            </div>                            
                            <div class="form-group">
                                <label for="name" class="col-md-4">Importe</label>
                                <div class="col-md-5">
                                    <div class="input-group">
                                        <span class="input-group-addon">$</span>
                                        <asp:TextBox ID="txtImporteAgregar" runat="server" class="form-control" Text="0" onkeypress="javascript:return validarNro(event)" Style="text-align: right"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-1">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtImporteAgregar" ValidationGroup="AgregarGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Origen Pago tarjeta:</label>
                                <div class="col-md-5">
                                    <asp:DropDownList ID="ListAgregarOrigenTarjeta" runat="server" class="form-control" >
                                        <asp:ListItem Text="Seleccione..." Value="-1" />
                                        <asp:ListItem Text="Todos" Value="0" />
                                        <asp:ListItem Text="Por Venta" Value="1" />
                                        <asp:ListItem Text="Por Anticipo" Value="2" />
                                        <asp:ListItem Text="Por Otras Cobranzas" Value="3" />
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-1">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" InitialValue="-1" ErrorMessage="<h3>*</h3>" ControlToValidate="ListAgregarOrigenTarjeta" ValidationGroup="AgregarGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Numero Liquidacion:</label>
                                <div class="col-md-5">
                                    <asp:TextBox ID="txtAgregarNumeroLiquidacion" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Numero Lote:</label>
                                <div class="col-md-5">
                                    <asp:TextBox ID="txtAgregarNumeroLote" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Numero Cupon:</label>
                                <div class="col-md-5">
                                    <asp:TextBox ID="txtAgregarNumeroCupon" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Ultimos 4 dig. tarjeta:</label>
                                <div class="col-md-5">
                                    <asp:TextBox ID="txtAgregarUltimosDigitos" runat="server" class="form-control"  onkeypress="javascript:return validarNro(event)" maxlength="4"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Observaciones:</label>
                                <div class="col-md-5">
                                    <asp:TextBox ID="txtAgregarObservaciones" runat="server" class="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                </div>
                            </div>

                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:LinkButton ID="lbtnAgregarPagoTarjeta" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnAgregarPagoTarjeta_Click"  ValidationGroup="AgregarGroup" />
                    </div>
                </div>

            </div>
        </div>

        <div id="modalLiquidarArchivo" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Agregar Liquidacion</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <label for="validateSelect" class="col-md-4">Archivo</label>
                            <div class="col-md-8">
                                <asp:FileUpload ID="FileUpload1" runat="server" />
                                <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="FileUpload1" ErrorMessage="Debe cargar el archivo .csv!" ForeColor="Red" SetFocusOnError="true"></asp:RequiredFieldValidator>--%>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:LinkButton ID="btnAgregarArchivo" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnAgregarArchivo_Click" />
                    </div>

                </div>

            </div>
        </div>

        <div id="modalEditar" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">

                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Editar Datos</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <div class="form-group">
                                <label class="col-md-4">Tarjeta:</label>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="ListEditarTarjeta" runat="server" class="form-control" />
                                </div>
                                <div class="col-md-2">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" InitialValue="-1" ErrorMessage="<h3>*</h3>" ControlToValidate="ListEditarTarjeta" ValidationGroup="EditarGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Numero Lote:</label>
                                <div class="col-md-6">
                                    <asp:TextBox ID="txtNumeroLote" runat="server" class="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtNumeroLote" ValidationGroup="EditarGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Numero Cupon:</label>
                                <div class="col-md-6">
                                    <asp:TextBox ID="txtNumeroCupon" runat="server" class="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtNumeroCupon" ValidationGroup="EditarGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Ultimos 4 dig. tarjeta:</label>
                                <div class="col-md-6">
                                    <asp:TextBox ID="txtUltimosDigitos" runat="server" class="form-control" maxlength="4"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtUltimosDigitos" ValidationGroup="EditarGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Observaciones:</label>
                                <div class="col-md-6">
                                    <asp:TextBox ID="txtObservaciones" runat="server" class="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                </div>
                            </div>
                            <asp:Label ID="lblIdPago" Text="0" runat="server" Visible="false"/>
                            <asp:Label ID="lblIdTarjeta" Text="0" runat="server" Visible="false"/>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:LinkButton ID="lbtnEditar" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnEditar_Click" ValidationGroup="EditarGroup" />
                    </div>
                </div>

            </div>
        </div>
        
        <link href="../../css/pages/reports.css" rel="stylesheet">


        <!-- Core Scripts - Include with every page -->
        <%--<script src="../../Scripts/jquery-1.10.2.js"></script>--%>
        <script src="../../Scripts/bootstrap.min.js"></script>        

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
        <script src="//cdn.datatables.net/plug-ins/1.10.9/sorting/date-eu.js"></script>
        <script src="../../Scripts/plugins/dataTables/custom.tables.js"></script>

        <link href="//cdn.datatables.net/1.10.2/css/jquery.dataTables.css" rel="stylesheet" />

        <script>

            function abrirdialog() {
                document.getElementById('abreDialog').click();
            }

        </script>

        <script>
            $(document).ready(function () {
                $('#dataTables-example').dataTable({
                    "bLengthChange": false,
                    "bFilter": false,
                    "bInfo": false,
                    "bAutoWidth": false,
                    "columnDefs": [
                        { type: 'date-eu', targets: 0 }
                    ],
                    "columnDefs": [
                        { type: 'date-eu', targets: 4 }
                    ]
                });
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
                    if (key == 46 || key == 8 ) // Detectar . (punto) , backspace (retroceso) y , (coma)
                    { return true; }
                    else
                    { return false; }
                }
                return true;
            }
        </script>  

        <script>
            function pageLoad() {
                $("#<%= txtFechaAgregar.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            }
        </script>

        <script>

            $(function () {
                $("#<%= txtDesdeV.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            });

            $(function () {
                $("#<%= txtHastaV.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            });
            $(function () {
                $("#<%= txtFechaDesde.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            });

            $(function () {
                $("#<%= txtFechaHasta.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            });

            $(function () {
                $("#<%= txtFechaAgregar.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            });

        </script>

        <script type="text/javascript">
            function openModal() {
                $('#modalEditar').modal('show');
            }
        </script>


    </div>
</asp:Content>

