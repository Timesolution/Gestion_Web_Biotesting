<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReporteAF.aspx.cs" Inherits="Gestion_Web.Formularios.Articulos.ReporteAF" %>

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
                
                <rsweb:ReportViewer ID="ReportViewer1" Width="100%" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" SizeToReportContent="True">

                </rsweb:ReportViewer>

            </div>
            <!-- /widget-content -->

        </div>

    </div>
</asp:Content>
