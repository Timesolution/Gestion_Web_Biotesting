﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ActividadClientes.aspx.cs" Inherits="Gestion_Web.Formularios.Clientes.ActividadClientes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <div class="col-md-12 col-xs-12">
            <div class="widget stacked">
                <div class="stat">
                    <h5><i class="icon-map-marker"></i>Maestros > Comisiones > Comisiones Por Vendedores</h5>
                </div>
                <div class="widget-header">
                    <i class="icon-wrench"></i>
                    <h3>Herramientas</h3>
                </div>
                <div class="widget-content">
                    <table style="width: 100%">
                        <tr>
                            <td style="width: 65%">
                                <h5>
                                    <asp:Label runat="server" ID="lblParametros" Text="" ForeColor="#cccccc"></asp:Label>
                                </h5>
                            </td>
                            <td style="width: 5%">
                                <div class="shortcuts" style="height: 100%">
                                    <a class="btn btn-primary" data-toggle="modal" data-placement="top" title="Tooltip on top" href="#modalBusqueda" style="width: 100%">
                                        <i class="shortcut-icon icon-filter"></i>
                                    </a>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>

        <div class="col-md-12">
            <div class="widget big-stats-container stacked">
                <div class="widget-content">
                    <div id="big_stats" class="cf">
                        <div class="stat">
                            <h4>Total</h4>
                            <asp:Label ID="labelNeto" runat="server" class="value"></asp:Label>
                            <asp:HiddenField id="labelNetoHidden" runat="server" />
                        </div>
                        <div class="stat">
                            <h4>Actividad</h4>
                            <asp:Label ID="labelTotal" runat="server" class="value"></asp:Label>
                            <asp:HiddenField id="labelTotalHidden" runat="server" />
                        </div>
                        <div class="stat">
                            <h4>Porcentaje</h4>
                            <asp:Label ID="labelPorcentaje" runat="server" class="value"></asp:Label>
                            <asp:HiddenField id="labelPorcentajeHidden" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-md-12 col-xs-12">
            <div class="widget stacked widget-table action-table">
                <div class="widget-header">
                    <i class="icon-money" style="width: 2%"></i>
                    <h3 style="width: 75%">Comisiones</h3>
                </div>
                <div class="widget-content">
                    <div class="panel-body">
                        <div class="table-responsive">
                            <table class="table table-striped table-bordered table-hover" id="tablaComisiones">
                                <thead>
                                    <tr>
                                        <th style="width: 5%">Fecha</th>
                                        <th style="width: 10%">Cliente</th>
                                        <th style="width: 5%">Codigo</th>
                                        <th style="width: 15%">Actividad hasta fecha</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:PlaceHolder ID="phComisiones" runat="server"></asp:PlaceHolder>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div id="modalBusqueda" class="modal fade" tabindex="-1" role="dialog">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" id="btnCerrarModalBusqueda" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Busqueda</h4>
                </div>
                <div class="modal-body">
                    <div role="form" class="form-horizontal col-md-12">
                        <div class="form-group">
                            <label class="col-md-4">Sucursal</label>
                            <div class="col-md-6">
                                <asp:DropDownList ID="DropDownListSucursales" runat="server" class="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-4">Vendedor</label>
                            <div class="col-md-6">
                                <asp:DropDownList ID="DropDownListVendedores" runat="server" class="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-4">Provincia</label>
                            <div class="col-md-6">
                                <asp:DropDownList ID="DropDownListProvincias" runat="server" class="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-4">Localidad</label>
                            <div class="col-md-6">
                                <asp:DropDownList ID="DropDownListLocalidades" runat="server" class="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-4">Dias Actividad</label>
                            <div class="col-md-6">
                                <asp:TextBox ID="txtDiasActividad" TextMode="Number" value="30" runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbtnBuscar" OnClientClick="Filtrar()" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success"/>
                </div>
            </div>
        </div>
    </div>

    <script src="../../../Scripts/libs/jquery-1.9.1.min.js"></script>
    <script src="../../../Scripts/libs/jquery-ui-1.10.0.custom.min.js"></script>
    <link href="../../../css/pages/reports.css" rel="stylesheet">
    <script src="//cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>
    <script src="//cdn.datatables.net/plug-ins/1.10.9/sorting/date-eu.js"></script>
    <script src="../../../Scripts/plugins/dataTables/custom.tables.js"></script>
    <link href="//cdn.datatables.net/1.10.2/css/jquery.dataTables.css" rel="stylesheet" />
    <script src="../../../Scripts/plugins/lightbox/jquery.lightbox.min.js"></script>
    <script src="../../../Scripts/plugins/msgbox/jquery.msgbox.min.js"></script>
    <script src="../../../scripts/demo/notifications.js"></script>

    <script>
        $(function ()
        {
            controlDropListSucursal = document.getElementById('<%= DropDownListSucursales.ClientID %>');
            controlDropListProvincias = document.getElementById('<%= DropDownListProvincias.ClientID %>');
            CargarVendedores();
            CargarLocalidades();
            controlDropListSucursal.addEventListener("change", CargarVendedores);
            controlDropListProvincias.addEventListener("change", CargarLocalidades);
        });

        function Filtrar()
        {
            event.preventDefault();

            var controlDropListLocalidad = document.getElementById('<%= DropDownListLocalidades.ClientID %>');
            var controlDropListProvincia = document.getElementById('<%= DropDownListProvincias.ClientID %>');
            var valorDiasActividad = document.getElementById('<%= txtDiasActividad.ClientID %>').value;

            var localidad = controlDropListLocalidad.selectedOptions[0].text;
            var provincia = controlDropListProvincia.selectedOptions[0].value;

            $.ajax({
                type: "POST",
                url: "ActividadClientes.aspx/Filtrar",
                contentType: "application/json",
                data: '{provincia: "' + provincia + '", localidad: "' + localidad + '", diasActividad: "' + valorDiasActividad + '"  }',
                dataType: 'json',
                error: function () {
                    $.msgbox("Error al filtrar.", { type: "error" });
                },
                success: OnSuccessFiltro
            });
        }

        function OnSuccessFiltro(response)
        {
            <%--var controlLabelNeto = document.getElementById('<%= labelNeto.ClientID %>');
            var controlLabelTotal = document.getElementById('<%= labelTotal.ClientID %>');
            var controlBotonFiltrar = document.getElementById('<%= lbtnBuscar.ClientID %>');
            var totalNetoHidden = document.getElementById('<%= labelNetoHidden.ClientID %>');
            var totalHidden = document.getElementById('<%= labelTotalHidden.ClientID %>');--%>

            var data = response.d;
            var obj = JSON.parse(data);

            document.getElementById('btnCerrarModalBusqueda').click();
            var table = $('#articulosTablaProveedor').DataTable({ "paging": false, "bInfo": false, "searching": false, "retrieve": true,"ordering": false});
            //$("#tablaComisiones").dataTable().fnDestroy();
            //$('#tablaComisiones').find("tr:gt(0)").remove();

            var totalNeto = 0;
            var total = 0;

            for (var i = 0; i < obj.length; i++) {
                table.append(
                    "<tr>" +
                    "<td> " + obj[i].fecha + "</td>" +
                    "<td> " + obj[i].tipo + "</td>" +
                    "<td> " + obj[i].codigo + "</td>" +
                    "<td> " + obj[i].descripcion + "</td>" +
                    "<td> " + obj[i].nombre + "</td>" +
                    '<td style="text-align:right"> ' + obj[i].precioSinIVA + "</td>" +
                    "<td> " + obj[i].grupoArticulo + "</td>" +
                    '<td style="text-align:right"> ' + obj[i].comision + "</td>" +
                    '<td style="text-align:right"> ' + obj[i].total + "</td>" +
                    "</tr> ");
                
                var splittedNeto = obj[i].precioSinIVA.split("$");
                var splittedTotal = obj[i].total.split("$");

                var numeroNeto = parseFloat(splittedNeto[1]);
                var numeroTotal = parseFloat(splittedTotal[1]);

                if (obj[i].tipo.toLowerCase().includes("nota"))
                {
                    numeroNeto = numeroNeto * (-1);
                    numeroTotal = numeroTotal * (-1);
                }

                totalNeto += parseFloat(numeroNeto);
                total += parseFloat(numeroTotal);
            };

            //controlLabelNeto.innerHTML = "$" + totalNeto.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ",").toString();
            //controlLabelTotal.innerHTML = "$" + total.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ",").toString();

            //totalNetoHidden.value = totalNeto.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ",").toString();
            //totalHidden.value = total.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ",").toString();

            //$('#tablaComisiones').dataTable(
            //    {                    
            //        //"bLengthChange": false,
            //        "bFilter": false,
            //        "bInfo": false,
            //        "bAutoWidth": false,
            //        "bStateSave": true,
            //        "pageLength": 25,
            //        "columnDefs": [
            //            { type: 'date-eu', targets: 5 }
            //        ]
            //    });

            //$(controlBotonFiltrar).removeAttr('disabled');
        }

        function CargarLocalidades()
        {
            event.preventDefault();

            var DropListProvincia = document.getElementById('<%= DropDownListProvincias.ClientID %>');

            var provincia = DropListProvincia.selectedOptions[0].text;

            $.ajax({
                type: "POST",
                url: "ActividadClientes.aspx/ObtenerLocalidades",
                contentType: "application/json",
                data: '{provincia: "' + provincia + '"  }',
                dataType: 'json',
                error: function () {
                    $.msgbox("No se pudieron cargar las localidades.", { type: "error" });
                },
                success: OnSuccessLocalidades
            });
        };

        function OnSuccessLocalidades(response)
        {
            var controlDropListLocalidades = document.getElementById('<%= DropDownListLocalidades.ClientID %>');

            while (controlDropListLocalidades.options.length > 0) {
                controlDropListLocalidades.remove(0);
            }

            var data = response.d;
            obj = JSON.parse(data);

            for (i = 0; i < obj.length; i++) {
                option = document.createElement('option');
                option.value = obj[i].Localidad;
                option.text = obj[i].Localidad;

                controlDropListLocalidades.add(option);
            }
        }

       function CargarVendedores()
       {
           event.preventDefault();

           var DropListSucursal = document.getElementById('<%= DropDownListSucursales.ClientID %>').value;

            $.ajax({
                type: "POST",
                url: "ActividadClientes.aspx/ObtenerVendedores",
                contentType: "application/json",
                data: '{idSucursal: "' + DropListSucursal + '"  }',
                dataType: 'json',
                error: function ()
                {
                    $.msgbox("No se pudieron cargar los vendedores.", {type: "error"});
                },
                success: OnSuccessVendedores
            });
        };

        function OnSuccessVendedores(response)
        {
            var controlDropListVendedores = document.getElementById('<%= DropDownListVendedores.ClientID %>');

            while (controlDropListVendedores.options.length > 0)
            {
                controlDropListVendedores.remove(0);
            } 

            var data = response.d;
            obj = JSON.parse(data);

            for (i = 0; i < obj.length; i++)
            {
                option = document.createElement('option');
                option.value = obj[i].id;
                option.text = obj[i].nombre;

                controlDropListVendedores.add(option);
            }
        }
    </script>

</asp:Content>
