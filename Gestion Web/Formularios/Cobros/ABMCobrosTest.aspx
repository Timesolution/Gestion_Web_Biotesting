<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ABMCobrosTest.aspx.cs" Inherits="Gestion_Web.Formularios.Cobros.ABMCobrosTest" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container">


        <div class="row">
            <div class="col-md-12 col-xs-12 ">
                <div class="widget widget-table">

                    <div class="widget-header">
                        <i class="icon-wrench"></i>
                        <h3>Documentos impagos

                        </h3>
                    </div>
                    <div class="widget-content">

                        <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Always" runat="server">
                            <ContentTemplate>
                                <table class="table table-striped table-bordered">
                                    <thead>
                                        <tr>
                                            <th>Numero</th>
                                            <th>Saldo</th>
                                            <th>Imputar</th>

                                        </tr>

                                    </thead>
                                    <tbody>
                                        <asp:PlaceHolder ID="phDocumentos" runat="server"></asp:PlaceHolder>
                                        <tr>
                                            <th>Total</th>
                                            <th>
                                                <asp:Label ID="labelSaldo" runat="server" class="form-control" disabled="" Text="" Style="text-align: right"></asp:Label>
                                            </th>
                                            <th>
                                                <asp:Label ID="labelTotal" runat="server" class="form-control" disabled="" Text="0" Style="text-align: right"></asp:Label>
                                            </th>
                                        </tr>
                                    </tbody>
                                </table>
                                </div>


                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                        <div class="col-md-6">
                        </div>


                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 col-xs-12 ">
                <div class="widget">
                    <div class="widget-header">
                        <i class="icon-pencil"></i>
                        <h3>Pagos</h3>
                    </div>

                    <div class="widget-content">
                        <section id="accordions">
                            <div class="panel-group accordion">
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h4 class="panel-title">
                                            <a class="accordion-toggle" data-toggle="collapse" data-parent=".accordion" href="#collapseOne">Efectivo
                                            </a>
                                        </h4>
                                    </div>
                                    <div id="collapseOne" class="panel-collapse collapse">
                                        <div class="panel-body">
                                            <asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Always" runat="server">
                                                <ContentTemplate>
                                                    <div action="/" id="validation-form" role="form" class="form-horizontal col-md-7">
                                                        <fieldset>
                                                            <div class="form-group">
                                                                <label for="validateSelect" class="col-md-4">Tipo</label>
                                                                <div class="col-md-4">
                                                                    <asp:DropDownList ID="DropListTipo" runat="server" class="form-control" AutoPostBack="True" OnSelectedIndexChanged="DropListTipo_SelectedIndexChanged"></asp:DropDownList>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ErrorMessage="Seleccione un tipo de Moneda" ControlToValidate="DropListTipo" InitialValue="Seleccione..." ValidationGroup="MonedaGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <label for="name" class="col-md-4">Monto</label>
                                                                <div class="col-md-4">
                                                                    <asp:TextBox ID="txtMonto" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtMonto" ValidationGroup="MonedaGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <label for="name" class="col-md-4">Cotizacion</label>
                                                                <div class="col-md-4">
                                                                    <asp:TextBox ID="txtCotizacion" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtCotizacion" ValidationGroup="MonedaGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <div class="col-md-4">
                                                                    <asp:Button ID="btnAgregarPagoM" runat="server" Text="Agregar" class="btn btn-success" OnClick="btnAgregarPagoM_Click" ValidationGroup="MonedaGroup" />
                                                                </div>
                                                            </div>
                                                        </fieldset>
                                                    </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h4 class="panel-title">
                                            <a class="accordion-toggle" data-toggle="collapse" data-parent=".accordion" href="#collapseTwo">Cheque
                                            </a>
                                        </h4>
                                    </div>
                                    <div id="collapseTwo" class="panel-collapse collapse">
                                        <div class="panel-body">
                                            <asp:UpdatePanel ID="UpdatePanel4" UpdateMode="Always" runat="server">
                                                <ContentTemplate>
                                                    <div action="/" role="form" class="form-horizontal col-md-7">

                                                        <br />
                                                        <fieldset>
                                                            <div class="form-group">
                                                                <label for="name" class="col-md-4">Fecha Cobro</label>

                                                                <div class="col-md-4">
                                                                    <asp:TextBox ID="txtFechaCh" runat="server" class="form-control"></asp:TextBox>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtFechaCh" ValidationGroup="ChequeGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <label for="name" class="col-md-4">Importe</label>

                                                                <div class="col-md-4">
                                                                    <asp:TextBox ID="txtImporteCh" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtImporteCh" ValidationGroup="ChequeGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <label for="name" class="col-md-4">Numero</label>

                                                                <div class="col-md-4">
                                                                    <asp:TextBox ID="txtNumeroCh" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtNumeroCh" ValidationGroup="ChequeGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>

                                                            <div class="form-group">
                                                                <label for="validateSelect" class="col-md-4">Banco</label>
                                                                <div class="col-md-4">
                                                                    <asp:DropDownList ID="DropListBancoCh" runat="server" class="form-control">
                                                                    </asp:DropDownList>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Seleccione un banco" ControlToValidate="DropListBancoCh" InitialValue="-1" ValidationGroup="ChequeGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <label for="validateSelect" class="col-md-4">Cuenta</label>
                                                                <div class="col-md-4">
                                                                    <asp:TextBox ID="txtCuentaCh" runat="server" class="form-control"></asp:TextBox>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtCuentaCh" ValidationGroup="ChequeGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <label for="validateSelect" class="col-md-4">Cuit</label>
                                                                <div class="col-md-4">
                                                                    <asp:TextBox ID="txtCuitCh" runat="server" class="form-control" onkeypress="javascript:return validarNroGuion(event)"></asp:TextBox>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtCuitCh" ValidationGroup="ChequeGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <asp:RegularExpressionValidator ControlToValidate="txtCuitCh" ID="RegularExpressionValidator4" runat="server" ErrorMessage="Formato de CUIT Invalido" ValidationExpression="^\d{2}\d{8}\d{1}$" ValidationGroup="ChequeGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RegularExpressionValidator>
                                                                </div>
                                                            </div>

                                                            <div class="form-group">
                                                                <label for="validateSelect" class="col-md-4">Librador</label>
                                                                <div class="col-md-4">
                                                                    <asp:TextBox ID="txtLibradorCh" runat="server" class="form-control"></asp:TextBox>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtLibradorCh" ValidationGroup="ChequeGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <div class="col-md-4">
                                                                    <asp:Button ID="btnAgregarPagoCh" runat="server" Text="Agregar" class="btn btn-success" OnClick="btnAgregarPagoCh_Click" ValidationGroup="ChequeGroup" />
                                                                </div>
                                                            </div>

                                                        </fieldset>
                                                    </div>

                                                    <div class="col-md-4">
                                                        <div class="widget stacked widget-table">

                                                            <div class="widget-header">
                                                                <span class="icon-external-link"></span>
                                                                <h3>Cheques</h3>
                                                            </div>
                                                            <!-- .widget-header -->

                                                            <div class="widget-content">
                                                                <table class="table table-bordered table-striped">

                                                                    <thead>
                                                                        <tr>
                                                                            <th>Cheque</th>
                                                                            <th>Monto</th>
                                                                            <th class="td-actions"></th>
                                                                        </tr>
                                                                    </thead>
                                                                    <tbody>
                                                                        <asp:PlaceHolder ID="phCheques" runat="server"></asp:PlaceHolder>
                                                                    </tbody>
                                                                </table>

                                                            </div>
                                                            <!-- .widget-content -->

                                                        </div>

                                                    </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>
                                <div class="panel panel-default">
                                    <div class="panel-heading">
                                        <h4 class="panel-title">
                                            <a class="accordion-toggle" data-toggle="collapse" data-parent=".accordion" href="#collapseThree">Transferencia
                                            </a>
                                        </h4>
                                    </div>
                                    <div id="collapseThree" class="panel-collapse collapse">
                                        <div class="panel-body">
                                            <asp:UpdatePanel ID="UpdatePanel5" UpdateMode="Always" runat="server">
                                                <ContentTemplate>
                                                    <div action="/" role="form" class="form-horizontal col-md-7">
                                                        <br />
                                                        <fieldset>
                                                            <div class="form-group">
                                                                <label for="name" class="col-md-4">Fecha Cobro</label>

                                                                <div class="col-md-4">
                                                                    <asp:TextBox ID="txtFechaTransf" runat="server" class="form-control"></asp:TextBox>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <label for="name" class="col-md-4">Importe</label>

                                                                <div class="col-md-4">
                                                                    <asp:TextBox ID="txtImporteTransf" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtImporteTransf" ValidationGroup="TransferGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <label for="validateSelect" class="col-md-4">Banco</label>
                                                                <div class="col-md-4">
                                                                    <asp:DropDownList ID="DropListBancoTransf" runat="server" class="form-control">
                                                                    </asp:DropDownList>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ErrorMessage="Seleccione un banco" ControlToValidate="DropListBancoTransf" InitialValue="0" ValidationGroup="TransferGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <label for="validateSelect" class="col-md-4">Cuenta</label>
                                                                <div class="col-md-4">
                                                                    <asp:TextBox ID="txtCuentaTransf" runat="server" class="form-control"></asp:TextBox>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtCuentaTransf" ValidationGroup="TransferGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <label for="validateSelect" class="col-md-4">CBU</label>
                                                                <div class="col-md-4">
                                                                    <asp:TextBox ID="txtCbu" runat="server" class="form-control" onkeypress="javascript:return validarNroGuion(event)"></asp:TextBox>
                                                                </div>
                                                                <div class="col-md-4">
                                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtCbu" ValidationGroup="TransferGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                                </div>
                                                            </div>
                                                            <div class="form-group">
                                                                <div class="col-md-4">
                                                                    <asp:Button ID="btn_AgregarPagoTrans" runat="server" Text="Agregar" class="btn btn-success" OnClick="btnAgregarPagoTrans_Click" ValidationGroup="TransferGroup" />
                                                                </div>
                                                            </div>
                                                        </fieldset>
                                                    </div>
                                                </ContentTemplate>
                                                <Triggers>
                                                </Triggers>
                                            </asp:UpdatePanel>
                                        </div>
                                    </div>
                                </div>

                            </div>
                        </section>
                        <br />
                        <div class="bs-example">
                            <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Always" runat="server">
                                <ContentTemplate>
                                    <table class="table table-striped table-bordered">
                                        <thead>
                                            <tr>
                                                <th>Tipo Pago</th>
                                                <th>Monto</th>
                                                <th>Cotizacion</th>
                                                <th>Total</th>

                                            </tr>

                                        </thead>
                                        <tbody>
                                            <asp:PlaceHolder ID="phEfectivo" runat="server"></asp:PlaceHolder>
                                            <th>Total</th>
                                            <th></th>
                                            <th></th>
                                            <th>
                                                <asp:TextBox ID="txtTotal" runat="server" class="form-control" disabled="" Text="0" Style="text-align: right"></asp:TextBox>
                                            </th>
                                        </tbody>
                                    </table>

                                    <div class="form-group">
                                        <div class="col-md-4">
                                            <a class="btn btn-success" data-toggle="modal" href="#modalAgregar">Finalizar</a>
                                        </div>
                                    </div>

                                    </div>


                                </ContentTemplate>
                                <Triggers>
                                </Triggers>
                            </asp:UpdatePanel>
                            <div class="col-md-6">
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="modalAgregar" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">

                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Recibo</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <div class="form-group">
                                <label class="col-md-4">Fecha</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtFechaCobro" runat="server" class="form-control"></asp:TextBox>
                                </div>

                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Numero</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtNumeroCobro" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Total Imputado</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtTotalImputadoCobro" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Total Cobro</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtTotalCobro" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <%--<div class="form-group">
                                <label class="col-md-4">Recibo</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtRecibo" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>--%>
                        </div>
                    </div>
                    <div class="modal-footer">

                        <asp:Button ID="btnAgregarPago" runat="server" Text="Finalizar" class="btn btn-success" OnClick="btnAgregarPago_Click" />
                    </div>

                </div>
            </div>
        </div>

    </div>



    <script src="../../Scripts/jquery-1.10.2.js"></script>


    <script src="../../Scripts/libs/jquery-1.9.1.min.js"></script>
    <script src="../../Scripts/libs/jquery-ui-1.10.0.custom.min.js"></script>


    <script src="../../Scripts/plugins/msgGrowl/js/msgGrowl.js"></script>
    <script src="../../Scripts/plugins/lightbox/jquery.lightbox.min.js"></script>
    <script src="../../Scripts/plugins/msgbox/jquery.msgbox.min.js"></script>
    <script src="../../Scripts/plugins/validate/jquery.validate.js"></script>

    <script src="../../Scripts/Application.js"></script>

    <script src="../../Scripts/demo/notifications.js"></script>
    <script src="../../Scripts/demo/sliders.js"></script>




    <script>
        $(function () {
            $("#<%= txtFechaCh.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            });

            $(function () {
                $("#<%= txtFechaTransf.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });
    </script>

    <script>
        $(document).ready(
            function () {
                var total = 0;
                for (var i = 1; i < document.getElementById('tbDocumentos').rows.length - 1; i++) {
                    var txtValor = document.getElementById('tbDocumentos').rows[i].cells[2].childNodes[0].value;
                    var valor = txtValor.replace(',', '.');
                    if (valor != 0) {
                        total = total + parseFloat(valor);
                    }
                    else {
                        total = total;
                    }
                }
                var ValorN = total.toFixed(2);
                document.getElementById('<%= txtTotal.ClientID %>').value = ValorN;
                    document.getElementById('<%= txtTotalImputadoCobro.ClientID %>').value = ValorN;
                }

        );
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
                if (key == 46 || key == 8 || key == 44) // Detectar . (punto) , backspace (retroceso) y , (coma)
                { return true; }
                else
                { return false; }
            }
            return true;
        }
    </script>

    <%--   <script>
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
                if (key == 46 || key == 8 || key == 45) // Detectar . (punto) , backspace (retroceso) y guion medio (-)
                { return true; }
                else
                { return false; }
            }
            return true;
        }
</script>--%>
</asp:Content>

