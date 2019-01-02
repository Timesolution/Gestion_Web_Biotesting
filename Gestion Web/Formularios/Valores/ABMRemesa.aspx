<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ABMRemesa.aspx.cs" Inherits="Gestion_Web.Formularios.Valores.ABMRemesa" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">
        <asp:UpdatePanel ID="UpdatePanel5" UpdateMode="Always" runat="server">
            <ContentTemplate>
                <div class="container">
                    <div class="row">

                        <div class="col-md-12">

                            <div class="widget stacked">
                                <div class="stat">
                                    <h5><i class="icon-map-marker"></i>Maestro > Valores > Remesas</h5>
                                    <asp:Label ID="lblFiltroAnterior" runat="server" Style="display: none;"></asp:Label>
                                </div>
                                <div class="widget-header">
                                    <i class="icon-pencil"></i>
                                    <h3>Orden de reparacion</h3>

                                </div>
                                <!-- /.widget-header -->
                                <div class="widget-content">
                                    <div class="bs-example">
                                        <div class="tab-content">
                                            <div class="tab-pane fade active in" id="home">
                                                <div id="validation-form" role="form" class="form-horizontal col-md-10">
                                                    <fieldset>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-3">Numero de Remesa</label>
                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtNumeroRemesa" runat="server" Enabled="false"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtNumeroRemesa" ValidationGroup="StoreGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-3">Fecha de entrega</label>
                                                            <div class="col-md-4">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon"><i class="icon-calendar"></i></span>
                                                                    <asp:TextBox ID="txtFecha" class="form-control" runat="server"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFecha" ValidationGroup="StoreGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                                        </div>

                                                        <div class="form-group">
                                                            <label for="name" class="col-md-3">Entrega</label>
                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtEntrega" runat="server" class="form-control"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtEntrega" ValidationGroup="StoreGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-3">Recibe</label>
                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtRecibe" runat="server" class="form-control"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtRecibe" ValidationGroup="StoreGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>

                                                        <div class="form-group">
                                                            <label for="name" class="col-md-3">Observacion</label>
                                                            <div class="col-md-4">
                                                                <asp:TextBox ID="txtObservacion" runat="server" class="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtObservacion" ValidationGroup="StoreGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                        </div>

                                                        <div class="form-group">
                                                            <label for="name" class="col-md-3">Billetes 1000</label>
                                                            <div class="col-md-3">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">N°</span>
                                                                    <asp:TextBox ID="txt1000Cant" Text="0" runat="server" Style="text-align: right;" class="form-control" onkeypress="javascript:return validarNro(event)" AutoPostBack="True" OnTextChanged="txt1000Cant_TextChanged"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">$</span>
                                                                    <asp:TextBox ID="txt1000Total" Text="0" runat="server" Style="text-align: right;" class="form-control" disabled></asp:TextBox>

                                                                </div>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txt1000Cant" ValidationGroup="StoreGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txt1000Total" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="StoreGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                                            </div>
                                                        </div>

                                                        <div class="form-group">
                                                            <label for="name" class="col-md-3">Billetes 500</label>
                                                            <div class="col-md-3">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">N°</span>
                                                                    <asp:TextBox ID="txt500Cant" Text="0" runat="server" Style="text-align: right;" class="form-control" onkeypress="javascript:return validarNro(event)" AutoPostBack="True" OnTextChanged="txt500Cant_TextChanged"></asp:TextBox><%--OnTextChanged="txt1000Cant_TextChanged"--%>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">$</span>
                                                                    <asp:TextBox ID="txt500Total" Text="0" runat="server" Style="text-align: right;" class="form-control" disabled></asp:TextBox>

                                                                </div>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txt500Cant" ValidationGroup="StoreGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txt500Total" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="StoreGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                                            </div>
                                                        </div>

                                                        <div class="form-group">
                                                            <label for="name" class="col-md-3">Billetes 200</label>
                                                            <div class="col-md-3">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">N°</span>
                                                                    <asp:TextBox ID="txt200Cant" Text="0" runat="server" Style="text-align: right;" class="form-control" onkeypress="javascript:return validarNro(event)" AutoPostBack="True" OnTextChanged="txt200Cant_TextChanged"></asp:TextBox><%--OnTextChanged="txt1000Cant_TextChanged"--%>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">$</span>
                                                                    <asp:TextBox ID="txt200Total" Text="0" runat="server" Style="text-align: right;" class="form-control" disabled></asp:TextBox>

                                                                </div>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txt200Cant" ValidationGroup="StoreGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txt200Total" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="StoreGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-3">Billetes 100</label>
                                                            <div class="col-md-3">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">N°</span>
                                                                    <asp:TextBox ID="txt100Cant" Text="0" runat="server" Style="text-align: right;" class="form-control" onkeypress="javascript:return validarNro(event)" AutoPostBack="True" OnTextChanged="txt100Cant_TextChanged"></asp:TextBox><%--OnTextChanged="txt1000Cant_TextChanged"--%>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">$</span>
                                                                    <asp:TextBox ID="txt100Total" Text="0" runat="server" Style="text-align: right;" class="form-control" disabled></asp:TextBox>

                                                                </div>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txt100Cant" ValidationGroup="StoreGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txt100Total" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="StoreGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-3">Billetes 50</label>
                                                            <div class="col-md-3">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">N°</span>
                                                                    <asp:TextBox ID="txt50Cant" Text="0" runat="server" Style="text-align: right;" class="form-control" onkeypress="javascript:return validarNro(event)" AutoPostBack="True" OnTextChanged="txt50Cant_TextChanged"></asp:TextBox><%--OnTextChanged="txt1000Cant_TextChanged"--%>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">$</span>
                                                                    <asp:TextBox ID="txt50Total" Text="0" runat="server" Style="text-align: right;" class="form-control" disabled></asp:TextBox>

                                                                </div>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txt50Cant" ValidationGroup="StoreGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="txt50Total" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="StoreGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-3">Billetes 20</label>
                                                            <div class="col-md-3">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">N°</span>
                                                                    <asp:TextBox ID="txt20Cant" Text="0" runat="server" Style="text-align: right;" class="form-control" onkeypress="javascript:return validarNro(event)" AutoPostBack="True" OnTextChanged="txt20Cant_TextChanged"></asp:TextBox><%--OnTextChanged="txt1000Cant_TextChanged"--%>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">$</span>
                                                                    <asp:TextBox ID="txt20Total" Text="0" runat="server" Style="text-align: right;" class="form-control" disabled></asp:TextBox>

                                                                </div>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txt20Cant" ValidationGroup="StoreGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="txt20Total" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="StoreGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-3">Billetes 10</label>
                                                            <div class="col-md-3">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">N°</span>
                                                                    <asp:TextBox ID="txt10Cant" Text="0" runat="server" Style="text-align: right;" class="form-control" onkeypress="javascript:return validarNro(event)" AutoPostBack="True" OnTextChanged="txt10Cant_TextChanged"></asp:TextBox><%--OnTextChanged="txt1000Cant_TextChanged"--%>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">$</span>
                                                                    <asp:TextBox ID="txt10Total" Text="0" runat="server" Style="text-align: right;" class="form-control" disabled></asp:TextBox>

                                                                </div>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txt10Cant" ValidationGroup="StoreGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator7" runat="server" ControlToValidate="txt10Total" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="StoreGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-3">Billetes 5</label>
                                                            <div class="col-md-3">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">N°</span>
                                                                    <asp:TextBox ID="txt5Cant" Text="0" runat="server" Style="text-align: right;" class="form-control" onkeypress="javascript:return validarNro(event)" AutoPostBack="True" OnTextChanged="txt5Cant_TextChanged"></asp:TextBox><%--OnTextChanged="txt1000Cant_TextChanged"--%>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">$</span>
                                                                    <asp:TextBox ID="txt5Total" Text="0" runat="server" Style="text-align: right;" class="form-control" disabled></asp:TextBox>

                                                                </div>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txt5Cant" ValidationGroup="StoreGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator8" runat="server" ControlToValidate="txt5Total" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="StoreGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                            <label for="name" class="col-md-3">Cambio 1</label>
                                                            <div class="col-md-3">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">N°</span>
                                                                    <asp:TextBox ID="txt1Cant" Text="0" runat="server" Style="text-align: right;" class="form-control" onkeypress="javascript:return validarNro(event)" AutoPostBack="True" OnTextChanged="txt1Cant_TextChanged"></asp:TextBox><%--OnTextChanged="txt1000Cant_TextChanged"--%>
                                                                </div>
                                                            </div>
                                                            <div class="col-md-3">
                                                                <div class="input-group">
                                                                    <span class="input-group-addon">$</span>
                                                                    <asp:TextBox ID="txt1Total" Text="0" runat="server" Style="text-align: right;" class="form-control" disabled></asp:TextBox>

                                                                </div>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txt1Cant" ValidationGroup="StoreGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                            </div>
                                                            <div class="col-md-4">
                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator9" runat="server" ControlToValidate="txt1Total" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato incorrecto" ValidationGroup="StoreGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" />
                                                            </div>
                                                        </div>
                                                        <div class="form-group">
                                                        <label for="name" class="col-md-3">Total Cargado</label>
                                                        <div class="col-md-3">
                                                            <div class="input-group">
                                                                <span class="input-group-addon">$</span>
                                                                <asp:TextBox ID="txtTotal" Text="0" runat="server" Style="text-align: right;" class="form-control" disabled></asp:TextBox>
                                                                <%--<input type="text" class="form-control" name="name" id="name">--%>
                                                            </div>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="Debe cargar billetes." InitialValue="0" ControlToValidate="txtTotal" ValidationGroup="StoreGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                        <div class="col-md-4">
                                                        </div>
                                                    </div>
                                                        <div class="col-md-8">
                                                            <asp:Button ID="btnAgregar" runat="server" Text="Agregar" class="btn btn-success" OnClick="btnAgregar_Click" ValidationGroup="StoreGroup"/>
                                                            <%--<asp:Button ID="btnGuardar" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnGuardar_Click" ValidationGroup="StoreGroup" Visible="false" />--%>
                                                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" class="btn btn-default" PostBackUrl="CajaF.aspx" />
                                                        </div>
                                                    </fieldset>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <link href="../../css/pages/reports.css" rel="stylesheet">
    <script src="../../Scripts/jquery-1.10.2.js"></script>

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

    <script src="../../Scripts/bootstrap.min.js"></script>

    <script>

        $(function () {
            $("#<%= txtFecha.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

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
