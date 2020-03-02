<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BalanceF.aspx.cs" Inherits="Gestion_Web.Formularios.Reportes.BalanceF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">

        <%--<div class="container">--%>
        <div>
            <div class="col-md-12">

            <%--<div class="row">
                <div class="col-md-12 col-xs-12">
                    <div class="widget stacked">

                        <div class="widget-header">
                            <i class="icon-wrench"></i>
                            <h3>Herramientas</h3>
                        </div>
                        <div class="widget-content">
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
            </div>--%>

            <div class="row">

                <div class="col-md-12">

                    <div class="widget big-stats-container stacked">

                        <div class="widget-content">

                            <div id="big_stats" class="cf">
                                <div class="stat">
                                    <h4>Activo</h4>
                                    <asp:Label ID="LabelActivo" runat="server" Text="" class="value" ForeColor="#003300"></asp:Label>
                                </div>
                                <!-- .stat -->

                                <div class="stat">
                                    <h4>Pasivo</h4>
                                    <asp:Label ID="LabelPasivo" runat="server" Text="" class="value" ForeColor="#990000"></asp:Label>
                                </div>
                                <!-- .stat -->

                                <%--<div class="stat">
                                    <h4>Clientes Atrasados</h4>
                                    <asp:Label ID="lblClientesAtrasados" runat="server" Text="" class="value"></asp:Label>
                                </div>--%>
                                <!-- .stat -->


                                <!-- .stat -->
                            </div>

                        </div>
                        <!-- /widget-content -->

                    </div>
                    <!-- /widget -->

                </div>
                <!-- /span12 -->

            </div>
            <!-- /row -->


            <div class="row">

                <div class="col-md-6">

                    <div class="widget stacked">

                        <div class="widget-header">
                            <i class="icon-star"></i>
                            <h3>Activos</h3>

                        </div>
                        <!-- /widget-header -->

                        <div class="widget-content">

                            <table class="table table-bordered table-striped">

                                <thead>
                                    <tr>
                                        <th style="text-align: left; width: 60%">Descripcion</th>
                                        <th style="text-align: left; width: 35%">Total</th>
                                        <th style="text-align: right; width: 5%"></th>
                                    </tr>

                                </thead>

                                <tbody>
                                    <asp:PlaceHolder ID="phTopClientes" runat="server"></asp:PlaceHolder>
                                </tbody>

                            </table>

                            <%--<div class="cirque-stats">
                                <asp:Literal ID="LitChar1" runat="server"></asp:Literal>
                            </div>--%>
                        </div>
                        <!-- /widget-content -->

                    </div>
                    <!-- /widget -->




                </div>
                <!-- /span6 -->


                <div class="col-md-6">

                    <div class="widget stacked">

                        <div class="widget-header">
                            <i class="icon-list-alt"></i>
                            <h3>Pasivo</h3>
                        </div>
                        <!-- /widget-header -->

                        <div class="widget-content">

                            <table class="table table-bordered table-striped">

                                <thead>
                                    <tr>
                                        <th style="text-align: left; width: 60%">Descripcion</th>
                                        <th style="text-align: left; width: 35%">Total</th>
                                        <th style="text-align: right; width: 5%"></th>
                                    </tr>

                                </thead>

                                <tbody>
                                    <asp:PlaceHolder ID="phPasivo" runat="server"></asp:PlaceHolder>
                                </tbody>

                            </table>

                        </div>
                        <!-- /widget-content -->

                    </div>
                    <!-- /widget -->

                </div>
                <!-- /span6 -->

            </div>

            <!-- /row -->
            <div class="row">
                <div class="col-md-12 col-xs-12">
                    <div class="widget stacked">

                        <div class="widget-header">
                            <i class="icon-wrench"></i>
                            <h3>Herramientas</h3>
                        </div>
                        <div class="widget-content">
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
            </div>

            <div class="row">
                <div class="col-md-12">
                    <div class="widget big-stats-container stacked">

                        <div class="widget-content">

                            <div id="big_stats" class="cf">
                                <div class="stat">
                                    <h4>Iva Ventas</h4>
                                    <asp:Label ID="lblIvaVentas" runat="server" Text="" class="value" ForeColor="#003300"></asp:Label>
                                </div>
                                <!-- .stat -->

                                <div class="stat">
                                    <h4>Iva Compras</h4>
                                    <asp:Label ID="lblIvaCompras" runat="server" Text="" class="value" ForeColor="#990000"></asp:Label>
                                </div>
                                <!-- .stat -->

                                <%--<div class="stat">
                                    <h4>Total Iva a pagar</h4>
                                    <asp:Label ID="lblIvaTotalPagar" runat="server" Text="" class="value"></asp:Label>
                                </div>--%>
                                <!-- .stat -->
                            </div>

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
                                    <h4>Total Iva a pagar</h4>
                                    <asp:Label ID="lblIvaTotalPagar" runat="server" Text="" class="value"></asp:Label>
                                </div>
                                <!-- .stat -->
                            </div>

                        </div>
                        <!-- /widget-content -->

                    </div>
                    <!-- /widget -->
                </div>

                <div class="col-md-6 col-xs-12">
                    <div class="widget stacked">

                        <div class="widget-header">
                            <i class="icon-list"></i>
                            <h3>Ventas</h3>
                        </div>
                        <div class="widget-content">
                            <table class="table table-bordered table-striped">

                                <thead>
                                    <tr>
                                        <th style="text-align: left; width: 60%">Descripcion</th>
                                        <th style="text-align: left; width: 35%">Total</th>
                                        <th style="text-align: right; width: 5%"></th>
                                    </tr>

                                </thead>

                                <tbody>
                                    <asp:PlaceHolder ID="phVentas" runat="server"></asp:PlaceHolder>
                                </tbody>

                            </table>

                        </div>
                        <!-- /widget-content -->

                    </div>
                    <!-- /widget -->
                </div>

                <div class="col-md-6 col-xs-12">
                    <div class="widget stacked">

                        <div class="widget-header">
                            <i class="icon-list"></i>
                            <h3>Compras</h3>
                        </div>
                        <div class="widget-content">
                            <table class="table table-bordered table-striped">

                                <thead>
                                    <tr>
                                        <th style="text-align: left; width: 60%">Descripcion</th>
                                        <th style="text-align: left; width: 35%">Total</th>
                                        <th style="text-align: right; width: 5%"></th>
                                    </tr>

                                </thead>

                                <tbody>
                                    <asp:PlaceHolder ID="phCompras" runat="server"></asp:PlaceHolder>
                                </tbody>

                            </table>
                        </div>
                        <!-- /widget-content -->

                    </div>
                    <!-- /widget -->
                </div>



                <div class="col-md-6 col-xs-12" hidden="hidden">

                    <div class="widget stacked">
                        <div class="widget-header">
                            <i class="icon-star"></i>
                            <h3>Iva</h3>
                        </div>

                        <div class="widget-content">

                            <h4>Total Iva Ventas
                                <asp:Literal ID="LitIvaVentas" runat="server" ClientIDMode="Static"></asp:Literal>
                            </h4>

                            <h4>Total Iva Compras
                                <asp:Literal ID="LitIvaCompras" runat="server" ClientIDMode="Static"></asp:Literal>
                            </h4>

                            <h4>Total Iva a Pagar
                                <asp:Literal ID="LitIvaPagar" runat="server"></asp:Literal>
                            </h4>


                            <%--<div class="stats">
                                <div class="stat" id="val1">
                                    <h4>Iva Ventas</h4>
                                    <span class="value">
                                       <asp:Literal ID="LitIvaVentas" runat="server" ClientIDMode="Static"></asp:Literal>
                                    10     
                                    </span>
                                        Iva Ventas

                                   
                                </div>
                                <!-- .stat -->

                                <div class="stat" id="val2">
                                    <h4>Iva Compras</h4>
                                    <span class="value">
                                        
                                        5
                                    </span>
                                    Iva Compras
                                </div>
                                <!-- .stat -->
                            </div>--%>
                            <%-- <div id="chart-stats" class="stats">

                                <div class="stat stat-chart">
                                    <div id="donut-chart" class="chart-holder"></div>
                                    <!-- #donut -->
                                </div>
                                <!-- /substat -->

                                <div class="stat stat-time">
                                    <span class="stat-value">
                                        <asp:Literal ID="LitTotal" runat="server"></asp:Literal></span>
                                    Total IPPs cargados
                                </div>
                                <!-- /substat -->

                            </div>--%>
                            <!-- /substats -->
                            <!-- .stat -->


                        </div>
                        <!-- /widget-content -->

                    </div>

                </div>

            </div>

        
            </div><!-- / col-md-12 -->
        </div><!-- /ex-container -->


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
                                    <div class="form-group">
                                        <label class="col-md-4">Sucursal</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListSucursal" runat="server" class="form-control"></asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>

                                    </div>

                                </ContentTemplate>
                                <Triggers>
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>


                    <div class="modal-footer">
                        <asp:LinkButton ID="lbtnBuscar" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" ValidationGroup="BusquedaGroup" OnClick="lbtnBuscar_Click" />
                    </div>
                </div>

            </div>
        </div>
        <!-- /container -->

    </div>
    <!-- /main -->

    <link href="../../css/pages/reports.css" rel="stylesheet">

    <script src="../../Scripts/libs/jquery-1.9.1.min.js"></script>
    <script src="../../Scripts/libs/jquery-ui-1.10.0.custom.min.js"></script>

    <script src="../../Scripts/plugins/metisMenu/jquery.metisMenu.js"></script>

    <script src="../../Scripts/Application.js"></script>
    <%-- <script src="../../Scripts/libs/bootstrap.min.js"></script>--%>

    <%--<script src="../../Scripts/charts/pie.js"></script>
    <script src="../../Scripts/charts/bar.js"></script>--%>

    <%--  <script src="//code.jquery.com/ui/1.10.4/jquery-ui.js"></script>--%>

    <script src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script src="//code.jquery.com/ui/1.11.4/jquery-ui.js"></script>


    <script>
        $(function () {
            $("#<%= txtFechaDesde.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

        $(function () {
            $("#<%= txtFechaHasta.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

    </script>
    <script>
        //$(function () {

        //    var data = [];
        //    var series = 3;
        //    //for( var i = 0; i<series; i++)
        //    //{
        //    var data1 = parseInt(document.getElementById('val1').innerText);
        //    var data2 = parseInt(document.getElementById('val2').innerText);



        //    data[0] = { label: "Iva Venta", data: data1 }
        //    data[1] = { label: "Iva Compra", data: data2 }

        //    //}

        //    $.plot($("#donut-chart"), data,
        //    {
        //        colors: ["#F90", "#222", "#777", "#AAA"],
        //        series: {
        //            pie: {
        //                innerRadius: 0.5,
        //                show: true
        //            }
        //        }
        //    });

        //});
    </script>


</asp:Content>
