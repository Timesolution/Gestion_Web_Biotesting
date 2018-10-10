<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportesCobros.aspx.cs" Inherits="Gestion_Web.Formularios.Reportes.ReportesCobros" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="main">

        <div>

            <div>
                <div class="col-md-12 col-xs-12">
                    <div class="widget stacked">
                        <div class="stat">
                            <h5><i class="icon-map-marker"></i>&nbsp Reportes > Impagas > Clientes</h5>
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
                                                        <asp:LinkButton ID="lbtnImprimir" OnClick="btnImprimir_Click" runat="server">Reporte impagas clientes</asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lbtnExportar2" OnClick="lbtnExportar2_Click" runat="server">Exportar reporte impagas</asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lbtnImprimirDetalle" OnClick="lbtnImprimirDetalle_Click" runat="server">Reporte impagas detallado</asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lbtnExportarImpagasDetallado" OnClick="lbtnExportarImpagasDetallado_Click" runat="server">Exportar impagas detallado</asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lbtnImprimirDetalleNotaDebito" OnClick="lbtnImprimirDetalleNotaDebito_Click" runat="server">Reporte Notas de Debito impagas detallado</asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lbtnExportar" OnClick="lbtnExportar_Click" runat="server">Exportar reporte detallado</asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lbtnImprimirVencimientos" OnClick="lbtnImprimirVencimientos_Click" runat="server">Imprimir detalle vencimientos</asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="lbtnExportarVencimientos" OnClick="lbtnExportarVencimientos_Click" runat="server">Exportar detalle vencimientos</asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="btnImpagasVendedor" OnClick="btnImpagasVendedor_Click" runat="server">Reporte impagas por vendedor</asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="btnExpImpagasVendedor" OnClick="btnExpImpagasVendedor_Click" runat="server">Exportar impagas por vendedor</asp:LinkButton>
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

            <div>

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
            <div>
                <div class="col-md-12 col-xs-12">
                    <div class="widget stacked">

                        <div class="widget-header">
                            <i class="icon-list"></i>
                            <h3>Ranking Impagas</h3>
                        </div>
                        <div class="widget-content">
                            <div class="bs-example">
                                <ul id="myTab" class="nav nav-tabs">
                                    <li class="active"><a href="#home" data-toggle="tab">Clientes</a></li>
                                    <li class=""><a href="#profile" data-toggle="tab">Vendedores</a></li>
                                </ul>
                                <div class="tab-content">
                                    <div class="tab-pane fade active in" id="home">
                                        <div class="col-md-8">

                                            <div class="widget stacked widget-table">

                                                <div class="widget-header">
                                                    <span class="icon-list-alt"></span>
                                                    <h3>Top Clientes</h3>
                                                </div>
                                                <!-- .widget-header -->

                                                <div class="widget-content">
                                                    <table class="table table-bordered table-striped">

                                                        <thead>
                                                            <tr>
                                                                <th>Cliente</th>
                                                                <th>Importe</th>
                                                                <th></th>
                                                            </tr>

                                                        </thead>

                                                        <tbody>
                                                            <asp:PlaceHolder ID="phTopClientes" runat="server"></asp:PlaceHolder>
                                                        </tbody>

                                                    </table>

                                                </div>
                                                <!-- .widget-content -->

                                            </div>
                                            <!-- /widget -->

                                        </div>
                                        <!-- /span4 -->
                                    </div>
                                    <div class="tab-pane fade" id="profile">
                                        <div class="col-md-8">

                                            <div class="widget stacked widget-table">

                                                <div class="widget-header">
                                                    <span class="icon-list-alt"></span>
                                                    <h3>Top Vendedores</h3>
                                                </div>
                                                <!-- .widget-header -->

                                                <div class="widget-content">
                                                    <table class="table table-bordered table-striped">

                                                        <thead>
                                                            <tr>
                                                                <th style="text-align: left; width: 70%">Vendedor</th>
                                                                <th style="text-align: left; width: 30%">Importe</th>
                                                            </tr>

                                                        </thead>

                                                        <tbody>
                                                            <asp:PlaceHolder ID="phTopVendedores" runat="server"></asp:PlaceHolder>
                                                        </tbody>

                                                    </table>

                                                </div>
                                                <!-- .widget-content -->

                                            </div>
                                            <!-- /widget -->

                                        </div>
                                        <!-- /span4 -->
                                    </div>
                                </div>
                            </div>
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
                                    <div class="form-group" style="display: none">
                                        <label class="col-md-4" style="display: none">Desde</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtFechaDesde" runat="server" Text="01/01/2000" Style="display: none" class="form-control"></asp:TextBox>
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
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaHasta" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                        <!-- /input-group -->

                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Sucursal</label>
                                        <div class="col-md-4">
                                            <asp:DropDownList ID="DropListSucursal" runat="server" class="form-control" disabled></asp:DropDownList>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaHasta" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                        <!-- /input-group -->

                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Buscar Cliente</label>
                                        <div class="col-md-4">
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
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListClientes" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Vendedor</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListVendedores" runat="server" class="form-control"></asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListVendedores" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
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
                                    <div class="form-group">
                                        <label class="col-md-4">Estado</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ListEstado" runat="server" class="form-control">                                                
                                                <asp:ListItem Value="0">Impagas</asp:ListItem>
                                                <asp:ListItem Value="1">Vencidas</asp:ListItem>
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
                        <asp:LinkButton ID="lbtnBuscar" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnBuscar_Click" ValidationGroup="BusquedaGroup" />
                    </div>
                </div>

            </div>
        </div>

        <div id="modalEnvioSMS" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Datos de envio sms</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">   
                            <div class="form-group">
                                <label class="col-md-2">Telefono</label>
                                <div class="col-md-3">
                                    <div class="input-group">
                                        <span class="input-group-addon">0</span>
                                        <asp:TextBox ID="txtCodArea" runat="server" class="form-control" MaxLength="4" onkeypress="javascript:return validarNroSinComa(event)"/>
                                    </div>
                                </div> 
                                <div class="col-md-3">
                                    <asp:TextBox ID="txtTelefono" runat="server" class="form-control" MaxLength="8" onkeypress="javascript:return validarNroSinComa(event)"/>
                                </div> 
                                <div class="col-md-2">
                                    <asp:RegularExpressionValidator ErrorMessage="*" Font-Bold="true" ForeColor="Red" SetFocusOnError="true" ControlToValidate="txtCodArea" runat="server" ValidationGroup="TelefonoGroup" ValidationExpression="^[1-9][0-9]{1,3}$" />
                                    <asp:RequiredFieldValidator ErrorMessage="*"  Font-Bold="true" ForeColor="Red"  SetFocusOnError="true"  ControlToValidate="txtCodArea" runat="server" ValidationGroup="TelefonoGroup" />
                                    <asp:RequiredFieldValidator ErrorMessage="*"  Font-Bold="true" ForeColor="Red"  SetFocusOnError="true"  ControlToValidate="txtTelefono" runat="server" ValidationGroup="TelefonoGroup" />
                                </div>  
                            </div>                         
                            <div class="form-group">
                                <label class="col-md-2">Mensaje</label>
                                <div class="col-md-8">
                                    <asp:TextBox ID="txtMensajeSMS" runat="server" class="form-control" Rows="4" TextMode="MultiLine" onkeypress="javascript:return validarEnter(event)"/>
                                </div> 
                            </div>
                            <asp:TextBox runat="server" ID="txtIdEnvioSMS" Text="0" style="display:none;"></asp:TextBox>
                        </div>                        
                    </div>
                    <div class="modal-footer">
                        <asp:UpdatePanel ID="UpdatePanelSms" runat="server">
                            <ContentTemplate>
                                <asp:LinkButton ID="lbtnEnviarSMS" runat="server" Text="<span class='shortcut-icon fa fa-paper-plane'></span>" class="btn btn-success" OnClick="lbtnEnviarSMS_Click" ValidationGroup="TelefonoGroup"></asp:LinkButton>                            
                            </ContentTemplate>
                        </asp:UpdatePanel>                        
                    </div>

                </div>
            </div>
        </div>

        <!-- /container -->

    </div>
    <!-- /main -->

    <link href="../../css/pages/reports.css" rel="stylesheet">
    <!-- Core Scripts - Include with every page -->
    <script src="../../Scripts/jquery-1.10.2.js"></script>
    <script src="../../Scripts/bootstrap.min.js"></script>

    <script src="../../Scripts/libs/jquery-1.9.1.min.js"></script>
    <script src="../../Scripts/libs/jquery-ui-1.10.0.custom.min.js"></script>
    <script src="../../Scripts/libs/bootstrap.min.js"></script>
    <script src="../../Scripts/Application.js"></script>

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

    <script>
        function openModal() {
            $('#modalEnvioSMS').modal('show');
        }
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
                if (key == 46 || key == 8)// || key == 44) // Detectar . (punto) , backspace (retroceso) y , (coma)
                { return true; }
                else
                { return false; }
            }
            return true;
        }
        //valida los campos solo numeros
        function validarNroSinComa(e) {
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
                if (key == 8)// || key == 44) // Detectar . (punto) , backspace (retroceso) y , (coma)
                { return true; }
                else
                { return false; }
            }
            return true;
        }
        function validarEnter(e) {
            var key;
            if (window.event) // IE
            {
                key = e.keyCode;
            }
            else if (e.which) // Netscape/Firefox/Opera
            {
                key = e.which;
            }

            if (key == 13) {
                return false;
            }
            return true;
        }
    </script>

</asp:Content>

