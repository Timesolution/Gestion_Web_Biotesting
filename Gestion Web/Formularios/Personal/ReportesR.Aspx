<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportesR.aspx.cs" Inherits="Gestion_Web.Formularios.Personal.ReportesR" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%--<%@ Register assembly="printButtonDLL" namespace="printButtonDLL" tagprefix="cc1" %>--%>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
      <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.5.0/jquery.min.js" language="javascript" type="text/javascript"></script>
<script type="text/javascript">
    function $_create(elem, tag, target) { return addElem(elem, target, tag) }
    function $_add(elem, target) { return addElem(elem, target) }
    function $_GB() { return GetBrowser(); }

    function GetBrowser() {
        //JQuery Script;
        if ($.browser.mozilla)
            return 'FF';
        else if ($.browser.msie)
            return 'IE';
        else if ($.browser.webkit)
            return 'OP';
        else if ($.browser.opera)
            return 'WK';
        else
            return 'FF';
    }

    function addElem(elem, target, tag) {
        if (typeof elem === 'string') {
            var el = document.getElementById(elem);
            if (!el) {

                el = document.createElement(tag);

                el.id = elem;
            }
            elem = el;
        }
        if (target) {
            var dest;
            if (typeof target === 'string')
                dest = document.getElementById(target);
            else
                dest = target;

            dest.appendChild(elem);
        }

        return elem;
    }

    function insert(elem, target) {
        if (typeof target === 'string')
            target = document.getElementById(target);
        var myDoc = target.contentWindow || target.contentDocument;
        if (myDoc.document) {
            myDoc = myDoc.document;
        }
        var headLoc = myDoc.getElementsByTagName("head").item(0);
        var scriptObj = myDoc.createElement("script");
        scriptObj.setAttribute("type", "text/javascript");
        scriptObj.innerHTML = 'window.print();';
        if (elem)
            elem = document.getElementById(elem);

        if (elem)
            headLoc.appendChild(elem);
        else
            headLoc.appendChild(scriptObj);

    }
  </script>
    <div class="col-md-12">
        <div class="widget stacked">

            <div class="widget-header">
                <i class="icon-wrench"></i>
                <h3>Reportes</h3>
            </div>
            <!-- /widget-header -->

            <div class="widget-content">
                 <%--<cc1:PrintButton ID="PrintButton1" runat="server" ReportName="ReportViewer1" />--%>
                 
                 <rsweb:ReportViewer ID="ReportViewer1" Width="100%" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
                     
                 </rsweb:ReportViewer>

            </div>
            <!-- /widget-content -->

        </div>

    </div>
</asp:Content>
