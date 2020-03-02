<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StoresBanners.aspx.cs" Inherits="Gestion_Web.Formularios.Stores.StoresBanners" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-md-12 col-xs-12">
        <div class="widget stacked">
            <div class="widget-header">
                <i class="icon-th-large"></i>
                <h3>Banners</h3>
            </div>
            <!-- /widget-header -->
            <div class="widget stacked">
                <div class="widget-content">
                    <div class="form-group">
                        <label>Banner 1</label>
                        <asp:FileUpload ID="FileUpload1" runat="server" />
                        <br />
                        <asp:Button ID="btnAct1" class="btn btn-success" runat="server" Text="Actualizar" OnClick="btnAct1_Click" />
                        <asp:Button ID="btnEliminar_1" class="btn btn-danger" runat="server" Text="Eliminar" OnClick="btnEliminar_1_Click" />
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
                        <br />
                        <asp:Button ID="btnAct2" class="btn btn-success" runat="server" Text="Actualizar" OnClick="btnAct2_Click" />
                        <asp:Button ID="btnEliminar_2" class="btn btn-danger" runat="server" Text="Eliminar" OnClick="btnEliminar_1_Click" />
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
                        <br />
                        <asp:Button ID="btnAct3" class="btn btn-success" runat="server" Text="Actualizar" OnClick="btnAct3_Click" />
                        <asp:Button ID="btnEliminar_3" class="btn btn-danger" runat="server" Text="Eliminar" OnClick="btnEliminar_1_Click" />
                        <div class="form-group">
                            <label>Previsualizacion</label>
                            <asp:Image class="form-control" ID="Image3" Height="200px" Width="200px" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="widget stacked">
                <div class="widget-content">
                    <div class="form-group">
                        <label>Banner 4</label>
                        <asp:FileUpload ID="FileUpload4" runat="server" />
                        <br />
                        <asp:Button ID="btnAct4" class="btn btn-success" runat="server" Text="Actualizar" OnClick="btnAct4_Click" />
                        <asp:Button ID="btnEliminar_4" class="btn btn-danger" runat="server" Text="Eliminar" OnClick="btnEliminar_1_Click" />
                        <div class="form-group">
                            <label>Previsualizacion</label>
                            <asp:Image class="form-control" ID="Image4" Height="200px" Width="200px" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="widget stacked">
                <div class="widget-content">
                    <div class="form-group">
                        <label>Banner 5</label>
                        <asp:FileUpload ID="FileUpload5" runat="server" />
                        <br />
                        <asp:Button ID="btnAct5" class="btn btn-success" runat="server" Text="Actualizar" OnClick="btnAct5_Click" />
                        <asp:Button ID="btnEliminar_5" class="btn btn-danger" runat="server" Text="Eliminar" OnClick="btnEliminar_1_Click" />
                        <div class="form-group">
                            <label>Previsualizacion</label>
                            <asp:Image class="form-control" ID="Image5" Height="200px" Width="200px" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="widget stacked">
                <div class="widget-content">
                    <div class="form-group">
                        <label>Banner 6</label>
                        <asp:FileUpload ID="FileUpload6" runat="server" />
                        <br />
                        <asp:Button ID="btnAct6" class="btn btn-success" runat="server" Text="Actualizar" OnClick="btnAct6_Click" />
                        <asp:Button ID="btnEliminar_6" class="btn btn-danger" runat="server" Text="Eliminar" OnClick="btnEliminar_1_Click" />
                        <div class="form-group">
                            <label>Previsualizacion</label>
                            <asp:Image class="form-control" ID="Image6" Height="200px" Width="200px" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="widget stacked">
                <div class="widget-content">
                    <div class="form-group">
                        <label>Banner 7</label>
                        <asp:FileUpload ID="FileUpload7" runat="server" />
                        <br />
                        <asp:Button ID="btnAct7" class="btn btn-success" runat="server" Text="Actualizar" OnClick="btnAct7_Click" />
                        <asp:Button ID="btnEliminar_7" class="btn btn-danger" runat="server" Text="Eliminar" OnClick="btnEliminar_1_Click" />
                        <div class="form-group">
                            <label>Previsualizacion</label>
                            <asp:Image class="form-control" ID="Image7" Height="200px" Width="200px" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="widget stacked">
                <div class="widget-content">
                    <div class="form-group">
                        <label>Banner 8</label>
                        <asp:FileUpload ID="FileUpload8" runat="server" />
                        <br />
                        <asp:Button ID="btnAct8" class="btn btn-success" runat="server" Text="Actualizar" OnClick="btnAct8_Click" />
                        <asp:Button ID="btnEliminar_8" class="btn btn-danger" runat="server" Text="Eliminar" OnClick="btnEliminar_1_Click" />
                        <div class="form-group">
                            <label>Previsualizacion</label>
                            <asp:Image class="form-control" ID="Image8" Height="200px" Width="200px" runat="server" />
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
