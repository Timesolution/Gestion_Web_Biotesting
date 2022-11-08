<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CuentaCorrienteF.aspx.cs" Inherits="Gestion_Web.Formularios.Facturas.CuentaCorrienteF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">

        <%--<div class="container">--%><div>


            <div class="col-md-12 col-xs-12">
                <div class="widget stacked">
                    <div class="stat">
                        <h5 id="hImpagas"><i class="icon-map-marker"></i>Ventas > Cuentas Corrientes</h5>
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
                                        <button type="button" class="btn btn-success dropdown-toggle" data-toggle="dropdown" id="btnAccion" runat="server" visible="false">Accion    <span class="caret"></span></button>
                                        <ul class="dropdown-menu">
                                            <li>
                                                <asp:LinkButton ID="btnImprimir" runat="server" OnClick="btnImprimir_Click">Imprimir reporte</asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="lbtnExportar" runat="server" OnClick="lbtnExportar_Click">Exportar reporte</asp:LinkButton>
                                            </li>

                                        </ul>
                                    </div>
                                </td>

                                <td>&nbsp
                                </td>

                                <td style="width: 95%">
                                    <h5>
                                        <asp:Label runat="server" ID="lblParametros" Text="" ForeColor="#cccccc"></asp:Label>
                                    </h5>
                                </td>

                                <td style="width: 5%">
                                    <div class="shortcuts" style="height: 100%">

                                        <asp:LinkButton class="btn btn-primary" ToolTip="Cobros" ID="lbnCobros" Visible="false" Style="width: 100%" runat="server" OnClick="lbnCobros_Click"> 
                                            <i ID="iconCobros" class="fa icon-usd"></i>
                                        </asp:LinkButton>

                                    </div>
                                </td>

                                <td>&nbsp
                                </td>

                                <td style="width: 5%">
                                    <div class="shortcuts" style="height: 100%">

                                        <asp:LinkButton class="btn btn-primary" ToolTip="Impagas" ID="btnImpagas" Visible="false" Style="width: 100%" runat="server" OnClick="btnImpagas_Click"> 
                                            <i ID="iconImpaga" class="fa fa-exclamation-triangle"></i>
                                        </asp:LinkButton>

                                    </div>
                                </td>

                                <td>&nbsp
                                </td>

                                <td style="width: 95%">
                                    <h5>
                                        <asp:Label runat="server" ID="Label3" Text="" ForeColor="#cccccc"></asp:Label>
                                    </h5>
                                </td>

                                <td style="width: 5%">
                                    <div class="shortcuts" style="height: 100%">

                                        <asp:LinkButton class="btn btn-primary" ToolTip="Exportar Reporte txt" ID="lbtnExportartxt" Style="width: 100%" runat="server" OnClick="lbtnExportartxt_Click"> 
                                            <i ID="iconExportartxt" class="fa fa-print"></i>
                                        </asp:LinkButton>

                                    </div>

                                </td>

                                <td>&nbsp
                                </td>

                                <td style="width: 5%">
                                    <div class="shortcuts" style="height: 100%">

                                        <a class="btn btn-primary" data-toggle="modal" href="#modalBusqueda" style="width: 130%">
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
                                <asp:Label ID="labelSaldo" runat="server" Text="" class="value"></asp:Label>
                                <asp:Label ID="labelMov" runat="server" Text="" class="value"></asp:Label>
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
                        <i class="icon-th-list" style="width: 2%"></i>
                        <h3 style="width: 75%">Movimientos Cuenta Corriente
                        </h3>
                        <h3>
                            <asp:Label ID="lblSaldo" runat="server" Style="text-align: right" Text="" ForeColor="#666666" Font-Bold="true"></asp:Label>
                        </h3>
                    </div>
                    <%--<div class="widget-content">
                        <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Always" runat="server">
                            <ContentTemplate>
                                <a class="btn btn-info" style="display: none" data-toggle="modal" id="abreDialog" href="#modalFacturaDetalle">Agregar Tipo Cliente</a>
                                <table class="table table-bordered table-striped" id="dataTablesCC-example">
                                    <thead>
                                        <tr>
                                            <th>Fecha</th>
                                            <th>Numero</th>
                                            <th>Debe</th>
                                            <th>Haber</th>
                                            <th>Saldo</th>
                                            <th>Saldo Acumulado</th>
                                        </tr>

                                    </thead>
                                    <tbody>
                                        <asp:PlaceHolder ID="phCuentaCorriente" runat="server"></asp:PlaceHolder>
                                    </tbody>
                                </table>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>--%>
                    <%-- widget content --%>

                    <div class="widget-content">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <div class="table-responsive col-md-12">
                                    <asp:GridView ID="GridCtaCte" class="table table-striped table-bordered table-hover"
                                        runat="server" AllowPaging="True" OnPageIndexChanging="GridInforme_PageIndexChanging" AllowSorting="true" PageSize="15">
                                        <Columns>
                                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-HorizontalAlign="Right">
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Numero" HeaderText="Descripcion" />
                                            <asp:BoundField DataField="Debe" HeaderText="Debe" DataFormatString="{0:$#,##0.00;-$#,##0.00;0}" ItemStyle-HorizontalAlign="Right">
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Haber" HeaderText="Haber" DataFormatString="{0:$#,##0.00;-$#,##0.00;0}" ItemStyle-HorizontalAlign="Right">
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="Saldo" HeaderText="Saldo" DataFormatString="{0:$#,##0.00;-$#,##0.00;0}" ItemStyle-HorizontalAlign="Right">
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="SaldoAcumulado" HeaderText="Saldo Acumulado" DataFormatString="{0:$#,##0.00;-$#,##0.00;0}" ItemStyle-HorizontalAlign="Right">
                                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                            </asp:BoundField>
                                            <asp:BoundField DataField="GuiaDespacho" HeaderText="Guia Despacho" />
                                            <asp:TemplateField HeaderText=" ">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnCompensar" runat="server" class="btn ui-tooltip" title data-original-title="Compensacion de cuentas" CommandArgument='<%# Bind("Id") %>' OnClientClick="openModal(this)" OnClick="btnCompensar_Click">
                                                        <asp:Label runat="server" ID="literal1" Style="display: none;" Text='<%# Bind("Id") %>'></asp:Label>
                                                        <i class="fa fa-exchange"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnComentario" runat="server" class="btn ui-tooltip" title data-original-title="Observaciones" CommandArgument='<%# Bind("TipoDoc") %>' OnClientClick="openModalComentario()" OnClick="btnComentario_Click">
                                                        <asp:Label runat="server" ID="Label1" Style="display: none;" Text='<%# Bind("Id") %>'></asp:Label>
                                                        <i class="shorcut-icon icon-comment"></i>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton ID="btnImprimir" runat="server" class="btn ui-tooltip" title data-original-title="Imprimir" CommandArgument='<%# Bind("TipoDoc") %>' OnClick="btnImprimir_Click1">
                                                        <asp:Label runat="server" ID="Label2" Style="display: none;" Text='<%# Bind("Id") %>'></asp:Label>
                                                        <i class="shorcut-icon icon-search"></i>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%--<asp:ButtonField ButtonType="Image" ImageUrl="~/img/exchange.png" CommandName="Traspasar" />--%>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
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
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>

                                    <div role="form" class="form-horizontal col-md-12">
                                        <div class="form-group">
                                            <label class="col-md-4">Desde</label>
                                            <div class="col-md-6">
                                                <div class="input-group">
                                                    <span class="input-group-addon"><i class="icon-calendar"></i></span>
                                                    <asp:TextBox ID="txtFechaDesde" class="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaDesde" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
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
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaHasta" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <asp:PlaceHolder ID="phClienteBusqueda" runat="server">

                                        <div class="form-group">
                                            <label class="col-md-4">Buscar Cliente</label>
                                            <div class="col-md-6">
                                                <asp:TextBox ID="txtCodCliente" class="form-control" runat="server"></asp:TextBox>
                                            </div>

                                            <div class="col-md-2">
                                                <asp:LinkButton ID="btnBuscarCod" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" OnClick="btnBuscarCod_Click" />
                                            </div>
                                        </div>
                                        </asp:PlaceHolder>
                                        <div class="form-group">
                                            <label class="col-md-4">Cliente</label>
                                            <div class="col-md-6">
                                                <asp:DropDownList ID="DropListClientes" runat="server" AutoPostBack="true" class="form-control" OnSelectedIndexChanged="DropListClientes_SelectedIndexChanged"></asp:DropDownList>
                                            </div>

                                            <div class="col-md-2">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListClientes" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4">Razon Social</label>
                                            <div class="col-md-6">
                                                <asp:DropDownList ID="ListRazonSocial" runat="server" AutoPostBack="true" class="form-control" OnSelectedIndexChanged="ListRazonSocial_SelectedIndexChanged"></asp:DropDownList>
                                            </div>

                                            <div class="col-md-2">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="ListRazonSocial" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <asp:PlaceHolder runat="server" ID="phSucursales">
                                            <div class="form-group">
                                                <label class="col-md-4">Sucursal</label>
                                                <div class="col-md-6">
                                                    <asp:DropDownList ID="DropListSucursal" runat="server" disabled class="form-control"></asp:DropDownList>
                                                    <!-- /input-group -->
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListSucursal" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </asp:PlaceHolder>
                                        <asp:PlaceHolder runat ="server" ID="phTipo">

                                        <div class="form-group">
                                            <label class="col-md-4">Tipo</label>
                                            <div class="col-md-6">
                                                <asp:DropDownList ID="DropListTipo" runat="server" class="form-control">
                                                    <asp:ListItem Value="-1">Ambos</asp:ListItem>
                                                    <asp:ListItem Value="0">FC</asp:ListItem>
                                                    <asp:ListItem Value="1">PRP</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        
                                            <%-- <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListTipo" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                    </div>--%>
                                                </div>
                                            </asp:PlaceHolder>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>


                        <div class="modal-footer">
                            <asp:LinkButton ID="lbtnBuscar" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" ValidationGroup="BusquedaGroup" OnClick="lbtnBuscar_Click" />
                        </div>
                    </div>

                </div>
            </div>

            <div id="modalCompensacion" class="modal fade" tabindex="-1" role="dialog">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                            <ContentTemplate>

                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                                    <h4 class="modal-title">Compensacion entre Cuentas</h4>
                                </div>
                                <div class="modal-body">
                                    <h3 id="lbAguarde" runat="server">Aguarde <i class="fa fa-spinner fa-spin"></i></h3>

                                    <asp:Panel ID="panel1" runat="server" hidden="true">
                                        <div role="form" class="form-horizontal col-md-12">
                                            <div class="form-group" id="divSucursal">
                                                <label class="col-md-4">Sucursal destino</label>
                                                <div class="col-md-6">
                                                    <asp:DropDownList ID="ListSucursalDestino" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ListSucursalDestino_SelectedIndexChanged"></asp:DropDownList>
                                                    <!-- /input-group -->
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="ListSucursalDestino" InitialValue="-1" ValidationGroup="CompensacionGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-md-4">Pto venta destino</label>
                                                <div class="col-md-6">
                                                    <asp:DropDownList ID="ListPuntoVentaDestino" runat="server" class="form-control"></asp:DropDownList>
                                                    <!-- /input-group -->
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="ListPuntoVentaDestino" InitialValue="-1" ValidationGroup="CompensacionGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <asp:Label ID="lblMovimiento" runat="server" Text="label" ForeColor="White"></asp:Label>
                                        </div>
                                    </asp:Panel>

                                </div>
                                <div class="modal-footer">
                                    <asp:LinkButton ID="lbtnAgregarCompensacion" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" ValidationGroup="CompensacionGroup" OnClick="lbtnAgregarCompensacion_Click" />
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                </div>
            </div>

            <div id="modalComentarios" class="modal fade" tabindex="-1" role="dialog">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Always">
                            <ContentTemplate>

                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                                    <h4 class="modal-title">Observaciones</h4>
                                </div>
                                <div class="modal-body">
                                    <h3 id="lbAguarde2" runat="server">Aguarde <i class="fa fa-spinner fa-spin"></i></h3>

                                    <asp:Panel ID="panel2" runat="server" hidden="true">
                                        <div role="form" class="form-horizontal col-md-12">
                                            <div class="form-group">
                                                <div class="col-md-12">
                                                    <asp:TextBox ID="txtComentario" runat="server" class="form-control" disabled TextMode="MultiLine" Rows="4" />
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                </div>
            </div>


        </div>



        <script>
            function cambiarIcono(valor, valor2) {
                document.getElementById('iconImpaga').className = valor;
                document.getElementById('hImpagas').textContent = valor2;
            }
        </script>

        <script type="text/javascript">
            function openModal(boton) {
                var texto = boton.children[0].textContent;
                document.getElementById('<%= lbAguarde.ClientID %>').hidden = false;
                document.getElementById('<%= panel1.ClientID %>').hidden = true;

                $('#modalCompensacion').modal('show');
            }
            function openModalComentario() {
                document.getElementById('<%= lbAguarde2.ClientID %>').hidden = false;
                document.getElementById('<%= panel2.ClientID %>').hidden = true;
                $('#modalComentarios').modal('show');
            }
        </script>

        <link href="../../css/pages/reports.css" rel="stylesheet">
        <!-- Page-Level Plugin Scripts - Tables -->
        <script src="//cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>
        <script src="../../Scripts/plugins/dataTables/custom.tables.js"></script>
        <link href="//cdn.datatables.net/1.10.2/css/jquery.dataTables.css" rel="stylesheet" />
        <%--Fin modalGrupo--%>
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
    </div>
    <script>
        function pageLoad() {
            $("#<%= txtFechaDesde.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%= txtFechaHasta.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        }
    </script>
    <%--<script type="text/javascript">
        function BloquearParaDistribuidor() {
            var usuario = document.getElementById(<%=Label3.ID %>);
            if (usuario == "Distribuidor") {
                document.getElementById(<%=divSucursal.id%>)
            }
        }
    </script>--%>
</asp:Content>
