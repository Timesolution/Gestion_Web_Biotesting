<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StockHistoricoF.aspx.cs" Inherits="Gestion_Web.Formularios.Articulos.StockHistoricoF" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">

        <div class="container">

                <div class="col-md-12 col-xs-12 hidden-print">
                    <div class="widget stacked">

                        <div class="widget-header">
                            <i class="icon-wrench"></i>
                            <h3>Stock Historico
                               <asp:Label ID="labelNombre" runat="server" Text="" Font-Bold="true"></asp:Label>
                               <asp:Label ID="labelSucursal" runat="server" Text="" Font-Bold="true"></asp:Label>
                            </h3>
                        </div>
                        <div class="widget-content">
                            <div class="col-md-12 col-xs-12">
                                <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Always" runat="server">
                                    <ContentTemplate>
                                        <a class="btn btn-info" style="display: none" data-toggle="modal" id="abreDialog" href="#modalFacturaDetalle">Agregar Tipo Cliente</a>



                                        <table class="table table-striped table-bordered">
                                            <thead>
                                                <tr>
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

                            </div>


                            <!-- /.content -->

                        </div>

                    </div>
                </div>
            </div>
        </div>

<%--    <div id="modalStock" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Agregar Stock</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                    <div class="form-group">
                        <label for="validateSelect" class="col-md-4">Sucursal</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtSucursal" runat="server" class="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-4">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="El campo es obligatorio " ControlToValidate="txtSucursal" ValidationGroup="StockGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="validateSelect" class="col-md-4">Articulo</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtArticulo" runat="server" class="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-4">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="El campo es obligatorio " ControlToValidate="txtArticulo" ValidationGroup="StockGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="form-group">
                        <label for="validateSelect" class="col-md-4">Stock</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtStock" runat="server" class="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-4">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="El campo es obligatorio " ControlToValidate="txtStock" ValidationGroup="StockGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnAgregarEstado" runat="server" Text="Guardar" class="btn btn-success" ValidationGroup="StockGroup" />
                </div>

            </div>
        </div>
    </div>--%>
</asp:Content>

