<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ComparativaProveedorF.aspx.cs" Inherits="Gestion_Web.Formularios.Articulos.ComparativaProveedorF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">

        <div class="container">

            <div class="col-md-12 col-xs-12">
                <div class="widget stacked">

                    <div class="widget-header">
                        <i class="icon-wrench"></i>
                        <h3>Herramientas</h3>
                    </div>
                    <!-- /widget-header -->

                    <div class="widget-content">
                        <%--<div role="form" class="form-horizontal col-md-12">

                            <div class="form-group">
                                <label class="col-md-3">Buscar Proveedor</label>
                                <div class="col-md-3">
                                    <asp:TextBox ID="txtCodProveedor" class="form-control" runat="server"></asp:TextBox>
                                </div>

                                <div class="col-md-2">
                                    <asp:LinkButton ID="btnBuscarProveedor" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" OnClick="btnBuscarProveedor_Click" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-3">Proveedor</label>
                                <div class="col-md-3">
                                    <asp:DropDownList ID="ListProveedor" runat="server" AutoPostBack="true" class="form-control"></asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    <asp:LinkButton ID="lbtnAgregarProveedor" runat="server" Text="Agregar" class="btn btn-success" OnClick="lbtnAgregarProveedor_Click" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-3">Proveedor numero</label>
                                <div class="col-md-3">
                                    <asp:DropDownList ID="ListProveedorNumero" runat="server" AutoPostBack="true" class="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </div>--%>
                        <div role="form" class="form-horizontal col-md-12">

                            <div class="form-group">
                                <label class="col-md-3">Codigo Articulo</label>
                                <div class="col-md-3">
                                    <asp:TextBox ID="txtCodArticulo" class="form-control" runat="server"></asp:TextBox>
                                </div>

                                <div class="col-md-2">
                                    <asp:LinkButton ID="btnBuscarArticulo" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" OnClick="btnBuscarArticulo_Click" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-3">Articulo</label>
                                <div class="col-md-3">
                                    <asp:DropDownList ID="ListArticulos" runat="server" AutoPostBack="true" class="form-control"></asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    <asp:LinkButton ID="lbtnAgregarArticulo" runat="server" Text="Agregar" class="btn btn-success" OnClick="lbtnAgregarArticulo_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- /widget-content -->

                </div>
                <!-- /widget -->
            </div>
            <div class="col-md-12 col-xs-12">
                <div class="widget stacked widget-table action-table">

                    <div class="widget-header">
                        <i class="icon-th-list"></i>
                        <h3>Estados

                        </h3>
                    </div>
                    <div class="widget-content">
                        <div class="panel-body">

                            <%--<div class="col-md-12 col-xs-12">--%>
                            <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <asp:GridView ID="GridTable" runat="server" CssClass="table table-striped" AutoGenerateColumns="False"
                                        EmptyDataText="Sin datos." OnRowCreated="GridTable_RowCreated" OnRowDataBound="GridTable_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="idArticulo" HeaderText="idArticulo" Visible="false" />
                                            <asp:BoundField DataField="idProveedor" HeaderText="idProveedor" Visible="false" />
                                            <asp:BoundField DataField="Codigo" HeaderText="Codigo" />
                                            <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
                                            <asp:BoundField DataField="Proveedor" HeaderText="Proveedor" />
                                            <asp:BoundField DataField="Precio" HeaderText="Precio" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                                        </Columns>
                                    </asp:GridView>


                                </ContentTemplate>
                                <Triggers>
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>

                    </div>

                </div>
            </div>

        </div>

        <script src="../../Scripts/plugins/dataTables/jquery-2.0.0.js"></script>
        <script src="../../Scripts/plugins/dataTables/jquery.dataTables.min.js"></script>
        <script src="../../Scripts/plugins/dataTables/custom.tables.js"></script>


        <!-- Core Scripts - Include with every page -->
        <script src="../../Scripts/jquery-1.10.2.js"></script>
        <script src="../../Scripts/bootstrap.min.js"></script>
        <script src="../Scripts/plugins/metisMenu/jquery.metisMenu.js"></script>

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

        <%--<script src="//code.jquery.com/jquery-1.9.1.js"></script>--%>
        <script src="//code.jquery.com/ui/1.10.4/jquery-ui.js"></script>

        <%--    <script>


        $(function () {
            $("#<%= txtFechaDesde.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

        $(function () {
            $("#<%= txtFechaHasta.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

  </script>--%>

        <script>

            function msgSucces() {
                $.msgGrowl({
                    type: 'success'
                    , title: 'Carga Exitosa'
                    , text: 'Se cargaron los proveedores del articulo.'
                });
            }
            
        </script>

        <!-- Page-Level Plugin Scripts - Tables -->
        <script src="//cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>
        <script src="../../Scripts/plugins/dataTables/custom.tables.js"></script>
        <link href="//cdn.datatables.net/1.10.2/css/jquery.dataTables.css" rel="stylesheet" />



        <script>
            $(document).ready(function () {
                $('#dataTables-example').dataTable({ "bFilter": false, "bInfo": false, "bPaginate": false });
            });
        </script>

        <script type="text/javascript">
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endReq);
            function endReq(sender, args) {
                $('#dataTables-example').dataTable({ "bFilter": false, "bInfo": false, "bPaginate": false });
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
                    if (key == 46 || key == 8) // Detectar . (punto) y backspace (retroceso)
                    { return true; }
                    else { return false; }
                }
                return true;
            }
        </script>

    </div>
</asp:Content>

