<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ABMUsuarios.aspx.cs" Inherits="Gestion_Web.Formularios.Seguridad.ABMUsuarios" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">


        <div class="row">

            <div class="col-md-12">

                <div class="widget stacked">

                    <div class="widget-header">
                        <i class="icon-pencil"></i>
                        <h3>Usuarios</h3>

                    </div>
                    <!-- /.widget-header -->

                    <div class="widget-content">

                        <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Always" runat="server">
                            <ContentTemplate>
                                <div id="validation-form" role="form" class="form-horizontal col-md-10">
                                    <fieldset>
                                        <div class="form-group">
                                            <label for="name" class="col-md-4">Usuario</label>

                                            <div class="col-md-4">

                                                <asp:TextBox ID="txtUsuario" runat="server" class="form-control"></asp:TextBox>

                                                <%--<input type="text" class="form-control" name="name" id="name">--%>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:RequiredFieldValidator ControlToValidate="txtUsuario" ID="RequiredFieldValidator2" runat="server" ErrorMessage="El campo es obligatorio" SetFocusOnError="true" ForeColor="Red" ValidationGroup="PVGroup" Font-Bold="true"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-4">Contraseña</label>

                                            <div class="col-md-4">

                                                <asp:TextBox ID="txtContraseña" runat="server" class="form-control"></asp:TextBox>

                                                <%--<input type="text" class="form-control" name="name" id="name">--%>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:RequiredFieldValidator ControlToValidate="txtContraseña" ID="RequiredFieldValidator5" runat="server" ErrorMessage="El campo es obligatorio" SetFocusOnError="true" ForeColor="Red" ValidationGroup="PVGroup" Font-Bold="true"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4">Empresa</label>
                                            <div class="col-md-4">

                                                <asp:DropDownList ID="DropListEmpresa" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="DropListEmpresa_SelectedIndexChanged"></asp:DropDownList>

                                            </div>

                                            <div class="col-md-4">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="DropListEmpresa" InitialValue="-1" ValidationGroup="PVGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4">Sucursal</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="DropListSucursal" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="DropListSucursal_SelectedIndexChanged"></asp:DropDownList>
                                                <!-- /input-group -->
                                            </div>
                                            <div class="col-md-4">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="DropListSucursal" InitialValue="-1" ValidationGroup="PVGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4">Pto venta</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="DropListPtoVenta" runat="server" class="form-control"></asp:DropDownList>
                                                <!-- /input-group -->
                                            </div>
                                            <div class="col-md-4">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="DropListPtoVenta" InitialValue="-1" ValidationGroup="PVGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4">Perfil</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="DropListPerfil" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="DropListPerfil_SelectedIndexChanged"></asp:DropDownList>

                                                <!-- /input-group -->
                                            </div>

                                            <div class="col-md-4">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="DropListPerfil" InitialValue="-1" ValidationGroup="PVGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <asp:Panel runat="server" ID="panelVendedor" Visible="false">
                                            <div class="form-group">

                                                <div class="col-md-4">
                                                    <asp:Label runat="server" ID="lblVendedor" Text="Vendedor" Font-Bold="true"></asp:Label>
                                                </div>

                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="ListVendedores" runat="server" class="form-control">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-md-1">
                                                    <a class="btn btn-info" data-toggle="modal" href="#modalVendedor" style="display:none;">
                                                        <i class="shortcut-icon icon-plus"></i>
                                                    </a>
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="ListVendedores" InitialValue="0" ValidationGroup="PVGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                         <asp:Panel runat="server" ID="panelClientes" Visible="false">
                                             <div class="form-group">
                                                <label class="col-md-4">Buscar Cliente</label>
                                                <div class="col-md-6">
                                                    <asp:TextBox ID="txtCodCliente" class="form-control" runat="server"></asp:TextBox>
                                                </div>

                                                <div class="col-md-2">
                                                    <asp:LinkButton ID="btnBuscarCod" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" OnClick="btnBuscarCod_Click" />
                                                </div>
                                            </div>
                                            <div class="form-group">

                                                <div class="col-md-4">
                                                    <asp:Label runat="server" ID="Label1" Text="Clientes" Font-Bold="true"></asp:Label>
                                                </div>

                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="ListClientes" runat="server" class="form-control">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-md-1">
                                                    
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="ListClientes" InitialValue="-1" ValidationGroup="PVGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </asp:Panel>

                                        <asp:Panel runat="server" ID="PanelFamilia" Visible="false">
                                             <div class="form-group">
                                                <label class="col-md-4">Buscar Cliente</label>
                                                <div class="col-md-6">
                                                    <asp:TextBox ID="txtCodClienteFamilia" class="form-control" runat="server"></asp:TextBox>
                                                </div>

                                                <div class="col-md-2">
                                                    <asp:LinkButton ID="btnBuscarCodClienteFamilia" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" />
                                                </div>
                                            </div>
                                            <div class="form-group">

                                                <div class="col-md-4">
                                                    <asp:Label runat="server" ID="Label2" Text="Clientes" Font-Bold="true"></asp:Label>
                                                </div>

                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="ListClientesFamilia" runat="server" class="form-control">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-md-1">
                                                    
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="ListClientesFamilia" InitialValue="-1" ValidationGroup="PDGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </fieldset>

                                </div>

                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>

                        <div class="col-md-8">
                            <asp:Button ID="btnAgregar" runat="server" Text="Guardar" class="btn btn-success" ValidationGroup="PVGroup" OnClick="btnAgregar_Click" />
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" class="btn btn-default" PostBackUrl="../Seguridad/UsuariosF.aspx" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%--roww--%>


    <!-- sample modal content -->
    <div id="modalGrupo" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Agregar Grupo</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label for="validateSelect" class="col-md-4">Grupo</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtGrupo" runat="server" class="form-control"></asp:TextBox>
                        </div>

                    </div>

                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbtnAgregarGrupo" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" />
                    <%--                        <asp:Button ID="btnAgregarGrupo" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnAgregarGrupo_Click" />--%>
                </div>

            </div>
        </div>
    </div>
    <%--Fin modalGrupo--%>

    <!-- sample modal content -->
    <div id="modalPais" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Agregar Pais</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label for="validateSelect" class="col-md-4">Pais</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtPais" runat="server" class="form-control"></asp:TextBox>
                        </div>

                    </div>

                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbtnAgregarPais" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" />
                    <%--                        <asp:Button ID="btnAgregarGrupo" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnAgregarGrupo_Click" />--%>
                </div>

            </div>
        </div>
    </div>
    <%--Fin modalGrupo--%>

    <div id="modalSubGrupo" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Agregar SubGrupo</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label for="validateSelect" class="col-md-4">Grupo</label>
                        <div class="col-md-4">
                            <asp:DropDownList ID="DropListGrupo2" runat="server" class="form-control"></asp:DropDownList>
                        </div>
                        <div class="col-md-4">
                            <asp:TextBox ID="TxtSubGrupo" runat="server" class="form-control"></asp:TextBox>
                        </div>

                    </div>


                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbtnAgregarSubGrupo" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" />

                    <%--                        <asp:Button ID="btnAgregarSubgrupo" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnAgregarSubgrupo_Click" />--%>
                </div>
            </div>
        </div>
    </div>
    <%--Fin modalSubGrupo--%>

    <div id="modalVendedor" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Agregar Vendedor</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <div class="form-group">
                            <label for="name" class="col-md-4">Legajo</label>

                            <div class="col-md-6">

                                <asp:TextBox ID="txtLegajo" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>

                                <%--<input type="text" class="form-control" name="name" id="name">--%>
                            </div>
                            <div class="col-md-2">
                                <asp:RequiredFieldValidator ControlToValidate="txtLegajo" ID="RequiredFieldValidator21" runat="server" ErrorMessage="<h3>*</h3>" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" ValidationGroup="VendedorGroup"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="name" class="col-md-4">Nombre</label>
                            <div class="col-md-6">
                                <asp:TextBox ID="txtNombre" runat="server" class="form-control"></asp:TextBox>
                                <%--<input type="text" class="form-control" name="name" id="name">--%>
                            </div>
                            <div class="col-md-2">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtNombre" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" ValidationGroup="VendedorGroup"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="name" class="col-md-4">Apellido</label>
                            <div class="col-md-6">
                                <asp:TextBox ID="txtApellido" runat="server" class="form-control"></asp:TextBox>
                                <%--<input type="text" class="form-control" name="name" id="name">--%>
                            </div>
                            <div class="col-md-2">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtApellido" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" ValidationGroup="VendedorGroup"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="name" class="col-md-4">DNI</label>
                            <div class="col-md-6">
                                <asp:TextBox ID="txtDni" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                <%--<input type="text" class="form-control" name="name" id="name">--%>
                            </div>
                            <div class="col-md-2">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtDni" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" ValidationGroup="VendedorGroup"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="name" class="col-md-4">Dirección</label>
                            <div class="col-md-6">
                                <asp:TextBox ID="txtDireccion" runat="server" class="form-control"></asp:TextBox>
                                <%--<input type="text" class="form-control" name="name" id="name">--%>
                            </div>
                            <div class="col-md-2">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator27" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtDireccion" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" ValidationGroup="VendedorGroup"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="name" class="col-md-4">Fecha Nacimiento</label>
                            <div class="col-md-6">
                                <asp:TextBox ID="txtFechaNacimiento" runat="server" class="form-control"></asp:TextBox>
                            </div>

                            <div class="col-md-2">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator28" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaNacimiento" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" ValidationGroup="VendedorGroup"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="name" class="col-md-4">Fecha Ingreso</label>
                            <div class="col-md-6">
                                <asp:TextBox ID="txtFechaIngreso" runat="server" name="fecha" class="form-control"></asp:TextBox>
                            </div>

                            <div class="col-md-2">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator29" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaIngreso" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" ValidationGroup="VendedorGroup"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="name" class="col-md-4">Cuit/Cuil</label>
                            <div class="col-md-6">
                                <asp:TextBox ID="txtCuitVendedor" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                <%--<input type="text" class="form-control" name="name" id="name">--%>
                            </div>
                            <div class="col-md-2">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator30" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtCuitVendedor" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" ValidationGroup="VendedorGroup"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-2">
                                <asp:RegularExpressionValidator ControlToValidate="txtCuitVendedor" ID="RegularExpressionValidator5" runat="server" ErrorMessage="<h3>*</h3>" ValidationExpression="^\d{2}\d{8}\d{1}$" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" ValidationGroup="VendedorGroup"></asp:RegularExpressionValidator>
                            </div>
                        </div>

                        <div class="form-group">
                            <label for="name" class="col-md-4">Observaciones</label>
                            <div class="col-md-6">
                                <asp:TextBox ID="txtObservacionesVendedor" runat="server" class="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                <%--<input type="text" class="form-control" name="name" id="name">--%>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="name" class="col-md-4">Comisión</label>

                            <div class="col-md-6">
                                <div class="input-group">
                                    <span class="input-group-addon">$</span>
                                    <asp:TextBox ID="txtComision" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)" Style="text-align: right"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator31" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtComision" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" ValidationGroup="VendedorGroup"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-2">
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="txtComision" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="<h3>*</h3>" ValidationGroup="VendedorGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                            </div>
                        </div>

                        <%--<div class="form-group">
                                                <label for="validateSelect" class="col-md-4">Perfil</label>
                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="DropListProveedor" runat="server" class="form-control"></asp:DropDownList>
                                                </div>
                                                <%--<div class="col-md-4">
                                                    <a class="btn btn-info" data-toggle="modal" href="../Proveedores/ProoveedoresABM.aspx">Agregar Proveedor</a>
                                                </div>--%>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnAgregarVendedor" runat="server" Text="Guardar" class="btn btn-success" ValidationGroup="VendedorGroup" OnClick="btnAgregarVendedor_Click" />
                </div>

            </div>
        </div>
    </div>


    </div>
    <%--<script src="//code.jquery.com/jquery-1.9.1.js"></script>
    <script src="//code.jquery.com/ui/1.10.4/jquery-ui.js"></script>--%>

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
                if (key == 46 || key == 8 || key == 44)// Detectar . (punto) y backspace (retroceso) y , (coma)
                { return true; }
                else
                { return false; }
            }
            return true;
        }
    </script>


</asp:Content>
