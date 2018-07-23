<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StoresF.aspx.cs" Inherits="Gestion_Web.Formularios.Stores.StoresF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-md-12 col-xs-12">
        <div class="widget stacked">
            <div class="widget-header">
                <i class="icon-th-large"></i>
                <h3>Galeria de Imagenes</h3>
            </div>
            <!-- /widget-header -->

            <div class="widget-content">
                <ul class="gallery-container">
                    <table class="table table-bordered table-striped" id="tbImagenes">
                        <thead>
                        </thead>
                        <tbody>
                            <asp:PlaceHolder ID="phImagenesArticulos" runat="server"></asp:PlaceHolder>
                        </tbody>
                    </table>
                </ul>
            </div>
            <!-- /widget-content -->
        </div>
    </div>
</asp:Content>
