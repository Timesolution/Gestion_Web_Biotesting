<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PagaresF.aspx.cs" Inherits="Gestion_Web.Formularios.Valores.PagaresF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">
        <div>
            <div class="col-md-12 col-xs-12">
                <div class="widget stacked">
                    <div class="stat">
                        <h5><i class="icon-map-marker"></i>Valores > Pagares</h5>
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
                                            <li>
                                                <asp:LinkButton ID="btnExportar" runat="server" Visible="false">Exportar</asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="btnLiquidar" runat="server" data-toggle="modal" href="#modalLiquidar">Liquidar</asp:LinkButton>
                                            </li>
                                        </ul>
                                    </div>
                                    <!-- /btn-group -->
                                </td>
                                <td style="width: 60%">
                                    <h5>
                                        <asp:Label runat="server" ID="lblParametros" Text="" ForeColor="#cccccc"></asp:Label>
                                    </h5>
                                </td>

                                <td style="width: 5%">
                                    <div class="shortcuts" style="height: 100%">

                                        <div class="shortcuts" style="height: 100%">
                                            <asp:LinkButton ID="lbImpresion" class="btn btn-primary" runat="server" Text="<span class='shortcut-icon icon-print'></span>" Visible="false" Style="width: 100%" />
                                        </div>
                                    </div>
                                </td>

                                <td style="width: 2%">

                                    <div class="btn-group pull-right" style="width: 100%">
                                        <button type="button" class="btn btn-primary dropdown-toggle ui-tooltip" title data-original-title="Listados" data-toggle="dropdown">
                                            <i class="shortcut-icon icon-print"></i>&nbsp
                                       
                                        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                                            <span class="caret"></span>
                                        </button>
                                        <ul class="dropdown-menu dropdown-menu-right" role="menu" aria-labelledby="dropdownMenu">
                                            <li class="dropdown-submenu dropdown-menu-right"><a tabindex="-1" href="#">Pagarés</a>
                                                <ul class="dropdown-menu">
                                                    <li>
                                                        <asp:LinkButton ID="btnReporteExcel" runat="server" OnClick="btnReporteExcel_Click">
                                                        <i class="fa fa-file-excel-o" aria-hidden="true"></i>
                                                        &nbsp Exportar
                                                        </asp:LinkButton>
                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="btnReportePdf" runat="server" OnClick="btnReportePdf_Click">
                                                        <i class="fa fa-file-pdf-o" aria-hidden="true"></i>
                                                        &nbsp Imprimir
                                                        </asp:LinkButton>
                                                    </li>
                                                </ul>
                                            </li>
                                        </ul>
                                    </div>

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
                                <h4>Total</h4>
                                <asp:Label ID="lblSaldo" runat="server" Text="" class="value"></asp:Label>
                            </div>
                            <!-- .stat -->
                            <div class="stat">
                                <h4>Comision</h4>
                                <asp:Label ID="lblComision" runat="server" Text="$0" class="value"></asp:Label>
                            </div>
                            <!-- .stat -->
                        </div>

                    </div>


                    <!-- /widget-content -->

                </div>
                <div class="widget stacked widget-table action-table">

                    <div class="widget-header">
                        <i class="icon-money" style="width: 2%"></i>
                        <h3 style="width: 75%">Pagares
                                    
                        </h3>
                    </div>
                    <div class="widget-content">
                        <div class="panel-body">

                            <%--<div class="col-md-12 col-xs-12">--%>
                            <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <div class="table-responsive">
                                        <table class="table table-striped table-bordered table-hover" id="dataTables-example">
                                            <thead>
                                                <tr>
                                                    <th>Fecha</th>
                                                    <th>Sucursal</th>
                                                    <th>Documento</th>
                                                    <th>Mutual</th>
                                                    <th># Socio</th>
                                                    <th>#Autorizacion</th>
                                                    <th>Numero</th>
                                                    <th style="text-align: right;">Importe</th>
                                                    <th>Vencimiento</th>
                                                    <th>Cuota</th>
                                                    <th>Estado</th>
                                                    <th></th>
                                                </tr>

                                            </thead>
                                            <tbody>
                                                <asp:PlaceHolder ID="phPagares" runat="server"></asp:PlaceHolder>
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

                            <div class="form-group">
                                <label class="col-md-3">
                                    Fecha Pagare
                               
                                </label>
                                <div class="col-md-3">
                                    <asp:TextBox ID="txtFechaDesde" placeholder="Desde" runat="server" class="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <asp:TextBox ID="txtFechaHasta" placeholder="Hasta" runat="server" class="form-control"></asp:TextBox>
                                    <!-- /input-group -->
                                </div>
                                <div class="col-md-1">
                                    <asp:RadioButton ID="RadioFecha" Checked="true" runat="server" GroupName="fecha" />
                                    <!-- /input-group -->
                                </div>
                                <div class="col-md-1">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaDesde" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-1">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaHasta" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                            </div>

                            <div class="form-group">
                                <label class="col-md-3">
                                    Fecha Vencimiento
                               
                                </label>
                                <div class="col-md-3">
                                    <asp:TextBox ID="txtDesdeVto" placeholder="Desde" runat="server" class="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <asp:TextBox ID="txtHastaVto" placeholder="Hasta" runat="server" class="form-control"></asp:TextBox>
                                    <!-- /input-group -->
                                </div>
                                <div class="col-md-1">
                                    <asp:RadioButton ID="RadioFechaVto" runat="server" GroupName="fecha" />
                                    <!-- /input-group -->
                                </div>
                                <div class="col-md-1">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtDesdeVto" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-1">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtHastaVto" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-3">Mutual</label>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="ListMutuales" runat="server" class="form-control"></asp:DropDownList>
                                    <!-- /input-group -->
                                </div>
                                <div class="col-md-2">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="ListMutuales" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-3">Sucursal</label>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="DropListSucursal" runat="server" class="form-control"></asp:DropDownList>
                                    <!-- /input-group -->
                                </div>
                                <div class="col-md-2">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListSucursal" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-3">Estado</label>
                                <div class="col-md-6">
                                    <asp:DropDownList ID="DropListEstado" runat="server" class="form-control"></asp:DropDownList>
                                    <!-- /input-group -->
                                </div>
                                <div class="col-md-2">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListEstado" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                    </div>


                    <div class="modal-footer">
                        <asp:LinkButton ID="lbtnBuscar" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnBuscar_Click" ValidationGroup="BusquedaGroup" />
                    </div>
                </div>

            </div>
        </div>

        <div id="modalLiquidar" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">

                    <div class="modal-header">
                        <button ID="btnCerrarModalLiquidacion" type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Agregar Liquidación</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <div class="form-group">
                                <label class="col-md-4">Numero Liquidación:</label>
                                <div class="col-md-6">
                                    <asp:TextBox ID="txtNumeroLiquidacion" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)" MaxLength="20"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:UpdatePanel ID="UpdatePanelLiquidar" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <asp:Button ID="btnSi" runat="server" Text="Confirmar" class="btn btn-success" OnClick="btnSi_Click" ValidationGroup="LiquidarGroup" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
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
                    "columnDefs": [
                        { type: 'date-eu', targets: [0, 7] }
                    ]
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
                    if (key == 46 || key == 8) // Detectar . (punto) , backspace (retroceso) y , (coma)
                    { return true; }
                    else
                    { return false; }
                }
                return true;
            }
        </script>

        <script>
            function pageLoad() {
                $("#<%= txtFechaDesde.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
                $("#<%= txtFechaHasta.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            }
        </script>

        <script>

            $(function () {
                $("#<%= txtDesdeVto.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            });

            $(function () {
                $("#<%= txtHastaVto.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            });
            $(function () {
                $("#<%= txtFechaDesde.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            });

            $(function () {
                $("#<%= txtFechaHasta.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            });

        </script>

        <script type="text/javascript">
            function openModal() {
                $('#modalEditar').modal('show');
            }
        </script>

        <script type="text/javascript">
            function cerrarModalLiquidacion() {
                document.getElementById('btnCerrarModalLiquidacion').click();
            }
        </script>


    </div>
</asp:Content>

