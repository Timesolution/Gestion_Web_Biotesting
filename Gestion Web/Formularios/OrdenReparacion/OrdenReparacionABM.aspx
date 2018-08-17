﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OrdenReparacionABM.aspx.cs" Inherits="Gestion_Web.Formularios.OrdenReparacion.OrdenReparacionABM" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../../Scripts/jquery-1.10.2.js"></script>
    <script src="../../Scripts/bootstrap.min.js"></script>
    <%--<script src="../../Scripts/plugins/metisMenu/jquery.metisMenu.js"></script>--%>

    <script src="../../Scripts/libs/jquery-1.9.1.min.js"></script>
    <script src="../../Scripts/libs/jquery-ui-1.10.0.custom.min.js"></script>
    <script src="../../Scripts/libs/bootstrap.min.js"></script>

    <script src="../../Scripts/plugins/hoverIntent/jquery.hoverIntent.minified.js"></script>
    <script src="../../Scripts/plugins/lightbox/jquery.lightbox.min.js"></script>

    <script src="../../Scripts/Application.js"></script>

    <script src="../../Scripts/demo/gallery.js"></script>

    <script src="../../Scripts/plugins/msgGrowl/js/msgGrowl.js"></script>
    <script src="../../Scripts/plugins/lightbox/jquery.lightbox.min.js"></script>
    <script src="../../Scripts/plugins/msgbox/jquery.msgbox.min.js"></script>
    <script src="../../Scripts/demo/notifications.js"></script>

    <div class="container">
        <div class="row">

            <div class="col-md-12">

                <div class="widget stacked">
                    <div class="stat">
                        <h5><i class="icon-map-marker"></i>Maestro > Ventas > Orden Reparacion</h5>
                        <asp:Label ID="lblFiltroAnterior" runat="server" Style="display: none;"></asp:Label>
                    </div>
                    <div class="widget-header">
                        <i class="icon-pencil"></i>
                        <h3>Orden de reparacion</h3>

                    </div>
                    <!-- /.widget-header -->
                    <div class="widget-content">
                        <div class="bs-example">
                            <div class="tab-content">
                                <div class="tab-pane fade active in" id="home">
                                    <div id="validation-form" role="form" class="form-horizontal col-md-10">
                                        <fieldset>
                                            <div class="form-group">
                                                <label for="name" class="col-md-4">Numero de orden</label>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="txtNumeroOrden" runat="server" Enabled="false"></asp:TextBox>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtNumeroOrden" ValidationGroup="StoreGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <%--<div class="form-group">
                                                <label for="name" class="col-md-4">Fecha</label>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="txtFecha" runat="server" class="form-control"></asp:TextBox>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtFecha" ValidationGroup="StoreGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>--%>
                                            <div class="form-group">
                                                <label for="name" class="col-md-4">Fecha</label>
                                                <div class="col-md-4">
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="icon-calendar"></i></span>
                                                        <asp:TextBox ID="txtFecha" class="form-control" runat="server" Enabled="false"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFecha" ValidationGroup="StoreGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                            </div>

                                            <div class="form-group">
                                                <label for="name" class="col-md-4">Numero de Serie</label>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="txtNumeroSerie" runat="server" class="form-control"></asp:TextBox>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtNumeroSerie" ValidationGroup="StoreGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="name" class="col-md-4">Sucursal Origen</label>
                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="ListSucursal" runat="server" class="form-control" Enabled="false"></asp:DropDownList>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="ListSucursal" ValidationGroup="StoreGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="name" class="col-md-4">Numero de PRP/Factura</label>
                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="ListNumeroPRPoFactura" runat="server" class="form-control" Enabled="false"></asp:DropDownList>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="ListNumeroPRPoFactura" ValidationGroup="StoreGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="name" class="col-md-4">Producto</label>
                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="ListProductos" runat="server" class="form-control"></asp:DropDownList>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="ListProductos" InitialValue="-1" ValidationGroup="StoreGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-4">Fecha de Compra</label>
                                                <div class="col-md-4">
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="icon-calendar"></i></span>
                                                        <asp:TextBox ID="txtFechaCompra" runat="server" class="form-control" Enabled="false"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaCompra" ValidationGroup="StoreGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="name" class="col-md-4">Cliente</label>
                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="ListCliente" runat="server" class="form-control" Enabled="false"></asp:DropDownList>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="ListCliente" ValidationGroup="StoreGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-4">Celular</label>
                                                <div class="col-md-2">
                                                    <div class="input-group">
                                                        <span class="input-group-addon">0</span>
                                                        <asp:TextBox ID="txtCodArea" runat="server" class="form-control" placeholder="Cod. Area" MaxLength="4"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:TextBox ID="txtCelular" runat="server" class="form-control" placeholder="Ej.: 1111 2222" MaxLength="8"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="name" class="col-md-4">Descripcion de la Falla</label>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="txtDescripcionFalla" runat="server" class="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtDescripcionFalla" ValidationGroup="StoreGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-4">Plazo Limite de Reparacion</label>
                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="DropListPlazoLimite" runat="server" class="form-control">
                                                        <asp:ListItem Value="30">30</asp:ListItem>
                                                        <asp:ListItem Value="60">60</asp:ListItem>
                                                        <asp:ListItem Value="90">90</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="name" class="col-md-4">Autoriza</label>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="txtAutoriza" runat="server" class="form-control"></asp:TextBox>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtAutoriza" ValidationGroup="StoreGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="col-md-8">
                                                <asp:LinkButton ID="btnAgregar" runat="server" Text="Agregar" class="btn btn-success" OnClick="btnAgregar_Click" ValidationGroup="StoreGroup" Visible="false"></asp:LinkButton>
                                                <asp:LinkButton ID="btnGuardar" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnGuardar_Click" ValidationGroup="StoreGroup" Visible="false"></asp:LinkButton>
                                                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" class="btn btn-default" PostBackUrl="FacturasF.aspx" />
                                            </div>
                                        </fieldset>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script>
        $(function () {
            $("#<%= txtFechaCompra.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

        $(function () {
            $("#<%= txtFecha.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });
    </script>
</asp:Content>