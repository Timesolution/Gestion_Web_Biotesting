function ObtenerFechaActual()
{
    var dt = new Date($.now());
    var fechaActual = ObtenerFechaDia() + "/" + ObtenerFechaMes() + "/" + dt.getFullYear();
    return fechaActual;
}

function ObtenerFechaMes()
{
    var dt = new Date($.now());

    var mes = dt.getMonth() + 1;

    var fechaActual = null;

    if (mes.toString().length < 2)
        fechaActual = "0" + (dt.getMonth() + 1);        
    else
        fechaActual = mes;

    return fechaActual;
}

function ObtenerFechaDia()
{
    var dt = new Date($.now());

    var dia = dt.getDate();

    var fechaActual = null;

    if (dia.toString().length < 2)
        fechaActual = "0" + (dt.getDate());
    else
        fechaActual = dia;

    return fechaActual;
}

function ComprobarFechaCorrecta(fecha)
{
    return fecha instanceof Date && !isNaN(fecha);
}

function obtenerFechaActual_ddMMyyyyEnString()
{
    var d = new Date();

    var month = d.getMonth() + 1;
    var day = d.getDate();
    var year = d.getFullYear();

    if (month.toString().length < 2) month = '0' + month;
    if (day.toString().length < 2) day = '0' + day;

    return [day, month, year].join('/');
}