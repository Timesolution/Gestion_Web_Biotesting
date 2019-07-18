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

function InvertirDiaPorMes(fecha)
{
    var fechaSeparada = fecha.split("/");

    var fechaNueva = new Date([fechaSeparada[1], fechaSeparada[0], fechaSeparada[2]].join('/'));

    return fechaNueva;
}