<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StockHistTest.aspx.cs" Inherits="Gestion_Web.Formularios.Articulos.StockHistTest" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">

        <div class="container">

                    <div class="col-md-12 col-xs-12">
                        <div class="widget stacked">

                            <div class="widget-header">
                                <i class="icon-wrench"></i>
                                <h3>Herramientas</h3>
                            </div>
                            <!-- /widget-header -->

                            <div class="widget-content">

                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 40%">
                                            <label class="col-md-4">Articulo</label>
                                            <div class="col-md-8">
                                                <div class="input-group">
                                                    <asp:DropDownList ID="DropListArticulos" runat="server" class="form-control"></asp:DropDownList>
                                                </div>
                                                <!-- /input-group -->
                                            </div>
                                        </td>
                                        <td style="width: 40%">
                                            <label class="col-md-4">Sucursal</label>
                                            <div class="col-md-8">
                                                <div class="input-group">
                                                    <asp:DropDownList ID="DropListSucursal" runat="server" class="form-control"></asp:DropDownList>
                                                </div>
                                                <!-- /input-group -->
                                            </div>
                                        </td>
                                        <td style="width: 20%">
                                            <div class="col-md-12">
                                                <span class="input-group-btn">
                                                    <asp:Button ID="btnBuscar" runat="server" Text="Generar Stock Historico" class="btn btn-primary" OnClick="btnBuscar_Click" />
                                                </span>
                                                <!-- /input-group -->
                                            </div>
                                        </td>


                                    </tr>

                                </table>
                            </div>
                            <!-- /widget-content -->

                        </div>
                        <!-- /widget -->
                    </div>
                    <div class="col-md-12 col-xs-12">
                        <div class="widget widget-table">

                            <div class="widget-header">
                                <i class="icon-wrench"></i>
                                <h3>Stock Historico

                                </h3>
                            </div>
                            <div class="widget-content">
                                <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Always" runat="server">
                                    <ContentTemplate>
                                        <a class="btn btn-info" style="display: none" data-toggle="modal" id="abreDialog" href="#modalFacturaDetalle">Agregar Tipo Cliente</a>
                                        <table class="table table-striped table-bordered">
                                            <thead>
                                                <tr>
                                                    <th>Codigo Stock</th>
                                                    <th>Sucursal</th>
                                                    <th>Articulo</th>
                                                    <th>Fecha</th>
                                                    <th>Stock</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:PlaceHolder ID="phStockHistorico" runat="server"></asp:PlaceHolder>
                                            </tbody>
                                        </table>

                                        </div>


                                    </ContentTemplate>
                                    <Triggers>
                                    </Triggers>
                                </asp:UpdatePanel>
                                <%--</div>--%>


                                <!-- /.content -->

                            </div>

                        </div>
                    </div>      
    </div>
    <script>

        function abrirdialog() {
            document.getElementById('abreDialog').click();
        }

    </script>
    

    <!-- Core Scripts - Include with every page -->
    <script src="../../Scripts/jquery-1.10.2.js"></script>
    <script src="../../Scripts/bootstrap.min.js"></script>
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

    <%--<script src="//code.jquery.com/jquery-1.9.1.js"></script>--%>

</div>
</asp:Content>
