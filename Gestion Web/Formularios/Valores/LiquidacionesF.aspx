<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LiquidacionesF.aspx.cs" Inherits="Gestion_Web.Formularios.Valores.LiquidacionesF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">

        <div>
            <div class="col-md-12 col-xs-12">
                <div class="widget stacked">
                    <div class="stat">
                        <h5><i class="icon-map-marker"></i>&nbsp Valores > Tarjetas > Liquidaciones</h5>
                    </div>
                    <div class="widget-header">
                        <i class="icon-wrench"></i>
                        <h3>Herramientas</h3>
                    </div>
                    <!-- /widget-header -->

                    <div class="widget-content">


                        <table style="width: 100%">
                            <tr>
                                <td style="width: 20%">
                                    <div class="btn-group">
                                        <button type="button" class="btn btn-success dropdown-toggle" data-toggle="dropdown" id="btnAccion" runat="server">Accion    <span class="caret"></span></button>
                                        <ul class="dropdown-menu">
                                            
                                        </ul>
                                    </div>
                                    <!-- /btn-group -->
                                </td>
                                <td style="width: 70%">
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

            <div class="col-md-12 col-xs-12">
                <div class="widget big-stats-container stacked">
                    <div class="widget-content">

                        <div id="big_stats" class="cf">
                            <div class="stat">
                                <h4>Saldo</h4>
                                <asp:Label ID="lblSaldo" runat="server" Text="" class="value"></asp:Label>
                            </div>
                            <!-- .stat -->
                        </div>

                    </div>


                    <!-- /widget-content -->

                </div>
                <div class="widget stacked widget-table action-table">

                    <div class="widget-header">
                        <i class="icon-money" style="width: 2%"></i>
                        <h3 style="width: 75%">Tarjetas
                                    
                        </h3>
                    </div>
                    <div class="widget-content">
                        <div class="panel-body">

                            <%--<div class="col-md-12 col-xs-12">--%>
                            <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <div class="table-responsive">
                                        <a class="btn btn-info" style="display: none" data-toggle="modal" id="abreDialog" href="#modalCargar"></a>
                                        <table class="table table-striped table-bordered table-hover" id="dataTables-example">
                                            <thead>
                                                <tr>                                                    
                                                    <th>Operador</th>
                                                    <th>Liquidacion</th>
                                                    <th>Total</th>
                                                    <th>Total Liquidado</th>
                                                    <th>Fecha Liquidacion</th>
                                                    <th>Cta Bancaria</th>
                                                    <th></th>
                                                </tr>

                                            </thead>
                                            <tbody>
                                                <asp:PlaceHolder ID="phLiquidaciones" runat="server"></asp:PlaceHolder>
                                            </tbody>
                                        </table>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <!-- /.content -->
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
                            <asp:UpdatePanel runat="server">
                                <ContentTemplate>                                
                                    <div class="form-group">
                                        <label class="col-md-3">
                                            Desde
                                        </label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtFechaDesde" placeholder="Desde" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaDesde" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-3">
                                            Hasta
                                        </label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtFechaHasta" placeholder="Hasta" runat="server" class="form-control"></asp:TextBox>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaHasta" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-3">Tipo Fecha</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ListTipoFecha" runat="server" class="form-control" >
                                                <asp:ListItem Text="Acreditacion" Value="0" />
                                                <asp:ListItem Text="Operacion" Value="1" />
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-3">Empresa</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ListEmpresa" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ListEmpresa_SelectedIndexChanged" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-3">Sucursal</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ListSucursal" runat="server" class="form-control" />
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>

                    <div class="modal-footer">
                        <asp:LinkButton ID="lbtnBuscar" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnBuscar_Click" ValidationGroup="BusquedaGroup" />
                    </div>
                </div>

            </div>
        </div>

        <div id="modalCargar" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">

                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Liquidar</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <ContentTemplate>                                
                                    <div class="form-group">
                                        <label class="col-md-3">Operador</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtOperadorCargar" runat="server" class="form-control" disabled></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-3">Liquidacion</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtNroLiquidacionCargar" runat="server" class="form-control" disabled></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-3">Total Liquidado</label>
                                        <div class="col-md-4">
                                            <div class="input-group">
                                                <span class="input-group-addon">$</span>
                                                <asp:TextBox ID="txtTotalLiquidadoCargar" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                            </div>                                            
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ErrorMessage="*" Font-Bold="true" ForeColor="Red" ControlToValidate="txtTotalLiquidadoCargar" runat="server" ValidationGroup="CargarGroup" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-3">Fecha Liquidado</label>
                                        <div class="col-md-4">
                                            <div class="input-group">
                                                <span class="input-group-addon"><i class="icon-calendar"></i></span>
                                                <asp:TextBox ID="txtFechaLiquidadoCargar" runat="server" class="form-control" ></asp:TextBox>
                                            </div>                                            
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ErrorMessage="*" Font-Bold="true" ForeColor="Red" ControlToValidate="txtFechaLiquidadoCargar" runat="server" ValidationGroup="CargarGroup"/>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-3">Cuenta Bancaria</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ListCuentaBcoCargar" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ErrorMessage="*" Font-Bold="true" ForeColor="Red" InitialValue="-1" ControlToValidate="ListCuentaBcoCargar" runat="server" ValidationGroup="CargarGroup"/>
                                        </div>
                                    </div>
                                    <asp:Label ID="lblIdMov" Text="0" runat="server" style="display:none;" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>

                    <div class="modal-footer">
                        <asp:LinkButton ID="lbtnCargarLiquidacion" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" ValidationGroup="CargarGroup" OnClick="lbtnCargarLiquidacion_Click"/>
                    </div>
                </div>

            </div>
        </div>
        
        <link href="../../css/pages/reports.css" rel="stylesheet">


        <!-- Core Scripts - Include with every page -->
        <%--<script src="../../Scripts/jquery-1.10.2.js"></script>--%>
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

        <!-- Page-Level Plugin Scripts - Tables -->
        <script src="//cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>
        <script src="//cdn.datatables.net/plug-ins/1.10.9/sorting/date-eu.js"></script>
        <script src="../../Scripts/plugins/dataTables/custom.tables.js"></script>

        <link href="//cdn.datatables.net/1.10.2/css/jquery.dataTables.css" rel="stylesheet" />

        <script>
            function abrirdialog() {
                document.getElementById('abreDialog').click();
            }
        </script>        

        <script>
            $(document).ready(function () {
                $('#dataTables-example').dataTable({
                    "bLengthChange": false,
                    "bFilter": false,
                    "bInfo": false,
                    "bAutoWidth": false,
                    "columnDefs": [ { type: 'date-eu', targets: 4 } ]
                });
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
                    if (key == 46 || key == 8 ) // Detectar . (punto) , backspace (retroceso) y , (coma)
                    { return true; }
                    else
                    { return false; }
                }
                return true;
            }
        </script>  
        
        <script>
            function pageLoad() {
                $("#<%= txtFechaLiquidadoCargar.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
                $("#<%= txtFechaDesde.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
                $("#<%= txtFechaHasta.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
                
            }
        </script>

        <script type="text/javascript">
            function openModal() {
                $('#modalEditar').modal('show');
            }
        </script>


    </div>
</asp:Content>

