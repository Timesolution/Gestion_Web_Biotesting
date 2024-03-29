﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ABMSubListas.aspx.cs" Inherits="Gestion_Web.Formularios.Facturas.ABMSubListas" %>
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
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 60%">
                                            <div class="form-group">
                                                <label class="col-md-2">Categoria</label>
                                                <div class="col-md-6">
                                                    <%--<div class="input-group">--%>
                                                        <asp:TextBox ID="txtCategoria" runat="server" class="form-control"></asp:TextBox>
                                                    <%--</div>--%>
                                                    <!-- /input-group -->
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtCategoria" SetFocusOnError="true" Font-Bold="true" ForeColor="Red" ValidationGroup="PaisGroup"></asp:RequiredFieldValidator>
                                                </div>
                                            </div>
                                        </td>
                                        <td style="width: 30%"></td>
                                        <td style="width: 10%">
                                            <div class="col-md-1">
                                                <div class="shortcuts">
                                                    <%--                                                <a onclick="btnAgregar_Click" class="btn btn-default ui-tooltip" data-toggle="tooltip" title data-original-title="Agregar" style="width: 100%">
                                                    <i class="shortcut-icon icon-plus"></i>
                                                    <span class="shortcut-label">Users</span>
                                                </a>--%>
                                                    <asp:LinkButton ID="lbtnAgregar" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnAgregar_Click" ValidationGroup="PaisGroup" />

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
                                <i class="icon-th-list"></i>
                                <h3>Categorias Lista Precios

                                </h3>
                            </div>
                            <div class="widget-content">
                               <div class="panel-body">

                                <%--<div class="col-md-12 col-xs-12">--%>
                                   <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                                       <ContentTemplate>
                                           <div class="table-responsive">
                                               <table class="table table-striped table-bordered table-hover" id="dataTables-example">
                                                   <thead>
                                                       <tr>
                                                           <th>Categorias</th>
                                                           <th></th>
                                                       </tr>

                                                   </thead>
                                                   <tbody>
                                                       <asp:PlaceHolder ID="phSubListas" runat="server"></asp:PlaceHolder>
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
                                        <asp:Label runat="server" ID="lblMensaje" Text="Esta seguro que desea eliminar la Categoria?" Style="text-align: center"></asp:Label>
                                    </h5>
                                </div>

                                <div class="col-md-3">
                                    <asp:TextBox runat="server" ID="txtMovimiento" Text="0" Style="display: none"></asp:TextBox>
                                </div>
                            </div>
                            <%--                                <div class="form-group">
                                    
                                </div>--%>
                        </div>


                        <div class="modal-footer">
                            <asp:Button runat="server" ID="btnSi" Text="Eliminar" class="btn btn-danger" OnClick="btnSi_Click"/>
                            <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Cancelar</button>

                            <%--                        <asp:LinkButton ID="LinkButton1" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="btnBuscar_Click" ValidationGroup="BusquedaGroup" />--%>
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

    <!-- Page-Level Plugin Scripts - Tables -->
    <script src="//cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>
    <script src="../../Scripts/plugins/dataTables/custom.tables.js"></script>
    <link href="//cdn.datatables.net/1.10.2/css/jquery.dataTables.css" rel="stylesheet"/>


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
                    else
                    { return false; }
                }
                return true;
            }
        </script>

</div>
</asp:Content>
