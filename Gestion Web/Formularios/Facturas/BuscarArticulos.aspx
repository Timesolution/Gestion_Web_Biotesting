<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BuscarArticulos.aspx.cs" Inherits="Gestion_Web.Formularios.Facturas.BuscarArticulos" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <%--    <style type="text/css">
        .style1
        {
            width: 69px;
        }
        .style2
        {
            width: 155px;
        }
        .style3
        {
            width: 303px;
        }
        .style4
        {
            width: 68px;
        }
    </style>--%>


    <link href="../../css/bootstrap.min.css" rel="stylesheet" />
    <link href="./css/bootstrap-responsive.min.css" rel="stylesheet" />

    <link href="https://fonts.googleapis.com/css?family=Open+Sans:400italic,600italic,400,600" rel="stylesheet" />
    <link href="../../css/font-awesome.min.css" rel="stylesheet" />

    <link href="../../css/ui-lightness/jquery-ui-1.10.0.custom.min.css" rel="stylesheet" />
    <link href="../../css/ui-lightness/jquery-ui-1.10.0.custom.css" rel="stylesheet" />

    <link href="../../css/base-admin-3.css" rel="stylesheet" />
    <link href="../../css/base-admin-3.css" rel="stylesheet" />
    <link href="../../css/base-admin-3-responsive.css" rel="stylesheet" />

    <link href="../../css/pages/plans.css" rel="stylesheet" />
    <link href="../../css/pages/dashboard.css" rel="stylesheet" />
    <link href="../../css/custom.css" rel="stylesheet" />
    <link href="../../css/pages/signin.css" rel="stylesheet" type="text/css" />

    <link href="../../Scripts/plugins/msgGrowl/css/msgGrowl.css" rel="stylesheet" />
    <link href="../../Scripts/plugins/lightbox/themes/evolution-dark/jquery.lightbox.css" rel="stylesheet" />
    <link href="../../Scripts/plugins/msgbox/jquery.msgbox.css" rel="stylesheet" />
</head>
<body>

    <form id="form1" runat="server" onkeypress='javascript:return validarEnter(event)'>
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>

            <div class="container">
                <div class="navbar navbar-inverse navbar-fixed-top hidden-print">
                    <div class="container">
                        <div class="navbar-header">
                            <%--<asp:LinkButton runat="server" ID="btnCerrar" class="navbar-toggle" OnClick="btnCerrar_Click" Text="<span class='icon-remove icon-white'></span>" enabled="false"/>--%>
                            <asp:Label CssClass="navbar-brand" runat="server" ID="LabelInicio" Text="Busqueda"></asp:Label>
                        </div>
                    </div>
                </div>
                <br />
                <br />

                <div class="col-md-12 col-xs-12 ">
                    <div class="widget stacked">
                        <div class="widget-header">
                            <i class="icon-wrench"></i>
                            <h3>Herramientas</h3>
                        </div>
                        <div class="widget-content">
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 80%">
                                        <div class="col-md-12">
                                            <div class="imput-group">
                                                <%--<asp:TextBox ID="txtBuscarArticulos" runat="server" class="form-control" Text=""></asp:TextBox>--%>
                                                <%--<input type="text" id="filter" placeholder="Buscar" class="form-control " onkeyup="doSearch()" style="margin:0 0 12px 55px;" />--%>
                                                <asp:TextBox ID="txtBuscarArticulos" runat="server" class="form-control" Text="" placeholder="Buscar"></asp:TextBox>
                                            </div>
                                        </div>
                                    </td>
                                    <td style="width: 20%">
                                        <span class="input-group-btn">
                                            <%--<button id="btnBuscarArticuloDescripcion" type="button" onclick="CargarArticulos()" class="btn btn-info"><span class='shortcut-icon icon-search'></span></button>--%>
                                            <asp:LinkButton ID="lbBuscarArticulos" runat="server" Text="Buscar" class="btn btn-primary" OnClick="btnBuscarArticulos_Click">
                                                    <i class="shortcut-icon icon-search"></i></asp:LinkButton>
                                        </span>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="widget widget-table">
                        
                        <table class="table table-striped table-bordered" id="articulosTabla">
                            <thead>
                                <tr>
                                    <th>Codigo</th>
                                    <th>Descripcion</th>
                                    <th>Stock</th>
                                    <th>Moneda</th>
                                    <th>Precio</th>
                                    <th></th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:PlaceHolder ID="phArticulos" runat="server"></asp:PlaceHolder>
                            </tbody>
                        </table>
                    </div>
                    <div class="widget-content">
                        <div style="text-align:right;">
                            <asp:LinkButton ID="lbtnAgregarArticulosMultiples" runat="server" Visible="false" Text="Agregar" class="btn btn-success" OnClick="lbtnAgregarArticulosMultiples_Click"></asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <br />
        <div class="row">
        </div>

    </form>
    <script src="../../Scripts/jquery-1.10.2.js"></script>
    <script src="../../Scripts/bootstrap.min.js"></script>

    <script src="//cdn.datatables.net/1.10.11/js/jquery.dataTables.min.js"></script>
    <script src="//cdn.datatables.net/plug-ins/1.10.9/sorting/date-eu.js"></script>
    <link href="//cdn.datatables.net/1.10.11/css/jquery.dataTables.min.css" rel="stylesheet" />

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
                if (key == 46 || key == 8 || key == 44 || key == 45) // Detectar . (punto) , backspace (retroceso) y , (coma)
                { return true; }
                else { return false; }
            }
            return true;
        }
    </script>

    <%--<script src="../../Scripts/libs/jquery-1.9.1.min.js"></script>
    <script src="../../Scripts/libs/jquery-ui-1.10.0.custom.min.js"></script>
    <script src="//cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>
    <script src="//cdn.datatables.net/plug-ins/1.10.9/sorting/date-eu.js"></script>
    <script src="../../Scripts/plugins/dataTables/custom.tables.js"></script>
    <link href="//cdn.datatables.net/1.10.2/css/jquery.dataTables.css" rel="stylesheet" />
    <script src="../../Scripts/plugins/lightbox/jquery.lightbox.min.js"></script>
    <script src="../../Scripts/plugins/msgbox/jquery.msgbox.min.js"></script>
    <script src="../../Scripts/plugins/dataTables/jquery.dataTables.js"></script>
    <script src="../../Scripts/plugins/dataTables/dataTables.bootstrap.js"></script>--%>

    <%--<script>
        $(document).ready(function () {
            $('#dataTables-example').dataTable();
        });
    </script>--%>

    <script type="text/javascript">
        //Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endReq);
        //function endReq(sender, args) {
        //    $('#dataTables-example').dataTable();
        //}

        function validarEnter(e) {
            var key;
            if (window.event) // IE
            {
                key = e.keyCode;
            }
            else if (e.which) // Netscape/Firefox/Opera
            {
                key = e.which;
            }

            if (key == 13) {
                var btnBuscarArticulo = document.getElementById("lbBuscarArticulos");
                btnBuscarArticulo.click();
                return false;
            }
            if (key == 14) {
                var btnBuscarArticulo2 = document.getElementById("lbBuscarArticulos");
                btnBuscarArticulo2.click();
                return false;
            }
            return true;
        }
    </script>

    <script>
        $(document).ready(function () {

            $('#articulosTabla').dataTable({
                "paging": true,
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

            $('.dataTables_filter').hide()
            $('#<%=txtBuscarArticulos.ClientID%>').on('keyup', function () {
                $('#articulosTabla').DataTable().search(
                    this.value
                ).draw();
            });
        })
    </script>
</body>
</html>


