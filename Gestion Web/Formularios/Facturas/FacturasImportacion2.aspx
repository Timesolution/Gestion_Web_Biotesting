<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FacturasImportacion2.aspx.cs" Inherits="Gestion_Web.Formularios.Facturas.FacturasImportacion2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="col-md-12 col-xs-12">
        <div class="widget stacked">

            <div class="stat">
                <h5><i class="icon-map-marker"></i>Ventas > Importar Ventas</h5>
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
                            <%-- <div class="btn-group">
                                    <button type="button" class="btn btn-success dropdown-toggle" data-toggle="dropdown" id="btnAccion" runat="server">Accion    <span class="caret"></span></button>
                                    <ul class="dropdown-menu">
                                        
                                    </ul>
                                </div>--%>
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
                                </div>
                            </div>
                        </td>
                        <td style="width: 2%">

                            <div class="btn-group" style="width: 100%">
                                <%--<button type="button" class="btn btn-primary dropdown-toggle ui-tooltip" title data-original-title="Listados" data-toggle="dropdown">
                                        <i class="shortcut-icon icon-print"></i>&nbsp
                                        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                                        <span class="caret"></span>
                                    </button>--%>
                                <%--<ul class="dropdown-menu" role="menu">        
                                        <li>
                                            <asp:LinkButton ID="btnImprimir" Text="Impresion detalle" runat="server" />
                                        </li>                               
                                        <li>
                                            <asp:LinkButton ID="btnExportar" Text="Exportar detalle" runat="server"  />
                                        </li>                               
                                    </ul>--%>
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
                                <a class="btn btn-primary" data-toggle="modal" href="#modalSubirArchivo" style="width: 100%">
                                    <i class="shortcut-icon icon-plus"></i>
                                </a>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <!-- /widget-content -->
            <%-- <div class="widget big-stats-container stacked">
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

            </div>--%>
            <div class="widget stacked widget-table action-table">

                <div class="widget-header">
                    <i class="icon-money" style="width: 2%"></i>
                    <h3 style="width: 75%">Lote
                                    
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
                                                <th>Lote</th>
                                                <th>Fecha</th>
                                                <th>Archivo</th>
                                                <th>Comprobantes</th>
                                                <th>Correctos</th>
                                                <th>Incorrectos</th>
                                                <th>Estado</th>
                                                <th>Resultado</th>
                                                <%--<th></th>--%>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:PlaceHolder ID="phLotes" runat="server"></asp:PlaceHolder>
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
        <!-- /widget -->
    </div>

    <div class="col-md-12 col-xs-12">
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
                            <label class="col-md-4">Desde</label>
                            <div class="col-md-4">

                                <asp:TextBox ID="txtFechaDesde" runat="server" class="form-control"></asp:TextBox>

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


                       <%-- <div class="form-group">
                            <label class="col-md-3">Sucursal</label>
                            <div class="col-md-6">
                                <asp:DropDownList ID="DropListSucursal" runat="server" class="form-control"></asp:DropDownList>
                                <!-- /input-group -->
                            </div>
                            <div class="col-md-2">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListSucursal" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                            </div>
                        </div>--%>
                        <div class="form-group">
                            <label class="col-md-3">Estado</label>
                            <div class="col-md-6">
                                <asp:DropDownList ID="DropListEstado" runat="server" class="form-control">
                                    <asp:ListItem Text="Todos" Value="0" />
                                    <asp:ListItem Text="Correcto" Value="1" />
                                    <asp:ListItem Text="Incorrecto" Value="-1" />
                                </asp:DropDownList>
                                <!-- /input-group -->
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbtnBuscar" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnBuscar_Click" ValidationGroup="BusquedaGroup" />
                </div>
            </div>

        </div>
    </div>


    <div id="modalSubirArchivo" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Importar Factura</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <asp:UpdatePanel ID="UpdatePanel4" UpdateMode="Always" runat="server">
                            <ContentTemplate>
                                <div class="form-group">
                                    <label class="col-md-4">Empresa</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="DropListEmpresa" ValidationGroup="add" runat="server" class="form-control" OnSelectedIndexChanged="DropListEmpresa_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                    </div>

                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ValidationGroup="add" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListEmpresa" InitialValue="-1" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                    <!-- /input-group -->
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Sucursal</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="ListSucursal" runat="server" ValidationGroup="add" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ListSucursal_SelectedIndexChanged"></asp:DropDownList>
                                        <!-- /input-group -->
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" ValidationGroup="add" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="ListSucursal" InitialValue="-1" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Punto de Venta</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="DropListPuntoVta" runat="server" ValidationGroup="add" class="form-control"></asp:DropDownList>

                                        <!-- /input-group -->
                                    </div>

                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ValidationGroup="add" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListPuntoVta" InitialValue="-1" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Archivo:</label>
                                    <div class="col-md-8">
                                        <asp:FileUpload ID="FileUpload1" runat="server" />
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="modal-footer">
                        <asp:LinkButton ID="btnImportarXML" ValidationGroup="add" runat="server" class="btn btn-success" Text="<span class='shortcut-icon icon-ok'></span>" OnClick="btnImportarXML_Click"></asp:LinkButton>
                    </div>
                </div>

            </div>
        </div>
    </div>

    <link href="../../css/pages/reports.css" rel="stylesheet">
    <script src="../../Scripts/plugins/dataTables/jquery-2.0.0.js"></script>
    <script src="../../Scripts/plugins/dataTables/jquery.dataTables.min.js"></script>
    <script src="../../Scripts/plugins/dataTables/custom.tables.js"></script>


    <!-- Core Scripts - Include with every page -->
    <script src="../../Scripts/jquery-1.10.2.js"></script>
    <script src="../../Scripts/bootstrap.min.js"></script>

    <script src="../Scripts/plugins/metisMenu/jquery.metisMenu.js"></script>

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

    <script>

        $(function () {
            $("#<%= txtFechaDesde.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

        $(function () {
            $("#<%= txtFechaHasta.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

    </script>


</asp:Content>
