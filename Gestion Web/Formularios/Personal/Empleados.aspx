<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Empleados.aspx.cs" Inherits="Gestion_Web.Formularios.Personal.Empleados" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">

        <div>

            <div class="col-md-12 col-xs-12">
                <div class="widget stacked">
                    <div class="stat">
                        <h5><i class="icon-map-marker"></i> Personal > Personal</h5>
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
                                                <asp:LinkButton ID="lbtnRemuneracion" runat="server" data-toggle="modal" href="#modalAgregar">Generar Remuneracion</asp:LinkButton>
                                            </li>
                                        </ul>
                                    </div>
                                    <!-- /btn-group -->
                                </td>
                                <td style="width: 30%">
                                    <div class="col-lg-8">
                                        <div class="input-group">
                                            <asp:TextBox ID="txtBusqueda" runat="server" class="form-control" placeholder="Buscar Empleado"></asp:TextBox>
                                            <span class="input-group-btn">
                                                <asp:LinkButton ID="lbBuscar" runat="server" Text="Buscar" class="btn btn-primary" OnClick="btnBuscar_Click">
                                                    <i class="shortcut-icon icon-search"></i></asp:LinkButton>
                                            </span>

                                        </div>
                                        <!-- /input-group -->
                                    </div>
                                </td>

                                <td style="width: 50%"></td>

                                <td style="width: 5%">
                                    <%--<div class="shortcuts" style="height:50%">--%>



                                    <a href="EmpleadosABM.aspx?accion=1" class="btn btn-primary ui-tooltip" data-toggle="tooltip" title data-original-title="Agregar" style="width: 100%">
                                        <i class="shortcut-icon icon-plus"></i>
                                        <%--<span class="shortcut-label">Users</span>--%>
                                    </a>

                                    <%--</div>--%>

                                </td>

                                <%--<td style="width: 5%">
                                    <div class="shortcuts">
                                        <%--<asp:LinkButton ID="LinkButton1" runat="server" class="shortcut" OnClick="LinkButton1_Click"></asp:LinkButton>

                                        <a href="/" id="linkEditar" runat="server" class="btn btn-default ui-tooltip" data-toggle="tooltip" title data-original-title="Editar" style="width: 100%">

                                            <i class="shortcut-icon icon-pencil"></i>
                                            <%--<span class="shortcut-label">Users</span>
                                        </a>
                                    </div>
                                </td>--%>


                                <%--<td style="width: 5%">
                                    <div class="shortcuts" style="height: 100%">
                                        <asp:LinkButton ID="LinkButton1" runat="server" class="shortcut" OnClick="LinkButton1_Click"></asp:LinkButton>

                                        <a href="/" id="A1" runat="server" class="btn btn-primary ui-tooltip" data-toggle="tooltip" title data-original-title="Imprimir" style="width: 100%">

                                            <i class="shortcut-icon icon-print"></i>
                                            <span class="shortcut-label">Users</span>
                                        </a>
                                    </div>
                                </td>--%>

                                <%--<td style="width: 5%">
                                    <div class="shortcuts" style="height: 100%">
                                        <%--<asp:LinkButton ID="LinkButton1" runat="server" class="shortcut" OnClick="LinkButton1_Click"></asp:LinkButton>

                                        <a href="/" id="linkStock" runat="server" class="btn btn-default ui-tooltip" data-toggle="tooltip" title data-original-title="Stock" style="width: 100%">
                                            <i class="shortcut-icon icon-edit"></i>

                                        </a>
                                    </div>
                                </td>--%>

                                <%--<td style="width: 5%">
                                    <div class="shortcuts">
                                        <%--<asp:LinkButton ID="LinkButton1" runat="server" class="shortcut" OnClick="LinkButton1_Click"></asp:LinkButton>

                                        <a href="/" id="A2" runat="server" class="btn btn-default ui-tooltip" data-toggle="tooltip" title data-original-title="Eliminar" style="width: 100%">

                                            <i class="shortcut-icon icon-trash"></i>
                                            <%--<span class="shortcut-label">Users</span>
                                        </a>
                                    </div>
                                </td>--%>
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
                        <i class="icon-group"></i>
                        <h3>Empleados</h3>
                    </div>
                    <div class="widget-content">
                        <div class="panel-body">

                            <%--<div class="col-md-12 col-xs-12">--%>
                            <div class="table-responsive">
                                <table class="table table-striped table-bordered table-hover" id="dataTables-example">
                                    <thead>
                                        <tr>
                                            <th style="width: 10%">Legajo</th>
                                            <th style="width: 30%">Nombre y Apellido</th>
                                            <th id="thTxtDNI" runat="server" style="width: 10%">DNI</th>
                                            <th id="thTxtCUIT" runat="server" style ="width: 15%">CUIT</th>
                                            <th style="width: 10%">Fecha Ingreso</th>
                                            <th style="width: 10%">Remuneracion</th>
                                            <th class="td-actions" style="width: 10%"></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:PlaceHolder ID="phEmpleados" runat="server"></asp:PlaceHolder>
                                    </tbody>
                                </table>
                            </div>


                        </div>


                        <!-- /.content -->

                    </div>

                </div>
            </div>
            <!-- /container -->
            <!-- /container -->

        </div>
        <!-- /main -->
        <div id="modalAgregar" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                        <ContentTemplate>
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                                <h4 class="modal-title">Remuneracion</h4>
                            </div>
                            <div class="modal-body">
                                <div role="form" class="form-horizontal col-md-12">
                                    <div class="form-group">
                                        <label class="col-md-4">Empresa</label>
                                        <div class="col-md-7">
                                            <asp:DropDownList ID="DropListEmpresa" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="DropListEmpresa_SelectedIndexChanged"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListEmpresa" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Sucursal</label>
                                        <div class="col-md-7">
                                            <asp:DropDownList ID="DropListSucursal" runat="server" class="form-control" OnSelectedIndexChanged="DropListSucursal_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListSucursal" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Punto Vta</label>
                                        <div class="col-md-7">
                                            <asp:DropDownList ID="DropListPtoVenta" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-1">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListPtoVenta" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Periodo</label>
                                        <div class="col-md-3">
                                           <asp:TextBox ID="txtPeriodo" runat="server" TextMode="Number" class="form-control" Text="2015" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:DropDownList ID="DropListMes" runat="server" class="form-control">
                                                <asp:ListItem Text="Enero" Value="ENE"></asp:ListItem>
                                                <asp:ListItem Text="Febrero" Value="FEB"></asp:ListItem>
                                                <asp:ListItem Text="Marzo" Value="MAR"></asp:ListItem>
                                                <asp:ListItem Text="Abril" Value="ABR"></asp:ListItem>
                                                <asp:ListItem Text="Mayo" Value="MAY"></asp:ListItem>
                                                <asp:ListItem Text="Junio" Value="JUN"></asp:ListItem>
                                                <asp:ListItem Text="Julio" Value="JUL"></asp:ListItem>
                                                <asp:ListItem Text="Agosto" Value="AGO"></asp:ListItem>
                                                <asp:ListItem Text="Septiembre" Value="SEP"></asp:ListItem>
                                                <asp:ListItem Text="Octubre" Value="OCT"></asp:ListItem>
                                                <asp:ListItem Text="Noviembre" Value="NOV"></asp:ListItem>
                                                <asp:ListItem Text="Diciembre" Value="DIC"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:LinkButton ID="lbtnAgregarRemuneracion" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnAgregarRemuneracion_Click" ValidationGroup="BusquedaGroup" />
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
                                    <asp:Label runat="server" ID="lblMensaje" Text="Esta seguro que desea eliminar el Personal?" Style="text-align: center"></asp:Label>
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

    </div>

    <%--<script src="//code.jquery.com/jquery-1.9.1.js"></script>
    <script src="//code.jquery.com/ui/1.10.4/jquery-ui.js"></--%>

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
    <script src="//cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>
    <script src="../../Scripts/plugins/dataTables/custom.tables.js"></script>
    <link href="//cdn.datatables.net/1.10.2/css/jquery.dataTables.css" rel="stylesheet" />

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
                if (key == 46 || key == 8 || key == 44) // Detectar . (punto) , backspace (retroceso) y , (coma)
                { return true; }
                else
                { return false; }
            }
            return true;
        }
    </script>

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

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endReq);
        function endReq(sender, args) {
            $('#dataTables-example').dataTable();
        }
    </script>

    <script>
        function abrirdialog(valor) {
            document.getElementById('<%= txtMovimiento.ClientID %>').value = valor;
        }
    </script>



</asp:Content>
