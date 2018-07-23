<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PanelAlertasSMS.aspx.cs" Inherits="Gestion_Web.Formularios.Herramientas.PanelAlertasSMS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">

        <div class="row">
            <div class="col-md-12">

                <div class="widget stacked">
                    <div class="stat">
                        <h5><i class="icon-map-marker"></i> Herramientas > Alertas</h5>
                    </div>
                    <div class="widget-header">
                        <i class="icon-pencil"></i>
                        <h3>Configuraciones</h3>

                    </div>
                    <!-- /.widget-header -->

                    <div class="widget-content">
                        <div class="form-horizontal col-md-12">
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                                <ContentTemplate>
                                    <div class="form-group">
                                        <label class="col-md-4">¿Desea activar el envio de SMS de aviso en el sistema?:</label>
                                        <div class="col-md-4">
                                            <asp:LinkButton ID="lbtnDesactivarSMS" runat="server" class="btn btn-danger" Text="Desactivar" OnClick="lbtnDesactivarSMS_Click" Visible="false"></asp:LinkButton>
                                        </div>
                                    </div>
                                    <asp:Panel ID="PanelCondiciones" runat="server" Style="margin-top: 0px;">
                                        <div class="form-group">
                                            <div class="alert alert-warning alert-dismissable col-md-8">
                                                <p>
                                                    Costo del servicio u$s 0.15 por SMS. Cobro solo por uso. La activación no implica costo alguno. Se facturará a mes vencido la cantidad de SMS enviados. Antes del envío de cada SMS el sistema válida la longitud y el formato de cada número telefónico (10 dígitos). No validando que el número ingresado exista o esta activo. Time Solution SRL no se responsabiliza por el contenido de cada SMS.
                                                </p>
                                                <p>
                                                    El servicio se brindara exclusivamente a aquellos clientes que acepten los términos y condiciones establecidos en el presente.
                                                </p>
                                                <p>
                                                    Para la desactivación del servicio el usuario deberá simplemente desactivar la opción de envío de SMS desde este mismo formulario. Anulando desde ese momento el envío de SMS. Los SMS enviados al momento se facturarán a mes vencido.
                                                </p>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-8">
                                                <asp:CheckBox ID="chkAceptaCondiciones" runat="server" Text=" &nbsp Acepto los terminos y condiciones de uso" />
                                                <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="*Debe aceptar para continuar" ClientValidationFunction="ValidateCheckBox" ForeColor="Red" Font-Bold="true" ValidationGroup="CondicionesGroup"></asp:CustomValidator>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-2">
                                                <asp:LinkButton ID="lbtnActivarSMS" runat="server" class="btn btn-success" Text="Activar" OnClick="lbtnActivarSMS_Click" ValidationGroup="CondicionesGroup" />
                                            </div>
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel ID="PanelConfig" runat="server" Visible="false">
                                        <div class="form-group">
                                            <label class="col-md-2">1. EMISION DE FC:</label>
                                            <div class="col-md-6">
                                                <div class="input-group">
                                                    <asp:CheckBox ID="chkAlertaFC" runat="server" class="input-group-addon" onclick="cambioChk(this,1);" />
                                                    <asp:TextBox ID="txtEnvioFact" runat="server" class="form-control" TextMode="MultiLine" Rows="4" disabled placeholder="Ingrese aqui el texto del mensaje a enviar..." />
                                                </div>
                                            </div>
                                            <div class="col-md-1">
                                                <a class="btn btn-primary ui-tooltip" data-toggle="modal" title data-original-title="Probar SMS" href="#modalEnvioSMS" onclick="pruebaSMS(1);">
                                                    Probar
                                                </a>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-2">2. EMISION DE NC:</label>
                                            <div class="col-md-6">
                                                <div class="input-group">
                                                    <asp:CheckBox ID="chkAlertaNC" runat="server" class="input-group-addon" onclick="cambioChk(this,2);" />
                                                    <asp:TextBox ID="txtEnvioNC" runat="server" class="form-control" TextMode="MultiLine" Rows="4" disabled placeholder="Ingrese aqui el texto del mensaje a enviar..." />
                                                </div>
                                            </div>
                                            <div class="col-md-1">
                                                <a class="btn btn-primary ui-tooltip" data-toggle="modal" title data-original-title="Probar SMS" href="#modalEnvioSMS" onclick="pruebaSMS(2);">
                                                    Probar
                                                </a>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-2">3. EMISION DE PRP:</label>
                                            <div class="col-md-6">
                                                <div class="input-group">
                                                    <asp:CheckBox ID="chkAlertaPRP" runat="server" class="input-group-addon" onclick="cambioChk(this,3);" />
                                                    <asp:TextBox ID="txtEnvioPRP" runat="server" class="form-control" TextMode="MultiLine" Rows="4" disabled placeholder="Ingrese aqui el texto del mensaje a enviar..." />
                                                </div>
                                            </div>
                                            <div class="col-md-1">
                                                <a class="btn btn-primary ui-tooltip" data-toggle="modal" title data-original-title="Probar SMS" href="#modalEnvioSMS" onclick="pruebaSMS(3);">
                                                    Probar
                                                </a>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-2">4. EMISION DE NC PRP:</label>
                                            <div class="col-md-6">
                                                <div class="input-group">
                                                    <asp:CheckBox ID="chkAlertaNCPRP" runat="server" class="input-group-addon" onclick="cambioChk(this,4);" />
                                                    <asp:TextBox ID="txtEnvioNCPRP" runat="server" class="form-control" TextMode="MultiLine" Rows="4" disabled placeholder="Ingrese aqui el texto del mensaje a enviar..." />
                                                </div>
                                            </div>
                                            <div class="col-md-1">
                                                <a class="btn btn-primary ui-tooltip" data-toggle="modal" title data-original-title="Probar SMS" href="#modalEnvioSMS" onclick="pruebaSMS(4);">
                                                    Probar
                                                </a>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-2">5. FC VENCIDA:</label>
                                            <div class="col-md-6">
                                                <div class="input-group">
                                                    <asp:CheckBox ID="chkAlertaFCVencida" runat="server" class="input-group-addon" onclick="cambioChk(this,5);" />
                                                    <asp:TextBox ID="txtEnvioFactVencida" runat="server" class="form-control" TextMode="MultiLine" Rows="4" disabled placeholder="Ingrese aqui el texto del mensaje a enviar..." />
                                                </div>
                                            </div>
                                            <div class="col-md-1">
                                                <a class="btn btn-primary ui-tooltip" data-toggle="modal" title data-original-title="Probar SMS" href="#modalEnvioSMS" onclick="pruebaSMS(5);">
                                                    Probar
                                                </a>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-2">6. SALDO EN CTA CTE.:</label>
                                            <div class="col-md-6">
                                                <div class="input-group">
                                                    <asp:CheckBox ID="chkAlertaSaldoCtaCte" runat="server" class="input-group-addon" onclick="cambioChk(this,6);" />
                                                    <asp:TextBox ID="txtDiaSaldoCtaCte" runat="server" class="form-control" TextMode="MultiLine" Rows="4" disabled placeholder="Ingrese aqui el texto del mensaje a enviar..." />
                                                </div>
                                            </div>
                                            <div class="col-md-1">
                                                <a class="btn btn-primary ui-tooltip" data-toggle="modal" title data-original-title="Probar SMS"href="#modalEnvioSMS" onclick="pruebaSMS(6);">
                                                    Probar
                                                </a>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-2">7. SALDO MÁXIMO SUPERADO:</label>
                                            <div class="col-md-6">
                                                <div class="input-group">
                                                    <asp:CheckBox ID="chkAlertaSaldoMax" runat="server" class="input-group-addon" onclick="cambioChk(this,7);" />
                                                    <asp:TextBox ID="txtEnvioSaldoMax" runat="server" class="form-control" TextMode="MultiLine" Rows="4" disabled placeholder="Ingrese aqui el texto del mensaje a enviar..." />
                                                </div>
                                            </div>
                                            <div class="col-md-1">
                                                <a class="btn btn-primary ui-tooltip" data-toggle="modal" title data-original-title="Probar SMS" href="#modalEnvioSMS" onclick="pruebaSMS(7);">
                                                    Probar
                                                </a>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-2">8. AVISO CUMPLEAÑOS:</label>
                                            <div class="col-md-6">
                                                <div class="input-group">
                                                    <asp:CheckBox ID="chkAlertaCumple" runat="server" class="input-group-addon" onclick="cambioChk(this,8);" />
                                                    <asp:TextBox ID="txtEnvioCumple" runat="server" class="form-control" TextMode="MultiLine" Rows="4" disabled placeholder="Ingrese aqui el texto del mensaje a enviar..." />
                                                </div>
                                            </div>
                                            <div class="col-md-1">
                                                <a class="btn btn-primary ui-tooltip" data-toggle="modal" title data-original-title="Probar SMS" href="#modalEnvioSMS" onclick="pruebaSMS(8);">
                                                    Probar
                                                </a>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-2">9. AVISO DE COBRO:</label>
                                            <div class="col-md-6">
                                                <div class="input-group">
                                                    <asp:CheckBox ID="chkAlertaCobro" runat="server" class="input-group-addon" onclick="cambioChk(this,9);" />
                                                    <asp:TextBox ID="txtEnvioCobro" runat="server" class="form-control" TextMode="MultiLine" Rows="4" disabled placeholder="'Ingrese su texto' @@COBRO " />
                                                </div>
                                            </div>
                                            <div class="col-md-1">
                                                <a class="btn btn-primary ui-tooltip" data-toggle="modal" title data-original-title="Probar SMS" href="#modalEnvioSMS" onclick="pruebaSMS(8);">
                                                    Probar
                                                </a>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <asp:Button ID="btnGuardar" runat="server" class="btn btn-success" Text="Guardar" OnClick="btnGuardar_Click" />
                                        </div>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>

                </div>
            </div>
        </div>

    </div>

    <div id="modalEnvioSMS" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <asp:UpdatePanel ID="UpdatePanelTestSMS" runat="server">
                    <ContentTemplate>
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h4 class="modal-title">Prueba de envio SMS</h4>
                        </div>
                        <div class="modal-body">
                            <div role="form" class="form-horizontal col-md-12">
                                <div class="form-group">
                                    <label class="col-md-2">Telefono</label>
                                    <div class="col-md-3">
                                        <div class="input-group">
                                            <span class="input-group-addon">0</span>
                                            <asp:TextBox ID="txtCodArea" runat="server" class="form-control" MaxLength="4" onkeypress="javascript:return validarNroSinComa(event)" />
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtTelefono" runat="server" class="form-control" MaxLength="8" onkeypress="javascript:return validarNroSinComa(event)" />
                                    </div>
                                    <div class="col-md-2">
                                        <asp:RegularExpressionValidator ErrorMessage="*" Font-Bold="true" ForeColor="Red" SetFocusOnError="true" ControlToValidate="txtCodArea" runat="server" ValidationGroup="AlertaSMSGroup" ValidationExpression="^[1-9][0-9]{1,3}$" />
                                        <asp:RequiredFieldValidator ErrorMessage="*" Font-Bold="true" ForeColor="Red" SetFocusOnError="true" ControlToValidate="txtCodArea" runat="server" ValidationGroup="AlertaSMSGroup" />
                                        <asp:RequiredFieldValidator ErrorMessage="*" Font-Bold="true" ForeColor="Red" SetFocusOnError="true" ControlToValidate="txtTelefono" runat="server" ValidationGroup="AlertaSMSGroup" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="col-md-2">Mensaje</label>
                                    <div class="col-md-10">
                                        <asp:TextBox ID="txtMensajeSMS" runat="server" class="form-control" TextMode="MultiLine" Rows="4" onkeypress="javascript:return validarEnter(event)" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:LinkButton ID="lbtnEnviarSMSPrueba" runat="server" Text="<span class='shortcut-icon fa fa-paper-plane'></span>" class="btn btn-success" OnClick="lbtnEnviarSMSPrueba_Click" ValidationGroup="AlertaSMSGroup"></asp:LinkButton>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>

    <link href="../../css/pages/reports.css" rel="stylesheet">
    <!-- Core Scripts - Include with every page -->
    <script src="../../Scripts/jquery-1.10.2.js"></script>

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

    <script src="../../Scripts/bootstrap.min.js"></script>

    <script>
        function pageLoad() {
            $("#<%= lbtnActivarSMS.ClientID %>").popover();
        }
        function pruebaSMS(numero) {
            document.getElementById("<%= txtMensajeSMS.ClientID %>").value = "";

            if (numero == 1)
                document.getElementById("<%= txtMensajeSMS.ClientID %>").value = document.getElementById("<%= txtEnvioFact.ClientID %>").value;
            if (numero == 2)
                document.getElementById("<%= txtMensajeSMS.ClientID %>").value = document.getElementById("<%= txtEnvioNC.ClientID %>").value;
            if (numero == 3)
                document.getElementById("<%= txtMensajeSMS.ClientID %>").value = document.getElementById("<%= txtEnvioPRP.ClientID %>").value;
            if (numero == 4)
                document.getElementById("<%= txtMensajeSMS.ClientID %>").value = document.getElementById("<%= txtEnvioNCPRP.ClientID %>").value;
            if (numero == 5)
                document.getElementById("<%= txtMensajeSMS.ClientID %>").value = document.getElementById("<%= txtEnvioFactVencida.ClientID %>").value;
            if (numero == 6)
                document.getElementById("<%= txtMensajeSMS.ClientID %>").value = document.getElementById("<%= txtDiaSaldoCtaCte.ClientID %>").value;
            if (numero == 7)
                document.getElementById("<%= txtMensajeSMS.ClientID %>").value = document.getElementById("<%= txtEnvioSaldoMax.ClientID %>").value;
            if (numero == 8)
                document.getElementById("<%= txtMensajeSMS.ClientID %>").value = document.getElementById("<%= txtEnvioCumple.ClientID %>").value;
            if (numero == 9)
                document.getElementById("<%= txtEnvioCobro.ClientID %>").value = document.getElementById("<%= txtEnvioCobro.ClientID %>").value;
        
        }
        function cambioChk(chk, numero) {
            if (chk.checked) {
                if (numero == 1)
                    $("#<%= txtEnvioFact.ClientID %>").removeAttr("disabled");
                if (numero == 2)
                    $("#<%= txtEnvioNC.ClientID %>").removeAttr("disabled");
                if (numero == 3)
                    $("#<%= txtEnvioPRP.ClientID %>").removeAttr("disabled");
                if (numero == 4)
                    $("#<%= txtEnvioNCPRP.ClientID %>").removeAttr("disabled");
                if (numero == 5)
                    $("#<%= txtEnvioFactVencida.ClientID %>").removeAttr("disabled");
                if (numero == 6)
                    $("#<%= txtDiaSaldoCtaCte.ClientID %>").removeAttr("disabled");
                if (numero == 7)
                    $("#<%= txtEnvioSaldoMax.ClientID %>").removeAttr("disabled");
                if (numero == 8)
                    $("#<%= txtEnvioCumple.ClientID %>").removeAttr("disabled");
                if (numero == 9)
                    $("#<%= txtEnvioCobro.ClientID %>").removeAttr("disabled");
            }
            else {
                if (numero == 1)
                    $("#<%= txtEnvioFact.ClientID %>").attr("disabled", "disabled");
                if (numero == 2)
                    $("#<%= txtEnvioNC.ClientID %>").attr("disabled", "disabled");
                if (numero == 3)
                    $("#<%= txtEnvioPRP.ClientID %>").attr("disabled", "disabled");
                if (numero == 4)
                    $("#<%= txtEnvioNCPRP.ClientID %>").attr("disabled", "disabled");
                if (numero == 5)
                    $("#<%= txtEnvioFactVencida.ClientID %>").attr("disabled", "disabled");
                if (numero == 6)
                    $("#<%= txtDiaSaldoCtaCte.ClientID %>").attr("disabled", "disabled");
                if (numero == 7)
                    $("#<%= txtEnvioSaldoMax.ClientID %>").attr("disabled", "disabled");
                if (numero == 8)
                    $("#<%= txtEnvioCumple.ClientID %>").attr("disabled", "disabled");
                if (numero == 9)
                    $("#<%= txtEnvioCobro.ClientID %>").attr("disabled", "disabled");
            }
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
        function ValidateCheckBox(sender, args) {
            if (document.getElementById("<%=chkAceptaCondiciones.ClientID %>").checked == true) {
                args.IsValid = true;
            } else {
                args.IsValid = false;
            }
        }
    </script>

</asp:Content>

