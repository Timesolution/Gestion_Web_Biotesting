<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RentabilidadFCosto.aspx.cs" Inherits="Gestion_Web.Formularios.Reportes.RentabilidadFCosto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-md-12">
        <div class="widget stacked">
            <div class="stat">
                <h5><i class="icon-map-marker"></i>Reportes > Rentabilidad</h5>
            </div>
            <div class="widget-header">
                <i class="icon-wrench"></i>
                <h3>Herramientas</h3>
            </div>
            <!-- /widget-header -->

            <div class="widget-content">
                <table style="width: 100%">
                    <tr>

                        <td style="width: 5%">
                            <div class="btn-group">
                                <button type="button" class="btn btn-success dropdown-toggle" data-toggle="dropdown" id="btnAccion" runat="server">Accion    <span class="caret"></span></button>
                                <ul class="dropdown-menu">
                                    <li class="dropdown-submenu dropdown-menu-right"><a tabindex="-1" href="#">Rentabilidad</a>
                                        <ul class="dropdown-menu">
                                            <li>
                                                <asp:LinkButton ID="lbtnReporteRentabilidad" runat="server" OnClick="lbtnReporteRentabilidad_Click">
                                                                    <i class="fa fa-file-excel-o" aria-hidden="true"></i>
                                                                    &nbsp Exportar
                                                                </asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="lbtnReporteRentabilidadPDF" runat="server" OnClick="lbtnReporteRentabilidadPDF_Click">
                                                                    <i class="fa fa-file-pdf-o" aria-hidden="true"></i>
                                                                    &nbsp Imprimir
                                                                </asp:LinkButton>
                                            </li>
                                        </ul>
                                    </li>
                                </ul>
                            </div>
                            <!-- /btn-group -->
                        </td>

                        <td style="width: 30%">
                            <div class="col-md-12">
                                <div class="input-group">
                                    <span class="input-group-btn">
                                        <asp:LinkButton ID="lbBuscar" runat="server" Text="Buscar" class="btn btn-primary" OnClick="lbBuscarProducto_Click">
                                                    <i class="shortcut-icon icon-search"></i></asp:LinkButton>
                                    </span>
                                    <asp:TextBox ID="txtBusqueda" runat="server" class="form-control" placeholder="Buscar Articulo"></asp:TextBox>

                                </div>
                                <!-- /input-group -->
                            </div>
                        </td>

                        <td style="width: 70%"></td>

                        <td style="width: 5%">
                            <div class="shortcuts" style="height: 100%">
                                <a class="btn btn-primary" data-toggle="modal" href="#modalBusqueda" style="width: 100%">
                                    <i class="shortcut-icon icon-filter"></i>
                                </a>
                            </div>
                        </td>

                        <td style="width: 5%">
                            <div class="shortcuts" style="height: 100%">
                            </div>
                        </td>


                    </tr>

                </table>
            </div>
            <!-- /widget-content -->

        </div>
        <div class="col-md-6">
            <div class="widget big-stats-container stacked">
                <div class="widget-content" style="display: none">
                    <div id="big_stats" class="cf">
                        <div class="stat">
                            <h4>Saldo</h4>
                            <asp:Label ID="labelSaldo" runat="server" Text="" class="value"></asp:Label>
                        </div>
                        <!-- .stat -->
                    </div>
                </div>
                <div class="widget-content">
                    <div id="big_stats" class="cf">
                        <div class="stat">
                            <h4>Total Vendido</h4>
                            <asp:Label ID="labelTotalVendido" runat="server" Text="" class="value"></asp:Label>
                        </div>
                    </div>
                </div>
                <br />
                <div class="widget-content">
                    <div id="big_stats" class="cf">
                        <div class="stat">
                            <h4>Rentabilidad</h4>
                            <asp:Label ID="labelRentabilidad" runat="server" Text="" class="value"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
            <div class="widget big-stats-container stacked">
                <div class="widget-content">
                    <div id="big_stats" class="cf">
                        <div class="stat">
                            <h4>Total Costo</h4>
                            <asp:Label ID="labelTotalCosto" runat="server" Text="" class="value"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="widget big-stats-container stacked">
                <div class="widget-content">
                    <div id="big_stats" class="cf">
                        <div class="stat">
                            <h4>Total Vendido Sin Iva</h4>
                            <asp:Label ID="labelTotalVendidoSinIva" runat="server" Text="" class="value"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
            <div class="widget big-stats-container stacked">
                <div class="widget-content">
                    <div id="big_stats" class="cf">
                        <div class="stat">
                            <h4>Rentabilidad Porcentaje</h4>
                            <asp:Label ID="labelPorRentabilidad" runat="server" Text="" class="value"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
          <div class="widget big-stats-container stacked">
                <div class="widget-content">
                    <div id="big_stats" class="cf">
                        <div class="stat">
                            <h4>Total Costo Con Iva</h4>
                            <asp:Label ID="labelTotalCostoConIva" runat="server" Text="" class="value"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>

        </div>
        <!-- /span12 -->

        <!-- /widget -->
        <div class="widget stacked">
            <div class="widget-header">
                <i class="icon-user"></i>
                <h3>Informe</h3>
            </div>

            <div class="widget-content">

                <div class="table-responsive col-md-12">
                    <asp:GridView ID="GridInforme" class="table table-striped table-bordered table-hover"
                        runat="server" AllowPaging="True" AllowSorting="True" OnPageIndexChanging="GridInforme_PageIndexChanging">
                        <Columns>
                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-HorizontalAlign="Right">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Codigo" HeaderText="Codigo" ItemStyle-HorizontalAlign="Right">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Numero" HeaderText="Numero" ItemStyle-HorizontalAlign="Right">
                                <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
                            <asp:BoundField DataField="Costo" Visible="false" HeaderText="Costo" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Costo Real" Visible="false" HeaderText="Costo Real" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Costo Imponible" HeaderText="Costo Sin Iva" DataFormatString="{0:$#,##0.0000;-$#,##0.0000;0}" ItemStyle-HorizontalAlign="Right">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Costo Imponible Con Iva" HeaderText="Costo Con Iva" DataFormatString="{0:$#,##0.0000;-$#,##0.0000;0}" ItemStyle-HorizontalAlign="Right">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" ItemStyle-HorizontalAlign="Right">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Precio Unitario" HeaderText="Precio Unitario Con Iva" DataFormatString="{0:$#,##0.0000;-$#,##0.0000;0}" ItemStyle-HorizontalAlign="Right">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Precio Unitario Sin Iva" HeaderText="Precio Unitario Sin Iva" DataFormatString="{0:$#,##0.0000;-$#,##0.0000;0}" ItemStyle-HorizontalAlign="Right">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Rentabilidad Costo" HeaderText="Rentabilidad Costo" DataFormatString="{0:$#,##0.0000;-$#,##0.0000;0}" ItemStyle-HorizontalAlign="Right">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Porcentaje Rentabilidad" HeaderText="Porcentaje Rentabilidad" DataFormatString="{0:P}" ItemStyle-HorizontalAlign="Right" />
                        </Columns>

                    </asp:GridView>
                </div>
            </div>
            <!-- /.widget-content -->
        </div>
        <!-- /.widget -->
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
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <div class="form-group">
                                    <label class="col-md-4">Desde</label>
                                    <div class="col-md-6">

                                        <asp:TextBox ID="txtFechaDesde" runat="server" class="form-control"></asp:TextBox>

                                        <!-- /input-group -->
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
                                    <!-- /input-group -->

                                </div>

                                <div class="form-group">
                                    <label for="name" class="col-md-4">Cod. Cliente</label>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtCodCliente" class="form-control" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:LinkButton ID="btnBuscarCodigoCliente" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" OnClick="btnBuscarCodigoCliente_Click" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="name" class="col-md-4">Cliente</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="DropListCliente" class="form-control" runat="server"></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label class="col-md-4">Sucursal</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="DropListSucursal" runat="server" class="form-control" disabled></asp:DropDownList>
                                        <!-- /input-group -->
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <asp:LinkButton ID="lbtnBuscar" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" ValidationGroup="BusquedaGroup" OnClick="lbtnBuscar_Click" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>

        </div>
    </div>

    <link href="../../css/pages/reports.css" rel="stylesheet">
    <script src="./js/libs/jquery-1.9.1.min.js"></script>
    <script src="./js/libs/jquery-ui-1.10.0.custom.min.js"></script>
    <script src="./js/libs/bootstrap.min.js"></script>
    <script src="//code.jquery.com/ui/1.10.4/jquery-ui.js"></script>


    <script>
        $(function () {
            $("#<%= txtFechaDesde.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

        $(function () {
            $("#<%= txtFechaHasta.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

    </script>

</asp:Content>
