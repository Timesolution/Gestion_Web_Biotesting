<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ABMConceptos.aspx.cs" Inherits="Gestion_Web.Formularios.Valores.ABMConceptos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">

        <div class="col-md-12 col-xs-12">
            <div class="widget stacked">

                <div class="stat">                        
                    <h5><i class="icon-map-marker"></i> Maestro > Bancos > Conceptos</h5>
                </div>

                <div class="widget-header">
                    <i class="icon-wrench"></i>
                    <h3>Herramientas</h3>
                </div>
                <!-- /widget-header -->

                <div class="widget-content">
                    <div role="form" class="form-horizontal col-md-12">
                        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always">
                            <ContentTemplate>                            
                                <div class="form-group">
                                    <label for="name" class="col-md-2">Concepto:</label>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtConcepto" runat="server" class="form-control"></asp:TextBox>
                                    </div>
                                    <div class="col-md-1">
                                        <asp:RequiredFieldValidator ControlToValidate="txtConcepto" ID="RequiredFieldValidator3" runat="server" ErrorMessage="<h3>*</h3>" SetFocusOnError="true" ValidationGroup="ConceptoGroup"  ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                    </div>                            
                                </div>
                                <div class="form-group">
                                    <label for="name" class="col-md-2">Buscar:</label>
                                    <div class="col-md-3">
                                        <asp:TextBox ID="txtBusquedaCta" runat="server" class="form-control"/>
                                    </div>                            
                                    <div class="col-md-1">
                                        <asp:LinkButton ID="lbtnBuscarCta" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" OnClick="lbtnBuscarCta_Click" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label for="name" class="col-md-2">Plan de cuentas:</label>
                                    <div class="col-md-4">
                                        <asp:DropDownList ID="ListCuentasContables" runat="server"  class="form-control" />
                                    </div>
                                    <div class="col-md-1">
                                        <asp:RequiredFieldValidator ControlToValidate="ListCuentasContables" InitialValue="-1" ID="RequiredFieldValidator1" runat="server" ErrorMessage="<h3>*</h3>" ValidationGroup="ConceptoGroup"  SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="col-md-1">
                                        <asp:LinkButton ID="lbtnAgregar" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnAgregar_Click" ValidationGroup="ConceptoGroup" />
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
                <!-- /widget-content -->

            </div>
            <!-- /widget -->
        </div>
        <div class="col-md-12 col-xs-12">
            <div class="widget stacked widget-table action-table">

                <div class="widget-header">
                    <i class="icon-money"></i>
                    <h3> Conceptos

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
                                                <th style="width: 40%">Concepto</th>
                                                <th style="width: 40%">Plan cuentas</th>
                                                <th></th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:PlaceHolder ID="phConceptos" runat="server"></asp:PlaceHolder>
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

        
    </div>

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
                                        <asp:Label runat="server" ID="lblMensaje" Text="Esta seguro que desea eliminar?" Style="text-align: center"></asp:Label>
                                    </h5>
                                </div>

                                <div class="col-md-3">
                                    <asp:TextBox runat="server" ID="txtMovimiento" Text="0" ></asp:TextBox>
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
        <%--Fin modalGrupo--%>

    
    <script>
        function abrirdialog(valor) {
            document.getElementById('<%= txtMovimiento.ClientID %>').value = valor;
        }
    </script>

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
