<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ConsultaFactura.aspx.cs" Inherits="Gestion_Web.Formularios.Zonas.ConsultaFactura" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container">
        <div class="row">

            <div class="col-md-12">

                <div class="widget stacked">

                    <div class="widget-header">
                        <i class="icon-pencil"></i>
                        <h3>Consulta</h3>
                    </div>
                    <!-- /.widget-header -->

                    <div class="widget-content">
                        <div class="bs-example">
                            <ul id="myTab" class="nav nav-tabs">
                                <li class="active"><a href="#home" data-toggle="tab">Detalle</a></li>
                            </ul>
                            <div class="tab-content">
                                <div class="tab-pane fade active in" id="home">
                                    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Always" runat="server">
                                        <ContentTemplate>
                                            <div role="form" class="form-horizontal col-md-12">
                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Cuit</label>

                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtCuit" runat="server" class="form-control" ></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Tipo Factura</label>

                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtTipoFactura" runat="server" class="form-control" ></asp:TextBox>
                                                    </div>
                                                </div>
                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Numero Comprobante</label>

                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtComprobante" runat="server" class="form-control" ></asp:TextBox>
                                                    </div>
                                                </div>

                                                <div class="form-group">
                                                    <label for="name" class="col-md-4">Punto Venta</label>

                                                    <div class="col-md-4">
                                                        <asp:TextBox ID="txtPuntoVenta" runat="server" class="form-control" ></asp:TextBox>
                                                    </div>
                                                </div>

                                            </div>

                                            <div class="col-md-8">
                                                <asp:Button ID="btnAgregar" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnAgregar_Click"/>
                                                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" class="btn btn-default" PostBackUrl="../Zonas/ZonasF.aspx" />
                                                <asp:Button ID="btnPlenario" Text="PRUEBA PLENARIO" runat="server" OnClick="btnPlenario_Click" />                                                
                                            </div>

                                        </ContentTemplate>
                                        <Triggers>
                                        </Triggers>
                                    </asp:UpdatePanel>

                                </div>
                            </div>

                        </div>


                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
