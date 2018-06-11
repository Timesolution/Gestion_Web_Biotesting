function completar8Ceros(txtBox, valor)
{
    //cadcero='';
    //for(i=0;i<(8-valor.length);i++)
    //{
    //    cadcero+='0';
    //}
    //txtBox.value = cadcero + valor;
    txtBox.value = pad(valor, 8);
}

function completar4Ceros(txtBox, valor) {
    
    
    txtBox.value = pad(valor, 4);
}

function pad(str, max) {
    str = str.toString();
    return str.length < max ? pad("0" + str, max) : str;
}