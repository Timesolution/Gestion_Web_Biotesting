<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OrdenReparacionObservacionesABM.aspx.cs" Inherits="Gestion_Web.Formularios.OrdenReparacion.OrdenReparacionObservacionesABM" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">
            <div class="widget stacked ">
                <div class="widget-header">                    
                    <h3>Observaciones</h3>
                </div>
                <div class="widget-content">
                    <div role="form" class="form-horizontal col-md-12">
                        <div class="form-group">
                            <label for="name" class="col-md-1">Observacion</label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtDetalleObservacion" runat="server" class="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                            </div>
                            <div class="col-md-1">
                                <asp:LinkButton ID="lbtnAgregarObservacion" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" ValidationGroup="EventosGroup" OnClick="lbtnAgregarObservacion_Click"/>
                            </div>
                            <div class="col-md-4">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtDetalleObservacion" ValidationGroup="EventosGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-12">

                        <div class="widget stacked widget-table">

                            <div class="widget-header">
                                <span class="icon-external-link"></span>
                                <h3>Observaciones</h3>
                            </div>
                            <div class="widget-content">
                                <table class="table table-bordered table-striped">
                                    <thead>
                                        <tr>
                                            <th>Fecha</th>
                                            <th>Detalle</th>
                                            <th>Usuario</th>                                            
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:PlaceHolder ID="phEventos" runat="server"></asp:PlaceHolder>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
        </div>
        </div>
    </div>
    <script src="../../Scripts/jquery-1.10.2.js"></script>
    <script src="../../Scripts/bootstrap.min.js"></script>
    <script src="../Scripts/plugins/metisMenu/jquery.metisMenu.js"></script>

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
    <script src="//code.jquery.com/ui/1.10.4/jquery-ui.js"></script>
    <script src="../../js/daypilot-modal-2.0.js"></script>

    <script src="//cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>
    <script src="../../Scripts/plugins/dataTables/custom.tables.js"></script>
    <link href="//cdn.datatables.net/1.10.2/css/jquery.dataTables.css" rel="stylesheet" />
</asp:Content>
