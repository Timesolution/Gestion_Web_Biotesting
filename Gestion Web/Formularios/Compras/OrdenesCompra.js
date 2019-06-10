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
                $.msgbox("Alerta Proveedor: " + data.d.replace(new RegExp('"', 'g'), ''), { type: "alert" });
        },
        error: function (e) {
            $.msgbox("No se pudo obtener la alerta del proveedor correctamente!", { type: "error" });
        }
    });
}
function CargarDatosProveedor(idProveedor) {
    $.ajax({
        type: "POST",
        url: 'OrdenesCompraABM.aspx/CargarProveedor_OC',
        data: JSON.stringify(
            {
                'idProveedor': idProveedor
            }
        ),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: OnSuccessCargarDatosProveedor,
        error: function (e) {
            $.msgbox("No se pudieron cargar los datos del proveedor correctamente!", { type: "error" });
            LimpiarCamposDatosProveedor();
        }
    });
}
function CargarArticulosProveedor(idProveedor, idSucursal) {
    $.ajax({
        type: "POST",
        url: 'OrdenesCompraABM.aspx/ObtenerArticulosProveedor',
        data: '{idProveedor: "' + idProveedor + '", idSucursal: "' + idSucursal + '"}',
        data: JSON.stringify(
            {
                'idProveedor': idProveedor,
                'idSucursal': idSucursal
            }
        ),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: OnSuccessCargarArticulosProveedor,
        error: function (e) {
            $.msgbox("No se pudieron cargar los articulos del proveedor correctamente!", { type: "error" });
        }
    });
}