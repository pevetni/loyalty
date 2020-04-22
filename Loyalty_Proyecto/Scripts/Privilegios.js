$(document).ready(function () {

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
    $("#hfTarjeta").val($("#tarjeta").val());
    return result;
}
