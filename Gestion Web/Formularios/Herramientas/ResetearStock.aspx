<%@ Page Title="" EnableEventValidation = "false" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ResetearStock.aspx.cs" Inherits="Gestion_Web.Formularios.Herramientas.ResetearStock" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-md-12">
                <div class="widget stacked">
                    <div class="stat">
                        <h5><i class="icon-map-marker"></i>Maestros > Herramientas > Resetear Stock</h5>
                    </div>
                    <div class="widget-header">
                        <i class="icon-refresh"></i>
                        <i class="icon-list-alt"></i>
                        <h3>Resetear Stock</h3>
                    </div>
                    <div class="widget-content">
                        <div class="form-horizontal col-md-12">
                            <div class="form-group">
                                <label class="col-md-2">Empresa</label>
                                <div class="col-md-3">
                                    <asp:DropDownList ID="DropListEmpresa" runat="server" class="form-control" />
                                    <asp:Label runat="server" ID="lbDropListEmpresaError" Style="display: none" Text="Seleccione un item." ForeColor="Red" Font-Bold="true"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-2">Sucursal</label>
                                <div class="col-md-3">
                                    <asp:DropDownList ID="DropListSucursal" runat="server" class="form-control" />
                                    <asp:Label runat="server" ID="lbDropListSucursalError" Style="display: none" Text="Seleccione un item." ForeColor="Red" Font-Bold="true"></asp:Label>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-2">Proveedor</label>
                                <div class="col-md-3">
                                    <asp:DropDownList ID="DropListProveedor" runat="server" class="form-control" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-2">Grupo</label>
                                <div class="col-md-3">
                                    <asp:DropDownList ID="DropListGrupo" runat="server" class="form-control" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-2">Sub Grupo</label>
                                <div class="col-md-3">
                                    <asp:DropDownList ID="DropListSubGrupo" runat="server" class="form-control" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-2">Marca</label>
                                <div class="col-md-3">
                                    <asp:DropDownList ID="DropListMarca" runat="server" class="form-control" />
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-4">
                                    <asp:Button runat="server" ID="ProcesarReinicioDeStock" OnClientClick="return ValidateForm();" Text="Generar Diferencias" class="btn btn-success" OnClick="ProcesarReinicioDeStock_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script>
        var controlDropListEmpresa;
        var controlDropListSucursal;
        var controlDropListGrupo;
        var controlDropListSubGrupo;
        var controlLabelEmpresaError;
        var controllabelSucursalError;

        function pageLoad() {
            controlDropListEmpresa = document.getElementById('<%= DropListEmpresa.ClientID %>');
            controlDropListEmpresa.addEventListener("change", CargarSucursales);

            controlDropListSucursal = document.getElementById('<%= DropListSucursal.ClientID %>');

            controlDropListGrupo = document.getElementById('<%= DropListGrupo.ClientID %>');
            controlDropListGrupo.addEventListener("change", CargarSubGrupos);

            controlDropListSubGrupo = document.getElementById('<%= DropListSubGrupo.ClientID %>');

            controlLabelEmpresaError = document.getElementById('<%= lbDropListEmpresaError.ClientID %>');
            controllabelSucursalError = document.getElementById('<%= lbDropListSucursalError.ClientID %>');
        }

        function CargarSucursales() {
            $.ajax({
                type: "POST",
                url: "ResetearStock.aspx/ObtenerSucursalesDependiendoDeLaEmpresa",
                data: '{empresa: "' + controlDropListEmpresa.value + '"  }',
                contentType: "application/json",
                dataType: 'json',
                error: function () {
                    alert("No se pudieron cargar las sucursales.");
                },
                success: OnSuccessSucursal
            });
        };

        function OnSuccessSucursal(response) {
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

        function CargarSubGrupos() {
            $.ajax({
                type: "POST",
                url: "ResetearStock.aspx/ObtenerSubGruposDependiendoDelGrupo",
                data: '{grupo: "' + controlDropListGrupo.value + '"  }',
                contentType: "application/json",
                dataType: 'json',
                error: function () {
                    alert("No se pudieron cargar los subGrupos.");
                },
                success: OnSuccessSubGrupos
            });
        };

        function OnSuccessSubGrupos(response) {
            while (controlDropListSubGrupo.options.length > 0) {
                controlDropListSubGrupo.remove(0);
            }

            var data = response.d;
            obj = JSON.parse(data);

            for (i = 0; i < obj.length; i++) {
                option = document.createElement('option');
                option.value = obj[i].id;
                option.text = obj[i].nombre;

                controlDropListSubGrupo.add(option);
            }
        }

        function ValidateForm() {
            var estadoDelFormulario = true;

            if (controlDropListEmpresa.value == 0) {
                controlLabelEmpresaError.style.display = "block";
                estadoDelFormulario = false;
            }
            if (controlDropListSucursal.value == 0) {
                controllabelSucursalError.style.display = "block";
                estadoDelFormulario = false;
            }
            if (estadoDelFormulario) {
                return true;
            }
            return false;
        }
    </script>
</asp:Content>
