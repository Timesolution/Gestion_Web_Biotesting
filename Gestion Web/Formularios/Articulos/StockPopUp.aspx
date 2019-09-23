<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StockPopUp.aspx.cs" Inherits="Gestion_Web.Formularios.Articulos.StockPopUp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link href="../../css/bootstrap.min.css" rel="stylesheet" />
    <link href="./css/bootstrap-responsive.min.css" rel="stylesheet" />

    <link href="https://fonts.googleapis.com/css?family=Open+Sans:400italic,600italic,400,600" rel="stylesheet" />
    <link href="../../css/font-awesome.min.css" rel="stylesheet" />

    <link href="../../css/ui-lightness/jquery-ui-1.10.0.custom.min.css" rel="stylesheet" />
    <link href="../../css/ui-lightness/jquery-ui-1.10.0.custom.css" rel="stylesheet" />

    <link href="../../css/base-admin-3.css" rel="stylesheet" />
    <link href="../../css/base-admin-3.css" rel="stylesheet" />
    <link href="../../css/base-admin-3-responsive.css" rel="stylesheet" />

    <link href="../../css/pages/plans.css" rel="stylesheet" />
    <link href="../../css/pages/dashboard.css" rel="stylesheet" />
    <link href="../../css/custom.css" rel="stylesheet" />
    <link href="../../css/pages/signin.css" rel="stylesheet" type="text/css" />

    <link href="../../Scripts/plugins/msgGrowl/css/msgGrowl.css" rel="stylesheet" />
    <link href="../../Scripts/plugins/lightbox/themes/evolution-dark/jquery.lightbox.css" rel="stylesheet" />
    <link href="../../Scripts/plugins/msgbox/jquery.msgbox.css" rel="stylesheet" />
</head>
<body>

    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>



            <br />
            <br />



            <div class="container">
                <div class="col-md-12 col-xs-12 ">
                    <div class="widget widget-table">

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
                                        <asp:Button ID="btn_Agregar" runat="server" Text="Guardar" class="btn btn-success" OnClick="btn_Agregar_Click" />
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <br />
            <div class="row">
            </div>

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
                else { return false; }
            }
            return true;
        }
    </script>

</body>
</html>

