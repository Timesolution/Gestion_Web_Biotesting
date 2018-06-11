<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ABMAsuntosSMS.aspx.cs" Inherits="Gestion_Web.Formularios.Herramientas.ABMAsuntosSMS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">

        <div class="row">
            <div class="col-md-12">

                <div class="widget stacked">
                    <div class="stat">
                        <h5><i class="icon-map-marker"></i>Herramientas > Campañas </h5>
                    </div>
                    <div class="widget-header">
                        <i class="icon-pencil"></i>
                        <h3>Configuraciones</h3>

                    </div>
                    <!-- /.widget-header -->

                    <div class="widget-content">
                        <div class="form-horizontal col-md-10">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <div class="form-group">
                                        <label class="col-md-1">Nombre:</label>
                                        <div class="col-md-5">
                                            <asp:TextBox ID="txtNombreAsunto" runat="server" class="form-control" />
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ErrorMessage="*" ForeColor="Red" Font-Bold="true" ControlToValidate="txtNombreAsunto" runat="server" ValidationGroup="AsuntoGroup" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-1">Fecha:</label>
                                        <div class="col-md-2">
                                            <asp:TextBox ID="txtFechaAsunto" runat="server" class="form-control" />
                                        </div>
                                        <div class="col-md-2">
                                            <asp:TextBox ID="txtHoraAsunto" runat="server" class="form-control" TextMode="Time" />
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ErrorMessage="*" ForeColor="Red" Font-Bold="true" ControlToValidate="txtFechaAsunto" runat="server" ValidationGroup="AsuntoGroup" />
                                            <asp:RequiredFieldValidator ErrorMessage="*" ForeColor="Red" Font-Bold="true" ControlToValidate="txtHoraAsunto" runat="server" ValidationGroup="AsuntoGroup" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <asp:Button ID="btnGuardar" runat="server" class="btn btn-success" Text="Guardar" OnClick="btnGuardar_Click"  ValidationGroup="AsuntoGroup"/>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>

                </div>
            </div>
        </div>

    </div>

    <link href="../../css/pages/reports.css" rel="stylesheet">
    <!-- Core Scripts - Include with every page -->
    <script src="../../Scripts/jquery-1.10.2.js"></script>

    <%--<script src="../Scripts/plugins/metisMenu/jquery.metisMenu.js"></script>--%>

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

    <script>
        function pageLoad() {
            $("#<%= txtFechaAsunto.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        }
    </script>

</asp:Content>

