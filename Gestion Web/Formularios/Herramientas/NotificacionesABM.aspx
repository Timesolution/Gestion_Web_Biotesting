<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NotificacionesABM.aspx.cs" Inherits="Gestion_Web.Formularios.Herramientas.Notificaciones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-md-12">
                <div class="widget stacked">
                    <div class="stat">
                        <h5><i class="icon-map-marker"></i>Herramientas > Notificaciones</h5>
                    </div>
                    <div class="widget-header">
                        <i class="icon-pencil"></i>
                        <h3>Notificaciones</h3>
                    </div>
                    <div class="widget-content">
                        <div class="form-horizontal col-md-12">
                            <div class="form-group">
                                <label class="col-md-2">Nombre campaña</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="TextBoxNombreCampaña" runat="server" class="form-control" TextMode="MultiLine" Rows="1" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-2">Titulo notificacion</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="TextBoxTituloNotificacion" runat="server" class="form-control" TextMode="MultiLine" Rows="1" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-2">Mensaje</label>
                                <div class="col-md-6">
                                    <asp:TextBox ID="TextBoxMensaje" class="form-control" runat="server" TextMode="MultiLine" Rows="4" />
                                </div>
                            </div>
                            <a class="btn btn-success" onclick="ComprobarCampos()" data-toggle="modal" href="#modalConfirmar" runat="server">Enviar</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="modalConfirmar" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" id="btnCerrarModalConfirmacion" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Enviar Notificacion</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <label style="font-size:large">¿Esta seguro que desea enviar esta notificacion?</label>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbtnEnviarNotificacion" OnClick="lbtnEnviarNotificacion_Click" runat="server" Text="Enviar" class="btn btn-success"/>
                    <button type="button" class="btn btn-danger" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                </div>
            </div>
        </div>
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
