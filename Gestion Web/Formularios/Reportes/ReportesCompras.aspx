<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportesCompras.aspx.cs" Inherits="Gestion_Web.Formularios.Reportes.ReportesCompras" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">
        <div>
            <div class="col-md-12">
                <div class="row">
                    <div class="col-md-12 col-xs-12">
                        <div class="widget stacked">
                            <div class="widget-header">
                                <i class="icon-wrench"></i>
                                <h3>Herramientas</h3>
                            </div>
                            <div class="widget-content">
                                <table style="width: 100%;">
                                    <tr>
                                        <td style="width: 20%">
                                            <asp:PlaceHolder runat="server" ID="phAccion">
                                                <div class="btn-group">
                                                    <button type="button" class="btn btn-success dropdown-toggle" data-toggle="dropdown" id="btnAccion" runat="server">Accion    <span class="caret"></span></button>
                                                    <ul class="dropdown-menu">
                                                        <li class="dropdown-submenu dropdown-menu-right"><a tabindex="-1" href="#">Articulos por Proveedor</a>
                                                            <ul class="dropdown-menu">
                                                                <li>
                                                                    <asp:LinkButton ID="lbtnReporteArticulosPorProveedor" runat="server" OnClick="lbtnReporteArticulosPorProveedor_Click">
                                                                    <i class="fa fa-file-excel-o" aria-hidden="true"></i>
                                                                    &nbsp Exportar
                                                                    </asp:LinkButton>
                                                                </li>
                                                                <li>
                                                                    <asp:LinkButton ID="lbtnReporteArticulosPorProveedorPDF" runat="server" OnClick="lbtnReporteArticulosPorProveedorPDF_Click">
                                                                    <i class="fa fa-file-pdf-o" aria-hidden="true"></i>
                                                                    &nbsp Imprimir
                                                                    </asp:LinkButton>
                                                                </li>
                                                            </ul>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </asp:PlaceHolder>
                                        </td>
                                        <td style="width: 95%">
                                            <h5>
                                                <asp:Label runat="server" ID="lblParametros" Text="" ForeColor="#cccccc"></asp:Label>
                                            </h5>
                                        </td>
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
                </div>

                <asp:PlaceHolder runat="server" ID="phSucursal" Visible="false">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="widget big-stats-container stacked">
                                <div class="widget-content">
                                    <div id="big_stats" class="cf">
                                        <div class="stat">
                                            <h5>Sucursal</h5>
                                            <asp:Label ID="lblSucursal" runat="server" Text="" class="value"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:PlaceHolder>

                <div class="row">
                    <div class="col-md-12 col-xs-12">
                        <div class="widget stacked">
                            <div class="widget-header">
                                <i class="icon-list"></i>
                                <h3>Top´s</h3>
                            </div>
                            <div class="widget-content">
                                <div class="bs-example">
                                    <ul id="myTab" class="nav nav-tabs">
                                        <li class="active"><a href="#home" data-toggle="tab">Articulos</a></li>
                                    </ul>
                                    <div class="tab-content">
                                        <div class="tab-pane fade active in" id="home">
                                            <div class="col-md-6">
                                                <div class="widget stacked widget-table">
                                                    <div class="widget-header">
                                                        <span class="icon-list-alt"></span>
                                                        <h3>Cantidad</h3>
                                                    </div>
                                                    <div class="widget-content">
                                                        <table class="table table-bordered table-striped">
                                                            <thead>
                                                                <tr>
                                                                    <th style="text-align: left; width: 30%">Codigo</th>
                                                                    <th style="text-align: left; width: 40%">Descripcion</th>
                                                                    <th style="text-align: right; width: 30%">Cantidad</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                <asp:PlaceHolder ID="phTopArticulosCantidad" runat="server"></asp:PlaceHolder>
                                                            </tbody>
                                                        </table>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
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
                            <asp:UpdatePanel ID="UpdatePanelBusqueda" UpdateMode="Always" runat="server">
                                <ContentTemplate>
                                    <fieldset>
                                        <div class="form-group">
                                            <label class="col-md-4">Desde</label>
                                            <div class="col-md-6">
                                                <asp:TextBox ID="txtFechaDesde" runat="server" class="form-control"></asp:TextBox>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaDesde" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4">Hasta</label>
                                            <div class="col-md-6">
                                                <asp:TextBox ID="txtFechaHasta" runat="server" class="form-control"></asp:TextBox>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaHasta" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4">Cod. Proveedor</label>
                                            <div class="col-md-6">
                                                <asp:TextBox ID="txtProveedor" class="form-control" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="btnBuscarProveedor" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" OnClick="btnBuscarProveedor_Click" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4">Proveedor</label>
                                            <div class="col-md-6">
                                                <asp:DropDownList ID="DropListProveedor" runat="server" class="form-control"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListProveedor" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </fieldset>
                                </ContentTemplate>
                                <Triggers>
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:LinkButton ID="lbtnBuscar" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnBuscar_Click" ValidationGroup="BusquedaGroup" />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <link href="../../css/pages/reports.css" rel="stylesheet">
    <script src="../../Scripts/jquery-1.10.2.js"></script>
    <script src="../../Scripts/bootstrap.min.js"></script>
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

    <script>
        function pageLoad() {
            $("#<%= txtFechaDesde.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });

            $("#<%= txtFechaHasta.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        }
    </script>
    <script>
        $(function () {
            $("#<%= txtFechaDesde.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

        $(function () {
            $("#<%= txtFechaHasta.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });
    </script>

</asp:Content>
