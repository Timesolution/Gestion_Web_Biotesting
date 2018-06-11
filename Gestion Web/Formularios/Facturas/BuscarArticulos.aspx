<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BuscarArticulos.aspx.cs" Inherits="Gestion_Web.Formularios.Facturas.BuscarArticulos" %>

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


    <link href="../../css/bootstrap.min.css" rel="stylesheet" />
    <link href="./css/bootstrap-responsive.min.css" rel="stylesheet" />

    <link href="http://fonts.googleapis.com/css?family=Open+Sans:400italic,600italic,400,600" rel="stylesheet" />
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


            <div class="container">
                <div class="navbar navbar-inverse navbar-fixed-top hidden-print">
                    <div class="container">
                        <div class="navbar-header">
                            <asp:LinkButton runat="server" ID="btnCerrar" class="navbar-toggle" OnClick="btnCerrar_Click" Text="<span class='icon-remove icon-white'></span>" />


                            <asp:Label CssClass="navbar-brand" runat="server" ID="LabelInicio" Text="Busqueda"></asp:Label>
                        </div>

                    </div>
                </div>

                <br />
                <br />

                <div class="col-md-12 col-xs-12 ">

                    <div class="widget stacked">

                        <div class="widget-header">
                            <i class="icon-wrench"></i>
                            <h3>Herramientas</h3>
                        </div>
                        <!-- /widget-header -->

                        <div class="widget-content">
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 80%">
                                        <div class="col-md-12">
                                            <div class="imput-group">
                                                <asp:TextBox ID="txtBuscarArticulos" runat="server" class="form-control" Text=""></asp:TextBox>

                                            </div>
                                        </div>
                                        <!-- /input-group -->
                                    </td>
                                    <td style="width: 20%">
                                        <span class="input-group-btn">
                                            <asp:LinkButton ID="lbBuscarArticulos" runat="server" Text="Buscar" class="btn btn-primary" OnClick="btnBuscarArticulos_Click">
                                                    <i class="shortcut-icon icon-search"></i></asp:LinkButton>
                                            <%--<button class="btn btn-primary" type="button">Buscar!</button>--%>
                                        </span>
                                    </td>


                                </tr>
                            </table>
                        </div>
                        <!-- /widget-content -->

                    </div>
                    <div class="widget widget-table">
                        <table class="table table-striped table-bordered">
                            <thead>
                                <tr>
                                    <th style="width: 20%; text-align: left">Codigo</th>
                                    <th style="width: 40%; text-align: left">Descripcion</th>
                                    <th style="width: 15%; text-align: left">Moneda</th>
                                    <th style="width: 15%; text-align: left">P.Venta</th>
                                    <th class="td-actions" style="width: 10%"></th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:PlaceHolder ID="phArticulos" runat="server"></asp:PlaceHolder>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>



        <%--                    </div>
                </div>
            </div>
        </div>--%>

        <%--        <div class="container">
            <div class="col-md-12 col-xs-12 ">
                <div class="widget widget-table">--%>

        <%--                    <div class="widget-header">
                        <i class="icon-wrench"></i>
                        <h3>Actualizar Stock

                        </h3>
                    </div>--%>
        <%--<div class="widget-content">
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
                                <div class="col-md-4">
                                    <asp:Button ID="btn_Agregar" runat="server" Text="Guardar" class="btn btn-success" OnClick="btn_Agregar_Click"/>
                                </div>
                            </div>--%>
        <%--<div class="form-group">
                                <label class="col-md-4">Recibo</label>
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtRecibo" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>--%>
        <%--                        </div>
                    </div>
                </div>
            </div>
        </div>--%>

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

    <!-- Page-Level Plugin Scripts - Tables -->
    <script src="../../Scripts/plugins/dataTables/jquery.dataTables.js"></script>
    <script src="../../Scripts/plugins/dataTables/dataTables.bootstrap.js"></script>

    <script>
        $(document).ready(function () {
            $('#dataTables-example').dataTable();
        });
    </script>

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endReq);
        function endReq(sender, args) {
            $('#dataTables-example').dataTable();
        }
    </script>

</body>
</html>


