<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StockInicialABM.aspx.cs" Inherits="Gestion_Web.Formularios.Compras.StockInicialABM" %>

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
                                <div id="validation-form" role="form" class="form-horizontal col-md-7">
                                    <fieldset>
                                        <div class="form-group">
                                            <label for="name" class="col-md-3">Sucursal</label>
                                            <div class="col-md-6">
                                                <asp:DropDownList ID="ListSucursal" class="form-control" runat="server" AutoPostBack="True"></asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="name" class="col-md-3">Proveedor</label>
                                            <div class="col-md-6">
                                                <asp:DropDownList ID="ListProveedor" class="form-control" runat="server" AutoPostBack="True" ></asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="name" class="col-md-3">Numero</label>

                                            <div class="col-md-2">
                                                <asp:TextBox ID="txtPVenta" MaxLength="4" runat="server" class="form-control" onchange="completar4Ceros(this, this.value)"></asp:TextBox>
                                            </div>

                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtNumero" MaxLength="8" runat="server" class="form-control" onchange="completar8Ceros(this, this.value)"></asp:TextBox>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:RequiredFieldValidator ControlToValidate="txtPVenta" ID="RequiredFieldValidator30" runat="server" ErrorMessage="*" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>

                                            </div>
                                            <div class="col-md-1">
                                                <asp:RequiredFieldValidator ControlToValidate="txtNumero" ID="RequiredFieldValidator6" runat="server" ErrorMessage="*" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>

                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-3">Tipo remito</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="ListTipoRemito" runat="server" class="form-control">
                                                    <asp:ListItem Text="Seleccione..." Value="-1" />
                                                    <asp:ListItem Text="FC" Value="1" />
                                                    <asp:ListItem Text="PRP" Value="2" />
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:RequiredFieldValidator ControlToValidate="ListTipoRemito" ID="RequiredFieldValidator1" InitialValue="-1" runat="server" ErrorMessage="*" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>

                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-3">Tipo compra</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="ListDevolucion" runat="server" class="form-control">
                                                    <asp:ListItem Text="Seleccione..." Value="-1" />
                                                    <asp:ListItem Text="Compra Mercaderia" Value="0" />
                                                    <%--<asp:ListItem Text="Devolucion Mercaderia" Value="1" />--%>
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:RequiredFieldValidator ControlToValidate="ListDevolucion" ID="RequiredFieldValidator2" InitialValue="-1" runat="server" ErrorMessage="*" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>

                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-3">Fecha</label>

                                            <div class="col-md-4">
                                                <div class="input-group">
                                                    <span class="input-group-addon"><i class="icon-calendar"></i></span>
                                                    <asp:TextBox ID="txtFecha" runat="server" class="form-control"></asp:TextBox>
                                                </div>

                                            </div>
                                            <div class="col-md-4">
                                                <asp:RequiredFieldValidator ControlToValidate="txtFecha" ID="RequiredFieldValidator42" runat="server" ErrorMessage="Seleccione Fecha" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>

                                            </div>
                                        </div>
                                        <asp:Panel ID="Panel1" Visible="true" DefaultButton="lbtnAgregarArticuloASP" runat="server" class="col-md-12">
                                            <table class="table table-bordered">
                                                <thead>
                                                    <tr>
                                                        <th>Cantidad</th>
                                                        <th>Codigo</th>
                                                        <th></th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr>
                                                        <td style="width: 5%">
                                                            <asp:TextBox ID="txtCantidad" runat="server" class="form-control" Style="text-align: right" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 10%">
                                                            <div style="width: 100%">
                                                                <asp:TextBox ID="txtCodigo" runat="server" class="form-control"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td style="width: 5%">
                                                            <asp:LinkButton ID="lbtnAgregarArticuloASP" class="btn btn-info" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" OnClick="lbtnAgregarArticuloASP_Click" Visible="true" />
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </asp:Panel>
                                    </fieldset>
                                </div>
                                <div class="col-md-5">
                                    <div class="form-group">
                                        <label for="name" class="col-md-3">Observacion</label>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtComentario" runat="server" class="form-control" TextMode="MultiLine" Rows="5" />
                                        </div>
                                    </div>
                                </div>

                                <table class="table table-striped table-bordered">
                                    <thead>
                                        <tr>
                                            <th>Codigo</th>
                                            <th>Descripcion</th>
                                            <th>Cantidad</th>
                                            <th></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:PlaceHolder ID="phProductos" runat="server"></asp:PlaceHolder>
                                    </tbody>
                                </table>
                                <div class="form-group">
                                    <asp:Button ID="btnAgregar" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnAgregar_Click" />
                                    <asp:Button ID="btnAbrirModal" runat="server" Style="display: none;" Text="Confirmar" class="btn btn-success" data-toggle="modal" href="#modalConfirmacion" />
                                </div>
                                <div class="col-md-2">
                                    <asp:Label ID="lblVerCargados" Text="0" runat="server" Visible="false" />
                                </div>
                            </div>
                            <!-- /widget-content -->

                        </div>
                        <!-- /widget -->
                    </div>

                    <div id="modalConfirmacion" class="modal fade" tabindex="-1" role="dialog">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                                    <ContentTemplate>
                                        <div class="modal-header">
                                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                                            <h4 class="modal-title">Confirmacion de Generación de Remito</h4>
                                        </div>
                                        <div class="modal-body">
                                            <div role="form" class="form-horizontal col-md-12">
                                                <div class="form-group">
                                                    <label class="col-md-4">Códigos no encontrados:</label>
                                                    <div class="col-md-4">
                                                        <asp:Label ID="lblCodigosIncorrectos" runat="server" Font-Bold="True"></asp:Label>
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <div class="alert alert-danger alert-dismissable" visible="false" runat="server" id="msjAlerta">
                                                        <asp:Label ID="lblAlerta" runat="server" Font-Bold="True"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="modal-footer">
                                            <asp:Button runat="server" ID="btnConfirmarRemito" Text="Aceptar" OnClick="btnConfirmarRemito_Click" class="btn btn-success" />
                                            <asp:Label runat="server" ID="lblCierre"></asp:Label>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>

                </ContentTemplate>

            </asp:UpdatePanel>
        </div>
    </div>


    <script src="../../Scripts/plugins/dataTables/jquery.dataTables.min.js"></script>
    <script src="../../Scripts/plugins/dataTables/custom.tables.js"></script>


    <!-- Core Scripts - Include with every page -->
    <script src="../../Scripts/jquery-1.10.2.js"></script>
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

    <script src="../../Scripts/JSFunciones1.js"></script>


    <script src="//code.jquery.com/ui/1.10.4/jquery-ui.js"></script>

    <!-- Page-Level Plugin Scripts - Tables -->
    <script src="//cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>
    <script src="../../Scripts/plugins/dataTables/custom.tables.js"></script>
    <link href="//cdn.datatables.net/1.10.2/css/jquery.dataTables.css" rel="stylesheet" />

    <script>
        $(function () {
            $("#<%= txtFecha.ClientID %>").datepicker(
                {
                    dateFormat: 'dd/mm/yy'
                }
                );
        });
    </script>

    <script>

        function abrirdialog() {
            document.getElementById('<%= btnAbrirModal.ClientID %>').click();
        }

    </script>

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endReq);
        function endReq(sender, args) {
            $(function () {
                $("#<%= txtFecha.ClientID %>").datepicker(
                    {
                        dateFormat: 'dd/mm/yy'
                    }
                    );
            });
        }
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
