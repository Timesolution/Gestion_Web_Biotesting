<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportesTrazabilidad.aspx.cs" Inherits="Gestion_Web.Formularios.Reportes.ReportesTrazabilidad" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="main">
        <div>
            <div>
                <div class="col-md-12 col-xs-12">
                    <div class="widget stacked">
                        <div class="stat">
                            <h5><i class="icon-map-marker"></i>Reportes > Trazabilidad </h5>
                        </div>
                        <div class="widget-header">
                            <i class="icon-wrench"></i>
                            <h3>Herramientas</h3>
                        </div>
                        <div class="widget-content">
                            <table style="width: 100%">
                                <tr>

                                    <td style="width: 20%">
                                        <asp:PlaceHolder ID="phAcciones" runat="server">
                                            <div class="btn-group">
                                                <button type="button" class="btn btn-success dropdown-toggle" data-toggle="dropdown" id="btnAccion" runat="server">Accion    <span class="caret"></span></button>
                                                <ul class="dropdown-menu">
                                                    <li>
                                                        <asp:LinkButton ID="lbtnImprimir" runat="server" OnClick="lbtnImprimir_Click">Imprimir</asp:LinkButton>
                                                    </li>
                                                </ul>
                                            </div>
                                        </asp:PlaceHolder>

                                    </td>

                                    <td style="width: 95%">
                                        <h5>
                                            <asp:Label runat="server" ID="lblParametros" Text="" ForeColor="#363636"></asp:Label>
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

            <asp:PlaceHolder ID="phCantidadDeRegistros" runat="server" Visible="false">
                <div class="col-md-12">
                    <div class="widget big-stats-container stacked">
                        <div class="widget-content">
                            <div id="big_stats" class="cf">

                                <div class="stat">
                                    <h3>Cantidad de Registros</h3>
                                    <span class="value">
                                        <asp:Label runat="server" ID="lbCantidadRegistros"></asp:Label>
                                    </span>
                                </div>

                                <div class="stat">
                                    <h3><asp:Label runat="server" ID="lbNombreColumnaTrazaUnidadMedida"></asp:Label></h3>
                                    <span class="value">
                                        <asp:Label runat="server" ID="lbCantidadTotal"></asp:Label>
                                    </span>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
            </asp:PlaceHolder>

            <div>
                <div class="col-md-12 col-xs-12">
                    <div class="widget stacked">

                        <div class="widget-header">
                            <i class="icon-road"></i>
                            <h3>Trazabilidad</h3>
                        </div>
                        <div class="widget-content">

                            <div class="form-group">
                                <table class="table table-striped table-bordered" id="dataTables-example">
                                    <thead>
                                        <tr>
                                            <th>#</th>
                                            <th>Codigo</th>
                                            <th>Descripcion</th>
                                            <asp:PlaceHolder ID="phTabla" runat="server"></asp:PlaceHolder>
                                            <th>Estado</th>
                                            <th>Sucursal</th>
                                            <th></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:PlaceHolder ID="phTrazabilidad" runat="server"></asp:PlaceHolder>
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
                                <asp:UpdatePanel ID="UpdatePanel4" UpdateMode="Always" runat="server">
                                    <ContentTemplate>
                                        <div class="form-group">
                                            <label class="col-md-4">Grupo</label>
                                            <div class="col-md-6">
                                                <asp:DropDownList ID="DropListGrupo" runat="server" class="form-control" OnSelectedIndexChanged="DropListGrupo_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="UpdatePanel4" runat="server">
                                                    <ProgressTemplate>
                                                        <h2>
                                                            <i class="fa fa-spinner fa-spin"></i>
                                                        </h2>
                                                    </ProgressTemplate>
                                                </asp:UpdateProgress>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListGrupo" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                            </div>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4">Sucursal</label>
                                            <div class="col-md-6">
                                                <asp:DropDownList ID="DropListSucursal" runat="server" class="form-control"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListSucursal" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                            </div>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4" style="display: none;">Desc Articulo</label>
                                            <div class="col-md-6">
                                                <asp:TextBox ID="txtDescArticulo" class="form-control" runat="server" Style="display: none;"></asp:TextBox>
                                            </div>

                                            <div class="col-md-2">
                                                <asp:LinkButton ID="btnBuscarCod" runat="server" class="btn btn-info" Style="display: none;" Text="<span class='shortcut-icon icon-search'></span>" OnClick="btnBuscarCod_Click" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4">Articulo</label>
                                            <div class="col-md-6">
                                                <asp:DropDownList ID="DropListArticulos" runat="server" class="form-control"></asp:DropDownList>
                                                <!-- /input-group -->
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4">Estado</label>
                                            <div class="col-md-6">
                                                <asp:DropDownList ID="DropListEstado" runat="server" class="form-control">
                                                    <asp:ListItem Text="TODOS" Value="-1" />
                                                    <asp:ListItem Text="EN STOCK" Value="1" />
                                                    <asp:ListItem Text="VENDIDO" Value="2" />
                                                    <asp:ListItem Text="TOMADA PEDIDO" Value="3" />
                                                    <asp:ListItem Text="DEVUELTO" Value="4" />
                                                </asp:DropDownList>
                                                <!-- /input-group -->
                                            </div>
                                        </div>
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
    </div>

    <link href="../../css/pages/reports.css" rel="stylesheet">

    <script src="../../Scripts/plugins/flot/jquery.flot.js"></script>
    <script src="../../Scripts/plugins/flot/jquery.flot.pie.js"></script>
    <script src="../../Scripts/plugins/flot/jquery.flot.orderBars.js"></script>
    <script src="../../Scripts/plugins/flot/jquery.flot.resize.js"></script>

    <script src="../../Scripts/Application.js"></script>

    <script src="../../Scripts/charts/pie.js"></script>
    <script src="../../Scripts/charts/bar.js"></script>

    <script src="//code.jquery.com/ui/1.10.4/jquery-ui.js"></script>

    <!-- Page-Level Plugin Scripts - Tables -->
    <script src="//cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>
    <script src="//cdn.datatables.net/plug-ins/1.10.9/sorting/date-eu.js"></script>
    <script src="../../Scripts/plugins/dataTables/custom.tables.js"></script>
    <link href="//cdn.datatables.net/1.10.2/css/jquery.dataTables.css" rel="stylesheet" />

    <script>

        $('#dataTables-example').dataTable({
            "bLengthChange": false,
            "pageLength": 15,
            "bFilter": true,
            "bInfo": false,
            "bAutoWidth": false,
            "language": {
                "search": "Buscar:"
            }
        });


        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endReq);
        function endReq(sender, args) {
            $('#dataTables-example').dataTable();
        }
    </script>
</asp:Content>
