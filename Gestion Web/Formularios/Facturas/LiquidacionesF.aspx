<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LiquidacionesF.aspx.cs" Inherits="Gestion_Web.Formularios.Facturas.LiquidacionesF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">
        <div class="widget stacked">
            <div class="stat">
                <h5><i class="icon-map-marker"></i>&nbsp Ventas > Ventas > Liquidaciones BQS</h5>
            </div>
            <div class="widget-header">
                <i class="icon-wrench"></i>
                <h3>Herramientas</h3>
            </div>
            <div class="widget-content">
                <div class="pull-right">
                    <a class="btn btn-primary" data-toggle="modal" href="#modalBusqeda">
                        <i class="shortcut-icon icon-filter"></i>
                    </a>
                    &nbsp
                    <a href="LiquidacionesABM.aspx" class="btn btn-primary" data-toggle="tooltip" data-original-title="Agregar">
                        <i class="shortcut-icon icon-plus"></i>
                    </a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
