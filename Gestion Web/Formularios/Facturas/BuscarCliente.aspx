<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BuscarCliente.aspx.cs" Inherits="Gestion_Web.Formularios.Facturas.BuscarCliente" %>

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
    <link href="../../css/font-awesome.min.css" rel="stylesheet">

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
            <br />

            <div class="container">
                <div class="col-md-12 col-xs-12 ">
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
                                                <asp:TextBox ID="txtBuscarCliente" runat="server" class="form-control" Text=""></asp:TextBox>

                                            </div>
                                        </div>
                                        <!-- /input-group -->
                                    </td>
                                    <td style="width: 20%">
                                        <span class="input-group-btn">
                                            <asp:LinkButton ID="lbBuscarCliente" runat="server" Text="Buscar" class="btn btn-primary" OnClick="btnBuscarCliente_Click">
                                                    <i class="shortcut-icon icon-search"></i></asp:LinkButton>
                                        </span>
                                    </td>


                                </tr>
                            </table>
                        </div>
                        <!-- /widget-content -->

                    </div>
                    <%-- <div class="widget widget-table action-table">
                        <div class="panel-body">
                            <div class="table-responsive">--%>
                    <div class="widget widget-table">
                        <table class="table table-striped table-bordered">
                            <%--<table style="width: 300" class="table table-striped table-bordered table-hover" id="dataTablesBC-example">--%>
                                <thead>
                                    <tr>
                                        <th style="width: 20%">Codigo</th>
                                        <th style="width: 30%">Razon Social</th>
                                        <th style="width: 30%">Alias</th>
                                        <th class="td-actions" style="width: 20%"></th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:PlaceHolder ID="phClientes" runat="server"></asp:PlaceHolder>
                                </tbody>
                            </table>
                    </div>
            </div>
        </div>
        </div>
        


        <br />
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
    <script src="../../Scripts/plugins/dataTables/jquery.dataTables.min.js"></script>
    <script src="../../Scripts/plugins/dataTables/custom.tables.js"></script>
    <link href="../../Scripts/plugins/dataTables/dataTables.css" rel="stylesheet" />


    <%-- <script>
        $(document).ready(function () {
            $('#dataTablesBC-example').dataTable();
        });
    </script>

    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endReq);
        function endReq(sender, args) {
            $('#dataTablesBC-example').dataTable();
        }
    </script>--%>
</body>
</html>


