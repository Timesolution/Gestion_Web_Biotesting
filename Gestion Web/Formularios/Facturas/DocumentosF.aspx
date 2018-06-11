<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DocumentosF.aspx.cs" Inherits="Gestion_Web.Formularios.Facturas.DocumentosF" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main">

        <div class="container">


            
                <div class="col-md-12 col-xs-12 hidden-print">
                    <div class="widget stacked">

                        <div class="widget-header">
                            <i class="icon-wrench"></i>
                            <h3>Documentos impagos

                            </h3>
                        </div>
                        <div class="widget-content">
                            <div class="col-md-12 col-xs-12">
                                <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Always" runat="server">
                                    <ContentTemplate>
                                        <a class="btn btn-info" style="display: none" data-toggle="modal" id="abreDialog" href="#modalFacturaDetalle">Agregar Tipo Cliente</a>
                                        <%--<asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />--%>
                                        <%--<a class="btn btn-info" data-toggle="modal" id="abreDialog2" onclick="abrirdialog()" >Agregar Tipo Cliente2</a>--%>



                                        <table class="table table-striped table-bordered">
                                            <thead>
                                                <tr>
                                                    <th>Numero</th>
                                                    <th>Saldo</th>
                                                    <th>Imputar</th>

                                                </tr>

                                            </thead>
                                            <tbody>
                                                <asp:PlaceHolder ID="phDocumentos" runat="server"></asp:PlaceHolder>
                                                <tr>
                                                    <th>Total</th>
                                                    <th>
                                                        <asp:Label ID="labelSaldo" runat="server" class="form-control" disabled="" Text=""></asp:Label>
                                                    </th>
                                                    <th>
                                                        <asp:Label ID="labelTotal" runat="server" class="form-control" disabled="" Text="0"></asp:Label>
                                                    </th>
                                                </tr>
                                            </tbody>
                                        </table>

<%--                                        <div class="col-md-8">
                                            <asp:TextBox ID="txtPrueba" runat="server" class="form-control"  OnTextChanged="txtPrueba_TextChanged" AutoPostBack="True"></asp:TextBox>
                                        </div>--%>

                                        </div>


                                    </ContentTemplate>
                                    <Triggers>
                                    </Triggers>
                                </asp:UpdatePanel>
                                <%--<div class="col-md-6">
                                </div>--%>
                                 <div class="col-md-8">
                                    <asp:Button ID="Button1" runat="server" Text="Seleccionar" class="btn btn-success" OnClick="btnSeleccionar_Click" />
                                </div>

                            </div>

                        </div>

                    </div>
                </div>


            

            
                             
           
        
            </div>
        <%--Fin modalGrupo--%>
    </div>

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
