<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MateriasPrimasABM.aspx.cs" Inherits="Gestion_Web.Formularios.MateriasPrimas.MateriasPrimasABM" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">

            <div class="col-md-12">

                <div class="widget stacked">
                    <div class="stat">
                        <h5><i class="icon-map-marker"></i> Maestro > MateriasPrimas > MateriasPrimas</h5>
                        <asp:Label ID="lblFiltroAnterior" runat="server" Style="display: none;"></asp:Label>
                    </div>
                    <div class="widget-header">
                        <i class="icon-pencil"></i>
                        <h3>Materias Primas</h3>

                    </div>
                    <!-- /.widget-header -->
                    <div class="widget-content">
                        <div class="bs-example">
                            <ul id="myTab" class="nav nav-tabs">
                                <li class="active"><a href="#home" data-toggle="tab">Detalle</a></li>

                            </ul>
                            <div class="tab-content">
                                <div class="tab-pane fade active in" id="home">
                                    <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Always" runat="server">
                                        <ContentTemplate>
                                            <div id="validation-form" role="form" class="form-horizontal col-md-10">
                                                <fieldset>

                                                    <div class="form-group">
                                                        <label for="name" class="col-md-4">Codigo Materia Prima</label>
                                                        <div class="col-md-4">
                                                            <asp:TextBox ID="txtCodMateriaPrima" runat="server" class="form-control"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtCodMateriaPrima" ValidationGroup="MateriasPrimasGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label for="name" class="col-md-4">Descripción</label>
                                                        <div class="col-md-4">
                                                            <asp:TextBox ID="txtDescripcion" runat="server" class="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label for="validateSelect" class="col-md-4">Moneda</label>
                                                        <div class="col-md-4">
                                                            <asp:DropDownList ID="ddlMonedaVenta" AutoPostBack="true" runat="server" class="form-control" OnSelectedIndexChanged="ddlMonedaVenta_SelectedIndexChanged"></asp:DropDownList>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ErrorMessage="Seleccione una Moneda" ControlToValidate="ddlMonedaVenta" InitialValue="-1" ValidationGroup="MateriasPrimasGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label for="name" class="col-md-4">Stock Minimo</label>
                                                        <div class="col-md-4">
                                                            <asp:TextBox ID="txtStockMinimo" Text="0" runat="server" class="form-control" Style="text-align: right;" TextMode="Number" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtStockMinimo" ValidationGroup="MateriasPrimasGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label for="validateSelect" class="col-md-4">Unidad de Medida</label>
                                                        <div class="col-md-4">
                                                            <asp:DropDownList ID="ddlUnidadDeMedida" AutoPostBack="true" runat="server" class="form-control" OnSelectedIndexChanged="ddlUnidadDeMedida_SelectedIndexChanged">
                                                                <asp:ListItem Value="Centimetros cubicos">Centimetros cubicos</asp:ListItem>
                                                                <asp:ListItem Value="Gramos">Gramos</asp:ListItem>
                                                                <asp:ListItem Value="Kilos">Kilos</asp:ListItem>
                                                                <asp:ListItem Value="Litros">Litros</asp:ListItem>
                                                                <asp:ListItem Value="Metros">Metros</asp:ListItem>
                                                                <asp:ListItem Value="Mililitros">Mililitros</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Seleccione una Moneda" ControlToValidate="ddlUnidadDeMedida" InitialValue="-1" ValidationGroup="MateriasPrimasGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label for="name" class="col-md-4">Importe</label>
                                                        <div class="col-md-4">
                                                            <asp:TextBox ID="txtImporte" Text="0" runat="server" class="form-control" Style="text-align: right;" TextMode="Number" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                                        </div>
                                                        <div class="col-md-4">
                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="El campo es obligatorio" ControlToValidate="txtImporte" ValidationGroup="MateriasPrimasGroup" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                        </div>
                                                    </div>

                                                    <div class="form-group">
                                                        <label for="validateSelect" class="col-md-4">Activo</label>
                                                        <div class="col-md-4">
                                                            <asp:DropDownList ID="ddlEstado" runat="server" class="form-control">
                                                                <asp:ListItem Value="1">Si</asp:ListItem>
                                                                <asp:ListItem Value="0">No</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </div>
                                                    </div>

                                                </fieldset>

                                            </div>

                                        </ContentTemplate>

                                    </asp:UpdatePanel>
                                    <%--BOTONES DE ABAJO--%>
                                    <div class="col-md-8">
                                        <asp:Button ID="btnGuardar" runat="server" Text="Guardar" Visible="true" class="btn btn-success" OnClick="btnGuardar_Click" ValidationGroup="MateriasPrimasGroup" />
                                        <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" class="btn btn-default" PostBackUrl="../MateriasPrimas/MateriasPrimasF.aspx" />
                                    </div>
                                    <%--FIN BOTONES DE ABAJO--%>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
