<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MateriasPrimasF.aspx.cs" Inherits="Gestion_Web.Formularios.MateriasPrimas.MateriasPrimasF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

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
                <%--"TABLA HERRAMIENTAS"--%>
                <table style="width: 100%">
                    <tr>
                        <td style="width: 30%">
                            <div class="col-md-12">
                                <div class="input-group">
                                    <span class="input-group-btn">
                                        <%--<asp:LinkButton ID="lbBuscar" runat="server" Text="Buscar" class="btn btn-primary" OnClick="btnBuscar_Click"><i class="shortcut-icon icon-search"></i></asp:LinkButton>--%>
                                    </span>
                                    <asp:TextBox ID="txtBusqueda" runat="server" class="form-control" placeholder="Buscar Articulo"></asp:TextBox>
                                </div>
                            </div>
                        </td>
                        <td style="width: 3%">
                            <a href="MateriasPrimasABM.aspx?a=1" class="btn btn-primary ui-tooltip" data-toggle="tooltip" title data-original-title="Agregar" style="width: 100%">
                                <i class="shortcut-icon icon-plus"></i>
                            </a>
                        </td>
                    </tr>
                </table>
                <%--FIN TABLA HERRAMIENTAS--%>
            </div>
        </div>
        <!-- /widget -->
    </div>

    <div class="col-md-12 col-xs-12">
        <div class="widget stacked widget-table action-table">

            <div class="widget-header">
                <i class="icon-bookmark"></i>
                <h3>Materias Primas</h3>
            </div>
            <div class="widget-content">
                <div class="panel-body">
                    <div class="table-responsive">
                        <%--"TABLA DE MATERIAS PRIMAS"--%>
                        <table class="table table-striped table-bordered table-hover" id="dataTables-example">
                            <thead>
                                <tr>
                                    <th runat="server" style="width: 11%">Codigo</th>
                                    <th runat="server" style="width: 23%">Descripcion</th>
                                    <th runat="server" style="width: 10%">Stock Minimo</th>
                                    <th runat="server" style="width: 20%">Unidad medida</th>
                                    <th runat="server" style="width: 10%">Importe</th>
                                    <th runat="server" style="width: 15%">Moneda</th>

                                    <th runat="server" class="td-actions" style="width: 1%"></th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:PlaceHolder ID="phMateriasPrimas" runat="server"></asp:PlaceHolder>
                            </tbody>
                        </table>
                        <%--FIN TABLA MATERIAS PRIMAS--%>
                    </div>
                </div>
            </div>
        </div>
    </div>


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
                                    <asp:Label runat="server" ID="lblMensaje" Text="Esta seguro que desea eliminar la materia prima?" Style="text-align: center"></asp:Label>
                                </h5>
                            </div>

                            <div class="col-md-3">
                                <asp:TextBox runat="server" ID="txtMovimiento" Text="0" Style="display: none"></asp:TextBox>
                            </div>
                        </div>

                    </div>

                    <div class="modal-footer">
                        <asp:Button runat="server" ID="btnEliminarMateriaPrima" Text="Eliminar" class="btn btn-danger" OnClick="btnEliminarMateriaPrima_Click" />
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
