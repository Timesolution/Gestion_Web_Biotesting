<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CamposLotesTarjetas.ascx.cs" Inherits="Gestion_Web.Controles.CamposLotesTarjetas" %>
<div class="form-group">
    <label id="lblCampo" runat="server" class="col-md-3">Campo </label>
    <div class="col-md-2">
        <div class="input-group">
            <span class="input-group-addon">Nº</span>
            <asp:TextBox ID="txtCupones" runat="server" class="form-control" TextMode="Number"></asp:TextBox>
        </div>
    </div>
    <div class="col-md-2">
        <div class="input-group">
            <span class="input-group-addon">$</span>
            <asp:TextBox ID="txtImporte" runat="server" class="form-control" onkeypress="javascript:return validarNro(event)"></asp:TextBox>
        </div>
    </div>
</div>
