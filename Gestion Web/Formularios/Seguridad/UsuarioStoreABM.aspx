<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UsuarioStoreABM.aspx.cs" Inherits="Gestion_Web.Formularios.Seguridad.UsuarioStoreABM" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">


        <div class="row">

            <div class="col-md-12">

                <div class="widget stacked">

                    <div class="widget-header">
                        <i class="icon-pencil"></i>
                        <h3>Usuario Store</h3>

                    </div>
                    <!-- /.widget-header -->

                    <div class="widget-content">
                        <div id="validation-form" role="form" class="form-horizontal col-md-10">

                            <div role="form" class="form-horizontal col-md-12">
                                <fieldset>
                                    <div class="form-group">

                                        <label class="col-md-4">Usuario</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtUsuarioStore" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtUsuarioStore" ValidationGroup="AgregarUsuarioAlStore" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Contraseña</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtContraseñaStore" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtContraseñaStore" ValidationGroup="AgregarUsuarioAlStore" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Nombre</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtNombreStore" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtNombreStore" ValidationGroup="AgregarUsuarioAlStore" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Apellido</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtApellidoStore" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <%--<div class="col-md-2">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtApellidoStore" ValidationGroup="AgregarUsuarioAlStore" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                                </div>--%>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Telefono</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtTelefonoStore" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <%--<div class="col-md-2">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtTelefonoStore" ValidationGroup="AgregarUsuarioAlStore" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                                </div>--%>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Mail</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtMailStore" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <%--<div class="col-md-2">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtMailStore" ValidationGroup="AgregarUsuarioAlStore" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                                </div>--%>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Coeficiente</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtCoeficienteStore" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtCoeficienteStore" ValidationGroup="AgregarUsuarioAlStore" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">PerfilStore</label>
                                        <div class="col-md-4">
                                            <asp:DropDownList ID="DropPerfilStore" runat="server" class="form-control" AutoPostBack="true"></asp:DropDownList>
                                            <%--OnSelectedIndexChanged="DropPerfilStore_SelectedIndexChanged"--%>

                                            <!-- /input-group -->
                                        </div>

                                        <div class="col-md-4">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="DropPerfilStore" InitialValue="0" ValidationGroup="AgregarUsuarioAlStore" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Store</label>
                                        <div class="col-md-4">
                                            <asp:DropDownList ID="DropStore" runat="server" class="form-control" AutoPostBack="true"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="DropStore" InitialValue="0" ValidationGroup="AgregarUsuarioAlStore" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </fieldset>
                            </div>

                        </div>
                        <div class="col-md-8">
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" class="btn btn-success" ValidationGroup="AgregarUsuarioAlStore" OnClick="btnGuardar_Click" />
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" class="btn btn-default" PostBackUrl="../Seguridad/UsuariosF.aspx" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

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

</asp:Content>
