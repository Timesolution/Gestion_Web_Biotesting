<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EntregasF.aspx.cs" Inherits="Gestion_Web.Formularios.Compras.Entregas.EntregasF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-md-12 col-xs-12">
        <div class="widget stacked">
            <div class="stat">
                <h5><i class="icon-map-marker"></i>Compras > Entregas</h5>
            </div>
            <div class="widget-header">
                <i class="icon-wrench"></i>
                <h3>Herramientas</h3>
            </div>
            <div class="widget-content">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 20%">
                            <div class="btn-group">
                                <button type="button" class="btn btn-success dropdown-toggle" data-toggle="dropdown" id="btnAccion" runat="server">Accion<span class="caret"></span></button>
                                <ul class="dropdown-menu">
                                </ul>
                            </div>
                        </td>
                        <td style="width: 65%">
                            <h5>
                                <asp:Label runat="server" ID="lblParametros" Text="" ForeColor="#cccccc"></asp:Label>
                            </h5>
                        </td>
                        <td style="width: 5%"></td>
                        <td style="width: 5%">
                            <div class="shortcuts" style="height: 100%">
                                <a class="btn btn-primary" data-toggle="modal" href="#modalBusqueda" style="width: 100%">
                                    <i class="shortcut-icon icon-filter"></i>
                                </a>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>

    <div class="col-md-12 col-xs-12">
        <div class="widget widget-table">
            <div class="widget-header">
                <i class="icon-th-list" style="width: 2%"></i>
                <h3 style="width: 75%">Compras
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
                                    <th style="width: 10%">Fecha</th>
                                    <th style="width: 10%">Fecha Entrega</th>
                                    <th style="width: 15%">Numero</th>
                                    <th style="width: 15%">Proveedor</th>
                                    <th style="width: 20%">Sucursal</th>
                                    <th style="width: 15%">Estado</th>
                                    <th style="width: 15%"></th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:PlaceHolder ID="phEntregas" runat="server"></asp:PlaceHolder>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="modalBusqueda" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Busqueda</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <fieldset>
                            <div class="form-group">
                                <label class="col-md-3">Fecha Orden Compra</label>
                                <div class="col-md-3">
                                    <asp:TextBox ID="txtFechaDesde" placeholder="Desde" runat="server" class="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <asp:TextBox ID="txtFechaHasta" placeholder="Hasta" runat="server" class="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-1">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaDesde" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-1">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaHasta" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="name" class="col-md-3">Cod. Proveedor</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtCodProveedor" class="form-control" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-md-5">
                                    <asp:LinkButton ID="btnBuscarCodigoProveedor" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" OnClick="btnBuscarCodigoProveedor_Click" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="name" class="col-md-3">Proveedor</label>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="ListProveedor" class="form-control" runat="server" AutoPostBack="True"></asp:DropDownList>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbtnBuscar" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" ValidationGroup="BusquedaGroup" OnClick="lbtnBuscar_Click" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
