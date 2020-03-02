<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProveedoresF.aspx.cs" Inherits="Gestion_Web.Formularios.Proveedores.ProveedoresF" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="main">

        <div class="container">

            <div class="row">

                <div class="col-md-6 col-xs-12">

                    <div class="widget stacked widget-table action-table">

                        <div class="widget-header">
                            <i class="icon-th-list"></i>
                            <h3>Proveedores</h3>
                        </div>
                        <!-- /widget-header -->


                        <div class="widget-content">
                           

                            <table id="tbProveedores" class="table table-striped table-bordered">
                                <thead>
                                    <tr>
                                        <th style="width: 40%">Razon Social</th>
                                        <th style="width: 40%">Alias</th>
                                        <th class="td-actions" style="width: 20%"></th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:PlaceHolder ID="phProveedores" runat="server"></asp:PlaceHolder>
                                </tbody>
                            </table>

                        </div>
                        <!-- /widget-content -->

                    </div>
                    <!-- /widget -->


                </div>



                <div class="col-md-6">


                    <div class="widget stacked">

                        <div class="widget-header">
                            <i class="icon-wrench"></i>
                            <h3>Herramientas</h3>
                        </div>
                        <!-- /widget-header -->

                        <div class="widget-content">
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 70%">
                                        <div class="col-md-12">
                                            <div class="input-group">
                                                <asp:TextBox ID="txtBusqueda" runat="server" class="form-control" placeholder="Buscar Proveedor"></asp:TextBox>
                                                <span class="input-group-btn">
                                                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" class="btn btn-primary" OnClick="btnBuscar_Click"/>
                                                    <%--<button class="btn btn-primary" type="button">Buscar!</button>--%>
                                                </span>
                                            </div>
                                            <!-- /input-group -->
                                        </div>
                                    </td>

                                    <td style="width: 15%">
                                        <a href="/../../Formularios/Clientes/ClientesABM.aspx?accion=3" class="btn btn-default" style="width: 100%">
                                            <i class="shortcut-icon icon-plus"></i>
                                            <%--<span class="shortcut-label">Users</span>--%>
                                        </a>
                                    </td>

                                    <td style="width: 15%">

                                        <%--<asp:LinkButton ID="LinkButton1" runat="server" class="shortcut" OnClick="LinkButton1_Click"></asp:LinkButton>--%>

                                        <a href="/" class="btn btn-default" style="width: 100%" runat="server" id="linkEditar">

                                            <i class="shortcut-icon icon-pencil"></i>
                                            <%--<span class="shortcut-label">Users</span>--%>
                                        </a>

                                    </td>


                                </tr>
                                <%--<tr>
                            <td>Saldo Cta Cte:
                            </td>
                            <td>$250.89
                            </td>

                        </tr>
                        <tr>
                            <td>Ultima Cobranza:
                            </td>
                            <td>20/01/2014
                            </td>
                            <td></td>
                        </tr>--%>
                            </table>
                        </div>
                        <!-- /widget-content -->

                    </div>
                    <!-- /widget -->

                    <div class="widget stacked">

                        <div class="widget-header">
                            <i class="icon-file"></i>
                            <h3>Detalle</h3>
                        </div>
                        <!-- /widget-header -->

                        <div class="widget-content">

                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <div class="plan-features">

                                        <ul>
                                            <asp:Label ID="Label1" runat="server"></asp:Label>
                                            <asp:PlaceHolder ID="phProveedorEncabezado" runat="server"></asp:PlaceHolder>
                                            <asp:PlaceHolder ID="phProveedorDetalle" runat="server"></asp:PlaceHolder>


                                        </ul>

                                    </div>
                                    <!-- /plan-features -->
                                </ContentTemplate>
                                <Triggers>
                                    <%-- <asp:AsyncPostBackTrigger ControlID="btnAgregar" EventName="Click" />--%>
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                    <!-- /widget-content -->

                </div>
                <!-- /widget -->

            </div>
            <!-- /row -->
        </div>
        <!-- /container -->
    </div> <!-- /main -->

    
</asp:Content>
