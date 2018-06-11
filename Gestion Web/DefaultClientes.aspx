<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DefaultClientes.aspx.cs" Inherits="Gestion_Web.DefaultClientes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">

        <div class="container">

            <div class="row">

                <div class="col-md-6">


                    <div class="widget stacked">

                        <div class="widget-header">
                            <i class="icon-bookmark"></i>
                            <h3>Mis Accesos</h3>
                        </div>
                        <!-- /widget-header -->

                        <div class="widget-content">

                            <div class="shortcuts">
                               

                                <a href="Formularios/Facturas/ABMPedidos.aspx" class="shortcut">
                                    <i class="shortcut-icon icon-shopping-cart"></i>
                                    <span class="shortcut-label">Hacer Pedido</span>
                                </a>

                                <a href="Formularios/Facturas/ABMPedidos.aspx" class="shortcut">
                                    <i class="shortcut-icon icon- shopping-cart"></i>
                                    <span class="shortcut-label">Hacer Pedido</span>
                                </a>

                                <a href="Formularios/Facturas/CuentaCorrienteF.aspx" class="shortcut">
                                    <i class="shortcut-icon icon-th-list"></i>
                                    <span class="shortcut-label">Cuenta Corriente</span>
                                </a>

                                <a href="javascript:;" class="shortcut">
                                    <i class="shortcut-icon  icon-adjust"></i>
                                    <span class="shortcut-label">Balance</span>
                                </a>
                            </div>
                            <!-- /shortcuts -->

                        </div>
                        <!-- /widget-content -->

                    </div>
                    <!-- /widget -->



                    <div class="widget stacked widget-table action-table">

                        <div class="widget-header">
                            <i class="icon-th-list"></i>
                            <h3>Facturas Emitidas por Sucursal</h3>
                        </div>
                        <!-- /widget-header -->

                        <div class="widget-content">

                            <table class="table table-striped table-bordered">
                                <thead>
                                    <tr>
                                        <th>Sucursal</th>
                                        <th class="td-actions"></th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:PlaceHolder ID="phSucursales" runat="server"></asp:PlaceHolder>
                                </tbody>
                            </table>

                        </div>
                        <!-- /widget-content -->

                    </div>
                    <!-- /widget -->


                </div>
                <!-- /span6 -->

            </div>
            <!-- /row -->

            <a class="btn btn-info" data-toggle="modal" id="abreDenegado" href="#modalDenegado" style="display: none">Agregar Tipo Cliente</a>

        </div>




        <div id="modalDenegado" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <%--<button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>--%>
                        <a href="/Default.aspx" class="close">x</a>
                        <h4 class="modal-title">Error!</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <div class="form-group">
                                <div class="col-md-4">
                                    <h1>
                                        <i class="icon-lock"></i>
                                    </h1>
                                </div>
                                <div class="col-md-8">
                                    <h4>
                                        <asp:Label runat="server" ID="Label1" Text="Acceso Denegado!!!" Style="text-align: center"></asp:Label>
                                    </h4>
                                </div>

                                <%--<div class="col-md-3">
                                        <asp:TextBox runat="server" ID="TextBox1" Text="0" style="display: none"></asp:TextBox>
                                    </div>--%>
                            </div>
                            <%--                                <div class="form-group">
                                    
                                </div>--%>
                        </div>


                        <div class="modal-footer">
                            <button type="button" class="btn btn-info" data-dismiss="modal" aria-hidden="true">Continuar</button>
                        </div>
                    </div>

                </div>
            </div>
        </div>
        <%--Fin modalGrupo--%>

        <!-- /main -->

        <!-- Le javascript
================================================== -->
        <!-- Placed at the end of the document so the pages load faster -->

        <script>

            function abrirDenegado() {
                document.getElementById('abreDenegado').click();
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
        <script src="../../Scripts/plugins/flot/jquery.flot.js"></script>
        <script src="../../Scripts/plugins/flot/jquery.flot.pie.js"></script>
        <script src="../../Scripts/plugins/flot/jquery.flot.orderBars.js"></script>
        <script src="../../Scripts/plugins/flot/jquery.flot.resize.js"></script>
        <script src="../../Scripts/demo/notifications.js"></script>

        <script src="../../Scripts/charts/area.js"></script>
        <script src="../../Scripts/charts/donut.js"></script>

        <script>
            $(function () {

                var data = [];
                var series = 3;
                //for( var i = 0; i<series; i++)
                //{
                //var data1 = parseInt(document.getElementById('val1').innerText);
                //var data2 = parseInt(document.getElementById('val2').innerText);
                //var data3 = parseInt(document.getElementById('val3').innerText);


                data[0] = { label: "Si", data: 10 }
                data[1] = { label: "No", data: 15 }
                data[2] = { label: "En Espera", data: 5 }
                //}

                $.plot($("#donut-chart2"), data,
                {
                    colors: ["#F90", "#222", "#777", "#AAA"],
                    series: {
                        pie: {
                            innerRadius: 0.5,
                            show: true
                        }
                    }
                });

            });
        </script>

        <%--        <script src="../../Scripts/plugins/msgGrowl/js/msgGrowl.js"></script>
        <script src="../../Scripts/plugins/lightbox/jquery.lightbox.min.js"></script>
        <script src="../../Scripts/plugins/msgbox/jquery.msgbox.min.js"></script>--%>
        <%--        <script src="../../Scripts/plugins/flot/jquery.flot.js"></script>
        <script src="../../Scripts/plugins/flot/jquery.flot.pie.js"></script>
     <script src="../../Scripts/plugins/flot/jquery.flot.orderBars.js"></script>
        <script src="../../Scripts/plugins/flot/jquery.flot.resize.js"></script>

        <script src="../../Scripts/Application.js"></script>

        <script src="../../Scripts/demo/notifications.js"></script>

        <script src="../../Scripts/charts/area.js"></script>
        <script src="../../Scripts/charts/donut.js"></script>--%>
    </div>
</asp:Content>
