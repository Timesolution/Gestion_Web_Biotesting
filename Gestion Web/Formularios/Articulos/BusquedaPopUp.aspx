<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BusquedaPopUp.aspx.cs" Inherits="Gestion_Web.Formularios.Articulos.BusquedaPopUp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
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
    </style>
</head>
<body>

    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>

    <%--<asp:UpdatePanel ID="CatalogPanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>--%>
 <%--                                   <table width="300" class="table table-striped table-bordered">
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

                                    <table  width="300" style="border: thin solid #000000; width: 607px;">
                                        <thead style="border-style: solid; border-width: thin">
                                            <tr style="border: thin solid #000000">
                                                <th style="border: thin solid #000000" class="style1">Codigo</th>
                                                
                                                <th style="border: thin solid #000000" class="style3">Sucursal</th>
                                                <th style="border: thin solid #000000" class="style2">Descripcion</th>
                                                
                                                <th style="border: thin solid #000000" class="style4"></th>
                                            </tr>
                                        </thead>
                                        <tbody style="border: thin solid #000000">


                                            <asp:PlaceHolder ID="phArticulos" runat="server"></asp:PlaceHolder>
                                        </tbody >
                                    </table>

                                    <br />
                                    <div class="row">
                                    
                                    
                                        </div>
                                    
                                    
                                <%--</ContentTemplate>
                                <Triggers>
                      
                                </Triggers>
                            </asp:UpdatePanel>--%>
    </div>
    </form>


</body>
</html>
