<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TrazabilidadF.aspx.cs" Inherits="Gestion_Web.Formularios.Compras.TrazabilidadF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">

        <div>
            <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                <ContentTemplate>
                    <div class="col-md-12 col-xs-12">

                        <div class="widget stacked">
                            <div class="stat">
                                <h5><i class="icon-map-marker"></i>Compras > Remitos > Trazabilidad</h5>
                            </div>
                            <div class="widget-header">
                                <i class="icon-shopping-cart"></i>
                                <h3>Remito Compra</h3>
                            </div>
                            <div class="widget-content">
                                <div class="table-responsive">
                                    <table class="table table-striped table-bordered table-hover" id="dataTables-example">
                                        <thead>
                                            <tr>
                                                <th style="width: 15%">Codigo</th>
                                                <th style="width: 55%">Descripcion</th>
                                                <th style="width: 10%">Cantidad</th>
                                                <th style="width: 20%"></th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:PlaceHolder ID="phItems" runat="server"></asp:PlaceHolder>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </div>

                        <div class="widget stacked">

                            <asp:PlaceHolder ID="phCantidadDeRegistros" runat="server" Visible="false">
                                <div class="col-md-12">
                                    <div class="widget big-stats-container stacked">
                                        <div class="widget-content">
                                            <div id="big_stats" class="cf">

                                                <div class="stat">
                                                    <h3><asp:Label runat="server" ID="lbNombreColumnaTrazaUnidadMedida"></asp:Label></h3>
                                                    <span class="value">
                                                        <asp:Label runat="server" ID="lbCantidadTotal"></asp:Label>
                                                    </span>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:PlaceHolder>

                            <div class="widget-header">
                                <i class="icon-road"></i>
                                <h3>Trazabilidad</h3>
                            </div>

                            <div class="widget-content">
                                <div id="validation-form" role="form" class="form-horizontal col-md-10">
                                    <fieldset>
                                        <asp:PlaceHolder ID="phCampos" runat="server"></asp:PlaceHolder>
                                        <div class="form-group">
                                            <label id="lblCantidadTrazas" runat="server" class="col-md-2">Cantidad </label>
                                            <div class="col-md-1">
                                                <asp:TextBox ID="txtCantidadTrazas" Text="1" runat="server" class="form-control text-right"></asp:TextBox>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <div class="col-md-2">
                                                <asp:Button ID="btnCargar" runat="server" Text="Agregar" class="btn btn-success" OnClick="btnCargar_Click" />
                                            </div>
                                            <div class="col-md-2">
                                                <asp:Button ID="btnCargarPorCantidad" runat="server" Text="Agregar por Cantidad" class="btn btn-success" OnClick="btnCargarPorCantidad_Click" />
                                            </div>
                                            <div class="col-md-2">
                                                <asp:Button ID="btnImportarPorCSV" Visible="false" runat="server" Text="Importar por CSV" class="btn btn-success" href="#modalImportarCSV" data-toggle="modal" />
                                            </div>
                                        </div>
                                    </fieldset>
                                </div>
                            </div>
                            &nbsp
                           
                            <div class="widget-header">
                                <i class="icon icon-info"></i>
                                <h3>
                                    <asp:Label ID="lblCodigo" runat="server" Text="0" class="value"></asp:Label>
                                    <asp:Label ID="lblDescripcion" runat="server" Text="0" class="value"></asp:Label>
                                    (
                                   
                                    <asp:Label ID="lblCantCargada" runat="server" Text="" class="value" />
                                    /
                                   
                                    <asp:Label ID="lblCantidad" runat="server" Text="0" class="value" />
                                    )
                                </h3>
                            </div>
                            <div class="widget-content">

                                <div class="form-group">
                                    <table class="table table-striped table-bordered">
                                        <thead>
                                            <tr>
                                                <th>#</th>
                                                <asp:PlaceHolder ID="phTabla" runat="server"></asp:PlaceHolder>
                                                <th></th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:PlaceHolder ID="phTrazabilidad" runat="server"></asp:PlaceHolder>
                                        </tbody>
                                    </table>

                                </div>
                            </div>
                            <!-- /widget-content -->

                        </div>
                        <!-- /widget -->
                        <div class="form-group">
                            <asp:Button ID="btnAgregar" runat="server" Text="Guardar" class="btn btn-success" Visible="false" />
                        </div>
                    </div>

                </ContentTemplate>

            </asp:UpdatePanel>
        </div>
    </div>

    <div id="modalImportarCSV" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Importar CSV</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Always">
                            <ContentTemplate>
                                <div class="form-group">
                                    <label for="validateSelect" class="col-md-2">Archivo:</label>
                                    <div class="col-md-6">
                                        <asp:FileUpload ID="FileUpload" runat="server" />
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:Button ID="btnImportarTrazasByExcel" runat="server" Text="Importar" class="btn btn-success" OnClick="btnImportarTrazasByExcel_Click" />
                </div>
            </div>
        </div>
    </div>

    <link href="../../css/pages/reports.css" rel="stylesheet">

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

    <script src="../../Scripts/JSFunciones1.js"></script>


    <script src="//code.jquery.com/ui/1.10.4/jquery-ui.js"></script>

    <!-- Page-Level Plugin Scripts - Tables -->
    <script src="//cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>
    <script src="../../Scripts/plugins/dataTables/custom.tables.js"></script>
    <link href="//cdn.datatables.net/1.10.2/css/jquery.dataTables.css" rel="stylesheet" />


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
                {
                    return true;
                }
                else {
                    return false;
                }
            }
            return true;
        }
    </script>


</asp:Content>
