<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CuentasBancariasABM.aspx.cs" Inherits="Gestion_Web.Formularios.Valores.CuentasBancariasABM" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-md-12">
                <div class="widget stacked">
                    <div class="widget-header">
                        <i class="icon-pencil"></i>
                        <h3>Cuenta Bancaria</h3>

                    </div>
                    <!-- /.widget-header -->

                    <div class="widget-content">
                        <div class="bs-example">
                            <ul id="myTab" class="nav nav-tabs">
                                <li class="active"><a href="#home" data-toggle="tab">Detalle</a></li>

                            </ul>
                            <div class="tab-content">
                                <div class="tab-pane fade active in" id="home">
                                    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Always" runat="server">
                                        <ContentTemplate>
                                            <div id="validation-form" role="form" class="form-horizontal col-md-7">

                                                <div class="form-group">
                                                    <label for="name" class="col-md-3">Banco</label>

                                                    <div class="col-md-6">
                                                        <asp:DropDownList ID="ListBanco" runat="server" class="form-control"></asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-3">
                                                        <asp:RequiredFieldValidator ControlToValidate="ListBanco" ID="RequiredFieldValidator18" runat="server" ErrorMessage="Seleccione Empresa" InitialValue="-1" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>

                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-3">Numero</label>
                                                    <div class="col-md-6">
                                                        <asp:TextBox ID="txtNumero" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RequiredFieldValidator ControlToValidate="txtNumero" ID="RequiredFieldValidator29" runat="server" ErrorMessage="Campo obligatorio" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-3">Descripcion</label>
                                                    <div class="col-md-6">
                                                        <asp:TextBox ID="txtDescripcion" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RequiredFieldValidator ControlToValidate="txtDescripcion" ID="RequiredFieldValidator1" runat="server" ErrorMessage="Campo obligatorio" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label id="lbCUITDNI" runat="server" for="name" class="col-md-3">CUIT</label>
                                                    <div class="col-md-6">
                                                        <asp:TextBox ID="txtCuit" runat="server" MaxLength="11" class="form-control"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RequiredFieldValidator ControlToValidate="txtCuit" ID="RequiredFieldValidator2" runat="server" ErrorMessage="Campo obligatorio" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-3">Librador</label>

                                                    <div class="col-md-6">
                                                        <asp:TextBox ID="txtLibrador" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RequiredFieldValidator ControlToValidate="txtLibrador" ID="RequiredFieldValidator5" runat="server" ErrorMessage="Campo obligatorio" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <div class="col-md-3">
                                                          
                                                    </div>
                                                    <div class="col-md-6">
                                                        <asp:TextBox ID="txtBusqueda"  placeholder="Busqueda plan" runat="server" class="form-control"></asp:TextBox>
                                                        </div>
                                                      <div class="col-md-2">
                                                             <asp:LinkButton ID="LinkButton1" runat="server"  class="btn btn-primary" OnClick="lbtnBuscarNiveles_Click">
                                                                            <i class="shortcut-icon icon-search"></i></asp:LinkButton>
                                                    </div>
                                                    </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-3">Plan de cuentas</label>

                                                    <div class="col-md-6">
                                                        <asp:DropDownList ID="ListPlanCuentas" runat="server" class="form-control"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-8">
                                                <asp:Button ID="btnAgregar" runat="server" Text="Guardar" class="btn btn-success" ValidationGroup="ArticuloGroup" OnClick="btnAgregar_Click" />
                                                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" class="btn btn-default" PostBackUrl="../Valores/CuentasBancariasF.aspx" />
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                            </div>

                        </div>


                    </div>
                </div>
            </div>
        </div>
    </div>
    <%--roww--%>


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


    <%--<script>


        $(function () {
            $("#<%= txtFecha.ClientID %>").datepicker();
        });

        $(function () {
            $("#<%= txtVencimiento.ClientID %>").datepicker();
        });

    </script>--%>

    <%-- <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endReq);
        function endReq(sender, args) {
            $(function () {
                $("#<%= txtFecha.ClientID %>").datepicker();
             });

             $(function () {
                 $("#<%= txtVencimiento.ClientID %>").datepicker();
             });
         }
    </script>--%>

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
