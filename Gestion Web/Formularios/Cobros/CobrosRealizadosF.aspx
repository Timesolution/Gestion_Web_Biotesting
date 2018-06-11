<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CobrosRealizadosF.aspx.cs" Inherits="Gestion_Web.Formularios.Cobros.CobrosRealizadosF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">

        <%--<div class="container">--%><div>

            <div class="col-md-12 col-xs-12">
                <div class="widget stacked">
                    <div class="stat">                        
                        <h5><i class="icon-map-marker"></i> Ventas > Cobros Realizados</h5>
                    </div>
                    <div class="widget-header">
                        <i class="icon-wrench"></i>
                        <h3>Herramientas</h3>
                    </div>
                    <!-- /widget-header -->

                    <div class="widget-content">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                            <ContentTemplate>

                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 20%">
                                            <div class="btn-group">
                                                <button type="button" class="btn btn-success dropdown-toggle" data-toggle="dropdown" id="btnAccion" runat="server">Accion    <span class="caret"></span></button>
                                                <ul class="dropdown-menu">
                                                    <li>
                                                        <asp:LinkButton ID="lbtnExportar" runat="server" OnClick="lbtnExportar_Click" >Exportar reporte</asp:LinkButton>
                                                    </li>
                                                    <li class="dropdown-submenu dropdown-menu-right"><a tabindex="-1" href="#">Cobranzas vendedores</a>  
                                                        <ul class="dropdown-menu" >
                                                            <li>
                                                                <asp:LinkButton ID="lbtnReporteCobranza" runat="server" OnClick="lbtnReporteCobranza_Click" >
                                                                    <i class="fa fa-file-excel-o" aria-hidden="true"></i>
                                                                    &nbsp Exportar
                                                                </asp:LinkButton>
                                                            </li>
                                                            <li>
                                                                <asp:LinkButton ID="lbtnReporteCobranzaPDF" runat="server" OnClick="lbtnReporteCobranzaPDF_Click" >
                                                                    <i class="fa fa-file-pdf-o" aria-hidden="true"></i>
                                                                    &nbsp Imprimir
                                                                </asp:LinkButton>
                                                            </li>
                                                        </ul>
                                                    </li>

                                                    <li class="dropdown-submenu dropdown-menu-right"><a tabindex="-1" href="#">Detalle Cobros</a>  
                                                        <ul class="dropdown-menu" >
                                                            <li>
                                                                <asp:LinkButton ID="lbtnReporteDetalleCobros" runat="server" OnClick="lbtnReporteDetalleCobros_Click" >
                                                                    <i class="fa fa-file-excel-o" aria-hidden="true"></i>
                                                                    &nbsp Exportar
                                                                </asp:LinkButton>
                                                            </li>
                                                            <li>
                                                                <asp:LinkButton ID="lbtnReporteDetalleCobrosPDF" runat="server" OnClick="lbtnReporteDetalleCobrosPDF_Click" >
                                                                    <i class="fa fa-file-pdf-o" aria-hidden="true"></i>
                                                                    &nbsp Imprimir
                                                                </asp:LinkButton>
                                                            </li>
                                                        </ul>
                                                    </li>

                                                </ul>
                                            </div>
                                        </td>
                                        <td style="width: 75%">
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

                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                    <!-- /widget-content -->

                </div>
                <!-- /widget -->
            </div>

            <div class="col-md-12">

                <div class="widget big-stats-container stacked">

                    <div class="widget-content">

                        <div id="big_stats" class="cf">
                            <div class="stat">
                                <h4>Saldo</h4>
                                <asp:Label ID="labelSaldo" runat="server" Text="" class="value"></asp:Label>
                            </div>
                            <!-- .stat -->


                        </div>

                    </div>
                    <!-- /widget-content -->

                </div>
                <!-- /widget -->

            </div>
            <!-- /span12 -->

            <div class="col-md-12 col-xs-12">
                <div class="widget widget-table">

                    <div class="widget-header">
                        <i class="icon-money" style="width: 2%"></i>
                        <h3 style="width: 75%">Cobros Realizados
                        </h3>
                        <h3>
                            <asp:Label ID="lblSaldo" runat="server" Style="text-align: end" Text="" ForeColor="#0099ff" Font-Bold="true"></asp:Label>
                        </h3>
                    </div>
                    <div class="widget-content">
                        <a class="btn btn-info" style="display: none" data-toggle="modal" id="abreDialog" href="#modalConfirmacion">Agregar Tipo Cliente</a>
                        <table class="table table-bordered table-striped" id="dataTables-example">
                            <thead>
                                <tr>
                                    <th style="text-align: left;width:10%;">Fecha</th>
                                    <th style="text-align: left;width:25%;">Numero</th>
                                    <th style="text-align: left;width:20%;">Cliente</th>
                                    <th style="text-align: right;width:10%;">Importe</th>
                                    <th style="text-align: left;width:25%;">Comentarios</th>
                                    <th style="text-align: left;width:10%;"></th>

                                </tr>
                            </thead>
                            <tbody>
                                <asp:PlaceHolder ID="phCobrosRealizados" runat="server"></asp:PlaceHolder>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
        <%--Fin modalGrupo--%>
    </div>

    <div id="modalConfirmacion" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Confirmacion de Eliminacion</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <div class="form-group">
                            <div class="col-md-2">
                                <h1>
                                    <i class="icon-warning-sign" style="color: orange"></i>
                                </h1>
                            </div>
                            <div class="col-md-7">
                                <h5>
                                    <asp:Label runat="server" ID="lblMensaje" Text="Esta seguro que desea eliminar el Cobro?" Style="text-align: center"></asp:Label>
                                </h5>
                            </div>

                            <div class="col-md-3">
                                <asp:TextBox runat="server" ID="txtMovimiento" Text="0" Style="display: none"></asp:TextBox>
                            </div>
                        </div>
                        <%--                                <div class="form-group">
                                    
                                </div>--%>
                    </div>


                    <div class="modal-footer">
                        <asp:Button runat="server" ID="btnSi" Text="Eliminar" class="btn btn-danger" OnClick="btnSi_Click" />
                        <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>

                        <%--                        <asp:LinkButton ID="LinkButton1" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnBuscar_Click" ValidationGroup="BusquedaGroup" />--%>
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
                                    <label class="col-md-4">Desde</label>
                                    <div class="col-md-4">

                                        <asp:TextBox ID="txtFechaDesde" runat="server" class="form-control"></asp:TextBox>

                                        <!-- /input-group -->
                                    </div>
                                    <div class="col-md-4">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaDesde" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Hasta</label>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtFechaHasta" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaHasta" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                    </div>
                                    <!-- /input-group -->

                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Buscar Cliente</label>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtCodCliente" class="form-control" runat="server"></asp:TextBox>
                                    </div>

                                    <div class="col-md-2">
                                        <asp:LinkButton ID="btnBuscarCod" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" OnClick="btnBuscarCod_Click" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Cliente</label>
                                    <div class="col-md-6">

                                        <asp:DropDownList ID="DropListClientes" runat="server" class="form-control"></asp:DropDownList>

                                        <!-- /input-group -->
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListClientes" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Empresa</label>
                                    <div class="col-md-6">

                                        <asp:DropDownList ID="DropListEmpresa" runat="server" class="form-control" OnSelectedIndexChanged="DropListEmpresa_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>

                                    </div>

                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListEmpresa" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                    <!-- /input-group -->
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Sucursal</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="DropListSucursal" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="DropListSucursal_SelectedIndexChanged"></asp:DropDownList>
                                        <!-- /input-group -->
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListSucursal" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Punto de Venta</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="DropListPuntoVta" runat="server" class="form-control"></asp:DropDownList>

                                        <!-- /input-group -->
                                    </div>

                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListPuntoVta" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Tipo</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="DropListTipo" runat="server" class="form-control">
                                            <asp:ListItem Value="0">Ambos</asp:ListItem>
                                            <asp:ListItem Value="1">FC</asp:ListItem>
                                            <asp:ListItem Value="2">PRP</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListTipo" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Vendedores</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="DropListVendedores" runat="server" class="form-control"></asp:DropDownList>
                                        <!-- /input-group -->
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>

                    </div>


                    <div class="modal-footer">
                        <asp:LinkButton ID="lbBuscar" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnBuscar_Click" ValidationGroup="BusquedaGroup" />
                    </div>
                </div>

            </div>
        </div>
    </div>


    <script>
        function abrirdialog(valor) {
            document.getElementById('<%= txtMovimiento.ClientID %>').value = valor;
            }
    </script>
    <link href="../../css/pages/reports.css" rel="stylesheet">
    <!-- Core Scripts - Include with every page -->
    <script src="../../Scripts/jquery-1.10.2.js"></script>
    <script src="../../Scripts/bootstrap.min.js"></script>
    <%--<script src="../Scripts/plugins/metisMenu/jquery.metisMenu.js"></script>--%>

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

    <!-- Page-Level Plugin Scripts - Tables -->
    <script src="../../Scripts/plugins/dataTables/jquery.dataTables.min.js"></script>
    <script src="../../Scripts/plugins/dataTables/custom.tables.js"></script>
    <link href="../../Scripts/plugins/dataTables/dataTables.css" rel="stylesheet" />

    <%--<script src="//code.jquery.com/jquery-1.9.1.js"></script>--%>
    <script src="//code.jquery.com/ui/1.10.4/jquery-ui.js"></script>

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



