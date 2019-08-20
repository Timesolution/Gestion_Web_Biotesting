<%@ Page EnableEventValidation = "false" Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ComisionesPorGrupoF.aspx.cs" Inherits="Gestion_Web.Formularios.Vendedores.Comisiones.ComisionesPorGrupoF" %>

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
                            <td style="width: 10%">
                                <div class="btn-group">
                                    <button type="button" class="btn btn-success dropdown-toggle" data-toggle="dropdown" id="btnAccion" runat="server">Accion<span class="caret"></span></button>
                                    <ul class="dropdown-menu">
                                        <li class="dropdown-submenu dropdown-menu-right"><a tabindex="-1" href="#">Exportar</a>
                                            <ul class="dropdown-menu">
                                                <li>
                                                    <asp:LinkButton ID="lbtnImprimir" runat="server" OnClick="lbtnImprimir_Click">
                                                            <i class="fa fa-file-pdf-o" aria-hidden="true"></i>
                                                            &nbsp Imprimir
                                                    </asp:LinkButton>
                                                </li>
                                                <li>
                                                    <asp:LinkButton ID="lbtnExportar" runat="server" OnClick="lbtnExportar_Click">
                                                            <i class="fa fa-file-excel-o" aria-hidden="true"></i>
                                                            &nbsp Exportar
                                                    </asp:LinkButton>
                                                </li>
                                            </ul>
                                        </li>
                                    </ul>
                                </div>
                            </td>
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
                            <h4>Neto</h4>
                            <asp:Label ID="labelNeto" runat="server" class="value"></asp:Label>
                            <asp:HiddenField id="labelNetoHidden" runat="server" />
                        </div>
                        <div class="stat">
                            <h4>Total</h4>
                            <asp:Label ID="labelTotal" runat="server" class="value"></asp:Label>
                            <asp:HiddenField id="labelTotalHidden" runat="server" />
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
                                        <th style="width: 10%">Documento</th>
                                        <th style="width: 5%">Cod. Articulo</th>
                                        <th style="width: 15%">Descripcion</th>
                                        <th style="width: 5%">Vendedor</th>
                                        <th style="width: 5%">Neto Item</th>
                                        <th style="width: 10%">Grupo</th>
                                        <th style="width: 5%">Comision</th>
                                        <th style="width: 5%">Total</th>
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
                            <label class="col-md-4">Desde</label>
                            <div class="col-md-6">
                                <asp:TextBox ID="txtFechaDesde" onchange="javascript:return ComprobacionFechaDesde()" runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-4">Hasta</label>
                            <div class="col-md-6">
                                <asp:TextBox ID="txtFechaHasta" onchange="javascript:return ComprobacionFechaHasta()" runat="server" class="form-control"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-4">Empresa</label>
                            <div class="col-md-6">
                                <asp:DropDownList ID="DropListEmpresa" runat="server" class="form-control" AutoPostBack="false"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-4">Sucursal</label>
                            <div class="col-md-6">
                                <asp:DropDownList ID="DropListSucursal" runat="server" class="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-4">Punto Venta</label>
                            <div class="col-md-6">
                                <asp:DropDownList ID="DropListPuntoVenta" runat="server" class="form-control"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-md-4">Vendedor</label>
                            <div class="col-md-6">
                                <asp:DropDownList ID="DropListVendedor" runat="server" class="form-control"></asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <asp:LinkButton ID="lbtnBuscar" href="#" OnClientClick="Filtrar(this)" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" AutoPostBack = "false"/>
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
    <script src="Comisiones.js" type="text/javascript"></script>    

    <script>
        $(function ()
        {
            var fechaActual = ObtenerFechaActual();

            var controlFechaHasta = document.getElementById('<%= txtFechaHasta.ClientID %>');
            var controlFechaDesde = document.getElementById('<%= txtFechaDesde.ClientID %>');
            var controlDropListEmpresa = document.getElementById('<%= DropListEmpresa.ClientID %>');
            var controlDropListSucursal = document.getElementById('<%= DropListSucursal.ClientID %>');

            controlFechaDesde.value = fechaActual;
            controlFechaHasta.value = fechaActual;

            $("#<%= txtFechaDesde.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
            $("#<%= txtFechaHasta.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });

            controlDropListEmpresa.addEventListener("change", CargarSucursales);
            controlDropListSucursal.addEventListener("change", CargarPuntosVenta);
            controlDropListSucursal.addEventListener("change", CargarVendedores);
        });
    </script>

    <script type="text/javascript">
        function ComprobacionFechaHasta()
        {
            var fechaActual = ObtenerFechaActual();

            var controlFechaHasta = document.getElementById('<%= txtFechaHasta.ClientID %>');
            var controlFechaDesde = document.getElementById('<%= txtFechaDesde.ClientID %>');

            var pattern = /^([0-9]{2})\/([0-9]{2})\/([0-9]{4})$/;

            //si es invalida
            if (!pattern.test(controlFechaHasta.value))
            {
                controlFechaHasta.value = fechaActual;
                return false;
            }

            //si hasta es mas chico que desde
            var desde = InvertirDiaPorMes(controlFechaDesde.value);
            var hasta = InvertirDiaPorMes(controlFechaHasta.value);
            var hoy = InvertirDiaPorMes(fechaActual);
            if (desde > hasta)
            {
                controlFechaHasta.value = fechaActual;
                controlFechaDesde.value = fechaActual;
                return false;
            }

            //si hasta es mas grande que hoy
            if (hasta > hoy)
            {
                controlFechaHasta.value = fechaActual;
                return false;
            }
        };        

        function ComprobacionFechaDesde()
        {
            var fechaActual = ObtenerFechaActual();

            var controlFechaHasta = document.getElementById('<%= txtFechaHasta.ClientID %>');
            var controlFechaDesde = document.getElementById('<%= txtFechaDesde.ClientID %>');

            var pattern = /^([0-9]{2})\/([0-9]{2})\/([0-9]{4})$/;

            //si es invalida
            if (!pattern.test(controlFechaDesde.value))
            {
                controlFechaDesde.value = fechaActual;
                return false;
            }

            var desde = InvertirDiaPorMes(controlFechaDesde.value);
            var hasta = InvertirDiaPorMes(controlFechaHasta.value);
            //si es mas grande que la fecha hasta
            if (desde > hasta)
            {
                controlFechaDesde.value = fechaActual;
                controlFechaHasta.value = fechaActual;
                return false;
            }
        };

        function CargarSucursales()
        {
            var controlDropListEmpresa = document.getElementById('<%= DropListEmpresa.ClientID %>').value;

            $.ajax({
                type: "POST",
                url: "ComisionesPorGrupoF.aspx/RecargarSucursales",
                data: '{empresa: "' + controlDropListEmpresa + '"  }',
                contentType: "application/json",
                dataType: 'json',
                error: function ()
                {
                    alert("No se pudieron cargar las sucursales.");
                },
                success: OnSuccessSucursal
            });
        };

        function OnSuccessSucursal(response)
        {
            var controlDropListSucursal = document.getElementById('<%= DropListSucursal.ClientID %>');

            while (controlDropListSucursal.options.length > 0)
            {
                controlDropListSucursal.remove(0);
            } 

            var data = response.d;
            obj = JSON.parse(data);

            for (i = 0; i < obj.length; i++)
            {
                option = document.createElement('option');
                option.value = obj[i].id;
                option.text = obj[i].nombre;

                controlDropListSucursal.add(option);
            }
        }

        function CargarPuntosVenta()
        {
            var DropListSucursal = document.getElementById('<%= DropListSucursal.ClientID %>').value;

            $.ajax({
                type: "POST",
                url: "ComisionesPorGrupoF.aspx/RecargarPuntoVenta",
                data: '{sucursal: "' + DropListSucursal + '"  }',
                contentType: "application/json",
                dataType: 'json',
                error: function ()
                {
                    alert("No se pudieron cargar los puntos de venta.");
                },
                success: OnSuccessPuntoVenta
            });
        };

        function OnSuccessPuntoVenta(response)
        {
            var controlDropListPuntoVenta = document.getElementById('<%= DropListPuntoVenta.ClientID %>');

            while (controlDropListPuntoVenta.options.length > 0)
            {
                controlDropListPuntoVenta.remove(0);
            } 

            var data = response.d;
            obj = JSON.parse(data);

            for (i = 0; i < obj.length; i++)
            {
                option = document.createElement('option');
                option.value = obj[i].id;
                option.text = obj[i].nombreFantasia;

                controlDropListPuntoVenta.add(option);
            }
        }

        function CargarVendedores()
        {
            var DropListSucursal = document.getElementById('<%= DropListSucursal.ClientID %>').value;

            $.ajax({
                type: "POST",
                url: "ComisionesPorGrupoF.aspx/RecargarVendedores",
                data: '{sucursal: "' + DropListSucursal + '"  }',
                contentType: "application/json",
                dataType: 'json',
                error: function ()
                {
                    alert("No se pudieron cargar los vendedores.");
                },
                success: OnSuccessVendedores
            });
        };

        function OnSuccessVendedores(response)
        {
            var controlDropListVendedor = document.getElementById('<%= DropListVendedor.ClientID %>');

            while (controlDropListVendedor.options.length > 0)
            {
                controlDropListVendedor.remove(0);
            } 

            var data = response.d;
            obj = JSON.parse(data);

            for (i = 0; i < obj.length; i++)
            {
                option = document.createElement('option');
                option.value = obj[i].id;
                option.text = obj[i].nombre;

                controlDropListVendedor.add(option);
            }
        }

        function Filtrar(obj)
        {
            var valorTxtFechaDesde = document.getElementById('<%= txtFechaDesde.ClientID %>').value;
            var valorTxtFechaHasta = document.getElementById('<%= txtFechaHasta.ClientID %>').value;
            var valorDropListEmpresa = document.getElementById('<%= DropListEmpresa.ClientID %>').value;
            var valorDropListSucursal = document.getElementById('<%= DropListSucursal.ClientID %>').value;
            var valorDropListPuntoVenta = document.getElementById('<%= DropListPuntoVenta.ClientID %>').value;
            var valorDropListVendedor = document.getElementById('<%= DropListVendedor.ClientID %>').value;            

            var fechaDesde = InvertirDiaPorMes(valorTxtFechaDesde);
            var fechaHasta = InvertirDiaPorMes(valorTxtFechaHasta);
            $(obj).attr('disabled', 'disabled');

            $.ajax({
                type: "POST",
                url: "ComisionesPorGrupoF.aspx/Filtrar",
                data: '{ fechaDesde: "' + fechaDesde.toUTCString() + '", fechaHasta: "' + fechaHasta.toUTCString() + '", idEmpresa: "' + valorDropListEmpresa + '", idSucursal: "' + valorDropListSucursal + '", idPuntoVenta: "' + valorDropListPuntoVenta + '", vendedor: "' + valorDropListVendedor +'" }',
                contentType: "application/json",
                dataType: 'json',
                error: (error) =>
                {
                    //console.log(JSON.stringify(error));
                    $.msgbox("No se pudo filtrar correctamente!", {type: "error"});
                    $(obj).removeAttr('disabled');
                }
                ,
                success: OnSuccessFiltro
            });            
        }

        function OnSuccessFiltro(response)
        {
            var controlLabelNeto = document.getElementById('<%= labelNeto.ClientID %>');
            var controlLabelTotal = document.getElementById('<%= labelTotal.ClientID %>');
            var controlBotonFiltrar = document.getElementById('<%= lbtnBuscar.ClientID %>');
            var totalNetoHidden = document.getElementById('<%= labelNetoHidden.ClientID %>');
            var totalHidden = document.getElementById('<%= labelTotalHidden.ClientID %>');

            var data = response.d;
            var obj = JSON.parse(data);

            document.getElementById('btnCerrarModalBusqueda').click();
            $("#tablaComisiones").dataTable().fnDestroy();
            $('#tablaComisiones').find("tr:gt(0)").remove();

            var totalNeto = 0;
            var total = 0;

            for (var i = 0; i < obj.length; i++) {
                $('#tablaComisiones').append(
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

            controlLabelNeto.innerHTML = "$" + totalNeto.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ",").toString();
            controlLabelTotal.innerHTML = "$" + total.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ",").toString();

            totalNetoHidden.value = totalNeto.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ",").toString();
            totalHidden.value = total.toFixed(2).replace(/\B(?=(\d{3})+(?!\d))/g, ",").toString();

            $('#tablaComisiones').dataTable(
                {                    
                    //"bLengthChange": false,
                    "bFilter": false,
                    "bInfo": false,
                    "bAutoWidth": false,
                    "bStateSave": true,
                    "pageLength": 25,
                    "columnDefs": [
                        { type: 'date-eu', targets: 5 }
                    ]
                });

            $(controlBotonFiltrar).removeAttr('disabled');
        }
    </script>

</asp:Content>
