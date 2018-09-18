<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AceptarMercaderia.aspx.cs" Inherits="Gestion_Web.Formularios.Facturas.AceptarMercaderia" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">

        <div>
            <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Always">
                <ContentTemplate>
                    <div class="col-md-12 col-xs-12">
                        <div class="widget stacked">

                            <div class="widget-header">
                                <i class="icon-wrench"></i>
                                <h3>Herramientas</h3>
                            </div>
                            <!-- /widget-header -->

                            <div class="widget-content">
                                <div id="validation-form" role="form" class="form-horizontal col-md-8">
                                    <fieldset>
                                        <div class="form-group">
                                            <label for="name" class="col-md-2">Sucursal</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="ListSucursal" class="form-control" runat="server" AutoPostBack="True"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-2">Pto Venta</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="ListPtoVenta" class="form-control" runat="server" AutoPostBack="True"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-2">Cod. Proveedor</label>
                                            <div class="col-md-3">
                                                <asp:TextBox ID="txtCodProveedor" class="form-control" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="btnBuscarCodigoProveedor" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-2">Proveedor</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="ListProveedor" class="form-control" runat="server" AutoPostBack="True"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                                                    <ProgressTemplate>
                                                        <i class="fa fa-spinner fa-spin"></i><span>&nbsp;&nbsp;Cargando artículos del Proveedor. Por favor aguarde.</span>
                                                    </ProgressTemplate>
                                                </asp:UpdateProgress>
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="name" class="col-md-2">Numero</label>

                                            <div class="col-md-2">
                                                <asp:TextBox ID="txtPVenta" MaxLength="4" runat="server" class="form-control" disabled onchange="completar4Ceros(this, this.value)"></asp:TextBox>
                                            </div>

                                            <div class="col-md-3">
                                                <asp:TextBox ID="txtNumero" MaxLength="8" runat="server" class="form-control" disabled onchange="completar8Ceros(this, this.value)"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-2">Fecha</label>

                                            <div class="col-md-3">
                                                <div class="input-group">
                                                    <span class="input-group-addon"><i class="icon-calendar"></i></span>
                                                    <asp:TextBox ID="txtFecha" runat="server" class="form-control"></asp:TextBox>
                                                </div>

                                            </div>
                                            <div class="col-md-2">
                                                <asp:RequiredFieldValidator ControlToValidate="txtFecha" ID="RequiredFieldValidator42" runat="server" ErrorMessage="*" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>

                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-2">Fecha Entrega</label>

                                            <div class="col-md-3">
                                                <div class="input-group">
                                                    <span class="input-group-addon"><i class="icon-calendar"></i></span>
                                                    <asp:TextBox ID="txtFechaEntrega" runat="server" class="form-control"></asp:TextBox>
                                                </div>

                                            </div>
                                            <div class="col-md-2">
                                                <asp:RequiredFieldValidator ControlToValidate="txtFechaEntrega" ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>

                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-2">Observaciones</label>
                                            <div class="col-md-6">
                                                <asp:TextBox ID="txtObservaciones" runat="server" class="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                            </div>
                                        </div>
                                        <%--<asp:Panel ID="Panel1" Visible="true" runat="server" class="col-md-12" Style="padding: 0px; margin-left: -1%;">
                                            <table class="table table-bordered ">
                                                <thead>
                                                    <tr>
                                                        <th>Codigo</th>
                                                        <th>Descripcion</th>
                                                        <th>Costo</th>
                                                        <th>Cantidad</th>
                                                        <th></th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr>
                                                        <td style="width: 20%">
                                                            <div class="form-group">
                                                                <div class="col-md-12">
                                                                    <div class="input-group">
                                                                        <asp:TextBox ID="txtCodigo" runat="server" class="form-control"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </td>
                                                        <td style="width: 30%">
                                                            <div style="width: 100%">
                                                                <asp:TextBox ID="txtDescripcion" runat="server" class="form-control" Style="text-align: right"></asp:TextBox>
                                                            </div>
                                                        </td>
                                                        <td style="width: 10%">
                                                            <asp:TextBox ID="txtPrecio" runat="server" class="form-control" Style="text-align: right" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 10%">
                                                            <asp:TextBox ID="txtCantidad" runat="server" class="form-control" Style="text-align: right" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 5%">
                                                            <asp:LinkButton ID="lbtnAgregarArticuloASP" class="btn btn-info" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" Visible="true" />
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </asp:Panel>--%>
                                    </fieldset>
                                </div>

                                <table class="table table-striped table-bordered">
                                    <thead>
                                        <tr>
                                            <th>Codigo</th>
                                            <th>Descripcion</th>
                                            <th>Cantidad</th>
                                            <th>Cantidad Recibida</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:PlaceHolder ID="phProductos" runat="server"></asp:PlaceHolder>
                                    </tbody>
                                </table>
                                <br />
                                <div class="btn-toolbar">
                                    <div class="btn-group">
                                        <asp:Button ID="btnAgregar" type="button" runat="server" Text="Guardar" class="btn btn-success"  />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
