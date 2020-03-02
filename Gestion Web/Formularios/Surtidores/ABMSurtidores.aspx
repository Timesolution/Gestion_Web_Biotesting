<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ABMSurtidores.aspx.cs" Inherits="Gestion_Web.Formularios.Surtidores.ABMSurtidores" %>

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
                                    <label class="col-md-1">Codigo:</label>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtCodigo" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="txtCodigo" runat="server" ValidationGroup="SurtidoresGroup" Font-Bold="true" ForeColor="Red"/>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-1">Descripcion:</label>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtDescripcion" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="txtDescripcion" runat="server" ValidationGroup="SurtidoresGroup" Font-Bold="true" ForeColor="Red"/>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-1">Numero:</label>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="txtNumero" runat="server" class="form-control" onkeypress="javascript:return validarNro(event);"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="txtNumero" runat="server" ValidationGroup="SurtidoresGroup" Font-Bold="true" ForeColor="Red"/>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-1">Contador:</label>
                                    <div class="col-md-2">
                                        <asp:TextBox ID="txtContador" runat="server" class="form-control" onkeypress="javascript:return validarNro(event);"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="txtContador" runat="server" ValidationGroup="SurtidoresGroup" Font-Bold="true" ForeColor="Red"/>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-1">Precio x Lt:</label>
                                    <div class="col-md-2">
                                        <div class="input-group">             
                                            <span class="input-group-addon">$</span>                           
                                            <asp:TextBox ID="txtPrecioLitro" runat="server" class="form-control" MaxLength="8" onkeypress="javascript:return validarNro(event);"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="txtPrecioLitro" runat="server" ValidationGroup="SurtidoresGroup" Font-Bold="true" ForeColor="Red"/>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-3">
                                        <asp:LinkButton ID="lbtnAgregar" runat="server" Text="Agregar" class="btn btn-success" OnClick="lbtnAgregar_Click" ValidationGroup="SurtidoresGroup" />
                                        <asp:Button ID="btnCancelar" runat="server" class="btn btn-default" Text="Cancelar" PostBackUrl="~/Formularios/Surtidores/SurtidoresF.aspx" />
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
