<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CCCompraF.aspx.cs" Inherits="Gestion_Web.Formularios.Compras.CCCompraF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">
        <div>
            <div class="col-md-12 col-xs-12">
                <div class="widget stacked">
                    <div class="stat">
                        <h5><i class="icon-map-marker"></i>Compras > Cuentas Corrientes</h5>
                    </div>
                    <div class="widget-header">
                        <i class="icon-wrench"></i>
                        <h3>Herramientas</h3>
                    </div>

                    <div class="widget-content">
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 20%;">
                                    <div class="btn-group">
                                        <button type="button" class="btn btn-success dropdown-toggle" data-toggle="dropdown" id="btnAccion" runat="server">Accion    <span class="caret"></span></button>
                                        <ul class="dropdown-menu">
                                            <li>
                                                <asp:LinkButton ID="btnExportarExcel" runat="server" data-toggle="modal" href="#modalExportar">Exportar</asp:LinkButton>
                                            </li>
                                        </ul>
                                    </div>
                                    <!-- /btn-group -->
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
                                <asp:Label ID="lblSaldo" runat="server" Text="" class="value"></asp:Label>
                            </div>
                            <div class="stat">
                                <h4>Proximo a vencer</h4>
                                <asp:Label ID="lblVencimiento" runat="server" Text="" class="value"></asp:Label>
                            </div>
                            <!-- .stat -->
                        </div>

                    </div>


                    <!-- /widget-content -->

                </div>
                <!-- /widget -->

            </div>

            <div class="col-md-12 col-xs-12">
                <div class="widget widget-table">

                    <div class="widget-header">
                        <i class="icon-th-list" style="width: 2%"></i>
                        <h3 style="width: 75%">Movimientos Cuenta Corriente
                        </h3>
                        <%--<h3>
                            <asp:Label ID="lblSaldo" runat="server" Style="text-align: right" Text="" ForeColor="#666666" Font-Bold="true"></asp:Label>
                        </h3>--%>
                    </div>
                    <div class="widget-content">
                        <%--<div class="panel-body">--%>
                        <%--<div class="col-md-12 col-xs-12">--%>
                        <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Always" runat="server">
                            <ContentTemplate>
                                <%--<div class="table-responsive">--%>
                                <a class="btn btn-info" style="display: none" data-toggle="modal" id="abreDialog" href="#Comentario">Agregar Tipo Cliente</a>
                                <table class="table table-bordered table-striped" id="dataTablesCC-example">
                                    <thead>
                                        <tr>
                                            <th>Fecha</th>
                                            <th>Numero</th>
                                            <th>Debe</th>
                                            <th>Haber</th>
                                            <th>Saldo</th>
                                            <th>Saldo Acumulado</th>
                                            <th></th>

                                        </tr>

                                    </thead>
                                    <tbody>
                                        <asp:PlaceHolder ID="phCuentaCorriente" runat="server"></asp:PlaceHolder>
                                    </tbody>
                                </table>

                                <%--</div>--%>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                        <%--<div class="col-md-6">
                                </div>--%>

                        <%--</div>--%>
                        <%--</div>--%>
                    </div>
                    <%-- widget content --%>
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
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <div class="form-group">
                                            <label class="col-md-4">Desde</label>
                                            <div class="col-md-6">
                                                <div class="input-group">
                                                    <span class="input-group-addon"><i class="icon-calendar"></i></span>
                                                    <asp:TextBox ID="txtFechaDesde" class="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaDesde" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4">Hasta</label>
                                            <div class="col-md-6">
                                                <div class="input-group">
                                                    <span class="input-group-addon"><i class="icon-calendar"></i></span>
                                                    <asp:TextBox ID="txtFechaHasta" class="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaHasta" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4">Cod Proveedor</label>
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
                                                <asp:DropDownList ID="ListProveedor" runat="server" class="form-control"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="ListProveedor" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4">Sucursal</label>
                                            <div class="col-md-6">
                                                <asp:DropDownList ID="DropListSucursal" runat="server" class="form-control"></asp:DropDownList>
                                                <!-- /input-group -->
                                            </div>
                                            <div class="col-md-2">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListSucursal" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4">Tipo</label>
                                            <div class="col-md-6">
                                                <asp:DropDownList ID="DropListTipo" runat="server" class="form-control">
                                                    <asp:ListItem Value="-1">Ambos</asp:ListItem>
                                                    <asp:ListItem Value="19">Compra</asp:ListItem>
                                                    <asp:ListItem Value="21">Pago</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>

                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4">Tipo Documento</label>
                                            <div class="col-md-6">
                                                <asp:DropDownList ID="DropListTipoDocumento" runat="server" class="form-control">
                                                    <asp:ListItem Value="-1">Ambos</asp:ListItem>
                                                    <asp:ListItem Value="0">FC</asp:ListItem>
                                                    <asp:ListItem Value="1">PRP</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>

                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>

                            </div>
                        </div>


                        <div class="modal-footer">
                            <asp:LinkButton ID="lbtnBuscar" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" ValidationGroup="BusquedaGroup" OnClick="lbtnBuscar_Click" />
                        </div>
                    </div>

                </div>
            </div>
            <div id="Comentario" class="modal fade" tabindex="-1" role="dialog">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h4 class="modal-title">Comentario</h4>
                        </div>
                        <div class="modal-body">
                            <div role="form" class="form-horizontal col-md-12">
                                <div class="form-group">
                                    <div class="col-md-12">
                                        <asp:TextBox ID="txtComentario2" runat="server" disabled class="form-control" TextMode="MultiLine" Rows="8" Columns="6"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div id="modalExportar" class="modal fade" tabindex="-1" role="dialog">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h4 class="modal-title">Imprimir</h4>
                        </div>
                        <div class="modal-body">
                        </div>
                        <div class="modal-footer">
                            <asp:LinkButton ID="lbtnImprimirPDF" runat="server" Text="Imprimir PDF" class="btn btn-success" OnClick="lbtnImprimirPDF_Click" />
                            <asp:LinkButton ID="lbtnExportarXLS" runat="server" Text="Exportar XLS" class="btn btn-success" OnClick="btnExportarExcel_Click" />
                        </div>
                    </div>
                </div>
            </div>

            <script>
                function abrirdialog(comentario) {
                    document.getElementById('<%= txtComentario2.ClientID %>').value = comentario;
                    document.getElementById('abreDialog').click();
                }
            </script>

        </div>
        <%--Fin modalGrupo--%>
    </div>

    <!-- Page-Level Plugin Scripts - Tables -->
    <script src="//cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>
    <script src="../../Scripts/plugins/dataTables/custom.tables.js"></script>
    <link href="//cdn.datatables.net/1.10.2/css/jquery.dataTables.css" rel="stylesheet" />
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

    <script>
        function pageLoad() {
            $("#<%= txtFechaDesde.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%= txtFechaHasta.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        }
    </script>

</asp:Content>
