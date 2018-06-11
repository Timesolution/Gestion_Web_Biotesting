<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MovimientoCajaF.aspx.cs" Inherits="Gestion_Web.Formularios.Valores.MovimientoCajaF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">

        <div>

            <div class="col-md-12 col-xs-12">
                <div class="widget stacked">

                    <div class="widget-header">
                        <i class="icon-wrench"></i>
                        <h3>Herramientas</h3>
                    </div>
                    <!-- /widget-header -->

                    <div class="widget-content">
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 30%">
                                    <label class="col-md-3">Movimiento</label>
                                    <div class="col-md-8">
                                        <asp:TextBox ID="txtMov" runat="server" class="form-control"></asp:TextBox>
                                        <!-- /input-group -->
                                    </div>
                                    <div class="col-md-1">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="*" ControlToValidate="txtMov" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" ValidationGroup="SubGrupoGroup"></asp:RequiredFieldValidator>
                                    </div>
                                </td>
                                <td style="width: 30%">
                                    <label class="col-md-2">Tipo</label>
                                    <div class="col-md-9">
                                        <asp:DropDownList ID="ListTipos" runat="server" class="form-control">
                                            <asp:ListItem Value="-1">Seleccione...</asp:ListItem>
                                            <asp:ListItem Value="1">Ingreso</asp:ListItem>
                                            <asp:ListItem Value="2">Egreso</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-1">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="ListTipos" InitialValue="-1" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" ValidationGroup="SubGrupoGroup"></asp:RequiredFieldValidator>
                                    </div>
                                </td>
                                <td style="width: 30%">
                                    <asp:Panel ID="PanelCtaContable" runat="server" Visible="false">
                                        <a href="#modalCuentas" data-toggle="modal" class="btn btn-success">Plan de ctas.</a>
                                    </asp:Panel>
                                </td>
                                <td style="width: 10%">
                                    <div class="col-md-1">
                                        <div class="shortcuts">
                                            <asp:LinkButton ID="lbtnAgregar" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnAgregar_Click" ValidationGroup="SubGrupoGroup" />
                                        </div>
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
                <div class="widget stacked widget-table action-table">

                    <div class="widget-header">
                        <i class="icon-th-list"></i>
                        <h3>Movimientos de Caja

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
                                                    <th>Movimiento</th>
                                                    <th>Tipo</th>
                                                    <asp:PlaceHolder ID="phColumna" Visible="false" runat="server">
                                                        <th>Cuenta</th>
                                                    </asp:PlaceHolder>
                                                    <th></th>
                                                </tr>

                                            </thead>
                                            <tbody>
                                                <asp:PlaceHolder ID="phMovimientosCaja" runat="server"></asp:PlaceHolder>
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
                                        <asp:Label runat="server" ID="lblMensaje" Text="Esta seguro que desea eliminar el movimiento de Caja?" Style="text-align: center"></asp:Label>
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

        <div id="modalCuentas" class="modal fade" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Cuentas contables</h4>
                    </div>
                    <div class="modal-body">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <div role="form" class="form-horizontal col-md-12">
                                    <div class="form-group">
                                        <div class="col-md-12">
                                            <label class="col-md-3">Nivel 1:</label>
                                            <div class="col-md-6">
                                                <asp:DropDownList ID="ListCtaContables1" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ListCtaContables1_SelectedIndexChanged" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-12">
                                            <label class="col-md-3">Nivel 2:</label>
                                            <div class="col-md-6">
                                                <asp:DropDownList ID="ListCtaContables2" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ListCtaContables2_SelectedIndexChanged" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-12">
                                            <label class="col-md-3">Nivel 3:</label>
                                            <div class="col-md-6">
                                                <asp:DropDownList ID="ListCtaContables3" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ListCtaContables3_SelectedIndexChanged" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-12">
                                            <label class="col-md-3">Nivel 4:</label>
                                            <div class="col-md-6">
                                                <asp:DropDownList ID="ListCtaContables" runat="server" class="form-control" />
                                            </div>
                                            <div class="col-md-1">
                                                <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="ListCtaContables" InitialValue="-1" ForeColor="Red" Font-Bold="true" runat="server" ValidationGroup="CtaContableGroup" />
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div class="modal-footer">
                            <asp:LinkButton ID="lbtnAgregarMovCtaCbe" runat="server" class="btn btn-success" Text="Guardar" OnClick="lbtnAgregarMovCtaCbe_Click" ValidationGroup="CtaContableGroup" />
                            <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                        </div>
                    </div>

                </div>
            </div>
        </div>
        <%--Fin modal--%>
        <script>
            function abrirdialog(valor) {
                document.getElementById('<%= txtMovimiento.ClientID %>').value = valor;
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

        <script src="../../Scripts/plugins/msgGrowl/js/msgGrowl.js"></script>
        <script src="../../Scripts/plugins/lightbox/jquery.lightbox.min.js"></script>
        <script src="../../Scripts/plugins/msgbox/jquery.msgbox.min.js"></script>
        <script src="../../Scripts/demo/notifications.js"></script>


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

    </div>
</asp:Content>


