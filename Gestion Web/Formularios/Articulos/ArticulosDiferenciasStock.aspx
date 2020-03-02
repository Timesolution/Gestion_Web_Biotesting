<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ArticulosDiferenciasStock.aspx.cs" Inherits="Gestion_Web.Formularios.Articulos.ArticulosDiferenciasStock" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container">
        <div class="row">
            <div class="col-md-12">
                <div class="widget stacked">
                    <div class="stat">
                        <h5><i class="icon-map-marker"></i>Maestros > Articulos > Articulos Diferencias Stock</h5>
                    </div>
                    <div class="widget-header">
                        <i class="icon-pencil"></i>
                        <h3>Importar Excel</h3>
                    </div>
                    <div class="widget-content">
                        <div class="form-horizontal col-md-12">
                            <fieldset>
                                <asp:UpdatePanel runat="server" ID="upArticulosDiferenciasStock">
                                    <ContentTemplate>
                                        <div class="form-group">
                                            <label class="col-md-2">Empresa</label>
                                            <div class="col-md-3">
                                                <asp:DropDownList ID="ListEmpresa" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ListEmpresa_SelectedIndexChanged" />
                                            </div>
                                            <div class="col-md-2">
                                                <asp:RequiredFieldValidator ErrorMessage="* Seleccione Empresa" InitialValue="0" ControlToValidate="ListEmpresa" runat="server" ForeColor="Red" Font-Bold="true" ValidationGroup="ArticulosDiferenciasStockGroup" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-2">Sucursal</label>
                                            <div class="col-md-3">
                                                <asp:DropDownList ID="ListSucursal" runat="server" class="form-control"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:RequiredFieldValidator ErrorMessage="* Seleccione Sucursal" InitialValue="0" ControlToValidate="ListSucursal" runat="server" ForeColor="Red" Font-Bold="true" ValidationGroup="ArticulosDiferenciasStockGroup" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-2">Seleccione Archivo</label>
                                            <div class="col-md-3">
                                                <asp:FileUpload ID="FileUpload" runat="server" accept="application/vnd.ms-excel, application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" />
                                            </div>
                                            <div class="col-md-4">
                                                <asp:RegularExpressionValidator ID="uplValidator" runat="server" ControlToValidate="FileUpload" ErrorMessage="Debe subir archivos excel" ValidationExpression="^.*\.(xls|xlsx)$" ForeColor="Red" Font-Bold="true" ValidationGroup="ArticulosDiferenciasStockGroup"></asp:RegularExpressionValidator>
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <div class="form-group">
                                    <div class="col-md-4">
                                        <asp:Button runat="server" ID="btnGenerarDiferenciasStock" Text="Generar Diferencias" class="btn btn-success" OnClick="btnGenerarDiferenciasStock_Click" ValidationGroup="ArticulosDiferenciasStockGroup" />
                                    </div>
                                </div>
                            </fieldset>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
