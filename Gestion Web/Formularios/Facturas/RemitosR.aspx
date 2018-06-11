<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RemitosR.aspx.cs" Inherits="Gestion_Web.Formularios.Facturas.RemitosR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">

        <div>
            <div class="col-md-12 col-xs-12 hidden-print">
                <div class="widget stacked">

                    <div class="stat">                        
                        <h5><i class="icon-map-marker"></i> Ventas > Remitos</h5>
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
                                                <asp:LinkButton ID="lbtnRemitir" runat="server" Enabled="false" data-toggle="modal" href="#modalConfirmacion">Anular</asp:LinkButton>

                                            </li>
                                            <li>
                                                <asp:LinkButton ID="lbtnFacturar" runat="server" OnClick="lbtnFacturar_Click">Facturar</asp:LinkButton>
                                            </li>

                                        </ul>
                                    </div>
                                    <!-- /btn-group -->
                                </td>
                                <td style="width: 65%"></td>
                                <%--<td style="width: 5%">
                                    <div class="shortcuts" style="height: 100%">
                                        <a href="/" class="btn btn-primary ui-tooltip" data-toggle="tooltip" title data-original-title="Imprimir" style="width: 100%">
                                            <i class="shortcut-icon icon-print"></i>
                                        </a>
                                    </div>
                                </td>--%>

                                <td style="width: 5%">
                                    <div class="shortcuts" style="height: 100%">



                                        <a class="btn btn-primary" data-toggle="modal" href="#modalBusqueda" style="width: 100%">
                                            <i class="shortcut-icon icon-filter"></i>
                                        </a>
                                    </div>
                                </td>
                                <td style="width: 5%">
                                    <div class="shortcuts" style="height: 100%">



                                        <a href="ABMRemitos.aspx" class="btn btn-primary ui-tooltip" data-toggle="tooltip" title data-original-title="Agregar" style="width: 100%">
                                            <i class="shortcut-icon icon-plus"></i>
                                        </a>
                                    </div>
                                </td>
                                <%--                                        <td style="width: 25%">
                                            <label class="col-md-4">Desde</label>
                                            <div class="col-md-8">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtFechaDesde" runat="server" class="form-control"></asp:TextBox>
                                                </div>
                                                <!-- /input-group -->
                                            </div>
                                        </td>
                                        <td style="width: 25%">
                                            <label class="col-md-4">Hasta</label>
                                            <div class="col-md-8">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtFechaHasta" runat="server" class="form-control"></asp:TextBox>
                                                </div>
                                                <!-- /input-group -->
                                            </div>
                                        </td>
                                        <td style="width: 25%">
                                            <label class="col-md-4">Sucursal</label>
                                            <div class="col-md-8">
                                                <div class="input-group">
                                                    <asp:DropDownList ID="DropListSucursal" runat="server" class="form-control"></asp:DropDownList>
                                                </div>
                                                <!-- /input-group -->
                                            </div>
                                        </td>
                                        <td style="width: 15%">
                                            <div class="col-md-12">
                                                <span class="input-group-btn">
                                                    <asp:LinkButton ID="lbtnBuscar" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-primary" OnClick="btnBuscar_Click" />
                                                </span>
                                                <!-- /input-group -->
                                            </div>
                                        </td>
                                        <td style="width: 10%">
                                            <div class="shortcuts" style="height: 100%">



                                                <a href="ABMFacturas.aspx" class="btn btn-default ui-tooltip" data-toggle="tooltip" title data-original-title="Agregar" style="width: 100%">
                                                    <i class="shortcut-icon icon-plus"></i>
                                    </a>
                                                </div>
                                        </td>--%>
                            </tr>

                        </table>
                    </div>
                    <!-- /widget-content -->

                </div>
                <!-- /widget -->
            </div>
            <div class="col-md-12">
            <div class="widget big-stats-container stacked">
                <div class="widget-content">
                    <asp:PlaceHolder ID="phSaldo" runat="server" Visible="true">
                        <div id="big_stats" class="cf">
                            <div class="stat">
                                <h4>Saldo</h4>
                                <asp:Label ID="lblSaldo" runat="server" Text="" class="value" Visible="false"></asp:Label>
                            </div>
                            <!-- .stat -->
                        </div>
                    </asp:PlaceHolder>
                </div>
                <!-- /widget-content -->
            </div>
            <!-- /widget -->
        </div>
            <div class="col-md-12 col-xs-12">
                <div class="widget widget-table">

                    <div class="widget-header">
                        <i class="icon-th-list" style="width: 2%"></i>
                        <h3 style="width: 75%">Remitos
                        </h3>
                        <%--<h3>
                            <asp:Label ID="lblSaldo" runat="server" Style="text-align: end" Text="" ForeColor="#0099ff" Font-Bold="true"></asp:Label>
                        </h3>--%>
                    </div>
                    <div class="widget-content">
                        <div class="panel-body">

                            <%--<div class="col-md-12 col-xs-12">--%>
                            <%-- <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>--%>
                            <div class="table-responsive">
                                <a class="btn btn-info" style="display: none" data-toggle="modal" id="abreDialog" href="#modalRemitoDetalle">Agregar Tipo Cliente</a>
                                <table class="table table-striped table-bordered table-hover" id="dataTablesR-example">
                                    <thead>
                                        <tr>
                                            <th>Fecha</th>
                                            <%--<th>Tipo</th>--%>
                                            <th>Numero</th>
                                            <th>Razon</th>
                                            <%--<th>Neto</th>
                                                    <th>IVA</th>
                                                    <th>Perp</th>--%>
                                            <th>Total</th>
                                            <th></th>
                                        </tr>

                                    </thead>
                                    <tbody>
                                        <asp:PlaceHolder ID="phRemitos" runat="server"></asp:PlaceHolder>
                                    </tbody>
                                </table>

                            </div>


                            <%-- </ContentTemplate>
                                <Triggers>
                                </Triggers>
                            </asp:UpdatePanel>--%>
                        </div>


                        <!-- /.content -->

                    </div>

                </div>
            </div>





        </div>

        <div id="modalRemitoDetalle" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">

                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Detalle Remito</h4>
                    </div>
                    <div class="modal-body">
                        <asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Always" runat="server">
                            <ContentTemplate>
                                <div class="col-md-12">
                                    <section class="content invoice">
                                        <!-- title row -->
                                        <div class="row">
                                            <div class="col-xs-12">
                                                <h2 class="page-header">
                                                    <i class="fa fa-globe"></i>
                                                    <asp:Label ID="labelTipoRemito" runat="server" Text="Label"></asp:Label>

                                                    <small class="pull-right">
                                                        <asp:Label ID="labelFechaRemito" runat="server" Text="Label"></asp:Label></small>
                                                </h2>
                                            </div>
                                            <!-- /.col -->
                                        </div>
                                        <!-- info row -->
                                        <div class="row invoice-info">
                                            <div class="col-sm-4 invoice-col">


                                                <address>
                                                    <strong>Time Solution</strong><br>
                                                    Maipu 821 8 D<br>
                                                    Cuit: 3-0222222222-0<br>
                                                    Telefono: 4444-9090<br />
                                                    Email: info@timesolution.com
                           
                                                </address>
                                            </div>
                                            <!-- /.col -->
                                            <div class="col-sm-4 invoice-col">
                                                Cliente
                           
                                        <address>
                                            <strong>
                                                <asp:Label ID="labelNombreCliente" runat="server" Text="Label"></asp:Label></strong><br>
                                            <asp:Label ID="labelDireccion" runat="server" Text="Label"></asp:Label><br>
                                            <asp:Label ID="labelTipoCuit" runat="server" Text="Label"></asp:Label><br>
                                            <asp:Label ID="labelNroCuit" runat="server" Text="Label"></asp:Label><br />


                                        </address>
                                            </div>
                                            <!-- /.col -->
                                            <div class="col-sm-4 invoice-col">
                                                <b>Forma de Pago:</b><asp:Label ID="LabelFormaPAgo" runat="server" Text="Label"></asp:Label><br />
                                                <b>Vendedor:</b><asp:Label ID="labelVendedor" runat="server" Text="Label"></asp:Label><br />
                                                <%--  <b>Lista Precio:</b><asp:Label ID="LabelListaPrecio" runat="server" Text="Label"></asp:Label><br />--%>
                                            </div>
                                            <!-- /.col -->
                                        </div>
                                        <!-- /.row -->

                                        <!-- Table row -->
                                        <div class="row">
                                            <div class="col-xs-12 table-responsive">
                                                <table class="table table-striped">
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
                                                        <asp:PlaceHolder ID="phitems2" runat="server"></asp:PlaceHolder>

                                                        <%--<tr>
                                                    <td>1</td>
                                                    <td>Call of Duty</td>
                                                    <td>455-981-221</td>
                                                    <td>El snort testosterone trophy driving gloves handsome</td>
                                                    <td>$64.50</td>
                                                </tr>
                                                <tr>
                                                    <td>1</td>
                                                    <td>Need for Speed IV</td>
                                                    <td>247-925-726</td>
                                                    <td>Wes Anderson umami biodiesel</td>
                                                    <td>$50.00</td>
                                                </tr>
                                                <tr>
                                                    <td>1</td>
                                                    <td>Monsters DVD</td>
                                                    <td>735-845-642</td>
                                                    <td>Terry Richardson helvetica tousled street art master</td>
                                                    <td>$10.70</td>
                                                </tr>
                                                <tr>
                                                    <td>1</td>
                                                    <td>Grown Ups Blue Ray</td>
                                                    <td>422-568-642</td>
                                                    <td>Tousled lomo letterpress</td>
                                                    <td>$25.99</td>
                                                </tr>--%>
                                                    </tbody>
                                                </table>
                                            </div>
                                            <!-- /.col -->

                                            <!-- /.row -->

                                            <div class="row">
                                                <!-- accepted payments column -->
                                                <div class="col-xs-6">
                                                    <%-- <p class="lead">Payment Methods:</p>--%>
                                                    <%--<img src="../../img/credit/visa.png" alt="Visa" />
                                        <img src="../../img/credit/mastercard.png" alt="Mastercard" />
                                        <img src="../../img/credit/american-express.png" alt="American Express" />
                                        <img src="../../img/credit/paypal2.png" alt="Paypal" />--%>
                                                    <p class="text-muted well well-sm no-shadow" style="margin-top: 10px;">
                                                    </p>
                                                </div>
                                                <!-- /.col -->
                                                <div class="col-xs-6">
                                                    <%--<p class="lead">Amount Due 2/22/2014</p>--%>
                                                    <div class="table-responsive">
                                                        <table class="table">
                                                            <tr>
                                                                <th style="width: 50%">Neto:</th>
                                                                <td>
                                                                    <asp:Label ID="labelNeto" runat="server" Text="Label"></asp:Label></td>

                                                            </tr>
                                                            <tr>
                                                                <th>Descuento</th>
                                                                <td>
                                                                    <asp:Label ID="labelDescuento" runat="server" Text="Label"></asp:Label></td>
                                                            </tr>
                                                            <tr>
                                                                <th>Subtotal</th>
                                                                <td>
                                                                    <asp:Label ID="labelSubTotal" runat="server" Text="Label"></asp:Label></td>
                                                            </tr>
                                                            <tr>
                                                                <th>Iva</th>
                                                                <td>
                                                                    <asp:Label ID="labelIva" runat="server" Text="Label"></asp:Label></td>
                                                            </tr>
                                                            <tr>
                                                                <th>Retención</th>
                                                                <td>
                                                                    <asp:Label ID="labelRetencion" runat="server" Text="Label"></asp:Label></td>
                                                            </tr>
                                                            <tr>
                                                                <th>Total</th>
                                                                <td>
                                                                    <asp:Label ID="labelTotal" runat="server" Text="Label"></asp:Label></td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </div>
                                                <!-- /.col -->
                                            </div>

                                        </div>
                                        <!-- /.row -->

                                        <!-- this row will not appear when printing -->
                                        <div class="row no-print">
                                            <div class="col-xs-12">
                                                <button class="btn btn-default" onclick="window.print();"><i class="fa fa-print"></i>Print</button>
                                                <%-- <button class="btn btn-success pull-right"><i class="fa fa-credit-card"></i>Submit Payment</button>--%>
                                                <%--<but
                                            ton class="btn btn-primary pull-right" style="margin-right: 5px;"><i class="fa fa-download"></i>Generate PDF</but>--%>
                                            </div>
                                        </div>
                                    </section>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="modal-footer">
                        <%--<asp:Button ID="Button2" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnAgregarTipoCliente_Click"  />--%>
                    </div>

                </div>
            </div>
        </div>
        <%--Fin modalGrupo--%>

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
                                <label class="col-md-4">Desde</label>
                                <div class="col-md-4">

                                    <asp:TextBox ID="txtFechaDesde" runat="server" class="form-control"></asp:TextBox>

                                    <!-- /input-group -->
                                </div>
                                <div class="col-md-4">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaDesde" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Hasta</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtFechaHasta" runat="server" class="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaHasta" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                                <!-- /input-group -->

                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Sucursal</label>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="DropListSucursal" runat="server" class="form-control"></asp:DropDownList>
                                    <!-- /input-group -->
                                </div>
                                <div class="col-md-2">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListSucursal" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
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
                                        <asp:Label runat="server" ID="lblMensaje" Text="Esta seguro que desea anular los Remitos Seleccionados?" Style="text-align: center"></asp:Label>
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

        <link href="../../css/pages/reports.css" rel="stylesheet">

        <script>

            function abrirdialog() {
                document.getElementById('abreDialog').click();
            }

        </script>

        <!-- Core Scripts - Include with every page -->
        <script src="../../Scripts/jquery-1.10.2.js"></script>
        <script src="../../Scripts/bootstrap.min.js"></script>
        <script src="../Scripts/plugins/metisMenu/jquery.metisMenu.js"></script>

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

        <%--<script src="//code.jquery.com/jquery-1.9.1.js"></script>--%>
        <script src="//code.jquery.com/ui/1.10.4/jquery-ui.js"></script>

        <script>


            $(function () {
                $("#<%= txtFechaDesde.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
                                        });

                                        $(function () {
                                            $("#<%= txtFechaHasta.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            });

        </script>


        <!-- Page-Level Plugin Scripts - Tables -->
        <script src="//cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>
        <script src="../../Scripts/plugins/dataTables/custom.tables.js"></script>
        <link href="//cdn.datatables.net/1.10.2/css/jquery.dataTables.css" rel="stylesheet" />

        <%--    <script>
        $(document).ready(function () {
            $('#dataTablesR-example').dataTable({ "bFilter": false, "bInfo": false, "bPaginate": false });
        });
    </script>

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endReq);
        function endReq(sender, args) {
            $('#dataTablesR-example').dataTable({ destroy: true, "bFilter": false, "bInfo": false, "bPaginate": false });
        }
    </script>--%>
    </div>
</asp:Content>
