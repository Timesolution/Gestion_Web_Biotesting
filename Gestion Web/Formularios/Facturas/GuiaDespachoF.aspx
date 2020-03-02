<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GuiaDespachoF.aspx.cs" Inherits="Gestion_Web.Formularios.Facturas.GuiaDespachoF" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">

            <div class="col-md-12">

                <div class="widget stacked">

                    <div class="stat">                        
                        <h5><i class="icon-map-marker"></i> Ventas > Guia Despacho</h5>
                    </div>

                    <div class="widget-header">
                        <i class="icon-pencil"></i>
                        <h3>Guia de Despacho</h3>
                    </div>
                    <!-- /.widget-header -->

                    <div class="widget-content">
                        <div class="bs-example">
                            <ul id="myTab" class="nav nav-tabs">
                                <li class="active"><a href="#home" data-toggle="tab">Detalle</a></li>

                            </ul>
                            <div class="tab-content">
                                <div class="tab-pane fade active in" id="home">
                                    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Always" runat="server">
                                        <ContentTemplate>
                                            <div role="form" class="form-horizontal col-md-12">
                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Fecha</label>

                                                    <div class="col-md-2">
                                                        <asp:TextBox ID="txtFechaDespacho" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">

                                                    <label for="name" class="col-md-4">Valor</label>

                                                    <div class="col-md-2">
                                                        <div class="input-group" >
                                                            <span class="input-group-addon">$</span>
                                                            <asp:TextBox ID="txtValorDespacho" runat="server"  Style="text-align: right;" class="form-control"  MaxLength="11" onkeypress="javascript:return validarSoloNro(event)"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                </div>

                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Nro Despacho</label>

                                                    <div class="col-md-2">
                                                        <asp:TextBox ID="txtNroDespacho" runat="server" Style="text-align: right;" class="form-control"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Transporte</label>

                                                    <div class="col-md-2">
                                                        <asp:TextBox ID="txtTransportes" runat="server" class="form-control" Text="-"></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Tipo Bulto</label>

                                                    <div class="col-md-2">
                                                        <asp:TextBox ID="txtTipoBulto" runat="server" class="form-control"></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Cantidad</label>

                                                    <div class="col-md-2">
                                                        <asp:TextBox ID="txtCantidadBulto" runat="server" onkeypress="javascript:return validarNro(event)" class="form-control"></asp:TextBox>
                                                    </div>

                                                    <div class="col-md-1">
                                                        <asp:LinkButton ID="btnAgregarDespacho" class="btn btn-info" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" OnClick="btnAgregarDespacho_Click" />
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Contrareembolso</label>

                                                    <div class="col-md-2">
                                                        <asp:CheckBox ID="chkContraReembolso" runat="server" />
                                                    </div>
                                                </div>

                                                <div class="col-md-10">
                                                    <div class="widget-content">

                                                        <table class="table table-bordered table-striped">
                                                            <thead>
                                                                <tr>
                                                                    <th style="width: 70%">Tipo de bulto</th>
                                                                    <th style="width: 15%">Cantidad</th>
                                                                    <th style="width: 15%">Accion</th>
                                                                </tr>
                                                            </thead>

                                                            <tbody>
                                                                <asp:PlaceHolder ID="phDespacho" runat="server"></asp:PlaceHolder>
                                                            </tbody>
                                                        </table>

                                                    </div>
                                                </div>

                                            </div>

                                        </ContentTemplate>
                                        <Triggers>
                                        </Triggers>
                                    </asp:UpdatePanel>

                                    <div class="col-md-8">
                                        <br />
                                        <asp:Button ID="btnAgregar" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnAgregar_Click" />
                                        <asp:Button ID="btnVolver" runat="server" Text="Guardar" class="btn btn-success" Visible="false" PostBackUrl="../Facturas/FacturasF.aspx" />
                                        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" class="btn btn-default" PostBackUrl="../Facturas/FacturasF.aspx" />
                                    </div>

                                </div>
                            </div>

                        </div>


                    </div>
                </div>
            </div>
        </div>
    </div>
    <%--roww--%>


    <!-- Core Scripts - Include with every page -->
    <script src="../../Scripts/jquery-1.10.2.js"></script>
    <script src="../../Scripts/bootstrap.min.js"></script>
    <%--<script src="../Scripts/plugins/metisMenu/jquery.metisMenu.js"></script>--%>

    <script src="../../Scripts/libs/jquery-1.9.1.min.js"></script>
    <script src="../../Scripts/libs/jquery-ui-1.10.0.custom.min.js"></script>
    <script src="../../Scripts/libs/bootstrap.min.js"></script>

    <script src="../../Scripts/plugins/hoverIntent/jquery.hoverIntent.minified.js"></script>
    <script src="../../Scripts/plugins/lightbox/jquery.lightbox.min.js"></script>

    <script src="../../Scripts/Application.js"></script>

    <script src="../../Scripts/demo/gallery.js"></script>

    <script src="../../Scripts/plugins/msgGrowl/js/msgGrowl.js"></script>
    <script src="../../Scripts/plugins/lightbox/jquery.lightbox.min.js"></script>
    <script src="../../Scripts/plugins/msgbox/jquery.msgbox.min.js"></script>
    <script src="../../Scripts/demo/notifications.js"></script>

    <script>
        $(function () {
            $("#<%= txtFechaDespacho.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });
    </script>

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endReq);
        function endReq(sender, args) {
            //$('#dataTables-example').dataTable();
            $("#<%= txtFechaDespacho.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
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
                if (key == 46 || key == 8 || key == 44)// Detectar . (punto) y backspace (retroceso) y , (coma)
                { return true; }
                else
                { return false; }
            }
            return true;
        }

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
