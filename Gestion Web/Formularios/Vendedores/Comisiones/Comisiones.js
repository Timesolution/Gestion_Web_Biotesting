function ObtenerFechaActual()
{
    var dt = new Date($.now());
    var fechaActual = dt.getDate() + "/" + "0" + (dt.getMonth() + 1) + "/" + dt.getFullYear();
    return fechaActual;
}