<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReporteListaDePreciosF.aspx.cs" Inherits="Gestion_Web.Formularios.Reportes.ReporteListasDePreciosF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">

        <div class="row">

            <div class="col-md-12">

                <div class="widget stacked">

                    <div class="stat">
                        <h5><i class="icon-map-marker"></i> Reportes > Articulos > Listas de precios</h5>
                    </div>
                    <div class="widget-header">
                        <i class="icon-pencil"></i>
                        <h3>Generar</h3>
                    </div>

                    <div class="widget-content">
                        <div class="form-horizontal col-md-12">
                            <fieldset>
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <ContentTemplate>

                                        <div class="form-group">
                                            <label class="col-md-4">Lista de Precios</label>
                                            <div class="col-md-6">
                                                <asp:DropDownList ID="ddlListaDePrecios" ValidationGroup="BusquedaLista" runat="server" class="form-control"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="ddlListaDePrecios" InitialValue="-1" ValidationGroup="BusquedaLista" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4">
                                                Precio Sin Iva
                                            </label>
                                            <div class="col-md-1">
                                                <asp:RadioButton ID="RadioSinIva" Checked="true" runat="server" GroupName="iva" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4">
                                                Precio Con Iva
                                   
                                            </label>
                                            <div class="col-md-1">
                                                <asp:RadioButton ID="RadioConIva" runat="server" GroupName="iva" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4">
                                                Agrupar por ubicacion
                                   
                                            </label>
                                            <div class="col-md-1">
                                                <asp:CheckBox ID="chkUbicacion" runat="server" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-4">
                                                Incluir Descuento por Cantidad
                                   
                                            </label>
                                            <div class="col-md-1">
                                                <asp:CheckBox ID="chkDescuentoCantidad" runat="server" visible="false"/>
                                            </div>
                                        </div>

                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <div class="form-group">
                                    <div class="col-md-4">
                                        <asp:Button ID="lbtnSolicitarInforme" Text="Solicitar Informe" runat="server" class="btn btn-success" OnClick="lbtnSolicitarInforme_Click" ValidationGroup="StockGroup" />
                                    </div>
                                </div>
                            </fieldset>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>


    <script src="../../Scripts/jquery-1.10.2.js"></script>
    <script src="../../Scripts/bootstrap.min.js"></script>
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
