<%@ Page EnableEventValidation = "false" Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OrdenesCompraABM.aspx.cs" Inherits="Gestion_Web.Formularios.Compras.OrdenesCompraABM" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="main">
        <div>
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
                                        <asp:DropDownList ID="ListSucursal" class="form-control" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="name" class="col-md-2">Pto Venta</label>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ListPtoVenta" class="form-control" runat="server"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="name" class="col-md-2">Cod. Proveedor</label>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtCodProveedor" class="form-control" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:LinkButton ID="btnBuscarCodigoProveedor" href="#" runat="server" Text="<span class='shortcut-icon icon-search'></span>" OnClientClick="BuscarProveedor()" class="btn btn-info" AutoPostBack="false" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="name" class="col-md-2">Proveedor</label>
                                    <div class="col-md-3">
                                        <asp:DropDownList ID="ListProveedor" class="form-control" runat="server" onchange="javascript:return SeleccionarProveedor()"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="name" class="col-md-2">Numero</label>

                                    <div class="col-md-2">
                                        <asp:TextBox ID="txtPVenta" MaxLength="4" runat="server" class="form-control" onkeydown="return validarNro(this)"></asp:TextBox>
                                    </div>

                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtNumero" MaxLength="8" runat="server" class="form-control" onkeydown="return validarNro(this)"></asp:TextBox>
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
                            </fieldset>
                        </div>
                        <div id="validation-form2" role="form" class="form-horizontal col-md-4">
                            <fieldset>
                                <div>
                                    <div class="form-group alert alert-info">
                                        <h3 style="text-align: center">
                                            <label>Datos del Proveedor</label>
                                        </h3>
                                    </div>
                                    <div class="form-group alert alert-info">
                                        <label class="col-md-5">Mail:</label>
                                        <div class="col-md-7">
                                            <asp:Label ID="lblMailOC" Text="" runat="server" />
                                            <asp:HiddenField id="hfLabelMailOC" runat="server"/>
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
                                    <%--<asp:Button ID="btnVerStockMinimo" type="button" runat="server" Text="Stock Minimo" class="btn btn-info" OnClientClick="FiltrarStockMinimo()" />--%> <%--OnClick="btnVerStockMinimo_Click"--%>
                                    <asp:LinkButton ID="lbtnVerStockMinimo" href="#" OnClientClick="FiltrarStockMinimo()" runat="server" Text="Stock Minimo" class="btn btn-info"/>
                                </div>
                                <div class="btn-group">
                                    <%--<asp:Button ID="btnVerOC" type="button" runat="server" Text="Ver OC" class="btn btn-info" OnClick="btnVerOC_Click" />
                                    <asp:Label ID="lblVerCargados" Text="0" runat="server" Visible="false" />--%>
                                    <asp:LinkButton ID="lbtnVerOC" href="#" OnClientClick="FiltrarVerOrdenCompra()" runat="server" Text="Ver OC" class="btn btn-info"/>
                                </div>
                                <div class="btn-group">
                                    <%--<asp:Button ID="btnVerTodos" type="button" runat="server" Text="Ver Todos" class="btn btn-info" OnClick="btnVerTodos_Click" Visible="true" />--%>
                                    <asp:LinkButton ID="lbtnVerTodos" href="#" OnClientClick="VerTodosLosArticulos()" runat="server" Text="Ver Todos" class="btn btn-info"/>
                                </div>
                            </div>
                            <br />
                        </div>

                        <table class="table table-striped table-bordered" id="articulosTablaProveedor">
                            <thead>
                                <tr>
                                    <th style="width: 10%">Codigo</th>
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
                                <asp:Panel runat="server" ID="panelProductos"></asp:Panel>
                            </tbody>
                        </table>
                        <br />
                        <div class="btn-toolbar">
                            <div class="btn-group">
                                <asp:Button ID="btnAgregar" runat="server" Text="Guardar" class="btn btn-success" OnClientClick="return AgregarOrdenCompra()" OnClick="lbtnAgregar_Click"/>
                                <%--<asp:LinkButton ID="lbtnAgregar" OnClientClick="AgregarOrdenCompra()" runat="server" Text="Guardar" class="btn btn-success" OnClick="lbtnAgregar_Click"/>--%>
                                <%--<asp:Label ID="lblCodigosOrdenCompra" runat="server" visible="true"></asp:Label>--%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Visible="false" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="80%">
        </rsweb:ReportViewer>

        <!-- Core Scripts - Include with every page -->
        <script src="../../Scripts/libs/jquery-1.9.1.min.js"></script>
        <script src="../../Scripts/libs/jquery-ui-1.10.0.custom.min.js"></script>
        <link href="../../css/pages/reports.css" rel="stylesheet">
        <script src="//cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>
        <script src="//cdn.datatables.net/plug-ins/1.10.9/sorting/date-eu.js"></script>
        <script src="../../Scripts/plugins/dataTables/custom.tables.js"></script>
        <link href="//cdn.datatables.net/1.10.2/css/jquery.dataTables.css" rel="stylesheet" />
        <script src="../../Scripts/plugins/lightbox/jquery.lightbox.min.js"></script>
        <script src="../../Scripts/plugins/msgbox/jquery.msgbox.min.js"></script>
        <script src="../../scripts/demo/notifications.js"></script>
        <script src="OrdenesCompra.js"></script>

        <%--<script src="../../Scripts/jquery-1.10.2.js"></script>
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
        <script src="//code.jquery.com/ui/1.10.4/jquery-ui.js"></script>--%>
        

        <!-- Page-Level Plugin Scripts - Tables -->
        <%--<script src="//cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>--%>
        <%--<script src="prueba.js" type="text/javascript"></script>--%>
        <%--<script src="../../Scripts/plugins/dataTables/custom.tables.js"></script>
        <link href="//cdn.datatables.net/1.10.2/css/jquery.dataTables.css" rel="stylesheet" />--%>

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

        <%--<script>
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
        </script>--%>

        <%--<script type="text/javascript">
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
        </script>--%>

        <script>
            $(function ()
            {
                var controlDropListSucursal = document.getElementById('<%= ListSucursal.ClientID %>');
                var controlDropListPuntoVenta = document.getElementById('<%= ListPtoVenta.ClientID %>');

                controlDropListSucursal.addEventListener("change", CargarPuntosVenta);
                controlDropListPuntoVenta.addEventListener("change", CargarNumeroOrden);
            });
        </script>

        <script type="text/javascript">

            function AgregarOrdenCompra()
            {
                if (!ComprobarDatosProveedor())
                {
                    $.msgbox("Los datos del proveedor no se encuentran cargados, por favor acceda al proveedor y carguelos para continuar.", { type: "alert" });
                    return false;
                }

                var controlDropListSucursal = document.getElementById('<%= ListSucursal.ClientID %>');

                var table = $('#articulosTablaProveedor').DataTable({ "paging": false, "bInfo": false, "searching": false, "retrieve": true,"ordering": false});

                var data = table.rows().data();

                var articulos = new Array();

                for (var i = 0; i < data.length; i++)
                {
                    var articulo = data[i];

                    var txtCantidad = document.getElementsByName("txtCantidad_" + articulo[0]);
                    var txtPrecio = document.getElementsByName("txtPrecio_" + articulo[0]);

                    if (parseInt(txtCantidad[0].value) > 0)
                    {
                        var articuloDatos = articulo[0].replace('&amp;','&') + ";" + articulo[1].replace('&amp;','&') + ";" + txtPrecio[0].value + ";" + articulo[3] + ";" + txtCantidad[0].value;
                        articulos.push(articuloDatos);
                    }
                }
                var controlDropListPuntoVenta = document.getElementById('<%= ListPtoVenta.ClientID %>');
                var articulosOrdenCompra = JSON.stringify(articulos);

                $.ajax({
                    type: "POST",
                    url: "OrdenesCompraABM.aspx/ObtenerArticulosParaGenerarOrdenCompra",
                    data: JSON.stringify({articulos: articulos}),
                    contentType: "application/json",
                    dataType: 'json',
                    error: function ()
                    {
                        $.msgbox("Error agregando orden de compra.", { type: "alert" });
                    }
                });

                return true;
            }

            function ComprobarDatosProveedor()
            {
                var mailProveedor = document.getElementById('<%=this.lblMailOC.ClientID%>');
                var requiereAnticipo = document.getElementById('<%=this.lblRequiereAnticipoOC.ClientID%>');
                var requiereAutorizacion = document.getElementById('<%=this.lblRequiereAutorizacionOC.ClientID%>');
                var montoAutorizacion = document.getElementById('<%=this.lblMontoAutorizacionOC.ClientID%>');
                var formaDePago = document.getElementById('<%=this.txtFormaDePago.ClientID%>');

                if (mailProveedor.innerHTML == "" ||
                    requiereAnticipo.innerHTML == "" ||
                    requiereAutorizacion.innerHTML == "" ||
                    montoAutorizacion.innerHTML == "" ||
                    formaDePago.value == "")
                    return false;

                return true;
            }

            function VerTodosLosArticulos()
            {
                var table = $('#articulosTablaProveedor').DataTable({ "paging": false, "bInfo": false, "searching": false, "retrieve": true,"ordering": false});

                var data = table.rows().data();

                for (var i = 0; i < data.length; i++)
                {
                    var codigo = data[i];
                    var row = document.getElementById("articulo_"+codigo[0]);
                    row.style.display = 'none';                    
                }

                for (var i = 0; i < data.length; i++)
                {
                    var codigo = data[i];
                    var row = document.getElementById("articulo_" + codigo[0]);
                    row.style.display = '';
                }
            }

            function FiltrarVerOrdenCompra()
            {
                var table = $('#articulosTablaProveedor').DataTable({ "paging": false, "bInfo": false, "searching": false, "retrieve": true,"ordering": false});

                var data = table.rows().data();

                for (var i = 0; i < data.length; i++)
                {
                    var codigo = data[i];
                    var row = document.getElementById("articulo_"+codigo[0]);
                    row.style.display = 'none';                    
                }

                for (var i = 0; i < data.length; i++)
                {
                    var codigo = data[i];

                    var txtCantidad = document.getElementsByName("txtCantidad_" + codigo[0]);
                    var row = document.getElementById("articulo_" + codigo[0]);

                    if (parseInt(txtCantidad[0].value) > 0)
                    {
                        row.style.display = '';
                    }
                    else
                    {
                        row.style.display = 'none';
                    }
                }
            };

            function FiltrarStockMinimo()
            {
                var table = $('#articulosTablaProveedor').DataTable({ "paging": false, "bInfo": false, "searching": false, "retrieve": true,"ordering": false});

                var data = table.rows().data();

                for (var i = 0; i < data.length; i++)
                {
                    var codigo = data[i];
                    var stockTotal = data[i];
                    var stockMinimo = data[i];
                    var row = document.getElementById("articulo_"+codigo[0]);
                    row.style.display = 'none';                    
                }

                for (var i = 0; i < data.length; i++)
                {
                    var codigo = data[i];
                    var stockTotal = data[i];
                    var stockMinimo = data[i];

                    if (stockMinimo[7] > stockTotal[6])
                    {
                        var row = document.getElementById("articulo_"+codigo[0]);
                        row.style.display = '';
                    }
                }
            };

            function CargarPuntosVenta()
            {
                var DropListSucursal = document.getElementById('<%= ListSucursal.ClientID %>');
                var listSucursalValue = DropListSucursal.options[DropListSucursal.selectedIndex].value;

                $.ajax({
                    type: "POST",
                    url: "OrdenesCompraABM.aspx/CargarPuntoVenta",
                    data: '{sucursal: "' + listSucursalValue + '"  }',
                    contentType: "application/json",
                    dataType: 'json',
                    error: function ()
                    {
                        $.msgbox("No se pudieron cargar los puntos de venta.", { type: "alert" });
                    },
                    success: OnSuccessPuntoVenta
                });
            };

            function OnSuccessPuntoVenta(response)
            {
                var controlDropListPuntoVenta = document.getElementById('<%= this.ListPtoVenta.ClientID %>');
                var idProveedor = document.getElementById('<%=this.ListProveedor.ClientID%>').value;
                var idSucursal = document.getElementById('<%=this.ListSucursal.ClientID%>').selectedOptions[0].value;

                while (controlDropListPuntoVenta.options.length > 0)
                {
                    controlDropListPuntoVenta.remove(0);
                }

                $("#articulosTablaProveedor").find("tr:gt(0)").remove();

                if (idSucursal > 0 && idSucursal != null)
                {
                    var data = response.d;
                    obj = JSON.parse(data);

                    for (i = 0; i < obj.length; i++)
                    {
                        option = document.createElement('option');
                        option.value = obj[i].id;
                        option.text = obj[i].nombreFantasia;

                        controlDropListPuntoVenta.add(option);
                    }

                    CargarNumeroOrden();

                    if (idSucursal > 0 && idSucursal != null && idProveedor > 0 && idProveedor != null)
                        CargarArticulosProveedor(idProveedor, idSucursal);
                }
            }

            function CargarNumeroOrden()
            {
                var DropListPuntoVenta = document.getElementById('<%= ListPtoVenta.ClientID %>').value;

                $.ajax({
                    type: "POST",
                    url: "OrdenesCompraABM.aspx/ObtenerNumeroOrden",
                    data: '{puntoVenta: "' + DropListPuntoVenta + '"  }',
                    contentType: "application/json",
                    dataType: 'json',
                    error: function ()
                    {
                        $.msgbox("No se pudieron obtener los numeros de orden.", { type: "alert" });
                    },
                    success: OnSuccessObtenerNumeroOrden
                });
            };

            function OnSuccessObtenerNumeroOrden(response)
            {
                var controlTextNumeroOrden = document.getElementById('<%= txtNumero.ClientID %>');
                var controlTextPuntoVenta = document.getElementById('<%= txtPVenta.ClientID %>');

                var data = response.d;
                obj = JSON.parse(data);
               
                controlTextNumeroOrden.value = obj.numero;
                controlTextPuntoVenta.value = obj.puntoVenta;
            }

            function BuscarProveedor()
            {
                var txtCodigoProveedor = document.getElementById('<%= txtCodProveedor.ClientID %>').value;

                $.ajax({
                    type: "POST",
                    url: "OrdenesCompraABM.aspx/BuscarProveedor",
                    data: '{codigoProveedor: "' + txtCodigoProveedor + '"  }',
                    contentType: "application/json",
                    dataType: 'json',
                    error: function ()
                    {
                        $.msgbox("No se pudo buscar el proveedor!", { type: "alert" });
                    },
                    success: OnSuccessBuscarProveedor
                });
            }

            function OnSuccessBuscarProveedor(response)
            {
                LimpiarCamposDatosProveedor();

                var controlDropListProveedor = document.getElementById('<%= ListProveedor.ClientID %>');

                while (controlDropListProveedor.options.length > 0)
                {
                    controlDropListProveedor.remove(0);
                }

                var data = response.d;
                obj = JSON.parse(data);

                for (i = 0; i < obj.length; i++)
                {
                    option = document.createElement('option');
                    option.value = obj[i].id;
                    option.text = obj[i].alias;

                    controlDropListProveedor.add(option);
                }

                SeleccionarProveedor();
            }

            function SeleccionarProveedor()
            {
                var idProveedor = document.getElementById('<%=this.ListProveedor.ClientID%>').value;
                var idSucursal = document.getElementById('<%=this.ListSucursal.ClientID%>').selectedOptions[0].value;

                $("#articulosTablaProveedor").find("tr:gt(0)").remove();

                LimpiarCamposDatosProveedor();

                if (idProveedor > 0 && idProveedor != null)
                {
                    ObtenerAlertaProveedor(idProveedor);
                    CargarDatosProveedor(idProveedor);

                    if(idSucursal > 0 && idSucursal != null)
                        CargarArticulosProveedor(idProveedor, idSucursal);
                    else
                        $.msgbox("Debe seleccionar una sucursal para cargar los articulos!", { type: "alert" });
                }
                else
                    $.msgbox("Debe seleccionar un proveedor!", { type: "alert" });
            }            

            function OnSuccessCargarDatosProveedor(response)
            {
                var data = response.d;
                obj = JSON.parse(data);

                var mailProveedor = document.getElementById('<%=this.lblMailOC.ClientID%>');
                var hiddenMail = document.getElementById('<%=this.hfLabelMailOC.ClientID%>');
                var requiereAnticipo = document.getElementById('<%=this.lblRequiereAnticipoOC.ClientID%>');
                var requiereAutorizacion = document.getElementById('<%=this.lblRequiereAutorizacionOC.ClientID%>');
                var montoAutorizacion = document.getElementById('<%=this.lblMontoAutorizacionOC.ClientID%>');
                var observacionProveedor = document.getElementById('<%=this.lblObservacion.ClientID%>');
                var formaDePago = document.getElementById('<%=this.txtFormaDePago.ClientID%>');

                if (obj.mail != null)
                {
                    hiddenMail.value = obj.mail;
                    mailProveedor.innerHTML = obj.mail;
                }
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
                $("#articulosTablaProveedor").find("tr:gt(0)").remove();

                for (var i = 0; i < obj.length; i++)
                {
                    $('#articulosTablaProveedor').append(
                        "<tr id='articulo_" + obj[i].codigo + "'> " +
                        "<td> " + obj[i].codigo + "</td>" +
                        "<td> " + obj[i].descripcion + "</td>" +
                        "<td><input name='txtPrecio_" + obj[i].codigo + "'type=\"string\" value=" + obj[i].precioSinIva.toFixed(2) + " style=\"text-align: right;\"></td>" +
                        "<td style=\"text-align: right;\"> " + obj[i].precioventa.toFixed(2) + "</td>" +
                        "<td><input name='txtCantidad_" + obj[i].codigo + "'type=\"number\" value=\"0.00\" style=\"text-align: right;\"></td>" +
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
        </script>

        <script>
            //valida los campos solo numeros
            function validarNro(e)
            {
                var key = event.keyCode || event.charCode;

                if (key == 8 || key == 46)
                    return false;

                //if (window.event) // IE
                //{
                //    key = e.keyCode;
                //}
                //else if (e.which) // Netscape/Firefox/Opera
                //{
                //    key = e.which;
                //}
                //if (key < 48 || key > 57) {
                //    if (key == 46 || key == 44)// Detectar . (punto) y backspace (retroceso) y , (coma)
                //    {
                //        return true;
                //    }
                //    else {
                //        return false;
                //    }
                //}
                return false;
            }
        </script>
</asp:Content>
