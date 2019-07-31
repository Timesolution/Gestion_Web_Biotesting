<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Cuentas_Contables_Mayor.aspx.cs" Inherits="Gestion_Web.Formularios.PlanCuentas.Cuentas_Contables_Mayor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="col-md-12 col-xs-12">
        <div class="widget stacked">
            <div class="stat">
                <h5><i class="icon-map-marker"></i>Maestros > Cuentas > Tipos</h5>
            </div>

            <div class="widget-header">
                <i class="icon-wrench"></i>
                <h3>Herramientas</h3>
            </div>

            <div class="widget-content">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 30%">
                            <label class="col-md-2">Tipo Movimiento</label>
                            <div class="col-md-3">
                                <div class="row">
                                    <asp:DropDownList ID="dropList_Mayor_TipoDeMovimiento" runat="server" class="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            &nbsp
                            <a href="#modal_DropList_Cuentas" data-toggle="modal" class="btn btn-success">Plan de ctas.</a>
                        </td>
                    </tr>
                </table>
            </div>
        </div>

        <div class="widget stacked widget-table action-table">

            <div class="widget-header">
                <i class="icon-th-list"></i>
                <h3>Asientos</h3>
            </div>
            <div class="widget-content">
                <div class="panel-body">
                    <div class="table-responsive">
                        <table class="table table-striped table-bordered table-hover" id="tabla">
                            <thead>
                                <tr>
                                    <th>Tipo Movimiento</th>
                                    <th>Cuenta</th>
                                    <th></th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:PlaceHolder ID="phCuentasContables_MayorTipoMovimiento" runat="server"></asp:PlaceHolder>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="modal_DropList_Cuentas" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" id="btnCerrarModalBusqueda" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Busqueda</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <div class="form-group">
                            <label class="col-md-4">Nivel 1</label>
                            <div class="col-md-6">
                                <asp:DropDownList ID="DropListNivel1" runat="server" class="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-4">Nivel 2</label>
                            <div class="col-md-6">
                                <asp:DropDownList ID="DropListNivel2" runat="server" class="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-4">Nivel 3</label>
                            <div class="col-md-6">
                                <asp:DropDownList ID="DropListNivel3" runat="server" class="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-4">Nivel 4</label>
                            <div class="col-md-6">
                                <asp:DropDownList ID="DropListNivel4" runat="server" class="form-control"></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbtnCrearRegistro" OnClick="lbtnCrearRegistro_Click" OnClientClick="javascript:return ValidarFormulario_ModalCrearRegistro()" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" AutoPostBack="false" />
                </div>
            </div>
        </div>
    </div>

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
        var dropLists = [];

        var controlTxtItem;
        var controlTxtCuenta;
        var controlLabelErrorTxtItem;
        var controlLabelErrorTxtCuenta;

        function pageLoad() {
            controlDropListNivel1 = document.getElementById('<%= DropListNivel1.ClientID %>');
            controlDropListNivel2 = document.getElementById('<%= DropListNivel2.ClientID %>');
            controlDropListNivel3 = document.getElementById('<%= DropListNivel3.ClientID %>');
            controlDropListNivel4 = document.getElementById('<%= DropListNivel4.ClientID %>');

            controlDropListNivel1.addEventListener("change", ChangeNivel1);
            controlDropListNivel2.addEventListener("change", CargarNivel3);
            controlDropListNivel3.addEventListener("change", CargarNivel4);

            CargarTablaConLosRegistros();
        }

        function CargarDropListNivel1PageLoad() {
            $.ajax({
                type: "POST",
                url: "Cuentas_Contables_Mayor.aspx/ObtenerJSON_ListaDeCuentasContablesByJerarquiaAndNivel",
                data: '{jerarquia: "' + 1 + '", nivel: 0}',
                contentType: "application/json",
                dataType: 'json',
                error: function () {
                    alert("No se pudo cargar el nivel 1.");
                },
                success: OnSuccessCargarNivel1
            });
        }

        function OnSuccessCargarNivel1(response) {
            controlDropListNivel1 = document.getElementById('<%= DropListNivel1.ClientID %>');
            while (controlDropListNivel1.options.length > 0) {
                controlDropListNivel1.remove(0);
            }

            var data = response.d;
            obj = JSON.parse(data);

            for (i = 0; i < obj.length; i++) {
                option = document.createElement('option');
                option.value = obj[i].id;
                option.text = obj[i].nombre;

                controlDropListNivel1.add(option);
            }
            CargarNivel2();
        }

        function ChangeNivel1() {
            BorrarLosDropListDeNiveles();
            CargarNivel2();
        }

        function BorrarLosDropListDeNiveles() {

            for (var i in dropLists) {
                while (dropLists[i].options.length > 0) {
                    dropLists[i].remove(0);
                }
            }
        }

        function CargarNivel2() {
            $.ajax({
                type: "POST",
                url: "Cuentas_Contables_Mayor.aspx/ObtenerJSON_ListaDeCuentasContablesByJerarquiaAndNivel",
                data: '{jerarquia: "' + 2 + '", nivel: "' + parseInt(controlDropListNivel1.value) + '"}',
                contentType: "application/json",
                dataType: 'json',
                error: function () {
                    alert("No se pudo cargar el nivel 2.");
                },
                success: OnSuccessCargarNivel2
            });
        }

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
                url: "Cuentas_Contables_Mayor.aspx/ObtenerJSON_ListaDeCuentasContablesByJerarquiaAndNivel",
                data: '{jerarquia: "' + 3 + '", nivel: "' + parseInt(controlDropListNivel2.value) + '"}',
                contentType: "application/json",
                dataType: 'json',
                error: function () {
                    alert("No se pudo cargar el nivel 3.");
                },
                success: OnSuccessCargarNivel3
            });
        }

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
                url: "Cuentas_Contables_Mayor.aspx/ObtenerJSON_ListaDeCuentasContablesByJerarquiaAndNivel",
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
        }

        function ValidateForm() {
            var correcto = true;
            if (controlTxtItem.value.length == 0) {
                controlLabelErrorTxtItem.style.display = "block";
                correcto = false;
            }
            if (controlTxtCuenta.value.length == 0) {
                controlLabelErrorTxtCuenta.style.display = "block";
                correcto = false;
            }
            if (correcto) {
                return true;
            }
            return false
        }

        function CargarTablaConLosRegistros() {
            $.ajax({
                type: "POST",
                url: "Cuentas_Contables_Mayor.aspx/TraerRegistrosDe_CuentasContables_MayorTipoMovimiento",
                contentType: "application/json",
                dataType: 'json',
                error: (error) => {
                    console.log(JSON.stringify(error));
                    $.msgbox("No se pudo cargar la tabla", { type: "error" });
                }
                ,
                success: OnSuccessCargarTabla
            });
        }

        function OnSuccessCargarTabla(response) {
            var data = response.d;
            var obj = JSON.parse(data);

            document.getElementById('btnCerrarModalBusqueda').click();
            $('#tabla').find("tr:gt(0)").remove();

            for (var i = 0; i < obj.length; i++) {
                $('#tabla').append(
                    "<tr>" +
                    "<td> " + obj[i].IdMayor_TipoMovimiento + "</td>" +
                    "<td> " + obj[i].IdCuenta_Contable + "</td>" +
                    //'<td> <a "id = ' + obj[i].Id + ' class= "btn btn-danger" autopostback="false" onclick="EliminarRegistroDeTabla(' + obj[i].Id + ')"><span class="shortcut-icon icon-trash"></span></a></td>' +
                    "</tr> ");
            };
        }

        function EliminarRegistroDeTabla(idAsiento) {
            $.ajax({
                type: "POST",
                url: "Cuentas_Contables_Mayor.aspx/EliminarRegistroDeTabla",
                data: '{idAsiento: "' + parseInt(idAsiento) + '"}',
                contentType: "application/json",
                dataType: 'json',
                error: function () {
                    alert("No se pudo borrar el asiento");
                },
                success: OnSuccessEliminarRegistroDeTabla
            });
        }

        function OnSuccessEliminarRegistroDeTabla(response) {
            var data = response.d;
            obj = JSON.parse(data);

            if (obj == "1") {
                alert("Eliminado con exito");
            }
            CargarTablaConLosRegistros();
        }
    </script>


</asp:Content>
