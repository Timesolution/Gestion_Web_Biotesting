<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ABMTrazabilidad.aspx.cs" Inherits="Gestion_Web.Formularios.Articulos.ABMTrazabilidad" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">


        <div class="row">

            <div class="col-md-12">

                <div class="widget stacked ">

                    <div class="widget-header">
                        <i class="icon-pencil"></i>
                        <h3>Administrador campos Trazabilidad</h3>

                    </div>
                    <!-- /.widget-header -->


                    <div class="widget-content">

                        <div role="form" class="form-horizontal col-md-10">
                            <div class="form-group">
                                <label for="validateSelect" class="col-md-4">Grupo articulo:</label>
                                <div class="col-md-4">
                                    <asp:DropDownList ID="DropListGrupo" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="DropListGrupo_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Seleccione un grupo" ControlToValidate="DropListGrupo" InitialValue="-1" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" ValidationGroup="AdminGroup"></asp:RequiredFieldValidator>
                                </div>
                            </div>

                            <div class="form-group">
                                <label for="validateSelect" class="col-md-4">Nombre campo:</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtNombre" runat="server" class="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-1">
                                    <asp:LinkButton ID="lbtnAgregar" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnAgregar_Click"  ValidationGroup="AdminGroup" />
                                </div>
                            </div>

                        </div>

                    </div>
                    <div class="widget-content">

                        <div class="col-md-12">
                            <table id="tbListas" class="table table-striped table-bordered">
                                <thead>
                                    <tr>                                        
                                        <th style="width: 90%">Campo</th>
                                        <th class="td-actions" style="width: 10%"></th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:PlaceHolder ID="phCamposTrazabilidad" runat="server"></asp:PlaceHolder>
                                </tbody>
                            </table>

                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>

    <div id="modalConfirmacion" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Confirmacion de Eliminacion</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <div class="form-group">
                            <div class="col-md-2">
                                <h1>
                                    <i class="icon-warning-sign" style="color: orange"></i>
                                </h1>
                            </div>
                            <div class="col-md-7">
                                <h5>
                                    <asp:Label runat="server" ID="lblMensaje" Text="Esta seguro que desea eliminar campo de trazabilidad?" Style="text-align: center"></asp:Label>
                                </h5>
                            </div>

                            <div class="col-md-3">
                                <asp:TextBox runat="server" ID="txtMovimiento" Text="0" Style="display: none"></asp:TextBox>
                            </div>
                        </div>
                        <%--                                <div class="form-group">
                                    
                                </div>--%>
                    </div>


                    <div class="modal-footer">
                        <asp:Button runat="server" ID="btnSi" Text="Eliminar" class="btn btn-danger" OnClick="btnSi_Click" />
                        <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>

                        <%--                        <asp:LinkButton ID="LinkButton1" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnBuscar_Click" ValidationGroup="BusquedaGroup" />--%>
                    </div>
                </div>

            </div>
        </div>
    </div>
    <%--Fin modalGrupo--%>

    <script>
        function abrirdialog(valor) {
            document.getElementById('<%= txtMovimiento.ClientID %>').value = valor;
            }
    </script>

    <%-- <!-- Core Scripts - Include with every page -->
  
    <script src="../../Scripts/libs/jquery-1.9.1.min.js"></script>
    <script src="../../Scripts/libs/jquery-ui-1.10.0.custom.min.js"></script>
    <script src="../../Scripts/libs/bootstrap.min.js"></script>

    <script src="../../Scripts/plugins/msgGrowl/js/msgGrowl.js"></script>
    <script src="../../Scripts/plugins/lightbox/jquery.lightbox.min.js"></script>
    <script src="../../Scripts/plugins/msgbox/jquery.msgbox.min.js"></script>

    <script src="../../Scripts/Application.js"></script>

    <%--<script src="../../Scripts/plugins/hoverIntent/jquery.hoverIntent.minified.js"></script>--%>



    <%-- <script src="../../Scripts/demo/gallery.js"></script>

      <script src="../../Scripts/jquery-1.10.2.js"></script>
    <script src="../../Scripts/bootstrap.min.js"></script>--%>
    <%--<script src="../Scripts/plugins/metisMenu/jquery.metisMenu.js"></script>--%>


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

</asp:Content>

