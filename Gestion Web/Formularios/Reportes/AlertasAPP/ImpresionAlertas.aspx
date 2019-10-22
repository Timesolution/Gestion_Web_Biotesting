<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ImpresionAlertas.aspx.cs" Inherits="Gestion_Web.Formularios.Reportes.AlertasAPP.ImpresionAlertas" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="widget-content">
        <div class="panel-body">
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="80%">
                <LocalReport ReportEmbeddedResource="Gestion_Web.Formularios.Reportes.AlertasAPP.ImpresionAlertas.rdlc">
                </LocalReport>
            </rsweb:ReportViewer>
        </div>
    </div>
</asp:Content>
