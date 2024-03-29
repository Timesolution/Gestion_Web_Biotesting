﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ABMTarjetas.aspx.cs" Inherits="Gestion_Web.Formularios.Valores.ABMTarjetas" %>

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

                    <div class="widget-content">
                        <div role="form" class="form-horizontal col-md-12">
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <div class="form-group">
                                        <label class="col-md-1">Operador</label>
                                        <div class="col-md-2">
                                            <asp:DropDownList ID="ListOperadores" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="ListOperadores" InitialValue="-1" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" ValidationGroup="TarjetaGroup"></asp:RequiredFieldValidator>
                                        </div>

                                        <label class="col-md-1">Nombre</label>
                                        <div class="col-md-2">
                                            <asp:TextBox ID="txtNombre" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtNombre" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" ValidationGroup="TarjetaGroup"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">                                        
                                        <label class="col-md-1">Cuotas</label>
                                        <div class="col-md-2">
                                            <asp:TextBox ID="txtCuotas" runat="server" class="form-control" Text="0" Style="text-align: right" onkeypress="javascript:return validarSoloNro(event)"></asp:TextBox>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtCuotas" ValidationExpression="\d+" SetFocusOnError="true" ValidationGroup="TarjetaGroup" ForeColor="Red" Font-Bold="true" />
                                        </div>

                                        <label class="col-md-1">Recargo</label>
                                        <div class="col-md-2">
                                            <asp:TextBox ID="txtRecargo" runat="server" class="form-control" Text="0" Style="text-align: right" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtRecargo" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" SetFocusOnError="true" ValidationGroup="TarjetaGroup" ForeColor="Red" Font-Bold="true" />
                                        </div>
                                    </div>
                                    <label>Tipo acreditacion</label>
                                    <div class="content clearfix">
                                        <div class="form-group">
                                            <label class="col-md-2">Acreditacion en </label>
                                            <div class="col-md-2">
                                                <asp:TextBox ID="txtDiasAcreditados" Style="text-align: right" runat="server" class="form-control" Text="0" onkeypress="javascript:return validarSoloNro(event)"></asp:TextBox>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtDiasAcreditados" ValidationExpression="\d+" SetFocusOnError="true" ValidationGroup="TarjetaGroup" ForeColor="Red" Font-Bold="true" />
                                            </div>
                                            <div class="col-md-1">
                                                <asp:RadioButton ID="rbtnDias" runat="server" GroupName="GroupDias" Checked="true" AutoPostBack="true" OnCheckedChanged="rbtnFecha_CheckedChanged" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-2">Dia Acreditacion </label>
                                            <div class="col-md-2">
                                                <asp:TextBox ID="txtFechaAcreditacion" Style="text-align: right" runat="server" class="form-control" disabled Text="0" onkeypress="javascript:return validarSoloNro(event)"></asp:TextBox>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaAcreditacion" ValidationExpression="\d+" SetFocusOnError="true" ValidationGroup="TarjetaGroup" ForeColor="Red" Font-Bold="true" />
                                            </div>
                                            <label class="col-md-2">Dia Cierre </label>
                                            <div class="col-md-2">
                                                <asp:TextBox ID="txtCierre" Style="text-align: right" runat="server" class="form-control" Text="0" disabled onkeypress="javascript:return validarSoloNro(event)"></asp:TextBox>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtCierre" ValidationExpression="\d+" SetFocusOnError="true" ValidationGroup="TarjetaGroup" ForeColor="Red" Font-Bold="true" />
                                            </div>
                                            
                                            <div class="col-md-1">
                                                <asp:RadioButton ID="rbtnFecha" runat="server" GroupName="GroupDias" AutoPostBack="true" OnCheckedChanged="rbtnFecha_CheckedChanged" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <%--<label class="col-md-1">Cuotas</label>
                                        <div class="col-md-2">
                                            <asp:TextBox ID="txtCuotas" runat="server" class="form-control" Text="0" Style="text-align: right" onkeypress="javascript:return validarSoloNro(event)"></asp:TextBox>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtCuotas" ValidationExpression="\d+" SetFocusOnError="true" ValidationGroup="TarjetaGroup" ForeColor="Red" Font-Bold="true" />
                                        </div>

                                        <label class="col-md-1">Recargo</label>
                                        <div class="col-md-2">
                                            <asp:TextBox ID="txtRecargo" runat="server" class="form-control" Text="0" Style="text-align: right" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtRecargo" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" SetFocusOnError="true" ValidationGroup="TarjetaGroup" ForeColor="Red" Font-Bold="true" />
                                        </div>--%>
                                        <div class="col-md-1">
                                            <asp:LinkButton ID="lbtnAgregar" runat="server" Text="Agregar" class="btn btn-success" OnClick="btnAgregar_Click" ValidationGroup="TarjetaGroup" />
                                        </div>
                                    </div>
                                    
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <!-- /widget-content -->

                </div>
                <!-- /widget -->
            </div>
            <div class="col-md-12 col-xs-12">
                <div class="widget stacked widget-table action-table">

                    <div class="widget-header">
                        <i class="icon-credit-card"></i>
                        <h3>Tarjetas

                        </h3>
                    </div>
                    <div class="widget-content">
                        <div class="panel-body">

                            <%--<div class="col-md-12 col-xs-12">--%>
                            <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <div class="table-responsive">
                                        <a class="btn btn-info" style="display: none" data-toggle="modal" id="abreDialog" href="#modalFacturaDetalle">Agregar Tipo Cliente</a>
                                        <table class="table table-striped table-bordered table-hover" id="dataTables-example">
                                            <thead>
                                                <tr>
                                                    <th style="width: 20%">Operador</th>
                                                    <th style="width: 30%">Nombre</th>
                                                    <th style="width: 10%">Acreditacion en</th>
                                                    <th style="width: 10%">Dia Acreditacion</th>
                                                    <th style="width: 10%">Cuotas</th>
                                                    <th style="width: 10%">Recargo</th>
                                                    <th class="td-actions" style="width: 10%"></th>
                                                </tr>

                                            </thead>
                                            <tbody>
                                                <asp:PlaceHolder ID="phTarjetas" runat="server"></asp:PlaceHolder>
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

        <%--<div id="modalAgregar" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Agregar tarjeta</h4>
                    </div>
                    <div class="modal-body">
                    </div>
                    <div class="modal-footer">
                        <asp:LinkButton ID="lbtnAgregar" runat="server" Text="Agregar" class="btn btn-success" OnClick="btnAgregar_Click" ValidationGroup="TarjetaGroup" />
                        <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                    </div>
                </div>
            </div>
        </div> --%>

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
                                        <asp:Label runat="server" ID="lblMensaje" Text="Esta seguro que desea eliminar la Tarjeta?" Style="text-align: center"></asp:Label>
                                    </h5>
                                </div>

                                <div class="col-md-3">
                                    <asp:TextBox runat="server" ID="txtMovimiento" Text="0" Style="display: none"></asp:TextBox>
                                </div>
                            </div>
                            <%--                                <div class="form-group">
                                    
                                </div>--%>
                        </div>


                        <div class="modal-footer">
                            <asp:Button runat="server" ID="btnSi" Text="Eliminar" class="btn btn-danger" OnClick="btnSi_Click" />
                            <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>

                            <%--                        <asp:LinkButton ID="LinkButton1" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnBuscar_Click" ValidationGroup="BusquedaGroup" />--%>
                        </div>
                    </div>

                </div>
            </div>
        </div>
        <%--Fin modalGrupo--%>

        <script>
            function abrirdialog(valor) {
                document.getElementById('<%= txtMovimiento.ClientID %>').value = valor;
            }
        </script>

        <script type="text/javascript">
            function openModal() {
                $('#modalAgregar').modal('show');
            }
        </script>

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
                    "bFilter": false,
                    "bInfo": false,
                    "bAutoWidth": false                    
                });
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
                    if (key == 46 || key == 8) // Detectar . (punto) y backspace (retroceso)
                    { return true; }
                    else
                    { return false; }
                }
                return true;
            }
        </script>

        <script>
            //valida los campos solo numeros
            function validarSoloNro(e) {
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
                    return false;
                }
                else { return true; }
            }
        </script>

    </div>
</asp:Content>
