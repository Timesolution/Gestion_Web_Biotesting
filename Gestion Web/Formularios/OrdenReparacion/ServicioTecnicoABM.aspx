<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ServicioTecnicoABM.aspx.cs" Inherits="Gestion_Web.Formularios.OrdenReparacion.ServicioTecnicoABM" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%--<div class="main">--%>

    <div class="container">
        <div class="row">
            <div class="col-md-12 col-xs-12">
                <div class="widget stacked">
                    <div class="widget-header">
                        <i class="icon-wrench"></i>
                        <h3>Herramientas</h3>
                    </div>
                    <div class="widget-content">
                        <section id="accordions">
                            <div class="panel-group accordion" id="acordions">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 95%">
                                                    <h4 class="panel-title">
                                                        <a class="accordion-toggle" data-toggle="collapse" data-parent=".accordion" href="#collapseOne">Alta Servicio Tecnico</a>
                                                    </h4>
                                                </td>
                                                <td style="width: 5%">
                                                    <asp:LinkButton ID="lbtnEfectivo" runat="server" Text="<span class='shortcut-icon icon-plus'></span>" class="accordion-toggle" data-toggle="collapse" data-parent=".accordion" href="#collapseOne" Style="color: black" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                    <div id="collapseOne" class="panel-collapse collapse">
                                        <div class="panel-body">
                                            <asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Always" runat="server">
                                                <ContentTemplate>
                                                    <div role="form" class="form-horizontal col-md-10">
                                                        <fieldset>
                                                            <div class="form-group">
                                                                <label for="name" class="col-md-4">Nombre Servicio Tecnico</label>
                                                                <div class="col-md-4">
                                                                    <asp:TextBox ID="txtNombre" runat="server" class="form-control"></asp:TextBox>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <asp:RequiredFieldValidator ControlToValidate="txtNombre" ID="RequiredFieldValidator2" runat="server" ErrorMessage="El campo es obligatorio" SetFocusOnError="true" ForeColor="Red" ValidationGroup="PVGroup" Font-Bold="true"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <label for="name" class="col-md-4">Direccion</label>
                                                                <div class="col-md-4">
                                                                    <asp:TextBox ID="txtDireccion" runat="server" class="form-control"></asp:TextBox>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <asp:RequiredFieldValidator ControlToValidate="txtDireccion" ID="RequiredFieldValidator5" runat="server" ErrorMessage="El campo es obligatorio" SetFocusOnError="true" ForeColor="Red" ValidationGroup="PVGroup" Font-Bold="true"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <label class="col-md-4">Celular</label>
                                                                <div class="col-md-2">
                                                                    <div class="input-group">
                                                                        <span class="input-group-addon">0</span>
                                                                        <asp:TextBox ID="txtCodArea" runat="server" class="form-control" placeholder="Cod. Area" MaxLength="4" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                                    </div>
                                                                </div>
                                                                <div class="col-md-2">
                                                                    <asp:TextBox ID="txtCelular" runat="server" class="form-control" placeholder="Ej.: 1111 2222" MaxLength="8" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <asp:RequiredFieldValidator ControlToValidate="txtCelular" ID="RequiredFieldValidator1" runat="server" ErrorMessage="El campo es obligatorio" SetFocusOnError="true" ForeColor="Red" ValidationGroup="PVGroup" Font-Bold="true"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <label for="name" class="col-md-4">Observaciones</label>
                                                                <div class="col-md-4">
                                                                    <asp:TextBox ID="txtObservaciones" runat="server" class="form-control"></asp:TextBox>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <asp:RequiredFieldValidator ControlToValidate="txtObservaciones" ID="RequiredFieldValidator3" runat="server" ErrorMessage="El campo es obligatorio" SetFocusOnError="true" ForeColor="Red" ValidationGroup="PVGroup" Font-Bold="true"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>

                                                            <asp:UpdatePanel runat="server" ID="updatePanelMarcas" UpdateMode="Always">
                                                                <ContentTemplate>
                                                                    <div class="form-group">
                                                                        <label for="name" class="col-md-4">Descripcion Marca</label>
                                                                        <div class="col-md-4">
                                                                            <asp:TextBox ID="txtDescMarca" class="form-control" runat="server"></asp:TextBox>
                                                                        </div>
                                                                        <div class="col-md-4">
                                                                            <asp:LinkButton ID="btnBuscarMarca" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" OnClick="btnBuscarMarca_Click" />
                                                                        </div>
                                                                    </div>

                                                                    <div class="form-group">
                                                                        <label for="name" class="col-md-4">Marcas</label>
                                                                        <div class="col-md-3">
                                                                            <asp:DropDownList ID="ListMarcas" class="form-control" runat="server">
                                                                            </asp:DropDownList>

                                                                        </div>
                                                                        <div class="col-md-3">
                                                                            <asp:LinkButton ID="btnAgregarMarcas" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnAgregarMarcas_Click" />
                                                                        </div>
                                                                    </div>

                                                                    <div class="form-group">
                                                                        <label for="name" class="col-md-4"></label>

                                                                        <div class="col-md-3">
                                                                            <asp:ListBox ID="ListBoxMarcas" runat="server" SelectionMode="Multiple" class="form-control"></asp:ListBox>
                                                                        </div>
                                                                        <div class="col-md-4">
                                                                            <asp:RequiredFieldValidator ControlToValidate="ListBoxMarcas" ID="RequiredFieldValidator4" runat="server" ErrorMessage="El campo es obligatorio" SetFocusOnError="true" ForeColor="Red" ValidationGroup="PVGroup" Font-Bold="true"></asp:RequiredFieldValidator>
                                                                        </div>
                                                                        <div class="col-md-3">
                                                                            <asp:LinkButton ID="btnQuitarMarca" runat="server" Text="<span class='shortcut-icon icon-trash'></span>" class="btn btn-danger" OnClick="btnQuitarMarca_Click" />
                                                                        </div>
                                                                    </div>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
                                                        </fieldset>
                                                    </div>
                                                    <div class="col-md-8">
                                                        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" class="btn btn-success" ValidationGroup="PVGroup" OnClick="btnGuardar_Click" Visible="false" />
                                                        <asp:Button ID="btnAgregar" runat="server" Text="Agregar" class="btn btn-success" ValidationGroup="PVGroup" OnClick="btnAgregar_Click" Visible="true" />
                                                        <asp:LinkButton ID="lbtnCancelar" runat="server" Text="Cancelar" class="btn btn-default" OnClick="lbtnCancelar_Click"></asp:LinkButton>
                                                    </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </section>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-12 col-xs-12">
            <div class="widget stacked widget-table action-table">

                <div class="widget-header">
                    <i class="icon-th-list"></i>
                    <h3>Servicios Tecnicos</h3>
                </div>
                <div class="widget-content">
                    <div class="panel-body">
                        <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                            <ContentTemplate>
                                <div class="table-responsive">
                                    <table class="table table-striped table-bordered table-hover" id="dataTables-example">
                                        <thead>
                                            <tr>
                                                <th>Nombre</th>
                                                <th>Direccion</th>
                                                <th>Telefono</th>
                                                <th>Observaciones</th>
                                                <th>Marcas</th>
                                                <th></th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:PlaceHolder ID="phServicioTecnico" runat="server"></asp:PlaceHolder>
                                        </tbody>
                                    </table>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%--</div>--%>
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
                                    <asp:Label runat="server" ID="lblMensaje" Text="Esta seguro que desea eliminar el servicio tecnico?" Style="text-align: center"></asp:Label>
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

    <script src="../../Scripts/libs/jquery-1.9.1.min.js"></script>
    <script src="//code.jquery.com/ui/1.11.2/jquery-ui.js"></script>

    <script>
        function abrirdialog(valor) {
            document.getElementById('<%= txtMovimiento.ClientID %>').value = valor;
        }
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
        $(function () {

            $('.accordion').on('show', function (e) {
                $(e.target).prev('.accordion-heading').addClass('accordion-opened');
            });

            $('.accordion').on('hide', function (e) {
                $(this).find('.accordion-heading').not($(e.target)).removeClass('accordion-opened');
            });

        });
    </script>

    <script src="../../Scripts/libs/jquery-ui-1.10.0.custom.min.js"></script>

    <script src="../../Scripts/plugins/msgGrowl/js/msgGrowl.js"></script>
    <script src="../../Scripts/plugins/lightbox/jquery.lightbox.min.js"></script>
    <script src="../../Scripts/plugins/msgbox/jquery.msgbox.min.js"></script>

    <%--<script src="../../Scripts/Application.js"></script>--%>

    <script src="../../Scripts/demo/notifications.js"></script>

</asp:Content>
