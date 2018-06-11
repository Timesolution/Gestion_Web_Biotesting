<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CashFlowF.aspx.cs" Inherits="Gestion_Web.Formularios.Valores.CashFlowF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">
        <div>
            <div class="col-md-12 col-xs-12">
                <div class="widget stacked">
                    <div class="stat">
                        <h5><i class="icon-map-marker"></i>Valores > Cashflow</h5>
                    </div>
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
                                    <div class="btn-toolbar">
                                        <div class="btn-group">
                                            <asp:Button ID="btnCalcularMontos" type="button" runat="server" Text="Calcular Montos" class="btn btn-success" OnClick="btnCalcularMontos_Click" />
                                        </div>
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
                <div class="widget stacked">

                    <div class="widget-header">
                        <i class="icon-list"></i>
                        <h3>Ventas</h3>
                    </div>
                    <div class="widget-content">
                        <section id="accordions">
                            <div class="panel-group accordion" id="acordions">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 95%">
                                                    <h4 class="panel-title">Ingresos
                                                        <%--<a class="accordion-toggle" data-toggle="collapse" data-parent=".accordion" href="#collapseOne">Ingresos
                                                        </a>--%>
                                                    </h4>
                                                </td>
                                                <%--<td style="width: 5%">
                                                    <asp:LinkButton ID="lbtnIngresos" runat="server" Text="<span class='shortcut-icon icon-plus'></span>" class="accordion-toggle" data-toggle="collapse" data-parent=".accordion" href="#collapseOne" Style="color: black" />
                                                </td>--%>
                                            </tr>
                                        </table>
                                    </div>
                                    <%--<div id="collapseOne" class="panel-collapse collapse">--%>
                                    <div class="panel-body">
                                        <div class="col-md-12 col-xs-12">
                                            <div class="widget widget-table">
                                                <div class="widget-header">
                                                    <span class="icon-external-link"></span>
                                                    <h3>Ingresos</h3>
                                                </div>
                                                <div class="widget-content">
                                                    <div class="col-md-8">
                                                    </div>
                                                    <table class="table table-bordered table-striped" id="dataTables-example" style="font-size: 85%;">
                                                        <thead>
                                                            <tr>
                                                                <th>Concepto</th>
                                                                <th>Periodo 1</th>
                                                                <th>Periodo 2</th>
                                                                <th>Periodo 3</th>
                                                                <th>Periodo 4</th>
                                                                <th>Periodo 5</th>
                                                                <th>Periodo 6</th>
                                                                <th>Periodo 7</th>
                                                                <th>Periodo 8</th>
                                                                <th>Periodo 9</th>
                                                                <th>Periodo 10</th>
                                                                <th>Periodo 11</th>
                                                                <th>Periodo 12</th>
                                                            </tr>
                                                            <tr>
                                                                <th>Caja</th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosCaja_1" CssClass="form-control" Text="6000.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosCaja_2" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosCaja_3" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosCaja_4" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosCaja_5" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosCaja_6" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosCaja_7" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosCaja_8" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosCaja_9" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosCaja_10" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosCaja_11" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosCaja_12" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                            </tr>
                                                            <tr>
                                                                <th>Cheques</th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosCheques_1" CssClass="form-control" Text="18543.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosCheques_2" CssClass="form-control" Text="15100.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosCheques_3" CssClass="form-control" Text="13755.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosCheques_4" CssClass="form-control" Text="12230.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosCheques_5" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosCheques_6" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosCheques_7" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosCheques_8" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosCheques_9" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosCheques_10" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosCheques_11" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosCheques_12" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                            </tr>
                                                            <tr>
                                                                <th>Tarjetas</th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosTarjetas_1" CssClass="form-control" Text="18090.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosTarjetas_2" CssClass="form-control" Text="12754.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosTarjetas_3" CssClass="form-control" Text="10004.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosTarjetas_4" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosTarjetas_5" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosTarjetas_6" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosTarjetas_7" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosTarjetas_8" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosTarjetas_9" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosTarjetas_10" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosTarjetas_11" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosTarjetas_12" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                            </tr>
                                                            <tr>
                                                                <th>Cuentas a Cobrar</th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosCuentasCobrar_1" CssClass="form-control" Text="21202.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosCuentasCobrar_2" CssClass="form-control" Text="11100.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosCuentasCobrar_3" CssClass="form-control" Text="5450.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosCuentasCobrar_4" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosCuentasCobrar_5" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosCuentasCobrar_6" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosCuentasCobrar_7" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosCuentasCobrar_8" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosCuentasCobrar_9" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosCuentasCobrar_10" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosCuentasCobrar_11" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosCuentasCobrar_12" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                            </tr>
                                                            <tr>
                                                                <th>Bancos</th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosBancos_1" CssClass="form-control" Text="7050.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosBancos_2" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosBancos_3" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosBancos_4" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosBancos_5" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosBancos_6" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosBancos_7" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosBancos_8" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosBancos_9" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosBancos_10" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosBancos_11" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosBancos_12" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                            </tr>
                                                            <tr>
                                                                <th>Otros</th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosOtros_1" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosOtros_2" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosOtros_3" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosOtros_4" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosOtros_5" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosOtros_6" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosOtros_7" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosOtros_8" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosOtros_9" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosOtros_10" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosOtros_11" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosOtros_12" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                            </tr>
                                                            <tr style="text-align:right;">
                                                                <th>Ventas</th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosVentas_1" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosVentas_2" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosVentas_3" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosVentas_4" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosVentas_5" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosVentas_6" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosVentas_7" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosVentas_8" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosVentas_9" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosVentas_10" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosVentas_11" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th style="text-align:right;">
                                                                    <asp:TextBox runat="server" ID="txtIngresosVentas_12" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                            </tr>
                                                            <tr>
                                                                <th style="text-align:right;font-size:135%;"><asp:Label runat="server" Text="Total Ingresos"></asp:Label></th>
                                                                <th style="text-align:right;font-size:135%;"><asp:Label runat="server" ID="lblTotalIngresos_1"></asp:Label></th>
                                                                <th style="text-align:right;font-size:135%;"><asp:Label runat="server" ID="lblTotalIngresos_2"></asp:Label></th>
                                                                <th style="text-align:right;font-size:135%;"><asp:Label runat="server" ID="lblTotalIngresos_3"></asp:Label></th>
                                                                <th style="text-align:right;font-size:135%;"><asp:Label runat="server" ID="lblTotalIngresos_4"></asp:Label></th>
                                                                <th style="text-align:right;font-size:135%;"><asp:Label runat="server" ID="lblTotalIngresos_5"></asp:Label></th>
                                                                <th style="text-align:right;font-size:135%;"><asp:Label runat="server" ID="lblTotalIngresos_6"></asp:Label></th>
                                                                <th style="text-align:right;font-size:135%;"><asp:Label runat="server" ID="lblTotalIngresos_7"></asp:Label></th>
                                                                <th style="text-align:right;font-size:135%;"><asp:Label runat="server" ID="lblTotalIngresos_8"></asp:Label></th>
                                                                <th style="text-align:right;font-size:135%;"><asp:Label runat="server" ID="lblTotalIngresos_9"></asp:Label></th>
                                                                <th style="text-align:right;font-size:135%;"><asp:Label runat="server" ID="lblTotalIngresos_10"></asp:Label></th>
                                                                <th style="text-align:right;font-size:135%;"><asp:Label runat="server" ID="lblTotalIngresos_11"></asp:Label></th>
                                                                <th style="text-align:right;font-size:135%;"><asp:Label runat="server" ID="lblTotalIngresos_12"></asp:Label></th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <asp:PlaceHolder ID="phIngresos" runat="server"></asp:PlaceHolder>
                                                        </tbody>
                                                    </table>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <%--</div>--%>
                                </div>

                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 95%">
                                                    <h4 class="panel-title">Egresos
                                                        <%--<a class="accordion-toggle" data-toggle="collapse" data-parent=".accordion" href="#collapseTwo">Egresos
                                                        </a>--%>
                                                    </h4>
                                                </td>
                                                <%--<td style="width: 5%">
                                                    <asp:LinkButton ID="lbtnEgresos" runat="server" Text="<span class='shortcut-icon icon-plus'></span>" class="accordion-toggle" data-toggle="collapse" data-parent=".accordion" href="#collapseTwo" Style="color: black" />
                                                </td>--%>
                                            </tr>
                                        </table>
                                    </div>
                                    <%--<div id="collapseTwo" class="panel-collapse collapse">--%>
                                    <div class="panel-body">
                                        <div class="col-md-12 col-xs-12">
                                            <div class="widget widget-table">
                                                <div class="widget-header">
                                                    <span class="icon-external-link"></span>
                                                    <h3>Egresos</h3>
                                                </div>
                                                <div class="widget-content">
                                                    <div class="col-md-8">
                                                    </div>
                                                    <table class="table table-bordered table-striped" id="dataTables-example">
                                                        <thead>
                                                            <tr>
                                                                <th>Concepto</th>
                                                                <th>Periodo 1</th>
                                                                <th>Periodo 2</th>
                                                                <th>Periodo 3</th>
                                                                <th>Periodo 4</th>
                                                                <th>Periodo 5</th>
                                                                <th>Periodo 6</th>
                                                                <th>Periodo 7</th>
                                                                <th>Periodo 8</th>
                                                                <th>Periodo 9</th>
                                                                <th>Periodo 10</th>
                                                                <th>Periodo 11</th>
                                                                <th>Periodo 12</th>
                                                            </tr>
                                                            <tr>
                                                                <th>Gastos Efectivo</th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosEfectivo_1" CssClass="form-control" Text="4100.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosEfectivo_2" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosEfectivo_3" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosEfectivo_4" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosEfectivo_5" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosEfectivo_6" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosEfectivo_7" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosEfectivo_8" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosEfectivo_9" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosEfectivo_10" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosEfectivo_11" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosEfectivo_12" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                            </tr>
                                                            <tr>
                                                                <th>Cheques</th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosCheques_1" CssClass="form-control" Text="2350.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosCheques_2" CssClass="form-control" Text="7520.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosCheques_3" CssClass="form-control" Text="3411.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosCheques_4" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosCheques_5" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosCheques_6" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosCheques_7" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosCheques_8" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosCheques_9" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosCheques_10" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosCheques_11" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosCheques_12" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                            </tr>
                                                            <tr>
                                                                <th>Cuentas a Pagar</th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosCuentasPagar_1" CssClass="form-control" Text="9560.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosCuentasPagar_2" CssClass="form-control" Text="8500.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosCuentasPagar_3" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosCuentasPagar_4" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosCuentasPagar_5" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosCuentasPagar_6" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosCuentasPagar_7" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosCuentasPagar_8" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosCuentasPagar_9" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosCuentasPagar_10" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosCuentasPagar_11" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosCuentasPagar_12" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                            </tr>
                                                            <tr>
                                                                <th>Sueldos</th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosSueldos_1" CssClass="form-control" Text="30000.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosSueldos_2" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosSueldos_3" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosSueldos_4" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosSueldos_5" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosSueldos_6" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosSueldos_7" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosSueldos_8" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosSueldos_9" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosSueldos_10" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosSueldos_11" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosSueldos_12" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                            </tr>
                                                            <tr>
                                                                <th>Impuestos</th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosImpuestos_1" CssClass="form-control" Text="12500.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosImpuestos_2" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosImpuestos_3" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosImpuestos_4" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosImpuestos_5" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosImpuestos_6" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosImpuestos_7" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosImpuestos_8" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosImpuestos_9" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosImpuestos_10" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosImpuestos_11" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosImpuestos_12" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                            </tr>
                                                            <tr>
                                                                <th>Bancos</th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosBancos_1" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosBancos_2" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosBancos_3" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosBancos_4" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosBancos_5" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosBancos_6" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosBancos_7" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosBancos_8" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosBancos_9" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosBancos_10" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosBancos_11" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtEgresosBancos_12" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                            </tr>
                                                            <tr>
                                                                <th style="text-align:right;font-size:135%;"><asp:Label runat="server" Text="Total Egresos"></asp:Label></th>
                                                                <th style="text-align:right;font-size:135%;"><asp:Label runat="server" ID="lblTotalEgresos_1"></asp:Label></th>
                                                                <th style="text-align:right;font-size:135%;"><asp:Label runat="server" ID="lblTotalEgresos_2"></asp:Label></th>
                                                                <th style="text-align:right;font-size:135%;"><asp:Label runat="server" ID="lblTotalEgresos_3"></asp:Label></th>
                                                                <th style="text-align:right;font-size:135%;"><asp:Label runat="server" ID="lblTotalEgresos_4"></asp:Label></th>
                                                                <th style="text-align:right;font-size:135%;"><asp:Label runat="server" ID="lblTotalEgresos_5"></asp:Label></th>
                                                                <th style="text-align:right;font-size:135%;"><asp:Label runat="server" ID="lblTotalEgresos_6"></asp:Label></th>
                                                                <th style="text-align:right;font-size:135%;"><asp:Label runat="server" ID="lblTotalEgresos_7"></asp:Label></th>
                                                                <th style="text-align:right;font-size:135%;"><asp:Label runat="server" ID="lblTotalEgresos_8"></asp:Label></th>
                                                                <th style="text-align:right;font-size:135%;"><asp:Label runat="server" ID="lblTotalEgresos_9"></asp:Label></th>
                                                                <th style="text-align:right;font-size:135%;"><asp:Label runat="server" ID="lblTotalEgresos_10"></asp:Label></th>
                                                                <th style="text-align:right;font-size:135%;"><asp:Label runat="server" ID="lblTotalEgresos_11"></asp:Label></th>
                                                                <th style="text-align:right;font-size:135%;"><asp:Label runat="server" ID="lblTotalEgresos_12"></asp:Label></th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <asp:PlaceHolder ID="phEgresos" runat="server"></asp:PlaceHolder>
                                                        </tbody>
                                                    </table>

                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <%--</div>--%>
                                </div>

                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 95%">
                                                    <h4 class="panel-title">Totales
                                                        <%--<a class="accordion-toggle" data-toggle="collapse" data-parent=".accordion" href="#collapseTwo">Egresos
                                                        </a>--%>
                                                    </h4>
                                                </td>
                                                <%--<td style="width: 5%">
                                                    <asp:LinkButton ID="lbtnEgresos" runat="server" Text="<span class='shortcut-icon icon-plus'></span>" class="accordion-toggle" data-toggle="collapse" data-parent=".accordion" href="#collapseTwo" Style="color: black" />
                                                </td>--%>
                                            </tr>
                                        </table>
                                    </div>
                                    <%--<div id="collapseTwo" class="panel-collapse collapse">--%>
                                    <div class="panel-body">
                                        <div class="col-md-12 col-xs-12">
                                            <div class="widget widget-table">
                                                <div class="widget-header">
                                                    <span class="icon-external-link"></span>
                                                    <h3>Totales</h3>
                                                </div>
                                                <div class="widget-content">
                                                    <div class="col-md-8">
                                                    </div>
                                                    <table class="table table-bordered table-striped" id="dataTables-example">
                                                        <thead>
                                                            <tr>
                                                                <th>Concepto</th>
                                                                <th>Periodo 1</th>
                                                                <th>Periodo 2</th>
                                                                <th>Periodo 3</th>
                                                                <th>Periodo 4</th>
                                                                <th>Periodo 5</th>
                                                                <th>Periodo 6</th>
                                                                <th>Periodo 7</th>
                                                                <th>Periodo 8</th>
                                                                <th>Periodo 9</th>
                                                                <th>Periodo 10</th>
                                                                <th>Periodo 11</th>
                                                                <th>Periodo 12</th>
                                                            </tr>
                                                            <tr>
                                                                <th>Total</th>
                                                                <th style="text-align: right;">
                                                                    <asp:Label runat="server" ID="lblTotal_1" CssClass="form-Control" Text="0.00"></asp:Label></th>
                                                                <th style="text-align: right;">
                                                                    <asp:Label runat="server" ID="lblTotal_2" CssClass="form-Control" Text="0.00"></asp:Label></th>
                                                                <th style="text-align: right;">
                                                                    <asp:Label runat="server" ID="lblTotal_3" CssClass="form-Control" Text="0.00"></asp:Label></th>
                                                                <th style="text-align: right;">
                                                                    <asp:Label runat="server" ID="lblTotal_4" CssClass="form-Control" Text="0.00"></asp:Label></th>
                                                                <th style="text-align: right;">
                                                                    <asp:Label runat="server" ID="lblTotal_5" CssClass="form-Control" Text="0.00"></asp:Label></th>
                                                                <th style="text-align: right;">
                                                                    <asp:Label runat="server" ID="lblTotal_6" CssClass="form-Control" Text="0.00"></asp:Label></th>
                                                                <th style="text-align: right;">
                                                                    <asp:Label runat="server" ID="lblTotal_7" CssClass="form-Control" Text="0.00"></asp:Label></th>
                                                                <th style="text-align: right;">
                                                                    <asp:Label runat="server" ID="lblTotal_8" CssClass="form-Control" Text="0.00"></asp:Label></th>
                                                                <th style="text-align: right;">
                                                                    <asp:Label runat="server" ID="lblTotal_9" CssClass="form-Control" Text="0.00"></asp:Label></th>
                                                                <th style="text-align: right;">
                                                                    <asp:Label runat="server" ID="lblTotal_10" CssClass="form-Control" Text="0.00"></asp:Label></th>
                                                                <th style="text-align: right;">
                                                                    <asp:Label runat="server" ID="lblTotal_11" CssClass="form-Control" Text="0.00"></asp:Label></th>
                                                                <th style="text-align: right;">
                                                                    <asp:Label runat="server" ID="lblTotal_12" CssClass="form-Control" Text="0.00"></asp:Label></th>
                                                            </tr>
                                                            <tr>
                                                                <th>Periodo Anterior</th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtTotalPeriodoAnterior_1" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtTotalPeriodoAnterior_2" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtTotalPeriodoAnterior_3" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtTotalPeriodoAnterior_4" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtTotalPeriodoAnterior_5" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtTotalPeriodoAnterior_6" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtTotalPeriodoAnterior_7" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtTotalPeriodoAnterior_8" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtTotalPeriodoAnterior_9" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtTotalPeriodoAnterior_10" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtTotalPeriodoAnterior_11" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                                <th>
                                                                    <asp:TextBox runat="server" ID="txtTotalPeriodoAnterior_12" CssClass="form-control" Text="0.00" onkeypress="javascript:return validarNro(event)"></asp:TextBox></th>
                                                            </tr>
                                                            <tr>
                                                                <th style="text-align: right;font-size:135%;">Total Final</th>
                                                                <th style="text-align: right;font-size:135%;">
                                                                    <asp:Label runat="server" ID="lblTotalFinal_1" CssClass="form-Control" Text="0.00"></asp:Label></th>
                                                                <th style="text-align: right;font-size:135%;">
                                                                    <asp:Label runat="server" ID="lblTotalFinal_2" CssClass="form-Control" Text="0.00"></asp:Label></th>
                                                                <th style="text-align: right;font-size:135%;">
                                                                    <asp:Label runat="server" ID="lblTotalFinal_3" CssClass="form-Control" Text="0.00"></asp:Label></th>
                                                                <th style="text-align: right;font-size:135%;">
                                                                    <asp:Label runat="server" ID="lblTotalFinal_4" CssClass="form-Control" Text="0.00"></asp:Label></th>
                                                                <th style="text-align: right;font-size:135%;">
                                                                    <asp:Label runat="server" ID="lblTotalFinal_5" CssClass="form-Control" Text="0.00"></asp:Label></th>
                                                                <th style="text-align: right;font-size:135%;">
                                                                    <asp:Label runat="server" ID="lblTotalFinal_6" CssClass="form-Control" Text="0.00"></asp:Label></th>
                                                                <th style="text-align: right;font-size:135%;">
                                                                    <asp:Label runat="server" ID="lblTotalFinal_7" CssClass="form-Control" Text="0.00"></asp:Label></th>
                                                                <th style="text-align: right;font-size:135%;">
                                                                    <asp:Label runat="server" ID="lblTotalFinal_8" CssClass="form-Control" Text="0.00"></asp:Label></th>
                                                                <th style="text-align: right;font-size:135%;">
                                                                    <asp:Label runat="server" ID="lblTotalFinal_9" CssClass="form-Control" Text="0.00"></asp:Label></th>
                                                                <th style="text-align: right;font-size:135%;">
                                                                    <asp:Label runat="server" ID="lblTotalFinal_10" CssClass="form-Control" Text="0.00"></asp:Label></th>
                                                                <th style="text-align: right;font-size:135%;">
                                                                    <asp:Label runat="server" ID="lblTotalFinal_11" CssClass="form-Control" Text="0.00"></asp:Label></th>
                                                                <th style="text-align: right;font-size:135%;">
                                                                    <asp:Label runat="server" ID="lblTotalFinal_12" CssClass="form-Control" Text="0.00"></asp:Label></th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <asp:PlaceHolder ID="phTotales" runat="server"></asp:PlaceHolder>
                                                        </tbody>
                                                    </table>
                                                </div>
                                            </div>

                                            <div class="col-md-1">
                                                <asp:LinkButton ID="lbtnRecalcularTotales" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" Visible="false" />
                                            </div>
                                        </div>
                                    </div>
                                    <%--</div>--%>
                                </div>

                            </div>
                        </section>

                    </div>
                    <!-- /widget-content -->

                </div>
                <!-- /widget -->
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
                                    <fieldset>
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
                                            <label class="col-md-4">Tipo de Periodo</label>
                                            <div class="col-md-6">
                                                <asp:DropDownList ID="ListTipoPeriodo" runat="server" class="form-control">
                                                    <asp:ListItem Value="1">Meses</asp:ListItem>
                                                    <asp:ListItem Value="2">Semanas</asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <asp:PlaceHolder runat="server" ID="phSucursal" Visible="false">
                                            <div class="form-group">
                                                <label class="col-md-4">Sucursal</label>
                                                <div class="col-md-6">
                                                    <asp:DropDownList ID="DropListSucursal" runat="server" class="form-control"></asp:DropDownList>
                                                    <!-- /input-group -->
                                                </div>
                                            </div>
                                        </asp:PlaceHolder>
                                    </fieldset>
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
    <script src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script src="//code.jquery.com/ui/1.11.4/jquery-ui.js"></script>

    <script>
        $(function () {
            $("#<%= txtFechaDesde.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
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

</asp:Content>
