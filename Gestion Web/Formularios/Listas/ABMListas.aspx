﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ABMListas.aspx.cs" Inherits="Gestion_Web.Formularios.Facturas.AdminListas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">


        <div class="row">

            <div class="col-md-12">

                <div class="widget stacked ">

                    <div class="widget-header">
                        <i class="icon-pencil"></i>
                        <h3>Listas de Precios</h3>

                    </div>
                    <!-- /.widget-header -->


                    <div class="widget-content">
                        <div class="col-md-6">
                            <table id="tbListas" class="table table-striped table-bordered">
                                <thead>
                                    <tr>
                                        <th style="width: 25%">Nombre</th>
                                        <th style="width: 20%">Porcentaje</th>
                                        <th style="width: 25%">Aumento/Descuento</th>
                                        <th style="width: 20%">Costo/Venta</th>
                                        <th class="td-actions" style="width: 10%"></th>
                                            
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:PlaceHolder ID="phListas" runat="server"></asp:PlaceHolder>
                                </tbody>
                            </table>

                        </div>

                        <div role="form" class="form-horizontal col-md-6">
                              <div class="form-group">
                                <label for="name" class="col-md-4">Nombre</label>

                                <div class="col-md-4">
                                    <asp:TextBox ID="txtNombre" runat="server" class="form-control"></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <asp:RequiredFieldValidator ControlToValidate="txtNombre" ID="RequiredFieldValidator3" runat="server" ErrorMessage="<h3>*</h3>" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="form-group">
                                <label for="name" class="col-md-4">Porcentaje</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtPorcentaje" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <asp:RequiredFieldValidator ControlToValidate="txtPorcentaje" ID="RequiredFieldValidator1" runat="server" ErrorMessage="<h3>*</h3>" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtPorcentaje" ValidationExpression="^(\d|-)?(\d|,)*\.?\d\,?\d*$" ErrorMessage="Formato Incorrecto" ForeColor="Red" Font-Bold="true" />
                                </div>
                            </div>    
                        <div class="form-group">
                            <label for="validateSelect" class="col-md-4">Aumento/Descuento</label>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ListAumentoDescuento" runat="server" class="form-control">
                                    <asp:ListItem Value="-1"> Seleccione... </asp:ListItem>
                                    <asp:ListItem Value="1"> Aumento </asp:ListItem>
                                    <asp:ListItem Value="2"> Descuento </asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-4">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="ListAumentoDescuento" InitialValue="-1" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>
                        </div>                    
                        <div class="form-group">
                            <label for="validateSelect" class="col-md-4">Costo/Venta</label>
                            <div class="col-md-4">
                                <asp:DropDownList ID="ListCostoVenta" runat="server" class="form-control">
                                    <asp:ListItem Value="-1"> Seleccione... </asp:ListItem>
                                    <asp:ListItem Value="1"> Costo </asp:ListItem>
                                    <asp:ListItem Value="2"> Venta </asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-4">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ErrorMessage="<h3>*</h3>" ControlToValidate="ListCostoVenta" InitialValue="-1" SetFocusOnError="true" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>
                        </div>                                                    
                            <div class="col-md-6" style="text-align: center">
                            <asp:Button ID="btnAgregar" runat="server" Text="Guardar" class="btn btn-success" OnClick="btnAgregar_Click"/>
                        </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

       <%-- <!-- Core Scripts - Include with every page -->
  
    <script src="../../Scripts/libs/jquery-1.9.1.min.js"></script>
    <script src="../../Scripts/libs/jquery-ui-1.10.0.custom.min.js"></script>
    <script src="../../Scripts/libs/bootstrap.min.js"></script>

    <script src="../../Scripts/plugins/msgGrowl/js/msgGrowl.js"></script>
    <script src="../../Scripts/plugins/lightbox/jquery.lightbox.min.js"></script>
    <script src="../../Scripts/plugins/msgbox/jquery.msgbox.min.js"></script>

    <script src="../../Scripts/Application.js"></script>

    <%--<script src="../../Scripts/plugins/hoverIntent/jquery.hoverIntent.minified.js"></script>--%>

    

   <%-- <script src="../../Scripts/demo/gallery.js"></script>

      <script src="../../Scripts/jquery-1.10.2.js"></script>
    <script src="../../Scripts/bootstrap.min.js"></script>--%>
    <%--<script src="../Scripts/plugins/metisMenu/jquery.metisMenu.js"></script>--%>


    <!-- Core Scripts - Include with every page -->
    <script src="../../Scripts/jquery-1.10.2.js"></script>
    <script src="../../Scripts/bootstrap.min.js"></script>
    <%--<script src="../Scripts/plugins/metisMenu/jquery.metisMenu.js"></script>--%>

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


          <script>
              //valida los campos solo numeros
              function validarNro(e) {
                  var key;
                  if (window.event) // IE
                  {
                      key = e.keyCode;
                  }
                  else if (e.which) // Netscape/Firefox/Opera
                  {
                      key = e.which;
                  }

                  if (key < 48 || key > 57) {
                      if (key == 46 || key == 8 || key == 44) // Detectar . (punto) , backspace (retroceso) y , (coma)
                      { return true; }
                      else
                      { return false; }
                  }
                  return true;
              }
</script>

</asp:Content>

