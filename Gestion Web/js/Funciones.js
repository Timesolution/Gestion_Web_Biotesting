function obtenerFechaActual_ddMMyyyyEnString() {
	var d = new Date();

	var month = d.getMonth() + 1;
	var day = d.getDate();
	var year = d.getFullYear();

	if (month.toString().length < 2) month = '0' + month;
	if (day.toString().length < 2) day = '0' + day;

	return [day, month, year].join('/');
}

function formatDateTo_MMddyyyy(date) {
	var d = new Date(date),
		month = '' + d.getDate(),
		day = '' + (d.getMonth() + 1),
		year = d.getFullYear();

	if (month.length < 2) month = '0' + month;
	if (day.length < 2) day = '0' + day;

	return [month, day, year].join('/');
}

function formatDateTo_ddMMyyyy(date) {
	var d = new Date(date),
		month = '' + (d.getMonth() + 1),
		day = '' + d.getDate(),
		year = d.getFullYear();

	if (month.length < 2) month = '0' + month;
	if (day.length < 2) day = '0' + day;

	return [day, month, year].join('/');
}

function ObtenerFechaActual() {
    var dt = new Date($.now());
    var fechaActual = ObtenerFechaDia() + "/" + ObtenerFechaMes() + "/" + dt.getFullYear();
    return fechaActual;
}

function ObtenerFechaMes() {
    var dt = new Date($.now());

    var mes = dt.getMonth() + 1;

    var fechaActual = null;

    if (mes.toString().length < 2)
        fechaActual = "0" + (dt.getMonth() + 1);
    else
        fechaActual = mes;

    return fechaActual;
}

function ObtenerFechaDia() {
    var dt = new Date($.now());

    var dia = dt.getDate();

    var fechaActual = null;

    if (dia.toString().length < 2)
        fechaActual = "0" + (dt.getDate());
    else
        fechaActual = dia;

    return fechaActual;
}

function InvertirDiaPorMes(fecha) {
    var fechaSeparada = fecha.split("/");

    var fechaNueva = new Date([fechaSeparada[1], fechaSeparada[0], fechaSeparada[2]].join('/'));

    return fechaNueva;
}