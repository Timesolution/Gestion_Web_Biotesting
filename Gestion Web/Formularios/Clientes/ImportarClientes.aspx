<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" CodeBehind="ImportarClientes.aspx.cs" Inherits="Gestion_Web.Formularios.Clientes.ImportarClientes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-md-12">
                <div class="widget stacked">
                    <div class="stat">
                        <h5><i class="icon-map-marker"></i>Maestros > Lista de Clientes > Importar Lista de Clientes</h5>
                    </div>
                    <div class="widget-header">
                        <i class="icon-list-alt"></i>
                        <h3>Importar Clientes</h3>
                    </div>
                    <div class="widget-content">
                        <div class="form-horizontal col-md-12">

                            <div class="form-group">
                                <label class="col-md-2">Provincias</label>
                                <div class="col-md-3">
                                    <asp:DropDownList ID="dropList_Provincias" class="form-control" runat="server"></asp:DropDownList>
                                    <asp:Label runat="server" ID="Label1" Style="display: none" Text="Seleccione un item." ForeColor="Red" Font-Bold="true"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-2">Vendedores</label>
                                <div class="col-md-3">
                                    <asp:DropDownList ID="dropList_Vendedores" class="form-control" runat="server"></asp:DropDownList>
                                </div>
                            </div>

                            <div class="form-group">
                                <label class="col-md-2">Importar clientes</label>
                                <div class="col-md-3">
                                    <asp:FileUpload ID="FileUpload1" runat="server" />
                                    <asp:Label runat="server" ID="lbDropListEmpresaError" Style="display: none" Text="Seleccione un item." ForeColor="Red" Font-Bold="true"></asp:Label>
                                </div>
                            </div>

                        </div>



                        <div class="form-horizontal col-md-12">
                            <div class="col-md-3">
                                <asp:Button ID="btnImportarPedido" runat="server" Text="Importar .csv" class="btn btn-success" OnClick="lbtnImportarClientes_Click" />
                            </div>
                        </div>
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
    <script src="//cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>
    <script src="../../Scripts/plugins/dataTables/custom.tables.js"></script>
    <link href="//cdn.datatables.net/1.10.2/css/jquery.dataTables.css" rel="stylesheet" />
</asp:Content>
