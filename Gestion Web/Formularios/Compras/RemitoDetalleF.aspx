<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RemitoDetalleF.aspx.cs" Inherits="Gestion_Web.Formularios.Compras.RemitoDetalleF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="main">

        <div class="container">

            <div class="col-md-12 col-xs-12">
                <div class="widget stacked">
                    <div class="stat">                        
                        <h5><i class="icon-map-marker"></i> Compras > Remito Detalle</h5>
                    </div>
                    <div class="widget-header">
                        <i class="icon-wrench"></i>
                        <h3>Herramientas</h3>
                    </div>
                    <!-- /widget-header -->

                    <div class="widget-content">
                        <h4>Fecha:
                                    <asp:Literal ID="LitFecha" runat="server"></asp:Literal>
                        </h4>
                        <h4>Numero:
                                    <asp:Literal ID="LitNumero" runat="server"></asp:Literal>
                        </h4>
                        <h4>Tipo:
                                    <asp:Literal ID="LitTipo" runat="server"></asp:Literal>
                        </h4>
                        <h4>Proveedor:                            
                                    <asp:Literal ID="LitProveedor" runat="server"></asp:Literal>
                        </h4>
                        <h4>Sucursal:
                                    <asp:Literal ID="LitSucursal" runat="server"></asp:Literal>
                        </h4>
                        <h4>Observacion:
                                    <asp:Literal ID="LitComentario" runat="server"></asp:Literal>
                        </h4>
                    </div>
                    <!-- /widget-content -->

                </div>
                <!-- /widget -->
            </div>

            <div class="col-md-12 col-xs-12">
                <div class="widget widget-table">
                    <div class="widget-header">
                        <i class="icon-th-list" style="width: 2%"></i>
                        <h3 style="width: 75%">Detalle
                        </h3>
                        <h3>
                            <asp:Label ID="lblSaldo" runat="server" Style="text-align: right" Text="" ForeColor="#666666" Font-Bold="true"></asp:Label>
                        </h3>
                    </div>
                    <div class="widget-content">
                        <div class="panel-body">

                            <div class="table-responsive">
                                <a class="btn btn-info" style="display: none" data-toggle="modal" id="abreDialog" href="#modalFacturaDetalle">Agregar Tipo Cliente</a>
                                <table class="table table-striped table-bordered table-hover" id="dataTables-example">
                                    <thead>
                                        <tr>
                                            <th>Codigo</th>
                                            <th>Descripcion</th>
                                            <th>Cantidad</th>

                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:PlaceHolder ID="phRemito" runat="server"></asp:PlaceHolder>
                                    </tbody>
                                </table>

                            </div>
                        </div>


                        <!-- /.content -->

                    </div>

                </div>
            </div>
        </div>
        
    </div>

</asp:Content>
