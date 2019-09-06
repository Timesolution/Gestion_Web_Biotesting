<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PuntoVentaABM.aspx.cs" Inherits="Gestion_Web.Formularios.Sucursales.PuntoVentaABM" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">


        <div class="row">

            <div class="col-md-12">

                <div class="widget stacked">

                    <div class="widget-header">
                        <i class="icon-pencil"></i>
                        <h3>Punto de Venta</h3>

                    </div>
                    <!-- /.widget-header -->

                    <div class="widget-content">

                        <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Always" runat="server">
                            <ContentTemplate>
                                <div id="validation-form" role="form" class="form-horizontal col-md-10">
                                    <fieldset>
                                        <div class="form-group">
                                            <label for="name" class="col-md-4">Empresa</label>

                                            <div class="col-md-3">

                                                <asp:TextBox ID="txtEmpresa" runat="server" class="form-control"></asp:TextBox>

                                                <%--<input type="text" class="form-control" name="name" id="name">--%>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:RequiredFieldValidator ControlToValidate="txtEmpresa" ID="RequiredFieldValidator3" runat="server" ErrorMessage="El campo es obligatorio" ValidationGroup="PVGroup"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-4">Punto de Venta</label>

                                            <div class="col-md-3">

                                                <asp:TextBox ID="txtPuntoVta" runat="server" class="form-control"></asp:TextBox>

                                                <%--<input type="text" class="form-control" name="name" id="name">--%>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:RequiredFieldValidator ControlToValidate="txtPuntoVta" ID="RequiredFieldValidator4" runat="server" ErrorMessage="El campo es obligatorio" ValidationGroup="PVGroup"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-4">Forma de Facturar</label>

                                            <div class="col-md-3">

                                                <asp:DropDownList ID="ddlFormaFactura" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFormaFactura_SelectedIndexChanged">
                                                    <asp:ListItem>Electronica</asp:ListItem>
                                                    <asp:ListItem>Preimpresa</asp:ListItem>
                                                    <asp:ListItem>Fiscal</asp:ListItem>
                                                </asp:DropDownList>

                                                <%--<input type="text" class="form-control" name="name" id="name">--%>
                                            </div>
                                            <asp:Panel ID="panelFiscal" runat="server" Visible="false">
                                                <label for="name" class="col-md-2">Tope facturacion</label>
                                                <div class="col-md-2">

                                                    <asp:TextBox ID="txtTope" runat="server" Text="0" class="form-control"></asp:TextBox>

                                                    <asp:RequiredFieldValidator ControlToValidate="txtTope" ID="RequiredFieldValidator5" runat="server" ErrorMessage="*" SetFocusOnError="true" ForeColor="Red" ValidationGroup="PVGroup"></asp:RequiredFieldValidator>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                        
                                        <div class="form-group">
                                            <label for="name" class="col-md-4">Moneda Facturacion</label>
                                            <div class="col-md-3">
                                                <asp:DropDownList ID="DropDownListMonedaFacturacion" runat="server" class="form-control" AutoPostBack="true"></asp:DropDownList>
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="name" class="col-md-4">Retiene Ganancias</label>

                                            <div class="col-md-3">

                                                <asp:DropDownList ID="ddlRetieneGanancias" runat="server" class="form-control">
                                                    <asp:ListItem>Si</asp:ListItem>
                                                    <asp:ListItem>No</asp:ListItem>
                                                </asp:DropDownList>

                                                <%--<input type="text" class="form-control" name="name" id="name">--%>
                                            </div>

                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-4">Retiene Ingresos Brutos</label>

                                            <div class="col-md-3">

                                                <asp:DropDownList ID="ddlRetIngresosBrutos" runat="server" class="form-control">
                                                    <asp:ListItem>Si</asp:ListItem>
                                                    <asp:ListItem>No</asp:ListItem>
                                                </asp:DropDownList>

                                                <%--<input type="text" class="form-control" name="name" id="name">--%>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-4">Nombre de Fantasia</label>

                                            <div class="col-md-3">

                                                <asp:TextBox ID="txtNombreFantasia" runat="server" class="form-control"></asp:TextBox>

                                                <%--<input type="text" class="form-control" name="name" id="name">--%>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:RequiredFieldValidator ControlToValidate="txtNombreFantasia" ID="RequiredFieldValidator1" runat="server" ErrorMessage="El campo es obligatorio" SetFocusOnError="true" ForeColor="Red" ValidationGroup="PVGroup"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-4">Direccion</label>

                                            <div class="col-md-3">

                                                <asp:TextBox ID="txtDireccion" runat="server" class="form-control"></asp:TextBox>

                                                <%--<input type="text" class="form-control" name="name" id="name">--%>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:RequiredFieldValidator ControlToValidate="txtDireccion" ID="RequiredFieldValidator2" runat="server" ErrorMessage="El campo es obligatorio" SetFocusOnError="true" ForeColor="Red" ValidationGroup="PVGroup"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>   
                                        <div class="form-group">
                                            <label for="name" class="col-md-4">CAI Remito</label>

                                            <div class="col-md-3">

                                                <asp:TextBox ID="txtCAIRemito" runat="server" class="form-control"></asp:TextBox>

                                                <%--<input type="text" class="form-control" name="name" id="name">--%>
                                            </div>
                                            <div class="col-md-4">
                                                
                                            </div>
                                        </div> 

                                        <div class="form-group">
                                            <label for="name" class="col-md-4">CAI Vencimiento</label>
                                            <div class="col-md-3">
                                                <asp:TextBox ID="txtCAIVencimiento" runat="server" class="form-control"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4">
                                                
                                            </div>
                                        </div> 

                                        <asp:Panel ID="panelContacto" runat="server" Visible="false">
                                            <div class="form-group">
                                                <label for="name" class="col-md-4">Datos contacto</label>                                            
                                            </div> 
                                            <div class="form-group">
                                                <label for="name" class="col-md-4">Mail:</label>
                                                <div class="col-md-3">
                                                    <asp:TextBox ID="txtMailPtoVenta" runat="server" type="mail" class="form-control"></asp:TextBox>                                                
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="name" class="col-md-4">Telefono:</label>
                                                <div class="col-md-3">
                                                    <asp:TextBox ID="txtTelPtoVenta" runat="server" class="form-control"></asp:TextBox>                                                
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
                            <asp:Button ID="btnAgregar" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnAgregar_Click" ValidationGroup="PVGroup" />
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" class="btn btn-default" PostBackUrl="../Sucursales/ABMPuntoVenta.aspx" />
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
