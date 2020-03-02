<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ImportacionesGastosF.aspx.cs" Inherits="Gestion_Web.Formularios.Compras.ImportacionesGastosF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">

        <div>
            <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                <ContentTemplate>
                    <div class="col-md-12 col-xs-12">

                        <div class="widget stacked">
                            <div class="stat">
                                <h5><i class="icon-map-marker"></i>&nbsp Compras > Importacion > Gastos </h5>
                            </div>
                            <div class="widget-header">
                                &nbsp <i class="fa fa-import"></i>
                                <h3>Importacion</h3>
                            </div>
                            <div class="widget-content">
                                <div role="form" class="form-horizontal col-md-6">
                                    <fieldset>
                                        <div class="form-group">
                                            <label for="name" class="col-md-2">Tipo Gasto:</label>
                                            <div class="col-md-6">
                                                <asp:DropDownList ID="ListTipoGasto" runat="server" class="form-control"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" InitialValue="-1" ErrorMessage="*" ControlToValidate="ListTipoGasto" ValidationGroup="BusquedaArticulo" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-2">Moneda:</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="ListMonedaImportacion" runat="server" class="form-control"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" InitialValue="-1" ErrorMessage="*" ControlToValidate="ListMonedaImportacion" ValidationGroup="BusquedaArticulo" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-2">Tipo Cambio:</label>
                                            <div class="col-md-4">
                                                <div class="input-group">
                                                    <span class="input-group-addon">$</span>
                                                    <asp:TextBox ID="txtCambioMonedaImportacion" runat="server" class="form-control" Style="text-align: right;" onkeypress="javascript:return validarNro(event)" />
                                                </div>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*" ControlToValidate="txtCambioMonedaImportacion" ValidationGroup="BusquedaArticulo" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-2">Importe Pesos:</label>
                                            <div class="col-md-4">
                                                <div class="input-group">
                                                    <span class="input-group-addon">$</span>
                                                    <asp:TextBox ID="txtTotalPesos" runat="server" class="form-control" Style="text-align: right;" onkeypress="javascript:return validarNro(event)" />
                                                </div>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*" ControlToValidate="txtTotalPesos" ValidationGroup="BusquedaArticulo" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:LinkButton ID="lbtnAgregarGasto" runat="server" class="btn btn-success" Text="<i class='shortcut-icon icon-ok'></i>" OnClick="lbtnAgregarGasto_Click" />
                                            </div>
                                        </div>
                                    </fieldset>
                                </div>
                            </div>
                            <br />  
                            <!-- /widget-content -->
                            <div class="widget big-stats-container stacked">
                                <div class="widget-content">

                                    <div id="big_stats" class="cf">
                                        <div class="stat">
                                            <h4>Total Gastos</h4>
                                            <asp:Label ID="lblTotalGastos" runat="server" Text="" class="value"></asp:Label>
                                        </div>
                                        <!-- .stat -->
                                    </div>

                                </div>


                                <!-- /widget-content -->

                            </div>
                            <!-- /widget -->
                            <div class="widget-content">
                                <div class="table-responsive">
                                    <table class="table table-striped table-bordered table-hover" id="dataTables-example">
                                        <thead>
                                            <tr>
                                                <th>Tipo Gasto</th>
                                                <th style="text-align: right;">Importe Pesos</th>
                                                <th>Moneda</th>
                                                <th style="text-align: right;">Tipo Cambio</th>                                                
                                                <th style="text-align: right;">Importe Total</th>
                                                <th style="width: 5%;"></th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:PlaceHolder ID="phGastos" runat="server"></asp:PlaceHolder>
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

    <script>
        function abrirdialog(valor) {
            document.getElementById('<%= txtMovimiento.ClientID %>').value = valor;
        }
    </script>


    <link href="../../css/pages/reports.css" rel="stylesheet">

    <!-- Core Scripts - Include with every page -->
    <script src="../../Scripts/jquery-1.10.2.js"></script>
    <script src="../../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/plugins/metisMenu/jquery.metisMenu.js"></script>

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


</asp:Content>
