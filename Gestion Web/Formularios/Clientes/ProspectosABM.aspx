<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProspectosABM.aspx.cs" Inherits="Gestion_Web.Formularios.Clientes.ProspectosABM"
    EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container">
        <div class="row">
            <div class="col-md-12">
                <div class="widget stacked ">
                    <div class="widget-header">
                        <i class="icon-pencil"></i>Prospecto
                        <h3>
                            <asp:Label ID="labelNombre" runat="server" Text=""></asp:Label>
                            <asp:Label ID="labelNombreCliente" runat="server" Text=""></asp:Label>
                            <asp:HiddenField ID="hiddenIdCliente" runat="server"></asp:HiddenField>
                            <asp:HiddenField ID="hiddenOrigenCliente" runat="server"></asp:HiddenField>
                        </h3>
                    </div>
                    <div class="widget-content">
                        <div class="bs-example">
                            <ul id="myTab" class="nav nav-tabs">
                                <li class="active"><a href="#home" data-toggle="tab">Datos</a></li>
                                <li><a href="#profile" data-toggle="tab">Direccion</a></li>
                                <li><a href="#Contacto" data-toggle="tab">Contacto</a></li>
                                <li><a href="#DatosFiscales" data-toggle="tab">Datos Fiscales</a></li>
                                <li><a href="#DatosPatrimoniales" data-toggle="tab">Datos Patrimoniales</a></li>
                                <li><a href="#DatosDistribucion" data-toggle="tab">Datos Distribucion</a></li>
                                <li><a href="#RecepcionMercaderia" data-toggle="tab">Recepcion Mercaderia</a></li>
                                <li><a href="#Garante" data-toggle="tab">Garante</a></li>
                                <li><a href="#Documentacion" data-toggle="tab">Documentacion</a></li>

                            </ul>
                            <div class="tab-content">
                                <div class="tab-pane fade active in" id="home">
                                    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Always" runat="server">
                                        <ContentTemplate>

                                            <div id="validation-form" role="form" class="form-horizontal col-md-10">
                                                <fieldset>
                                                    <div class="form-group">
                                                        <label for="name" class="col-md-4">Nombre y Apellido</label>

                                                        <div class="col-md-4">

                                                            <asp:TextBox ID="txtNombreApellido" runat="server" class="form-control"></asp:TextBox>

                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtNombreApellido" ValidationGroup="ClienteGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label class="col-md-4">Fecha de Nacimiento: </label>
                                                        <div class="col-md-4">
                                                            <div class="input-group">
                                                                <span class="input-group-addon"><i class="shortcut-icon icon-calendar"></i></span>
                                                                <asp:TextBox ID="txtFechaNacimiento" runat="server" class="form-control"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label for="name" class="col-md-4">Nacionalidad</label>

                                                        <div class="col-md-4">

                                                            <asp:TextBox ID="txtNacionalidad" runat="server" class="form-control"></asp:TextBox>

                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtNacionalidad" ValidationGroup="ClienteGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label for="validateSelect" class="col-md-4">Documento</label>
                                                        <div class="col-md-4">
                                                            <asp:DropDownList ID="ListTipoDocumento" runat="server" class="form-control"></asp:DropDownList>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:TextBox ID="txtNumeroDocumento" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label for="name" class="col-md-4">Estado civil</label>
                                                        <div class="col-md-4">
                                                            <asp:DropDownList ID="ListEstadoCivil" runat="server" class="form-control"></asp:DropDownList>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label class="col-md-4">Nombre y apellido del conyuge(si corresponde)</label>
                                                        <div class="col-md-4">
                                                            <asp:TextBox ID="txtNombreYApellidoConyuge" runat="server" class="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label for="name" class="col-md-4">Dni Conyuge</label>

                                                        <div class="col-md-4">
                                                            <asp:TextBox ID="txtDniConyuge" runat="server" class="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label for="name" class="col-md-4">Estudios alcanzados</label>
                                                        <div class="col-md-4">
                                                            <asp:DropDownList ID="ListEstudiosAlcanzados" runat="server" class="form-control">
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label for="name" class="col-md-4">Profesion</label>

                                                        <div class="col-md-4">
                                                            <asp:TextBox ID="txtProfesion" runat="server" class="form-control"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                    <div class="form-group">
                                                        <label for="name" class="col-md-4">Trabaja actualmente en realacion dependencia?</label>
                                                        <div class="col-md-4">
                                                            <asp:DropDownList ID="ListRelacionDependencia" runat="server" class="form-control">
                                                                <asp:ListItem Text="Si" Value="1"></asp:ListItem>
                                                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                </fieldset>
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    <div class="col-md-8">
                                        <asp:Button ID="btnAgregar" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnAgregar_Click" ValidationGroup="ClienteGroup" />
                                        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" class="btn btn-default" PostBackUrl="~/Formularios/Clientes/ProspectosF.aspx" />
                                    </div>
                                </div>

                                <%--Direccion--%>
                                <div class="tab-pane fade" id="profile">
                                    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Always" runat="server">
                                        <ContentTemplate>
                                            <div class="form-horizontal col-md-12">

                                                <%--<fieldset>--%>

                                                <div class="form-group">
                                                    <label for="validateSelect" class="col-md-4">Tipo</label>
                                                    <div class="col-md-4">
                                                        <asp:DropDownList ID="ListTipo" class="form-control" disabled="disabled" runat="server">
                                                            <asp:ListItem Text="Personal" Value="1"></asp:ListItem>
                                                            <%-- <asp:ListItem Text="Patrimonial" Value="2"></asp:ListItem>
                                                            <asp:ListItem Text="Recepcion" Value="3"></asp:ListItem>--%>
                                                        </asp:DropDownList>
                                                    </div>

                                                    <div class="col-md-4">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ErrorMessage="*" ControlToValidate="ListTipo" ValidationGroup="DireccionGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Calle</label>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtCalle" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtCalle" ValidationGroup="DireccionGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label class="col-md-4">Numero</label>
                                                    <div class="col-md-4">

                                                        <asp:TextBox ID="txtCalleNumero" runat="server" class="form-control" onkeypress="javascript:return validarSoloNro(event)"></asp:TextBox>

                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <label for="validateSelect" class="col-md-4">Provincia</label>
                                                    <div class="col-md-4">
                                                        <asp:DropDownList ID="ListProvincia" class="form-control" runat="server" OnSelectedIndexChanged="ListProvincia_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                                    </div>

                                                    <div class="col-md-4">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator18" InitialValue="-1" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="ListProvincia" ValidationGroup="DireccionGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <label for="validateSelect" class="col-md-4">Localidad</label>
                                                    <div class="col-md-4">
                                                        <asp:DropDownList ID="ListLocalidad" class="btn btn-default ui-tooltip dropdown-toggle" runat="server" OnSelectedIndexChanged="ListLocalidad_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                                        <asp:TextBox ID="txtLocalidad" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                    <%--                                                    <div class="col-md-4">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="ListLocalidad" ValidationGroup="DireccionGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>

                                                    </div>--%>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Codigo Postal</label>

                                                    <div class="col-md-4">

                                                        <asp:TextBox ID="txtCodigoPostal" runat="server" class="form-control" ReadOnly="true" onkeypress="javascript:return validarSoloNro(event)"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtCodigoPostal" ValidationGroup="DireccionGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Barrio</label>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtBarrio" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-4"></div>
                                                </div>

                                                <div class="form-group">
                                                    <label for="validateSelect" class="col-md-4">Señale que tipo de vivienda</label>
                                                    <div class="col-md-4">
                                                        <asp:DropDownList ID="ListTipoVivienda" class="form-control" runat="server"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="validateSelect" class="col-md-4">Metrox aprox.</label>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtMetrosVivienda" runat="server" class="form-control" onkeypress="javascript:return validarSoloNro(event)"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <div class="col-md-1">
                                                        <asp:LinkButton ID="lbtnAgregarDireccion" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnAgregarDireccion_Click" ValidationGroup="DireccionGroup" />
                                                    </div>
                                                </div>

                                            </div>
                                            <%-- </fieldset>--%>

                                            <div class="col-md-12">

                                                <div class="widget stacked widget-table">

                                                    <div class="widget-header">
                                                        <span class="icon-external-link"></span>
                                                        <h3>Direcciones</h3>
                                                    </div>
                                                    <!-- .widget-header -->

                                                    <div class="widget-content">
                                                        <table class="table table-bordered table-striped">

                                                            <thead>
                                                                <tr>
                                                                    <th style="width: 15%">Tipo</th>
                                                                    <th style="width: 20%">Calle</th>
                                                                    <th style="width: 20%">Provincia</th>
                                                                    <th style="width: 15%">Localidad</th>
                                                                    <th style="width: 10%">Barrio</th>
                                                                    <th style="width: 5%">Codigo Postal</th>
                                                                    <th style="width: 15%"></th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>
                                                                <asp:PlaceHolder ID="phDireccion" runat="server"></asp:PlaceHolder>
                                                            </tbody>
                                                        </table>

                                                    </div>
                                                    <!-- .widget-content -->

                                                </div>

                                            </div>

                                        </ContentTemplate>

                                    </asp:UpdatePanel>
                                </div>
                                <%--Fin direccion--%>



                                <%-- Contactos --%>
                                <div class="tab-pane fade" id="Contacto">
                                    <asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Always" runat="server">
                                        <ContentTemplate>
                                            <div role="form" class="form-horizontal col-md-12">
                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Numero telefono</label>

                                                    <div class="col-md-4">
                                                        <%--<asp:TextBox ID="txtNumeroContacto" runat="server" class="form-control" onkeypress="javascript:return validarNroGuion(event)"></asp:TextBox>--%>
                                                        <asp:TextBox ID="txtNumeroTelefono" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Numero celular</label>

                                                    <div class="col-md-4">
                                                        <%--<asp:TextBox ID="txtNumeroContacto" runat="server" class="form-control" onkeypress="javascript:return validarNroGuion(event)"></asp:TextBox>--%>
                                                        <asp:TextBox ID="txtNumeroCelular" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Mail</label>

                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtMailContacto" runat="server" class="form-control"></asp:TextBox>

                                                    </div>
                                                    <div class="col-md-1">
                                                        <asp:LinkButton ID="lbtnAgregarContacto" OnClick="lbtnAgregarContacto_Click" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" ValidationGroup="ContactoGroup" />
                                                    </div>
                                                    <div class="col-md-3">
                                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Debe tener un formato mail Ej: ejemplo@mail.com" ControlToValidate="txtMailContacto" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="ContactoGroup"></asp:RegularExpressionValidator>
                                                    </div>
                                                </div>

                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <%-- Fin Contactos --%>

                                <%--Datos Fiscales--%>
                                <div class="tab-pane fade" id="DatosFiscales">
                                    <asp:UpdatePanel ID="UpdatePanel7" UpdateMode="Always" runat="server">
                                        <ContentTemplate>
                                            <div role="form" class="form-horizontal col-md-10">

                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Razon social</label>

                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtRazonSocial" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">CUIT</label>

                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtCuit" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Condicion IVA</label>

                                                    <div class="col-md-4">
                                                        <asp:DropDownList ID="listCondicionIVA" runat="server" class="form-control"></asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <div class="col-md-4">
                                                        <asp:Button ID="btnAgregarDatosFiscales" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnAgregarDatosFiscales_Click" />
                                                    </div>
                                                </div>
                                            </div>

                                        </ContentTemplate>
                                        <Triggers>
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <%-- Fin Datos Fiscales --%>

                                <%--Datos Patrimoniales--%>
                                <div class="tab-pane fade" id="DatosPatrimoniales">
                                    <asp:UpdatePanel ID="UpdatePanel8" UpdateMode="Always" runat="server">
                                        <ContentTemplate>
                                            <div role="form" class="form-horizontal col-md-10">

                                                <div class="form-group">
                                                    <label for="validateSelect" class="col-md-4">Posee algun rodado</label>
                                                    <div class="col-md-4">
                                                        <asp:DropDownList ID="ListRodado" class="form-control" runat="server">
                                                            <asp:ListItem Text="Si" Value="1" />
                                                            <asp:ListItem Text="No" Value="2" />
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Año</label>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtAñoRodado" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                    </div>
                                                    <%--  <div class="col-md-1">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" ControlToValidate="txtAñoRodado" ValidationGroup="PatrimonioGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </div>--%>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Modelo</label>

                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtModeloRodado" runat="server" class="form-control"></asp:TextBox>
                                                    </div>

                                                    <%--<div class="col-md-1">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="txtModeloRodado" ValidationGroup="PatrimonioGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </div>--%>
                                                </div>
                                                <div class="form-group">
                                                    <label for="validateSelect" class="col-md-4">Posee vivienda</label>
                                                    <div class="col-md-4">
                                                        <asp:DropDownList ID="ListPoseeVivienda" class="form-control" runat="server">
                                                            <asp:ListItem Text="Si" Value="1" />
                                                            <asp:ListItem Text="No" Value="2" />
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="validateSelect" class="col-md-4">Provincia</label>
                                                    <div class="col-md-4">
                                                        <asp:DropDownList ID="ListProvinciaPatrimonial" class="form-control" runat="server" OnSelectedIndexChanged="ListProvinciaPatrimonial_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                                    </div>

                                                    <div class="col-md-4">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" InitialValue="-1" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="ListProvinciaPatrimonial" ValidationGroup="PatrimonioGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <label for="validateSelect" class="col-md-4">Localidad</label>
                                                    <div class="col-md-4">
                                                        <asp:DropDownList ID="ListLocalidadPatrimonial" class="btn btn-default ui-tooltip dropdown-toggle" runat="server"></asp:DropDownList>
                                                        <%--<asp:TextBox ID="TextBox1" runat="server" class="form-control"></asp:TextBox>--%>
                                                    </div>
                                                    <%--                                                    <div class="col-md-4">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="ListLocalidad" ValidationGroup="DireccionGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>

                                                    </div>--%>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Direccion</label>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtDireccionPatrimonial" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Observaciones</label>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtObservacionesPatrimoniales" runat="server" TextMode="MultiLine" Rows="10" class="form-control"></asp:TextBox>
                                                    </div>
                                                    <%--<div class="col-md-1">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="*" ControlToValidate="txtDireccionPatrimonial" ValidationGroup="PatrimonioGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </div>--%>
                                                </div>

                                                <div class="form-group">
                                                    <div class="col-md-4">
                                                        <asp:Button ID="btnAgregarDatosPatrimoniales" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnAgregarDatosPatrimoniales_Click" ValidationGroup="PatrimonioGroup" />
                                                    </div>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <%-- Fin Datos Patrimoniales --%>

                                <%-- Recepcion Mercaderia --%>
                                <div class="tab-pane fade" id="RecepcionMercaderia">
                                    <asp:UpdatePanel ID="UpdatePanel4" UpdateMode="Always" runat="server">
                                        <ContentTemplate>
                                            <div role="form" class="form-horizontal col-md-10">
                                                <div class="form-group">
                                                    <label for="validateSelect" class="col-md-4">Direccion</label>
                                                    <div class="col-md-4">
                                                        <asp:DropDownList ID="ListRecepcionDireccion" class="form-control" runat="server">
                                                        </asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Dar de alta direccion tipo Recepcion" ControlToValidate="ListRecepcionDireccion" ValidationGroup="RecepcionGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Indicaciones especiales para encontrar domicilio</label>

                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtIndicacionEspecialDomicilio" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Indicaciones especiales para la entrega, directa de empresa</label>

                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtIndicacionEspecialDomicilioDirectaEmpresa" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Expreso en caso de vivir mas de 60 KM</label>

                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtExpreso" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <div class="col-md-4">
                                                        <asp:Button ID="btnAgregarRecepcionMercaderia" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnAgregarRecepcionMercaderia_Click" ValidationGroup="RecepcionGroup" />
                                                    </div>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <%-- Fin Recepcion Mercaderia --%>

                                <%-- Datos Distribucion --%>
                                <div class="tab-pane fade" id="DatosDistribucion">
                                    <asp:UpdatePanel ID="UpdatePanel6" UpdateMode="Always" runat="server">
                                        <ContentTemplate>
                                            <div role="form" class="form-horizontal col-md-12">

                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Nombre del grupo</label>

                                                    <div class="col-md-6">
                                                        <asp:TextBox ID="txtNombreGrupo" runat="server" class="form-control ui-popover" data-container="" data-toggle="popover" data-trigger="hover" data-placement="right"
                                                            data-content="Nombre de fantasía con el que quiere denominar a su grupo." data-original-title="Atención !"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">

                                                    <label for="name" class="col-md-4">Codigo Distribuidor Madre</label>
                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtCodDistribuidor" class="form-control" runat="server"></asp:TextBox>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:LinkButton ID="btnBuscarCodigoDistribuidor" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" OnClick="btnBuscarCodigoDistribuidor_Click" />
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="validateSelect" class="col-md-4">Proveedor</label>
                                                    <div class="col-md-4">
                                                        <asp:DropDownList ID="ListDistribuidor" runat="server" class="form-control"></asp:DropDownList>
                                                    </div>
                                                    <div class="col-md-2">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="ListDistribuidor" ValidationGroup="DistribuicionGroup" SetFocusOnError="true" Font-Bold="true" InitialValue="-1" ForeColor="Red"></asp:RequiredFieldValidator>
                                                    </div>

                                                </div>
                                                <div class="form-group">
                                                    <div class="col-md-4">
                                                        <asp:Button ID="btnAgregarDatoDistribucion" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnAgregarDatoDistribucion_Click" ValidationGroup="DistribuicionGroup" />
                                                    </div>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>

                                <%-- Garante --%>

                                <div class="tab-pane fade" id="Garante">
                                    <ul id="myTab2" class="nav nav-tabs">
                                        <li class="active"><a href="#datosgarante" data-toggle="tab">Datos Personales</a></li>
                                        <li><a href="#contactogarante" data-toggle="tab">Datos de Contacto</a></li>
                                        <li><a href="#patrimoniogarante" data-toggle="tab">Datos Patrimoniales</a></li>

                                    </ul>
                                    <div class="tab-content">
                                        <div class="tab-pane fade active in" id="datosgarante">

                                            <asp:UpdatePanel ID="UpdatePanel5" UpdateMode="Always" runat="server">
                                                <ContentTemplate>


                                                    <div role="form" class="form-horizontal col-md-10">

                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Nombre</label>

                                                            <div class="col-md-4">

                                                                <asp:TextBox ID="txtNombreGarante" runat="server" class="form-control"></asp:TextBox>

                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtNombreGarante" ValidationGroup="GaranteGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Apellido</label>

                                                            <div class="col-md-4">

                                                                <asp:TextBox ID="txtApellidoGarante" runat="server" class="form-control"></asp:TextBox>

                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtApellidoGarante" ValidationGroup="GaranteGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label class="col-md-4">Fecha de Nacimiento: </label>
                                                            <div class="col-md-4">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon"><i class="shortcut-icon icon-calendar"></i></span>
                                                                    <asp:TextBox ID="txtFechaNacimientoGarante" runat="server" class="form-control"></asp:TextBox>
                                                                </div>

                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtFechaNacimientoGarante" ValidationGroup="GaranteGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>

                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Nacionalidad</label>

                                                            <div class="col-md-4">

                                                                <asp:TextBox ID="txtNacionalidadGarante" runat="server" class="form-control"></asp:TextBox>

                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtNacionalidadGarante" ValidationGroup="GaranteGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>

                                                        <div class="form-group">
                                                            <label for="validateSelect" class="col-md-4">Documento</label>
                                                            <div class="col-md-4">
                                                                <asp:DropDownList ID="ListTiposDocumentosGarante" runat="server" class="form-control"></asp:DropDownList>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtDocumentoGarante" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtDocumentoGarante" ValidationGroup="GaranteGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>

                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Estado civil</label>
                                                            <div class="col-md-4">
                                                                <asp:DropDownList ID="ListEstadoCivilGarante" runat="server" class="form-control"></asp:DropDownList>
                                                            </div>
                                                        </div>

                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Domicilio Personal del Garante: Calle </label>

                                                            <div class="col-md-4">

                                                                <asp:TextBox ID="txtDomicilioGarante" runat="server" class="form-control"></asp:TextBox>

                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtDomicilioGarante" ValidationGroup="GaranteGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">N° </label>

                                                            <div class="col-md-4">

                                                                <asp:TextBox ID="txtNumeroDireccionGarante" TextMode="Number" runat="server" class="form-control"></asp:TextBox>

                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtNumeroDireccionGarante" ValidationGroup="GaranteGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>

                                                        <div class="form-group">
                                                            <label for="validateSelect" class="col-md-4">Provincia</label>
                                                            <div class="col-md-4">
                                                                <asp:DropDownList ID="ListProvinciaGarantes" class="form-control" runat="server" OnSelectedIndexChanged="ListProvinciaGarantes_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                                            </div>

                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator13" InitialValue="-1" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="ListProvinciaPatrimonial" ValidationGroup="PatrimonioGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>

                                                        <div class="form-group">
                                                            <label for="validateSelect" class="col-md-4">Localidad</label>
                                                            <div class="col-md-4">
                                                                <asp:DropDownList ID="ListLocalidadGarantes" OnSelectedIndexChanged="ListLocalidadGarantes_SelectedIndexChanged" AutoPostBack="True" class="btn btn-default ui-tooltip dropdown-toggle" runat="server"></asp:DropDownList>
                                                                <%--<asp:TextBox ID="TextBox1" runat="server" class="form-control"></asp:TextBox>--%>
                                                            </div>
                                                            <%--                                                    <div class="col-md-4">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="ListLocalidad" ValidationGroup="DireccionGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>

                                                    </div>--%>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">CP </label>

                                                            <div class="col-md-4">

                                                                <asp:TextBox ID="txtCodigoPostalGarante" runat="server" ReadOnly="true" class="form-control"></asp:TextBox>

                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtCodigoPostalGarante" ValidationGroup="GaranteGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>

                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Profesion</label>

                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtProfesionGarante" runat="server" class="form-control"></asp:TextBox>
                                                            </div>

                                                        </div>

                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Trabaja actualmente en relacion dependencia?</label>
                                                            <div class="col-md-4">
                                                                <asp:DropDownList ID="ListRelacionDependenciaGarante" runat="server" class="form-control">
                                                                    <asp:ListItem Text="Si" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Antiguedad</label>

                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtAntiguedadGarante" runat="server" class="form-control"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Cargo</label>
                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtCargoGarante" runat="server" class="form-control"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Razon Social</label>
                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtRazonSocialEmpleadorGarante" runat="server" class="form-control"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Domicilio</label>
                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtDomicilioEmpleadorGarante" runat="server" class="form-control"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">CUIT</label>
                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtCUITEmpleadorGarante" runat="server" class="form-control"></asp:TextBox>
                                                            </div>
                                                        </div>

                                                        <div class="form-group">
                                                            <label class="col-md-4">Nombre del conyuge</label>
                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtNombreConyugeGarante" runat="server" class="form-control"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label class="col-md-4">Apellido del conyuge</label>
                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtApellidoConyugeGarante" runat="server" class="form-control"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Dni Conyuge</label>

                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtDNIConyugeGarante" runat="server" class="form-control"></asp:TextBox>
                                                            </div>
                                                        </div>



                                                        <div class="form-group">
                                                            <div class="col-md-4">
                                                                <asp:Button ID="btnAgregarDatosGarante" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnAgregarDatosGarante_Click" ValidationGroup="GaranteGroup" />
                                                            </div>
                                                        </div>



                                                    </div>

                                                </ContentTemplate>
                                                <Triggers>
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>



                                        <%-- Contacto Garante --%>

                                        <div class="tab-pane fade" id="contactogarante">
                                            <asp:UpdatePanel ID="UpdatePanel9" UpdateMode="Always" runat="server">
                                                <ContentTemplate>
                                                    <div role="form" class="form-horizontal col-md-10">
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Telefono Fijo</label>

                                                            <div class="col-md-4">
                                                                <%--<asp:TextBox ID="txtNumeroContacto" runat="server" class="form-control" onkeypress="javascript:return validarNroGuion(event)"></asp:TextBox>--%>
                                                                <asp:TextBox ID="txtTelefonoFijoGarante" TextMode="Number" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Telefono Celular</label>

                                                            <div class="col-md-4">
                                                                <%--<asp:TextBox ID="txtNumeroContacto" runat="server" class="form-control" onkeypress="javascript:return validarNroGuion(event)"></asp:TextBox>--%>
                                                                <asp:TextBox ID="txtTelefonoCelularGarante" TextMode="Number" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                            </div>
                                                        </div>

                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Correo Electronico</label>

                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtCorreoElectronicoGarante" runat="server" class="form-control"></asp:TextBox>

                                                            </div>
                                                            <div class="col-md-3">
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Debe tener un formato mail Ej: ejemplo@mail.com" ControlToValidate="txtCorreoElectronicoGarante" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ValidationGroup="GarantePatrimonioGroup"></asp:RegularExpressionValidator>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Facebook</label>

                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtFacebookGarante" runat="server" ValidationGroup="GarantePatrimonioGroup" class="form-control"></asp:TextBox>

                                                            </div>

                                                        </div>
                                                        <div class="form-group">
                                                            <div class="col-md-4">
                                                                <asp:Button ID="btnAgregarContactoGarante" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnAgregarContactoGarante_Click" />
                                                            </div>
                                                        </div>

                                                    </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>


                                        <%-- Fin Contacto Garante --%>

                                        <%-- Patrimonio Garante --%>

                                        <div class="tab-pane fade" id="patrimoniogarante">
                                            <asp:UpdatePanel ID="UpdatePanel10" UpdateMode="Always" runat="server">
                                                <ContentTemplate>
                                                    <div role="form" class="form-horizontal col-md-10">

                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Posee Vivienda Propia?</label>
                                                            <div class="col-md-4">
                                                                <asp:DropDownList ID="ListPoseeViviendaGarantePatrimonial" runat="server" class="form-control">
                                                                    <asp:ListItem Text="Si" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Domicilio: Calle </label>

                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtCallePatrimonialGarante" runat="server" class="form-control"></asp:TextBox>

                                                            </div>

                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">N°</label>

                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtDomicilioPatrimonialGarante" TextMode="Number" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                            </div>
                                                        </div>


                                                        <div class="form-group">
                                                            <label for="validateSelect" class="col-md-4">Provincia</label>
                                                            <div class="col-md-4">
                                                                <asp:DropDownList ID="ListProvinciaGarantePatrimoniales" class="form-control" runat="server" OnSelectedIndexChanged="ListProvinciaGarantePatrimoniales_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                                            </div>

                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator20" InitialValue="-1" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="ListProvinciaPatrimonial" ValidationGroup="PatrimonioGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>

                                                        <div class="form-group">
                                                            <label for="validateSelect" class="col-md-4">Localidad</label>
                                                            <div class="col-md-4">
                                                                <asp:DropDownList ID="ListLocalidadGarantePatrimoniales" OnSelectedIndexChanged="ListLocalidadGarantePatrimoniales_SelectedIndexChanged" AutoPostBack="True" class="btn btn-default ui-tooltip dropdown-toggle" runat="server"></asp:DropDownList>
                                                                <%--<asp:TextBox ID="TextBox1" runat="server" class="form-control"></asp:TextBox>--%>
                                                            </div>
                                                            <%--                                                    <div class="col-md-4">
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="ListLocalidad" ValidationGroup="DireccionGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>

                                                    </div>--%>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">CP </label>

                                                            <div class="col-md-4">

                                                                <asp:TextBox ID="txtCodigoPostalGarantePatrimonial" ReadOnly="true" runat="server" class="form-control"></asp:TextBox>

                                                            </div>

                                                        </div>
                                                        <div class="form-group">
                                                            <label for="validateSelect" class="col-md-4">Señale que tipo de vivienda</label>
                                                            <div class="col-md-4">
                                                                <asp:DropDownList ID="ListTiposViviendasGarante" class="form-control" runat="server"></asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Metros Aproximados</label>

                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtMetrosViviendaGarante" TextMode="Number" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Posee Algun Rodado Propio?</label>
                                                            <div class="col-md-4">
                                                                <asp:DropDownList ID="ListPoseeRodadoGarantePatrimonial" runat="server" class="form-control">
                                                                    <asp:ListItem Text="Si" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Modelo</label>

                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtModeloRodadoGarante" runat="server" class="form-control"></asp:TextBox>

                                                            </div>

                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Marca </label>

                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtMarcaRodadoGarante" runat="server" class="form-control"></asp:TextBox>

                                                            </div>

                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-4">Año</label>

                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtAñoRodadoGarante" TextMode="Number" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                            </div>

                                                        </div>
                                                        <div class="form-group">
                                                            <div class="col-md-4">
                                                                <asp:Button ID="btnAgregarPatrimonioGarante" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnAgregarPatrimonioGarante_Click" />
                                                            </div>
                                                        </div>

                                                    </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>

                                <%-- Fin Patrimonio Garante --%>

                                <%-- Fin Garante --%>
                                <%-- Documentacion --%>
                                <div class="tab-pane fade" id="Documentacion">
                                    <asp:UpdatePanel ID="UpdatePanel11" UpdateMode="Always" runat="server">
                                        <ContentTemplate>
                                            <div role="form" class="form-horizontal col-md-12">
                                                
                                                   
                                                <div class="form-group">
                                                    <label for="name" class="col-md-2">Adjuntar Fianza</label>
                                                    <div class="col-md-4">
                                                        <asp:FileUpload ID="FileFianza" runat="server" />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:LinkButton ID="btnAgregarArchivoFianza" runat="server" Text="<span class='shortcut-icon icon-upload'></span>" class="btn btn-info" OnClick="verificarBoton" />
                                                        <asp:LinkButton ID="btnDescargarArchivoFianza" Visible="false" runat="server" Text="<span class='shortcut-icon icon-download'></span>" class="btn btn-success" />
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-2">Adjuntar Pagare</label>
                                                    <div class="col-md-4">
                                                        <asp:FileUpload ID="FilePagare" runat="server" />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:LinkButton ID="btnAgregarArchivoPagare" runat="server" Text="<span class='shortcut-icon icon-upload'></span>" class="btn btn-info" OnClick="verificarBoton" />
                                                        <asp:LinkButton ID="btnDescargarArchivoPagare" Visible="false" runat="server" Text="<span class='shortcut-icon icon-download'></span>" class="btn btn-success" />
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-2">Adjuntar Contrato Comercial</label>
                                                    <div class="col-md-4">
                                                        <asp:FileUpload ID="FileContratoComercial" runat="server" />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:LinkButton ID="btnAgregarArchivoContratoComercial" runat="server" Text="<span class='shortcut-icon icon-upload'></span>" class="btn btn-info" OnClick="verificarBoton" />
                                                        <asp:LinkButton ID="btnDescargarArchivoContratoComercial" Visible="false" runat="server" Text="<span class='shortcut-icon icon-download'></span>" class="btn btn-success" />
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-2">Adjuntar Copia del DNI/LC/LE del DISTRIBUIDOR</label>
                                                    <div class="col-md-4">
                                                        <asp:FileUpload ID="FileDNIDistribuidor" runat="server" />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:LinkButton ID="btnAgregarArchivoDNIDistribuidor" runat="server" Text="<span class='shortcut-icon icon-upload'></span>" class="btn btn-info" OnClick="verificarBoton" />
                                                        <asp:LinkButton ID="btnDescargarArchivoDNIDistribuidor" Visible="false" runat="server" Text="<span class='shortcut-icon icon-download'></span>" class="btn btn-success" />
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-2">Adjuntar Copia del DNI/LC/LE del GARANTE</label>
                                                    <div class="col-md-4">
                                                        <asp:FileUpload ID="FileDNIGarante" runat="server" />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:LinkButton ID="btnAgregarArchivoDNIGarante" runat="server" Text="<span class='shortcut-icon icon-upload'></span>" class="btn btn-info" OnClick="verificarBoton" />
                                                        <asp:LinkButton ID="btnDescargarArchivoDNIGarante" Visible="false" runat="server" Text="<span class='shortcut-icon icon-download'></span>" class="btn btn-success" />
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-2">Adjuntar Copia de un servicio a nombre del DISTRIBUIDOR</label>
                                                    <div class="col-md-4">
                                                        <asp:FileUpload ID="FileServicioDistribuidor" runat="server" />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:LinkButton ID="btnAgregarArchivoServicioDistribuidor" runat="server" Text="<span class='shortcut-icon icon-upload'></span>" class="btn btn-info" OnClick="verificarBoton" />
                                                        <asp:LinkButton ID="btnDescargarArchivoServicioDistribuidor" Visible="false" runat="server" Text="<span class='shortcut-icon icon-download'></span>" class="btn btn-success" />
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <<label for="name" class="col-md-2">Adjuntar Copia de un servicio a nombre del GARANTE</label>
                                                    <div class="col-md-4">
                                                        <asp:FileUpload ID="FileServicioGarante" runat="server" />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:LinkButton ID="btnAgregarArchivoServicioGarante" runat="server" Text="<span class='shortcut-icon icon-upload'></span>" class="btn btn-info" OnClick="verificarBoton" />
                                                        <asp:LinkButton ID="btnDescargarArchivoServicioGarante" Visible="false" runat="server" Text="<span class='shortcut-icon icon-download'></span>" class="btn btn-success" />
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-2">Adjuntar Constancia de Inscripción en AFIP</label>
                                                    <div class="col-md-4">
                                                        <asp:FileUpload ID="FileConstanciaAFIP" runat="server" />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:LinkButton ID="btnAgregarArchivoConstanciaAFIP" runat="server" Text="<span class='shortcut-icon icon-upload'></span>" class="btn btn-info" OnClick="verificarBoton" />
                                                        <asp:LinkButton ID="btnDescargarArchivoConstanciaAFIP" Visible="false" runat="server" Text="<span class='shortcut-icon icon-download'></span>" class="btn btn-success" />
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-2">Adjuntar Copia del recibo de sueldo del garante</label>
                                                    <div class="col-md-4">
                                                        <asp:FileUpload ID="FileReciboSueldoGarante" runat="server" />
                                                    </div>
                                                    <div class="col-md-4">
                                                        <asp:LinkButton ID="btnAgregarArchivoReciboSueldoGarante" runat="server" Text="<span class='shortcut-icon icon-upload'></span>" class="btn btn-info" OnClick="verificarBoton" />
                                                        <asp:LinkButton ID="btnDescargarArchivoReciboSueldoGarante" Visible="false" runat="server" Text="<span class='shortcut-icon icon-download'></span>" class="btn btn-success" />
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <div class="shortcuts" style="height: 100%">
                                                        <label for="name" class="col-md-4">Adjuntar Descargar “Código de ética” y aceptar “Código de ética”</label>


                                                    </div>
                                                </div>
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                            <asp:PostBackTrigger ControlID="btnAgregarArchivoFianza" />
                                            <asp:PostBackTrigger ControlID="btnAgregarArchivoPagare" />
                                            <asp:PostBackTrigger ControlID="btnAgregarArchivoContratoComercial" />
                                            <asp:PostBackTrigger ControlID="btnAgregarArchivoDNIDistribuidor" />
                                            <asp:PostBackTrigger ControlID="btnAgregarArchivoDNIGarante" />
                                            <asp:PostBackTrigger ControlID="btnAgregarArchivoServicioDistribuidor" />
                                            <asp:PostBackTrigger ControlID="btnAgregarArchivoServicioGarante" />
                                            <asp:PostBackTrigger ControlID="btnAgregarArchivoConstanciaAFIP" />
                                            <asp:PostBackTrigger ControlID="btnAgregarArchivoReciboSueldoGarante" />

                                        </Triggers>
                                    </asp:UpdatePanel>
                                </div>
                                <%-- Fin Documentacion --%>
                            </div>
                        </div>
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
                                    <asp:Label runat="server" ID="lblMensaje" Text="Esta seguro que desea eliminar evento?" Style="text-align: center"></asp:Label>
                                </h5>
                            </div>

                            <div class="col-md-3">
                                <asp:TextBox runat="server" ID="txtMovimientoEventoCliente" Text="0" Style="display: none"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <%--<asp:Button runat="server" ID="btnSiEventoCliente" Style="display: none" Text="Eliminar" class="btn btn-danger" OnClick="btnSiEventoCliente_Click" />--%>
                        <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script src="../../Scripts/libs/jquery-1.9.1.min.js"></script>
    <script src="//code.jquery.com/ui/1.11.2/jquery-ui.js"></script>

    <script type="text/javascript">
        function myconfirmbox() {
            if (confirm("message")) {
                //trigger the button click
                <%--$('#' + '<%= btnSiEventoCliente.ClientID %>').click();--%>
                return true;
            }
            else {
                return false;
            }
        }
    </script>
    <script>

        <%--function abrirdialog(valor) {
            document.getElementById('<%= txtMovimientoEventoCliente.ClientID %>').value = valor;
            document.getElementById('<%= hiddenIdDireccion.ClientID %>').value = valor;
        }

        function abrirdialog2(valor) {
            document.getElementById('<%= hiddenIdProspecto.ClientID %>').value = valor;
        }--%>
</script>
    <script>
        function pageLoad() {
            $("#<%= txtFechaNacimiento.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%= txtFechaNacimientoGarante.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });

        };
    </script>

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

    <script type="text/javascript">

        ////Verifico en que tab estoy parado para cuando haga un PostaBack
        //$(function () {
        //    var tabName = $("[id*=TabName]").val() != "" ? $("[id*=TabName]").val() : "home";
        //    $('#myTab a[href="#' + tabName + '"]').tab('show');
        //    $("#myTab a").click(function () {
        //        $("[id*=TabName]").val($(this).attr("href").replace("#", ""));
        //    });
        //});

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
                else { return false; }
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

    <script>
        //valida los campos solo numeros
        function validarNroGuion(e) {
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
                if (key == 45) // Detectar  guion medio (-)
                { return true; }
                else { return false; }
            }
            return true;
        }

        function ObtenerRegistrosYLLenarTablaIIBB() {
            var controlHiddenIdCliente = document.getElementById('<%= hiddenIdCliente.ClientID %>');

            $.ajax({
                type: "POST",
                url: "ClientesABM.aspx/ObtenerRegistrosIIBBProvinciaByCliente",
                data: '{ IdCliente: "' + controlHiddenIdCliente.value + '"}',
                contentType: "application/json",
                dataType: 'json',
                error: (error) => {
                    console.log(JSON.stringify(error));
                    $.msgbox("No se pudo filtrar !", { type: "error" });
                }
                ,
                success: CargarTablaIIBB
            });
            return false;
        }

        function CargarTablaIIBB(response) {
            var data = response.d;
            var obj = JSON.parse(data);

            if (obj == "-2") {
                $.msgbox("Provincia ya existente", { type: "error" });
                return false;
            }

            $('#tabla_IngresosBrutos').find("tr:gt(0)").remove();

            for (var i = 0; i < obj.length; i++) {
                var percepcionORetencion = 0;
                percepcionORetencion = obj[i].Retencion;
                if (hiddenOrigenCliente.value == "1") {
                    percepcionORetencion = obj[i].Percepcion;
                }
                $('#tabla_IngresosBrutos').append(
                    "<tr>" +
                    "<td> " + obj[i].Provincia + "</td>" +
                    '<td style="text-align:right">' + percepcionORetencion + "</td>" +
                    '<td style="text-align:right">' + obj[i].Modo + "</td>" +
                    '<td style="text-align:right"> <a "id = ' + obj[i].Id + ' class= "btn btn-danger" autopostback="false" onclick="javascript: return EliminarRegistroDeTabla(' + obj[i].Id + ',' + obj[i].IdCliente + ')"><span class="shortcut-icon icon-trash"></span></a></td>' +
                    "</tr> ");
            };
        }

        function EliminarRegistroDeTabla(IdIIBBProvincia, IdCliente) {
            $.ajax({
                type: "POST",
                url: "ClientesABM.aspx/EliminarRegistroIIBBProvincia",
                data: '{ IdIIBBProvincia: "' + IdIIBBProvincia + '", IdCliente: "' + IdCliente + '"}',
                contentType: "application/json",
                dataType: 'json',
                error: (error) => {
                    console.log(JSON.stringify(error));
                    $.msgbox("No se pudo filtrar !", { type: "error" });
                }
                ,
                success: CargarTablaIIBB
            });
        }
    </script>

    <script>

        function mensajeError() {
            $.msgbox("Debe rellenar los datos personales primero.");
        }

        function mensajeAlert(mensaje) {
            $.msgbox(mensaje, { type: "alert" });
        }

        function mensajeAgregado() {
            $.msgGrowl({
                type: 'success'
                , title: 'Guardado'
                , text: 'Se agrego con exito!.'
            });
        }

        function mensajeEditado() {
            $.msgGrowl({
                type: 'success'
                , title: 'Editado'
                , text: 'Se edito con exito!.'
            });
        }

        function mensajeErrorCatch(mensaje) {
            $.msgGrowl({
                type: 'error'
                , title: 'Editado'
                , text: mensaje
            });
        }

        function mensajeEliminado() {
            $.msgGrowl({
                type: 'success'
                , title: 'Eliminado'
                , text: 'Se elimino con exito!.'
            });
        }



    </script>


</asp:Content>
