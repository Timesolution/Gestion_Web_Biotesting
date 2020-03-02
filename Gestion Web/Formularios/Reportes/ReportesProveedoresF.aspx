<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportesProveedoresF.aspx.cs" Inherits="Gestion_Web.Formularios.Reportes.ReportesProveedoresF" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="main">

        <div >

            <div >
                <div class="col-md-12 col-xs-12">
                    <div class="widget stacked">
                        <div class="stat">                        
                            <h5><i class="icon-map-marker"></i> Reportes > Impagas > Proveedores </h5>
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
                                                        <asp:LinkButton ID="lbtnImprimir" runat="server" OnClick="lbtnImprimir_Click">Imprimir reporte</asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lbtnExportar" runat="server" OnClick="lbtnExportar_Click">Exportar reporte</asp:LinkButton>
                                                    </li>
                                                </ul>
                                            </div>
                                        </asp:PlaceHolder>
                                        <!-- /btn-group -->

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
                        <!-- /widget-content -->

                    </div>
                    <!-- /widget -->
                </div>
            </div>

            <div >

                <div class="col-md-12">

                    <div class="widget big-stats-container stacked">

                        <div class="widget-content">

                            <div id="big_stats" class="cf">
                                
                                <div class="stat">
                                    <h4>Documentos Impagos ($)</h4>
                                    <asp:Label ID="lblDocumentosImpagosPesos" runat="server" Text="" class="value"></asp:Label>
                                </div>
                                <!-- .stat -->
                               
                            </div>

                        </div>
                        <!-- /widget-content -->

                    </div>
                    <!-- /widget -->

                </div>
                <!-- /span12 -->

            </div>
            <!-- /row -->

            <div >
                <div class="col-md-12 col-xs-12">
                    <div class="widget stacked">

                        <div class="widget-header">
                            <i class="icon-list"></i>
                            <h3>Ranking Impagas</h3>
                        </div>
                        <div class="widget-content">
                            <div class="widget stacked widget-table">
                                <table class="table table-bordered table-striped">
                                    <thead>
                                        <tr>
                                            <th style="text-align: left; width: 80%">Proveedor</th>
                                            <th style="text-align: left; width: 15%">Importe</th>
                                            <th style="text-align: right; width: 5%"></th>
                                        </tr>

                                    </thead>

                                    <tbody>
                                        <asp:PlaceHolder ID="phTopProveedores" runat="server"></asp:PlaceHolder>
                                    </tbody>

                                </table>
                            </div>
                            <!-- /widget-content -->

                        </div>
                    <!-- /widget -->
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
                                        <label class="col-md-4">Hasta</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtFechaHasta" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaHasta" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                        <!-- /input-group -->
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Sucursal</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListSucursal" runat="server" class="form-control" disabled></asp:DropDownList>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaHasta" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                        <!-- /input-group -->

                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Buscar Proveedor</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtCodCliente" class="form-control" runat="server"></asp:TextBox>
                                        </div>

                                        <div class="col-md-2">
                                            <asp:LinkButton ID="btnBuscarCod" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" OnClick="btnBuscarCod_Click" />
                                        </div>
                                    </div>    
                                    <div class="form-group">
                                        <label class="col-md-4">Proveedor</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListClientes" runat="server" class="form-control"></asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListClientes" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    
                                    <div class="form-group">
                                        <label class="col-md-4">Tipo</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListTipo" runat="server" class="form-control">
                                                <asp:ListItem Value="-1">Ambos</asp:ListItem>
                                                <asp:ListItem Value="0">FC</asp:ListItem>
                                                <asp:ListItem Value="1">PRP</asp:ListItem>
                                            </asp:DropDownList>
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
        <!-- /container -->

    </div>
    <!-- /main -->



    <link href="../../css/pages/reports.css" rel="stylesheet">

    <script src="../../Scripts/plugins/flot/jquery.flot.js"></script>
    <script src="../../Scripts/plugins/flot/jquery.flot.pie.js"></script>
    <script src="../../Scripts/plugins/flot/jquery.flot.orderBars.js"></script>
    <script src="../../Scripts/plugins/flot/jquery.flot.resize.js"></script>

    <script src="../../Scripts/Application.js"></script>

    <script src="../../Scripts/charts/pie.js"></script>
    <script src="../../Scripts/charts/bar.js"></script>

    <script src="//code.jquery.com/ui/1.10.4/jquery-ui.js"></script>

    <script>
        function pageLoad() {      
            $("#<%= txtFechaHasta.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        }
    </script>

    <script>
        
        $(function () {
            $("#<%= txtFechaHasta.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

    </script>

</asp:Content>
