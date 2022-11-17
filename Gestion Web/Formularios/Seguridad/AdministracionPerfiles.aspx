<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdministracionPerfiles.aspx.cs" Inherits="Gestion_Web.Formularios.Seguridad.AdministracionPerfiles" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container" style="margin-bottom:38px;">
        <div class="row">
            <div class="col-md-12 col-xs-12">
            <div class="widget stacked">

                <div class="widget-header">
                    <i class="icon-wrench"></i>
                    <h3>Herramientas</h3>
                </div>
                <!-- /widget-header -->

                <div class="widget-content">
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 55%">
                                <div class="form-group">
                                    <%--<asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Always" runat="server">
                                        <ContentTemplate>--%>
                                    <label class="col-md-4">Permiso </label>
                                    <div class="col-md-7">
                                        <asp:DropDownList ID="DropListPermiso" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="DropListPermiso_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                    <%--</ContentTemplate>
                                        <Triggers>
                                        </Triggers>
                                    </asp:UpdatePanel>--%>
                                </div>
                            </td>

                            <td style="width: 39%">
                                <%--                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListPermiso" InitialValue="-1" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                            </td>
                            <%--<td style="width: 5%">--%>
                            <%--<div class="col-md-1">--%>

                            <%--<div class="shortcuts">

                                    </div>--%>
                            <%--</div>--%>
                            <%--</td>--%>
                            <td style="width: 3%">
                                <%--<div class="col-md-1">--%>
                                <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Always" runat="server">
                                    <ContentTemplate>
                                        <%--<div class="shortcuts">--%>
                                        <asp:LinkButton ID="lbtnCheckAll" runat="server" Text="<span class='shortcut-icon icon-check'></span>" class="btn btn-success" OnClientClick="javascript:CheckAll();" />

                                        <%--</div>--%>
                                    </ContentTemplate>
                                    <Triggers>
                                    </Triggers>
                                </asp:UpdatePanel>
                                <%--</div>--%>
                            </td>
                            <td style="width: 3%">
                                <%--<div class="col-md-1">--%>
                                <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Always" runat="server">
                                    <ContentTemplate>
                                        <%--<div class="shortcuts">--%>
                                        <asp:LinkButton ID="lbtnCheckNone" runat="server" Text="<span class='shortcut-icon icon-remove'></span>" class="btn btn-success" OnClientClick="javascript:CheckNone();" />

                                        <%--</div>--%>
                                    </ContentTemplate>
                                    <Triggers>
                                    </Triggers>
                                </asp:UpdatePanel>
                                <%--</div>--%>
                            </td>
                        </tr>

                    </table>
                </div>
                <!-- /widget-content -->

            </div>
            <!-- /widget -->
        </div>
        </div>

        <div class="row">
            <div class="col-md-12 col-xs-12">
            <div class="widget stacked widget-table action-table">

                <div class="widget-header">
                    <i class="icon-th-list"></i>
                    <h3>Administracion de Permisos

                    </h3>
                </div>
                <div class="widget-content">
                    <div class="panel-body">

                        <%--<div class="col-md-12 col-xs-12">--%>
                        <%--<asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
                            <ContentTemplate>--%>
                        <div class="table-responsive">
                            <a class="btn btn-info" style="display: none" data-toggle="modal" id="abreDialog" href="#modalFacturaDetalle">Agregar Tipo Cliente</a>
                            <table class="table table-bordered table-striped" id="tbMenus">
                                <thead>
                                    <tr>
                                        <th style="width: 95%">Pagina</th>
                                        <th style="width: 5%">Permitir</th>
                                    </tr>

                                </thead>
                                <tbody>
                                    <asp:PlaceHolder ID="phMenus" runat="server"></asp:PlaceHolder>
                                </tbody>
                            </table>

                        </div>


                        <%--</ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>--%>
                    </div>


                    <!-- /.content -->

                </div>

            </div>
        </div>
        </div>

        <div class="row">
            <div class="col-md-12">
                <asp:LinkButton ID="lbtnActualizar" runat="server" Text="Guardar" class="btn btn-success" OnClick="lbtnActualizar_Click" />

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
        function CheckAll() {
            var total = 0;
            for (var i = 1; i < document.getElementById('tbMenus').rows.length; i++) {
                document.getElementById('tbMenus').rows[i].cells[1].childNodes[0].checked = true;
            }
        }
    </script>

    <script>
        function CheckNone() {
            var total = 0;
            for (var i = 1; i < document.getElementById('tbMenus').rows.length; i++) {
                document.getElementById('tbMenus').rows[i].cells[1].childNodes[0].checked = false;
            }
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
                else { return false; }
            }
            return true;
        }
    </script>


</asp:Content>


