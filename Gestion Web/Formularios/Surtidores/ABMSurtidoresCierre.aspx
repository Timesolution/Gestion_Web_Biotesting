<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ABMSurtidoresCierre.aspx.cs" Inherits="Gestion_Web.Formularios.Surtidores.ABMSurtidoresCierre" %>

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
                                <div class="form-group">
                                    <label class="col-md-2">Surtidor:</label>
                                    <div class="col-md-3">
                                        <asp:DropDownList ID="ListSurtidor" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ListSurtidor_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ErrorMessage="*" InitialValue="-1" ControlToValidate="ListSurtidor" runat="server" ValidationGroup="SurtidoresGroup" Font-Bold="true" ForeColor="Red" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-2">Usuario:</label>
                                    <div class="col-md-3">
                                        <asp:DropDownList ID="ListUsuario" runat="server" class="form-control"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-2">Fecha:</label>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="txtFechaCierre" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="txtHoraCierre" runat="server" TextMode="Time" class="form-control" Width="80%"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="txtFechaCierre" runat="server" ValidationGroup="SurtidoresGroup" Font-Bold="true" ForeColor="Red" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-2">Cant. Inicial:</label>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtCantInicial" runat="server" Text="0" class="form-control" onkeypress="javascript:return validarNro(event);" disabled></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="txtCantInicial" runat="server" ValidationGroup="SurtidoresGroup" Font-Bold="true" ForeColor="Red" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-2">Cant. Cierre:</label>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtCantCierre" runat="server" Text="0" class="form-control" AutoPostBack="true" OnTextChanged="txtCantCierre_TextChanged" onkeypress="javascript:return validarNro(event);"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="txtCantCierre" runat="server" ValidationGroup="SurtidoresGroup" Font-Bold="true" ForeColor="Red" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-2">Cant. Litros:</label>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtCantLitrosTotal" runat="server" disabled Text="0" class="form-control" onkeypress="javascript:return validarNro(event);"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-2">Cant. Pesos:</label>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtCantPesosTotal" runat="server" disabled Text="0" class="form-control" onkeypress="javascript:return validarNro(event);"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-2">Observaciones:</label>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtObservacion" runat="server" class="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="txtCantCierre" runat="server" ValidationGroup="SurtidoresGroup" Font-Bold="true" ForeColor="Red" />
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-md-3">
                                        <asp:LinkButton ID="lbtnAgregar" runat="server" Text="Agregar" class="btn btn-success" OnClick="lbtnAgregar_Click" ValidationGroup="SurtidoresGroup" />
                                        <asp:Button ID="btnCancelar" runat="server" class="btn btn-default" Text="Cancelar" PostBackUrl="~/Formularios/Surtidores/SurtidoresCierreF.aspx" />
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

    

    <%--<script src="//code.jquery.com/jquery-1.9.1.js"></script>--%>
    <%--<script src="//code.jquery.com/ui/1.10.4/jquery-ui.js"></script>--%>
    <script>
        function pageLoad() {
            $("#<%= txtFechaCierre.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });                        
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
                if (key == 46 || key == 8) // Detectar . (punto) y backspace (retroceso)
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
