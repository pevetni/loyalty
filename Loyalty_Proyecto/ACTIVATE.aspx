<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ACTIVATE.aspx.cs" Inherits="SGM_LOYALTY.ACTIVATE" %>

<!DOCTYPE html>

<link href="CSS/familyOpenSans.css" rel="stylesheet" />

<script src="Scripts/Activate_modernizr_min.js" type="text/javascript"></script>

<script src="Scripts/Activate_jquery_min.js" type="text/javascript"></script>
<script src="Scripts/Activate_bootstrap_min.js" type="text/javascript"></script>

<script src="Scripts/bootStrapValidator.js"></script>

<link href="CSS/Activate_bootstrap_min.css" rel="stylesheet" />
<%--<link href="https://maxcdn.bootstrapcdn.com/bootstrap/3.2.0/css/bootstrap.min.css" rel="stylesheet">--%>
<link href="CSS/bootstrap-3.2.0/dist/css/bootstrap.min.css" rel="stylesheet" />
<link href="CSS/Activate_bootstrap_theme_min.css" rel="stylesheet" />
<link href="CSS/Activate_bootstrapValidator_min.css" rel="stylesheet" />

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <title>Diarco+</title>

    <link href="CSS/Activate.css" rel="stylesheet" />
    <script src="Scripts/Activate.js"></script>
    <script type="text/javascript" > 
        function checkdni(documento) {
            var quantity = documento.length;

            $('#nombre').val('');
            $("#nombre").prop('readonly', '');
            $('#apellido').val('');
            $("#apellido").prop('readonly', '');

            if (quantity == 8) {
                //Aca le tenes que colocar el servicio de nosis y dentro la logica
                $("#hfValidado").val(0);
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

    </script>

</head>
<body onload="muestra_oculta('legalcontent');admin_mensaje();">

<div class="container">
    <div style="height:120px" align="center">
        <table style="height:100%" class="tabla_sin" >
            <tr>
                <td align="center">
	                <a href="/" title="Inicio" rel="home" id="logo">
	                    <img src="https://www.diarco.com.ar/sites/default/files/logo_diarco_3.png" alt="Inicio" height="100" data-sticky-height="40" />
	                </a>
                </td>
            </tr>
        </table>
    </div>

    <div class="form-group">
        <table style="width:100%; background-color:white" class="tabla_sin" >
            <tr style="height:3px">
                <td colspan="3" style="background-color:#303030;"></td>
            </tr>
            <tr>
                <td colspan="3" class="lblTitle">
                    Diarco+ Activate
                </td>
            </tr>
            <tr style="height:3px">
                <td style="background-color:gray; width:5%"></td>
                <td style="background-color:red; width:20%"></td>
                <td style="background-color:#303030; width:75%"></td>
            </tr>
        </table>
		<div class="row">
			<div class="col-md-12">
					
			</div>
		</div>
	</div>

    <form class="form-horizontal" action=" " method="post"  id="contact_form" runat="server" style="width:98%">
        <fieldset>
            <!-- Documento Unico-->
            <div class="form-group">
              <label class="col-md-4 control-label" >Documento Unico</label> 
                <div class="col-md-4 inputGroupContainer">
                <div class="input-group">
              <span class="input-group-addon"><i class="glyphicon glyphicon-user"></i></span>
              <input id="documento" name="documento" placeholder="Ingrese número" class="form-control"  type="number" onkeyup="checkdni(documento.value)">
                </div>
              </div>
            </div>

            <!-- Apellido-->
            <div class="form-group">
              <label class="col-md-4 control-label" >Apellido</label> 
                <div class="col-md-4 inputGroupContainer">
                <div class="input-group">
              <span class="input-group-addon"><i class="glyphicon glyphicon-user"></i></span>
              <input id="apellido" name="apellido" placeholder="Ingrese su apellido" class="form-control"  type="text" autocomplete="off">
                </div>
              </div>
            </div>

            <!-- Nombre-->
            <div class="form-group">
              <label class="col-md-4 control-label">Nombre</label>  
              <div class="col-md-4 inputGroupContainer">
              <div class="input-group">
              <span class="input-group-addon"><i class="glyphicon glyphicon-user"></i></span>
              <input id="nombre" name="nombre" placeholder="Ingrese su nombre" class="form-control"  type="text" autocomplete="off">
                </div>
              </div>
            </div>

            <!-- Genero -->
             <div class="form-group">
                <label class="col-md-4 control-label">Genero</label>
                <div class="col-md-4">
                    <div class="radio">
                        <label>
                            <input type="radio" name="genero" value="F" required /> Femenino
                        </label>
                    </div>
                    <div class="radio">
                        <label>
                            <input type="radio" name="genero" value="M" required /> Masculino
                        </label>
                    </div>
                </div>
            </div>

            <!-- Fecha de Nacimiento-->
            <div class="form-group">
              <label class="col-md-4 control-label">Fecha de Nacimiento</label>  
                <div class="col-md-4 inputGroupContainer">
                <div class="input-group">
                    <span class="input-group-addon"><i class="glyphicon glyphicon-calendar"></i></span>
              <input id="fecha" name="fecha" placeholder="" class="form-control"  type="date" autocomplete="off">
                </div>
              </div>
            </div>

            <!-- Dirección-->
            <div class="form-group">
              <label class="col-md-4 control-label">Dirección</label>  
                <div class="col-md-4 inputGroupContainer">
                <div class="input-group">
                    <span class="input-group-addon"><i class="glyphicon glyphicon-home"></i></span>
                    <input id="direccion" name="direccion" placeholder="Ingrese su dirección" class="form-control" type="text" autocomplete="off">
                </div>
              </div>
            </div>

            <!-- Provincia -->
            <div class="form-group"> 
                <label class="col-md-4 control-label">Provincia</label>
                <div class="col-md-4 selectContainer">
                    <div class="input-group">
                        <span class="input-group-addon"><i class="glyphicon glyphicon-list"></i></span>
                        <select name="provincia" id="provincia" class="form-control selectpicker" onchange="getCiudades()" >
                        </select>
                    </div>
                </div>
            </div>

            <!-- Ciudad -->
            <div class="form-group"> 
                <label class="col-md-4 control-label">Ciudad</label>
                <div class="col-md-4 selectContainer">
                    <div class="input-group">
                        <span class="input-group-addon"><i class="glyphicon glyphicon-list"></i></span>
                        <select name="ciudad" id="ciudad" class="form-control selectpicker" >
                        </select>
                    </div>
                </div>
            </div>

            <!-- Código Postal-->
            <div class="form-group">
                <label class="col-md-4 control-label">Código Postal</label>  
                <div class="col-md-4 inputGroupContainer">
                    <div class="input-group">
                        <span class="input-group-addon"><i class="glyphicon glyphicon-home"></i></span>
                        <input id="postal" name="postal" placeholder="Ingrese código postal" class="form-control"  type="text" autocomplete="off">
                    </div>
                </div>
            </div>

            <!-- E-Mail-->
            <div class="form-group">
                <label class="col-md-4 control-label">E-Mail</label>  
                <div class="col-md-4 inputGroupContainer">
                    <div class="input-group">
                        <span class="input-group-addon"><i class="glyphicon glyphicon-envelope"></i></span>
                        <input id="email" name="email" placeholder="Ingrese su correo electrónico" class="form-control"  type="text" autocomplete="off">
                    </div>
                </div>
            </div>

            <!-- Teléfono Fijo-->
            <div class="form-group">
                <label class="col-md-4 control-label">Teléfono Fijo</label>  
                <div class="col-md-4 inputGroupContainer">
                    <div class="input-group">
                        <span class="input-group-addon"><i class="glyphicon glyphicon-earphone"></i></span>
                        <input id="telefono" name="telefono" placeholder="(011)5082-8000" class="form-control" type="text" autocomplete="off">
                    </div>
                </div>
            </div>

            <!-- Teléfono Celular-->
            <div class="form-group">
                <label class="col-md-4 control-label">Teléfono Celular</label>  
                <div class="col-md-4 inputGroupContainer">
                    <div class="input-group">
                        <span class="input-group-addon"><i class="glyphicon glyphicon-phone"></i></span>
                        <input id="celular" name="celular" placeholder="(011)5082-8000" class="form-control" type="text" autocomplete="off">
                    </div>
                </div>
            </div>


            <!-- Grupo-->
            <div class="form-group">
                <label class="col-md-4 control-label">Grupo</label>  
                <div class="col-md-4 inputGroupContainer">
                    <div class="input-group">
                        <span class="input-group-addon"><i class="glyphicon glyphicon-shopping-cart"></i></span>
                        <input id="grupo" name="grupo" placeholder="Ingrese código de grupo" class="form-control" type="text" autocomplete="off">
                    </div>
                </div>
            </div>

            <!-- Tarjeta-->
            <div class="form-group">
            <label class="col-md-4 control-label">Tarjeta</label>
                <div class="col-md-4 inputGroupContainer">
                    <div class="input-group">
                        <span class="input-group-addon"><i class="glyphicon glyphicon-barcode"></i></span>
                        <input id="tarjeta" name="tarjeta" placeholder="Ingrese número de tarjeta" class="form-control" type="number" autocomplete="off">
                    </div>
                </div>
            </div>


            <!-- Terminos y Condiciones -->
   
            <div class="form-group"> 
                <label class="col-md-4 control-label">Términos y Condiciones</label>
                <div class="col-md-4 selectContainer">
                    <div class="input-group">
                        <span class="input-group-addon"><i class="glyphicon glyphicon-exclamation-sign"></i></span>
                        <select name="acepta" class="form-control selectpicker" onclick="muestra_oculta('legalcontent');" >
                            <option value=" " >Acepta las condiciones</option>
                            <option>SI</option>
                        </select>
                    </div>
                </div>
            </div>


            <div id="maincontent" align="center" class="form-group">
                <table style="width:100%">
                    <tr>
                        <td colspan="2" align="left">
                            <div id="legal" class="titulo_boton">
                                Términos y condiciones
                                <a style='cursor: pointer;' onClick="muestra_oculta('legalcontent');" title="" class="boton_mostrar">Mostrar / Ocultar</a>
                            </div>

                            <div id="legalcontent">
                                <table style="width:100%">
                                    <tr>
                                        <td>
                                            <iframe id="content" width="100%" style="border:none" src="TerminosYCondiciones.htm"></iframe>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <br />
                    <tr>
                        <td colspan="2" align="center">
                            <br />
                            <asp:Button ID="btnSend" runat="server" CssClass="button" Text="Enviar" CausesValidation="true" OnClientClick="return responseToSave();" OnClick="btnSend_Click"/>

                        </td>
                    </tr>


                </table>
            </div>

        </fieldset>

        <div style="visibility:hidden">
            <asp:hiddenfield id="hfDNI" value="" runat="server"/>
            <asp:hiddenfield id="hfApellido" value="" runat="server"/>
            <asp:hiddenfield id="hfNombre" value="" runat="server"/>
            <asp:hiddenfield id="hfGenero" value="" runat="server"/>
            <asp:hiddenfield id="hfFechaNacimiento" value="" runat="server"/>
            <asp:hiddenfield id="hfTelefono" value="" runat="server"/>
            <asp:hiddenfield id="hfCelular" value="" runat="server"/>
            <asp:hiddenfield id="hfDireccion" value="" runat="server"/>
            <asp:hiddenfield id="hfPostal" value="" runat="server"/>
            <asp:hiddenfield id="hfProvincia" value="" runat="server"/>
            <asp:hiddenfield id="hfProvinciaNombre" value="" runat="server"/>
            <asp:hiddenfield id="hfCiudad" value="" runat="server"/>
            <asp:hiddenfield id="hfCiudadNombre" value="" runat="server"/>
            <asp:hiddenfield id="hfEmail" value="" runat="server"/>
            <asp:hiddenfield id="hfGrupo" value="" runat="server"/>
            <asp:hiddenfield id="hfTarjeta" value="" runat="server"/>
            <asp:hiddenfield id="hfOperacion" value="" runat="server"/>
            <asp:hiddenfield id="hfValidado" value="" runat="server"/>
        </div>
    </form>

    <div id="mainok" align="center" class="content">
        <table>
            <tr style="height:100px">
                <td class="lbldata"><asp:Label ID="lblMensajeFinRegistracion" runat="server" />
                </td>
            </tr>
        </table>
    </div>

    <div id="mainerror" align="center" class="content">
        <table>
            <tr style="height:100px">
                <td class="lbldata">
                    <asp:Label ID="lblError" runat="server" />
                </td>
            </tr>
        </table>
    </div>

    <div class="row" style="background-color:red">
        <div class="col-sm-4" style="background-color:red">
            <a href="/diarco/" id="A2" rel="home" title="Home">
                <img alt="Home" data-sticky-height="40" src="https://www.diarco.com.ar/sites/default/files/logo_diarco_3.png" style="max-width: 80%;" /> 
            </a>
        </div>
        <div class="col-sm-4" style="background-color:red">
            <table style="width:100%" class="tabla_sin" >
                <tr>
                    <td class="lblwhitedata">
                        <strong>Contacto</strong>
                    </td>
                </tr>
                <tr>
                    <td class="lblwhitedata">
                        <p>Teléfono: (011) 5082-8000</p>
                    </td>
                </tr>
                <tr>
                    <td class="lblwhitedata">
                        <p><strong>Email:</strong> <a href="mailto:info@diarco.com.ar" class="lblwhitedata">info@diarco.com.ar</a></p>
                    </td>
                </tr>
            </table>
        </div>
        <div class="col-sm-4" style="background-color:red">
            <table style="width:100%" class="tabla_sin" >
                <tr>
                    <td class="lblwhitedata">
                        <strong>Horario de atención</strong>
                    </td>
                </tr>
                <tr>
                    <td class="lblwhitedata">
                        <p>Lunes a Viernes<br />de 8:00 a 18.00 hs</p>
                        <p>Sábados<br />de 8:00 a 17:00 hs<br /><a href="http://qr.afip.gob.ar/?qr=9f8r_Z3k1J9HY600lv9z2w,," target="_F960AFIPInfo"><img src="https://www.diarco.com.ar/DATAWEB.jpg" border="0"></a></p>
                    </td>
                </tr>
                <tr>
                    <td class="lblwhitedata">
                                    
                    </td>
                </tr>
            </table>
        </div>
    </div>

</div>

</body>
</html>
