<%@ Page Title="Gestion Solution" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gestion_Web._Default" %>



<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">

        <div class="container">

            <div class="row">
                <asp:PlaceHolder ID="phMain" Visible="false" runat="server">
                    <div class="col-md-6 col-xs-12">

                        <div class="widget widget-nopad stacked">

                            <div class="widget-header">
                                <i class="icon-warning-sign"></i>
                                <h3>Alertas</h3>
                            </div>
                            <!-- /widget-header -->

                            <div class="widget-content">
                                <br />
                                <div role="form" class="form-horizontal col-md-12">

                                    <div onclick="location.href='Formularios/Articulos/Articulos.aspx?accion=3&d=1';" class="alert alert-danger alert-dismissable">
                                        <button type="button" class="close" data-dismiss="alert" aria-hidden="true"></button>
                                        <a href="Formularios/Articulos/Articulos.aspx?accion=3&d=1" class="alert-link">Cambios de Precios</a>.
                                    </div>

                                    <div onclick="location.href='Formularios/Reportes/ReportesCobros.aspx';" class="alert alert-danger alert-dismissable">
                                        <button type="button" class="close" data-dismiss="alert" aria-hidden="true"></button>
                                        <a href="Formularios/Reportes/ReportesCobros.aspx" class="alert-link">Facturas Impagas</a>.
                                    </div>
                                    <div class="alert alert-warning alert-dismissable">
                                        <asp:LinkButton ID="lbtnAlertaPedidos" Text="Pedidos de vendedores pendientes: " runat="server" class="alert-link" OnClick="lbtnAlertaPedidos_Click" />
                                    </div>
                                </div>
                            </div>
                            <!-- /widget-content -->

                        </div>
                        <!-- /widget -->


                        <div class="widget stacked">

                            <div class="widget-header">
                                <i class="icon-file"></i>
                                <h3>Post-it (Notas Rapidas) </h3>
                            </div>
                            <!-- /widget-header -->

                            <div class="widget-content" style="background-color: #ffffe6">
                                <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Always" runat="server">
                                    <ContentTemplate>
                                        <asp:TextBox ID="txtMemo" runat="server" class="form-control" TextMode="MultiLine" Style="width: 100%" OnTextChanged="txtMemo_TextChanged" AutoPostBack="true" Rows="12" BackColor="#ffffe6" BorderStyle="None" ForeColor="#6666ff"></asp:TextBox>
                                        <%--<input type="text" class="form-control" name="name" id="name">--%>
                                    </ContentTemplate>
                                    <Triggers>
                                    </Triggers>
                                </asp:UpdatePanel>



                            </div>
                            <!-- /widget-content -->

                        </div>
                        <!-- /widget -->

                    </div>
                    <!-- /span6 -->


                    <div class="col-md-6">


                        <div class="widget stacked">

                            <div class="widget-header">
                                <i class="icon-bookmark"></i>
                                <h3>Mis Accesos</h3>
                            </div>
                            <!-- /widget-header -->

                            <div class="widget-content">

                                <div class="shortcuts">
                                    <a href="Formularios/Clientes/Clientesaspx.aspx" class="shortcut">
                                        <i class="shortcut-icon icon-group"></i>
                                        <span class="shortcut-label">Clientes</span>
                                    </a>

                                    <a href="Formularios/Articulos/Articulos.aspx" class="shortcut">
                                        <i class="shortcut-icon icon-bookmark"></i>
                                        <span class="shortcut-label">Articulos</span>
                                    </a>

                                    <a href="Formularios/Facturas/ABMPedidos.aspx" class="shortcut">
                                        <i class="shortcut-icon icon-shopping-cart"></i>
                                        <span class="shortcut-label">Pedidos</span>
                                    </a>

                                    <a href="Formularios/Facturas/ABMFacturas.aspx" class="shortcut">
                                        <i class="shortcut-icon icon-file-text"></i>
                                        <span class="shortcut-label">Facturación</span>
                                    </a>

                                    <a href="Formularios/Cobros/CobranzaF.aspx" class="shortcut">
                                        <i class="shortcut-icon icon-money"></i>
                                        <span class="shortcut-label">Cobros</span>
                                    </a>

                                    <a href="Formularios/Valores/CajaF.aspx" class="shortcut">
                                        <i class="shortcut-icon icon-inbox"></i>
                                        <span class="shortcut-label">Caja</span>
                                    </a>

                                    <a href="Formularios/Facturas/CuentaCorrienteF.aspx" class="shortcut">
                                        <i class="shortcut-icon icon-th-list"></i>
                                        <span class="shortcut-label">Cta Cte</span>
                                    </a>
                                    <a runat="server" id="etiquetaMuñoz"  class="shortcut">
                                        <i runat="server" id="iconoMuñoz"></i>
                                        <span runat="server" id="spanMuñoz" class="shortcut-label"></span>
                                    </a>
                                </div>
                                <!-- /shortcuts -->

                            </div>
                            <!-- /widget-content -->

                        </div>
                        <!-- /widget -->
                        <asp:Panel ID="panelMascotas" runat="server" Visible="false">
                            <div class="widget stacked">

                                <div class="widget-header">
                                    <i class="icon-github-alt"></i>
                                    <h3>Mascotas</h3>
                                </div>
                                <!-- /widget-header -->

                                <div class="widget-content">

                                    <div class="shortcuts">
                                        <asp:LinkButton ID="lbtnMascotasLink" runat="server" class="shortcut" OnClick="lbtnMascotasLink_Click">
                                            <i class="shortcut-icon icon-group"></i>
                                            <span class="shortcut-label">Propietarios</span>
                                        </asp:LinkButton>

                                        <asp:LinkButton ID="lbtnMascotasLink2" runat="server" class="shortcut" OnClick="lbtnMascotasLink2_Click">
                                            <i class="shortcut-icon icon-github-alt"></i>
                                            <span class="shortcut-label">Mascotas</span>
                                        </asp:LinkButton>

                                        <asp:LinkButton ID="lbtnMascotasLink3" runat="server" class="shortcut" OnClick="lbtnMascotasLink3_Click">
                                            <i class="shortcut-icon icon-calendar"></i>
                                            <span class="shortcut-label">Agenda</span>
                                        </asp:LinkButton>

                                        <asp:LinkButton ID="lbtnMascotasLink4" runat="server" class="shortcut" OnClick="lbtnMascotasLink4_Click">
                                            <i class="shortcut-icon icon-stethoscope"></i>
                                            <span class="shortcut-label">Historias</span>
                                        </asp:LinkButton>

                                    </div>
                                    <!-- /shortcuts -->

                                </div>
                                <!-- /widget-content -->

                            </div>
                            <!-- /widget -->
                        </asp:Panel>



                        <div class="widget stacked widget-table action-table">

                            <div class="widget-header">
                                <i class="icon-th-list"></i>
                                <h3>Facturas Emitidas por Sucursal</h3>
                            </div>
                            <!-- /widget-header -->

                            <div class="widget-content">

                                <table class="table table-striped table-bordered">
                                    <thead>
                                        <tr>
                                            <th>Sucursal</th>
                                            <th class="td-actions"></th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <asp:PlaceHolder ID="phSucursales" runat="server"></asp:PlaceHolder>
                                    </tbody>
                                </table>

                            </div>
                            <!-- /widget-content -->

                        </div>
                        <!-- /widget -->


                        <div class="widget stacked">
                            <div class="widget-header" style="background: #c1f4de">
                                <table>
                                    <tr>
                                        <td style="width: 92%">
                                            <i class="icon-time"></i>
                                            <h3>Prox. vencimientos CRM</h3>
                                        </td>
                                        <td style="width: 8%">
                                            <a class="btn ui-tooltip" href="Formularios/Facturas/CRM.aspx?fechadesde=01/01/2000&fechaHasta=01/01/2000&fechaVencimientoDesde=<%=DateTime.Now.ToString("dd/MM/yyyy") %>&fechaVencimientoHasta=<%=DateTime.Now.AddDays(7).ToString("dd/MM/yyyy") %>&cl=-1&estado=0&fpf=0&fpfv=1&us=-1&des=" target="_blank" style="color: black; margin-bottom: 2%;" title data-original-title="Ver mas">
                                                <i class='fa fa-arrow-right'></i>
                                            </a>
                                        </td>
                                    </tr>
                                </table>

                            </div>
                            <div class="widget-content" style="padding-top: 1%;">
                                <ul class="news-items">
                                    <asp:PlaceHolder ID="phSeguimiento" runat="server"></asp:PlaceHolder>
                                </ul>
                            </div>
                        </div>

                        <div class="widget stacked">
                            <div class="widget-header" style="background: #f2dede">
                                <table>
                                    <tr>
                                        <td style="width: 92%">
                                            <i class="icon-time"></i>
                                            <h3>CRM vencidos</h3>
                                        </td>
                                        <td style="width: 8%">

                                            <a class="btn ui-tooltip" href="Formularios/Facturas/CRM.aspx?fechadesde=01/01/2000&fechaHasta=<%=DateTime.Now.ToString("dd/MM/yyyy") %>&fechaVencimientoDesde=01/01/2000&fechaVencimientoHasta=<%=DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy") %>&cl=-1&estado=1&fpf=0&fpfv=1&us=-1" target="_blank" style="color: black; margin-bottom: 2%;" title data-original-title="Ver mas">
                                                <i class='fa fa-arrow-right'></i>
                                            </a>
                                        </td>
                                    </tr>
                                </table>

                            </div>
                            <div class="widget-content" style="padding-top: 1%;">
                                <ul class="news-items">
                                    <asp:PlaceHolder ID="phSeguimientoVencidos" runat="server"></asp:PlaceHolder>
                                </ul>
                            </div>
                        </div>

                    </div>
                    <!-- /span6 -->
                </asp:PlaceHolder>
                <asp:PlaceHolder ID="phClientes" Visible="false" runat="server">
                    <div class="col-md-12">
                        <div class="widget stacked">

                            <div class="widget-header">
                                <i class="icon-bookmark"></i>
                                <h3>Mis Accesos</h3>
                            </div>

                            <div class="widget-content">
                                <div class="shortcuts">
                                    <a href="Formularios/Articulos/ArticulosC.aspx" class="shortcut">
                                        <i class="shortcut-icon icon-bookmark"></i>
                                        <span class="shortcut-label">Crear Pedidos</span>
                                    </a>

                                    <asp:LinkButton ID="btnHacerPedido" runat="server" Visible="false" CssClass="shortcut" href="Formularios/Facturas/ABMPedidos.aspx">
                                        <i class="shortcut-icon icon-plus"></i>
                                        <span class="shortcut-label">Hacer Pedido</span>
                                    </asp:LinkButton>

                                      <asp:LinkButton ID="btnRecepcionMercaderia" runat="server" Visible="false" CssClass="shortcut" href="Formularios/Facturas/FacturasMercaderiasF.aspx">
                                        <i class="shortcut-icon icon-truck"></i>
                                        <span class="shortcut-label">Recepcion</span>
                                    </asp:LinkButton>
                                    <%--<a href="Formularios/Facturas/ABMPedidos.aspx" class="shortcut">
                                        <i class="shortcut-icon icon-plus"></i>
                                        <span class="shortcut-label">Hacer Pedido</span>
                                    </a>--%>
                                    <%--<asp:LinkButton ID="btnPedidos" runat="server"CssClass="shortcut" OnClick="btnPedidos_Click">
                                       
                                    </asp:LinkButton>--%>
                                    <a href="Formularios/Facturas/PedidosP.aspx" class="shortcut">
                                        <i class="shortcut-icon icon-shopping-cart"></i>
                                        <span class="shortcut-label">Listado Pedidos</span>
                                    </a>
                                    <asp:LinkButton ID="btnCuentaCorriente" runat="server" CssClass="shortcut" href="Formularios/Facturas/CuentaCorrienteF.aspx">
                                        <i class="shortcut-icon icon-th-list"></i>
                                        <span class="shortcut-label">Cuenta Corriente</span>
                                    </asp:LinkButton>
                                   
                                     <asp:LinkButton ID="LinkButton1" runat="server" Visible="false" CssClass="shortcut" href="#modalBusqueda" data-toggle="modal">
                                        <i class="shortcut-icon icon-list-alt "></i>
                                        <span class="shortcut-label">Reportes Art</span>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnCliente2" runat="server" Visible="false" CssClass="shortcut" href="Formularios/Clientes/ClientesABM.aspx?accion=1">
                                        <i class="shortcut-icon icon-group"></i>
                                        <span class="shortcut-label">Mi Equipo</span>
                                    </asp:LinkButton>
                                    <asp:LinkButton ID="btnUsuario" runat="server" Visible="false" CssClass="shortcut" href="Formularios/Seguridad/UsuariosF.aspx">
                                        <i class="shortcut-icon icon-user"></i>
                                        <span class="shortcut-label">Crear Usuario</span>
                                    </asp:LinkButton>
                                       <asp:LinkButton ID="LinkButton2" runat="server" Visible="false" CssClass="shortcut" href="Formularios/Reportes/InformesN.aspx">
                                        <i class="shortcut-icon icon-list-alt "></i>
                                        <span class="shortcut-label">Reportes</span>
                                    </asp:LinkButton>
                                   <%-- <a href="Formularios/Facturas/CuentaCorrienteF.aspx" class="shortcut">
                                        <i class="shortcut-icon icon-th-list"></i>
                                        <span class="shortcut-label">Cuenta Corriente</span>
                                    </a>--%>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:PlaceHolder>

                <asp:PlaceHolder ID="phImportacion" Visible="false" runat="server">
                    <div class="col-md-12">


                        <div class="widget stacked">

                            <div class="widget-header">
                                <i class="icon-bookmark"></i>
                                <h3>Mis Accesos</h3>
                            </div>
                            <!-- /widget-header -->

                            <div class="widget-content">

                                <div class="shortcuts">

                                    <a href="Formularios/Facturas/FacturasImportacion.aspx" class="shortcut">
                                        <i class="shortcut-icon icon-plus"></i>
                                        <span class="shortcut-label">Importar Archivo</span>
                                    </a>

                                    <a href="Formularios/Articulos/Articulos.aspx" class="shortcut">
                                        <i class="shortcut-icon icon-bookmark"></i>
                                        <span class="shortcut-label">Articulos</span>
                                    </a>

                                    <a href="Formularios/Clientes/Clientesaspx.aspx" class="shortcut">
                                        <i class="shortcut-icon icon-group"></i>
                                        <span class="shortcut-label">Clientes</span>
                                    </a>

                                    <a href="Formularios/Facturas/FacturasF.aspx" class="shortcut">
                                        <i class="shortcut-icon icon-file-text"></i>
                                        <span class="shortcut-label">Facturas</span>
                                    </a>

                                    <a href="Formularios/Facturas/RemitosR.aspx" class="shortcut">
                                        <i class="shortcut-icon icon-th-list"></i>
                                        <span class="shortcut-label">Remitos</span>
                                    </a>





                                    <%--<a href="Formularios/Facturas/PedidosP.aspx" class="shortcut">
                                    <i class="shortcut-icon icon-shopping-cart"></i>
                                    <span class="shortcut-label">Listado Pedidos</span>
                                </a>

                                <a href="Formularios/Facturas/CuentaCorrienteF.aspx" class="shortcut">
                                    <i class="shortcut-icon icon-th-list"></i>
                                    <span class="shortcut-label">Cuenta Corriente</span>
                                </a>--%>
                                </div>
                                <!-- /shortcuts -->

                            </div>
                            <!-- /widget-content -->

                        </div>
                        <!-- /widget -->


                    </div>

                </asp:PlaceHolder>

            </div>
            <!-- /row -->

            <a class="btn btn-info" data-toggle="modal" id="abreDenegado" href="#modalDenegado" style="display: none">Agregar Tipo Cliente</a>

        </div>

         <div id="modalBusqueda" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">

                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Busqueda</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <asp:UpdatePanel ID="UpdatePanel4" UpdateMode="Always" runat="server">
                                <ContentTemplate>
                                    <div class="form-group">
                                        <label class="col-md-4">Desde</label>
                                        <div class="col-md-4">

                                            <asp:TextBox ID="txtFechaDesde" runat="server" class="form-control"></asp:TextBox>

                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-4">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaDesde" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Hasta</label>
                                        <div class="col-md-4">
                                            <asp:TextBox ID="txtFechaHasta" runat="server" class="form-control"></asp:TextBox>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="txtFechaHasta" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                        <!-- /input-group -->

                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Sucursal</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListSucursal" disabled runat="server" class="form-control"></asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListSucursal" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Proveedor</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListProveedores" disabled runat="server" class="form-control" AutoPostBack="true" ></asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListProveedores" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Grupo</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListGrupo"  disabled runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="DropListGrupo_SelectedIndexChanged"></asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListGrupo" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">SubGrupo</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListSubGrupo" disabled runat="server" class="form-control"></asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListSubGrupo" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Articulo</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtDescArticulo" class="form-control" runat="server"></asp:TextBox>
                                        </div>

                                        <div class="col-md-2">
                                            <asp:LinkButton ID="btnBuscarCod" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" OnClick="btnBuscarCod_Click" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Articulo</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListArticulos" runat="server" class="form-control"></asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListArticulos" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Cliente</label>
                                        <div class="col-md-6">
                                            <asp:TextBox ID="txtDescCliente" class="form-control" runat="server"></asp:TextBox>
                                        </div>

                                        <div class="col-md-2">
                                            <asp:LinkButton ID="btnBuscarCliente" runat="server" Text="<span class='shortcut-icon icon-search'></span>" class="btn btn-info" OnClick="btnBuscarCliente_Click" />
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Cliente</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListClientes" runat="server" class="form-control"></asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListClientes" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-md-4">Vendedor</label>
                                        <div class="col-md-6">
                                            <asp:DropDownList ID="DropListVendedores" runat="server" class="form-control"></asp:DropDownList>
                                            <!-- /input-group -->
                                        </div>
                                        <div class="col-md-2">
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="DropListVendedores" InitialValue="-1" ValidationGroup="BusquedaGroup" SetFocusOnError="true" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </ContentTemplate>
                                <Triggers>
                                </Triggers>
                            </asp:UpdatePanel>
                        </div>
                    </div>


                    <div class="modal-footer">
                        <asp:LinkButton ID="lbtnReporte" runat="server" Text="<span class='shortcut-icon icon-ok'></span>" class="btn btn-success" OnClick="lbtnReporte_Click" ValidationGroup="BusquedaGroup" />
                    </div>
                </div>

            </div>
        </div>

        <div id="modalCRM" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">CRM</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <div class="col-md-2">
                                <h1>
                                    <i class="icon-warning-sign" style="color: orange"></i>
                                </h1>
                            </div>
                            <div class="col-md-7">
                                <h5>
                                    <asp:Label runat="server" ID="lblMensaje" Text="TIENE TAREAS PENDIENTES EN EL CRM" Style="text-align: center"></asp:Label>
                                </h5>
                            </div>

                        </div>

                    </div>
                    <div class="modal-footer" style="background: #f7f7f7">
                        <button type="button" class="btn btn-default" data-dismiss="modal" aria-hidden="true">Aceptar</button>

                    </div>

                </div>
            </div>
        </div>

        <div id="modalDenegado" class="modal fade" tabindex="-1" role="dialog">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <%--<button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>--%>
                        <a href="/Default.aspx" class="close">x</a>
                        <h4 class="modal-title">Error!</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <div class="form-group">
                                <div class="col-md-4">
                                    <h1>
                                        <i class="icon-lock"></i>
                                    </h1>
                                </div>
                                <div class="col-md-8">
                                    <h4>
                                        <asp:Label runat="server" ID="Label1" Text="Acceso Denegado!!!" Style="text-align: center"></asp:Label>
                                    </h4>
                                </div>

                                <%--<div class="col-md-3">
                                        <asp:TextBox runat="server" ID="TextBox1" Text="0" style="display: none"></asp:TextBox>
                                    </div>--%>
                            </div>
                            <%--                                <div class="form-group">
                                    
                                </div>--%>
                        </div>


                        <div class="modal-footer">
                            <button type="button" class="btn btn-info" data-dismiss="modal" aria-hidden="true">Continuar</button>
                        </div>
                    </div>

                </div>
            </div>
        </div>
        <%--Fin modalGrupo--%>

        <!-- /main -->

        <!-- Le javascript
================================================== -->
        <!-- Placed at the end of the document so the pages load faster -->

        <script>

            function abrirDenegado() {
                document.getElementById('abreDenegado').click();
            }


        </script>



        <!-- Core Scripts - Include with every page -->
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

        <script type="text/javascript">
            function modalCRM() {
                $('#modalCRM').modal('show');
            }
        </script>
        <script>

            $(function () {
                $("#<%= txtFechaDesde.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });

        $(function () {
            $("#<%= txtFechaHasta.ClientID %>").datepicker({ dateFormat: 'dd/mm/yy' });
        });
        </script>

    </div>
</asp:Content>
