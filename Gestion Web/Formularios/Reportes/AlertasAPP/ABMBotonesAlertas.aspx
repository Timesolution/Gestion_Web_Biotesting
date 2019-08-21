<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ABMBotonesAlertas.aspx.cs" Inherits="Gestion_Web.Formularios.Reportes.AlertasAPP.ABMBotonesAlertas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">
        <div class="container">
            <div class="col-md-12 col-xs-12">
                <div class="widget stacked">
                    <div class="widget-header">
                        <i class="icon-wrench"></i>
                        <h3>Herramientas</h3>
                    </div>
                    <div class="widget-content">
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 45%">
                                    <label class="col-md-2">Boton</label>
                                    <div class="col-md-6">
                                        <asp:DropDownList ID="listBotones" runat="server" class="form-control"></asp:DropDownList>
                                    </div>
                                </td>
                                <td style="width: 45%">
                                    <label class="col-md-2">Mensaje</label>
                                    <div class="col-md-6">
                                        <asp:TextBox ID="txtMensaje" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                </td>
                                <td style="width: 10%">
                                    <div class="col-md-1">
                                        <div class="shortcuts">
                                            <asp:LinkButton ID="lbtnAgregar" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClientClick="return ValidarMensaje()" OnClick="lbtnAgregar_Click"/>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>

            <div class="col-md-12 col-xs-12">
                <div class="widget stacked widget-table action-table">
                    <div class="widget-header">
                        <i class="icon-th-list"></i>
                        <h3>Mensajes</h3>
                    </div>
                    <div class="widget-content">
                        <div class="panel-body">
                            <div class="table-responsive">
                                <table class="table table-striped table-bordered table-hover" id="dataTables-example">
                                    <thead>
                                        <tr>
                                            <th>Boton</th>
                                            <th>Mensaje</th>
                                            <th></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:PlaceHolder ID="phBotones" runat="server"></asp:PlaceHolder>
                                    </tbody>
                                </table>
                            </div>
                        </div>
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
                                <div class="col-md-2">
                                    <h1>
                                        <i class="icon-warning-sign" style="color: orange"></i>
                                    </h1>
                                </div>
                                <div class="col-md-7">
                                    <h5>
                                        <asp:Label runat="server" ID="lblMensaje" Text="Esta seguro que desea eliminar el mensaje?" Style="text-align: center"></asp:Label>
                                    </h5>
                                </div>
                                <div class="col-md-3">
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

        <script src="../../../../Scripts/libs/jquery-1.9.1.min.js"></script>
        <script src="../../../../Scripts/libs/jquery-ui-1.10.0.custom.min.js"></script>
        <link href="../../../../css/pages/reports.css" rel="stylesheet">
        <script src="//cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>
        <script src="//cdn.datatables.net/plug-ins/1.10.9/sorting/date-eu.js"></script>
        <script src="../../../../Scripts/plugins/dataTables/custom.tables.js"></script>
        <link href="//cdn.datatables.net/1.10.2/css/jquery.dataTables.css" rel="stylesheet" />
        <script src="../../../../Scripts/plugins/lightbox/jquery.lightbox.min.js"></script>
        <script src="../../../../Scripts/plugins/msgbox/jquery.msgbox.min.js"></script>
        <script src="../../../../scripts/demo/notifications.js"></script>

        <script>

            function abrirdialog(valor)
            {
                document.getElementById('<%= txtMovimiento.ClientID %>').value = valor;
            }

            function ValidarMensaje() {
                var mensaje = document.getElementById('MainContent_txtMensaje');

                if (mensaje.value == "" || mensaje.value == undefined)
                {
                    $.msgbox("El campo mensaje esta vacio!", { type: "alert" });
                    return false;
                }
            }

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
                    else { return false; }
                }
                return true;
            }
        </script>

    </div>
</asp:Content>
