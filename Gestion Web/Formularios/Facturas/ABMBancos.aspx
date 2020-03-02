<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ABMBancos.aspx.cs" Inherits="Gestion_Web.Formularios.Facturas.ABMBancos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">

        <div class="col-md-12 col-xs-12">
            <div class="widget stacked">

                <div class="stat">                        
                    <h5><i class="icon-map-marker"></i> Maestro > Bancos > Entidades</h5>
                </div>

                <div class="widget-header">
                    <i class="icon-wrench"></i>
                    <h3>Herramientas</h3>
                </div>
                <!-- /widget-header -->

                <div class="widget-content">
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 45%">
                                <label for="name" class="col-md-4">Codigo</label>

                                <div class="col-md-6">
                                    <asp:TextBox ID="txtCodigo" runat="server" class="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:RequiredFieldValidator ControlToValidate="txtCodigo" ID="RequiredFieldValidator3" runat="server" ErrorMessage="<h3>*</h3>" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                            </td>
                            <td style="width: 45%">
                                <label for="name" class="col-md-4">Entidad</label>
                                <div class="col-md-6">
                                    <asp:TextBox ID="txtEntidad" runat="server" class="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <asp:RequiredFieldValidator ControlToValidate="txtEntidad" ID="RequiredFieldValidator1" runat="server" ErrorMessage="<h3>*</h3>" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                </div>
                            </td>
                            <td style="width: 10%">
                                <div class="shortcuts">
                                    <%--                                                <a onclick="btnAgregar_Click" class="btn btn-default ui-tooltip" data-toggle="tooltip" title data-original-title="Agregar" style="width: 100%">
                                                    <i class="shortcut-icon icon-plus"></i>
                                                    <span class="shortcut-label">Users</span>
                                                </a>--%>
                                    <div class="col-md-1">
                                        <asp:LinkButton ID="lbtnAgregar" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnAgregar_Click" />
                                    </div>
                                </div>
                            </td>
                            <%-- <td style="width: 5%">
                                            <div class="shortcuts" style="height: 100%">
                                               <a onclick="" class="btn btn-default ui-tooltip" data-toggle="tooltip" title data-original-title="Agregar" style="width: 100%">
                                                    <i class="shortcut-icon icon-pencil"></i>
                                                    <%--<span class="shortcut-label">Users</span>
                                                </a>
                                                <asp:LinkButton ID="lbtn" runat="server" Text="<span class='shortcut-icon icon-pencil'></span>" class="btn btn-default" style="width: 100%"/>

                                            </div>
                                            <%--</div>
                                        </td>--%>
                            <%--<td style="width: 5%">
                                            <div class="shortcuts" style="height: 100%">



                                                <a onclick="" class="btn btn-default ui-tooltip" data-toggle="tooltip" title data-original-title="Agregar" style="width: 100%">
                                                    <i class="shortcut-icon icon-trash"></i>
                                                    <%--<span class="shortcut-label">Users</span>
                                                </a>
                                            </div>
                                            <%--</div>
                                        </td>--%>
                            <%--<td style="width: 5%">
                                            <div class="shortcuts" style="height: 100%">



                                                <a onclick="" class="btn btn-default ui-tooltip" data-toggle="tooltip" title data-original-title="Agregar" style="width: 100%">
                                                    <i class="shortcut-icon icon-print"></i>
                                                    <%--<span class="shortcut-label">Users</span>--%>
                            <%--     </a>
                                            </div>--%>
                            <%--</div>
                                        </td>--%>
                        </tr>

                    </table>
                </div>
                <!-- /widget-content -->

            </div>
            <!-- /widget -->
        </div>
        <div class="col-md-12 col-xs-12">
            <div class="widget stacked widget-table action-table">

                <div class="widget-header">
                    <i class="icon-money"></i>
                    <h3>Bancos

                    </h3>
                </div>
                <div class="widget-content">
                    <div class="panel-body">

                        <%--<div class="col-md-12 col-xs-12">--%>
                        <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                            <ContentTemplate>
                                <div class="table-responsive">
                                    <a class="btn btn-info" style="display: none" data-toggle="modal" id="abreDialog" href="#modalFacturaDetalle">Agregar Tipo Cliente</a>
                                    <table class="table table-striped table-bordered table-hover" id="dataTables-example">
                                        <thead>
                                            <tr>
                                                <th style="width: 10%">Codigo</th>
                                                <th style="width: 90%">Entidad</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:PlaceHolder ID="phBancos" runat="server"></asp:PlaceHolder>
                                        </tbody>
                                    </table>

                                </div>


                            </ContentTemplate>
                            <Triggers>
                            </Triggers>
                        </asp:UpdatePanel>
                    </div>


                    <!-- /.content -->

                </div>

            </div>
        </div>

        <%--<div class="row">
            <div class="col-md-12">

                <div class="widget stacked ">

                    <div class="widget-header">
                        <i class="icon-pencil"></i>
                        <h3>Bancos</h3>

                    </div>
                    <div class="widget-content">

                        <div class="col-md-6">
                            <table id="tbClientes" class="table table-striped table-bordered">

                            </table>

                        </div>

                        <div role="form" class="form-horizontal col-md-6">
                              <div class="form-group">

                            </div>
                            <div class="form-group">

                            </div>                                                       
                            <div class="col-md-6" style="text-align: center">
                            <asp:Button ID="btnAgregar" runat="server" Text="Guardar" class="btn btn-success" />
                        </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>--%>
    </div>

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


</asp:Content>
