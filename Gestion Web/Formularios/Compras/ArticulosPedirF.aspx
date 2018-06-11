<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ArticulosPedirF.aspx.cs" Inherits="Gestion_Web.Formularios.Compras.ArticulosPedirF" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

     <div class="main">

        <div>
            <div class="stat">                        
                        <h5><i class="icon-map-marker"></i> Compras > Articulos a Pedir</h5>
                    </div>
          
            <div class="col-md-12 col-xs-12">
                <div class="widget widget-table">
                    <div class="widget-header">
                        <i class="icon-th-list" style="width: 2%"></i>
                        <h3 style="width: 75%">Articulos a Pedir
                        </h3>
                        <h3>
                            <asp:Label ID="lblSaldo" runat="server" Style="text-align: right" Text="" ForeColor="#666666" Font-Bold="true"></asp:Label>
                        </h3>
                    </div>
                    <div class="widget-content">
                        <div class="panel-body">

                            <div class="table-responsive">
                                <a class="btn btn-info" style="display: none" data-toggle="modal" id="abreDialog" href="#modalFacturaDetalle">Agregar Tipo Cliente</a>
                                <table class="table table-striped table-bordered table-hover" id="dataTables-example">
                                    <thead>
                                        <tr>
                                            <th>Codigo</th>
                                            <th>Descripción</th>
                                            <th>Fecha</th>
                                            
                                            <th></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:PlaceHolder ID="phOrdenes" runat="server"></asp:PlaceHolder>
                                    </tbody>
                                </table>

                            </div>


                        </div>


                        <!-- /.content -->

                    </div>

                </div>
            </div>


        </div>
    </div>

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

    <!-- Page-Level Plugin Scripts - Tables -->
    <script src="../../Scripts/plugins/dataTables/jquery.dataTables.min.js"></script>
    <script src="../../Scripts/plugins/dataTables/custom.tables.js"></script>
    <link href="../../Scripts/plugins/dataTables/dataTables.css" rel="stylesheet" />

    <%--<script src="//code.jquery.com/jquery-1.9.1.js"></script>--%>
    <script src="//code.jquery.com/ui/1.10.4/jquery-ui.js"></script>

    

</asp:Content>
