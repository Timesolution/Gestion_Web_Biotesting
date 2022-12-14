<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TareasF.aspx.cs" Inherits="Gestion_Web.Formularios.Herramientas.TareasF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">
        <div class="container">
            <div class="col-md-12 col-xs-12">
                <div class="widget stacked">

                    <div class="widget-header">
                        <i class="icon-wrench"></i>
                        <h3>Herramientas</h3>
                    </div>
                    <!-- /widget-header -->

                    <div class="widget-content" style="padding-top: 15px;">


                        <table style="width: 100%">
                            <tr>

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
            <div class="col-md-12">

                <div class="widget big-stats-container stacked">
                    <div class="widget-content">
                        <div id="big_stats" class="cf">
                            <div class="stat">
                                <h4>Total Tareas</h4>
                                <asp:Label ID="labelTotal" runat="server" Text="" class="value"></asp:Label>
                            </div>
                        </div>
                        <!-- /widget-content -->

                    </div>
                    <!-- /widget -->
                </div>

                
                <div class="widget stacked widget-table action-table">

                        <div class="widget-header">
                            <h3 style="width: 75%"><b><i class="shorcut-icon fa fa-phone"></i></b>&nbsp Codigos                                    
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
                                                        <th>Cliente</th>
                                                        <th>Telefono</th>
                                                        <th>Mensaje</th>
                                                        <th>Estado</th>
                                                    </tr>

                                                </thead>
                                                <tbody>
                                                    <asp:PlaceHolder ID="phTareas" runat="server"></asp:PlaceHolder>
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

            <div id="modalBusqueda" class="modal fade" tabindex="-1" role="dialog">
                <div class="modal-dialog">
                    <div class="modal-content">

                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h4 class="modal-title">Busqueda</h4>
                        </div>
                        <div class="modal-body">
                            <div role="form" class="form-horizontal col-md-12">
                                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
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
                                            <label class="col-md-4">Estado</label>
                                            <div class="col-md-6">
                                                <asp:DropDownList ID="ListEstado" runat="server" CssClass="form-control">
                                                    <asp:ListItem Text="Todos" Value="0" />
                                                    <asp:ListItem Text="Pendientes" Value="1" />
                                                    <asp:ListItem Text="Enviados" Value="2" />                                                    
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="ListEstado" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                            </div>
                                            <!-- /input-group -->
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

        </div>
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

        <!-- Page-Level Plugin Scripts - Tables -->
        <script src="//cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>
        <script src="//cdn.datatables.net/plug-ins/1.10.9/sorting/date-eu.js"></script>
        <script src="../../Scripts/plugins/dataTables/custom.tables.js"></script>

        <link href="//cdn.datatables.net/1.10.2/css/jquery.dataTables.css" rel="stylesheet" />

        <script>

            $(document).ready(function () {
                $('#dataTables-example').dataTable({
                    "bLengthChange": false,
                    "pageLength": 15,
                    "bFilter": true,
                    "bInfo": false,
                    "bAutoWidth": false,
                    "columnDefs": [
                    { type: 'date-eu', targets: [0] }
                    ],
                    "language": {
                        "search": "Buscar:"
                    }
                });
            });


            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endReq);
            function endReq(sender, args) {
                $('#dataTables-example').dataTable();
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
        </script>

    </div>
</asp:Content>
