function ObtenerAlertaProveedor(idProveedor)
{
    $.ajax({
        type: "POST",
        url: 'OrdenesCompraABM.aspx/CargarAlertaProveedor',
        data: JSON.stringify(
            {
                'idProveedor': idProveedor
            }
        ),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            if (data.d !== "")
                $.msgbox("Alerta Proveedor: " + data.d.replace(new RegExp('"', 'g'), ''), { type: "info" });
        },
        error: function (e) {
            $.msgbox("No se pudo obtener la alerta del proveedor correctamente!", { type: "error" });
        }
    });
}