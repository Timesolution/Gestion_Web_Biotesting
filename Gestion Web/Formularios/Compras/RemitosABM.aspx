<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RemitosABM.aspx.cs" Inherits="Gestion_Web.Formularios.Compras.RemitosABM" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">

        <div>
            <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                <ContentTemplate>
                    <div class="col-md-12 col-xs-12">
                        <div class="widget stacked">

                            <div class="widget-header">
                                <i class="icon-wrench"></i>
                                <h3>Herramientas</h3>
                            </div>
                            <!-- /widget-header -->

                            <div class="widget-content">
                                <div id="validation-form" role="form" class="form-horizontal col-md-7">
                                    <fieldset>
                                        <div class="form-group">
                                            <label for="name" class="col-md-3">Sucursal</label>
                                            <div class="col-md-6">
                                                <asp:DropDownList ID="ListSucursal" class="form-control" disabled runat="server" AutoPostBack="True"></asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="name" class="col-md-3">Proveedor</label>
                                            <div class="col-md-6">
                                                <asp:DropDownList ID="ListProveedor" class="form-control" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ListProveedor_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="name" class="col-md-3">Numero</label>

                                            <div class="col-md-2">
                                                <asp:TextBox ID="txtPVenta" MaxLength="4" runat="server" class="form-control" onchange="completar4Ceros(this, this.value)"></asp:TextBox>
                                            </div>

                                            <div class="col-md-4">
                                                <asp:TextBox ID="txtNumero" MaxLength="8" runat="server" class="form-control" onchange="completar8Ceros(this, this.value)"></asp:TextBox>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:RequiredFieldValidator ControlToValidate="txtPVenta" ID="RequiredFieldValidator30" runat="server" ErrorMessage="*" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>

                                            </div>
                                            <div class="col-md-1">
                                                <asp:RequiredFieldValidator ControlToValidate="txtNumero" ID="RequiredFieldValidator6" runat="server" ErrorMessage="*" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>

                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-3">Tipo remito</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="ListTipoRemito" runat="server" class="form-control">
                                                    <asp:ListItem Text="Seleccione..." Value="-1" />
                                                    <asp:ListItem Text="FC" Value="1" />
                                                    <asp:ListItem Text="PRP" Value="2" />
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:RequiredFieldValidator ControlToValidate="ListTipoRemito" ID="RequiredFieldValidator1" InitialValue="-1" runat="server" ErrorMessage="*" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>

                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-3">Tipo compra</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="ListDevolucion" runat="server" class="form-control">
                                                    <asp:ListItem Text="Seleccione..." Value="-1" />
                                                    <asp:ListItem Text="Compra Mercaderia" Value="0" />
                                                    <asp:ListItem Text="Devolucion Mercaderia" Value="1" />
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:RequiredFieldValidator ControlToValidate="ListDevolucion" ID="RequiredFieldValidator2" InitialValue="-1" runat="server" ErrorMessage="*" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>

                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-3">Fecha</label>

                                            <div class="col-md-4">
                                                <div class="input-group">
                                                    <span class="input-group-addon"><i class="icon-calendar"></i></span>
                                                    <asp:TextBox ID="txtFecha" runat="server" class="form-control"></asp:TextBox>
                                                </div>

                                            </div>
                                            <div class="col-md-4">
                                                <asp:RequiredFieldValidator ControlToValidate="txtFecha" ID="RequiredFieldValidator42" runat="server" ErrorMessage="Seleccione Fecha" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>

                                            </div>
                                        </div>
                                        <asp:Panel ID="panelDespacho" runat="server" Visible="false">                                                    
                                            <div class="form-group">
                                                <label for="name" class="col-md-3">Fecha despacho</label>
                                                <div class="col-md-4">
                                                    <div class="input-group">
                                                        <span class="input-group-addon"><i class="icon-calendar"></i></span>
                                                        <asp:TextBox ID="txtFechaDespacho" runat="server" class="form-control"></asp:TextBox>
                                                    </div>                                                    
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="name" class="col-md-3">Numero despacho</label>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="txtNumeroDespacho" runat="server" class="form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="name" class="col-md-3">Numero Lote</label>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="txtLote" runat="server" class="form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="name" class="col-md-3">Fecha vencimiento</label>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="txtVencimiento" runat="server" class="form-control"></asp:TextBox>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                        <asp:Panel ID="Panel1" Visible="false" runat="server">
                                            <table class="table table-striped table-bordered col-md-10">
                                                <thead>
                                                    <tr>
                                                        <th>Codigo</th>
                                                        <th>Costo</th>
                                                        <th>Cantidad</th>
                                                        <th></th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr>
                                                        <td style="width: 20%">
                                                            <div class="form-group">
                                                                <div class="col-md-12">
                                                                    <div class="input-group">
                                                                        <asp:TextBox ID="txtCodigo" runat="server" class="form-control"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </td>
                                                        <td style="width: 70%">

                                                            <asp:TextBox ID="txtCantidad" runat="server" class="form-control" TextMode="Number" Text="0" AutoPostBack="True" Style="text-align: right"></asp:TextBox>

                                                        </td>

                                                        <td style="width: 10%">
                                                            <asp:LinkButton ID="lbtnAgregarArticuloASP" class="btn btn-info" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" Visible="true" OnClick="lbtnAgregarArticuloASP_Click" />
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </asp:Panel>
                                    </fieldset>
                                </div>
                                <div class="col-md-5">
                                    <div class="form-group">
                                        <label for="name" class="col-md-3">Observacion</label>
                                        <div class="col-md-9">
                                            <asp:TextBox ID="txtComentario" runat="server" class="form-control" TextMode="MultiLine" Rows="5"/>
                                        </div>
                                    </div>
                                </div>

                                <a class="btn btn-info" style="display: none" data-toggle="modal" id="abreDialog" href="#modalTrazabilidad"></a>
                                <table class="table table-striped table-bordered">
                                    <thead>
                                        <tr>
                                            <th>Codigo</th>
                                            <th>Descripcion</th>
                                            <th>Precio</th>
                                            <th>Cantidad</th>
                                            <th>Lote</th>
                                            <th>Vencimiento</th>
                                            <th></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:PlaceHolder ID="phProductos" runat="server"></asp:PlaceHolder>
                                        <%--<div class="table-responsive col-md-12">
                                            <asp:GridView ID="GridProductos" class="table table-striped table-bordered table-hover"
                                                runat="server" AllowPaging="True" OnPageIndexChanging="GridProductos_PageIndexChanging" PageSize="50">
                                                <Columns>
                                                    <asp:BoundField DataField="Codigo" HeaderText="Codigo" />
                                                    <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
                                                    <asp:BoundField DataField="Costo" HeaderText="Costo" DataFormatString="{0:C}" ItemStyle-HorizontalAlign="Right">
                                                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Cantidad">
                                                        <ItemTemplate>
                                                            <asp:TextBox ID="txtGridCantidad" runat="server" Style="text-align: right;" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText=" ">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="btnVerEditar" runat="server" class="btn ui-tooltip" title data-original-title="Ver y/o Editar" CommandArgument='<%# Bind("IdArticulos") %>' OnClick="btnVerEditar_Click">                                                            
                                                            <i class="shortcut-icon icon-search"></i>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>--%>
                                    </tbody>
                                </table>
                                <div class="form-group">
                                    <asp:Button ID="btnAgregar" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnAgregar_Click" />
                                    <asp:Button ID="btnFiltrar" runat="server" Text="Filtrar" Visible="false" class="btn btn-success" OnClick="btnFiltrar_Click" />
                                </div>
                            </div>
                            <!-- /widget-content -->

                        </div>
                        <!-- /widget -->
                    </div>

                </ContentTemplate>

            </asp:UpdatePanel>
        </div>
    </div>

     <div id="modalTrazabilidad" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog" style="width: 80%;">
                <div class="modal-content">
                    <div class="modal-header">
                        <button id="btnCerrarTraza" type="button" class="close" data-dismiss="modal" aria-hidden="true" style="display: none;">×</button>
                        <h4 class="modal-title">Trazabilidad</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <asp:UpdatePanel ID="UpdatePanel6" UpdateMode="Always" runat="server">
                                <ContentTemplate>
                                    <div class="widget big-stats-container stacked">

                                        <div class="widget-content">

                                            <div id="big_stats" class="cf">
                                                <div class="stat">
                                                    <h4>Cantidad</h4>
                                                    <asp:Label ID="lblTrazaActual" runat="server" Text="0" class="value"></asp:Label>
                                                    <label class="value">/</label>
                                                    <asp:Label ID="lblTrazaTotal" runat="server" Text="0" class="value"></asp:Label>
                                                </div>
                                                <!-- .stat -->
                                            </div>

                                        </div>
                                        <!-- /widget-content -->

                                    </div>
                                    <!-- /widget -->
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div role="form" class="form-horizontal col-md-12">
                            <div class="form-group">
                                <asp:UpdatePanel ID="UpdatePanel7" UpdateMode="Always" runat="server">
                                    <ContentTemplate>
                                        <table class="table table-striped table-bordered">
                                            <thead>
                                                <tr>
                                                    <th>#</th>
                                                    <asp:PlaceHolder ID="phCamposTrazabilidad" runat="server"></asp:PlaceHolder>
                                                    <th></th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:PlaceHolder ID="phItemsTrazabilidad" runat="server"></asp:PlaceHolder>
                                            </tbody>
                                        </table>
                                    </ContentTemplate>
                                    <Triggers>
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:UpdatePanel ID="UpdatePanel8" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <asp:Label ID="lblMovTraza" runat="server" Style="display: none;"></asp:Label>
                                <asp:LinkButton ID="AgregarTraza" runat="server" class="btn btn-success" Text="<span class='shortcut-icon icon-ok'></span>" OnClick="AgregarTraza_Click"></asp:LinkButton>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>

                </div>
            </div>
        </div>
    
    <script src="../../Scripts/plugins/dataTables/jquery.dataTables.min.js"></script>
    <script src="../../Scripts/plugins/dataTables/custom.tables.js"></script>


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

    <script src="../../Scripts/JSFunciones1.js"></script>


    <script src="//code.jquery.com/ui/1.10.4/jquery-ui.js"></script>

    <!-- Page-Level Plugin Scripts - Tables -->
    <script src="//cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>
    <script src="../../Scripts/plugins/dataTables/custom.tables.js"></script>
    <link href="//cdn.datatables.net/1.10.2/css/jquery.dataTables.css" rel="stylesheet" />

    <script>
        $(function () {
            $("#<%= txtFecha.ClientID %>").datepicker(
                {
                    dateFormat: 'dd/mm/yy'
                }
                );
        });
    </script>

    <script>
        function cerrarModal() {
            document.getElementById('btnCerrarTraza').click();
        }
    </script>

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endReq);
        function endReq(sender, args) {
            $(function () {
                $("#<%= txtFecha.ClientID %>").datepicker(
                    {
                        dateFormat: 'dd/mm/yy'
                    }
                    );
            });
        }
    </script>

    <script>
        $(document).ready(function () {
            $('#dataTables-example').dataTable({

                "bInfo": false,
                "bAutoWidth": false,
                "language": {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "Mostrar _MENU_ registros",
                    "sZeroRecords": "No se encontraron resultados",
                    "sEmptyTable": "Ningún dato disponible en esta tabla",
                    "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
                    "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
                    "sInfoPostFix": "",
                    "sSearch": "Buscar:",
                    "sUrl": "",
                    "sInfoThousands": ",",
                    "sLoadingRecords": "Cargando...",
                    "oPaginate": {
                        "sFirst": "Primero",
                        "sLast": "Último",
                        "sNext": "Siguiente",
                        "sPrevious": "Anterior"
                    },
                    "oAria": {
                        "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                        "sSortDescending": ": Activar para ordenar la columna de manera descendente"
                    }
                }

            });
        });
    </script>

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endReq);
        function endReq(sender, args) {
            $('#dataTables-example').dataTable({


                "bInfo": false,
                "bAutoWidth": false,

                "language": {
                    "sProcessing": "Procesando...",
                    "sLengthMenu": "Mostrar _MENU_ registros",
                    "sZeroRecords": "No se encontraron resultados",
                    "sEmptyTable": "Ningún dato disponible en esta tabla",
                    "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
                    "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                    "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
                    "sInfoPostFix": "",
                    "sSearch": "Buscar:",
                    "sUrl": "",
                    "sInfoThousands": ",",
                    "sLoadingRecords": "Cargando...",
                    "oPaginate": {
                        "sFirst": "Primero",
                        "sLast": "Último",
                        "sNext": "Siguiente",
                        "sPrevious": "Anterior"
                    },
                    "oAria": {
                        "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
                        "sSortDescending": ": Activar para ordenar la columna de manera descendente"
                    }
                }


            });
        }
    </script>

    <script>
        function abrirdialog() {
            document.getElementById('abreDialog').click();
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
                if (key == 46 || key == 8 || key == 44)// Detectar . (punto) y backspace (retroceso) y , (coma)
                {
                    return true;
                }
                else {
                    return false;
                }
            }
            return true;
        }

        function updatebox(valor, id) {
            var cantActual = document.getElementById("<%=lblTrazaActual.ClientID%>").textContent;
            var cantTotal = document.getElementById("<%=lblTrazaTotal.ClientID%>").textContent;

            var chk1 = document.getElementById(id);
            if (cantActual == cantTotal) {
                if (chk1.checked == false) {
                    cantActual = parseInt(parseInt(cantActual) - 1);
                    document.getElementById('<%= lblTrazaActual.ClientID %>').textContent = cantActual;
                }

                document.getElementById(id).checked = false;
            }
            else {
                if (chk1.checked) {
                    cantActual = parseInt(parseInt(cantActual) + 1);
                }
                else {
                    cantActual = parseInt(parseInt(cantActual) - 1);
                }
                document.getElementById('<%= lblTrazaActual.ClientID %>').textContent = cantActual;
            }

        }

    </script>


</asp:Content>
