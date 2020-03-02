<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StoresABM.aspx.cs" Inherits="Gestion_Web.Formularios.Stores.StoresABM" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script src="../../Scripts/jquery-1.10.2.js"></script>
    <script src="../../Scripts/bootstrap.min.js"></script>
    <%--<script src="../../Scripts/plugins/metisMenu/jquery.metisMenu.js"></script>--%>

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

    <div class="container">
        <div class="row">

            <div class="col-md-12">

                <div class="widget stacked">
                    <div class="stat">
                        <h5><i class="icon-map-marker"></i>Maestro > Stores > Store</h5>
                        <asp:Label ID="lblFiltroAnterior" runat="server" Style="display: none;"></asp:Label>
                    </div>
                    <div class="widget-header">
                        <i class="icon-pencil"></i>
                        <h3>Store</h3>

                    </div>
                    <!-- /.widget-header -->
                    <div class="widget-content">
                        <div class="bs-example">
                            <div class="tab-content">
                                <div class="tab-pane fade active in" id="home">
                                    <div id="validation-form" role="form" class="form-horizontal col-md-10">
                                        <fieldset>
                                            <div class="form-group">
                                                <label for="name" class="col-md-4">Nombre Store</label>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="txtNombreStore" runat="server" class="form-control"></asp:TextBox>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtNombreStore" ValidationGroup="StoreGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="name" class="col-md-4">Detalle Store</label>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="txtDetalleStore" runat="server" class="form-control"></asp:TextBox>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtDetalleStore" ValidationGroup="StoreGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="col-md-8">
                                                <asp:LinkButton ID="btnAceptar" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnAceptar_Click" ValidationGroup="StoreGroup" Visible="false"></asp:LinkButton>
                                                <asp:LinkButton ID="btnAgregar" runat="server" Text="Agregar" class="btn btn-success" Onclick="btnAgregar_Click" ValidationGroup="StoreGroup" Visible="false"></asp:LinkButton>
                                                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" class="btn btn-default" PostBackUrl="StoresF.aspx" />
                                            </div>
                                        </fieldset>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
