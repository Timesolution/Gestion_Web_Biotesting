<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ABMMutuales.aspx.cs" Inherits="Gestion_Web.Formularios.Valores.ABMMutuales" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">
        <div class="col-md-12 col-xs-12">
            <div class="widget stacked">

                <div class="widget-header">
                    <i class="icon-wrench"></i>
                    <h3>Herramientas</h3>
                </div>
                <!-- /widget-header -->

                <div class="widget-content">
                    <div role="form" class="form-horizontal col-md-10">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <fieldset>
                                    <div class="form-group">
                                        <label class="col-md-2">Nombre:</label>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtNombre" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-2">Direccion:</label>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtDireccion" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-2">Telefono:</label>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtTelefono" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-2">Comision:</label>
                                        <div class="col-md-3">
                                            <div class="input-group">
                                                <span class="input-group-addon">%</span>
                                                <asp:TextBox ID="txtComision" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-2">Prox. Nro Pagare:</label>
                                        <div class="col-md-3">
                                            <div class="input-group">
                                                <span class="input-group-addon">Nº</span>
                                                <asp:TextBox ID="txtNumero" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label for="name" class="col-md-2">Cod. Cliente</label>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtCodCliente" class="form-control" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:LinkButton ID="btnBuscarCodigoCliente" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" OnClick="btnBuscarCodigoCliente_Click" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label for="name" class="col-md-2">Cliente</label>
                                        <div class="col-md-3">
                                            <asp:DropDownList ID="ListCliente" class="form-control" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ListCliente_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="ListCliente" InitialValue="-1" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" ValidationGroup="MutualGroup"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-2">Observaciones:</label>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtObservacion" runat="server" TextMode="MultiLine" Rows="4" class="form-control"></asp:TextBox>
                                        </div>
                                    </div>
                                </fieldset>
                                <div class="form-group">
                                    <div class="col-md-3">
                                        <asp:LinkButton ID="lbtnAgregar" runat="server" Text="Agregar" class="btn btn-success" OnClick="btnAgregar_Click" ValidationGroup="MutualGroup" />
                                        <asp:Button ID="btnCancelar" runat="server" class="btn btn-default" Text="Cancelar" PostBackUrl="~/Formularios/Valores/MutualesF.aspx" />
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <!-- /widget-content -->

            </div>
            <!-- /widget -->
        </div>

    </div>

    <script type="text/javascript">
        function openModal() {
            $('#modalAgregar').modal('show');
        }
    </script>

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
        $(document).ready(function () {
            $('#dataTables-example').dataTable({
                "bLengthChange": false,
                "bFilter": false,
                "bInfo": false,
                "bAutoWidth": false
            });
        });
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
                if (key == 8) // Detectar . (punto) y backspace (retroceso)
                { return true; }
                else
                { return false; }
            }
            return true;
        }
    </script>

    <script>
        //valida los campos solo numeros
        function validarSoloNro(e) {
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
                return false;
            }
            else { return true; }
        }
    </script>


</asp:Content>
