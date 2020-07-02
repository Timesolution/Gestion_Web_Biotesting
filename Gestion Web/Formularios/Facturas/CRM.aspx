<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CRM.aspx.cs" Inherits="Gestion_Web.Formularios.Facturas.CRM" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <%--<div class="container">--%>
    <div>

        <div class="col-md-12 col-xs-12">
            <div class="widget stacked">

                <div class="stat">
                    <h5><i class="icon-map-marker"></i>Ventas > CRM</h5>
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
                                <!-- /btn-group -->
                            </td>
                            <td style="width: 63%">
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

                                </div>

                            </td>

                            <td style="width: 5%">
                                <div class="shortcuts" style="height: 100%">



                                    <a class="btn btn-primary" data-toggle="modal" href="#modalBusqueda" style="width: 100%">
                                        <i class="shortcut-icon icon-filter"></i>
                                    </a>
                                </div>
                            </td>
                            <td style="width: 5%">
                                <div class="shortcuts" style="height: 100%">

                                    <a class="btn btn-primary ui-tooltip" data-toggle="tooltip" title data-original-title="Agregar" style="width: 100%">
                                        <i class="shortcut-icon icon-plus"></i>
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
                            <h4>Total</h4>
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
                    <i class="icon-th-list" style="width: 2%"></i>
                    <h3 style="width: 75%">CRM
                    </h3>
                    <h3>
                        <asp:Label ID="lblSaldo" runat="server" Style="text-align: right" Text="" ForeColor="#666666" Font-Bold="true" Visible="true"></asp:Label>
                    </h3>
                </div>
                <div class="widget-content">
                    <div class="panel-body">

                        <div class="col-md-12 col-xs-12">
                            <div class="table-responsive">
                                <a class="btn btn-info" style="display: none" data-toggle="modal" id="abreDialog" href="#modalFacturaDetalle">Agregar Tipo Cliente</a>
                                <table class="table table-striped table-bordered table-hover" id="dataTables-example">

                                    <thead>
                                        <tr>

                                         <tr>
                                            <th>Cliente</th>
                                            <th>Fecha</th>
                                            <th>Detalle</th>
                                            <th>Tarea</th>
                                            <th style="text-align: right;">Vencimiento</th>
                                            <th style="text-align: right;">Estado</th>
                                            <th style="text-align: right;">Usuario</th>
                                            <th class="td-actions" style="width: 1%"></th>
                                        </tr>

                                    </thead>
                                    <tbody>
                                        <asp:PlaceHolder ID="phFacturas" runat="server"></asp:PlaceHolder>
                                    </tbody>
                                </table>

                            </div>
                        </div>
                        <!-- /.content -->
                    </div>
                </div>
            </div>

        </div>

        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Visible="false" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="80%">
        </rsweb:ReportViewer>

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
                                        <label class="col-md-4">Fecha</label>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtFechaDesde" placeholder="Desde" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtFechaHasta" placeholder="Hasta" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RadioButton ID="RdFecha" Checked="true" runat="server" GroupName="fecha" />
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaDesde" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaHasta" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-md-4">Fecha vencimiento</label>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtVencimientoDesde" placeholder="Desde" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtVencimientoHasta" placeholder="Hasta" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RadioButton ID="RdFechaVencimiento" runat="server" GroupName="fecha" />
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaDesde" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaHasta" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>


                                    <div class="form-group">
                                        <label class="col-md-4">Cod Cliente</label>
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
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-md-4">Estado</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListEstado" runat="server" class="form-control">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListEstado" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-md-4">Usuario</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListUsuarios" runat="server" class="form-control">
                                            </asp:DropDownList>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <label class="col-md-4">Descripcion</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtDescripcion" class="form-control" runat="server"></asp:TextBox>
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

    <div id="modalConfirmarFinalizado" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" id="btnCerrarModalConfirmacion2" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Finalizar</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <label style="font-size: large">Desea finalizar la tarea?</label>
                    </div>
                    <div class="col-md-3">
                           <asp:TextBox runat="server" ID="txtMovimiento" Text="0" Style="display: none"></asp:TextBox>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbtnConfirmarEventoFinalizado" OnClick="lbtnConfirmarEventoFinalizado_Click" runat="server" Text="Confirmar" class="btn btn-success" />
                    <button type="button" class="btn btn-danger" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                </div>
            </div>
        </div>
    </div>


    <script src="../../Scripts/JSFunciones1.js"></script>

    <script>

        function abrirdialog() {
            document.getElementById('abreDialog').click();
        }

    </script>

    <script type="text/javascript">

        function abrirdialog(valor) {
            document.getElementById('<%= txtMovimiento.ClientID %>').value = valor;
        }
    </script>

    <script type="text/javascript">
        function openModalMail() {
            $('#modalEnvioMail').modal('show');
        }
    </script>


    <script type="text/javascript">
        function openModal() {
            $('#modalFacturar').modal('show');
        }
    </script>
    <script type="text/javascript">
        function openModalEditarFactura() {
            $('#modalEditarFactura').modal('show');
        }
    </script>
    <script type="text/javascript">
        function openModalAgregarFactura() {
            $('#modalAgregarFactura').modal('show');
        }
    </script>
    <script type="text/javascript">
        function openModalNotaDebitoCreditoDiferenciaCambio() {
            $('#modalNotaDebitoCreditoDiferenciaCambio').modal('show');
        }
    </script>


    <link href="../../css/pages/reports.css" rel="stylesheet">
    <!-- Core Scripts - Include with every page -->
    <script src="../../Scripts/jquery-1.10.2.js"></script>
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

    <%--<script src="//code.jquery.com/jquery-1.9.1.js"></script>--%>
    <script src="//code.jquery.com/ui/1.10.4/jquery-ui.js"></script>

    <script>
        function pageLoad() {
            $("#<%= txtFechaDesde.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
                $("#<%= txtFechaHasta.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        }
    </script>



    <script>
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
                if (key == 46 || key == 8 || key == 44)// Detectar . (punto) y backspace (retroceso) y , (coma)
                { return true; }
                else { return false; }
            }
            return true;
        }
    </script>


    </div>
</asp:Content>
