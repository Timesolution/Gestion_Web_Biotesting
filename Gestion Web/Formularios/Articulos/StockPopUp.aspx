<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockPopUp.aspx.cs" Inherits="Gestion_Web.Formularios.Articulos.StockPopUp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
<%--    <style type="text/css">
        .style1
        {
            width: 69px;
        }
        .style2
        {
            width: 155px;
        }
        .style3
        {
            width: 303px;
        }
        .style4
        {
            width: 68px;
        }
    </style>--%>


    <link href="../../css/bootstrap.min.css" rel="stylesheet"/>
    <link href="./css/bootstrap-responsive.min.css" rel="stylesheet"/>
    
    <link href="http://fonts.googleapis.com/css?family=Open+Sans:400italic,600italic,400,600" rel="stylesheet"/>
    <link href="../../css/font-awesome.min.css" rel="stylesheet">
    
    <link href="../../css/ui-lightness/jquery-ui-1.10.0.custom.min.css" rel="stylesheet"/>
    <link href="../../css/ui-lightness/jquery-ui-1.10.0.custom.css" rel="stylesheet"/>
    
    <link href="../../css/base-admin-3.css" rel="stylesheet"/>
    <link href="../../css/base-admin-3.css" rel="stylesheet"/>
    <link href="../../css/base-admin-3-responsive.css" rel="stylesheet"/>
    
    <link href="../../css/pages/plans.css" rel="stylesheet"/> 
    <link href="../../css/pages/dashboard.css" rel="stylesheet"/> 
    <link href="../../css/custom.css" rel="stylesheet"/>
    <link href="../../css/pages/signin.css" rel="stylesheet" type="text/css"/>

    <link href="../../Scripts/plugins/msgGrowl/css/msgGrowl.css" rel="stylesheet"/>
	<link href="../../Scripts/plugins/lightbox/themes/evolution-dark/jquery.lightbox.css" rel="stylesheet"/>
	<link href="../../Scripts/plugins/msgbox/jquery.msgbox.css" rel="stylesheet"/>	
</head>
<body>

    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>

    <%--<asp:UpdatePanel ID="CatalogPanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>--%>
                                    <%--<table width="300" class="table table-striped table-bordered">
                                        <thead>
                                            <tr>
                                                <th>Descripcion</th>
                                                <th></th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td style="width: 90%">
                                                
                                                    <asp:TextBox ID="txtDescripcion" runat="server" class="form-control" 
                                                        Width="536px" ></asp:TextBox>
                                                </td>
                                                <td style="width: 10%">
                                                    

                                                    <%--<asp:Button ID="btnAgregarArticuloASP" class="btn btn-primary" runat="server" 
                                                        Text="Buscar" Visible="true" onclick="btnAgregarArticuloASP_Click"  />
                                                    <asp:ImageButton ID="ImageButton1" runat="server" 
                                                        onclick="btnAgregarArticuloASP_Click" Height="28px" 
                                                        ImageUrl="~/images/Search-48.png" Width="33px" />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>--%>

                                    <br />
                                    <br />


<%--                                    <table width="300" style="border: thin solid #000000; width: 607px;">
                                        <thead style="border-style: solid; border-width: thin">
                                            <tr style="border: thin solid #000000">
                                                <th style="border: thin solid #000000" class="style1">Sucursal</th>
                                                
                                                <th style="border: thin solid #000000" class="style3">Descripcion</th>
                                                <th style="border: thin solid #000000" class="style2">Stock</th>
                                                
                                                <th style="border: thin solid #000000" class="style4"></th>
                                            </tr>
                                        </thead>
                                        <tbody style="border: thin solid #000000">


                                            <asp:PlaceHolder ID="phStock" runat="server"></asp:PlaceHolder>
                                        </tbody >
                                    </table>--%>

<%--            <div class="container">
            <div class="col-md-12 col-xs-12 ">
                <div class="widget widget-table">

                    <div class="widget-header">
                        <i class="icon-wrench"></i>
                        <h3>Actualizar Stock

                        </h3>
                    </div>
                    <div class="widget-content">--%>

                                <%--<div class="modal-dialog">
                <div class="modal-content">

                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                        <h4 class="modal-title">Recibo</h4>
                    </div>
                    <div class="modal-body">
                        <div role="form" class="form-horizontal col-md-12">
                            <div class="form-group">
                                <label class="col-md-4">Fecha</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtFechaCobro" runat="server" class="form-control"></asp:TextBox>
                                </div>

                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Numero</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtNumeroCobro" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Total Imputado</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtTotalImputadoCobro" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Total Cobro</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtTotalCobro" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <%--<div class="form-group">
                                <label class="col-md-4">Recibo</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtRecibo" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">

                        <asp:Button ID="btnAgregarPago" runat="server" Text="Finalizar" class="btn btn-success" />
                    </div>

                </div>
            </div>--%>

                                          <%--<table class="table table-striped table-bordered">
                                        <thead>
                                            <tr>
                                                <th>Codigo</th>
                                                <th>Sucursal</th>
                                                <th>Descripcion</th>
                                                <th>Stock</th>
                                                <th></th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <asp:PlaceHolder ID="phStock" runat="server"></asp:PlaceHolder>
                                        </tbody>
                                    </table>--%>


                

<%--                    </div>
                </div>
            </div>
        </div>--%>

        <div class="container">
            <div class="col-md-12 col-xs-12 ">
                <div class="widget widget-table">

<%--                    <div class="widget-header">
                        <i class="icon-wrench"></i>
                        <h3>Actualizar Stock

                        </h3>
                    </div>--%>
                    <div class="widget-content">
                        <div role="form" class="form-horizontal col-md-12">
                            <div class="form-group">
                                <label class="col-md-4">Codigo</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtCodigo" runat="server" class="form-control" Disabled=""></asp:TextBox>
                                </div>

                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Sucursal</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtSucursal" runat="server" class="form-control" Disabled=""></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Articulo</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtArticulo" runat="server" class="form-control" Disabled=""></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Stock Actual</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtStockActual" runat="server" class="form-control" Disabled=""></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-md-4">Agregar</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtAgregarStock" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)" Text="0"></asp:TextBox>
                                </div>
                            </div>
                             <div class="form-group">
                                <label class="col-md-4">Comentario</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtComentarios" runat="server" class="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <div class="col-md-4">
                                    <asp:Button ID="btn_Agregar" runat="server" Text="Guardar" class="btn btn-success" OnClick="btn_Agregar_Click"/>
                                </div>
                            </div>
                            <%--<div class="form-group">
                                <label class="col-md-4">Recibo</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtRecibo" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>--%>
                        </div>
                    </div>
                </div>
            </div>
        </div>

                                    <br />
                                    <div class="row">
                                    
                                    
                                        </div>
                                    
                                    
                                <%--</ContentTemplate>
                                <Triggers>
                      
                                </Triggers>
                            </asp:UpdatePanel>--%>
    </div>
    </form>

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
                  if (key == 46 || key == 8 || key == 44 || key == 45) // Detectar . (punto) , backspace (retroceso) y , (coma)
                  { return true; }
                  else
                  { return false; }
              }
              return true;
          }
</script>

</body>
</html>

