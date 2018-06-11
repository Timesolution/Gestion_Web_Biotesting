<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProoveedoresABM.aspx.cs" Inherits="Gestion_Web.Formularios.Proveedores.ProoveedoresABM" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">


        <div class="row">

            <div class="col-md-12">

                <div class="widget stacked ">

                    <div class="widget-header">
                        <i class="icon-pencil"></i>
                        <h3>Proveedor</h3>

                    </div>
                    <!-- /.widget-header -->


                    <div class="widget-content">



                        <div class="bs-example">
                            <ul id="myTab" class="nav nav-tabs">
                                <li class="active"><a href="#home" data-toggle="tab">Datos Proveedor</a></li>
                                <li class=""><a href="#profile" data-toggle="tab">Direccion</a></li>
                                <li class=""><a href="#Contacto" data-toggle="tab">Contacto</a></li>
                                <li class=""><a href="#Expreso" data-toggle="tab">Expreso</a></li>
                            </ul>
                            <div class="tab-content">
                                <div class="tab-pane fade active in" id="home">
                                <form action="/" id="validation-form" role="form" class="form-horizontal col-md-7">
                                     <fieldset>
                                            <div class="form-group">
                                                <label for="name" class="col-md-4">Codigo Proveedor</label>

                                                <div class="col-md-6">

                                                    <asp:TextBox ID="txtCodCliente" runat="server" class="form-control"></asp:TextBox>
                                                    <%--<input type="text" class="form-control" name="name" id="name">--%>
                                                </div>
                                            </div>
                                            <br />
                                            <br />
                                            <div class="form-group">
                                                <label for="validateSelect" class="col-md-4">Tipo</label>
                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="DropListTipo" runat="server" class="form-control"></asp:DropDownList>
                                                </div>
                                                <div class="col-md-4">
                                                    <a class="btn btn-info" data-toggle="modal" href="#modalTipoCliente">Agregar Tipo Proveedor</a>
                                                </div>
                                            </div>
                                            <br />
                                            <br />
                                            <div class="form-group">
                                                <label for="name" class="col-md-4">Razon Social</label>

                                                <div class="col-md-4">

                                                    <asp:TextBox ID="txtRazonSocial" runat="server" class="form-control"></asp:TextBox>
                                                    <%--<input type="text" class="form-control" name="name" id="name">--%>
                                                </div>
                                            </div>
                                            <br />

                                            <div class="form-group">
                                                <label for="name" class="col-md-4">Alias</label>

                                                <div class="col-md-4">

                                                    <asp:TextBox ID="txtAlias" runat="server" class="form-control"></asp:TextBox>
                                                    <%--<input type="text" class="form-control" name="name" id="name">--%>
                                                </div>
                                            </div>
                                            <br />

                                            <div class="form-group">
                                                <label for="validateSelect" class="col-md-4">Grupo</label>
                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="DropListGrupo" runat="server" class="form-control"></asp:DropDownList>

                                                </div>
                                                <div class="col-md-4">
                                                    <a class="btn btn-info" data-toggle="modal" href="#modalGrupo">Agregar Grupo</a>
                                                </div>
                                            </div>
                                            <br />

                                            <div class="form-group">
                                                <label for="validateSelect" class="col-md-4">Categoria</label>
                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="DropListCategoria" runat="server" class="form-control"></asp:DropDownList>

                                                </div>
                                                <div class="col-md-4">
                                                    <a class="btn btn-info" data-toggle="modal" href="#modalCategoria">Agregar Categoria</a>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="form-group">
                                                <label for="validateSelect" class="col-md-4">Pais</label>
                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="DropListPais" runat="server" class="form-control"></asp:DropDownList>

                                                </div>
                                            </div>
                                            <br />
                                            <div class="form-group">
                                                <label for="name" class="col-md-4">Cuit</label>

                                                <div class="col-md-4">

                                                    <asp:TextBox ID="txtCuit" runat="server" class="form-control"></asp:TextBox>
                                                    <asp:TextBox ID="txtID" runat="server" class="form-control" Visible="False"></asp:TextBox>
                                                    <%--<input type="text" class="form-control" name="name" id="name">--%>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="form-group">
                                                <label for="validateSelect" class="col-md-4">Iva</label>
                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="DropListIva" runat="server" class="form-control"></asp:DropDownList>

                                                </div>
                                            </div>
                                            <br />

                                            <div class="form-group">
                                                <label for="name" class="col-md-4">Saldo Maximo</label>

                                                <div class="col-md-4">

                                                    <asp:TextBox ID="txtSaldoMaximo" runat="server" class="form-control"></asp:TextBox>
                                                    <%--<input type="text" class="form-control" name="name" id="name">--%>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="form-group">
                                                <label for="name" class="col-md-4">Vencimiento FC</label>

                                                <div class="col-md-4">

                                                    <asp:TextBox ID="txtVencFC" runat="server" class="form-control"></asp:TextBox>
                                                    <%--<input type="text" class="form-control" name="name" id="name">--%>
                                                </div>
                                            </div>

                                            <br />

                                            <div class="form-group">
                                                <label for="name" class="col-md-4">Descuento FC</label>

                                                <div class="col-md-4">

                                                    <asp:TextBox ID="txtDescFC" runat="server" class="form-control"></asp:TextBox>
                                                    <%--<input type="text" class="form-control" name="name" id="name">--%>
                                                </div>
                                            </div>

                                            <br />

                                            <div class="form-group">
                                                <label for="validateSelect" class="col-md-4">Estado</label>

                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="txtEstado" runat="server" class="form-control"></asp:DropDownList>

                                                </div>
                                            </div>

                                            <br />

                                            <div class="form-group">
                                                <label for="message" class="col-md-4">Observaciones</label>

                                                <div class="col-md-4">

                                                    <asp:TextBox ID="txtObservaciones" runat="server" class="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                                    <%--<input type="text" class="form-control" name="name" id="name">--%>
                                                </div>
                                            </div>

                                            <br />
                                            <br />
                                            <div class="form-group">
                                                <div class="col-md-4"></div>

                                                <div class="col-md-4">
                                                    <%-- <asp:Button ID="btnAgregar" runat="server" Text="Agregar" class="btn btn-success" OnClick="btnAgregar_Click" />
                                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" class="btn btn-default" />--%>
                                                </div>
                                            </div>
                                        </fieldset>
                                    </form>

                                </div>

                                <%--Direccion--%>
                                <div class="tab-pane fade" id="profile">

                                    <form action="/" role="form" class="form-horizontal col-md-7">

                                        <%--<div class="alert alert-warning">

							<p><strong>Hit validate below to view this themes awesome error messages: </strong> 

								&nbsp;&nbsp;
							<button class="btn btn-default"><i class="icon-ok"></i> Validate Form</button></p>

						</div> <!-- /.alert -->--%>


                                        <br />





                                        <fieldset>
                                            <div class="form-group">
                                                <label for="name" class="col-md-4">Nombre</label>

                                                <div class="col-md-4">

                                                    <asp:TextBox ID="txtDirecNombre" runat="server" class="form-control"></asp:TextBox>
                                                    <%--<input type="text" class="form-control" name="name" id="name">--%>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="name" class="col-md-4">Dirección</label>

                                                <div class="col-md-4">

                                                    <asp:TextBox ID="txtDirecDireccion" runat="server" class="form-control"></asp:TextBox>
                                                    <%--<input type="text" class="form-control" name="name" id="name">--%>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="form-group">
                                                <label for="validateSelect" class="col-md-4">Localidad</label>
                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="DropListLocalidad" runat="server" class="form-control"></asp:DropDownList>

                                                </div>
                                            </div>
                                            <br />
                                            <div class="form-group">
                                                <label for="validateSelect" class="col-md-4">Provincia</label>
                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="DropListProvincia" runat="server" class="form-control"></asp:DropDownList>

                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="validateSelect" class="col-md-4">Pais</label>
                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="DropListDirecPais" runat="server" class="form-control"></asp:DropDownList>

                                                </div>
                                            </div>
                                            <br />
                                            <div class="form-group">
                                                <label for="name" class="col-md-4">Codigo Postal</label>

                                                <div class="col-md-4">

                                                    <asp:TextBox ID="TextBox2" runat="server" class="form-control"></asp:TextBox>
                                                    <%--<input type="text" class="form-control" name="name" id="name">--%>
                                                </div>
                                            </div>
                                            <br />

                                        </fieldset>
                                    </form>


                                </div>
                                <%--Fin direccion--%>

                                <%-- Contactos --%>

                                <div class="tab-pane fade" id="Contacto">


                                    <form action="/" role="form" class="form-horizontal col-md-7">

                                        <%--<div class="alert alert-warning">

							<p><strong>Hit validate below to view this themes awesome error messages: </strong> 

								&nbsp;&nbsp;
							<button class="btn btn-default"><i class="icon-ok"></i> Validate Form</button></p>

						</div> <!-- /.alert -->--%>


                                        <br />

                                        <fieldset>
                                            <div class="form-group">
                                                <label for="name" class="col-md-4">Nombre</label>

                                                <div class="col-md-4">

                                                    <asp:TextBox ID="txtNombreContacto" runat="server" class="form-control"></asp:TextBox>
                                                    <%--<input type="text" class="form-control" name="name" id="name">--%>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="form-group">
                                                <label for="validateSelect" class="col-md-4">Tipo</label>
                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="DropListTipoContacto" runat="server" class="form-control"></asp:DropDownList>

                                                </div>
                                            </div>
                                            <br />
                                            <div class="form-group">
                                                <label for="name" class="col-md-4">Cargo</label>

                                                <div class="col-md-4">

                                                    <asp:TextBox ID="txtCargoContacto" runat="server" class="form-control"></asp:TextBox>
                                                    <%--<input type="text" class="form-control" name="name" id="name">--%>
                                                </div>
                                            </div>
                                            <br />

                                            <div class="form-group">
                                                <label for="name" class="col-md-4">Numero</label>

                                                <div class="col-md-4">

                                                    <asp:TextBox ID="txtNumeroContacto" runat="server" class="form-control"></asp:TextBox>
                                                    <%--<input type="text" class="form-control" name="name" id="name">--%>
                                                </div>
                                            </div>


                                            <br />
                                            <div class="form-group">
                                                <label for="name" class="col-md-4">Mail</label>

                                                <div class="col-md-4">

                                                    <asp:TextBox ID="txtMailContacto" runat="server" class="form-control"></asp:TextBox>
                                                    <%--<input type="text" class="form-control" name="name" id="name">--%>
                                                </div>
                                            </div>
                                            <br />



                                        </fieldset>
                                    </form>

                                </div>
                                <%-- Fin Contactos --%>

                                <%--Expreso--%>
                                <div class="tab-pane fade" id="Expreso">


                                    <form action="/" role="form" class="form-horizontal col-md-7">

                                        <%--<div class="alert alert-warning">

							<p><strong>Hit validate below to view this themes awesome error messages: </strong> 

								&nbsp;&nbsp;
							<button class="btn btn-default"><i class="icon-ok"></i> Validate Form</button></p>

						</div> <!-- /.alert -->--%>


                                        <br />

                                        <fieldset>
                                            <div class="form-group">
                                                <label for="name" class="col-md-4">Nombre</label>

                                                <div class="col-md-4">

                                                    <asp:TextBox ID="txtExprNombre" runat="server" class="form-control"></asp:TextBox>
                                                    <%--<input type="text" class="form-control" name="name" id="name">--%>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="form-group">
                                                <label for="validateSelect" class="col-md-4">Tipo</label>
                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="DropDownList1" runat="server" class="form-control"></asp:DropDownList>

                                                </div>
                                            </div>

                                            <br />

                                            <div class="form-group">
                                                <label for="name" class="col-md-4">Direccion</label>

                                                <div class="col-md-4">

                                                    <asp:TextBox ID="txtExpDireccion" runat="server" class="form-control"></asp:TextBox>
                                                    <%--<input type="text" class="form-control" name="name" id="name">--%>
                                                </div>
                                            </div>


                                            <br />
                                            <div class="form-group">
                                                <label for="name" class="col-md-4">Telefono</label>

                                                <div class="col-md-4">

                                                    <asp:TextBox ID="txtExpTelefono" runat="server" class="form-control"></asp:TextBox>
                                                    <%--<input type="text" class="form-control" name="name" id="name">--%>
                                                </div>
                                            </div>
                                            <br />

                                            <div class="form-group">
                                                <label for="name" class="col-md-4">Cuit</label>

                                                <div class="col-md-4">

                                                    <asp:TextBox ID="txtExprCuit" runat="server" class="form-control"></asp:TextBox>
                                                    <%--<input type="text" class="form-control" name="name" id="name">--%>
                                                </div>
                                            </div>
                                            <br />
                                        </fieldset>
                                    </form>

                                </div>
                                <%-- Expreso --%>
                                <div class="col-md-4">
                                    <asp:Button ID="btnAgregar" runat="server" Text="Guardar" class="btn btn-success"  />
                                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" class="btn btn-default" />

                                </div>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="modalTipoCliente" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Agregar Tipo Proveedor</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label for="validateSelect" class="col-md-4">Tipo</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtTipoCliente2" runat="server" class="form-control"></asp:TextBox>
                        </div>

                    </div>

                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnAgregarTipoCliente" runat="server" Text="Guardar" class="btn btn-success"  />
                </div>

            </div>
        </div>
    </div>  <%--Fin modalGrupo--%>

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
                            <asp:TextBox ID="txtGrupo2" runat="server" class="form-control"></asp:TextBox>
                        </div>

                    </div>

                </div>
                <div class="modal-footer">
                    <asp:Button ID="Button1" runat="server" Text="Guardar" class="btn btn-success"   />
                </div>

            </div>
        </div>
    </div>  <%--Fin modalGrupo--%>

    <div id="modalCategoria" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Agregar Categoria</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label for="validateSelect" class="col-md-4">Categoria</label>
                        <div class="col-md-4">
                            <asp:TextBox ID="txtCategoria2" runat="server" class="form-control"></asp:TextBox>
                        </div>

                    </div>

                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnAgregarCategoria" runat="server" Text="Guardar" class="btn btn-success" />
                </div>

            </div>
        </div>
    </div>  <%--Fin modalGrupo--%>

</asp:Content>
