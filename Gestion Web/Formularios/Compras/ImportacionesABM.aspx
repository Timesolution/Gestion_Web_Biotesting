<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ImportacionesABM.aspx.cs" Inherits="Gestion_Web.Formularios.Compras.ImportacionesABM" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">

        <div>
            <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                <ContentTemplate>
                    <div class="col-md-12 col-xs-12">
                        <div class="widget stacked">

                            <div class="widget-header">
                                <i class="icon-wrench"></i>
                                <h3>Herramientas</h3>
                            </div>
                            <!-- /widget-header -->

                            <div class="widget-content">
                                <div id="validation-form" role="form" class="form-horizontal col-md-10">
                                    <fieldset>
                                        <div class="form-group">
                                            <label for="name" class="col-md-3">Sucursal</label>
                                            <div class="col-md-3">
                                                <asp:DropDownList ID="ListSucursal" class="form-control" runat="server"></asp:DropDownList>
                                            </div>
                                            <label for="name" class="col-md-2">Proveedor</label>
                                            <div class="col-md-3">
                                                <asp:DropDownList ID="ListProveedor" class="form-control" runat="server"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:RequiredFieldValidator ControlToValidate="ListSucursal" InitialValue="-1" ID="RequiredFieldValidator4" runat="server" ErrorMessage="*" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:RequiredFieldValidator ControlToValidate="ListProveedor" InitialValue="-1" ID="RequiredFieldValidator5" runat="server" ErrorMessage="*" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-3">Nro Despacho</label>
                                            <div class="col-md-3">
                                                <asp:TextBox ID="txtNumeroDespacho" runat="server" class="form-control"></asp:TextBox>
                                            </div>
                                            <label for="name" class="col-md-2">Fecha Despacho</label>
                                            <div class="col-md-3">
                                                <div class="input-group">
                                                    <span class="input-group-addon"><i class="icon-calendar"></i></span>
                                                    <asp:TextBox ID="txtFechaDespacho" runat="server" class="form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:RequiredFieldValidator ControlToValidate="txtNumeroDespacho" ID="RequiredFieldValidator14" runat="server" ErrorMessage="*" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:RequiredFieldValidator ControlToValidate="txtFechaDespacho" ID="RequiredFieldValidator42" runat="server" ErrorMessage="*" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-3">Nro Factura</label>
                                            <div class="col-md-3">
                                                <asp:TextBox ID="txtNumeroFactura" runat="server" class="form-control"></asp:TextBox>
                                            </div>
                                            <label for="name" class="col-md-2">Fecha Factura</label>
                                            <div class="col-md-3">
                                                <div class="input-group">
                                                    <span class="input-group-addon"><i class="icon-calendar"></i></span>
                                                    <asp:TextBox ID="txtFechaFactura" runat="server" class="form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:RequiredFieldValidator ControlToValidate="txtNumeroFactura" ID="RequiredFieldValidator13" runat="server" ErrorMessage="*" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:RequiredFieldValidator ControlToValidate="txtFechaFactura" ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-3">Cotizacion Despacho U$D</label>
                                            <div class="col-md-3">
                                                <div class="input-group">
                                                    <span class="input-group-addon">$</span>
                                                    <asp:TextBox ID="txtCotizacionDespacho" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                </div>
                                            </div>
                                            <label for="name" class="col-md-2">RELACION U$S/EURO</label>
                                            <div class="col-md-3">
                                                <div class="input-group">
                                                    <span class="input-group-addon">$</span>
                                                    <asp:TextBox ID="txtRelacionEuro" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:RequiredFieldValidator ControlToValidate="txtCotizacionDespacho" ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:RequiredFieldValidator ControlToValidate="txtRelacionEuro" ID="RequiredFieldValidator3" runat="server" ErrorMessage="*" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-3">Total Factura</label>
                                            <div class="col-md-3">
                                                <div class="input-group">
                                                    <span class="input-group-addon">$</span>
                                                    <asp:TextBox ID="txtTotalFactura" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                </div>
                                            </div>                                            
                                            <label for="name" class="col-md-2">Moneda Factura</label>
                                            <div class="col-md-3">
                                                <asp:DropDownList ID="ListMonedaImportacion" runat="server" class="form-control" ></asp:DropDownList>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:RequiredFieldValidator ControlToValidate="txtTotalFactura" ID="RequiredFieldValidator11" runat="server" ErrorMessage="*" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:RequiredFieldValidator ControlToValidate="ListMonedaImportacion" InitialValue="-1" ID="RequiredFieldValidator12" runat="server" ErrorMessage="*" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>

                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-3">Total Despacho</label>
                                            <div class="col-md-3">
                                                <div class="input-group">
                                                    <span class="input-group-addon">$</span>
                                                    <asp:TextBox ID="txtTotalDespacho" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                </div>
                                            </div>                                            
                                            <label for="name" class="col-md-2">Coeficiente Despacho/Factura</label>
                                            <div class="col-md-3">
                                                <div class="input-group">
                                                    <span class="input-group-addon">%</span>
                                                    <asp:TextBox ID="txtCoeficienteDF" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:RequiredFieldValidator ControlToValidate="txtTotalDespacho" ID="RequiredFieldValidator6" runat="server" ErrorMessage="*" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:RequiredFieldValidator ControlToValidate="txtCoeficienteDF" ID="RequiredFieldValidator7" runat="server" ErrorMessage="*" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-3">Codigo Autorizacion DJAI</label>
                                            <div class="col-md-3">
                                                <asp:TextBox ID="txtCodigoAutorizacion" runat="server" class="form-control"></asp:TextBox>
                                            </div>                                            
                                            <label for="name" class="col-md-2">Nro Referencia</label>
                                            <div class="col-md-3">
                                                <asp:TextBox ID="txtNroReferencia" runat="server" class="form-control"></asp:TextBox>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:RequiredFieldValidator ControlToValidate="txtCodigoAutorizacion" ID="RequiredFieldValidator8" runat="server" ErrorMessage="*" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:RequiredFieldValidator ControlToValidate="txtNroReferencia" ID="RequiredFieldValidator9" runat="server" ErrorMessage="*" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-3">Tipo Cambio Gastos</label>
                                            <div class="col-md-3">
                                                <div class="input-group">
                                                    <span class="input-group-addon">$</span>
                                                    <asp:TextBox ID="txtCambioGastos" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:RequiredFieldValidator ControlToValidate="txtCambioGastos" ID="RequiredFieldValidator10" runat="server" ErrorMessage="*" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-3">Observaciones</label>
                                            <div class="col-md-8">
                                                <asp:TextBox ID="txtObservaciones" runat="server" class="form-control" TextMode="MultiLine" Rows="5"/>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <asp:Button ID="btnAgregar" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnAgregar_Click" ValidationGroup="ArticuloGroup"/>
                                            <asp:Button ID="btnVolver" runat="server" Text="Volver" class="btn btn-default" Onclick="btnVolver_Click" />
                                        </div>
                                    </fieldset>
                                </div>                               
                                
                            </div>
                            <!-- /widget-content -->
                        </div>
                        <!-- /widget -->
                    </div>

                </ContentTemplate>

            </asp:UpdatePanel>
        </div>
    </div>

    <!-- Core Scripts - Include with every page -->
    <script src="../../Scripts/jquery-1.10.2.js"></script>
    <script src="../../Scripts/bootstrap.min.js"></script>
    <script src="../../Scripts/libs/jquery-1.9.1.min.js"></script>
    <script src="../../Scripts/libs/jquery-ui-1.10.0.custom.min.js"></script>
    <script src="../../Scripts/libs/bootstrap.min.js"></script>
    <script src="../../Scripts/plugins/hoverIntent/jquery.hoverIntent.minified.js"></script>
    <script src="../../Scripts/Application.js"></script>
    <script src="../../Scripts/plugins/msgGrowl/js/msgGrowl.js"></script>
    <script src="../../Scripts/plugins/lightbox/jquery.lightbox.min.js"></script>
    <script src="../../Scripts/plugins/msgbox/jquery.msgbox.min.js"></script>
    <script src="../../Scripts/demo/notifications.js"></script>
    <script src="//code.jquery.com/ui/1.10.4/jquery-ui.js"></script>
    <!-- Page-Level Plugin Scripts - Tables -->
    <script src="//cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>
    <script src="../../Scripts/plugins/dataTables/custom.tables.js"></script>
    <link href="//cdn.datatables.net/1.10.2/css/jquery.dataTables.css" rel="stylesheet" />

    <script>
        $(function () {
            $("#<%= txtFechaFactura.ClientID %>").datepicker({dateFormat: 'dd/mm/yy'});
        });
        $(function () {
            $("#<%= txtFechaDespacho.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' }); 
        });
    </script>

    <script>
        $(document).ready(function () {
            $('#dataTables-example').dataTable({

                "bInfo": false,
                "bAutoWidth": false,
                "language": {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "Mostrar _MENU_ registros",
                    "sZeroRecords": "No se encontraron resultados",
                    "sEmptyTable": "Ningún dato disponible en esta tabla",
                    "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
                    "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
                    "sInfoPostFix": "",
                    "sSearch": "Buscar:",
                    "sUrl": "",
                    "sInfoThousands": ",",
                    "sLoadingRecords": "Cargando...",
                    "oPaginate": {
                        "sFirst": "Primero",
                        "sLast": "Último",
                        "sNext": "Siguiente",
                        "sPrevious": "Anterior"
                    },
                    "oAria": {
                        "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                        "sSortDescending": ": Activar para ordenar la columna de manera descendente"
                    }
                }

            });
        });
    </script>

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endReq);
        function endReq(sender, args) {
            $('#dataTables-example').dataTable({


                "bInfo": false,
                "bAutoWidth": false,

                "language": {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "Mostrar _MENU_ registros",
                    "sZeroRecords": "No se encontraron resultados",
                    "sEmptyTable": "Ningún dato disponible en esta tabla",
                    "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
                    "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
                    "sInfoPostFix": "",
                    "sSearch": "Buscar:",
                    "sUrl": "",
                    "sInfoThousands": ",",
                    "sLoadingRecords": "Cargando...",
                    "oPaginate": {
                        "sFirst": "Primero",
                        "sLast": "Último",
                        "sNext": "Siguiente",
                        "sPrevious": "Anterior"
                    },
                    "oAria": {
                        "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                        "sSortDescending": ": Activar para ordenar la columna de manera descendente"
                    }
                }


            });
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
                if (key == 46 || key == 8 || key == 44)// Detectar . (punto) y backspace (retroceso) y , (coma)
                {
                    return true;
                }
                else {
                    return false;
                }
            }
            return true;
        }
    </script>


</asp:Content>
