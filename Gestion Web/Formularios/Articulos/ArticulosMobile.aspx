<%@ Page EnableEventValidation="false" Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ArticulosMobile.aspx.cs" Inherits="Gestion_Web.Formularios.Articulos.ArticulosMobile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="col-md-12 col-xs-12">
        <div class="widget stacked">
            <div class="stat"> 
                <h5><i class="icon-map-marker"></i>Maestro > Articulos > Articulos</h5>
            </div>
            <div class="widget-header">
                <i class="icon-wrench"></i>
                <h3>Herramientas</h3>
            </div>
            <!-- /widget-header -->

            <div class="widget-content">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 100%">
                        <%-- <div class="row">--%>
                            <div class="col-md-12">
                                <div class="input-group">
                                    <span class="input-group-btn">
                                        <asp:LinkButton ID="lbBuscar" runat="server" Text="Buscar" class="btn btn-primary" OnClick="btnBuscar_Click">
                                                    <i class="shortcut-icon icon-search"></i></asp:LinkButton>
                                    </span>
                                    <asp:TextBox ID="txtBusqueda" runat="server" style="max-width:100% !important" class="form-control" placeholder="Buscar Articulo"></asp:TextBox>

                                </div>
                                <!-- /input-group -->
                            </div>
                         <%-- </div>--%>
                        </td>
                    </tr>
                </table>
            </div>
            <!-- /widget-content -->

        </div>
        <!-- /widget -->
    </div>
    <div class="col-md-12 col-xs-12">
        <div class="widget stacked widget-table action-table">

            <div class="widget-header">
                <i class="icon-bookmark"></i>
                <h3>Articulos</h3>
            </div>
            <div class="widget-content">
                <div class="panel-body">

                    <%--<div class="col-md-12 col-xs-12">--%>
                    <div class="table-responsive">
                        <table class="table table-striped table-bordered table-hover" id="dataTables-example">
                            <thead>
                                <tr>
                                    <th runat="server" style="width: 5%">Codigo</th>
                                    <th runat="server" style="width: 35%">Descripcion</th>
                            
                                    <asp:PlaceHolder ID="phColumna7" Visible="false" runat="server">
                                       <%-- <th runat="server" style="width: 10%">Presentacion</th>--%>
                                    </asp:PlaceHolder>
                                    <asp:PlaceHolder ID="phColumna8" Visible="false" runat="server">
                                       <%-- <th runat="server" style="width: 10%">Stock</th>--%>
                                    </asp:PlaceHolder>
                                    <asp:PlaceHolder ID="phColumna9" Visible="false" runat="server">
                                       <%-- <th runat="server" id="thMarca" style="width: 5%">Marca</th>--%>
                                    </asp:PlaceHolder>
                                    <asp:PlaceHolder ID="phColumna4" Visible="false" runat="server">
                                       <%-- <th runat="server" style="width: 5%">Moneda</th>--%>
                                    </asp:PlaceHolder>
                                
                                    <asp:PlaceHolder ID="phColumna10" Visible="false" runat="server">
                                        <th runat="server" id="thPrecioVentaMonedaOriginal" style="width: 5%">P. Venta</th>
                                    </asp:PlaceHolder>
                                    <asp:PlaceHolder ID="phColumna6" runat="server">
                                        <th runat="server" id="headerPrecio" style="width: 5%">P.Venta en ($)</th>
                                    </asp:PlaceHolder>
                                   

                                </tr>
                            </thead>
                            <tbody>
                                <asp:PlaceHolder ID="phArticulos" runat="server"></asp:PlaceHolder>
                            </tbody>
                        </table>
                    </div>
               

                </div>


                <!-- /.content -->

            </div>

        </div>
    </div>
    <!-- /container -->


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

    <!-- Page-Level Plugin Scripts - Tables -->
    <script src="//cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>
    <script src="//cdn.datatables.net/plug-ins/1.10.9/sorting/date-eu.js"></script>
    <script src="../../Scripts/plugins/dataTables/custom.tables.js"></script>

    <link href="//cdn.datatables.net/1.10.2/css/jquery.dataTables.css" rel="stylesheet" />

    <script>
        function mostrarMensaje(Boton) {
            $.msgbox("Are you sure that you want to permanently delete the selected element?", {
                type: "confirm",
                buttons: [
                    { type: "submit", value: "Yes" },
                    { type: "submit", value: "No" },
                    { type: "cancel", value: "Cancel" }
                ]
            }, function (result) {
                if (result == 'Yes') {
                    //ejecuto postback
                    __doPostBack('ctl00$MainContent$btnEliminar_2903', '');
                }
            });
        }

    </script>

    <script>
        $(document).ready(function () {

                $('#dataTables-example').dataTable({
                    "bLengthChange": false,
                    "bFilter": false,
                    "bInfo": false,
                    "bAutoWidth": false,
                    "bStateSave": true
                });
           
        });
    </script>


    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endReq);
        function endReq(sender, args) {
            $('#dataTables-example').dataTable();
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
                if (key == 45 || key == 46 || key == 8 || key == 44)// Detectar . (punto) y backspace (retroceso) y , (coma)
                { return true; }
                else { return false; }
            }
            return true;
        }
    </script>
    <script type="text/javascript">
        function MensajeArchivoDescargado() {
            $.msgGrowl({
                type: 'success'
                , title: 'Descargado'
                , text: 'El archivo se encuentra en la carpeta ArchivoExportacion/Salida.'
            });
        }
    </script>

 

</asp:Content>
