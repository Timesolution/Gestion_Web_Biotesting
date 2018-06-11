<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="VendedoresABM.aspx.cs" Inherits="Gestion_Web.Formularios.Vendedores.VendedoresABM" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">


        <div class="row" style="padding-left: 2%; padding-right: 2%;">

            <div class="col-md-12">

                <div class="widget stacked ">

                    <div class="widget-header">
                        <i class="icon-pencil"></i>
                        <h3>Vendedores</h3>

                    </div>
                    <!-- /.widget-header -->


                    <div class="widget-content">
                        <%--                        <asp:UpdatePanel ID="UpdateVendedores" runat="server" UpdateMode="Always">
                            <ContentTemplate>--%>
                        <table id="tbClientes" class="table table-striped table-bordered">
                            <thead>
                                <tr>
                                    <th style="width: 20%" id="tdLegajo">Legajo</th>
                                    <th style="width: 30%" id="tdNombre">Nombre</th>
                                    <th style="width: 30%" id="tdApellido">Apellido</th>

                                    <th class="td-actions" style="width: 20%" id="tdAction"></th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:PlaceHolder ID="phVendedores" runat="server"></asp:PlaceHolder>
                            </tbody>
                        </table>

                        <form action="/" role="form" class="form-horizontal col-md-7">
                            <asp:Label runat="server" Visible="false" ID="lblIdVendedor"></asp:Label>
                            <div class="form-group">
                                <label for="name" class="col-md-4">Legajo</label>

                                <div class="col-md-4">
                                    <input id="tLegajo" runat="server" class="form-control" disabled />

                                    <%--<asp:TextBox ID="txtLegajo" runat="server" class="form-control" disabled></asp:TextBox>--%>

                                    <%--<input type="text" class="form-control" name="name" id="name">--%>
                                </div>
                                <div class="col-md-4">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Debe seleccionar un empleado" ControlToValidate="tLegajo" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" ValidationGroup="VendedorGroup"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <br />
                            <br />
                            <div class="form-group">
                                <label for="name" class="col-md-4">Nombre</label>

                                <div class="col-md-4">

                                    <input id="tNombre" runat="server" class="form-control" disabled />
                                </div>
                                <div class="col-md-4">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Debe seleccionar un empleado" ControlToValidate="tNombre" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" ValidationGroup="VendedorGroup"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <br />
                            <br />
                            <div class="form-group">
                                <label for="name" class="col-md-4">Apellido</label>

                                <div class="col-md-4">

                                    <input id="tApellido" runat="server" class="form-control" disabled />
                                </div>
                                <div class="col-md-4">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe seleccionar un empleado" ControlToValidate="tApellido" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" ValidationGroup="VendedorGroup"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <br />
                            <br />
                            <div class="form-group">
                                <label for="name" class="col-md-4">Comisión</label>

                                <div class="col-md-4">

                                    <asp:TextBox ID="txtComision" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>

                                </div>
                                <div class="col-md-4">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtComision" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" ValidationGroup="VendedorGroup"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-4">
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtComision" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="VendedorGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                </div>
                            </div>
                            <br />
                            <asp:UpdatePanel ID="UpdatePanelComision" UpdateMode="Always" runat="server" Visible="false">
                                <ContentTemplate>
                                    <div class="form-group">
                                        <label for="name" class="col-md-4">Comisiones por Grupo</label>

                                        <div class="col-md-3">
                                            <asp:DropDownList ID="ListGruposArticulos" class="form-control" runat="server">
                                            </asp:DropDownList>
                                            <div class="col-md-3">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator35" runat="server" ErrorMessage="*" ControlToValidate="ListGruposArticulos" InitialValue="-1" ValidationGroup="GruposComisionesGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:LinkButton ID="btnAgregarGrupo" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" ValidationGroup="GruposComisionesGroup" OnClick="btnAgregarGrupo_Click" />
                                        </div>
                                    </div>
                                    <br />
                                    <br />
                                    <br />
                                    <div class="form-group">
                                        <label for="name" class="col-md-4"></label>

                                        <div class="col-md-3">
                                            <asp:ListBox ID="ListBoxGruposComisiones" runat="server" class="form-control"></asp:ListBox>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:LinkButton ID="btnQuitarGrupoComision" runat="server" Text="<span class='shortcut-icon icon-trash'></span>" class="btn btn-danger" OnClick="btnQuitarGrupoComision_Click" />
                                        </div>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <br />
                            <br />
                            <br />
                            <br />
                            <div class="form-group">
                                <label for="name" class="col-md-4">Sucursal</label>
                                <div class="col-md-3">
                                    <asp:DropDownList ID="DropListSucursal" class="form-control" runat="server">
                                        <%--<asp:ListItem Text="Seleccione..." Value="-1"></asp:ListItem>        --%>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Debe seleccionar una sucursal" InitialValue="-1" ControlToValidate="DropListSucursal" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" ValidationGroup="VendedorGroup"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <br />
                            <br />
                            <div class="col-md-4">
                                <div class="col-md-4">
                                    <asp:Button ID="btnAgregar" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnAgregar_Click" ValidationGroup="VendedorGroup" />
                                </div>
                                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" class="btn btn-default" PostBackUrl="~/Formularios/Vendedores/Vendedores.aspx" />
                            </div>
                            <%--                            </ContentTemplate>
                        </asp:UpdatePanel>--%>
                    </div>
                </div>
            </div>
        </div>
        <%--roww--%>
    </div>

    <script src="../../Scripts/libs/jquery-1.9.1.min.js"></script>
    <script src="../../Scripts/libs/jquery-ui-1.10.0.custom.min.js"></script>

    <script src="../../Scripts/plugins/msgGrowl/js/msgGrowl.js"></script>
    <script src="../../Scripts/plugins/lightbox/jquery.lightbox.min.js"></script>
    <script src="../../Scripts/plugins/msgbox/jquery.msgbox.min.js"></script>

    <script src="../../Scripts/Application.js"></script>

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

    <script>
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endReq);
        function endReq(sender, args) {

        }
    </script>

    <script type="text/javascript">
        function DisableButton() {
            document.getElementById("<%=btnAgregar.ClientID %>").disabled = true;
            document.getElementById("<%=btnCancelar.ClientID %>").disabled = true;

        }
        window.onbeforeunload = DisableButton;
    </script>

</asp:Content>
