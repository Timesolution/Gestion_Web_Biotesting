<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportesArticulos.aspx.cs" Inherits="Gestion_Web.Formularios.Reportes.ReportesArticulos" %>

<%--<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="main">
        <%--<div class="container">--%>
        <div>
            <div class="col-md-12">



                <div class="row">
                    <div class="col-md-12 col-xs-12">
                        <div class="widget stacked">

                            <div class="widget-header">
                                <i class="icon-wrench"></i>
                                <h3>Herramientas</h3>
                            </div>
                            <div class="widget-content">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 20%">
                                            <asp:PlaceHolder ID="phAcciones" runat="server">
                                                <div class="btn-group">
                                                    <button type="button" class="btn btn-success dropdown-toggle" data-toggle="dropdown" id="btnAccion" runat="server">Accion    <span class="caret"></span></button>
                                                    <ul class="dropdown-menu">
                                                        <li>
                                                            <asp:LinkButton ID="lbtnImprimir" runat="server" OnClick="lbtnImprimir_Click">Imprimir reporte x Sucursal</asp:LinkButton>
                                                        </li>
                                                        <li>
                                                            <asp:LinkButton ID="lbtnExportar" runat="server" OnClick="lbtnExportar_Click">Exportar reporte x Sucursal</asp:LinkButton>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </asp:PlaceHolder>
                                        </td>

                                        <td style="width: 95%">
                                            <h5>
                                                <asp:Label runat="server" ID="lblParametros" Text="" ForeColor="#cccccc"></asp:Label>
                                            </h5>
                                        </td>

                                        <td style="width: 5%">
                                            <div class="shortcuts" style="height: 100%">
                                                <a class="btn btn-primary" data-toggle="modal" href="#modalBusqueda" style="width: 100%">
                                                    <i class="shortcut-icon icon-filter"></i>
                                                </a>
                                            </div>
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

                    <div class="col-md-12">

                        <div class="widget big-stats-container stacked">

                            <div class="widget-content">

                                <div id="big_stats" class="cf">
                                    <div class="stat">
                                        <h5>Sucursal</h5>
                                        <asp:Label ID="lblSucursal" runat="server" Text="" class="value"></asp:Label>
                                    </div>
                                    <!-- .stat -->

                                    <%--<div class="stat">
                                        <h5>Lista Precio</h5>
                                        <asp:Label ID="lblLista" runat="server" Text="" class="value"></asp:Label>
                                    </div>--%>
                                    <!-- .stat -->
                                </div>

                            </div>
                            <!-- /widget-content -->

                        </div>
                        <!-- /widget -->

                    </div>
                    <!-- /span12 -->

                </div>
                <!-- /row -->

                <div class="row">
                    <div class="col-md-12 col-xs-12">
                        <div class="widget stacked">

                            <div class="widget-header">
                                <i class="icon-list"></i>
                                <h3>Top´s</h3>
                            </div>
                            <div class="widget-content">
                                <div class="bs-example">
                                    <ul id="myTab" class="nav nav-tabs">
                                        <li class="active"><a href="#home" data-toggle="tab">Articulos</a></li>                                        
                                    </ul>
                                    <div class="tab-content">
                                        <div class="tab-pane fade active in" id="home">
                                            <div class="col-md-6">

                                                <div class="widget stacked widget-table">

                                                    <div class="widget-header">
                                                        <span class="icon-list-alt"></span>
                                                        <h3>Cantidad</h3>
                                                    </div>
                                                    <!-- .widget-header -->

                                                    <div class="widget-content">
                                                        <table class="table table-bordered table-striped">

                                                            <thead>
                                                                <tr>
                                                                    <th style="text-align: left; width: 30%">Codigo</th>
                                                                    <th style="text-align: left; width: 40%">Descripcion</th>
                                                                    <th style="text-align: right; width: 30%">Cantidad</th>
                                                                </tr>

                                                            </thead>

                                                            <tbody>
                                                                <asp:PlaceHolder ID="phTopArticulosCantidad" runat="server"></asp:PlaceHolder>
                                                            </tbody>

                                                        </table>

                                                    </div>
                                                    <!-- .widget-content -->

                                                </div>
                                                <!-- /widget -->

                                            </div>
                                            <!-- /span6 -->
                                            <div class="col-md-6">

                                                <div class="widget stacked widget-table">

                                                    <div class="widget-header">
                                                        <span class="icon-list-alt"></span>
                                                        <h3>Importe</h3>
                                                    </div>
                                                    <!-- .widget-header -->

                                                    <div class="widget-content">
                                                        <table class="table table-bordered table-striped">

                                                            <thead>
                                                                <tr>
                                                                    <th style="text-align: left; width: 30%">Codigo</th>
                                                                    <th style="text-align: left; width: 40%">Descripcion</th>
                                                                    <th style="text-align: right; width: 30%">Total</th>
                                                                </tr>

                                                            </thead>

                                                            <tbody>
                                                                <asp:PlaceHolder ID="phTopArticulosImporte" runat="server"></asp:PlaceHolder>
                                                            </tbody>

                                                        </table>

                                                    </div>
                                                    <!-- .widget-content -->

                                                </div>
                                                <!-- /widget -->

                                            </div>
                                            <!-- /span6 -->
                                        </div>

                                        

                                    </div>
                                </div>
                            </div>
                            <!-- /widget-content -->

                        </div>
                        <!-- /widget -->
                    </div>
                </div>


            </div>
            <!-- / col-md-12 -->
        </div>


        <div id="modalBusqueda" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">

                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Busqueda</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <asp:UpdatePanel ID="UpdatePanel4" UpdateMode="Always" runat="server">
                                <ContentTemplate>
                                    <div class="form-group">
                                        <label class="col-md-4">Desde</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtFechaDesde" runat="server" class="form-control"></asp:TextBox>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-4">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaDesde" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Hasta</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtFechaHasta" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaHasta" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                        <!-- /input-group -->
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Sucursal</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListSucursal" runat="server" class="form-control"></asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListSucursal" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    
                                    <div class="form-group">
                                        <label class="col-md-4">Desc Articulo</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtDescArticulo" class="form-control" runat="server"></asp:TextBox>
                                        </div>

                                        <div class="col-md-2">
                                            <asp:LinkButton ID="btnBuscarCod" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" OnClick="btnBuscarCod_Click" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Articulo</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListArticulos" runat="server" class="form-control"></asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListArticulos" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>    
                                    <div class="form-group">
                                        <label class="col-md-4">Proveedor</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListProveedor" runat="server" class="form-control"></asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                        
                                    </div>      
                                    <div class="form-group">
                                        <label class="col-md-4">Listas de precio</label>
                                    </div>
                                    <div class="form-group">
                                        <%--<label class="col-md-4">Listas de precio</label>--%>
                                        <div class="col-md-10">
                                            <asp:CheckBoxList ID="chkListListas" runat="server" RepeatLayout="table" RepeatColumns="2" RepeatDirection="horizontal">
                                            </asp:CheckBoxList>
                                        </div>
                                    </div>                          
                                </ContentTemplate>
                                <Triggers>
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>


                    <div class="modal-footer">
                        <asp:LinkButton ID="lbtnBuscar" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnBuscar_Click" ValidationGroup="BusquedaGroup" />
                    </div>
                </div>

            </div>
        </div>
        <!-- /container -->

    </div>
    <!-- /main -->



    <link href="../../css/pages/reports.css" rel="stylesheet">
    <<!-- Core Scripts - Include with every page -->
    <script src="../../Scripts/jquery-1.10.2.js"></script>
    <script src="../../Scripts/bootstrap.min.js"></script>

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
        function pageLoad() {
            $("#<%= txtFechaDesde.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });

            $("#<%= txtFechaHasta.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        }
    </script>

    <script>

        $(function () {
            $("#<%= txtFechaDesde.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

        $(function () {
            $("#<%= txtFechaHasta.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

    </script>

</asp:Content>
