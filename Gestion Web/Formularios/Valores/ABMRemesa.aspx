<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ABMRemesa.aspx.cs" Inherits="Gestion_Web.Formularios.Valores.ABMRemesa" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">
        <asp:UpdatePanel ID="UpdatePanel5" UpdateMode="Always" runat="server">
            <ContentTemplate>
                <div class="container">
                    <div class="row">

                        <div class="col-md-12">

                            <div class="widget stacked">
                                <div class="stat">
                                    <h5><i class="icon-map-marker"></i>Maestro > Valores > Remesas</h5>
                                    <asp:Label ID="lblFiltroAnterior" runat="server" Style="display: none;"></asp:Label>
                                </div>
                                <div class="widget-header">
                                    <i class="icon-pencil"></i>
                                    <h3>Orden de reparacion</h3>

                                </div>
                                <!-- /.widget-header -->
                                <div class="widget-content">
                                    <div class="bs-example">
                                        <div class="tab-content">
                                            <div class="tab-pane fade active in" id="home">
                                                <div id="validation-form" role="form" class="form-horizontal col-md-10">
                                                    <fieldset>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Numero de Remesa</label>
                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtNumeroRemesa" runat="server" Enabled="false"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtNumeroRemesa" ValidationGroup="StoreGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Fecha de entrega</label>
                                                            <div class="col-md-4">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon"><i class="icon-calendar"></i></span>
                                                                    <asp:TextBox ID="txtFecha" class="form-control" runat="server" Enabled="false"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFecha" ValidationGroup="StoreGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                                        </div>

                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Entrega</label>
                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtEntrega" runat="server" class="form-control"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtEntrega" ValidationGroup="StoreGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Recibe</label>
                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtRecibe" runat="server" class="form-control"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtRecibe" ValidationGroup="StoreGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>                                                        
                                                        
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Observacion</label>
                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtObservacion" runat="server" class="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtObservacion" ValidationGroup="StoreGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                        
                                                        <div class="col-md-8">
                                                            <%--<asp:Button ID="btnAgregar" runat="server" Text="Agregar" class="btn btn-success" OnClick="btnAgregar_Click" ValidationGroup="StoreGroup" Visible="false"/>
                                                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnGuardar_Click" ValidationGroup="StoreGroup" Visible="false" />
                                                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" class="btn btn-default" PostBackUrl="FacturasF.aspx" />--%>
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <link href="../../css/pages/reports.css" rel="stylesheet">
    <script src="../../Scripts/jquery-1.10.2.js"></script>

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

    <script src="../../Scripts/bootstrap.min.js"></script>

</asp:Content>
