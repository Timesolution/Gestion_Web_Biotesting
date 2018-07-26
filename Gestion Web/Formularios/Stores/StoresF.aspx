<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StoresF.aspx.cs" Inherits="Gestion_Web.Formularios.Stores.StoresF1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container">
        <div class="row">

            <div class="col-md-12 col-xs-12">
                <div class="widget stacked">
                    <div class="stat">
                        <h5><i class="icon-map-marker"></i>Maestro > Stores</h5>
                    </div>
                    <div class="widget-header">
                        <i class="icon-wrench"></i>
                        <h3>Herramientas</h3>
                    </div>
                    <!-- /widget-header -->

                    <div class="widget-content">
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 30%">
                                    <div class="col-md-12">
                                        <div class="input-group">
                                        </div>
                                </td>
                                <td style="width: 2%">
                                    <div class="btn-group" style="height: 100%">
                                        <a class="btn btn-primary ui-tooltip" data-original-title="Agregar Store" href="StoresABM.aspx?accion=1" style="width: 100%">
                                            <i class="shortcut-icon icon-plus"></i>
                                        </a>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>

            <%--stores--%>

            <div class="col-md-12 col-xs-12">
                <div class="widget stacked widget-table action-table">
                    <div class="widget-header">
                        <i class="icon-bookmark"></i>
                        <h3>Stores</h3>
                    </div>
                    <div class="widget-content">
                        <div class="panel-body">
                            <div class="table-responsive">
                                <table class="table table-striped table-bordered table-hover" id="dataTables-example">
                                    <thead>
                                        <tr>
                                            <th runat="server" style="width: 35%">Nombre Store</th>
                                            <th runat="server" style="width: 35%">Detalle</th>
                                            <th runat="server" class="td-actions" style="width: 10%"></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:PlaceHolder ID="phArticulos" runat="server"></asp:PlaceHolder>
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

</asp:Content>
