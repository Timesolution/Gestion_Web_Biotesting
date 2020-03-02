<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminSMS.aspx.cs" Inherits="Gestion_Web.Formularios.Herramientas.AdminSMS" %>

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
                                <h4>Total Operaciones</h4>
                                <asp:Label ID="labelTotal" runat="server" Text="" class="value"></asp:Label>
                            </div>
                            <!-- .stat -->
                            <div class="stat">
                                <h4>SMS Enviados</h4>
                                <asp:Label ID="labelSaldo" runat="server" Text="" class="value"></asp:Label>
                            </div>
                            <!-- .stat -->
                            <div class="stat">
                                <h4>Tel. Validados</h4>
                                <asp:Label ID="labelValidos" runat="server" Text="" class="value"></asp:Label>
                            </div>

                            <div class="stat">
                                <h4>Tel. A Validar</h4>
                                <asp:Label ID="labelValidar" runat="server" Text="" class="value"></asp:Label>
                            </div>
                        </div>
                        <!-- /widget-content -->

                    </div>
                    <!-- /widget -->
                </div>

                <!-- /span12 -->
                <div class="col-md-12 col-xs-12">
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
                                                        <th>DNI</th>
                                                        <th>Telefono</th>
                                                        <th>Codigo</th>
                                                        <th>Motivo</th>
                                                        <th>Documento</th>
                                                        <th>Sucursal</th>
                                                        <th>Vendedor</th>
                                                        <th></th>
                                                    </tr>

                                                </thead>
                                                <tbody>
                                                    <asp:PlaceHolder ID="phCodigos" runat="server"></asp:PlaceHolder>
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
                                            <label class="col-md-4">Empresa</label>
                                            <div class="col-md-6">
                                                <asp:DropDownList ID="ListEmpresa" runat="server" class="form-control" disabled AutoPostBack="true" OnSelectedIndexChanged="ListEmpresa_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="ListEmpresa" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                            </div>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4">Sucursal</label>
                                            <div class="col-md-6">
                                                <asp:DropDownList ID="ListSucursal" runat="server" class="form-control" disabled AutoPostBack="true" OnSelectedIndexChanged="ListSucursal_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="ListSucursal" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                            </div>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4">Vendedor</label>
                                            <div class="col-md-6">
                                                <asp:DropDownList ID="ListVendedor" runat="server" class="form-control">
                                                    <asp:ListItem Text="Todos" Value="0" />
                                                </asp:DropDownList>
                                            </div>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4">Estado</label>
                                            <div class="col-md-6">
                                                <asp:DropDownList ID="ListEstado" runat="server" class="form-control">
                                                    <asp:ListItem Text="Todos" Value="0" />
                                                    <asp:ListItem Text="A Validar" Value="1" />
                                                    <asp:ListItem Text="Validados" Value="2" />
                                                    <asp:ListItem Text="Anulados" Value="9" />
                                                </asp:DropDownList>
                                            </div>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4">Dni</label>
                                            <div class="col-md-6">
                                                <asp:TextBox ID="txtDniBuscar" runat="server" class="form-control" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4">Telefono</label>
                                            <div class="col-md-6">
                                                <asp:TextBox ID="txtTelefonoBuscar" runat="server" class="form-control" />
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

            <div id="modalValidar" class="modal fade" tabindex="-1" role="dialog">
                <div class="modal-dialog">
                    <div class="modal-content">

                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h4 class="modal-title">Validar</h4>
                        </div>
                        <div class="modal-body">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <div role="form" class="form-horizontal col-md-12">
                                        <div class="form-group">
                                            <label class="col-md-2">Telefono</label>
                                            <div class="col-md-3">
                                                <div class="input-group">
                                                    <span class="input-group-addon">0</span>
                                                    <asp:TextBox ID="txtCodAreaValidar" runat="server" class="form-control" placeholder="Ej:3735" MaxLength="4" onkeypress="javascript:return validarNroSinComa(event)" />
                                                </div>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtTelefonoValidar" runat="server" class="form-control" MaxLength="8" />
                                            </div>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="btnEnviarCodigoValidar" runat="server" class="btn btn-info ui-tooltip" data-toggle="tooltip" title data-original-title="Enviar codigo" Text="<i class='fa fa-phone' aria-hidden='true'></i>" OnClick="btnEnviarCodigoValidar_Click" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-2">Codigo</label>
                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtCodigoValidar" runat="server" class="form-control" disabled MaxLength="4" />
                                            </div>
                                        </div>

                                        <div class="col-md-3">
                                            <asp:TextBox runat="server" ID="txtMovimientoValidar" Text="0" Style="display: none"></asp:TextBox>
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div class="modal-footer">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                <ContentTemplate>
                                    <asp:LinkButton ID="lbtnValidar" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnValidar_Click" ValidationGroup="ValidarGroup" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>

                </div>
            </div>

            <div id="modalConfirmacion" class="modal fade" tabindex="-1" role="dialog">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h4 class="modal-title">Confirmacion de Eliminacion</h4>
                        </div>
                        <div class="modal-body">
                            <div role="form" class="form-horizontal col-md-12">
                                <div class="form-group">
                                    <div class="col-md-1">
                                        <h1>
                                            <i class="icon-warning-sign" style="color: orange"></i>
                                        </h1>
                                    </div>
                                    <div class="col-md-10">
                                        <h5>
                                            <asp:Label runat="server" ID="lblMensaje" Text="Esta seguro que desea eliminar el telefono de todos los DNI?" Style="text-align: center"></asp:Label>
                                        </h5>
                                    </div>

                                    <div class="col-md-1">
                                        <asp:TextBox runat="server" ID="txtMovimiento" Text="0" Style="display: none"></asp:TextBox>
                                    </div>
                                </div>

                            </div>


                            <div class="modal-footer">
                                <asp:Button runat="server" ID="btnSi" Text="Eliminar" class="btn btn-danger" OnClick="btnSi_Click" />
                                <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
            <%--Fin modalGrupo--%>

            <div id="modalConfirmacion2" class="modal fade" tabindex="-1" role="dialog">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h4 class="modal-title">Confirmacion de validacion</h4>
                        </div>
                        <div class="modal-body">
                            <div role="form" class="form-horizontal col-md-12">
                                <div class="form-group">
                                    <div class="col-md-2">
                                        <h1>
                                            <i class="icon-warning-sign" style="color: orange"></i>
                                        </h1>
                                    </div>
                                    <div class="col-md-9">
                                        <h5>
                                            <asp:Label runat="server" ID="Label1" Text="Esta seguro que desea omitir esta validacion?" Style="text-align: center"></asp:Label>
                                        </h5>
                                    </div>

                                    <div class="col-md-1">
                                        <asp:TextBox runat="server" ID="txtMovimientoOmitir" Text="0" Style="display: none"></asp:TextBox>
                                    </div>
                                </div>

                            </div>


                            <div class="modal-footer">
                                <asp:Button runat="server" ID="btnSiOmitir" Text="Omitir" class="btn btn-success" OnClick="btnSiOmitir_Click" />
                                <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
            <%--Fin modalGrupo--%>

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

                $('#dataTables-example').dataTable({
                    "bLengthChange": false,
                    "pageLength": 15,
                    "bFilter": true,
                    "bInfo": false,
                    "bAutoWidth": false,
                    "language": {
                        "search": "Buscar:"
                    }
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
                function abrirdialog(valor) {
                    document.getElementById('<%= txtMovimiento.ClientID %>').value = valor;
                }
            </script>

            <script>
                function abrirdialog2(valor) {
                    document.getElementById('<%= txtMovimientoValidar.ClientID %>').value = valor;
                }
            </script>

            <script>
                function abrirdialog3(valor) {
                    document.getElementById('<%= txtMovimientoOmitir.ClientID %>').value = valor;
                }
            </script>

            <script type="text/javascript">
                function desbloquearCodigo() {
                    document.getElementById("<%= this.txtCodigoValidar.ClientID %>").removeAttribute("disabled");
                }
                function bloquear() {
                    document.getElementById("<%= this.txtCodigoValidar.ClientID %>").setAttribute("disabled", "disabled");
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
            </script>

        </div>
</asp:Content>
