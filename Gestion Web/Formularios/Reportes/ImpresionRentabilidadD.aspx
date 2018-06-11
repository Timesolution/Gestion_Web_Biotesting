<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ImpresionRentabilidadD.aspx.cs" Inherits="Gestion_Web.Formularios.Reportes.ImpresionRentabilidadD" %>


<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>



<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">

        <div class="container">

           
            <div class="col-md-12 col-xs-12 hidden-print">
                <div class="widget stacked">

                    <div class="widget-header">
                        <i class="icon-wrench"></i>
                        <h3>Herramientas</h3>
                    </div>
                    <!-- /widget-header -->

                    <div class="widget-content">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 95%"></td>
                                        <td style="width: 5%">
                                            <div class="shortcuts" style="height: 100%">



                                                <a href="ABMCobros.aspx" class="btn btn-primary ui-tooltip" data-toggle="tooltip" title data-original-title="Agregar" style="width: 100%">
                                                    <i class="shortcut-icon icon-arrow-left"></i>
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
            <div class="col-md-12 col-xs-12">
                        <div class="widget stacked widget-table action-table">

                            <div class="widget-header">
                                <i class="icon-th-list"></i>
                                <h3>
                                    Formulario Ventas
                                </h3>
                            </div>
                            <div class="widget-content">
                               <div class="panel-body">
                                               
                                   <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="80%">
                                       <LocalReport ReportEmbeddedResource="Gestion_Web.Formularios.Cobros.Cobros.rdlc">
                                       </LocalReport>
                                   </rsweb:ReportViewer>
                                   <%--<CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="True" ReportSourceID="CrystalReportSource1" ToolPanelWidth="300px" ToolPanelHeight="300px" ToolPanelView="None" Width="903px" PageZoomFactor="163" />
                                   <CR:CrystalReportSource ID="CrystalReportSource1" runat="server">
                                       <Report FileName="Pedidos.rpt">
                                       </Report>
                                   </CR:CrystalReportSource>
                                </div>--%>


                                <!-- /.content -->

                            </div>

                        </div>
                    </div>


            

        
    </div>
    </div>  <%--Fin modalGrupo--%>

            <!-- Core Scripts - Include with every page -->
    <script src="../../Scripts/jquery-1.10.2.js"></script>
    <script src="../../Scripts/bootstrap.min.js"></script>
    <%--<script src="../Scripts/plugins/metisMenu/jquery.metisMenu.js"></script>--%>

    <script src="../../Scripts/libs/jquery-1.9.1.min.js"></script>
    <script src="../../Scripts/libs/jquery-ui-1.10.0.custom.min.js"></script>
    <%--<script src="../../Scripts/libs/bootstrap.min.js"></script>--%>
        
    <script src="../../Scripts/plugins/hoverIntent/jquery.hoverIntent.minified.js"></script>
    
    <script src="../../Scripts/Application.js"></script>

    <script src="../../Scripts/demo/gallery.js"></script>
    
    <script src="../../Scripts/plugins/msgGrowl/js/msgGrowl.js"></script>
    <script src="../../Scripts/plugins/lightbox/jquery.lightbox.min.js"></script>
    <script src="../../Scripts/plugins/msgbox/jquery.msgbox.min.js"></script>
    <script src="../../Scripts/demo/notifications.js"></script>

    <%--<script src="//code.jquery.com/jquery-1.9.1.js"></script>--%>
    <script src="//code.jquery.com/ui/1.10.4/jquery-ui.js"></script>



       <%-- </div>--%>

    </div>

</asp:Content>
