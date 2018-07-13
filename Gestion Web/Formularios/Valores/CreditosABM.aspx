<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreditosABM.aspx.cs" Inherits="Gestion_Web.Formularios.Valores.CreditosABM" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-md-12">
                <div class="widget stacked">
                    <div class="stat">
                    </div>

                    <div class="widget-header">
                        <i class="icon-pencil"></i>
                        <h3>Editar</h3>
                    </div>
                    <div class="widget-content">
                        <div class="form-horizontal col-md-12">
                            <fieldset>
                                <asp:Label runat="server" ID="lblIdSolicitud" Visible="false"></asp:Label>
                                <div class="form-group">
                                    <label class="col-md-2">Dni</label>
                                    <div class="col-md-3">
                                        <asp:TextBox runat="server" ID="txtDni" CssClass="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-2">Número Solicitud</label>
                                    <div class="col-md-3">
                                        <asp:TextBox runat="server" ID="txtNumeroSolicitud" CssClass="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-4">
                                        <asp:Button ID="lbtnGuardar" Text="Guardar" runat="server" class="btn btn-success" OnClick="lbtnGuardar_Click" />
                                    </div>
                                </div>
                            </fieldset>
                        </div>
                    </div>
                    <br />
                    <br />
                    <div class="widget-header">
                        <i class="icon-pencil"></i>
                        <h3>Validar Seguro</h3>
                    </div>
                    <div class="widget-content">
                        <div class="form-horizontal col-md-12">
                            <fieldset>
                                <div class="form-group">
                                    <label class="col-md-2">Comentario</label>
                                    <div class="col-md-6">
                                        <asp:TextBox runat="server" ID="txtComentarioValidarSeguro" CssClass="form-control" TextMode="MultiLine" Rows="5"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <div class="col-md-4">
                                        <asp:Button ID="btnValidarSeguro" Text="Validar" runat="server" class="btn btn-success" OnClick="btnValidarSeguro_Click" />
                                    </div>
                                </div>
                            </fieldset>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>

    <script>
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
                if (key == 8)
                { return true; }
                else
                { return false; }
            }
            return true;
        }
    </script>

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

</asp:Content>
