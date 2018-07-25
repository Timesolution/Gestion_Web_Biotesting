<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StoresBanners.aspx.cs" Inherits="Gestion_Web.Formularios.Stores.StoresBanners" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-md-12 col-xs-12">
        <div class="widget stacked">
            <div class="widget-header">
                <i class="icon-th-large"></i>
                <h3>Galeria de Imagenes</h3>
            </div>
            <!-- /widget-header -->
            <div class="widget stacked">
                <div class="widget-content">
                    <div class="form-group">
                        <label>Banner 1</label>
                        <asp:FileUpload ID="FileUpload1" runat="server" />
                        <asp:Button ID="btnAct1" class="btn btn-primary" runat="server" Text="Actualizar" OnClick="btnAct1_Click" />
                        <div class="form-group">
                            <label>Previsualizacion</label>
                            <asp:Image class="form-control" ID="Image1" Height="200px" Width="200px" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="widget stacked">
                <div class="widget-content">
                    <div class="form-group">
                        <label>Banner 2</label>
                        <asp:FileUpload ID="FileUpload2" runat="server" />
                        <asp:Button ID="btnAct2" class="btn btn-primary" runat="server" Text="Actualizar" OnClick="btnAct2_Click" />
                        <div class="form-group">
                            <label>Previsualizacion</label>
                            <asp:Image class="form-control" ID="Image2" Height="200px" Width="200px" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="widget stacked">
                <div class="widget-content">
                    <div class="form-group">
                        <label>Banner 3</label>
                        <asp:FileUpload ID="FileUpload3" runat="server" />
                        <asp:Button ID="btnAct3" class="btn btn-primary" runat="server" Text="Actualizar" OnClick="btnAct3_Click" />
                        <div class="form-group">
                            <label>Previsualizacion</label>
                            <asp:Image class="form-control" ID="Image3" Height="200px" Width="200px" runat="server" />
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
