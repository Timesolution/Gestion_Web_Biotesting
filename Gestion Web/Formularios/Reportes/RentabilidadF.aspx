<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RentabilidadF.aspx.cs" Inherits="Gestion_Web.Formularios.Informes.RentabilidadF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-md-12">
        <div class="widget stacked">
            <div class="stat">
                <h5><i class="icon-map-marker"></i> Reportes > Rentabilidad Costo</h5>
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
                                    <li>
                                        <asp:LinkButton ID="lbtnRemitir" runat="server" OnClick="lbtnRemitir_Click">Exportar a Excel</asp:LinkButton>
                                        <%--<asp:Button ID="BtnExcel" class="btn" runat="server" Text="Exportar a Excel" OnClick="lbtnRemitir_Click" />--%>
                                    </li>
                                    <li>
                                        <asp:LinkButton ID="btnRentPorSucursal" runat="server" OnClick="btnRentPorSucursal_Click">Rentabilidad por Sucursal</asp:LinkButton>
                                        <%--<asp:Button ID="BtnExcel" class="btn" runat="server" Text="Exportar a Excel" OnClick="lbtnRemitir_Click" />--%>
                                    </li>

                                </ul>
                            </div>
                            <!-- /btn-group -->
                        </td>
                        <td style="width: 30%">
                            <%--<div class="col-md-12">
                                <div class="input-group">
                                    <span class="input-group-btn">
                                        <asp:LinkButton ID="lbBuscar" runat="server" Text="Buscar" class="btn btn-primary" OnClick="lbBuscarProducto_Click">
                                                    <i class="shortcut-icon icon-search"></i></asp:LinkButton>
                                    </span>
                                    <asp:TextBox ID="txtBusqueda" runat="server" class="form-control" placeholder="Buscar Articulo"></asp:TextBox>

                                </div>
                                <!-- /input-group -->
                            </div>--%>
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

        <div class="col-md-12">
            <div class="widget big-stats-container stacked">
                <div class="widget-content">
                    <div id="big_stats" class="cf">
                        <div class="stat">
                            <h4>Total Vendido</h4>
                            <asp:Label ID="labelTotalVendido" runat="server" Text="" class="value"></asp:Label>
                        </div>
                        <div class="stat">
                            <h4>Total Costo</h4>
                            <asp:Label ID="labelTotalCosto" runat="server" Text="" class="value"></asp:Label>
                        </div>
                    </div>
                    <div id="big_stats" class="cf">
                        <div class="stat">
                            <h4>Rentabilidad</h4>
                            <asp:Label ID="labelRentabilidad" runat="server" Text="" class="value"></asp:Label>
                        </div>
                        <div class="stat">
                            <h4>Rentabilidad Porcentaje</h4>
                            <asp:Label ID="labelPorRentabilidad" runat="server" Text="" class="value"></asp:Label>
                        </div>
                    </div>
                </div>
                <!-- /widget-content -->
            </div>
            <!-- /widget -->
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
                        runat="server" AllowPaging="True" OnPageIndexChanging="GridInforme_PageIndexChanging">
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
                            <asp:BoundField DataField="Costo" HeaderText="Costo" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Costo Real" HeaderText="Costo Real" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Costo Imponible" HeaderText="Costo Imponible" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" ItemStyle-HorizontalAlign="Right">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Precio Unitario" HeaderText="Precio Unitario" DataFormatString="{0:$#,##0.00;-$#,##0.00;0}" ItemStyle-HorizontalAlign="Right">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Rentabilidad Costo" HeaderText="Rentabilidad Costo" DataFormatString="{0:$#,##0.00;-$#,##0.00;0}" ItemStyle-HorizontalAlign="Right">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
                            <asp:BoundField DataField="Porcentaje Rentabilidad" HeaderText="Porcentaje Rentabilidad" DataFormatString="{0:P}" ItemStyle-HorizontalAlign="Right">
                                <ItemStyle HorizontalAlign="Right"></ItemStyle>
                            </asp:BoundField>
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

                        <div class="form-group" style="margin-bottom:1px;">
                            <label class="col-md-4">Articulo</label>
                            <div class="col-md-6">
                                <asp:TextBox ID="txtBusqueda" runat="server" class="form-control" placeholder="Buscar Articulo"></asp:TextBox>                                
                                <!-- /input-group -->
                            </div>                            
                            <div class="col-md-2">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaDesde" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group" style="margin-bottom:15px;">
                            <label class="col-md-4"></label>                            
                            <div class="col-md-6">
                                <label>Busqueda Art. exacta: </label> 
                                <asp:CheckBox ID="chkTipoBusqueda" runat="server" />
                            </div>
                        </div>
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
                            <label class="col-md-4">Sucursal</label>
                            <div class="col-md-6">
                                <asp:DropDownList ID="DropListSucursal" runat="server" class="form-control"></asp:DropDownList>
                                <!-- /input-group -->
                            </div>

                        </div>
                        <div class="form-group">
                            <label class="col-md-4">Numero factura</label>
                            <div class="col-md-6">
                                <asp:TextBox ID="txtNroFactura" runat="server" class="form-control" placeholder="Ej.: 00000001" TextMode="Number" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                <!-- /input-group -->
                            </div>

                        </div>
                        <div class="form-group">
                            <label class="col-md-4">Iva Costo</label>
                            <div class="col-md-6">
                                <asp:DropDownList ID="ListConIva" runat="server" class="form-control">
                                    <asp:ListItem Text="Costo con Iva" Value="0" />
                                    <asp:ListItem Text="Costo sin Iva" Value="1" />
                                </asp:DropDownList>
                            </div>

                        </div>

                    </div>
                </div>


                <div class="modal-footer">
                    <asp:LinkButton ID="lbtnBuscar" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" ValidationGroup="BusquedaGroup" OnClick="lbtnBuscar_Click" />
                </div>
            </div>

        </div>
    </div>
    <link href="../../css/pages/reports.css" rel="stylesheet" />
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
    <script>
        //valida los campos solo numeros
        function validarNro(e) {
            var key;
            if (window.event) // IE
            {
                key = e.keyCode;
            }
            else if (e.which) // Netscape/Firefox/Opera
            {
                key = e.which;
            }

            if (key < 48 || key > 57) {
                if (key == 46 || key == 8 || key == 44) // Detectar . (punto) , backspace (retroceso) y , (coma)
                { return true; }
                else
                { return false; }
            }
            return true;
        }
    </script>
</asp:Content>
