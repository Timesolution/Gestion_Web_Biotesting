﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DiferenciasMercaderiaF.aspx.cs" Inherits="Gestion_Web.Formularios.Facturas.DiferenciasMercaderiaF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">

        <div>
            <div class="col-md-12 col-xs-12 hidden-print">
                <div class="widget stacked">

                    <div class="stat">
                        <h5><i class="icon-map-marker"></i>Ventas > Ventas > Diferencias Mercaderias</h5>
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
                                        <button type="button" class="btn btn-success dropdown-toggle" data-toggle="dropdown" id="btnAccion" runat="server">Accion<span class="caret"></span></button>
                                        <ul class="dropdown-menu">
                                            <li>
                                                <asp:LinkButton ID="lbtnCorregirOrigen" runat="server" data-toggle="modal" Visible="false" href="#modalConfirmacionOrigen">Subir dif. stock suc. origen</asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="lbtnCorregirDestino" runat="server" data-toggle="modal" Visible="true" href="#modalConfirmacionDestino">Subir dif. stock suc. destino</asp:LinkButton>
                                            </li>
                                            <li>
                                                <asp:LinkButton ID="lbtnResolucionAdministrativa" runat="server" data-toggle="modal" Visible="false" href="#modalResolucionAdministrativa">Resolucion administrativa</asp:LinkButton>
                                            </li>
                                        </ul>
                                    </div>
                                </td>
                                <td style="width: 65%"></td>
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
                </div>
            </div>
            <div class="col-md-12 col-xs-12">
                <div class="widget widget-table">

                    <div class="widget-header">
                        <i class="icon-th-list" style="width: 2%"></i>
                        <h3 style="width: 75%">Facturas</h3>
                    </div>
                    <div class="widget-content">
                        <div class="panel-body">
                            <div class="table-responsive">
                                <asp:UpdatePanel ID="UpdatePanel8" UpdateMode="Always" runat="server">
                                    <ContentTemplate>
                                        <a class="btn btn-info" style="display: none" data-toggle="modal" id="abreDialog" href="#Comentario">Agregar Tipo Cliente</a>
                                        <table class="table table-striped table-bordered table-hover" id="dataTablesR-example">
                                            <thead>
                                                <tr>
                                                    <th>Fecha</th>
                                                    <th>Numero Factura</th>
                                                    <th>Sucursal Origen</th>
                                                    <th>Sucursal Destino</th>
                                                    <th>Articulo</th>
                                                    <th>Cantidad Enviada</th>
                                                    <th>Cantidad Recibida</th>
                                                    <th>Diferencia</th>
                                                    <th>Estado</th>
                                                    <th></th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:PlaceHolder ID="phFacturas" runat="server"></asp:PlaceHolder>
                                            </tbody>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>


            <div id="modalConfirmacionOrigen" class="modal fade" tabindex="-1" role="dialog">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h4 class="modal-title">Confirmacion subir stock</h4>
                        </div>
                        <div class="modal-body">
                            <div role="form" class="form-horizontal col-md-12">
                                <div class="form-group">
                                    <div class="col-md-2">
                                        <h1>
                                            <i class="icon-warning-sign" style="color: orange"></i>
                                        </h1>
                                    </div>
                                    <div class="col-md-7">
                                        <h5>
                                            <asp:Label runat="server" ID="Label1" Text="Esta seguro que desea subir stock en la sucursal de origen?" Style="text-align: center"></asp:Label>
                                        </h5>
                                    </div>

                                    <div class="col-md-3">
                                        <asp:TextBox runat="server" ID="TextBox1" Text="0" Style="display: none"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="name" class="col-md-3">Observacion</label>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtConfirmacionOrigen" runat="server" class="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtConfirmacionOrigen" ValidationGroup="ConfirmacionOrigen" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:Button runat="server" ID="btnSubirStockOrigen" Text="Aceptar" class="btn btn-success" OnClick="btnSubirStockOrigen_Click" ValidationGroup="ConfirmacionOrigen" />
                                <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div id="modalResolucionAdministrativa" class="modal fade" tabindex="-1" role="dialog">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h4 class="modal-title">Confirmacion subir stock</h4>
                        </div>
                        <div class="modal-body">
                            <div role="form" class="form-horizontal col-md-12">
                                <div class="form-group">
                                    <div class="col-md-2">
                                        <h1>
                                            <i class="icon-warning-sign" style="color: orange"></i>
                                        </h1>
                                    </div>
                                    <div class="col-md-7">
                                        <h5>
                                            <asp:Label runat="server" ID="Label3" Text="Esta seguro que desea realizar una resolucion administrativa?" Style="text-align: center"></asp:Label>
                                        </h5>
                                    </div>

                                    <div class="col-md-3">
                                        <asp:TextBox runat="server" ID="TextBox3" Text="0" Style="display: none"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="name" class="col-md-3">Observacion</label>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtConfirmacionAdministrativa" runat="server" class="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtConfirmacionAdministrativa" ValidationGroup="ResolucionAdministrativa" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>

                            <div class="modal-footer">
                                <asp:Button runat="server" ID="btnResolucionAdministrativa" Text="Aceptar" class="btn btn-success" OnClick="btnResolucionAdministrativa_Click" ValidationGroup="ResolucionAdministrativa" />
                                <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div id="modalConfirmacionDestino" class="modal fade" tabindex="-1" role="dialog">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h4 class="modal-title">Confirmacion subir stock</h4>
                        </div>
                        <div class="modal-body">
                            <div role="form" class="form-horizontal col-md-12">
                                <div class="form-group">
                                    <div class="col-md-2">
                                        <h1>
                                            <i class="icon-warning-sign" style="color: orange"></i>
                                        </h1>
                                    </div>
                                    <div class="col-md-7">
                                        <h5>
                                            <asp:Label runat="server" ID="Label2" Text="Esta seguro que desea subir stock en la sucursal de destino?" Style="text-align: center"></asp:Label>
                                        </h5>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:TextBox runat="server" ID="TextBox8" Text="0" Style="display: none"></asp:TextBox>
                                    </div>

                                </div>
                                <div class="form-group">
                                    <label for="name" class="col-md-3">Observacion</label>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtConfirmacionDestino" runat="server" class="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtConfirmacionDestino" ValidationGroup="ConfirmacionDestino" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:Button runat="server" ID="btnSubirStockDestino" Text="Aceptar" class="btn btn-success" OnClick="btnSubirStockDestino_Click" ValidationGroup="ConfirmacionDestino" />
                                <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                            </div>
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

            <div id="modalBusqueda" class="modal fade" tabindex="-1" role="dialog">
                <div class="modal-dialog">
                    <div class="modal-content">

                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h4 class="modal-title">Busqueda</h4>
                        </div>
                        <div class="modal-body">
                            <asp:UpdatePanel runat="server" ID="asdasd">
                                <ContentTemplate>
                                    <div role="form" class="form-horizontal col-md-12">
                                        <div class="form-group">
                                            <label class="col-md-4">Desde</label>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtFechaDesde" runat="server" class="form-control"></asp:TextBox>
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
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4">Sucursal Origen</label>
                                            <div class="col-md-6">
                                                <asp:DropDownList ID="DropListSucursalOrigen" runat="server" class="form-control"></asp:DropDownList>
                                                <!-- /input-group -->
                                            </div>
                                            <div class="col-md-2">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListSucursalOrigen" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4">Sucursal Destino</label>
                                            <div class="col-md-6">
                                                <asp:DropDownList ID="DropListSucursalDestino" runat="server" class="form-control" Enabled="false"></asp:DropDownList>
                                                <!-- /input-group -->
                                            </div>
                                            <div class="col-md-2">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListSucursalDestino" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <%--<div class="form-group">
                                    <label class="col-md-4">Estado</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="DropListEstados" runat="server" class="form-control"></asp:DropDownList>
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListEstados" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                    </div>
                                </div>--%>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                        <div class="modal-footer">
                            <asp:LinkButton ID="lbtnBuscar" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" OnClick="lbtnBuscar_Click" class="btn btn-success" ValidationGroup="BusquedaGroup" />
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
    </div>

    <!-- Page-Level Plugin Scripts - Tables -->
    <script src="//cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>
    <script src="../../Scripts/plugins/dataTables/custom.tables.js"></script>
    <link href="//cdn.datatables.net/1.10.2/css/jquery.dataTables.css" rel="stylesheet" />
    <link href="../../css/pages/reports.css" rel="stylesheet">

    <!-- Core Scripts - Include with every page -->
    <script src="../../Scripts/jquery-1.10.2.js"></script>
    <script src="../../Scripts/bootstrap.min.js"></script>

    <script src="../../Scripts/libs/jquery-1.9.1.min.js"></script>
    <script src="../../Scripts/libs/jquery-ui-1.10.0.custom.min.js"></script>
    <script src="../../Scripts/libs/bootstrap.min.js"></script>

    <script src="../../Scripts/plugins/hoverIntent/jquery.hoverIntent.minified.js"></script>

    <script src="../../Scripts/Application.js"></script>


    <script src="../Scripts/plugins/metisMenu/jquery.metisMenu.js"></script>

    <script src="../../Scripts/demo/gallery.js"></script>

    <script src="../../Scripts/plugins/msgGrowl/js/msgGrowl.js"></script>
    <script src="../../Scripts/plugins/lightbox/jquery.lightbox.min.js"></script>
    <script src="../../Scripts/plugins/msgbox/jquery.msgbox.min.js"></script>
    <script src="../../Scripts/demo/notifications.js"></script>

    <%--<script src="//code.jquery.com/jquery-1.9.1.js"></script>--%>
    <script src="//code.jquery.com/ui/1.10.4/jquery-ui.js"></script>
    <script src="../../js/daypilot-modal-2.0.js"></script>

    <script>


        $(function () {
            $("#<%= txtFechaDesde.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

        $(function () {
            $("#<%= txtFechaHasta.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

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
                if (key == 8)// Detectar . (punto) y backspace (retroceso) y , (coma)
                { return true; }
                else { return false; }
            }
            return true;
        }

    </script>
</asp:Content>
