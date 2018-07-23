<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ImportacionesDetalleF.aspx.cs" Inherits="Gestion_Web.Formularios.Compras.ImportacionesDetalleF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">
        <asp:UpdatePanel runat="server" ID="UpdatePanel1">
            <ContentTemplate>
                <div class="col-md-12 col-xs-12">

                    <div class="widget stacked">
                        <div class="stat">
                            <h5><i class="icon-map-marker"></i>&nbsp Compras > Importacion > Detalle </h5>
                        </div>
                        <div class="widget-header">
                            &nbsp <i class="fa fa-import"></i>
                            <h3>Importacion</h3>
                        </div>
                        <div class="widget-content">
                            <div role="form" class="form-horizontal col-md-6">
                                <fieldset>
                                    <div class="form-group">
                                        <label for="name" class="col-md-2">Busqueda:</label>
                                        <div class="col-md-5">
                                            <div class="input-group">
                                                <asp:TextBox ID="txtBusqueda" runat="server" class="form-control" placeholder="Buscar Articulo"></asp:TextBox>
                                                <span class="input-group-btn">
                                                    <asp:LinkButton ID="lbBuscar" runat="server" Text="Buscar" class="btn btn-primary" ValidationGroup="BusquedaArticulo" OnClick="lbBuscar_Click">
					                                        <i class="shortcut-icon icon-search"></i></asp:LinkButton>
                                                </span>
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ErrorMessage="Ingrese un Codigo" ControlToValidate="txtBusqueda" ValidationGroup="BusquedaArticulo" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label for="name" class="col-md-2">Articulo:</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ListArticulosBusqueda" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ListArticulosBusqueda_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" InitialValue="-1" ErrorMessage="*" ControlToValidate="txtBusqueda" ValidationGroup="BusquedaArticulo" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label for="name" class="col-md-2">Cantidad:</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtCantidad" runat="server" class="form-control" Style="text-align: right;" onkeypress="javascript:return validarNro(event)" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label for="name" class="col-md-2">SIM:</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtSIM" runat="server" class="form-control" Style="text-align: right;" TextMode="MultiLine" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label for="name" class="col-md-2">FOB:</label>
                                        <div class="col-md-4">
                                            <div class="input-group">
                                                <span class="input-group-addon">
                                                    <asp:Label ID="lblMoneda" runat="server" Text="$" />
                                                </span>
                                                <asp:TextBox ID="txtFOB" runat="server" class="form-control" Style="text-align: right;" onkeypress="javascript:return validarNro(event)" />
                                            </div>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:LinkButton ID="lbtnAgregarArticulo" runat="server" class="btn btn-success" Text="<i class='shortcut-icon icon-ok'></i>" OnClick="lbtnAgregarArticulo_Click" />
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                            <div role="form" class="form-horizontal col-md-6">
                                <fieldset>
                                    <div class="widget-content">
                                        <div class="form-group">
                                            <label for="name" class="col-md-3">Stock Actual:</label>
                                            <div class="col-md-3">
                                                <asp:TextBox ID="txtStockActual" runat="server" Style="text-align: right;" class="form-control" disabled Text="0.00"></asp:TextBox>
                                            </div>
                                            <label for="name" class="col-md-3">PPP</label>
                                            <div class="col-md-3">
                                                <asp:TextBox ID="txtPPP" runat="server" Style="text-align: right;" class="form-control" disabled Text="0.00"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-3">Precio Compra:</label>
                                            <div class="col-md-3">
                                                <asp:TextBox ID="txtPrecioCompra" Style="text-align: right;" runat="server" class="form-control" disabled Text="0.00"></asp:TextBox>
                                            </div>
                                            <label for="name" class="col-md-3">Precio Vta:</label>
                                            <div class="col-md-3">
                                                <asp:TextBox ID="txtPrecioVenta" runat="server" Style="text-align: right;" class="form-control" disabled Text="0.00"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                        </div>
                        <br />
                        <div class="widget big-stats-container stacked">
                            <div class="widget-content">

                                <div id="big_stats" class="cf">
                                    <div class="stat">
                                        <h4>Total FOB</h4>
                                        <asp:Label ID="lblTotalFob" runat="server" Text="" class="value"></asp:Label>
                                    </div>
                                    <!-- .stat -->
                                    <div class="stat">
                                        <h4>Total P.Compra</h4>
                                        <asp:Label ID="lblTotalCompra" runat="server" Text="" class="value"></asp:Label>
                                    </div>
                                    <!-- .stat -->
                                </div>
                            </div>
                            <!-- /widget-content -->
                        </div>
                        <!-- /widget -->
                        <!-- /widget-content -->
                        <div class="widget-content">
                            <div class="table-responsive">
                                <table class="table table-striped table-bordered table-hover" id="dataTables-example">
                                    <thead>
                                        <tr>
                                            <th>Codigo</th>
                                            <th style="width: 20%;">Articulo</th>
                                            <th style="text-align: right;">Cantidad</th>
                                            <th style="text-align: right;">SIM</th>
                                            <th style="text-align: right;">FOB</th>
                                            <th style="text-align: right;">Total FOB</th>
                                            <th style="text-align: right;">PPP</th>
                                            <th style="text-align: right;">P.Compra</th>
                                            <th style="text-align: right;">Total P.Compra</th>
                                            <th style="text-align: right;">P.Compra ARG.</th>
                                            <th style="text-align: right;">P. Venta</th>
                                            <th style="text-align: right;">Stock Act.</th>
                                            <th style="width: 10%;"></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:PlaceHolder ID="phItems" runat="server"></asp:PlaceHolder>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                    <!-- /widget -->
                    <div class="form-group">
                        <asp:Button ID="btnAgregar" runat="server" Text="Guardar" class="btn btn-success" Visible="false" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>

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
                                        <asp:Label runat="server" ID="lblMensaje" Text="Esta seguro que desea eliminar?" Style="text-align: center"></asp:Label>
                                    </h5>
                                </div>

                                <div class="col-md-3">
                                    <asp:TextBox runat="server" ID="txtMovimiento" Text="0" Style="display: none"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button runat="server" ID="btnSi" Text="Eliminar" class="btn btn-danger" OnClick="btnSi_Click" />
                            <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                        </div>
                    </div>

                </div>
            </div>
        </div>

        <div id="modalActualizar" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                                <h4 class="modal-title">Actualizacion costo</h4>
                            </div>
                            <div class="modal-body">
                                <div role="form" class="form-horizontal col-md-12">
                                    <div class="form-group">
                                        <div class="col-md-6" style="padding:0px;">
                                            <h4>
                                                <asp:Label Text="Escenario 1: Mantiene Margen" runat="server" />
                                            </h4>
                                        </div>
                                        <div class="col-md-6">
                                            <h4>
                                                <asp:Label Text="Escenario 2: Mantiene P. Vta" runat="server" />
                                            </h4>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-3">
                                            <asp:Label Text="Costo nuevo:" runat="server" />
                                        </div>
                                        <div class="col-md-3" style="padding: 0px;">
                                            <div class="input-group">
                                                <span class="input-group-addon">$</span>
                                                <asp:TextBox ID="txtCostoNuevo" runat="server" class="form-control" Text="1.0" disabled Style="text-align: right;" />
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:Label Text="Costo nuevo:" runat="server" disabled />
                                        </div>
                                        <div class="col-md-3" style="padding: 0px;">
                                            <div class="input-group">
                                                <span class="input-group-addon">$</span>
                                                <asp:TextBox ID="txtCostoNuevo2" runat="server" class="form-control" Text="0.0" disabled Style="text-align: right;" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-3">
                                            <asp:Label Text="Margen actual:" runat="server" />
                                        </div>
                                        <div class="col-md-3" style="padding: 0px;">
                                            <div class="input-group">
                                                <span class="input-group-addon">%</span>
                                                <asp:TextBox ID="txtMargenActual" runat="server" class="form-control" Text="1.0" disabled Style="text-align: right;" />
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:Label Text="Margen nuevo:" runat="server" />
                                        </div>
                                        <div class="col-md-3" style="padding: 0px;">
                                            <div class="input-group">
                                                <span class="input-group-addon">%</span>
                                                <asp:TextBox ID="txtMargenNuevo" runat="server" class="form-control" Text="1.0" disabled Style="text-align: right;" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-3">
                                            <asp:Label Text="Precio Vta nuevo:" runat="server" />
                                        </div>
                                        <div class="col-md-3" style="padding: 0px;">
                                            <div class="input-group">
                                                <span class="input-group-addon">$</span>
                                                <asp:TextBox ID="txtPrecioVentaNuevo" runat="server" class="form-control" Text="1.0" disabled Style="text-align: right;" />
                                            </div>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:Label Text="Precio Vta actual:" runat="server" />
                                        </div>
                                        <div class="col-md-3" style="padding: 0px;">
                                            <div class="input-group">
                                                <span class="input-group-addon">$</span>
                                                <asp:TextBox ID="txtPrecioVentaActual" runat="server" class="form-control" Text="1.0" disabled Style="text-align: right;" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-3">
                                            <asp:LinkButton ID="lbtnActualizarPrecioVta" runat="server" class="btn btn-success" Text="Actualizar Precio Vta" OnClick="lbtnActualizarPrecioVta_Click" />
                                        </div>
                                        <div class="col-md-3">
                                        </div>
                                        <div class="col-md-3">
                                            <asp:LinkButton ID="lbtnActualizarMargen" runat="server" class="btn btn-success" Text="Actualizar Margen" OnClick="lbtnActualizarMargen_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:TextBox  ID="txtIdArticulo" runat="server" Visible="false" Text="0"/>
                                <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>

        <script>
            function abrirdialog(valor) {
                document.getElementById('<%= txtMovimiento.ClientID %>').value = valor;
            }
            function abrirdialog2(valor) {
                $('#modalActualizar').modal('show');
            }
        </script>

        <link href="../../css/pages/reports.css" rel="stylesheet">

        <!-- Core Scripts - Include with every page -->
        <script src="../../Scripts/jquery-1.10.2.js"></script>
        <script src="../../Scripts/bootstrap.min.js"></script>

        <script src="../../Scripts/libs/jquery-1.9.1.min.js"></script>
        <script src="../../Scripts/libs/jquery-ui-1.10.0.custom.min.js"></script>
        <script src="../../Scripts/libs/bootstrap.min.js"></script>

        <script src="../../Scripts/Application.js"></script>

        <script src="../../Scripts/plugins/msgGrowl/js/msgGrowl.js"></script>
        <script src="../../Scripts/plugins/lightbox/jquery.lightbox.min.js"></script>
        <script src="../../Scripts/plugins/msgbox/jquery.msgbox.min.js"></script>
        <script src="../../Scripts/demo/notifications.js"></script>


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

    </div>
</asp:Content>
