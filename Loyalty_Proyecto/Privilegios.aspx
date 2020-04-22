<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Privilegios.aspx.cs" Inherits="SGM_LOYALTY.Privilegios" %>

<!DOCTYPE html>

<link href="CSS/familyOpenSans.css" rel="stylesheet" />

<script src="Scripts/Activate_modernizr_min.js" type="text/javascript"></script>
<script src="Scripts/Activate_jquery_min.js" type="text/javascript"></script>
<script src="Scripts/Activate_bootstrap_min.js" type="text/javascript"></script>

<script src="Scripts/bootStrapValidator.js"></script>

<link href="CSS/Activate_bootstrap_min.css" rel="stylesheet" />
<link href="https://maxcdn.bootstrapcdn.com/bootstrap/3.2.0/css/bootstrap.min.css" rel="stylesheet">
<link href="CSS/Activate_bootstrap_theme_min.css" rel="stylesheet" />
<link href="CSS/Activate_bootstrapValidator_min.css" rel="stylesheet" />

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <title>Privilegios</title>

    <link href="CSS/Activate.css" rel="stylesheet" />
    <script src="Scripts/Privilegios.js"></script>

</head>
<body onload="admin_mensaje();">

<div class="container">
    <div style="height:120px" align="center">
        <table style="height:100%" class="tabla_sin" >
            <tr>
                <td align="center">
	                <a href="/" title="Inicio" rel="home" id="logo">
	                    <img src="Images/Privilegios.jpg" alt="Inicio" height="100" data-sticky-height="40" />
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
                    Privilegios - Activación Tarjeta
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
              <input id="documento" name="documento" placeholder="Ingrese número" class="form-control"  type="number" >
                </div>
              </div>
            </div>

            <!-- Tarjeta-->
            <div class="form-group" id ="tottar" style="visibility:hidden;" >
            <label class="col-md-4 control-label">Tarjeta</label>
                <div class="col-md-4 inputGroupContainer">
                    <div class="input-group">
                        <span class="input-group-addon"><i class="glyphicon glyphicon-barcode"></i></span>
                        <input id="tarjeta" name="tarjeta" placeholder="Ingrese número de tarjeta" class="form-control" type="number" autocomplete="off">
                    </div>
                </div>
            </div>

            <!-- Apellido-->
            <div class="form-group" id ="totape" style="visibility:hidden;" >
              <label class="col-md-4 control-label" >Apellido</label> 
                <div class="col-md-4 inputGroupContainer">
                    <div class="input-group">
                        <span class="input-group-addon"><i class="glyphicon glyphicon-user"></i></span>
                        <input id="apellido" name="apellido" placeholder="Ingrese su apellido" class="form-control"  type="text">
                    </div>
              </div>
            </div>

            <!-- Nombre-->
            <div class="form-group" id ="totnom" style="visibility:hidden;">
              <label class="col-md-4 control-label">Nombre</label>  
                  <div class="col-md-4 inputGroupContainer">
                      <div class="input-group">
                          <span class="input-group-addon"><i class="glyphicon glyphicon-user"></i></span>
                          <input id="nombre" name="nombre" placeholder="Ingrese su nombre" class="form-control"  type="text">
                       </div>
                  </div>
            </div>
                
            <!-- Dirección-->
            <div class="form-group" id ="totdic" style="visibility:hidden;">
              <label class="col-md-4 control-label">Dirección</label>  
                <div class="col-md-4 inputGroupContainer">
                <div class="input-group">
                    <span class="input-group-addon"><i class="glyphicon glyphicon-home"></i></span>
                    <input id="direccion" name="direccion" placeholder="Ingrese su dirección" class="form-control" type="text">
                </div>
              </div>
            </div>

            <div id="maincontent" align="center" class="form-group">
                <table style="width:100%">
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
            <asp:hiddenfield id="hfTarjeta" value="" runat="server"/>
            <asp:hiddenfield id="hfOperacion" value="" runat="server"/>
            <asp:hiddenfield id="hfNombre" value="" runat="server"/>
        </div>
    </form>

    <div id="mainok" align="center" class="content">
        <table>
            <tr style="height:100px">
                <td class="lbldata">Muchas Gracias por Registrarte
                </td>
            </tr>
        </table>
    </div>

    <div id="mainerror" align="center" class="content">
        <table>
            <tr style="height:100px">
                <td class="lblrequired">Ha ocurrido un error, contacte al administrador del sistema.
                    <br />
                    <asp:Label ID="lblError" runat="server"></asp:Label>
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
                        <p><strong>Email:</strong> <a href="mailto:sistemas@diarco.com.ar" class="lblwhitedata">sistemas@diarco.com.ar</a></p>
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
