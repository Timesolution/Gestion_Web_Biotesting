<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LotesTarjetasABM.aspx.cs" Inherits="Gestion_Web.Formularios.Valores.LotesTarjetasABM" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">

            <div class="col-md-12">

                <div class="widget stacked">

                    <div class="widget-header">
                        <i class="icon-pencil"></i>
                        <h3>Cierre Lote</h3>

                    </div>

                    <div class="widget-content">
                        <div class="bs-example">
                            <div id="validation-form" role="form" class="form-horizontal col-md-12">
                                <fieldset>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <div class="form-group">
                                                <label class="col-md-3">Posnet</label>
                                                <div class="col-md-3">
                                                    <asp:DropDownList ID="DropListPosnet" runat="server" class="form-control" AutoPostBack="true" ></asp:DropDownList>
                                                    <!-- /input-group -->
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Campo Obligatorio" ControlToValidate="DropListPosnet" InitialValue="-1" ValidationGroup="ArticuloGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="form-group" Style="display:none;">
                                                <label for="name" class="col-md-3">Tarjeta</label>
                                                <div class="col-md-3">
                                                    <asp:DropDownList ID="ListTarjetas" runat="server" class="form-control"></asp:DropDownList>
                                                    <!-- /input-group -->
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:RequiredFieldValidator ControlToValidate="ListTarjetas" ID="RequiredFieldValidator1" runat="server" InitialValue="-1" ErrorMessage="Campo Obligatorio" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <label for="name" class="col-md-3">Fecha Cierre</label>
                                                <div class="col-md-3">
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="icon-calendar"></i></span>
                                                        <asp:TextBox ID="txtFecha" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:RequiredFieldValidator ControlToValidate="txtFecha" ID="RequiredFieldValidator29" runat="server" ErrorMessage="Seleccione Fecha" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>

                                            <div class="form-group">
                                                <label for="name" class="col-md-3">Numero Lote</label>
                                                <div class="col-md-3">
                                                    <asp:TextBox ID="txtNumero" runat="server" type="number" class="form-control" disabled ></asp:TextBox>
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:RequiredFieldValidator ControlToValidate="txtNumero" ID="RequiredFieldValidator2" runat="server" ErrorMessage="Campo Obligatorio" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>

                                            <div class="form-group" Style="display:none;">
                                                <label for="name" class="col-md-3">Importe</label>
                                                <div class="col-md-3">
                                                    <div class="input-group">
                                                        <span class="input-group-addon">$</span>
                                                        <asp:TextBox ID="txtImporte" Text="0" runat="server" Style="text-align: right;" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator35" runat="server" ErrorMessage="Campo Obligatorio" ControlToValidate="txtImporte" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="name" class="col-md-3">Cantidad Cupones:</label>
                                                <div class="col-md-3">
                                                    <asp:TextBox ID="txtCantCupones" Text="0" runat="server" Style="text-align: right;display:none;" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Campo Obligatorio" ControlToValidate="txtImporte" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>

                                            <asp:PlaceHolder ID="phTarjetas" runat="server" />

                                            <div class="col-md-8">
                                                <asp:Button ID="btnAgregar" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnAgregar_Click" ValidationGroup="ArticuloGroup" />
                                                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" class="btn btn-default" PostBackUrl="../Valores/LotesTarjetasF.aspx" />
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </fieldset>
                            </div>
                        </div>

                    </div>


                </div>
            </div>
        </div>
    </div>

    <%--roww--%>

    <link href="../../css/pages/reports.css" rel="stylesheet">
    <!-- Core Scripts - Include with every page -->
    <script src="../../Scripts/jquery-1.10.2.js"></script>
    <script src="../../Scripts/bootstrap.min.js"></script>
    <%--<script src="../Scripts/plugins/metisMenu/jquery.metisMenu.js"></script>--%>

    <script src="../../Scripts/libs/jquery-1.9.1.min.js"></script>
    <script src="../../Scripts/libs/jquery-ui-1.10.0.custom.min.js"></script>
    <script src="../../Scripts/libs/bootstrap.min.js"></script>

    <script src="../../Scripts/plugins/hoverIntent/jquery.hoverIntent.minified.js"></script>
    <script src="../../Scripts/plugins/lightbox/jquery.lightbox.min.js"></script>

    <script src="../../Scripts/Application.js"></script>

    <script src="../../Scripts/demo/gallery.js"></script>

    <script src="../../Scripts/plugins/msgGrowl/js/msgGrowl.js"></script>
    <script src="../../Scripts/plugins/lightbox/jquery.lightbox.min.js"></script>
    <script src="../../Scripts/plugins/msgbox/jquery.msgbox.min.js"></script>
    <script src="../../Scripts/demo/notifications.js"></script>


    <script>
        $(function () {
            $("#<%= txtFecha.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });
    </script>

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endReq);
        function endReq(sender, args) {
            $(function () {
                $("#<%= txtFecha.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
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
                { return true; }
                else
                { return false; }
            }
            return true;
        }
    </script>

    <script type="text/javascript">
        function updateProgress(percentage) {
            document.getElementById('ProgressBar').style.width = percentage + "%";
        }
    </script>

</asp:Content>
