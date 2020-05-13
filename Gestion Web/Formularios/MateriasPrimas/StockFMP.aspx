<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StockFMP.aspx.cs" Inherits="Gestion_Web.Formularios.MateriasPrimas.StockFMP" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">

        <div class="container">

            <div class="col-md-12 col-xs-12 hidden-print">
                <div class="widget stacked">

                    <div class="stat">
                        <h5><i class="icon-map-marker"></i>Maestro > Materias Primas > Stock</h5>
                    </div>
                    <div class="widget-header">
                        <div>
                            <i class="icon-wrench"></i>
                            <h3>Stock 
                                    <asp:Label ID="labelNombre" runat="server" Text="" Font-Bold="true"></asp:Label>
                            </h3>
                        </div>
                    </div>
                    <div class="widget big-stats-container stacked">
                        <div class="widget-content">

                            <div id="big_stats" class="cf">
                                <div class="stat">
                                    <h4>
                                        <asp:Label ID="lblSucursal" runat="server"></asp:Label>
                                    </h4>
                                    <asp:Label ID="lblStockSuc" runat="server" Text="" class="value"></asp:Label>
                                </div>
                                <!-- .stat -->

                                <div class="stat">
                                    <h4>
                                        <asp:Label ID="lblTotal" Text="TOTAL SUCURSALES" runat="server"></asp:Label>
                                    </h4>
                                    <asp:Label ID="lblStockTotalSucursales" runat="server" Text="0.00" class="value"></asp:Label>
                                </div>
                                <!-- .stat -->
                            </div>

                        </div>


                        <!-- /widget-content -->

                    </div>
                    <!-- /widget -->
                    <div class="widget-content">


                        <div class="bs-example">
                            <ul id="myTab" class="nav nav-tabs">
                                <li class="active"><a href="#home" data-toggle="tab">Stock</a></li>
                                <li class=""><a href="#profile" data-toggle="tab">Stock Agrupado</a></li>
                                <li class=""><a href="#profile2" data-toggle="tab">Movimiento Stock</a></li>

                            </ul>
                            <div class="tab-content">
                                <div class="tab-pane fade active in" id="home">
                                    <div class="col-md-12 col-xs-12">
                                        <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Always" runat="server">
                                            <ContentTemplate>
                                                <table class="table table-striped table-bordered table-hover">
                                                    <thead>

                                                        <tr>
                                                            <th>Sucursal</th>
                                                            <th>Stock</th>

                                                            <asp:PlaceHolder runat="server" ID="phImportacionesPendientes" Visible="false">
                                                                <th>Importaciones Pendientes</th>
                                                            </asp:PlaceHolder>

                                                            <asp:PlaceHolder runat="server" ID="phRemitosPendientes" Visible="false">
                                                                <th>Remitos Pendientes</th>
                                                            </asp:PlaceHolder>

                                                            <asp:PlaceHolder runat="server" ID="phStockReal" Visible="false">
                                                                <th>Stock Real</th>
                                                            </asp:PlaceHolder>

                                                            <th></th>
                                                        </tr>

                                                    </thead>
                                                    <tbody>
                                                        <asp:PlaceHolder ID="phStock" runat="server"></asp:PlaceHolder>
                                                    </tbody>
                                                </table>



                                            </ContentTemplate>
                                            <Triggers>
                                            </Triggers>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>

                                <div class="tab-pane fade" id="profile">
                                    <div class="col-md-12 col-xs-12">
                                        <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Always" runat="server">
                                            <ContentTemplate>
                                                <table class="table table-striped table-bordered">
                                                    <thead>
                                                        <tr>
                                                            <th>Sucursal</th>
                                                            <th>Stock</th>

                                                        </tr>

                                                    </thead>
                                                    <tbody>
                                                        <asp:PlaceHolder ID="phStockAgrupado" runat="server"></asp:PlaceHolder>
                                                    </tbody>
                                                </table>

                                            </ContentTemplate>
                                            <Triggers>
                                            </Triggers>
                                        </asp:UpdatePanel>


                                    </div>
                                </div>

                                <div class="tab-pane fade" id="profile2">

                                    <div class="col-md-12 col-xs-12">
                                        <div class="widget big-stats-container stacked">
                                            <div class="widget-content">

                                                <div id="big_stats" class="cf">
                                                    <div class="stat">
                                                        <h4>En su sucursal:</h4>
                                                        <asp:Label ID="labelSaldo" runat="server" Text="" class="value"></asp:Label>
                                                    </div>
                                                    <!-- .stat -->
                                                </div>

                                            </div>
                                            <!-- /widget-content -->
                                        </div>
                                        <!-- /widget -->
                                        <div class=" widget-content" style="width: 100%">
                                            <%--<div class="btn-group">
                                                <button type="button" class="btn btn-success dropdown-toggle" data-toggle="dropdown" id="btnAccion" runat="server" visible="false">Accion    <span class="caret"></span></button>
                                                <ul class="dropdown-menu">
                                                    <li>
                                                        <asp:LinkButton ID="btnExportarExcel" runat="server" OnClick="btnExportarExcel_Click">Exportar a Excel</asp:LinkButton>

                                                    </li>
                                                    <li>
                                                        <asp:LinkButton ID="LinkButton1" Visible="True" runat="server" OnClick="LinkButton1_Click">Corregir stock</asp:LinkButton>

                                                    </li>
                                                </ul>
                                            </div>--%>
                                            <!-- /btn-group -->
                                            <asp:Label ID="lblParametros" runat="server" Style="color: #CCCCCC;"></asp:Label>
                                            <div class="btn-group pull-right" style="height: 100%">
                                                <a class="btn btn-primary ui-tooltip" data-toggle="modal" title data-original-title="Filtrar Materias primas" href="#modalBusqueda" style="width: 100%">
                                                    <i class="shortcut-icon icon-filter"></i>
                                                </a>
                                            </div>
                                        </div>
                                        <asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Always" runat="server">
                                            <ContentTemplate>
                                                <table class="table table-striped table-bordered">
                                                    <thead>
                                                        <tr>
                                                            <th>Fecha</th>
                                                            <th>Descripcion</th>
                                                            <th>Cantidad</th>
                                                            <th>Cliente</th>
                                                        </tr>

                                                    </thead>
                                                    <tbody>
                                                        <asp:PlaceHolder ID="phMovimientoStock" runat="server"></asp:PlaceHolder>
                                                    </tbody>
                                                </table>

                                            </ContentTemplate>
                                            <Triggers>
                                            </Triggers>
                                        </asp:UpdatePanel>


                                    </div>

                                </div>

                                <!-- /.content -->

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
                    <h4 class="modal-title">Filtrar Articulos</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <asp:UpdatePanel ID="UpdatePanel4" UpdateMode="Always" runat="server">
                            <ContentTemplate>
                                <div class="form-group">
                                    <label class="col-md-4">Fecha Desde</label>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtFechaDesdeMov" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Fecha Hasta</label>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtFechaHastaMov" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Sucursal</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="lstSucursal" runat="server" disabled class="form-control"></asp:DropDownList>
                                    </div>

                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="lstSucursal" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>

                    </div>


                    <div class="modal-footer">
                        <asp:LinkButton ID="btnFiltrar" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnFiltrar_Click" />
                    </div>
                </div>

            </div>
        </div>
    </div>

    <div id="modalBusquedapf" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Filtrar</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <asp:UpdatePanel ID="UpdatePanel5" UpdateMode="Always" runat="server">
                            <ContentTemplate>
                                <div class="form-group">
                                    <label class="col-md-4">Fecha Desde</label>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtDesdePF" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Fecha Hasta</label>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtHastaPF" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-4">Sucursal</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="ListSucursalPF" runat="server" disabled class="form-control"></asp:DropDownList>
                                    </div>

                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="ListSucursalPF" InitialValue="-1" ValidationGroup="BusquedaPF" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>

                    </div>


                    <div class="modal-footer">
                        <asp:LinkButton ID="btnFiltrarPF" ValidationGroup="BusquedaPF" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnFiltrarPF_Click" />
                    </div>
                </div>

            </div>
        </div>
    </div>

    <link href="../../css/pages/reports.css" rel="stylesheet">
    <script src="../../Scripts/jquery-1.10.2.js"></script>
    <script src="../../Scripts/bootstrap.min.js"></script>
    <%--<script src="../Scripts/plugins/metisMenu/jquery.metisMenu.js"></script>--%>

    <script src="../../Scripts/libs/jquery-1.9.1.min.js"></script>
    <script src="../../Scripts/libs/jquery-ui-1.10.0.custom.min.js"></script>
    <script src="../../Scripts/libs/bootstrap.min.js"></script>
    <script src="../../Scripts/plugins/hoverIntent/jquery.hoverIntent.minified.js"></script>
    <script src="../../Scripts/plugins/lightbox/jquery.lightbox.min.js"></script>

    <script src="../../Scripts/Application.js"></script>

    <script src="../../Scripts/plugins/msgGrowl/js/msgGrowl.js"></script>
    <script src="../../Scripts/plugins/lightbox/jquery.lightbox.min.js"></script>
    <script src="../../Scripts/plugins/msgbox/jquery.msgbox.min.js"></script>
    <script src="../../Scripts/demo/notifications.js"></script>

    <script src="../../js/daypilot-modal-2.0.js"></script>

    <script>
        function create(idBoton) {
            //var d = document.getElementById("TheBody_txtDescripcion").value;
            //              var resource = d.options[d.selectedIndex].value;

            var modal = new DayPilot.Modal();
            modal.closed = function () {
                if (this.result == "OK") {
                    __doPostBack("UpdateButton", "");
                    location.reload();
                }
            };
            //modal.showUrl("ModalCreate.aspx?start=" + start + "&resource=" + resource);
            modal.showUrl("StockPopUpMP.aspx?idStock=" + idBoton);
        }

        function edit(id) {
            var modal = new DayPilot.Modal();
            modal.closed = function () {
                if (this.result == "OK") {
                    __doPostBack("UpdateButton", "");
                    location.reload();
                }
            };
            modal.showUrl("ModalEdit.aspx?id=" + id);
        }
    </script>

    <script>


        $(function () {
            $("#<%= txtFechaDesdeMov.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

        $(function () {
            $("#<%= txtFechaHastaMov.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

         $(function () {
            $("#<%= txtDesdePF.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

        $(function () {
            $("#<%= txtHastaPF.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

    </script>

</asp:Content>
