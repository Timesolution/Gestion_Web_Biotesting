<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ImportarClientes.aspx.cs" Inherits="Gestion_Web.Formularios.Clientes.ImportarClientes" %>

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


    <script src="../../Scripts/libs/jquery-1.9.1.min.js"></script>
    <script src="../../Scripts/libs/bootstrap.min.js"></script>
    <script src="../../Scripts/plugins/msgbox/jquery.msgbox.min.js"></script>
    <script src="../../Scripts/demo/notifications.js"></script>
</asp:Content>
