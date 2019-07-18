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