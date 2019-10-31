<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LiquidacionesABM.aspx.cs" Inherits="Gestion_Web.Formularios.Facturas.LiquidacionesABM" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">
        <div class="widget stacked">
            <div class="widget-header">
                <i class="icon-list"></i>
                <h3>Generar Liquidacion</h3>
            </div>
            <div class="widget-content">
                <div class="form-horizontal" style="margin-bottom: 20px">
                    <div class="col-md-9" style="margin-bottom: 10px">
                        <label class="col-md-2">Fecha Liquidacion</label>
                        <div class="col-md-3">
                            <div class="input-group">
                                <span class="input-group-addon"><i class="shortcut-icon icon-calendar"></i></span>
                                <asp:TextBox runat="server" ID="txtFecha" style="text-align:right" AutoComplete="off" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-1">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="  *  " ControlToValidate="txtFecha" ValidationGroup="LiquidacionesGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-md-9" style="margin-bottom: 10px">
                        <label class="col-md-2">Numero de Liquidacion</label>
                        <div class="col-md-3">
                            <asp:TextBox runat="server" ID="txtNroLiqui" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-1">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="  *  " ControlToValidate="txtNroLiqui" ValidationGroup="LiquidacionesGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-md-9" style="margin-bottom: 10px">
                        <label class="col-md-2">Seleccione Sucursal</label>
                        <div class="col-md-3">
                            <asp:DropDownList runat="server" ID="ListSucursales" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="col-md-1">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="  *  " ControlToValidate="ListSucursales" InitialValue="-1" ValidationGroup="LiquidacionesGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-md-9" style="margin-bottom: 10px">
                        <label class="col-md-2">Importe</label>
                        <div class="col-md-3">
                            <div class="input-group">
                                <span class="input-group-addon">$</span>
                                <asp:TextBox runat="server" ID="txtImporte" style="text-align:right" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-2">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="  *  " ControlToValidate="txtImporte" ValidationGroup="LiquidacionesGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Solo acepta numeros" ControlToValidate="txtImporte" ValidationGroup="LiquidacionesGroup" Font-Bold="true" ForeColor="Red" ValidationExpression="^[1-9]\d*(\.\d+)?$" />
                        </div>
                    </div>
                </div>
                <hr class="col-md-6" style="border-top: 5px solid #D5D5D5; padding-left: 0; padding-right: 0" />
                <div class="form-group" style="margin-bottom: 20px">
                    <div class="col-md-9" style="margin-bottom: 10px">
                        <label class="col-md-2">Seleccionar Producto</label>
                        <div class="col-md-3">
                            <asp:DropDownList runat="server" ID="ListProductos" CssClass="form-control"></asp:DropDownList>
                        </div>
                        <div class="col-md-1">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="  *  " ControlToValidate="ListProductos" ValidationGroup="ProductosGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-md-9" style="margin-bottom: 20px">
                        <label class="col-md-2">Cantidad</label>
                        <div class="col-md-3">
                            <asp:TextBox runat="server" ID="txtCantidad" style="text-align:right" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-1">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="  *  " ControlToValidate="txtCantidad" ValidationGroup="ProductosGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="col-md-9 pull-left">
                        <asp:LinkButton ID="lbtAgregarProducto" runat="server" Text="Agregar Producto" OnClientClick="AgregarProductoEnPH()" class="btn btn-success" ValidationGroup="ProductosGroup" />
                    </div>
                </div>
                <hr class="col-md-6" style="border-top: 5px solid #D5D5D5; padding-left: 0; padding-right: 0" />
                <div class="form-group" style="margin-bottom: 20px">
                    <div class="col-md-11" style="margin-bottom: 10px">
                        <div class="col-md-6">
                            <table class="table table-bordered table-hover" id="tableProductos">
                                <thead>
                                    <tr>
                                        <th style="width: 25%">Cod. Producto</th>
                                        <th style="width: 40%">Descripcion</th>
                                        <th style="width: 25%">Cant. Consumida</th>
                                        <th style="width:10%"></th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:PlaceHolder ID="phProductos" runat="server" />
                                    <asp:HiddenField ID="hiddenProd" runat="server" />
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
                <hr class="col-md-6" style="border-top: 5px solid #D5D5D5; padding-left: 0; padding-right: 0" />
                <div class="col-md-12 pull-left">
                    <asp:LinkButton ID="lbtnAgregar" runat="server" Text="Agregar" OnClick="lbtnAgregar_Click" class="btn btn-success" ValidationGroup="LiquidacionesGroup" />
                    <asp:LinkButton ID="lbtnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-default" PostBackUrl="~/Formularios/Facturas/LiquidacionesF.aspx" />
                </div>
            </div>
        </div>

        <script src="../../Scripts/jquery-1.10.2.js"></script>
        <script src="../../Scripts/bootstrap.min.js"></script>

        <script src="../../Scripts/libs/jquery-1.9.1.min.js"></script>
        <script src="../../Scripts/libs/jquery-ui-1.10.0.custom.min.js"></script>
        <script src="../../Scripts/libs/bootstrap.min.js"></script>
        <script src="../../Scripts/plugins/msgbox/jquery.msgbox.min.js"></script>

        <script>
            function borrarProd(idprod) {
                event.preventDefault();
                var pepe = document.getElementById('<%= hiddenProd.ClientID%>').value;
                var reg = "\\d+,+(" + idprod + ")+,+\\d*;*";
                var re = new RegExp(reg);
                if (document.getElementById('<%= hiddenProd.ClientID%>').value.includes(idprod)) {
                    document.getElementById('<%= hiddenProd.ClientID%>').value = document.getElementById('<%= hiddenProd.ClientID%>').value.replace(re, "");
                    var pepe = document.getElementById('<%= hiddenProd.ClientID%>').value;
                    document.getElementById("prod_"+idprod).outerHTML="";
                }
            }
            function pageLoad() {
                $("#<%= txtFecha.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
                var date = new Date();
                document.getElementById('<%= txtFecha.ClientID%>').value = date.toLocaleDateString();
            }

            function AgregarProductoEnPH() {
                event.preventDefault();
                var ddlPro = document.getElementById('<%= ListProductos.ClientID%>');
                var idPro = ddlPro.value;
                var cant = document.getElementById('<%= txtCantidad.ClientID%>').value;

                $.ajax({
                    method: "POST",
                    url: "LiquidacionesABM.aspx/GetArticuloForTable",
                    data: '{id: "' + idPro + '", cant: "' + cant + '" }',
                    contentType: "application/json",
                    dataType: 'json',
                    error: (error) => {
                        console.log(JSON.stringify(error));
                        //$.msgbox("No se pudo cargar la tabla", { type: "error" });
                    },
                    success: succesAgregarPr
                });
            }


            function succesAgregarPr(response) {
                var obj = JSON.parse(response.d);
                if (document.getElementById('<%= hiddenProd.ClientID%>').value.includes(obj.codigo)) {
                    return;
                }
                $('#tableProductos').append(
                    "<tr id=\"prod_"+obj.codigo+"\">" +
                    "<td> " + obj.codigo + "</td>" +
                    "<td> " + obj.descripcion + "</td>" +
                    "<td> " + obj.cantidad + "</td>" +
                    "<td> <a class=\"btn btn-info \" onclick=\"javascript: return borrarProd('"+ obj.codigo.toString() +"');\" >" +
                    "<i class=\"shortcut-icon icon-trash\"></i> </a> " +
                    "</td > " +
                    "</tr>"
                );
                if (document.getElementById('<%= hiddenProd.ClientID%>').value == "") {
                    document.getElementById('<%= hiddenProd.ClientID%>').value += obj.id + "," + obj.codigo + "," + obj.cantidad;
                }
                else {
                    document.getElementById('<%= hiddenProd.ClientID%>').value += ";" + obj.id + "," + obj.codigo + "," + obj.cantidad;
                }
            }
        </script>
    </div>
</asp:Content>
