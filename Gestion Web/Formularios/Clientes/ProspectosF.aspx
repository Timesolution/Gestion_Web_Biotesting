﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProspectosF.aspx.cs" Inherits="Gestion_Web.Formularios.Clientes.ProspectosF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%--<div class="container">--%>
    <div>

        <div class="col-md-12 col-xs-12">
            <div class="widget stacked">

                <div class="stat">
                    <h5><i class="icon-map-marker"></i>Clientes > Prospectos</h5>
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
                            </td>
                            <td style="width: 63%">
                                <h5>
                                    <asp:Label runat="server" ID="lblParametros" Text="" ForeColor="#cccccc"></asp:Label>
                                </h5>
                            </td>
                            <td style="width: 5%">
                            </td>
                            <td style="width: 2%">
                            </td>
                            <td style="width: 5%">
                                <div class="shortcuts" style="height: 100%">
                                    <a href="ProspectosABM.aspx?a=1" class="btn btn-primary ui-tooltip" data-toggle="tooltip" title data-original-title="Agregar" style="width: 100%">
                                        <i class="shortcut-icon icon-plus"></i>
                                    </a>
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
            <div class="widget widget-table">
                <div class="widget-header">
                    <i class="icon-th-list" style="width: 2%"></i>
                    <h3 style="width: 75%">Prospectos
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
                                            <th>Nombre y apellido</th>
                                            <th>Fecha Nacimiento</th>
                                            <th>Tipo documento</th>
                                            <th>Documento</th>
                                            <th class="td-actions" style="width: 10%"></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:PlaceHolder ID="phProspectos" runat="server"></asp:PlaceHolder>
                                    </tbody>
                                </table>

                            </div>
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
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>

                                   <div class="form-group">
                                        <label class="col-md-4">Fecha nacimiento</label>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtFechaDesde" placeholder="Desde" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtFechaHasta" placeholder="Hasta" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaDesde" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaHasta" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>

                                    <%--<div class="form-group">
                                        <label class="col-md-4">Prospecto</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListEstado" runat="server" class="form-control">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListEstado" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>--%>

                                    <div class="form-group">
                                        <label class="col-md-4">Documento</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtDocumento" placeholder=" 25515265" class="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>

                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>


                    <div class="modal-footer">
                        <asp:LinkButton ID="lbtnBuscar" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnBuscar_Click" ValidationGroup="BusquedaGroup" />
                    </div>
                </div>
            </div>
        </div>

    <div id="modalConfirmar" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Eliminar</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <label style="font-size: large">Desea eliminar el registro?</label>
                    </div>
                    <div class="col-md-3">
                           <asp:TextBox runat="server" ID="txtMovimiento" Text="0" Style="display: none"></asp:TextBox>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbtnEliminarProspecto" OnClick="lbtnEliminarProspecto_Click" runat="server" Text="Confirmar" class="btn btn-success" />
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
