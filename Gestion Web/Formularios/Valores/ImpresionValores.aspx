<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ImpresionValores.aspx.cs" Inherits="Gestion_Web.Formularios.Valores.ImpresionValores" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server"> 
    
    <div class="col-md-12">
        <div class="widget stacked">

            <div class="widget-header">
                <i class="icon-wrench"></i>
                <h3>Reportes</h3>
            </div>
            <!-- /widget-header -->

            <div class="widget-content">
                 <!--<asp:Button ID="printReport" runat="server" Text="PRINT" Width="75px" UseSubmitBehavior="false"></asp:Button> -->                               
                 
                 <rsweb:ReportViewer ID="ReportViewer1"  runat="server" ClientIDMode="Static" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="80%" SizeToReportContent="true" ShowPrintButton="true">
                     
                 </rsweb:ReportViewer>
                <input id="printreport" type="button" value="Print this Report" />
            </div>
            <!-- /widget-content -->

        </div>

    </div>

     <script type="text/javascript">
         //------------------------------------------------------------------
         // Cross-browser Multi-page Printing with ASP.NET ReportViewer
         // by Chtiwi Malek.
         // http://www.codicode.com
         //------------------------------------------------------------------

         // Linking the print function to the print button
         $('#printreport').click(function () {
             printReport('ReportViewer1');
         });

         // Print function (require the reportviewer client ID)
         function printReport(report_ID) {
             var rv1 = $('#' + report_ID);
             var iDoc = rv1.parents('html');

             // Reading the report styles
             var styles = iDoc.find("head style[id$='ReportControl_styles']").html();
             if ((styles == undefined) || (styles == '')) {
                 iDoc.find('head script').each(function () {
                     var cnt = $(this).html();
                     var p1 = cnt.indexOf('ReportStyles":"');
                     if (p1 > 0) {
                         p1 += 15;
                         var p2 = cnt.indexOf('"', p1);
                         styles = cnt.substr(p1, p2 - p1);
                     }
                 });
             }
             if (styles == '') { alert("Cannot generate styles, Displaying without styles.."); }
             styles = '<style type="text/css">' + styles + "</style>";

             // Reading the report html
             var table = rv1.find("div[id$='_oReportDiv']");
             if (table == undefined) {
                 alert("Report source not found.");
                 return;
             }

             // Generating a copy of the report in a new window
             var docType = '<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01//EN" "http://www.w3.org/TR/html4/loose.dtd">';
             var docCnt = styles + table.parent().html();
             var docHead = '<head><title>Printing ...</title><style>body{margin:5;padding:0;}</style></head>';
             var winAttr = "location=yes,statusbar=no,directories=no,menubar=no,titlebar=no,toolbar=no,dependent=no,width=720,height=600,resizable=yes,screenX=200,screenY=200,personalbar=no,scrollbars=yes";;
             var newWin = window.open("", "_blank", winAttr);
             writeDoc = newWin.document;
             writeDoc.open();
             writeDoc.write(docType + '<html>' + docHead + '<body onload="window.print();">' + docCnt + '</body></html>');
             writeDoc.close();

             // The print event will fire as soon as the window loads
             newWin.focus();
             // uncomment to autoclose the preview window when printing is confirmed or canceled.
             // newWin.close();
         };

        </script>

     <%--Fin modalGrupo--

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

        <%--<script src="//code.jquery.com/jquery-1.9.1.js"></script>--
        <script src="//code.jquery.com/ui/1.10.4/jquery-ui.js"></script>--%>
    

</asp:Content>
