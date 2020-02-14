<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ActPadronF.aspx.cs" Inherits="Gestion_Web.Formularios.Herramientas.ActPadronF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">

        <div class="row">

            <div class="col-md-12">

                <div class="widget stacked">

                    <div class="stat">                        
                        <h5><i class="icon-map-marker"></i> Herramientas > II. BB.</h5>
                    </div>
                    <div class="widget-header">
                        <i class="icon-pencil"></i>
                        <h3>Actualización padron ingresos brutos</h3>
                    </div>
                    <!-- /.widget-header -->

                    <div class="widget-content">
                        <div class="form-horizontal col-md-12">
                            <fieldset>
                                <div class="form-group">
                                    <label for="validateSelect" class="col-md-4">Padron Buenoas Aires actualizado al</label>

                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtFechaActualizacion" Style="text-align: right;" class="form-control" runat="server" disabled></asp:TextBox>
                                    </div>

                                    <div class="col-md-2">
                                        <asp:LinkButton ID="lbtnActualizar" runat="server" Text="<span class='shortcut-icon icon-refresh'></span>" OnClick="lbtnActualizar_Click" data-toggle="tooltip" title data-original-title="Actualizar padron"  class="btn btn-success ui-tooltip" />
                                    </div>
                                </div>
                                
                                <div class="form-group">
                                    <label for="validateSelect" class="col-md-4">Padron CABA actualizado al</label>
                                    <div class="col-md-4">
                                        <asp:TextBox ID="txtFechaActualizacionCABA" Style="text-align: right;" class="form-control" runat="server" disabled></asp:TextBox>
                                    </div>

                                    <div class="col-md-2">
                                        <asp:LinkButton ID="lbtnActualizarCABA" runat="server" Text="<span class='shortcut-icon icon-refresh'></span>" data-toggle="tooltip" title data-original-title="Actualizar padron CABA"  class="btn btn-success ui-tooltip" />
                                        <asp:HiddenField ID="patharchivo" runat="server" />
                                        <asp:LinkButton ID="lbtnCargarPadronCABA" runat="server" OnClick="lbtnCargarPadronCABA_Click" Text="<span class='fa fa-arrow-circle-down'></span>" data-toggle="tooltip" title data-original-title="Descargar archivo"  class="btn btn-info ui-tooltip" />
                                    </div>
                                </div>
                                <%--<div class="form-group">
                                    <label for="validateSelect" class="col-md-4">Cargar padron CABA</label>
                                    <div class="col-md-4">
                                    <input type="file" name="btnCargarpadron" text=""  runat="server" />
                                    </div>

                                </div>--%>
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
</asp:Content>
