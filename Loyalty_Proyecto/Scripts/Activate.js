$(document).ready(function () {

    getProvincias();

    $('#contact_form').bootstrapValidator({
        // To use feedback icons, ensure that you use Bootstrap v3.1.0 or later
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            documento: {
                validators: {
                    stringLength: {
                        min: 7,
                    },
                    notEmpty: {
                        message: 'Por favor, ingrese número de documento'
                    }
                }
            },
            apellido: {
                validators: {
                    stringLength: {
                        min: 2,
                    },
                    notEmpty: {
                        message: 'Por favor, ingrese su apellido'
                    }
                }
            },
            nombre: {
                validators: {
                    stringLength: {
                        min: 2,
                    },
                    notEmpty: {
                        message: 'Por favor, ingrese su nombre'
                    }
                }
            },
            fecha: {
                validators: {
                    stringLength: {
                        min: 10,
                    },
                    notEmpty: {
                        message: 'Por favor, ingrese fecha de nacimiento'
                    }
                }
            },

            direccion: {
                validators: {
                    stringLength: {
                        min: 8,
                    },
                    notEmpty: {
                        message: 'Por favor, ingrese su dirección'
                    }
                }
            },
            provincia: {
                validators: {
                    notEmpty: {
                        message: 'Por favor, seleccione su provincia'
                    }
                }
            },
            ciudad: {
                validators: {
                    notEmpty: {
                        message: 'Por favor, seleccione su ciudad'
                    }
                }
            },
            postal: {
                validators: {
                    notEmpty: {
                        message: 'Por favor, ingrese código postal'
                    }
                }
            },


            email: {
                validators: {
                    notEmpty: {
                        message: 'Por favor, ingrese su dirección de correo eléctrónico'
                    },
                    emailAddress: {
                        message: 'Por favor, ingrese su dirección de correo eléctrónico'
                    }
                }
            },
            telefono: {
                validators: {
                    notEmpty: {
                        message: 'Por favor, ingrese número de teléfono'
                    },
                    telefono: {
                        country: 'AR',
                        message: 'Por favor, ingrese código de área'
                    }
                }
            },
            celular: {
                validators: {
                    notEmpty: {
                        message: 'Por favor, ingrese número de teléfono'
                    },
                    celular: {
                        country: 'AR',
                        message: 'Por favor, ingrese código de área'
                    }
                }
            },

            postal: {
                validators: {
                    notEmpty: {
                        message: 'Por favor, ingrese código postal'
                    }
                }
            },

            grupo: {
                validators: {
                    stringLength: {
                        min: 4,
                        max: 4,
                        message: 'El valor debe contener 4 números'
                    },
                    notEmpty: {
                        message: 'Por favor, ingrese número del grupo'
                    }
                }
            },

            tarjeta: {
                validators: {
                    stringLength: {
                        min: 13,
                        max: 13,
                        message: 'El valor debe contener 13 números'
                    },
                    notEmpty: {
                        message: 'Por favor, ingrese número de tarjeta'
                    }
                }
            },

            acepta: {
                validators: {
                    notEmpty: {
                        message: 'Debe aceptar los términos y condiciones'
                    }
                }
            },




        }
    })
        .on('success.form.bv', function (e) {
            $('#success_message').slideDown({ opacity: "show" }, "slow") // Do something ...
            $('#contact_form').data('bootstrapValidator').resetForm();

            // Prevent form submission
            e.preventDefault();

            // Get the form instance
            var $form = $(e.target);

            // Get the BootstrapValidator instance
            var bv = $form.data('bootstrapValidator');

            // Use Ajax to submit form data
            $.post($form.attr('action'), $form.serialize(), function (result) {
                console.log(result);
            }, 'json');
        });
});

function getProvincias() {
    $.ajax({
        type: "POST", url: "ACTIVATE.aspx/GetProvincias", dataType: "json", contentType: "application/json", success: function (res) {
            $("#provincia").html('');
            var options = '';
            options += '<option disabled selected>Seleccione una provincia</option>';
            $.each(res.d, function (data, value) {
                options += "<option value='" + value.Id + "'>" + value.Nombre + "</option>";
            });

            $("#provincia").append(options);
        }

    });
};

function getCiudades() {
    $.ajax({
        type: "POST", url: "ACTIVATE.aspx/GetCiudades", dataType: "json", data: '{Provincia:' + $("#provincia").val() + '}', contentType: "application/json", success: function (res) {
            try {
                $("#ciudad").html('');
                var options = '';
                options += '<option disabled selected>Seleccione una ciudad</option>';
                $("#ciudad").append(options);
                $.each(res.d, function (data, value) {
                    $("#ciudad").append($("<option></option>").text(value.Nombre).val(value.Id));
                });
            }
            catch (exception) {
                alert(exception.message);
            }
        }

    });
};

function muestra_oculta(id) {
    if (document.getElementById) { //se obtiene el id
        var el = document.getElementById(id); //se define la variable "el" igual a nuestro div
        el.style.display = (el.style.display == 'none') ? 'block' : 'none'; //damos un atributo display:none que oculta el div
    };
}

function admin_mensaje() {
    if (document.getElementById('hfOperacion').value == 'nuevo') {
        document.getElementById('contact_form').style.display = 'block';
        document.getElementById('mainok').style.display = 'none';
        document.getElementById('mainerror').style.display = 'none';
        document.getElementById('txtDNI').focus();
    };
    if (document.getElementById('hfOperacion').value == 'ok') {
        document.getElementById('contact_form').style.display = 'none';
        document.getElementById('mainok').style.display = 'block';
        document.getElementById('mainerror').style.display = 'none';
    };
    if (document.getElementById('hfOperacion').value == 'error') {
        document.getElementById('contact_form').style.display = 'none';
        document.getElementById('mainok').style.display = 'none';
        document.getElementById('mainerror').style.display = 'block';
    };
}

function responseToSave() {
    var result = true;
    $("#hfDNI").val($("#documento").val());
    $("#hfApellido").val($("#apellido").val());
    $("#hfNombre").val($("#nombre").val());
    $("#hfGenero").val($("input[name='genero']:checked").val());
    $("#hfFechaNacimiento").val($("#fecha").val());
    $("#hfTelefono").val($("#telefono").val());
    $("#hfCelular").val($("#celular").val());
    $("#hfDireccion").val($("#direccion").val());
    $("#hfPostal").val($("#postal").val());
    $("#hfProvincia").val($("#provincia option:selected").val());
    $("#hfProvinciaNombre").val($("#provincia option:selected").text());
    $("#hfCiudad").val($("#ciudad option:selected").val());
    $("#hfCiudadNombre").val($("#ciudad option:selected").text());
    $("#hfEmail").val($("#email").val());
    $("#hfGrupo").val($("#grupo").val());
    $("#hfTarjeta").val($("#tarjeta").val());
    return result;
}

function checkdni(documento) {
    var quantity = documento.length;

    $('#nombre').val('');
    $("#nombre").prop('readonly', '');
    $('#apellido').val('');
    $("#apellido").prop('readonly', '');

    if (quantity == 8) {
        //Aca le tenes que colocar el servicio de nosis y dentro la logica
        $.ajax({
            type: "GET",
            url: 'https://localhost:44342/validar/' + documento,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            success: function (resultado) {
                //logica al ejecutar el ajax
                resultado.contenido.datos.variables.forEach(function (variable) {
                    if (variable.nombre === 'VI_Nombre') {
                        $('#nombre').val(variable.valor);
                        $("#nombre").prop('readonly', 'readonly');
                        $("#hfValidado").val(1);
                    }

                    if (variable.nombre === 'VI_Apellido') {
                        $('#apellido').val(variable.valor);
                        $("#apellido").prop('readonly', 'readonly');
                        $("#hfValidado").val(1);
                    }

                })
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                //en caso de que ocurra un error
            }
        });
    }
}
