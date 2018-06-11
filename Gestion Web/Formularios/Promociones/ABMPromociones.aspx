<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ABMPromociones.aspx.cs" Inherits="Gestion_Web.Formularios.Promociones.ABMPromociones" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">
        <div class="col-md-12 col-xs-12">
            <div class="widget stacked">

                <div class="widget-header">
                    <i class="icon-wrench"></i>
                    <h3>Datos</h3>
                </div>
                <!-- /widget-header -->
                <div class="widget-content">
                    <div role="form" class="form-horizontal col-md-12">
                        <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Always" runat="server">
                            <ContentTemplate>
                                <div id="validation-form" role="form" class="form-horizontal col-md-10">
                                    <fieldset>
                                        <div class="form-group">
                                            <label for="name" class="col-md-2">Nombre</label>
                                            <div class="col-md-3">
                                                <asp:TextBox ID="txtPromocion" runat="server" class="form-control"></asp:TextBox>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="txtPromocion" ValidationGroup="PromoGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-2">Empresa</label>
                                            <div class="col-md-3">
                                                <asp:DropDownList ID="ListEmpresa" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ListEmpresa_SelectedIndexChanged" />
                                            </div>
                                            <div class="col-md-1">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" InitialValue="-1" ControlToValidate="ListEmpresa" ValidationGroup="PromoGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>
                                            <label for="name" class="col-md-2">Sucursal</label>
                                            <div class="col-md-3">
                                                <asp:DropDownList ID="ListSucursal" runat="server" class="form-control" />
                                            </div>
                                            <div class="col-md-1">
                                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="*" InitialValue="-1" ControlToValidate="ListSucursal" ValidationGroup="PromoGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                                <asp:Button ID="btnAgregarSucursal" runat="server" class="btn btn-success" Text="Agregar" OnClick="btnAgregarSucursal_Click" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-8"></label>
                                            <div class="col-md-3">
                                                <asp:ListBox ID="ListBoxSucursales" runat="server" class="form-control"></asp:ListBox>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:Button ID="btnEliminarSucursal" runat="server" class="btn btn-danger" Text="Eliminar" OnClick="btnEliminarSucursal_Click" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-2">Forma de pago</label>
                                            <div class="col-md-3">
                                                <asp:DropDownList ID="ListFormasPago" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ListFormasPago_SelectedIndexChanged" />
                                            </div>
                                            <div class="col-md-1">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="*" InitialValue="-1" ControlToValidate="ListFormasPago" ValidationGroup="PromoGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>
                                            <label for="name" class="col-md-2">Lista Precio</label>
                                            <div class="col-md-3">
                                                <asp:DropDownList ID="ListListasPrecio" runat="server" class="form-control" />
                                            </div>
                                            <div class="col-md-1">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="*" InitialValue="-1" ControlToValidate="ListListasPrecio" ValidationGroup="PromoGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-2">Tipo</label>
                                            <div class="col-md-3">
                                                <asp:DropDownList ID="ListTipoPromocion" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ListTipoPromocion_SelectedIndexChanged">
                                                    <asp:ListItem Text="Seleccione..." Value="-1" />
                                                    <asp:ListItem Text="Porcentual" Value="0" />
                                                    <asp:ListItem Text="Precio fijo" Value="1" />
                                                </asp:DropDownList>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="*" InitialValue="-1" ControlToValidate="ListTipoPromocion" ValidationGroup="PromoGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>
                                            <asp:Panel ID="PanelDto" runat="server" Visible="false">
                                                <label for="name" class="col-md-2">Descuento</label>
                                                <div class="col-md-3">
                                                    <div class="input-group">
                                                        <span class="input-group-addon">%</span>
                                                        <asp:TextBox ID="txtDescuento" runat="server" Text="0" Style="text-align: right;" class="form-control" onkeypress="javascript:return validarNro(event)" />
                                                    </div>
                                                </div>
                                                <div class="col-md-1">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="*" ControlToValidate="txtDescuento" ValidationGroup="PromoGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </asp:Panel>
                                            <asp:Panel ID="PanelPrecio" runat="server" Visible="false">
                                                <label for="name" class="col-md-2">Precio Final</label>
                                                <div class="col-md-3">
                                                    <div class="input-group">
                                                        <span class="input-group-addon">$</span>
                                                        <asp:TextBox ID="txtPrecio" runat="server" Text="0" Style="text-align: right;" class="form-control" onkeypress="javascript:return validarNro(event)" />
                                                    </div>
                                                </div>
                                                <div class="col-md-1">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="*" ControlToValidate="txtPrecio" ValidationGroup="PromoGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-2">Desde</label>
                                            <div class="col-md-3">
                                                <div class="input-group">
                                                    <span class="input-group-addon"><i class="icon-calendar"></i></span>
                                                    <asp:TextBox ID="txtDesde" runat="server" class="form-control" />
                                                </div>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="*" ControlToValidate="txtDesde" ValidationGroup="PromoGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>
                                            <label for="name" class="col-md-2">Hasta</label>
                                            <div class="col-md-3">
                                                <div class="input-group">
                                                    <span class="input-group-addon"><i class="icon-calendar"></i></span>
                                                    <asp:TextBox ID="txtHasta" runat="server" class="form-control" />
                                                </div>
                                            </div>
                                            <div class="col-md-1">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="*" ControlToValidate="txtDesde" ValidationGroup="PromoGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <asp:Panel ID="panelTarjetas" runat="server" Visible="false">
                                            <div class="form-group">
                                            <label for="name" class="col-md-2">Tarjetas</label>
                                                <div class="col-md-3">
                                                    <asp:DropDownList ID="ListTarjetas" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ListGrupos_SelectedIndexChanged" />
                                                </div>
                                                <div class="col-md-1">
                                                    <asp:Button ID="btnAgregarTarjetas" runat="server" class="btn btn-success" Text="Agregar" OnClick="btnAgregarTarjetas_Click" />                                                    
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="name" class="col-md-2"></label>
                                                <div class="col-md-3">
                                                    <asp:ListBox ID="ListBoxTarjetas" runat="server" class="form-control"></asp:ListBox>
                                                </div>
                                                <div class="col-md-1">
                                                    <asp:Button ID="btnEliminarTarjetas" runat="server" class="btn btn-danger" Text="Eliminar" OnClick="btnEliminarTarjetas_Click" />
                                                </div>
                                            </div>                                        
                                        </asp:Panel>
                                        <asp:Panel ID="panelArticulos" runat="server" Visible="false">
                                            <div class="form-group">
                                                <label for="name" class="col-md-2">Proveedor</label>
                                                <div class="col-md-3">
                                                    <asp:DropDownList ID="ListProveedores" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ListProveedores_SelectedIndexChanged" />
                                                </div>
                                                <div class="col-md-1">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="*" InitialValue="-1" ControlToValidate="ListProveedores" ValidationGroup="PromoGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                                <label for="name" class="col-md-2">Grupo</label>
                                                <div class="col-md-3">
                                                    <asp:DropDownList ID="ListGrupos" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ListGrupos_SelectedIndexChanged" />
                                                </div>
                                                <div class="col-md-1">
                                                    <asp:Button ID="btnAgregarGrupo" runat="server" class="btn btn-success" Text="Agregar" OnClick="btnAgregarGrupo_Click" />
                                                    
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="name" class="col-md-8"></label>
                                                <div class="col-md-3">
                                                    <asp:ListBox ID="ListBoxGrupos" runat="server" class="form-control"></asp:ListBox>
                                                </div>
                                                <div class="col-md-1">
                                                    <asp:Button ID="btnEliminarGrupo" runat="server" class="btn btn-danger" Text="Eliminar" OnClick="btnEliminarGrupo_Click" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="name" class="col-md-2">Articulo:</label>
                                                <div class="col-md-3">
                                                    <asp:TextBox ID="txtBuscarArticulo" runat="server" class="form-control" />
                                                </div>
                                                <div class="col-md-1">
                                                    <asp:LinkButton ID="btnBuscarCod" runat="server" Text="<i class='icon-search'></i>" class="btn btn-info" OnClick="btnBuscarCod_Click" />
                                                </div>
                                                <label for="name" class="col-md-1">Articulos</label>
                                                <div class="col-md-4">
                                                    <asp:DropDownList ID="ListArticulos" runat="server" class="form-control" />
                                                </div>
                                                <div class="col-md-1">
                                                    <asp:Button ID="btnAgregarArticulo" runat="server" class="btn btn-success" Text="Agregar" OnClick="btnAgregarArticulo_Click" />
                                                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="*" InitialValue="-1" ControlToValidate="ListArticulos" ValidationGroup="PromoGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="name" class="col-md-6"></label>
                                                <div class="col-md-5">
                                                    <asp:ListBox ID="ListBoxArticulos" runat="server" class="form-control"></asp:ListBox>
                                                </div>
                                                <div class="col-md-1">
                                                    <asp:Button ID="btnEliminarArticulo" runat="server" class="btn btn-danger" Text="Eliminar" OnClick="btnEliminarArticulo_Click" />
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="name" class="col-md-2">Cantidad</label>
                                                <div class="col-md-3">
                                                    <asp:TextBox ID="txtCantidad" runat="server" class="form-control" Text="0" onkeypress="javascript:return validarNro(event)" />
                                                </div>
                                                <div class="col-md-1">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="*" ControlToValidate="txtCantidad" ValidationGroup="PromoGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </asp:Panel>

                                        <br />
                                        <div class="form-group" style="margin-bottom: 0%;">
                                            <asp:Button ID="btnGuardar" Text="Guardar" runat="server" class="btn btn-success" OnClick="btnGuardar_Click" ValidationGroup="PromoGroup" />
                                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" class="btn btn-default" PostBackUrl="~/Formularios/Promociones/PromocionesF.aspx" />
                                        </div>
                                    </fieldset>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <!-- /widget-content -->
            </div>
            <!-- /widget -->
        </div>
    </div>
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

    <!-- Page-Level Plugin Scripts - Tables -->
    <script src="//cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>
    <script src="//cdn.datatables.net/plug-ins/1.10.9/sorting/date-eu.js"></script>
    <script src="../../Scripts/plugins/dataTables/custom.tables.js"></script>

    <link href="//cdn.datatables.net/1.10.2/css/jquery.dataTables.css" rel="stylesheet" />

    <script>
        function pageLoad() {
            $("#<%= txtDesde.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%= txtHasta.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
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


</asp:Content>


