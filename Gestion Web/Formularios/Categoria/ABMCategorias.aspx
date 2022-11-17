<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ABMCategorias.aspx.cs" Inherits="Gestion_Web.Formularios.Categoria.ABMCategorias" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">

        <%--<div class="container">--%><div>


            <div class="col-md-12 col-xs-12">
                <div class="widget stacked">
                    <div class="stat">
                        <h5 id="hImpagas"><i class="icon-map-marker"></i>Categorías</h5>
                    </div>

                    <div class="widget-header">
                        <i class="icon-wrench"></i>
                        <h3>Herramientas</h3>
                    </div>

                    <div class="widget-content">
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 5%"></td>

                                <td style="width: 90%">
                                    <%--agregar un buscador--%>
                                    <input id="filter" type="text" class="form-control" placeholder="Buscar categoria" />
                                </td>

                                <td style="width: 5%">
                                    <div class="shortcuts" style="height: 100%">

                                        <a class="btn btn-primary" onclick="abrirModalAgregar()">
                                            <i class="fa icon-plus"></i>
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
                <div class="widget widget-table">

                    <div class="widget-header">
                        <i class="icon-th-list" style="width: 2%"></i>
                        <h3 style="width: 75%">Categorías
                        </h3>
                    </div>

                    <div class="widget-content">
                        <div class="table-responsive" style="margin: 25px;">
                            <table class="table table-striped table-bordered table-hover dataTable no-footer" id="tableCategoria">
                                <thead>
                                    <tr>
                                        <th>Nombre</th>
                                        <th>Alerta</th>
                                        <th></th>
                                    </tr>
                                </thead>
                                <tbody>
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <%-- widget content --%>
                </div>
            </div>

            <div id="modalAddCategoria" class="modal fade" tabindex="-1" role="dialog">
                <div class="modal-dialog">
                    <div class="modal-content">

                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h4 class="modal-title">Agregar</h4>
                        </div>
                        <div class="modal-body">

                            <div class="form-horizontal col-md-12">
                                <div class="form-group">
                                    <label class="col-md-4">Categoría</label>
                                    <div class="col-md-6">
                                        <input type="text" id="name" class="form-control">
                                    </div>
                                    <div>
                                        <p class="text-danger hidden" id="validName">*</p>
                                    </div>
                                </div>
                            </div>

                            <div class="form-horizontal col-md-12">
                                <div class="form-group">
                                    <label class="col-md-4">Alerta</label>
                                    <div class="col-md-6">
                                        <select id="alertAddCateg" class="form-control"></select>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <div class="modal-footer">
                            <a id="lbtnAgregar" class="btn btn-success" onclick="btnAgregar_Click()">
                                <span class='shortcut-icon icon-ok'></span>
                            </a>
                        </div>
                    </div>
                </div>
            </div>

            <div id="modalModifCategoria" class="modal fade" tabindex="-1" role="dialog">
                <div class="modal-dialog">
                    <div class="modal-content">

                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h4 class="modal-title">Modificar</h4>
                        </div>
                        <div class="modal-body">

                            <div class="form-horizontal col-md-12">
                                <input type="hidden" id="idCatHide" />
                                <div class="form-group">
                                    <label class="col-md-4">Categoría</label>
                                    <div class="col-md-6">
                                        <input type="text" id="nameCateg" class="form-control">
                                    </div>
                                    <div>
                                        <p class="text-danger hidden" id="validNameCateg">*</p>
                                    </div>
                                </div>
                            </div>

                            <div class="form-horizontal col-md-12">
                                <div class="form-group">
                                    <label class="col-md-4">Alerta</label>
                                    <div class="col-md-6">
                                        <select id="alertEditCateg" class="form-control"></select>
                                    </div>
                                </div>
                            </div>

                        </div>
                        <div class="modal-footer">
                            <a id="lbtnModifcar" class="btn btn-success" onclick="btnModificar_Click()">
                                <span class='shortcut-icon icon-ok'></span>
                            </a>
                        </div>
                    </div>
                </div>
            </div>

            <div id="modalConfirmacion" class="modal" tabindex="-1" role="dialog">
                <div class="modal-dialog modal-sm">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal"><span aria-hidden="true">&times;</span><span class="sr-only">Close</span></button>
                            <h4 class="modal-title">Eliminar categoría</h4>
                        </div>
                        <div class="modal-body">
                            <p>
                                Esta seguro que lo desea eliminar?
                            </p>
                        </div>
                        <div class="modal-footer">
                            <asp:Button runat="server" ID="btnEliminar" Text="Eliminar" class="buttonLoading btn btn-danger" OnClientClick="EliminarCategoria(event)" />
                            <button type="button" class="btn btn-white" data-dismiss="modal">Cancelar</button>
                            <asp:HiddenField ID="hiddenID" runat="server" />
                        </div>
                    </div>
                </div>
            </div>

        </div>

        <script src="../../Scripts/libs/jquery-1.9.1.min.js"></script>
        <script src="../../Scripts/libs/jquery-ui-1.10.0.custom.min.js"></script>

        <%--Tiene que estar los dos para que funcione los modals y el menu--%>
        <script src="../../Scripts/bootstrap.min.js"></script>
        <script src="../../Scripts/libs/bootstrap.min.js"></script>

        <%--<script src="../../Scripts/bootstrap.min.js"></script>--%>
        <script src="../../Scripts/plugins/msgbox/jquery.msgbox.min.js"></script>
        <script src="//cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>
        <link href="//cdn.datatables.net/1.10.2/css/jquery.dataTables.css" rel="stylesheet" />

        <script>
            $(document).ready(function () {
                $('#tableCategoria').dataTable({})
                //Activamos el tooltip
                $('[data-toggle=tooltip]').tooltip();

                GetCategorias()
                GetAlertas()
            })

            function GetCategorias() {
                $.ajax({
                    method: "POST",
                    url: "ABMCategorias.aspx/getCategorias",
                    data: "{}",
                    contentType: "application/json",
                    dataType: 'json',
                    error: (error) => {
                        console.log(JSON.stringify(error));
                    },
                    success: function (resp) {
                        console.log(resp.d);
                        if (resp.d != null && resp.d != '') {
                            let jsonNotif = JSON.parse(resp.d)

                            console.log(jsonNotif);

                            let stringifyData = '['

                            for (var x = 0; x < jsonNotif.length; x++) {
                                console.log(jsonNotif[x]);

                                let data = {
                                    "nombre": `<td>${jsonNotif[x].nombre}</td>`,
                                    "alerta": `<td>${jsonNotif[x].descripcion}</td>`,
                                    "Accion": `<td style="text-align:center;">
                                        <a onclick="abrirModifCateg(${jsonNotif[x].id},'${jsonNotif[x].nombre}',${jsonNotif[x].alerta})" class="btn btn-success" data-toggle="tooltip" title data-original-title="Editar">
                                            <i class="fa fa-pencil"></i>
                                        </a>
                                        <a onclick="cargarModalConfirmacion(${jsonNotif[x].id})" class="btn btn-danger" data-toggle="tooltip" title data-original-title="Eliminar">
                                            <i class="fa fa-trash"></i>
                                        </a>
                                    </td>`

                                }
                                stringifyData += JSON.stringify(data) + ','

                            }

                            stringifyData = stringifyData.slice(0, -1) + ']'
                            let jsonData = JSON.parse(stringifyData)

                            $('#tableCategoria').DataTable({
                                "bLengthChange": false,
                                "bFilter": true,
                                "bInfo": false,
                                destroy: true,
                                data: jsonData,
                                columns: [
                                    { data: 'nombre' },
                                    { data: 'alerta' },
                                    { data: 'Accion' }
                                ]
                            });
                            $('.dataTables_filter').hide();
                            $('#filter').on('keyup', function () {
                                $('#tableCategoria').DataTable().search(
                                    this.value
                                ).draw();
                            });
                        }
                    }
                });
            }

            function GetAlertas() {
                $.ajax({
                    method: "POST",
                    url: "ABMCategorias.aspx/getAlertas",
                    data: "{}",
                    contentType: "application/json",
                    dataType: 'json',
                    error: (error) => {
                        console.log(JSON.stringify(error));
                    },
                    success: function (resp) {
                        console.log(resp.d);
                        let alertAddCateg = ''
                        let alertEditCateg = ''

                        if (resp.d != null && resp.d != '') {
                            let alerts = JSON.parse(resp.d)
                            alerts.forEach(element => {

                                alertAddCateg += `<option value="${element.id}">${element.nombre}</option>`
                                alertEditCateg += `<option value="${element.id}">${element.nombre}</option>`
                            })

                            document.getElementById('alertAddCateg').innerHTML = alertAddCateg
                            document.getElementById('alertEditCateg').innerHTML = alertEditCateg
                        }
                    }
                });
            }

            function abrirModalAgregar() {
                $('#modalAddCategoria').modal('show')
            }

            function btnAgregar_Click() {
                let name = document.getElementById('name').value
                let alert = document.getElementById('alertAddCateg').value
                document.getElementById('validName').className = 'text-danger hidden'

                if (name != '') {

                    $.ajax({
                        method: "POST",
                        url: "ABMCategorias.aspx/addCategorias",
                        data: "{nombre: '" + name + "', alerta: '"+ alert +"'}",
                        contentType: "application/json",
                        dataType: 'json',
                        error: (error) => {
                            console.log(JSON.stringify(error));
                        },
                        success: function (resp) {
                            $.msgbox("Operacion realizada con exito!");
                            $('#modalAddCategoria').modal('hide')
                            GetCategorias()
                        }
                    })
                } else {
                    document.getElementById('validName').className= 'text-danger'
                }
            }

            function abrirModifCateg(idCategoria, nombre,alerta) {
                document.getElementById('idCatHide').value = idCategoria
                document.getElementById('nameCateg').value = nombre
                document.getElementById('alertEditCateg').value = alerta

                $('#modalModifCategoria').modal('show')
            }

            function btnModificar_Click() {
                let idCategoria = document.getElementById('idCatHide').value
                let name = document.getElementById('nameCateg').value
                let alert = document.getElementById('alertEditCateg').value
                document.getElementById('validNameCateg').className='text-danger hidden'

                if (name != '') {
                    $.ajax({
                        method: "POST",
                        url: "ABMCategorias.aspx/modifCategorias",
                        data: "{id: '" + idCategoria + "',nombre: '" + name + "',alerta: '"+ alert +"'}",
                        contentType: "application/json",
                        dataType: 'json',
                        error: (error) => {
                            console.log(JSON.stringify(error));
                        },
                        success: function (resp) {
                            $.msgbox("Operacion realizada con exito!");
                            $('#modalModifCategoria').modal('hide')
                            GetCategorias()
                        }
                    })
                } else {
                    document.getElementById('validNameCateg').className = 'text-danger'
                }
            }

            function cargarModalConfirmacion(id) {
                document.getElementById('<%=hiddenID.ClientID%>').value = id
                $('#modalConfirmacion').modal('show')
            }

            function EliminarCategoria(e) {
                e.preventDefault()
                let id = document.getElementById('<%=hiddenID.ClientID%>').value
                $.ajax({
                    type: "POST",
                    url: "ABMCategorias.aspx/EliminarCategorias",
                    data: "{id:'" + id + "',estado:'0'}",
                    contentType: "application/json",
                    dataType: 'json',
                    error: (error) => {
                        console.log(JSON.stringify(error));
                    },
                    success: (respuesta) => {
                        $('#modalConfirmacion').modal('hide')
                        if (respuesta.d == 0) {
                            $.msgbox("Se elimino la categoria correctamente!", {
                                type: "confirm",
                                buttons: [
                                    { type: "submit", value: "Aceptar" }
                                ]
                            });
                        } else if (respuesta.d > 0) {
                            $.msgbox(`Tiene <strong class="text-danger">${respuesta.d}</strong> articulo/s asignado/s a esta categoría.<br> No es posible eliminarla.`, { type: "error" });
                        } else {
                            $.msgbox("Error al eliminar la categoría!.", { type: "error" });
                        }
                        GetCategorias()
                    }
                });
            }
        </script>
</asp:Content>
