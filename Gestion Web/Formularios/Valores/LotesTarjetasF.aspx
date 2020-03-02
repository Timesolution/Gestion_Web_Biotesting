﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LotesTarjetasF.aspx.cs" Inherits="Gestion_Web.Formularios.Valores.LotesTarjetasF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">
        <%--<div class="container">--%><div>
            <div class="col-md-12 col-xs-12">
                <div class="widget stacked">
                    <div class="stat">
                        <h5><i class="icon-map-marker"></i> Valores > Lotes</h5>
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
                                    <div class="btn-group">
                                        <button type="button" class="btn btn-success dropdown-toggle" data-toggle="dropdown" id="btnAccion" runat="server">Accion    <span class="caret"></span></button>
                                        <ul class="dropdown-menu">
                                            <li>
                                                <asp:LinkButton ID="btnLiquidar" runat="server" data-toggle="modal" href="#modalLiquidar">Liquidar</asp:LinkButton>
                                            </li>
                                        </ul>
                                    </div>
                                    <!-- /btn-group -->
                                </td>
                                <td style="width: 75%">&nbsp
                                </td>
                                <td style="width: 2%">
                                    <div class="shortcuts" style="height: 100%">
                                        <a class="btn btn-primary" data-toggle="modal" href="#modalBusqueda" style="width: 100%">
                                            <i class="shortcut-icon icon-filter"></i>
                                        </a>
                                    </div>
                                </td>
                                <td style="width: 1%">&nbsp
                                </td>
                                <td style="width: 2%">
                                    <div class="shortcuts" style="height: 100%">
                                        <%--<a class="btn btn-primary" data-toggle="modal" href="#modalAgregar" style="width: 100%">
                                            <i class="shortcut-icon icon-plus"></i>
                                        </a>--%>
                                        <asp:LinkButton ID="btnAgregar" runat="server" Text="<span class='shortcut-icon icon-plus'></span>" class="btn btn-primary" OnClick="btnAgregar_Click" />
                                    </div>
                                </td>
                            </tr>

                        </table>
                    </div>
                    <!-- /widget-content -->

                </div>
                <!-- /widget -->


                <div class="widget big-stats-container stacked">

                    <div class="widget-content">

                        <div id="big_stats" class="cf">
                            <div class="stat">
                                <h4>Saldo</h4>
                                <asp:Label ID="Label1" runat="server" Text="" class="value"></asp:Label>
                            </div>
                            <!-- .stat -->
                        </div>

                    </div>
                    <!-- /widget-content -->

                </div>



            </div>
            <div class="col-md-12 col-xs-12">
                <div class="widget stacked widget-table action-table">

                    <div class="widget-header">
                        <i class="icon-money" style="width: 2%"></i>
                        <h3 style="width: 75%">Lotes tarjetas                                    
                        </h3>
                        <h3>
                            <asp:Label ID="lblSaldo" runat="server" Style="text-align: end" Text="" ForeColor="#0099ff" Font-Bold="true"></asp:Label>
                        </h3>
                    </div>
                    <div class="widget-content">
                        <div class="panel-body">

                            <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <div class="table-responsive">
                                        <a class="btn btn-info" style="display: none" data-toggle="modal" id="abreDialog" href="#modalFacturaDetalle">Agregar Tipo Cliente</a>
                                        <table class="table table-striped table-bordered table-hover" id="dataTables-example">
                                            <thead>
                                                <tr>
                                                    <th>Fecha Cierre</th>
                                                    <th>Posnet</th>
                                                    <th>Tarjeta</th>
                                                    <th>Numero Lote</th>
                                                    <th>Cant. Cupones</th>
                                                    <th>Importe</th>                                                    
                                                    <th>Liquidado</th>
                                                    <th>Restan</th>
                                                    <th></th>
                                                </tr>

                                            </thead>
                                            <tbody>
                                                <asp:PlaceHolder ID="phCierres" runat="server"></asp:PlaceHolder>
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
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                                <h4 class="modal-title">Busqueda</h4>
                            </div>
                            <div class="modal-body">
                                <div role="form" class="form-horizontal col-md-12">

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
                                        <label class="col-md-4">Posnet</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListPosnet" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="DropListPosnet_SelectedIndexChanged"></asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListPosnet" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Tarjeta</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="ListTarjetas" runat="server" class="form-control"></asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="ListTarjetas" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Incluir importe $0.00</label>
                                        <div class="col-md-6">
                                            <asp:CheckBox ID="chkImporteCero" runat="server" />                                            
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:LinkButton ID="lbtnBuscar" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnBuscar_Click" ValidationGroup="BusquedaGroup" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
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
                                <div class="col-md-2">
                                    <h1>
                                        <i class="icon-warning-sign" style="color: orange"></i>
                                    </h1>
                                </div>
                                <div class="col-md-7">
                                    <h5>
                                        <asp:Label runat="server" ID="lblMensaje" Text="Esta seguro que desea anular este cierre de lote?" Style="text-align: center"></asp:Label>
                                    </h5>
                                </div>

                                <div class="col-md-3">
                                    <asp:TextBox runat="server" ID="txtCierre" Text="0" Style="display: none"></asp:TextBox>
                                </div>

                                <div class="col-md-3">
                                    <asp:TextBox runat="server" ID="TextBox1" Text="0" Style="display: none"></asp:TextBox>
                                </div>

                            </div>

                        </div>


                        <div class="modal-footer">
                            <asp:Button runat="server" ID="btnQuitarCierre" Text="Eliminar" class="btn btn-danger" OnClick="btnQuitarCierre_Click" />
                            <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                        </div>
                    </div>

                </div>
            </div>
        </div>

        <div id="modalLiquidar" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">

                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Agregar Liquidacion</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                                <ContentTemplate>       
                                    <div class="form-group">
                                        <label class="col-md-4">Fecha Liquidacion:</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtFechaLiquidacion" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaLiquidacion" ValidationGroup="ImputarGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Numero Liquidacion:</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtNumeroLiquidacion" runat="server" class="form-control" ></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                    
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtNumeroLiquidacion" ValidationGroup="ImputarGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Liquidar importe:</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtImporteLiquidar" runat="server" disabled Text="0" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RadioButton ID="rbtnImporte" runat="server" AutoPostBack="true" OnCheckedChanged="rbtnImporte_CheckedChanged" GroupName="radioLiquidar" />
                                        </div>                                
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Liquidar total:</label>
                                        <div class="col-md-4">                                    
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RadioButton ID="rbtnTotal" runat="server" AutoPostBack="true" OnCheckedChanged="rbtnTotal_CheckedChanged" GroupName="radioLiquidar" />
                                        </div>

                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>                            

                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:LinkButton ID="btnSi" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnSi_Click" ValidationGroup="ImputarGroup" />
                    </div>
                </div>

            </div>
        </div>



        <script>

            function abrirdialog(valor) {
                document.getElementById('<%= txtCierre.ClientID %>').value = valor;
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
                    if (key == 46 || key == 8 || key == 44)// Detectar . (punto) y backspace (retroceso) y , (coma)
                    { return true; }
                    else
                    { return false; }
                }
                return true;
            }
        </script>
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

        <%--<script src="//code.jquery.com/jquery-1.9.1.js"></script>--%>
        <script src="//code.jquery.com/ui/1.10.4/jquery-ui.js"></script>

        <script>


            $(function () {
                $("#<%= txtFechaDesde.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            });

            $(function () {
                $("#<%= txtFechaHasta.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            });

            $(function () {
                $("#<%= txtFechaLiquidacion.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            });

        </script>
    </div>
</asp:Content>
