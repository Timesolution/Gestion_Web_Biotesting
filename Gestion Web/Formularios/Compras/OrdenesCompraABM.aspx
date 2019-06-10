<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OrdenesCompraABM.aspx.cs" Inherits="Gestion_Web.Formularios.Compras.OrdenesCompraABM" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="main">
        <div>
<%--            <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Always">
                <ContentTemplate>--%>
                    <div class="col-md-12 col-xs-12">
                        <div class="widget stacked">
                            <div class="widget-header">
                                <i class="icon-wrench"></i>
                                <h3>Herramientas</h3>
                            </div>
                            <div class="widget-content">
                                <div id="validation-form" role="form" class="form-horizontal col-md-8">
                                    <fieldset>
                                        <div class="form-group">
                                            <label for="name" class="col-md-2">Sucursal</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="ListSucursal" class="form-control" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ListSucursal_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-2">Pto Venta</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="ListPtoVenta" class="form-control" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ListPtoVenta_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-2">Cod. Proveedor</label>
                                            <div class="col-md-3">
                                                <asp:TextBox ID="txtCodProveedor" class="form-control" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="btnBuscarCodigoProveedor" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" OnClick="btnBuscarCodigoProveedor_Click" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-2">Proveedor</label>
                                            <div class="col-md-3">
                                                <asp:DropDownList ID="ListProveedor" class="form-control" runat="server" AutoPostBack="True" onchange="javascript:return BuscarProveedor()" ></asp:DropDownList> <%--OnSelectedIndexChanged="ListProveedor_SelectedIndexChanged"--%>
                                            </div>
                                            <%--<div class="col-md-1">
                                                <asp:LinkButton ID="lbtnCargarArticulos" runat="server" Text="<span class='shortcut-icon icon-refresh'></span>" class="btn btn-info" OnClick="lbtnCargarArticulos_Click" />
                                            </div>--%>
                                            <%--<div class="col-md-4">
                                                <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                                                    <ProgressTemplate>
                                                        <i class="fa fa-spinner fa-spin"></i><span>&nbsp;&nbsp;Procesando cambios. Por favor aguarde.</span>
                                                    </ProgressTemplate>
                                                </asp:UpdateProgress>
                                            </div>--%>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-2">Numero</label>

                                            <div class="col-md-2">
                                                <asp:TextBox ID="txtPVenta" MaxLength="4" runat="server" class="form-control" disabled onchange="completar4Ceros(this, this.value)"></asp:TextBox>
                                            </div>

                                            <div class="col-md-3">
                                                <asp:TextBox ID="txtNumero" MaxLength="8" runat="server" class="form-control" disabled onchange="completar8Ceros(this, this.value)"></asp:TextBox>
                                            </div>
                                            <div class="col-md-1">
                                                <%-- <asp:RequiredFieldValidator ControlToValidate="txtPVenta" ID="RequiredFieldValidator30" runat="server" ErrorMessage="*" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                            </div>
                                            <div class="col-md-1">
                                                <%--<asp:RequiredFieldValidator ControlToValidate="txtNumero" ID="RequiredFieldValidator6" runat="server" ErrorMessage="*" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-2">Fecha</label>

                                            <div class="col-md-3">
                                                <div class="input-group">
                                                    <span class="input-group-addon"><i class="icon-calendar"></i></span>
                                                    <asp:TextBox ID="txtFecha" runat="server" class="form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:RequiredFieldValidator ControlToValidate="txtFecha" ID="RequiredFieldValidator42" runat="server" ErrorMessage="*" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-2">Fecha Entrega</label>

                                            <div class="col-md-3">
                                                <div class="input-group">
                                                    <span class="input-group-addon"><i class="icon-calendar"></i></span>
                                                    <asp:TextBox ID="txtFechaEntrega" runat="server" class="form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:RequiredFieldValidator ControlToValidate="txtFechaEntrega" ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-2">Observaciones</label>
                                            <div class="col-md-6">
                                                <asp:TextBox ID="txtObservaciones" runat="server" class="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                            </div>
                                        </div>
                                        <asp:Panel ID="Panel1" Visible="true" runat="server" class="col-md-12" Style="padding: 0px; margin-left: -1%;">
                                            <table class="table table-bordered ">
                                                <thead>
                                                    <tr>
                                                        <th>Codigo</th>
                                                        <th>Descripcion</th>
                                                        <th>Costo</th>
                                                        <th>Cantidad</th>
                                                        <th></th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr>
                                                        <td style="width: 20%">
                                                            <div class="form-group">
                                                                <div class="col-md-8">
                                                                    <div class="input-group">
                                                                        <asp:TextBox ID="txtCodigo" runat="server" class="form-control"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <%--<div class="col-md-2">
                                                                    <asp:LinkButton ID="lbtnBuscarArticulo" runat="server" Text="<span class='shortcut-icon icon-search'></span>" data-toggle="modal" class="btn btn-info" href="#modalBuscarArticuloDescripcion" />
                                                                </div>--%>
                                                            </div>
                                                        </td>
                                                        <td style="width: 30%">
                                                            <div style="width: 100%">
                                                                <asp:TextBox ID="txtDescripcion" runat="server" class="form-control" Style="text-align: right"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td style="width: 10%">
                                                            <asp:TextBox ID="txtPrecio" runat="server" class="form-control" Style="text-align: right" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 10%">
                                                            <asp:TextBox ID="txtCantidad" runat="server" class="form-control" Style="text-align: right" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 5%">
                                                            <asp:LinkButton ID="lbtnAgregarArticuloASP" class="btn btn-info" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" Visible="true" OnClick="lbtnAgregarArticuloASP_Click" />
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </asp:Panel>
                                    </fieldset>
                                </div>
                                <div id="validation-form2" role="form" class="form-horizontal col-md-4">
                                    <fieldset>
                                        <div>
                                            <div class="form-group alert alert-info">
                                                <label class="col-md-5">Mail:</label>
                                                <div class="col-md-7">
                                                    <asp:Label ID="lblMailOC" Text="" runat="server" />
                                                </div>
                                            </div>
                                            <div class="form-group alert alert-info">
                                                <label class="col-md-5">Requiere Autorización:</label>
                                                <div class="col-md-7">
                                                    <asp:Label ID="lblRequiereAutorizacionOC" Text="" runat="server" />
                                                </div>
                                            </div>
                                            <div class="form-group alert alert-info">
                                                <label class="col-md-5">Monto Autorización:</label>
                                                <div class="col-md-7">
                                                    <asp:Label ID="lblMontoAutorizacionOC" Text="" runat="server" />
                                                </div>
                                            </div>
                                            <div class="form-group alert alert-info">
                                                <label class="col-md-5">Requiere Anticipo:</label>
                                                <div class="col-md-7">
                                                    <asp:Label ID="lblRequiereAnticipoOC" Text="" runat="server" />
                                                </div>
                                            </div>
                                            <div class="form-group alert alert-info">
                                                <label class="col-md-5">Observaciones:</label>
                                                <div class="col-md-7">
                                                    <asp:Label ID="lblObservacion" Text="" runat="server" />
                                                </div>
                                            </div>
                                            <div class="form-group alert alert-info">
                                                <label class="col-md-5">Forma de pago:</label>
                                                <div class="col-md-7">
                                                    <asp:TextBox ID="txtFormaDePago" TextMode="MultiLine" Rows="4" runat="server" class="form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </fieldset>
                                </div>

                                <div id="validation-form3" role="form" class="form-horizontal col-md-8">
                                    <div class="btn-toolbar">
                                        <div class="btn-group">
                                            <asp:Button ID="btnVerStockMinimo" type="button" runat="server" Text="Stock Minimo" class="btn btn-info" OnClick="btnVerStockMinimo_Click" />
                                        </div>
                                        <div class="btn-group">
                                            <asp:Button ID="btnVerStockMinimoSucursal" type="button" runat="server" Text="Stock Minimo Sucursal" class="btn btn-info" OnClick="btnVerStockMinimoSucursal_Click" />
                                        </div>
                                        <div class="btn-group">
                                            <asp:Button ID="btnVerOC" type="button" runat="server" Text="Ver OC" class="btn btn-info" OnClick="btnVerOC_Click" />
                                            <asp:Label ID="lblVerCargados" Text="0" runat="server" Visible="false" />
                                        </div>
                                        <div class="btn-group">
                                            <asp:Button ID="btnVerTodos" type="button" runat="server" Text="Ver Todos" class="btn btn-info" OnClick="btnVerTodos_Click" Visible="true" />
                                        </div>
                                    </div>
                                    <br />
                                </div>

                                <table class="table table-striped table-bordered" id="articulosTablaProveedor">
                                    <thead>
                                        <tr>
                                            <th style="width: 20%">Codigo</th>
                                            <th style="width: 20%">Descripcion</th>
                                            <th style="width: 5%">Precio</th>
                                            <th style="width: 10%">Precio Mas IVA</th>
                                            <th style="width: 5%">Cantidad</th>
                                            <th style="width: 10%">Stock Sucursal</th>
                                            <th style="width: 10%">Stock Total</th>
                                            <th style="width: 10%">Stock Minimo</th>
                                            <th></th>
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
                                    <%--<div class="alert alert-info alert-dismissable col-md-2">
                                        <asp:Label ID="lblCartelTotal" runat="server" Text="asd"/>
                                    </div>--%>
                                </div>
                            </div>
                        </div>
                    </div>
<%--                </ContentTemplate>
            </asp:UpdatePanel>--%>
        </div>

        <div id="modalBuscarArticuloDescripcion" class="modal fade" tabindex="-1" role="dialog">
            <asp:UpdatePanel ID="UpdatePanel7" ChildrenAsTriggers="true" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="Panel2" runat="server" DefaultButton="btnBuscarArticuloDescripcion">
                        <div class="modal-dialog" style="width: 60%;">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                                    <h4 class="modal-title">Busqueda de Articulos</h4>
                                </div>
                                <div class="modal-body">
                                    <div role="form" class="form-horizontal col-md-12">
                                        <div class="form-group">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Buscar Articulo</label>
                                                    <div class="col-md-3">
                                                        <asp:TextBox ID="txtDescripcionArticulo" class="form-control" runat="server"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-1">
                                                        <asp:LinkButton ID="btnBuscarArticuloDescripcion" ClientIDMode="AutoID" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" OnClick="btnBuscarArticuloDescripcion_Click" />
                                                    </div>
                                                    <div class="col-md-3">
                                                        <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanel7">
                                                            <ProgressTemplate>
                                                                <i class="fa fa-spinner fa-spin"></i><span>&nbsp;&nbsp;Procesando cambios. Por favor aguarde.</span>
                                                            </ProgressTemplate>
                                                        </asp:UpdateProgress>
                                                    </div>
                                                    <%--<div class="col-md-3">
                                                        <a class="btn btn-info" onclick="BuscarArticulo();">
                                                            <i class="shortcut-icon icon-refresh"></i>
                                                        </a>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <a class="btn btn-info" onclick="LimpiarTabla();">
                                                            <i class="shortcut-icon icon-trash"></i>
                                                        </a>
                                                    </div>--%>
                                                </div>
                                            </div>
                                        </div>
                                        <table class="table table-striped table-bordered" id="articulosTabla">
                                            <thead>
                                                <tr>
                                                    <th style="width: 10%">Codigo</th>
                                                    <th style="width: 30%">Descripcion</th>
                                                    <th style="width: 10%">Costo</th>
                                                    <th style="width: 10%">Precio de Venta</th>
                                                    <th style="width: 10%"></th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:PlaceHolder ID="phBuscarArticulo" runat="server"></asp:PlaceHolder>
                                            </tbody>
                                        </table>
                                    </div>
                                    <div class="modal-footer">
                                        <asp:LinkButton ID="lbtnAgregarArticulosBuscadosATablaItems" runat="server" Text="Guardar" class="btn btn-success" OnClick="lbtnAgregarArticulosBuscadosATablaItems_Click" />
                                        <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                        </div>
                    </asp:Panel>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnBuscarArticuloDescripcion" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>

        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Visible="false" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="80%">
        </rsweb:ReportViewer>

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
        <script src="OrdenesCompra.js"></script>

        <!-- Page-Level Plugin Scripts - Tables -->
        <script src="//cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>
        <%--<script src="prueba.js" type="text/javascript"></script>--%>
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
            $(function () {
                $("#<%= txtFechaEntrega.ClientID %>").datepicker(
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
                    $("#<%= txtFecha.ClientID %>").datepicker(
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
                    $("#<%= txtFechaEntrega.ClientID %>").datepicker(
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

        <script type="text/javascript">

            function BuscarProveedor()
            {
                var idProveedor = document.getElementById('<%=this.ListProveedor.ClientID%>').value;
                var idSucursal = document.getElementById('<%=this.ListSucursal.ClientID%>').selectedOptions[0].value;

                LimpiarCamposDatosProveedor();
                ObtenerAlertaProveedor(idProveedor);
                CargarDatosProveedor(idProveedor);
                CargarArticulosProveedor(idProveedor, idSucursal);
            }

            function CargarDatosProveedor(idProveedor)
            {
                $.ajax({
                    type: "POST",
                    url: 'OrdenesCompraABM.aspx/CargarProveedor_OC',
                    data: JSON.stringify(
                        {
                            'idProveedor': idProveedor
                        }
                    ),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: OnSuccessCargarDatosProveedor,
                    error: function (e)
                    {
                        $.msgbox("No se pudieron cargar los datos del proveedor correctamente!", { type: "error" });
                        LimpiarCamposDatosProveedor();
                    }
                });
            }

            function CargarArticulosProveedor(idProveedor,idSucursal)
            {
                $.ajax({
                    type: "POST",
                    url: 'OrdenesCompraABM.aspx/ObtenerArticulosProveedor',
                    data: '{idProveedor: "' + idProveedor + '", idSucursal: "' + idSucursal + '"}',
                    data: JSON.stringify(
                        {
                            'idProveedor': idProveedor,
                            'idSucursal': idSucursal
                        }                        
                    ),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: OnSuccessCargarArticulosProveedor,
                    error: function (e)
                    {
                        $.msgbox("No se pudieron cargar los articulos del proveedor correctamente!", { type: "error" });
                    }
                });
            }

            function OnSuccessCargarDatosProveedor(response)
            {
                var data = response.d;
                obj = JSON.parse(data);

                var mailProveedor = document.getElementById('<%=this.lblMailOC.ClientID%>');
                var requiereAnticipo = document.getElementById('<%=this.lblRequiereAnticipoOC.ClientID%>');
                var requiereAutorizacion = document.getElementById('<%=this.lblRequiereAutorizacionOC.ClientID%>');
                var montoAutorizacion = document.getElementById('<%=this.lblMontoAutorizacionOC.ClientID%>');
                var observacionProveedor = document.getElementById('<%=this.lblObservacion.ClientID%>');
                var formaDePago = document.getElementById('<%=this.txtFormaDePago.ClientID%>');

                if (obj.mail != null)
                    mailProveedor.innerHTML = obj.mail;
                if (obj.requiereAnticipo != null)
                    requiereAnticipo.innerHTML = "Si";
                if (obj.requiereAutorizacion != null)
                    requiereAutorizacion.innerHTML = "Si";
                if (obj.montoAutorizacion != null)
                    montoAutorizacion.innerHTML = "$" + obj.montoAutorizacion;
                if (obj.observaciones != null)
                    observacionProveedor.innerHTML = obj.observaciones;
                if (obj.formaDePago != null)
                    formaDePago.value = obj.formaDePago;

                if (obj.requiereAnticipo == "0")
                    requiereAnticipo.innerHTML = "No";
                if (obj.requiereAutorizacion == "0")
                    requiereAutorizacion.innerHTML = "No";
            }

            function LimpiarCamposDatosProveedor()
            {
                var mailProveedor = document.getElementById('<%=this.lblMailOC.ClientID%>');
                var requiereAnticipo = document.getElementById('<%=this.lblRequiereAnticipoOC.ClientID%>');
                var requiereAutorizacion = document.getElementById('<%=this.lblRequiereAutorizacionOC.ClientID%>');
                var montoAutorizacion = document.getElementById('<%=this.lblMontoAutorizacionOC.ClientID%>');
                var observacionProveedor = document.getElementById('<%=this.lblObservacion.ClientID%>');
                var formaDePago = document.getElementById('<%=this.txtFormaDePago.ClientID%>');

                mailProveedor.innerHTML = "";
                requiereAnticipo.innerHTML = "";
                requiereAutorizacion.innerHTML = "";
                montoAutorizacion.innerHTML = "";
                observacionProveedor.innerHTML = "";
                formaDePago.value = "";
            }

            function OnSuccessCargarArticulosProveedor(response)
            {
                var data = response.d;
                obj = JSON.parse(data);

                $("#articulosTablaProveedor").dataTable().fnDestroy();
                $('#articulosTablaProveedor').find("tr:gt(0)").remove();

                for (var i = 0; i < obj.length; i++)
                {
                    $('#articulosTablaProveedor').append(
                        "<tr> " +
                        "<td> " + obj[i].codigo + "</td>" +
                        "<td> " + obj[i].descripcion + "</td>" +
                        "<td><input name=\"txtPrecio\" type=\"string\" value=" + obj[i].precioSinIva.toFixed(2) + " style=\"text-align: right;\"></td>" +
                        "<td style=\"text-align: right;\"> " + obj[i].precioventa.toFixed(2) + "</td>" +
                        "<td><input name=\"txtCantidad\" type=\"number\" value=\"0.00\" style=\"text-align: right;\"></td>" +
                        "<td style=\"text-align: right;\"> " + obj[i].StockSucursal.toFixed(2) + "</td>" +
                        "<td style=\"text-align: right;\"> " + obj[i].StockTotal.toFixed(2) + "</td>" +
                        "<td style=\"text-align: right;\"> " + obj[i].stockMinimo.toFixed(2) + "</td>" +                        
                        "<td> " + CrearAlerta(obj[i].stockMinimo,obj[i].StockTotal) + "</td>" +
                        "</tr> ");
                };
            }

            function CrearAlerta(stockMinimo,stockTotal)
            {
                var alertaTemp = "";

                if (parseFloat(stockMinimo.toFixed(2)) > parseFloat(stockTotal.toFixed(2)))
                    alertaTemp = "<span><span><i class=\"fa fa-exclamation-triangle text-danger\"></i>";

                return alertaTemp;
            }
            <%--function BuscarArticulo()
            {
                var textbox = document.getElementById('<%=this.txtDescripcionArticulo.ClientID%>').value;
                var json = null;

                $.ajax({
                    type: "POST",
                    url: 'OrdenesCompraABM.aspx/ObtenerDatosArticuloYDibujarlosEnPantalla',
                    data: JSON.stringify(
                        {
                            'txtDescripcion': textbox
                        }
                    ),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data)
                    {
                        json = JSON.parse(data.d);
                        addHtmlTableRow(json);
                    },
                    error: function (e)
                    {
                        $("#divResult").html("Something Wrong.");
                    }
                });
            }

            function addHtmlTableRow(data)
            {
                var table = document.getElementById("articulosTabla");
                //var temp = table.outerHTML;
                LimpiarTabla();
                for (var i = 0; i <= data.length - 1; i++)
                {
                    newRow = table.insertRow(table.length);

                    cell1 = newRow.insertCell(0),
                    cell2 = newRow.insertCell(1),
                    cell3 = newRow.insertCell(2),
                    cell4 = newRow.insertCell(3),
                    cell5 = newRow.insertCell(4);

                    //table.rows[i + 1].id = data[i].id;
                    cell1.innerHTML = data[i].codigo;
                    cell2.innerHTML = data[i].descripcion;
                    cell3.innerHTML = data[i].costo;
                    cell4.innerHTML = data[i].precioVenta;
                    cell5.innerHTML = "<asp:LinkButton ID=\"btnBuscarArticuloDescripcion_" + i + "Text=\" < span class='shortcut-icon icon-search' ></span > \" class=\"btn btn - info\"/>;"
                }
            }--%>
            function LimpiarTabla()
            {
                $("#articulosTabla").find("tr:gt(0)").remove();
            }
        </script>
</asp:Content>
