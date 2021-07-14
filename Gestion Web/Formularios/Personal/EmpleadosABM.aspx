<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EmpleadosABM.aspx.cs" EnableEventValidation="false" Inherits="Gestion_Web.Formularios.Personal.EmpleadosABM" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">


        <div class="row">

            <div class="col-md-12">

                <div class="widget stacked ">

                    <div class="widget-header">
                        <i class="icon-pencil"></i>
                        <h3>Empleados</h3>

                    </div>
                    <!-- /.widget-header -->


                    <div class="widget-content">

                        <div class="bs-example">
                            <ul id="myTab" class="nav nav-tabs">
                                <li class="active"><a href="#home" data-toggle="tab">Empleados</a></li>
                            </ul>
                            <div class="tab-content">
                                <div class="tab-pane fade active in" id="home">
                                    <form action="/" id="validation-form" role="form" class="form-horizontal col-md-7">
                                        <fieldset>
                                            <div class="form-group">
                                                <label for="name" class="col-md-4">Legajo</label>

                                                <div class="col-md-4">

                                                    <asp:TextBox ID="txtLegajo" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>

                                                    <%--<input type="text" class="form-control" name="name" id="name">--%>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:RequiredFieldValidator ControlToValidate="txtLegajo" ID="RequiredFieldValidator1" runat="server" ErrorMessage="El campo es obligatorio" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="form-group">
                                                <label for="name" class="col-md-4">Nombre</label>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="txtNombre" runat="server" class="form-control"></asp:TextBox>
                                                    <%--<input type="text" class="form-control" name="name" id="name">--%>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtNombre" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="form-group">
                                                <label for="name" class="col-md-4">Apellido</label>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="txtApellido" runat="server" class="form-control"></asp:TextBox>
                                                    <%--<input type="text" class="form-control" name="name" id="name">--%>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtApellido" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <br />

                                            <div class="form-group">
                                                <label id="labelDNI" runat="server" for="name" class="col-md-4">DNI</label>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="txtDni" runat="server" MaxLength="8" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                    <%--<input type="text" class="form-control" name="name" id="name">--%>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtDni" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="form-group">
                                                <label for="name" class="col-md-4">Dirección</label>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="txtDireccion" runat="server" class="form-control"></asp:TextBox>
                                                    <%--<input type="text" class="form-control" name="name" id="name">--%>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtDireccion" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="form-group">
                                                <label for="name" class="col-md-4">Fecha Nacimiento</label>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="txtFechaNacimiento" runat="server" class="form-control"></asp:TextBox>
                                                </div>

                                                <div class="col-md-4">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtFechaNacimiento" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="form-group">
                                                <label for="name" class="col-md-4">Fecha Ingreso</label>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="txtFechaIngreso" runat="server" name="fecha" class="form-control"></asp:TextBox>
                                                </div>

                                                <div class="col-md-4">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtFechaIngreso" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="form-group">
                                                <label id="labelCUIT" runat="server" for="name" class="col-md-4">CUIT</label>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="txtCuit" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)" MaxLength="11"></asp:TextBox>

                                                </div>
                                                <div class="col-md-4">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtCuit" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:RegularExpressionValidator ControlToValidate="txtCuit" ID="RegularExpressionValidator4" runat="server" ErrorMessage="Formato de CUIT Invalido" ValidationExpression="^\d{2}\d{8}\d{1}$" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RegularExpressionValidator>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="form-group">
                                                <label for="name" class="col-md-4">Remuneracion</label>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="txtRemuneracion" runat="server" Text="0" class="form-control" onkeypress="javascript:return validarNro(event)" style="text-align:right;"></asp:TextBox>
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtRemuneracion" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="form-group">
                                                <label for="name" class="col-md-4">Observaciones</label>
                                                <div class="col-md-4">
                                                    <asp:TextBox ID="txtObservaciones" runat="server" class="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                                    <%--<input type="text" class="form-control" name="name" id="name">--%>
                                                </div>
                                            </div>
                                            <br />
                                            
                                            <%--<div class="form-group">
                                                <label for="validateSelect" class="col-md-4">Perfil</label>
                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="DropListProveedor" runat="server" class="form-control"></asp:DropDownList>
                                                </div>
                                                <%--<div class="col-md-4">
                                                    <a class="btn btn-info" data-toggle="modal" href="../Proveedores/ProoveedoresABM.aspx">Agregar Proveedor</a>
                                                </div>--%>


                                            <br />


                                        </fieldset>
                                    </form>
                                </div>

                            </div>

                            <div class="col-md-8">
                                <asp:Button ID="btnAgregar" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnAgregar_Click" />
                                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" class="btn btn-default" PostBackUrl="~/Formularios/Personal/Empleados.aspx" />
                            </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
    </div>
    <%--roww--%>
        

    

    
 </div>

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

    <%--<script src="//code.jquery.com/jquery-1.9.1.js"></script>--%>
    <script src="//code.jquery.com/ui/1.10.4/jquery-ui.js"></script>

    <script>


        $(function () {
            $("#<%= txtFechaNacimiento.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

        $(function () {
            $("#<%= txtFechaIngreso.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });




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
                //if (key == 46 || key == 8) // Detectar . (punto) y backspace (retroceso)
                //{
                //    return true;
                //}
                //else
                //{
                return false;
                //}
            }
            return true;
        }
    </script>

</asp:Content>
