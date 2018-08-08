<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="InformeBTBF.aspx.cs" Inherits="Gestion_Web.Formularios.Herramientas.InformeBTBF" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">

        <div class="row">

            <div class="col-md-12">

                <div class="widget stacked">

                    <div class="stat">
                        <h5><i class="icon-map-marker"></i>Herramientas > Regimen Informativo</h5>
                    </div>
                    <div class="widget-header">
                        <i class="icon-pencil"></i>
                        <h3>Generar</h3>
                    </div>
                    <!-- /.widget-header -->

                    <div class="widget-content">
                        <div class="form-horizontal col-md-12">
                            <fieldset>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>
                                        <div class="form-group">
                                            <label class="col-md-2">Empresa</label>
                                            <div class="col-md-3">
                                                <asp:DropDownList ID="ListEmpresa" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ListEmpresa_SelectedIndexChanged" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-2">Sucursal</label>
                                            <div class="col-md-3">
                                                <asp:DropDownList ID="ListSucursal" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ListSucursal_SelectedIndexChanged" >
                                                    <asp:ListItem Text="Todas" Value="0" />
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-2">Pto Venta</label>
                                            <div class="col-md-3">
                                                <asp:DropDownList ID="ListPuntoVta" runat="server" class="form-control" >
                                                    <asp:ListItem Text="Todos" Value="0" />
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-2">Periodo</label>
                                            <div class="col-md-2">
                                                <div class="input-group">
                                                    <span class="input-group-addon"><i class="icon-calendar"></i></span>
                                                    <asp:TextBox ID="txtFechaDesde" Style="text-align: right;" class="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="col-md-2">
                                                <div class="input-group">
                                                    <span class="input-group-addon"><i class="icon-calendar"></i></span>
                                                    <asp:TextBox ID="txtFechaHasta" Style="text-align: right;" class="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>                                        
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <div class="form-group">
                                    <div class="col-md-4">
                                        <asp:Button ID="lbtnGenerar" Text="Generar" runat="server" class="btn btn-success" OnClick="lbtnGenerar_Click" ValidationGroup="RegimenGroup" />
                                    </div>
                                </div>
                            </fieldset>
                        </div>
                    </div>
                </div>
            </div>
        </div>
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

    <script>
        function ValidarVuelos() {

            var V = ValidateFly();
            envio(V);
            if (V == 1)
                return false;
            if (V == 0)
                return true;
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
    </script>
    <script>
        function pageLoad() {
            $("#<%= txtFechaDesde.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%= txtFechaHasta.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        }
    </script>
</asp:Content>
