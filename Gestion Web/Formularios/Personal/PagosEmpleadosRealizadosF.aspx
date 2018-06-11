<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PagosEmpleadosRealizadosF.aspx.cs" Inherits="Gestion_Web.Formularios.Personal.PagosEmpleadosRealizadosF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">

        <div>

            <div class="col-md-12 col-xs-12">
                <div class="widget stacked">
                    <div class="stat">
                        <h5><i class="icon-map-marker"></i> Personal > Pagos</h5>
                    </div>
                    <div class="widget-header">
                        <i class="icon-wrench"></i>
                        <h3>Herramientas</h3>
                    </div>
                    <!-- /widget-header -->

                    <div class="widget-content">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                            <ContentTemplate>

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

                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
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
                                <h4>Saldo</h4>
                                <asp:Label ID="labelSaldo" runat="server" Text="" class="value"></asp:Label>
                            </div>
                            <!-- .stat -->
                        </div>

                    </div>
                    <!-- /widget-content -->

                </div>
                <!-- /widget -->

            </div>
            <div class="col-md-12 col-xs-12">
                <div class="widget widget-table">

                    <div class="widget-header">
                        <i class="icon-th-list" style="width: 2%"></i>
                        <h3 style="width: 75%">Compras Impagas
                        </h3>
                        <h3>
                            <asp:Label ID="lblSaldo" runat="server" Style="text-align: end" Text="" ForeColor="#0099ff" Font-Bold="true"></asp:Label>
                        </h3>
                    </div>
                    <div class="widget-content">
                        <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Always" runat="server">
                            <ContentTemplate>
                                <a class="btn btn-info" style="display: none" data-toggle="modal" id="abreDialog" href="#modalFacturaDetalle">Agregar Tipo Cliente</a>

                                <table class="table table-bordered table-striped" id="dataTables-example">
                                    <thead>
                                        <tr>
                                            <th style="text-align: left">Fecha</th>
                                            <th style="text-align: left">Numero</th>
                                            <th style="text-align: right">Importe</th>
                                            <th></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:PlaceHolder ID="phCobranzas" runat="server"></asp:PlaceHolder>
                                        <th></th>
                                        <th></th>
                                        <th></th>
                                        <th></th>
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
        <%--Fin modalGrupo--%>
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
                            <div class="col-md-2">
                                <h1>
                                    <i class="icon-warning-sign" style="color: orange"></i>
                                </h1>
                            </div>
                            <div class="col-md-7">
                                <h5>
                                    <asp:Label runat="server" ID="lblMensaje" Text="Esta seguro que desea eliminar el Pago?" Style="text-align: center"></asp:Label>
                                </h5>
                            </div>

                            <div class="col-md-3">
                                <asp:TextBox runat="server" ID="txtMovimiento" Text="0" Style="display: none"></asp:TextBox>
                            </div>
                        </div>

                    </div>


                    <div class="modal-footer">
                        <asp:Button runat="server" ID="btnEliminar" Text="Eliminar" class="btn btn-danger" OnClick="btnEliminar_Click" />
                        <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>


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
                    <div role="form" class="form-horizontal col-md-12">
                        <asp:UpdatePanel ID="UpdatePanel4" UpdateMode="Always" runat="server">
                            <ContentTemplate>
                                <div class="form-group">
                                    <label class="col-md-4">Empleado</label>
                                    <div class="col-md-6">

                                        <asp:DropDownList ID="ListEmpleados" runat="server" class="form-control" AutoPostBack="true" ></asp:DropDownList>

                                        <!-- /input-group -->
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="ListEmpleados" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Empresa</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="DropListEmpresa" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="DropListEmpresa_SelectedIndexChanged"></asp:DropDownList>

                                    </div>

                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListEmpresa" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                    <!-- /input-group -->
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Sucursal</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="DropListSucursal" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="DropListSucursal_SelectedIndexChanged"></asp:DropDownList>
                                        <!-- /input-group -->
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListSucursal" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                 <div class="form-group">
                                    <label class="col-md-4">Punto de Venta</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="ListPuntoVenta" runat="server" class="form-control" AutoPostBack="true"></asp:DropDownList>
                                        <!-- /input-group -->
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="ListPuntoVenta" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>

                    </div>


                    <div class="modal-footer">
                        <asp:LinkButton ID="lbBuscar" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" ValidationGroup="BusquedaGroup" OnClick="lbBuscar_Click" />
                    </div>
                </div>

            </div>
        </div>

        <script>
            function abrirdialog(valor) {
                document.getElementById('<%= txtMovimiento.ClientID %>').value = valor;
            }
        </script>
        <link href="../../css/pages/reports.css" rel="stylesheet">
        <!-- Core Scripts - Include with every page -->
        <script src="../../Scripts/jquery-1.10.2.js"></script>
        <script src="../../Scripts/bootstrap.min.js"></script>
        <%--<script src="../Scripts/plugins/metisMenu/jquery.metisMenu.js"></script>--%>

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
        <script src="../../Scripts/plugins/dataTables/jquery.dataTables.min.js"></script>
        <script src="../../Scripts/plugins/dataTables/custom.tables.js"></script>
        <link href="../../Scripts/plugins/dataTables/dataTables.css" rel="stylesheet" />


    </div>
</asp:Content>
