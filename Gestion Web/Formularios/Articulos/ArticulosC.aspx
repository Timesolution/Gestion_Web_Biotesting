<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ArticulosC.aspx.cs" Inherits="Gestion_Web.Formularios.Articulos.ArticulosC" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="col-md-12 col-xs-12">

        <div class="row" style="margin-left: 0 !important">
            <div class="col-md-6 col-xs-6">
                <asp:PlaceHolder ID="PlaceHolder1" runat="server" Visible="true">
                    <div class="widget stacked">
                        <div class="widget-header">
                            <i class="icon-wrench"></i>
                            <h3>Grupos</h3>
                        </div>
                        <div class="widget-content">
                            <div class="btn-toolbar">
                                <asp:PlaceHolder ID="phBotonesGrupos" runat="server"></asp:PlaceHolder>
                            </div>
                        </div>
                    </div>
                </asp:PlaceHolder>
            </div>

            <div class="col-md-6 col-xs-6">
                <asp:PlaceHolder ID="PlaceHolder2" runat="server" Visible="true">
                    <div class="widget stacked">
                        <div class="widget-header">
                            <i class="icon-wrench"></i>
                            <h3>Subgrupos</h3>
                        </div>
                        <div class="widget-content">
                            <div class="btn-toolbar">
                                <asp:PlaceHolder ID="phBotonesSubGrupos" runat="server"></asp:PlaceHolder>
                            </div>
                        </div>
                    </div>
                </asp:PlaceHolder>
            </div>
        </div>


        <div class="row">
            <div style="padding-left: 2%" class="col-md-8 col-xs-8">
                <div class="widget stacked widget-table action-table">
                    <div class="widget-header" style="padding-bottom: 6%; padding-right: 1%">
                        <i class="icon-bookmark"></i>
                        <h3>Articulos</h3>
                        <%--  <asp:LinkButton ID="lbBuscar" runat="server" Text="Buscar" class="btn btn-primary" Style="float: right; margin: 1% 1% 0% 1%" OnClick="btnBuscar_Click">
                                                    <i class="shortcut-icon icon-search"></i></asp:LinkButton>--%>

                        <a class="btn btn-primary ui-tooltip" data-toggle="modal" title data-original-title="Filtrar Articulos" href="#modalBusqueda" style="float: right; margin: 1% 1% 0% 1%">
                            <i style="margin-right: 0px;color:white;margin-left: 0px" class="shortcut-icon icon-filter"></i>
                        </a>
                        <a class="btn btn-primary ui-tooltip" data-toggle="modal" title data-original-title="Buscar Articulos" href="#modalBusqueda2" style="float: right; margin: 1% 1% 0% 1%">
                            <i style="margin-right: 0px;color:white;  margin-left: 0px" class="shortcut-icon icon-search"></i>
                        </a>
                    </div>
                    <div class="widget-content">


                        <div class="panel-body">
                            <div class="table-responsive">
                                <table class="table table-striped table-bordered table-hover" id="dataTables-example">
                                    <thead>
                                        <tr>
                                            <th style="width: 10%"></th>
                                            <th style="width: 10%">Codigo</th>
                                            <th style="width: 30%">Descripcion</th>
                                            <th style="width: 5%">P.Venta</th>
                                            <th style="width: 15%">Cantidad</th>

                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:PlaceHolder ID="phArticulos" runat="server"></asp:PlaceHolder>
                                        <asp:PlaceHolder ID="phCarro" runat="server"></asp:PlaceHolder>
                                    </tbody>
                                </table>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-4 col-xs-4 sticky" style="position: sticky;
    top: 7%;">
                <div class="widget stacked widget-table action-table">
                    <div class="widget-header" style="padding-bottom: 12%">
                        <i class="icon-shopping-cart"></i>
                        <h3>Carrito</h3>
                    </div>
                    <div class="widget-content">
                        <div class="panel-body">
                            <div class="table-responsive">
                                <table class="table table-striped table-bordered table-hover" id="dataTables-example2">
                                    <thead>
                                        <tr>
                                            <th style="width: 5%"></th>
                                            <th style="width: 10%">Codigo</th>
                                            <th style="width: 5%">P.Venta</th>
                                            <th style="width: 20%">Cantidad</th>

                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:PlaceHolder ID="phArticulosCarrito" runat="server"></asp:PlaceHolder>
                                        <asp:PlaceHolder ID="phCarroCargado" runat="server"></asp:PlaceHolder>
                                    </tbody>
                                </table>
                            </div>
                            <asp:LinkButton ID="lbtnGenerarPedido" Text="Generar pedido" runat="server" class="btn btn-success" Style="font-size: 0.9em; margin-top: 20%" OnClick="lbtnGenerarPedido_Click" />
                            <asp:LinkButton ID="lbtnGenerarPedidoBorrador" Text="Generar pedido borrador" runat="server" Style="font-size: 0.9em; margin-top: 20%" class="btn btn-success" OnClick="lbtnGenerarPedidoBorrador_Click" />
                            <%--<asp:LinkButton ID="lbtnVerPedido" Text="Ver pedido" runat="server" class="btn btn-default" OnClick="lbtnVerPedido_Click" />--%>
                            <asp:LinkButton ID="lbtnContinuarPedido" Text="Continuar pedido" runat="server" class="btn btn-default" OnClick="lbtnContinuarPedido_Click" Visible="false" />
                            <a class="btn btn-default" data-toggle="modal" data-original-title="Observaciones" href="#modalAgregarComentariosAlPedido" style="font-size: 0.9em; margin-top: 20%">Observaciones</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>

    <div id="modalConfirmacion" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Confirmacion de pedido</h4>
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
                                    <asp:Label runat="server" ID="lblMensaje" Text="Confirmar pedido" Style="text-align: center"></asp:Label>
                                </h5>
                            </div>

                            <div class="col-md-3">
                                <asp:TextBox runat="server" ID="txtMovimiento" Text="0" Style="display: none"></asp:TextBox>
                            </div>
                        </div>

                    </div>


                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                    </div>
                </div>

            </div>
        </div>
    </div>
    <%--Fin modalGrupo--%>

    <div id="modalPedidoBorrador" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Confirmacion de pedido</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <div class="form-group">

                            <asp:Label runat="server" ID="LabelCliente" Text="Cliente"></asp:Label>

                            <asp:DropDownList runat="server" ID="ListClientes" class="form-control"></asp:DropDownList>
                        </div>

                    </div>


                    <div class="modal-footer">
                        <asp:LinkButton ID="lbtnGenerarPedidoModalBorrador" Text="Generar pedido" runat="server" class="btn btn-success" OnClick="lbtnGenerarPedidoModalBorrador_Click" />
                        <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                    </div>
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
                    <h4 class="modal-title">Filtrar Articulos</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <asp:UpdatePanel ID="UpdatePanel4" UpdateMode="Always" runat="server">
                            <ContentTemplate>
                                <div class="form-group">
                                    <label class="col-md-4">Linea</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="ListGrupo" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ListGrupo_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Rubro</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="ListSubGrupo" runat="server" class="form-control"></asp:DropDownList>
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="modal-footer">
                        <asp:LinkButton ID="btnFiltrar" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnFiltrar_Click" />
                    </div>
                </div>

            </div>
        </div>
    </div>
    <div id="modalBusqueda2" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Buscar Articulos</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Always" runat="server">
                            <ContentTemplate>
                                <div class="form-group">
                                    <label class="col-md-4">Articulo</label>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtBusqueda" runat="server" class="form-control" placeholder="Buscar Articulo"></asp:TextBox>
                                    </div>
                                </div>

                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <div class="modal-footer">
                        <asp:LinkButton ID="btnBuscar" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnBuscar_Click" />
                    </div>
                </div>

            </div>
        </div>
    </div>


    <div id="modalAgregarComentariosAlPedido" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Comentarios</h4>
                </div>
                <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Always" runat="server">
                    <ContentTemplate>
                        <div class="modal-body">
                            <div role="form" class="form-horizontal col-md-12">

                                <div class="form-group">
                                    <div class="col-md-12">
                                        <asp:TextBox runat="server" ID="txtComentarios" class="form-control" TextMode="MultiLine" Rows="5" cols="20"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:LinkButton ID="lbtnGuardarComentarios" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnGuardarComentarios_Click" />
                            </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
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
        function abrirdialog(valor) {
            document.getElementById('<%= txtMovimiento.ClientID %>').value = valor;
        }
    </script>

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
        function showModalPedidoBorrador() {
            $("#modalPedidoBorrador").modal('show');
        }
    </script>

    <script>
        $(document).ready(function () {
            $('#dataTables-example').dataTable({
                "bLengthChange": false,
                "bFilter": false,
                "bInfo": false,
                "bAutoWidth": false,
                "bStateSave": true,
                "pageLength": 50
                //"columnDefs": [
                //    { type: 'date-eu', targets: 5 }
                //]
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
                if (key == 46 || key == 8 || key == 44)// Detectar . (punto) y backspace (retroceso) y , (coma)
                { return true; }
                else { return false; }
            }
            return true;
        }
    </script>
</asp:Content>
