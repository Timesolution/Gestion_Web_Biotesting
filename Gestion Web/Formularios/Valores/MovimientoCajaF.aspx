<%@ Page Title="" Language="C#" EnableEventValidation="false" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MovimientoCajaF.aspx.cs" Inherits="Gestion_Web.Formularios.Valores.MovimientoCajaF" %>

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
                                        <asp:DropDownList ID="dropListTipo_Debe_Haber" runat="server" class="form-control">
                                            <asp:ListItem Value="-1">Seleccione...</asp:ListItem>
                                            <asp:ListItem Value="1">Ingreso</asp:ListItem>
                                            <asp:ListItem Value="2">Egreso</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-1">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="dropListTipo_Debe_Haber" InitialValue="-1" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" ValidationGroup="SubGrupoGroup"></asp:RequiredFieldValidator>
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
                                            <asp:LinkButton ID="lbtnAgregar1" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnAgregar1OModificarMovimiento_Click" ValidationGroup="SubGrupoGroup" />
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
                                                <asp:DropDownList ID="ListCtaContables1" runat="server" class="form-control" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-12">
                                            <label class="col-md-3">Nivel 2:</label>
                                            <div class="col-md-6">
                                                <asp:DropDownList ID="ListCtaContables2" runat="server" class="form-control" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-12">
                                            <label class="col-md-3">Nivel 3:</label>
                                            <div class="col-md-6">
                                                <asp:DropDownList ID="ListCtaContables3" runat="server" class="form-control" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-12">
                                            <label class="col-md-3">Nivel 4:</label>
                                            <div class="col-md-6">
                                                <asp:DropDownList ID="ListCtaContables4" runat="server" class="form-control" />
                                            </div>
                                            <div class="col-md-1">
                                                <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="ListCtaContables4" InitialValue="-1" ForeColor="Red" Font-Bold="true" runat="server" ValidationGroup="CtaContableGroup" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="col-md-12">
                                            <label class="col-md-3">Nivel 5:</label>
                                            <div class="col-md-6">
                                                <asp:DropDownList ID="ListCtaContables5" runat="server" class="form-control" />
                                            </div>
                                            <div class="col-md-1">
                                                <asp:RequiredFieldValidator ErrorMessage="*" ControlToValidate="ListCtaContables5" InitialValue="-1" ForeColor="Red" Font-Bold="true" runat="server" ValidationGroup="CtaContableGroup" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <div class="modal-footer">
                            <asp:LinkButton ID="lbtnAgregarMovCtaCbe" runat="server" class="btn btn-success" Text="Guardar" OnClientClick="Javascript: return AgregarOModificarCuentaContableCaja();" ValidationGroup="CtaContableGroup" />
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
            var controlDropListNivel1;
            var controlDropListNivel2;
            var controlDropListNivel3;
            var controlDropListNivel4;
            var controlDropListNivel5;

            var controlTxtDescripcion;
            var controlDropListTipo_Debe_Haber;

            var dropLists = [];

            function pageLoad() {
                AsignarControles_DropListNiveles();

                controlTxtDescripcion = document.getElementById('<%= txtMov.ClientID %>');
                controlDropListTipo_Debe_Haber = document.getElementById('<%= dropListTipo_Debe_Haber.ClientID %>');
            }

            function BorrarLosDropListDeNiveles() {

                for (var i in dropLists) {
                    while (dropLists[i].options.length > 0) {
                        dropLists[i].remove(0);
                    }
                }
            }

            function AsignarControles_DropListNiveles() {
                controlDropListNivel1 = document.getElementById('<%= ListCtaContables1.ClientID %>');
                controlDropListNivel2 = document.getElementById('<%= ListCtaContables2.ClientID %>');
                controlDropListNivel3 = document.getElementById('<%= ListCtaContables3.ClientID %>');
                controlDropListNivel4 = document.getElementById('<%= ListCtaContables4.ClientID %>');
                controlDropListNivel5 = document.getElementById('<%= ListCtaContables5.ClientID %>');

                controlDropListNivel1.addEventListener("change", CargarNivel2);
                controlDropListNivel2.addEventListener("change", CargarNivel3);
                controlDropListNivel3.addEventListener("change", CargarNivel4);
                controlDropListNivel4.addEventListener("change", CargarNivel5);

                dropLists.push(controlDropListNivel2);
                dropLists.push(controlDropListNivel3);
                dropLists.push(controlDropListNivel4);
                dropLists.push(controlDropListNivel5);
            }

            function CargarNivel2() {
                $.ajax({
                    type: "POST",
                    url: "MayorF.aspx/ObtenerJSON_ListaDeCuentasContablesByJerarquiaAndNivel",
                    data: '{jerarquia: "' + 2 + '", nivel: "' + parseInt(controlDropListNivel1.value) + '"}',
                    contentType: "application/json",
                    dataType: 'json',
                    error: function () {
                        alert("No se pudo cargar el nivel 2.");
                    },
                    success: OnSuccessCargarNivel2
                });
            };

            function OnSuccessCargarNivel2(response) {
                while (controlDropListNivel2.options.length > 0) {
                    controlDropListNivel2.remove(0);
                }

                var data = response.d;
                obj = JSON.parse(data);

                for (i = 0; i < obj.length; i++) {
                    option = document.createElement('option');
                    option.value = obj[i].id;
                    option.text = obj[i].nombre;

                    controlDropListNivel2.add(option);
                }
                if (controlDropListNivel1.value == 0) {
                    return;
                }
                CargarNivel3();
            }

            function CargarNivel3() {
                $.ajax({
                    type: "POST",
                    url: "MayorF.aspx/ObtenerJSON_ListaDeCuentasContablesByJerarquiaAndNivel",
                    data: '{jerarquia: "' + 3 + '", nivel: "' + parseInt(controlDropListNivel2.value) + '"}',
                    contentType: "application/json",
                    dataType: 'json',
                    error: function () {
                        alert("No se pudo cargar el nivel 3.");
                    },
                    success: OnSuccessCargarNivel3
                });
            };

            function OnSuccessCargarNivel3(response) {
                while (controlDropListNivel3.options.length > 0) {
                    controlDropListNivel3.remove(0);
                }

                var data = response.d;
                obj = JSON.parse(data);

                for (i = 0; i < obj.length; i++) {
                    option = document.createElement('option');
                    option.value = obj[i].id;
                    option.text = obj[i].nombre;

                    controlDropListNivel3.add(option);
                }
                if (controlDropListNivel1.value == 0) {
                    return;
                }
                CargarNivel4();
            }

            function CargarNivel4() {
                $.ajax({
                    type: "POST",
                    url: "MayorF.aspx/ObtenerJSON_ListaDeCuentasContablesByJerarquiaAndNivel",
                    data: '{jerarquia: "' + 4 + '", nivel: "' + parseInt(controlDropListNivel3.value) + '"}',
                    contentType: "application/json",
                    dataType: 'json',
                    error: function () {
                        alert("No se pudo cargar el nivel 4.");
                    },
                    success: OnSuccessCargarNivel4
                });
            }

            function OnSuccessCargarNivel4(response) {
                while (controlDropListNivel4.options.length > 0) {
                    controlDropListNivel4.remove(0);
                }

                var data = response.d;
                obj = JSON.parse(data);

                for (i = 0; i < obj.length; i++) {
                    option = document.createElement('option');
                    option.value = obj[i].id;
                    option.text = obj[i].nombre;

                    controlDropListNivel4.add(option);
                }
                CargarNivel5();
            }

            function CargarNivel5() {
                $.ajax({
                    type: "POST",
                    url: "MayorF.aspx/ObtenerJSON_ListaDeCuentasContablesByJerarquiaAndNivel",
                    data: '{jerarquia: "' + 5 + '", nivel: "' + parseInt(controlDropListNivel4.value) + '"}',
                    contentType: "application/json",
                    dataType: 'json',
                    error: function () {
                        alert("No se pudo cargar el nivel 5.");
                    },
                    success: OnSuccessCargarNivel5
                });
            }

            function OnSuccessCargarNivel5(response) {
                while (controlDropListNivel5.options.length > 0) {
                    controlDropListNivel5.remove(0);
                }

                var data = response.d;
                obj = JSON.parse(data);

                for (i = 0; i < obj.length; i++) {
                    option = document.createElement('option');
                    option.value = obj[i].id;
                    option.text = obj[i].nombre;

                    controlDropListNivel5.add(option);
                }
            }

            function AgregarOModificarCuentaContableCaja() {
                var GET = {};
                var query = window.location.search.substring(1).split("&");
                for (var i = 0, max = query.length; i < max; i++) {
                    if (query[i] === "") // check for trailing & with no param
                        continue;

                    var param = query[i].split("=");
                    GET[decodeURIComponent(param[0])] = decodeURIComponent(param[1] || "");
                }
                var queryString_valor = GET.valor;
                var queryString_idCuentaContable_Caja = GET.id;

                if (queryString_idCuentaContable_Caja == undefined)
                    queryString_idCuentaContable_Caja = 0;

                if (queryString_valor == undefined)
                    queryString_valor = 0;

                var idMov = parseInt(controlDropListNivel5.value);
                $.ajax({
                    type: "POST",
                    url: "MovimientoCajaF.aspx/AgregarOModificarMovimiento",
                    data: '{queryString_idMovimiento_Caja: "' + parseInt(queryString_idCuentaContable_Caja) + '", queryString_valor: "' + parseInt(queryString_valor) +
                        '", textDescripcionDelMovimiento: "' + controlTxtDescripcion.value + '", valorDropListTipo_Debe_Haber: "' + parseInt(controlDropListTipo_Debe_Haber.value) +
                        '", idCuentaContable_Nivel5: "' + parseInt(controlDropListNivel5.value) + '"}',
                    contentType: "application/json",
                    dataType: 'json',
                    error: function () {
                        alert("No se pudo eliminar el registro");
                    },
                    success: OnSuccess_AgregarOModificarCuentaContableCaja
                });
            }

            function OnSuccess_AgregarOModificarCuentaContableCaja(response) {
                var data = response.d;
                obj = JSON.parse(data);

                alert(obj);
            }

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
                    else { return false; }
                }
                return true;
            }
        </script>

    </div>
</asp:Content>


