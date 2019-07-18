<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ImportarListaDePrecio.aspx.cs" Inherits="Gestion_Web.Formularios.Articulos.ImportarListaDePrecio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-md-12">
                <div class="widget stacked">
                    <div class="stat">
                        <h5><i class="icon-map-marker"></i>Maestros > Lista de Precios > Importar Lista de Precios</h5>
                    </div>
                    <div class="widget-header">
                        <i class="icon-list-alt"></i>
                        <h3>Importar Lista de Precio</h3>
                    </div>
                    <div class="widget-content">
                        <div class="form-horizontal col-md-12">
                            <div class="form-group">
                                <label class="col-md-2">Importar lista</label>
                                <div class="col-md-3">
                                    <asp:FileUpload ID="FileUpload1" runat="server" />
                                    <asp:Label runat="server" ID="lbDropListEmpresaError" Style="display: none" Text="Seleccione un item." ForeColor="Red" Font-Bold="true"></asp:Label>
                                </div>
                                <div class="col-md-3">
                                    <asp:Button ID="btnImportarPedido" runat="server" Text="Importar .csv" class="btn btn-success" OnClick="btnImportarListaDePrecio_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script src="../../Scripts/libs/jquery-1.9.1.min.js"></script>
    <script src="../../Scripts/libs/bootstrap.min.js"></script>
    <script src="../../Scripts/plugins/msgbox/jquery.msgbox.min.js"></script>
    <script src="../../Scripts/demo/notifications.js"></script>
</asp:Content>
