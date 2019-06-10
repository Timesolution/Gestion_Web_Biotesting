<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ResetearStock.aspx.cs" Inherits="Gestion_Web.Formularios.Herramientas.ResetearStock" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-md-12">
                <div class="widget stacked">
                    <div class="stat">
                        <h5><i class="icon-map-marker"></i>Maestros > Herramientas > Resetear Stock</h5>
                    </div>
                    <div class="widget-header">
                        <i class="icon-list-alt"></i>
                        <h3>Resetear Stock</h3>
                    </div>
                    <div class="widget-content">
                        <div class="form-horizontal col-md-12">
                            <fieldset>
                                <asp:UpdatePanel runat="server" ID="upArticulosDiferenciasStock">
                                    <ContentTemplate>
                                        <div class="form-group">
                                            <label class="col-md-2">Empresa</label>
                                            <div class="col-md-3">
                                                <asp:DropDownList ID="DropListEmpresa" runat="server" class="form-control" AutoPostBack="true"/>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:RequiredFieldValidator ErrorMessage="* Seleccione Empresa" InitialValue="0" ControlToValidate="ListEmpresa" runat="server" ForeColor="Red" Font-Bold="true" ValidationGroup="ArticulosDiferenciasStockGroup" />
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <label class="col-md-2">Sucursal</label>
                                            <div class="col-md-3">
                                                <asp:DropDownList ID="DropListSucursal" runat="server" class="form-control"></asp:DropDownList>
                                            </div>
                                            <div class="col-md-2">
                                                <asp:RequiredFieldValidator ErrorMessage="* Seleccione Sucursal" InitialValue="0" ControlToValidate="ListSucursal" runat="server" ForeColor="Red" Font-Bold="true" ValidationGroup="ArticulosDiferenciasStockGroup" />
                                            </div>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <div class="form-group">
                                    <div class="col-md-4">
                                        <asp:Button runat="server" ID="btnGenerarDiferenciasStock" Text="Generar Diferencias" class="btn btn-success" OnClick="btnGenerarDiferenciasStock_Click" ValidationGroup="ArticulosDiferenciasStockGroup" />
                                    </div>
                                </div>
                            </fieldset>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script>
        function pageLoad() {
            var controlDropListEmpresa = document.getElementById('<%= DropListEmpresa.ClientID %>');
            controlDropListEmpresa.addEventListener("change", CargarSucursales);
        }

        function CargarSucursales() {
            var controlDropListEmpresa = document.getElementById('<%= DropListEmpresa.ClientID %>').value;

            $.ajax({
                type: "POST",
                url: "ArticulosDiferenciasStock.aspx/ObtenerSucursalesDependiendoDeLaEmpresa",
                data: '{empresa: "' + controlDropListEmpresa + '"  }',
                contentType: "application/json",
                dataType: 'json',
                error: function () {
                    alert("No se pudieron cargar las sucursales.");
                },
                success: OnSuccessSucursal
            });
        };

        function OnSuccessSucursal(response) {
            var controlDropListSucursal = document.getElementById('<%= DropListSucursal.ClientID %>');

            while (controlDropListSucursal.options.length > 0) {
                controlDropListSucursal.remove(0);
            }

            var data = response.d;
            obj = JSON.parse(data);

            for (i = 0; i < obj.length; i++) {
                option = document.createElement('option');
                option.value = obj[i].id;
                option.text = obj[i].nombre;

                controlDropListSucursal.add(option);
            }
        }
    </script>
</asp:Content>
