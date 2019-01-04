<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MateriasPrimas_Composicion.aspx.cs" Inherits="Gestion_Web.Formularios.MateriasPrimas.ComposicionMateriaPrima" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <asp:UpdatePanel runat="server" ID="updatePanel1">
        <ContentTemplate>
            <%--ENCABEZADO DE FORMULARIO DE DATOS--%>
            <div class="col-md-12 col-xs-12">
                <div class="widget stacked">
                    <div class="stat">
                        <h5><i class="icon-map-marker"></i>Maestro > Materias Primas</h5>
                    </div>
                    <div class="widget-header">
                        <i class="icon-wrench"></i>
                        <h3>Herramientas</h3>
                    </div>

                    <div class="widget-content">
                        <div role="form" class="form-horizontal col-md-6">
                            <fieldset>

                                <div class="form-group">
                                    <label for="name" class="col-md-3">Busqueda de materia prima:</label>
                                    <div class="col-md-5">
                                        <div class="input-group">
                                            <asp:TextBox ID="txtMPBusqueda" runat="server" class="form-control" placeholder="Buscar Materia Prima"></asp:TextBox>
                                            <span class="input-group-btn">
                                                <asp:LinkButton ID="lbtnBuscarMP" runat="server" Text="Buscar" class="btn btn-primary" OnClick="lbtnBuscarMP_Click">
					                                        <i class="shortcut-icon icon-search"></i></asp:LinkButton>
                                            </span>
                                        </div>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Ingrese un Codigo" ControlToValidate="txtMPBusqueda" ValidationGroup="BusquedaMateriaPrima" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label for="name" class="col-md-3">Materias Primas:</label>
                                    <div class="col-md-5">
                                        <asp:DropDownList ID="ListMPBusqueda" runat="server" class="form-control" AutoPostBack="false"></asp:DropDownList>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <label for="name" class="col-md-3">Cantidad:</label>
                                    <div class="col-md-5">
                                        <asp:TextBox ID="txtCantidadMP" runat="server" class="form-control" Style="text-align: right;" onkeypress="javascript:return validarNro(event)" />
                                    </div>
                                    <div class="col-md-3">
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Ingrese una cantidad" ControlToValidate="txtCantidadMP" ValidationGroup="botonAgregarMP" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <div class="col-md-5"></div>
                                    <div class="col-md-1">
                                        <asp:LinkButton ID="lbtnAgregarMP" runat="server" class="btn btn-success" Text="<i class='shortcut-icon icon-ok'></i>" OnClick="lbtnAgregarMP_Click" AutoPostBack="true" ValidationGroup="botonAgregarMP" />
                                    </div>
                                    <div class="col-md-5">
                                        <asp:LinkButton ID="lbtnCancelarEdicion" runat="server" href="../Articulos/Articulos.aspx" class="btn btn-danger" Text="cancelar" />
                                    </div>
                                </div>

                            </fieldset>
                        </div>
                    </div>
                </div>
                <!-- /widget -->
            </div>
            <%--FIN ENCABEZADO DE FORMULARIO DE DATOS--%>

            <%--TABLA DE LAS COMPOSICIONES--%>
            <div class="col-md-12 col-xs-12">
                <div class="widget stacked widget-table action-table">
                    <div class="widget-header">
                        <i class="icon-bookmark"></i>
                        <h3>Materias Primas</h3>
                    </div>
                    <div class="widget-content">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                            <ContentTemplate>
                                <div class="panel-body">
                                    <div class="table-responsive">
                                        <table class="table table-striped table-bordered table-hover" id="dataTables-example">
                                            <thead>
                                                <tr>
                                                    <th>Codigo</th>
                                                    <th>Descripcion</th>
                                                    <th>Cantidad</th>
                                                    <th>Unidad</th>
                                                    <th>Importe</th>
                                                    <th>Total</th>
                                                    <th class="td-actions"></th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <asp:PlaceHolder ID="phComposiciones" runat="server"></asp:PlaceHolder>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <%--FIN TABLA DE LAS COMPOSICIONES--%>
        </ContentTemplate>
    </asp:UpdatePanel>

    <%--MODAL CONFIRMACION--%>
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
                                    <asp:Label runat="server" ID="lblMensaje" Text="Esta seguro que desea eliminar la composicion?" Style="text-align: center"></asp:Label>
                                </h5>
                            </div>

                            <div class="col-md-3">
                                <asp:TextBox runat="server" ID="txtMovimiento" Text="0" Style="display: none"></asp:TextBox>
                            </div>
                        </div>

                    </div>


                    <div class="modal-footer">
                        <asp:Button runat="server" ID="btnSi" Text="Eliminar" class="btn btn-danger" OnClick="btnSi_Click" />
                        <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>
                    </div>
                </div>

            </div>
        </div>
    </div>
    <%--FIN MODAL CONFIRMACION--%>


    <script>
        function abrirdialog(valor) {
            document.getElementById('<%= txtMovimiento.ClientID %>').value = valor;
        }
    </script>
</asp:Content>
