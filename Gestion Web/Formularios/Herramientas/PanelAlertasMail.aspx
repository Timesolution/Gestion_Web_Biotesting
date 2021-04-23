<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PanelAlertasMail.aspx.cs" Inherits="Gestion_Web.Formularios.Herramientas.PanelAlertasMail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">

        <div class="row">
            <div class="col-md-12">

                <div class="widget stacked">
                    <div class="stat">
                        <h5><i class="icon-map-marker"></i>Herramientas > Alertas de Mail</h5>
                    </div>
                    <div class="widget-header">
                        <i class="icon-pencil"></i>
                        <h3>Configuraciones</h3>

                    </div>
                    <!-- /.widget-header -->

                    <div class="widget-content">
                        <div class="form-horizontal col-md-12">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <asp:Panel ID="PanelConfig" runat="server">

                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-4">Observaciones de mail paciente agendado</label>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtObservacionesAgenda" runat="server" TextMode="MultiLine" class="form-control" Rows="6" ToolTip="Observaciones del mail"></asp:TextBox>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnObservacionesMailAgenda" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnObservacionesMailAgenda_Click" />
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-4">Días antes del recordatorio de tuno</label>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtDiasRecordatorioMail" TextMode="Number" runat="server" class="form-control" ToolTip="Dias" onkeypress="javascript:return ValidarSoloNumeros(event)"></asp:TextBox>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnDiasRecordatorioMail" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnDiasRecordatorioMail_Click" />
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-4">Observaciones del mail de recordatorio de turno</label>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtObservacionesRecordatorioMail" TextMode="MultiLine" runat="server" Rows="6" class="form-control" ToolTip="Observaciones del mail de recordatorio"></asp:TextBox>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnObservacionesRecordatorioMail" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnObservacionesRecordatorioMail_Click" />
                                            </div>
                                        </div>

                                    </asp:Panel>
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
        function ValidarSoloNumeros(e) {
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
                if (key == 8 || key == 46)//46 = '.'
                {
                    if (key == 46 && controlTxtImporte_ModalAgregarRegistro.value.includes(".", 0)) {
                        return false;
                    }
                    return true;
                }
                else { return false; }
            }
            return true;
        }
    </script>


</asp:Content>

