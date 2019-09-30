<%@ Page Title="" Language="C#" enableEventValidation="false" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Cuentas_Contables_Mayor.aspx.cs" Inherits="Gestion_Web.Formularios.PlanCuentas.Cuentas_Contables_Mayor" %>

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
                        <td style="width: 100%">
                            <div class="col-md-3">
                                <label>Tipo Movimiento</label>
                                <asp:DropDownList ID="dropList_Mayor_TipoDeMovimiento" runat="server" class="form-control"></asp:DropDownList>
                            </div>
                         <%--   <div class="col-md-3">
                                <label>Debe / Haber</label>
                                <asp:DropDownList ID="DropDownList1" runat="server" class="form-control">
                                    <asp:ListItem Text="Debe" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Haber" Value="2"></asp:ListItem>
                                </asp:DropDownList>
                            </div>--%>
                            <div class="col-md-3">
                                <br />
                                <a href="#modal_DropList_Cuentas" data-toggle="modal" class="btn btn-success">Plan de ctas.</a>
                            </div>
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
                        <div class="form-group">
                            <label class="col-md-4">Nivel 5</label>
                            <div class="col-md-6">
                                <asp:DropDownList ID="DropListNivel5" runat="server" class="form-control"></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbtnCrearRegistro" OnClientClick="javascript:return CrearRegistroCuentaContable();" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" AutoPostBack="false" />
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
        var controlDropListNivel5;
        var controlDropList_Mayor_TipoDeMovimiento;
        var dropLists = [];

        var controlTxtItem;
        var controlTxtCuenta;
        var controlLabelErrorTxtItem;
        var controlLabelErrorTxtCuenta;
        var dropListsNiveles;

        function pageLoad() {
            controlDropListNivel1 = document.getElementById('<%= DropListNivel1.ClientID %>');
            controlDropListNivel2 = document.getElementById('<%= DropListNivel2.ClientID %>');
            controlDropListNivel3 = document.getElementById('<%= DropListNivel3.ClientID %>');
            controlDropListNivel4 = document.getElementById('<%= DropListNivel4.ClientID %>');
            controlDropListNivel5 = document.getElementById('<%= DropListNivel5.ClientID %>');

            controlDropList_Mayor_TipoDeMovimiento = document.getElementById('<%= dropList_Mayor_TipoDeMovimiento.ClientID %>');

            controlDropListNivel1.addEventListener("change", CargarNivel2);
            controlDropListNivel2.addEventListener("change", CargarNivel3);
            controlDropListNivel3.addEventListener("change", CargarNivel4);
            controlDropListNivel4.addEventListener("change", CargarNivel5);

            CargarTablaConLosRegistros();

            dropListsNiveles = [];
            dropListsNiveles.push(controlDropListNivel1);
            dropListsNiveles.push(controlDropListNivel2);
            dropListsNiveles.push(controlDropListNivel3);
            dropListsNiveles.push(controlDropListNivel4);
            dropListsNiveles.push(controlDropListNivel5);
        }

        function CrearRegistroCuentaContable() {

            $.ajax({
                type: "POST",
                url: "Cuentas_Contables_Mayor.aspx/CrearRegistro_CuentaContable_MayorTipoDeMovimiento",
                data: '{nivel5: "' + parseInt(controlDropListNivel5.value) + '", idTipoMovimiento: "' + parseInt(controlDropList_Mayor_TipoDeMovimiento.value) + '"}',
                contentType: "application/json",
                dataType: 'json',
                error: function () {
                    alert("No se pudo crear el registro.");
                },
                success: OnSuccess_CrearRegistroCuentaContable
            });
        }

        function OnSuccess_CrearRegistroCuentaContable(response) {
            var data = response.d;
            obj = JSON.parse(data);

            alert(data);
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
            CargarNivel5();
        }

        function CargarNivel5() {
            $.ajax({
                type: "POST",
                url: "Cuentas_Contables_Mayor.aspx/ObtenerJSON_ListaDeCuentasContablesByJerarquiaAndNivel",
                data: '{jerarquia: "' + 5 + '", nivel: "' + parseInt(controlDropListNivel4.value) + '"}',
                contentType: "application/json",
                dataType: 'json',
                error: function () {
                    alert("No se pudo cargar el nivel 4.");
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

        function ValidateForm() {
            var correcto = true;
            
            for (var i in dropListsNiveles) {
                if (dropListsNiveles[i].value == 0) {
                    correcto = false;
                    alert('Debe seleccionar todos los niveles');
                }
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
                    '<td> <a "id = ' + obj[i].Id + ' class= "btn btn-danger" autopostback="false" onclick="EliminarRegistroDeTabla(' + obj[i].Id + ')"><span class="shortcut-icon icon-trash"></span></a></td>' +
                    "</tr> ");
            };
        }

        function EliminarRegistroDeTabla(idAsiento) {
            $.ajax({
                type: "POST",
                url: "Cuentas_Contables_Mayor.aspx/EliminarRegistroDeTabla",
                data: '{idCuentasContable_MayorTipoMovimiento: "' + parseInt(idAsiento) + '"}',
                contentType: "application/json",
                dataType: 'json',
                error: function () {
                    alert("No se pudo eliminar el registro");
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
