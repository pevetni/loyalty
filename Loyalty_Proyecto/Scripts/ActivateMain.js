$.datepicker.regional['es'] = {
    closeText: 'Cerrar',
    prevText: '< Ant',
    nextText: 'Sig >',
    currentText: 'Hoy',
    monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
    monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
    dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
    dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Juv', 'Vie', 'Sáb'],
    dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sá'],
    weekHeader: 'Sm',
    dateFormat: 'dd/mm/yy',
    firstDay: 1,
    isRTL: false,
    showMonthAfterYear: false,
    yearSuffix: ''
};

$.datepicker.setDefaults($.datepicker.regional['es']);

$(function () {
    $("#fecha").datepicker({ changeYear: true, yearRange: "-100:+0", changeMonth: true, });
    getProvincias();
    $("input[type='radio']").checkboxradio();
    $('#legal').hide();

});


function getProvincias() {
    alert('provincia');
    $.ajax({
        type: "POST", url: "ACTIVATE.aspx/GetProvincias", dataType: "json", contentType: "application/json", success: function (res) {
            $("#provincia").html('');
            var options = '';
            options += '<option disabled selected>Seleccione una provincia</option>';
            $.each(res.d, function (data, value) {
                options += "<option value='" + value.Id + "'>" + value.Nombre + "</option>";
            });

            $("#listProvincias").append(options);
        }

    });
}

function getCiudades() {
    $.ajax({
        type: "POST", url: "ACTIVATE.aspx/GetCiudades", dataType: "json", data: '{Provincia:' + $("#listProvincias").val() + '}', contentType: "application/json", success: function (res) {
            try {
                $("#listCiudades").html('');
                var options = '';
                options += '<option disabled selected>Seleccione una ciudad</option>';
                $("#listCiudades").append(options);
                $.each(res.d, function (data, value) {
                    $("#listCiudades").append($("<option></option>").text(value.Nombre).val(value.Id));
                });
            }
            catch (exception) {
                alert(exception.message);
            }
        }

    });
};

function getGrupo() {
    $.ajax({
        type: "POST", url: "ACTIVATE.aspx/GetGrupos", dataType: "json", data: '{Codigo:' + $("#txtGrupo").val() + '}', contentType: "application/json", success: function (res) {
            try {
                $.each(res.d, function (data, value) {
                    if (value.Codigo == 'NO')
                    {
                        $("#txtGrupo").attr('class', 'txtboxRequired');
                    }
                    else 
                    {
                        $("#txtGrupo").attr('class', 'txtbox');
                    }
                });
            }
            catch (exception) {
                alert(exception.message);
            }
        }

    });
};

function getTarjeta() {
    $.ajax({
        type: "POST", url: "ACTIVATE.aspx/GetTarjetas", dataType: "json", data: '{Codigo:' + $("#txtTarjeta").val() + '}', contentType: "application/json", success: function (res) {
            try {
                $.each(res.d, function (data, value) {
                    if (value.Codigo == 'NO') {
                        $("#txtTarjeta").attr('class', 'txtboxRequired');
                    }
                    else {
                        $("#txtTarjeta").attr('class', 'txtbox');
                    }
                });
            }
            catch (exception) {
                alert(exception.message);
            }
        }

    });
};

function numbersonly(e) {
    var unicode = e.charCode ? e.charCode : e.keyCode
    if (unicode != 8 && unicode != 44) {
        if ((unicode < 48 && unicode != 13 && unicode != 9) || unicode > 57) //if not a number
        { return false } //disable key press    
    }
}

function validar(e) {
    var result = true;
    if ($("#txtDNI").val() == "" || $("#txtDNI").val().length < 7) {
        $("#txtDNI").attr('class', 'txtboxRequired');
        $("#txtDNI").focus();
        result = false;
    }
    else {
        $("#txtDNI").attr('class', 'txtbox');
    }

    if ($("#txtApellido").val() == "") {
        $("#txtApellido").attr('class', 'txtboxRequired');
        $("#txtApellido").focus();
        result = false;
    }
    else {
        $("#txtApellido").attr('class', 'txtbox');
    }

    if ($("#txtNombre").val() == "") {
        $("#txtNombre").attr('class', 'txtboxRequired');
        $("#txtNombre").focus();
        result = false;
    }
    else {
        $("#txtNombre").attr('class', 'txtbox');
    }

        
    if ($("#fecha").val() == "") {
        $("#fecha").attr('class', 'txtboxRequired');
        result = false;
    }
    else {
        $("#fecha").attr('class', 'txtbox');

        if (validate_fecha($("#fecha").val()) != true) {
            $("#fecha").attr('class', 'txtboxRequired');
            result = false;
        }
        else {
            if (parseInt(edad($("#fecha").val())) < 18) {
                $("#fecha").attr('class', 'txtboxRequired');
                result = false;
            }
        }
    }


    if ($("#txtCalle").val() == "") {
        $("#txtCalle").attr('class', 'txtboxRequired');
        $("#txtCalle").focus();
        result = false;
    }
    else {
        $("#txtCalle").attr('class', 'txtbox');
    }

    if ($("#txtEmail").val() == "") {
        $("#txtEmail").attr('class', 'txtboxRequired');
        $("#txtEmail").focus();
        result = false;
    }
    else {
        $("#txtEmail").attr('class', 'txtbox');
    }

    if ($("#txtGrupo").val() == "") {
        $("#txtGrupo").attr('class', 'txtboxRequired');
        $("#txtGrupo").focus();
        result = false;
    }
    else {
        $("#txtGrupo").attr('class', 'txtbox');
    }

    getGrupo();

    if ($("#txtTarjeta").val() == "" || $("#txtTarjeta").val().length != 13) {
        $("#txtTarjeta").attr('class', 'txtboxRequired');
        $("#txtTarjeta").attr('title', 'El dato requerido debe tener 13 caracteres');
        $("#txtTarjeta").focus();
        result = false;
    }
    else {
        $("#txtTarjeta").attr('class', 'txtbox');
    }

    getTarjeta();

    var selectedValue = $("input[name='radio-1']:checked").val();
    if (selectedValue) {
        $("#optF").attr('class', 'lbldata');
        $("#optM").attr('class', 'lbldata');
    }
    else {
        $("#optF").attr('class', 'lblrequired');
        $("#optM").attr('class', 'lblrequired');
        result = false;
    }

    selectedValue = $("input[name='chkAccept']:checked").val();
    if (selectedValue) {
        $("#legal").attr('class', 'titulo_boton');
    }
    else {
        $("#legal").attr('class', 'ltitulo_boton_required');
        result = false;
    }


    responseToSave();

    return result;
}

function validate_fecha(fecha) {
    var patron = new RegExp("^([0-9]{2})([/])([0-9]{1,2})([/])([0-9]{1,4})$");
    if (fecha.search(patron) == 0) {
        var values = fecha.split("/");
            
        if (values[1] < 1 || values[1] > 12 || values[0] < 1 || values[0] > 31 || values[2] < 1900)
        {
            return false;
        };

        if ((values[1] == 4 || values[1] == 6 || values[1] == 9 || values[1] == 11) && values[0] > 30)
        {
            return false;
        }

        if ((values[1] == 1 || values[1] == 3 || values[1] == 5 || values[1] == 7 || values[1] == 8 || values[1] == 10 || values[1] == 12) && values[0] > 31)
        {
            return false;
        }

        var isLeap = values[2] % 4;
        if ((values[1] == 2 && isLeap == 0) && values[0] > 29)
        {
            return false;
        }
        if ((values[1] == 2 && isLeap != 0) && values[0] > 28)
        {
            return false;
        }
        return true;
    }
    return false;
}

edad = function (f1)
{

    var fecha_hoy = new Date();
    var ahora_ano = fecha_hoy.getYear();
    var ahora_mes = fecha_hoy.getMonth() + 1;
    var ahora_dia = fecha_hoy.getDate();

    var aFecha1 = f1.split('/');
    var fFecha1 = Date.UTC(aFecha1[2], aFecha1[1] - 1, aFecha1[0]);
    var dia = aFecha1[0];
    var mes = aFecha1[1];
    var ano = aFecha1[2];

    // realizamos el calculo
    var edad = (ahora_ano + 1900) - ano;
    if (ahora_mes < mes) {
        edad--;
    }
    if ((mes == ahora_mes) && (ahora_dia < dia)) {
        edad--;
    }
    if (edad > 1900) {
        edad -= 1900;
    }
    return edad;
}

restaFechas = function (f1) {
    fec = new Date;
    dia = fec.getDate();
    if (dia < 10) dia = '0' + dia;
    mes = fec.getMonth();
    if (mes < 10) mes = '0' + mes;
    anio = fec.getFullYear();
    fecha = dia + '/' + mes + '/' + anio;

    var aFecha1 = f1.split('/');
    var aFecha2 = fecha.split('/');
    var fFecha1 = Date.UTC(aFecha1[2], aFecha1[1] - 1, aFecha1[0]);
    var fFecha2 = Date.UTC(aFecha2[2], aFecha2[1] - 1, aFecha2[0]);
    var dif = fFecha2 - fFecha1;
    var dias = Math.floor(dif / (1000 * 60 * 60 * 24));
    return dias;
}

function responseToSave() {
    $("#hfDNI").val($("#txtDNI").val());
    $("#hfApellido").val($("#txtApellido").val());
    $("#hfNombre").val($("#txtNombre").val());
    $("#hfGenero").val($("input[name='radio-1']:checked").val());
    $("#hfFechaNacimiento").val($("#fecha").val());
    $("#hfTelefono").val($("#txtTelefono").val());
    $("#hfCelular").val($("#txtCelular").val());
    $("#hfDireccion").val($("#txtCalle").val());
    $("#hfPostal").val($("#txtPostal").val());
    $("#hfProvincia").val($("#listProvincias option:selected").val());
    $("#hfProvinciaNombre").val($("#listProvincias option:selected").text());
    $("#hfCiudad").val($("#listCiudades option:selected").val());
    $("#hfCiudadNombre").val($("#listCiudades option:selected").text());
    $("#hfEmail").val($("#txtEmail").val());
    $("#hfGrupo").val($("#txtGrupo").val());
    $("#hfTarjeta").val($("#txtTarjeta").val());

}

function muestra_oculta_msg(id, ver) {
    if (document.getElementById) { //se obtiene el id
        var el = document.getElementById(id); //se define la variable "el" igual a nuestro div
        if (ver == 'si') {
            el.style.display = 'block';
        }
        else {
            el.style.display = 'none';
        }
    }
}

function muestra_oculta(id) {
    if (document.getElementById) { //se obtiene el id
        var el = document.getElementById(id); //se define la variable "el" igual a nuestro div
        el.style.display = (el.style.display == 'none') ? 'block' : 'none'; //damos un atributo display:none que oculta el div
    };


}

function admin_mensaje() {
    if (document.getElementById('hfOperacion').value == 'nuevo') {
        document.getElementById('maincontent').style.display = 'block';
        document.getElementById('mainok').style.display = 'none';
        document.getElementById('mainerror').style.display = 'none';
        document.getElementById('txtDNI').focus();
    };
    if (document.getElementById('hfOperacion').value == 'ok') {
        document.getElementById('maincontent').style.display = 'none';
        document.getElementById('mainok').style.display = 'block';
        document.getElementById('mainerror').style.display = 'none';
    };
    if (document.getElementById('hfOperacion').value == 'error') {
        document.getElementById('maincontent').style.display = 'none';
        document.getElementById('mainok').style.display = 'none';
        document.getElementById('mainerror').style.display = 'block';
    };
}

function tabE(obj, e) {
    var e = (typeof event != 'undefined') ? window.event : e;// IE : Moz 
    if (e.keyCode == 13) {
        var ele = document.forms[0].elements;
        for (var i = 0; i < ele.length; i++) {
            var q = (i == ele.length - 1) ? 0 : i + 1;// if last element : if any other 
            if (obj == ele[i]) { ele[q].focus(); break }
        }
        return false;
    }
}

    

