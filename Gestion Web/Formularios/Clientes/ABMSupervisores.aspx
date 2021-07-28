<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ABMSupervisores.aspx.cs" Inherits="Gestion_Web.Formularios.Clientes.ABMSupervisores" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">


        <div class="row">

            <div class="col-md-12">
                
                <div class="widget stacked">

                    <div class="widget-header">
                        <i class="icon-pencil"></i>
                        <h3>Supervisores</h3>

                    </div>
                    <!-- /.widget-header -->

                    <div class="widget-content">

                        <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Always" runat="server">
                            <ContentTemplate>
                                <div id="validation-form" role="form" class="form-horizontal col-md-10">
                                    <fieldset>
                                       <h3 style="font-weight:bold">SUPERVISOR</h3>
                                        <div class="form-group" style="margin-bottom: 0">
                                            <label for="name" class="col-md-4">Buscar Cliente</label>

                                            <div class="col-md-4">

                                                <asp:TextBox ID="txtClienteS" runat="server" class="form-control"></asp:TextBox>
                                                <asp:RequiredFieldValidator ControlToValidate="txtClienteS" ID="RequiredFieldValidator2" runat="server" ErrorMessage="El campo es obligatorio" SetFocusOnError="true" ForeColor="Red" ValidationGroup="PVgroupClienteS" Font-Bold="true"></asp:RequiredFieldValidator>

                                                <%--<input type="text" class="form-control" name="name" id="name">--%>
                                            </div>
                                            <div class="col-md-4">
                                              <asp:LinkButton ID="lbBuscarCliente" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" ValidationGroup="PVgroupClienteS" onclick="lbBuscarClienteS_Click"/>

                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label class="col-md-4">Clientes</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="DropListClienteS" runat="server" class="form-control" AutoPostBack="true"></asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="DropListClienteS" InitialValue="-1" ValidationGroup="PVGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>

                                                <!-- /input-group -->
                                            </div>

                                            <div class="col-md-4">
                                            </div>
                                        </div>
                                        <h3 style="font-weight:bold">Clientes (vendedor)</h3>
                                        <div class="form-group"style="margin-bottom: 0">
                                            <label for="name" class="col-md-4">Buscar Cliente</label>

                                            <div class="col-md-4">

                                                <asp:TextBox ID="txtClienteV" runat="server" class="form-control"></asp:TextBox>
                                                <asp:RequiredFieldValidator ControlToValidate="txtClienteV" ID="RequiredFieldValidator1" runat="server" ErrorMessage="El campo es obligatorio" SetFocusOnError="true" ForeColor="Red" ValidationGroup="PVgroupClienteV" Font-Bold="true"></asp:RequiredFieldValidator>

                                                <%--<input type="text" class="form-control" name="name" id="name">--%>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:LinkButton ID="lbBuscarClienteV" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" ValidationGroup="PVgroupClienteV" onclick="lbBuscarClienteV_Click" />

                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label class="col-md-4">Clientes</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="DropListClienteV" runat="server" class="form-control" AutoPostBack="true"></asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="DropListClienteV" InitialValue="-1" ValidationGroup="PVGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>

                                                <!-- /input-group -->
                                            </div>

                                            <div class="col-md-4">
                                            </div>
                                        </div>

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
                                    </fieldset>
                                </div>
                   
                               <div class="col-md-8">
                                    <asp:Button ID="BtnAgregar" runat="server" Text="Agregar" ValidationGroup="PVGroup" class="btn btn-success"  OnClick="btnAgregar_Click" />
                               </div>          

                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>

<%--            <asp:PlaceHolder ID="PHUsuariosStore" Visible="true" runat="server">
                <div class="col-md-12 col-xs-12">
                    <div class="widget stacked widget-table action-table">
                        <div class="widget-header">
                            <h3>Datos Usuario Store</h3>
                        </div>
                        <div class="widget-content">
                            <div class="panel-body">
                                <asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Conditional" runat="server">
                                    <ContentTemplate>
                                        <div class="table-responsive">
                                            <a class="btn btn-info" style="display: none" data-toggle="modal" id="abreDialog" href="#modalFacturaDetalle">Agregar Tipo Cliente</a>
                                            <table class="table table-striped table-bordered table-hover" id="dataTables-example">
                                                <thead>
                                                    <tr>
                                                        <th style="width: 10%">Usuario</th>
                                                        <th style="width: 10%">Contraseña</th>
                                                        <th style="width: 10%">Nombre</th>
                                                        <th style="width: 10%">Apellido</th>
                                                        <th style="width: 10%">Telefono</th>
                                                        <th style="width: 10%">Mail</th>
                                                        <th style="width: 10%">Coeficiente</th>
                                                        <th style="width: 10%">Perfil</th>
                                                        <th style="width: 10%">Store</th>
                                                        <th class="td-actions" style="width: 10%"></th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <asp:PlaceHolder ID="PHUsuariosStoreTabla" runat="server"></asp:PlaceHolder>
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
            </asp:PlaceHolder>--%>

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
                        <td style="width: 100%">
                        <%-- <div class="row">--%>
                            <label class="col-md-4">Buscar Supervisor/vendedor: </label>
                            <div class="col-md-4">
                                <div class="input-group">
                                    <span class="input-group-btn">
                                        <asp:LinkButton ID="lbBuscar" runat="server" Text="Buscar" class="btn btn-primary" onclick="lbBuscar_Click">
                                                    <i class="shortcut-icon icon-search"></i></asp:LinkButton>
                                    </span>
                                    <asp:TextBox ID="txtBusqueda" runat="server" style="max-width:100% !important" class="form-control" placeholder="Buscar"></asp:TextBox>

                                </div>
                                <!-- /input-group -->
                            </div>
                         <%-- </div>--%>
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
                <i class="icon-bookmark"></i>
                <h3>Supervisores</h3>
            </div>
            <div class="widget-content">
                <div class="panel-body">

                    <%--<div class="col-md-12 col-xs-12">--%>
                    <div class="table-responsive">
                        <table class="table table-striped table-bordered table-hover" id="dataTables-example">
                            <thead>
                                <tr>
                                    <th runat="server" style="width: 20%">Supervisor</th>
                                    <th runat="server" style="width: 20%">Vendedor</th>
                                    <asp:PlaceHolder ID="phColumna6" runat="server">
                                        <th runat="server" id="headerButtons" style="width: 5%"></th>
                                    </asp:PlaceHolder>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:PlaceHolder ID="phSupervisiones" runat="server"></asp:PlaceHolder>
                            </tbody>
                        </table>
                    </div>
                </div>
                <!-- /.content -->
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
    <div id="modalSupervision" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Editar Supervision</h4>
                </div>
                <div class="modal-body">
                   
                 <div class="widget-content">

                        <asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Always" runat="server">
                            <ContentTemplate>
                                <div id="validation-form2" role="form" class="form-horizontal col-md-10">
                                    <fieldset>
                                       <h3 style="font-weight:bold">SUPERVISOR</h3>
                                      <div class="form-group">
                                            <label for="name" class="col-md-4">Buscar Cliente</label>

                                            <div class="col-md-6">

                                                <asp:TextBox ID="txtClienteS2" runat="server" class="form-control"></asp:TextBox>
                                                <asp:RequiredFieldValidator ControlToValidate="txtClienteS2" ID="RequiredFieldValidator6" runat="server" ErrorMessage="El campo es obligatorio" SetFocusOnError="true" ForeColor="Red" ValidationGroup="PVgroupClienteS2" Font-Bold="true"></asp:RequiredFieldValidator>
                                                
                                                <%--<input type="text" class="form-control" name="name" id="name">--%>
                                            </div>
                                            <div class="col-md-2">
                                              <asp:LinkButton ID="lbBuscarClienteS2" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" ValidationGroup="PVgroupClienteS2" onclick="lbBuscarClienteS2_Click"/>

                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label class="col-md-4">Clientes</label>
                                            <div class="col-md-6">
                                                <asp:DropDownList ID="DropListClientesS2" runat="server" class="form-control" AutoPostBack="true"></asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="DropListClientesS2" InitialValue="-1" ValidationGroup="PVGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>

                                                <!-- /input-group -->
                                            </div>

                                            <div class="col-md-2">
                                            </div>
                                        </div>
                                        <h3 style="font-weight:bold">Clientes (vendedor)</h3>
                                        <div class="form-group">
                                            <label for="name" class="col-md-4">Buscar Cliente</label>

                                            <div class="col-md-6">

                                                <asp:TextBox ID="txtClienteV2" runat="server" class="form-control"></asp:TextBox>
                                                <asp:RequiredFieldValidator ControlToValidate="txtClienteV2" ID="RequiredFieldValidator9" runat="server" ErrorMessage="El campo es obligatorio" SetFocusOnError="true" ForeColor="Red" ValidationGroup="PVgroupClienteV2" Font-Bold="true"></asp:RequiredFieldValidator>

                                                <%--<input type="text" class="form-control" name="name" id="name">--%>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbBuscarClienteV2" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" ValidationGroup="PVgroupClienteV2" onclick="lbBuscarClienteV2_Click" />

                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label class="col-md-4">Clientes</label>
                                            <div class="col-md-6">
                                                <asp:DropDownList ID="DropListClientesV2" runat="server" class="form-control" AutoPostBack="true"></asp:DropDownList>

                                                <!-- /input-group -->
                                            </div>

                                            <div class="col-md-2">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="DropListClientesV2" InitialValue="-1" ValidationGroup="PVgroupClienteV2" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>

                                       <%-- <asp:Panel runat="server" ID="panel1" Visible="false">
                                            <div class="form-group">
                                                <label class="col-md-4">Buscar Cliente</label>
                                                <div class="col-md-6">
                                                    <asp:TextBox ID="TextBox3" class="form-control" runat="server"></asp:TextBox>
                                                </div>

                                                <div class="col-md-2">
                                                    <asp:LinkButton ID="LinkButton3" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" OnClick="btnBuscarCod_Click" />
                                                </div>
                                            </div>
                                            <div class="form-group">

                                                <div class="col-md-4">
                                                    <asp:Label runat="server" ID="Label2" Text="Clientes" Font-Bold="true"></asp:Label>
                                                </div>

                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="DropDownList3" runat="server" class="form-control">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-md-1">
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="ListClientes" InitialValue="-1" ValidationGroup="PVGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </asp:Panel>--%>
                                    </fieldset>
                                </div>
                   
                               <%--<div class="col-md-8">
                                    <asp:Button ID="Button1" runat="server" Text="Agregar" class="btn btn-success" ValidationGroup="PVGroup" OnClick="btnAgregar_Click" />
                               </div>    --%>      

                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>

                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbtnModificarSupervision" runat="server" Text="<span class='shortcut-icon icon-ok'></span> Confirmar Cambio" class="btn btn-success" OnClick="lbtnModificarSupervision_Click" />
                   <asp:TextBox runat="server" ID="txtbxIdSupervision" Text="0" Style="display:none"></asp:TextBox>

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

    <div id="modalAgregarUsuarioAlStore" visible="false" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Agregar Usuario al Store</h4>
                </div>
                <div class="modal-body">
                    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Always" runat="server">
                        <ContentTemplate>
                            <div role="form" class="form-horizontal col-md-12">
                                <fieldset>
                                    <div class="form-group">

                                        <label class="col-md-4">Usuario</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtUsuarioStore" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtUsuarioStore" ValidationGroup="AgregarUsuarioAlStore" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Contraseña</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtContraseñaStore" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtContraseñaStore" ValidationGroup="AgregarUsuarioAlStore" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Nombre</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtNombreStore" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtNombreStore" ValidationGroup="AgregarUsuarioAlStore" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Apellido</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtApellidoStore" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <%--<div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtApellidoStore" ValidationGroup="AgregarUsuarioAlStore" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>--%>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Telefono</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtTelefonoStore" runat="server" onkeypress="javascript:return validarNroSinComa(event)" class="form-control"></asp:TextBox>
                                        </div>
                                        <%--<div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtTelefonoStore" ValidationGroup="AgregarUsuarioAlStore" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>--%>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Mail</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtMailStore" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                       <%-- <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtMailStore" ValidationGroup="AgregarUsuarioAlStore" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>--%>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Coeficiente</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtCoeficienteStore" runat="server" onkeypress="javascript:return validarNroSinComa(event)" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtCoeficienteStore" ValidationGroup="AgregarUsuarioAlStore" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">PerfilStore</label>
                                        <div class="col-md-4">
                                            <asp:DropDownList ID="DropPerfilStore" runat="server" class="form-control" AutoPostBack="true"></asp:DropDownList>
                                            <%--OnSelectedIndexChanged="DropPerfilStore_SelectedIndexChanged"--%>

                                            <!-- /input-group -->
                                        </div>

                                        <div class="col-md-4">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="DropPerfilStore" InitialValue="0" ValidationGroup="AgregarUsuarioAlStore" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Store</label>
                                        <div class="col-md-4">
                                            <asp:DropDownList ID="DropStore" runat="server" class="form-control" AutoPostBack="true"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="DropStore" InitialValue="0" ValidationGroup="AgregarUsuarioAlStore" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </fieldset>
                            </div>
                            </div>
                        </ContentTemplate>
                        <Triggers>
                        </Triggers>
                    </asp:UpdatePanel>
                    <div class="modal-footer">
                        <asp:LinkButton ID="AgregarUsuarioAlStore" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnAgregarUsuarioAlStore_Click" ValidationGroup="AgregarUsuarioAlStore" />
                    </div>
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
                                    <asp:Label runat="server" ID="lblMensaje" Text="Esta seguro que desea eliminar la relación superviso -> vendedor" Style="text-align: center"></asp:Label>
                                </h5>
                            </div>

                            <div class="col-md-3">
                                <asp:TextBox runat="server" ID="txtIDSupervision" Text="0" Style="display:none"></asp:TextBox>
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

        function validarNroSinComa(e) {
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
                if (key == 8)// || key == 44) // Detectar . (punto) , backspace (retroceso) y , (coma)
                { return true; }
                else
                { return false; }
            }
            return true;
        }
    </script>

    <script type="text/javascript">
        function abrirConfirmacion(valor) {
            $('#modalConfirmacion').modal('show');
            document.getElementById('<%= txtIDSupervision.ClientID %>').value = valor;

        }
        function abrirdialog(id)
        {
            document.getElementById('MainContent_txtIDSupervision').value = id;
        }

        function openModalAgregarUsuarioAlStore() {
            $('#modalAgregarUsuarioAlStore').modal('show');
        }
        function abrirModalSupervision(id,supervisor,vendedor) {
            $('#modalSupervision').modal('show');
            document.getElementById('MainContent_DropListClientesS2').value = supervisor;
            document.getElementById('MainContent_DropListClientesV2').value = vendedor;
            document.getElementById('MainContent_txtbxIdSupervision').value = id;

        }
    </script>




</asp:Content>
