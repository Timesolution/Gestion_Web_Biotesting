<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" CodeBehind="ImportarClientes.aspx.cs" Inherits="Gestion_Web.Formularios.Clientes.ImportarClientes" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

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
                                <label class="col-md-2">Archivo a Importar</label>
                                <div class="col-md-3">
                                    <asp:FileUpload ID="FileUpload1" runat="server" />
                                    <asp:Label runat="server" ID="lbDropListEmpresaError" Style="display: none" Text="Seleccione un item." ForeColor="Red" Font-Bold="true"></asp:Label>
                                </div>
                            </div>
                        </div>

                        <div class="form-horizontal col-md-12">
                            <div class="col-md-3">
                                <asp:Button ID="btnImportarUsuarios" runat="server" Text="Importar Usuarios" class="btn btn-success" OnClick="lbtnImportarUsuarios_Click" />
                            </div>
                            <div class="col-md-3">
                                <asp:Button ID="btnImportarClientes" runat="server" Text="Importar Clientes" class="btn btn-success" OnClick="lbtnImportarClientes_Click" />
                            </div>
                        </div>

                        <rsweb:ReportViewer ID="ReportViewer1" Visible="false" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="80%">
                            <LocalReport ReportEmbeddedResource="Gestion_Web.Formularios.Cobros.Cobros.rdlc">
                            </LocalReport>
                        </rsweb:ReportViewer>
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
