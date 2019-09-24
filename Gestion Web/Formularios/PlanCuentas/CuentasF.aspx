<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CuentasF.aspx.cs" Inherits="Gestion_Web.Formularios.PlanCuentas.CuentasF" %>

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
                                        <label class="col-md-1">Jerarquia:</label>
                                        <div class="col-md-2">
                                            <asp:DropDownList ID="ListJerarquia" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ListJerarquia_SelectedIndexChanged">
                                                <asp:ListItem Text="Seleccione..." Value="-1" />
                                                <asp:ListItem Text="1" Value="1" />
                                                <asp:ListItem Text="2" Value="2" />
                                                <asp:ListItem Text="3" Value="3" />
                                                <asp:ListItem Text="4" Value="4" />
                                                <asp:ListItem Text="5" Value="5" />
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="ListJerarquia" InitialValue="-1" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" ValidationGroup="CuentasGroup"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>                                    
                                    <div class="form-group">
                                        <div class="col-md-12">
                                            <asp:Panel ID="panelNivel1" runat="server" Visible="false">
                                                <label class="col-md-1">Nivel 1:</label>
                                                <div class="col-md-2">
                                                    <asp:DropDownList ID="ListNivel1" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ListNivel1_SelectedIndexChanged" >
                                                        <asp:ListItem Text="Seleccione..." Value="-1" />
                                                    </asp:DropDownList>
                                                </div>
                                            </asp:Panel>
                                            <asp:Panel ID="panelNivel2" runat="server" Visible="false">
                                                <label class="col-md-1">Nivel 2:</label>
                                                <div class="col-md-2">
                                                    <asp:DropDownList ID="ListNivel2" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ListNivel2_SelectedIndexChanged">
                                                        <asp:ListItem Text="Seleccione..." Value="-1" />
                                                    </asp:DropDownList>
                                                </div>
                                            </asp:Panel>
                                            <asp:Panel ID="panelNivel3" runat="server" Visible="false">
                                                <label class="col-md-1">Nivel 3:</label>
                                                <div class="col-md-2">
                                                    <asp:DropDownList ID="ListNivel3" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ListNivel3_SelectedIndexChanged">
                                                        <asp:ListItem Text="Seleccione..." Value="-1" />
                                                    </asp:DropDownList>
                                                </div>
                                            </asp:Panel>
                                            <asp:Panel ID="panelNivel4" runat="server" Visible="false">
                                                <label class="col-md-1">Nivel 4:</label>
                                                <div class="col-md-2">
                                                    <asp:DropDownList ID="ListNivel4" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ListNivel4_SelectedIndexChanged">
                                                        <asp:ListItem Text="Seleccione..." Value="-1" />
                                                    </asp:DropDownList>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                    </div>                                    
                                    <div class="form-group">
                                        <label class="col-md-1">Codigo:</label>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtCodigo" runat="server" class="form-control"/>
                                        </div>
                                        <label class="col-md-1">Descripcion:</label>
                                        <div class="col-md-3">
                                            <asp:TextBox ID="txtDescripcion" runat="server" class="form-control"/>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:LinkButton ID="lbtnAgregar" Text="<span class='shortcut-icon icon-ok'></span>" runat="server" class="btn btn-success" ValidationGroup="CuentasGroup" OnClick="lbtnAgregar_Click" />
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
                        <i class="icon-list"></i>
                        <h3>Cuentas</h3>
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
                                                    <th style="width: 25%">Codigo</th>
                                                    <th style="width: 45%">Cuenta</th>
                                                    <th class="td-actions" style="width: 10%"></th>
                                                </tr>

                                            </thead>
                                            <tbody>
                                                <asp:PlaceHolder ID="phCuentas" runat="server"></asp:PlaceHolder>
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
                                        <asp:Label runat="server" ID="lblMensaje" Text="Esta seguro que desea eliminar?" Style="text-align: center"></asp:Label>
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


