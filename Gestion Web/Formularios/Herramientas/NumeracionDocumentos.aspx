<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="NumeracionDocumentos.aspx.cs" Inherits="Gestion_Web.Formularios.Herramientas.NumeracionDocumentos" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">


        <%--<div class="row">

            <div class="col-md-12">

                <div class="widget stacked">

                    <div class="widget-header">
                        <i class="icon-pencil"></i>
                        <h3>Panel de Control</h3>

                    </div>
                    <!-- /.widget-header -->

                    <div class="widget-content">

                        <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Always" runat="server">
                            <ContentTemplate>
                                <div class="form-horizontal col-md-10">
                                    <fieldset>

                                        <div class="form-group">
                                            <label for="validateSelect" class="col-md-4">Presupuesto</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="DropListPorcentajeIVA" runat="server" class="form-control">
                                                    <asp:ListItem Value="-1">Seleccione..</asp:ListItem>
                                                    <asp:ListItem Value="0">0%</asp:ListItem>
                                                    <asp:ListItem Value="10.5">10,5%</asp:ListItem>
                                                    <asp:ListItem Value="21">21%</asp:ListItem>
                                                </asp:DropDownList>

                                            </div>
                                            <div class="col-md-4">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="Seleccione un Porcentaje" ControlToValidate="DropListPorcentajeIVA" InitialValue="-1" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>


                                    </fieldset>

                                </div>

                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>

                        <div class="col-md-8">
                            <asp:Button ID="btnAgregar" runat="server" Text="Guardar" class="btn btn-success" ValidationGroup="ArticuloGroup" OnClick="btnAgregar_Click" />

                        </div>
                    </div>
                </div>
            </div>
        </div>--%>

        <div class="row">
            <div class="col-md-12">
                <div class="widget stacked">

                    <div class="stat">                        
                        <h5><i class="icon-map-marker"></i> Herramientas > Numeracion Documentos</h5>
                    </div>

                    <div class="widget-header">
                        <i class="icon-list"></i>
                        <h3>Numeracion

                        </h3>
                    </div>
                    <div class="widget-content">

                        <div class="form-horizontal col-md-12">
                            <fieldset>
                                <div class="form-group">
                                    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Always" runat="server">
                                        <ContentTemplate>
                                             <label class="col-md-2">Sucursal </label>
                                            <div class="col-md-3">
                                                <asp:DropDownList ID="ListSucursal" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ListSucursal_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                            <label class="col-md-2">Punto de Venta </label>
                                            <div class="col-md-3">
                                                <asp:DropDownList ID="ListPuntoVenta" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="DropListClientes_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                        </ContentTemplate>
                                        <Triggers>
                                        </Triggers>
                                    </asp:UpdatePanel>
                                    <div class="col-md-2">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="ListPuntoVenta" InitialValue="-1" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-2 pull-right">
                                        <asp:LinkButton ID="lbtnActualizar2" runat="server" Text="Guardar" class="btn btn-success" OnClick="lbtnActualizar_Click" OnClientClick="javascript:return ValidarVuelos();" />
                                    </div>
                                </div>
                            </fieldset>
                        </div>
                        <asp:UpdatePanel ID="UpdatePanel3" UpdateMode="Always" runat="server">
                            <ContentTemplate>
                                <a class="btn btn-info" style="display: none" data-toggle="modal" id="abreDialog" href="#modalFacturaDetalle">Agregar Tipo Cliente</a>
                                <table class="table table-bordered table-striped" id="dataTablesCC-example">
                                    <thead>
                                        <tr>
                                            <th style="width: 80%">Tipo</th>
                                            <th style="width: 20%">Numeracion</th>
                                        </tr>

                                    </thead>
                                    <tbody>
                                        <asp:PlaceHolder ID="phNumeracion" runat="server"></asp:PlaceHolder>
                                    </tbody>
                                </table>

                                <%--</div>--%>
                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                        <div class="col-md-8">
                            <asp:LinkButton ID="lbtnActualizar" runat="server" Text="Guardar" class="btn btn-success" OnClick="lbtnActualizar_Click" OnClientClick="javascript:return ValidarVuelos();" />
                        </div>
                    </div>

                    <%-- widget content --%>
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
    <%--        <div id="modalPais" class="modal fade" tabindex="-1" role="dialog">
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
                         <asp:LinkButton ID="lbtnAgregarPais" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnAgregarPais_Click"/>
                    </div>

                </div>
            </div>
        </div>--%>
    <%--Fin modalGrupo--%>

    <%--        <div id="modalSubGrupo" class="modal fade" tabindex="-1" role="dialog">
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
                                                 <asp:LinkButton ID="lbtnAgregarSubGrupo" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnAgregarSubgrupo_Click" />

                    </div>
                </div>
            </div>
        </div>
    </div>--%>
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


    <%--    <script>


        $(function () {
            $("#<%= txtFechaAlta.ClientID %>").datepicker();
        });

        $(function () {
            $("#<%= txtUltModificacion.ClientID %>").datepicker();
        });

        $(function () {
            $("#<%= txtModificado.ClientID %>").datepicker();
        });


  </script>--%>

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

