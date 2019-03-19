<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EntregasMercaderiaF.aspx.cs" Inherits="Gestion_Web.Formularios.Compras.EntregasMercaderiaF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">

        <div>
            <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Always">
                <ContentTemplate>
                    <div class="col-md-12 col-xs-12">
                        <div class="widget stacked">

                            <div class="widget-header">
                                <i class="icon-wrench"></i>
                                <h3>Herramientas</h3>
                            </div>
                            <!-- /widget-header -->

                            <div class="widget-content">
                                <div id="validation-form" role="form" class="form-horizontal col-md-8">
                                    <fieldset>
                                        <div class="form-group">
                                            <label for="name" class="col-md-2">Sucursal</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="ListSucursal" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ListSucursal_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-2">Pto Venta</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="ListPtoVenta" runat="server" AutoPostBack="True"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-2">Proveedor</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="ListProveedor" runat="server" AutoPostBack="True"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                                                    <ProgressTemplate>
                                                        <i class="fa fa-spinner fa-spin"></i><span>&nbsp;&nbsp;Cargando artículos del Proveedor. Por favor aguarde.</span>
                                                    </ProgressTemplate>
                                                </asp:UpdateProgress>
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="name" class="col-md-2">Numero</label>

                                            <div class="col-md-2">
                                                <asp:TextBox ID="txtPVenta" MaxLength="4" runat="server" class="form-control" onchange="completar4Ceros(this, this.value)"></asp:TextBox>
                                            </div>

                                            <div class="col-md-3">
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
                                            <label for="name" class="col-md-2">Fecha Mercaderia Arribada</label>

                                            <div class="col-md-3">
                                                <div class="input-group">
                                                    <span class="input-group-addon"><i class="icon-calendar"></i></span>
                                                    <asp:TextBox ID="txtFechaMercaderiaArribo" runat="server" class="form-control"></asp:TextBox>
                                                </div>

                                            </div>
                                            <div class="col-md-2">
                                                <asp:RequiredFieldValidator ControlToValidate="txtFechaMercaderiaArribo" ID="RequiredFieldValidator42" runat="server" ErrorMessage="*" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>

                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="name" class="col-md-2">Fecha OC</label>

                                            <div class="col-md-3">
                                                <div class="input-group">
                                                    <span class="input-group-addon"><i class="icon-calendar"></i></span>
                                                    <asp:TextBox ID="txtFechaOC" runat="server" class="form-control" Disabled></asp:TextBox>
                                                </div>

                                            </div>
                                            <div class="col-md-2">
                                                <asp:RequiredFieldValidator ControlToValidate="txtFechaOC" ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>

                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-2">Fecha de Ingreso de mercaderia</label>

                                            <div class="col-md-3">
                                                <div class="input-group">
                                                    <span class="input-group-addon"><i class="icon-calendar"></i></span>
                                                    <asp:TextBox ID="txtFechaMercaderiaIngresada" runat="server" class="form-control"></asp:TextBox>
                                                </div>

                                            </div>
                                            <div class="col-md-2">
                                                <asp:RequiredFieldValidator ControlToValidate="txtFechaMercaderiaIngresada" ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>

                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-2">Observaciones</label>
                                            <div class="col-md-6">
                                                <asp:TextBox ID="txtObservaciones" runat="server" class="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                            </div>
                                        </div>
                                    </fieldset>
                                </div>
                                <table class="table table-striped table-bordered">
                                    <thead>
                                        <tr>
                                            <th>Codigo</th>
                                            <th>Descripcion</th>
                                            <th>Cantidad Pedida</th>
                                            <th>Cantidad ya Recibida</th>
                                            <th>Cantidad Recibida</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:PlaceHolder ID="phProductos" runat="server"></asp:PlaceHolder>
                                    </tbody>
                                </table>
                                <br />
                                <div class="btn-toolbar">
                                    <div class="btn-group">
                                        <asp:Button ID="btnAgregar" type="button" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnAgregar_Click" />
                                    </div>
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


    <div id="modalSeleccionarOpcion" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Opciones</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel runat="server" ID="UpdatePanel3" UpdateMode="Always">
                        <ContentTemplate>
                            <div role="form" class="form-horizontal">
                                <asp:Label runat="server" Font-Bold="true" Font-Size="Medium" Text="Hay cantidades recibidas mayores a las solicitadas, desea recibirlas?"></asp:Label>
                            </div>
                            <br />
                            <div role="form" class="form-horizontal col-md-12 text-center">
                                <asp:Button ID="btnRecibirTodo" type="button" runat="server" Text="Si, recibir todo" class="btn btn-success" OnClientClick="disableBtn(this)" OnClick="btnRecibirTodo_Click" />
                                <%--<input id ="btn" type="button" class="btn btn-warning" value="No, recibir solo lo solicitado" onclick="disableBtn(this)"/>--%>
                                <asp:Button ID="btnRecibirSoloLoSolicitado" runat="server" Text="No, recibir solo lo solicitado" class="btn btn-warning" OnClick="btnRecibirLoSolicitado_Click"/>
                                <%--<a id="aRecibirSoloLoSolicitado" runat="server" class="btn btn-warning" OnClick="disableBtn(this)">No, recibir solo lo solicitado</a>--%>
                                <asp:Button ID="btnRechazarTodo" runat="server" Text="Rechazar todo" class="btn btn-danger" OnClientClick="disableBtn(this)" OnClick="lbtnRechazarTodo_Click" />
                            </div>
                            <div class="modal-footer"></div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>

    <%--<div id="modalCerrarOrden" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">ATENCION!</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-lg-10">
                        <div class="col-md-10">
                            <asp:Label runat="server" Font-Size="Medium" Text="La cantidad recibida es inferior a la pedida, desea cerrar la orden?"></asp:Label>
                        </div>
                        <div class="form-group">
                            <fieldset>
                                <div class="col-md-12">
                                    <asp:Label runat="server" Font-Size="Medium" class="col-md-6">Nueva fecha de entrega</asp:Label>
                                    <div class="input-group">
                                        <span class="input-group-addon"><i class="icon-calendar"></i></span>
                                        <asp:TextBox ID="txtNuevaFechaEntrega" runat="server" class="form-control col-md-6"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <asp:RequiredFieldValidator ControlToValidate="txtNuevaFechaEntrega" ID="RequiredFieldValidator3" runat="server" ErrorMessage="*" ValidationGroup="NuevaFechaEntregaGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                </div>
                            </fieldset>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <div role="form" class="form-horizontal col-md-12 text-right">
                        <asp:LinkButton ID="lbtnGuardar" runat="server" Text="Guardar" class="btn btn-success" ValidationGroup="NuevaFechaEntregaGroup" OnClick="lbtnGuardar_Click" />
                        <asp:LinkButton ID="lbtnCerrar" runat="server" Text="Cerrar Orden" class="btn btn-danger" OnClick="lbtnCerrar_Click" />
                    </div>
                </div>
                </div>
            </div>
        </div>--%>

    <div id="modalCerrarOrden" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">ATENCION!</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <asp:UpdatePanel runat="server" ID="UpdatePanel2" UpdateMode="Always">
                            <ContentTemplate>
                                <div class="form-group">
                                    <label class="col-md-12" style="font-size: medium">La cantidad recibida es inferior a la pedida, desea cerrar la orden?</label>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4" style="font-size: small">Nueva fecha de entrega</label>
                                    <div class="input-group col-md-4">
                                        <span class="input-group-addon"><i class="icon-calendar"></i></span>
                                        <asp:TextBox ID="txtNuevaFechaEntrega" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:RequiredFieldValidator ControlToValidate="txtNuevaFechaEntrega" ID="RequiredFieldValidator3" runat="server" ErrorMessage="*" ValidationGroup="NuevaFechaEntregaGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group col-md-12">
                                    <asp:Label ID="lblFechaEntregaError" runat="server" class="text-danger" Style="font-size: small" Visible="false">La fecha de entrega es menor a la actual</asp:Label>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbtnGuardar" runat="server" Text="Guardar" class="btn btn-success" ValidationGroup="NuevaFechaEntregaGroup" OnClick="lbtnGuardar_Click" />
                    <asp:LinkButton ID="lbtnCerrar" runat="server" Text="Cerrar Orden" class="btn btn-danger" OnClick="lbtnCerrar_Click" />
                </div>
            </div>
        </div>
    </div>

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

    <script src="../../Scripts/JSFunciones1.js"></script>

    <script>
        function pageLoad() {
            $("#<%= txtNuevaFechaEntrega.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        }
    </script>

    <script>
        function openModal() {
            $('#modalSeleccionarOpcion').modal('show');
        }

        function openModal2() {
            $('#modalCerrarOrden').modal('show');
        }
    </script>

    <script>
        $(function () {
            $("#<%= txtNuevaFechaEntrega.ClientID %>").datepicker(
                {
                    dateFormat: 'dd/mm/yy'
                }
            );
        });
    </script>

    <script>
        $(function () {
            $("#<%= txtFechaMercaderiaArribo.ClientID %>").datepicker(
                {
                    dateFormat: 'dd/mm/yy'
                }
            );
        });
    </script>

    <script>
        $(function () {
            $("#<%= txtFechaMercaderiaIngresada.ClientID %>").datepicker(
                {
                    dateFormat: 'dd/mm/yy'
                }
            );
        });
    </script>

    <script>
        $(function () {
            $("#<%= txtFechaOC.ClientID %>").datepicker(
                {
                    dateFormat: 'dd/mm/yy'
                }
            );
        });
    </script>

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endReq);
        function endReq(sender, args) {
            $(function () {
                $("#<%= txtFechaMercaderiaArribo.ClientID %>").datepicker(
                    {
                        dateFormat: 'dd/mm/yy'
                    }
                );
            });
        }
    </script>

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endReq);
        function endReq(sender, args) {
            $(function () {
                $("#<%= txtFechaMercaderiaIngresada.ClientID %>").datepicker(
                    {
                        dateFormat: 'dd/mm/yy'
                    }
                );
            });
        }
    </script>

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endReq);
        function endReq(sender, args) {
            $(function () {
                $("#<%= txtFechaOC.ClientID %>").datepicker(
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
                "paging": false,
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

                "paging": false,
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

    <script>
        function disableBtn(t)
        {
            <%--var btn = document.getElementById('<%=btnRecibirTodo.ClientID %>');--%>
            <%--var btn2 = document.getElementById('<%=btnRecibirSoloLoSolicitado.ClientID %>');--%>
            <%--var btn3 = document.getElementById('<%=btnRechazarTodo.ClientID %>');--%>

            t.disabled = true;
            //btn2.disabled = true;
            //btn3.disabled = true;

            t.value = 'Aguarde…';

            //btn2.click();
            //btn2.value = 'Aguarde…';
            //btn3.value = 'Aguarde…';
            //__doPostBack('btnRecibirLoSolicitado_Click', '');

            return true;
        }
    </script>

</asp:Content>
