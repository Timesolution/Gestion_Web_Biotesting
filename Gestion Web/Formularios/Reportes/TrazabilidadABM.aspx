<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TrazabilidadABM.aspx.cs" Inherits="Gestion_Web.Formularios.Reportes.TrazabilidadABM" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">

        <div>
            <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                <ContentTemplate>
                    <div class="col-md-12 col-xs-12">
                        
                        <div class="widget stacked">

                            <div class="widget big-stats-container stacked">
                                <div class="widget-content">
                                    <div id="big_stats" class="cf">
                                        <div class="stat">
                                            <h4><asp:Label ID="lblCodigo" runat="server" Text=""></asp:Label></h4>
                                            <asp:Label ID="lblArticulo" runat="server" Text="" class="value"></asp:Label>
                                        </div>
                                        <!-- .stat -->
                                    </div>
                                </div>
                                <!-- /widget-content -->
                            </div>
                            <!-- /widget -->

                            <div class="widget-header">
                                <i class="icon-road"></i>
                                <h3>Trazabilidad</h3>
                            </div>
                            <!-- /widget-header -->

                            <div class="widget-content">
                                <asp:Panel ID="panelDatos" runat="server" >
                                    <div id="validation-form" role="form" class="form-horizontal col-md-6">
                                        <fieldset>
                                            <asp:PlaceHolder ID="phCampos" runat="server"></asp:PlaceHolder>
                                            <div class="form-group">
                                                <div class="col-md-12">
                                                    <asp:Button ID="btnCargar" runat="server" Text="Agregar" class="btn btn-success" OnClick="btnCargar_Click" />
                                                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" class="btn btn-default" OnClick="btnCancelar_Click" />                                            
                                                    <asp:UpdateProgress ID="UpdateProgress1" AssociatedUpdatePanelID="UpdatePanel1" runat="server" >
                                                        <ProgressTemplate>
                                                            <h4>
                                                                <i class="fa fa-spinner fa-spin"></i> Procesando 
                                                            </h4>
                                                        </ProgressTemplate>
                                                    </asp:UpdateProgress>
                                                </div>
                                            </div>
                                        </fieldset>
                                    </div>
                                </asp:Panel>
                                <asp:Panel ID="panelDetalle" runat="server" Visible="false">
                                    <div id="validation-form" role="form" class="form-horizontal col-md-6">
                                        <fieldset>
                                            <div class="form-group">
                                                <asp:Label ID="lblFechaIngreso" runat="server" class="col-md-2">Fecha Ingreso</asp:Label>
                                                <div class="col-md-6">                                                
                                                    <asp:TextBox ID="txtFechaIngreso" runat="server" class="form-control" disabled/>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label ID="lblSucIngreso" runat="server" class="col-md-2">Suc. Ingreso</asp:Label>
                                                <div class="col-md-6">                                                
                                                    <asp:TextBox ID="txtSucIngreso" runat="server" class="form-control" disabled/>
                                                </div>
                                            </div>
                                            <div class="form-group">                                            
                                                <asp:Label ID="lblRemitoIngreso" runat="server" class="col-md-2">Remito Ingreso</asp:Label>
                                                <div class="col-md-6">
                                                    <asp:TextBox ID="txtRemitoIngreso" runat="server" class="form-control" disabled/>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label ID="lblFechaEgreso" runat="server" class="col-md-2">Fecha Egreso</asp:Label>
                                                <div class="col-md-6">                                                
                                                    <asp:TextBox ID="txtFechaEgreso" runat="server" class="form-control" disabled/>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label ID="lblSucEgreso" runat="server" class="col-md-2">Suc. Egreso</asp:Label>
                                                <div class="col-md-6">                                                
                                                    <asp:TextBox ID="txtSucEgreso" runat="server" class="form-control" disabled/>
                                                </div>
                                            </div>
                                            <div class="form-group">                                            
                                                <asp:Label ID="lblRemitoEgreso" runat="server" class="col-md-2">Remito Egreso</asp:Label>
                                                <div class="col-md-6">
                                                    <asp:TextBox ID="txtRemitoEgreso" runat="server" class="form-control" disabled/>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label ID="lblFactura" runat="server" class="col-md-2">Factura/PRP</asp:Label>
                                                <div class="col-md-6">
                                                    <asp:TextBox ID="txtFactura" runat="server" class="form-control" disabled/>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <asp:Label ID="lblCliente" runat="server" class="col-md-2">Cliente</asp:Label>
                                                <div class="col-md-6">                                                
                                                    <asp:TextBox ID="txtCliente" runat="server" class="form-control" disabled/>
                                                </div>
                                            </div>
                                            <div class="col-md-6">
                                                <asp:Button ID="btnVolver" runat="server" Text="Volver" class="btn btn-default" OnClick="btnVolver_Click" />
                                            </div>
                                        </fieldset>
                                    </div>                                
                                </asp:Panel>
                            </div>
                        </div>
                        <!-- /widget -->
                    </div>

                </ContentTemplate>

            </asp:UpdatePanel>
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
