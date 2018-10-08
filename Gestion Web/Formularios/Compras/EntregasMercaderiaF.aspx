<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EntregasMercaderiaF.aspx.cs" Inherits="Gestion_Web.Formularios.Compras.EntregasMercaderiaF" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">

        <div>
            <asp:UpdatePanel runat="server" ID="UpdatePanel1" UpdateMode="Always">
                <ContentTemplate>
                    <div class="col-md-12 col-xs-12">
                        <div class="widget stacked">

                            <div class="widget-header">
                                <i class="icon-wrench"></i>
                                <h3>Herramientas</h3>
                            </div>
                            <!-- /widget-header -->

                            <div class="widget-content">
                                <div id="validation-form" role="form" class="form-horizontal col-md-8">
                                    <fieldset>
                                        <div class="form-group">
                                            <label for="name" class="col-md-2">Sucursal</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="ListSucursal" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ListSucursal_SelectedIndexChanged"></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-2">Pto Venta</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="ListPtoVenta" runat="server" AutoPostBack="True" ></asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-2">Proveedor</label>
                                            <div class="col-md-4">
                                                <asp:DropDownList ID="ListProveedor" runat="server" AutoPostBack="True"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-4">
                                                <asp:UpdateProgress runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                                                    <ProgressTemplate>
                                                        <i class="fa fa-spinner fa-spin"></i><span>&nbsp;&nbsp;Cargando artículos del Proveedor. Por favor aguarde.</span>
                                                    </ProgressTemplate>
                                                </asp:UpdateProgress>
                                            </div>
                                        </div>

                                        <div class="form-group">
                                            <label for="name" class="col-md-2">Numero</label>

                                            <div class="col-md-2">
                                                <asp:TextBox ID="txtPVenta" MaxLength="4" runat="server" class="form-control" onchange="completar4Ceros(this, this.value)"></asp:TextBox>
                                            </div>

                                            <div class="col-md-3">
                                                <asp:TextBox ID="txtNumero" MaxLength="8" runat="server" class="form-control" onchange="completar8Ceros(this, this.value)"></asp:TextBox>
                                            </div>
                                            <div class="col-md-1">
                                                <%-- <asp:RequiredFieldValidator ControlToValidate="txtPVenta" ID="RequiredFieldValidator30" runat="server" ErrorMessage="*" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                            </div>
                                            <div class="col-md-1">
                                                <%--<asp:RequiredFieldValidator ControlToValidate="txtNumero" ID="RequiredFieldValidator6" runat="server" ErrorMessage="*" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-2">Fecha</label>

                                            <div class="col-md-3">
                                                <div class="input-group">
                                                    <span class="input-group-addon"><i class="icon-calendar"></i></span>
                                                    <asp:TextBox ID="txtFecha" runat="server" class="form-control"></asp:TextBox>
                                                </div>

                                            </div>
                                            <div class="col-md-2">
                                                <asp:RequiredFieldValidator ControlToValidate="txtFecha" ID="RequiredFieldValidator42" runat="server" ErrorMessage="*" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>

                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-2">Fecha Entrega</label>

                                            <div class="col-md-3">
                                                <div class="input-group">
                                                    <span class="input-group-addon"><i class="icon-calendar"></i></span>
                                                    <asp:TextBox ID="txtFechaEntrega" runat="server" class="form-control"></asp:TextBox>
                                                </div>

                                            </div>
                                            <div class="col-md-2">
                                                <asp:RequiredFieldValidator ControlToValidate="txtFechaEntrega" ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ValidationGroup="ArticuloGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>

                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label for="name" class="col-md-2">Observaciones</label>
                                            <div class="col-md-6">
                                                <asp:TextBox ID="txtObservaciones" runat="server" class="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                            </div>
                                        </div>                                        
                                    </fieldset>
                                </div>
                                <table class="table table-striped table-bordered">
                                    <thead>
                                        <tr>
                                            <th>Codigo</th>
                                            <th>Descripcion</th>
                                            <th>Cantidad Pedida</th>
                                            <th>Cantidad Entregada</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:PlaceHolder ID="phProductos" runat="server"></asp:PlaceHolder>
                                    </tbody>
                                </table>
                                <br />
                                <div class="btn-toolbar">
                                    <div class="btn-group">
                                        <asp:Button ID="btnAgregar" type="button" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnAgregar_Click" />
                                    </div>
                                    <%--<div class="alert alert-info alert-dismissable col-md-2">
                                        <asp:Label ID="lblCartelTotal" runat="server" Text="asd"/>
                                    </div>--%>
                                </div>

                            </div>
                            <!-- /widget-content -->

                        </div>
                        <!-- /widget -->
                    </div>

                </ContentTemplate>

            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
