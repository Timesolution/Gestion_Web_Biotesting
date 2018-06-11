<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TransportesABM.aspx.cs" Inherits="Gestion_Web.Formularios.Transportes.TransportesABM" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">

            <div class="col-md-12">

                <div class="widget stacked">

                    <div class="widget-header">
                        <i class="icon-pencil"></i>
                        <h3>Compras</h3>
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
                                            <div role="form" class="form-horizontal col-md-12">
                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Nombre</label>

                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtNombreExpreso" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">CUIT</label>

                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtCuitExpreso" runat="server" class="form-control" MaxLength="11" onkeypress="javascript:return validarSoloNro(event)"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Telefono</label>

                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtTelefonoExpreso" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Direccion</label>

                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtDireccionExpreso" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Observaciones</label>

                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtObservacionesExpreso" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                
                                               
                                            </div>

                                            <div class="col-md-8">
                                                <asp:Button ID="btnAgregar" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnAgregar_Click" />
                                                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" class="btn btn-default" PostBackUrl="../Transportes/TransportesF.aspx" />
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
