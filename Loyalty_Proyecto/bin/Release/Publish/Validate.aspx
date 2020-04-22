<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Validate.aspx.cs" Inherits="SGM_LOYALTY.Validate" %>

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

    <title>Diarco+</title>

    <link href="CSS/Activate.css" rel="stylesheet" />
    <script src="Scripts/Activate.js"></script>


</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div style="height: 120px" align="center">
                <table style="height: 100%" class="tabla_sin">
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
                <table style="width: 100%; background-color: white;" class="tabla_sin">
                    <tr style="height: 3px">
                        <td colspan="3" style="background-color: #303030;"></td>
                    </tr>
                    <tr>
                        <td colspan="3" class="lblTitle">Diarco+ Activación cliente
                        </td>
                    </tr>
                    <tr style="height: 3px">
                        <td style="background-color: gray; width: 5%"></td>
                        <td style="background-color: red; width: 20%"></td>
                        <td style="background-color: #303030; width: 75%"></td>
                    </tr>
                </table>
                <div class="row" style="margin-top: 20px; margin-bottom: 20px;">
                    <div class="col-md-12">
                        <asp:Label ID="lblMensajeActivacion" runat="server" CssClass="text-info" />
                    </div>
                </div>

                <div class="row" style="background-color: red; margin-left: 0px; margin-right: 15px;">
                    <div class="col-sm-4" style="background-color: red">
                        <a href="/diarco/" id="A2" rel="home" title="Home">
                            <img alt="Home" data-sticky-height="40" src="https://www.diarco.com.ar/sites/default/files/logo_diarco_3.png" style="max-width: 80%;" />
                        </a>
                    </div>
                    <div class="col-sm-4" style="background-color: red;">
                        <table style="width: 100%" class="tabla_sin">
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
                    <div class="col-sm-4" style="background-color: red">
                        <table style="width: 100%" class="tabla_sin">
                            <tr>
                                <td class="lblwhitedata">
                                    <strong>Horario de atención</strong>
                                </td>
                            </tr>
                            <tr>
                                <td class="lblwhitedata">
                                    <p>Lunes a Viernes<br />
                                        de 8:00 a 18.00 hs</p>
                                    <p>Sábados<br />
                                        de 8:00 a 17:00 hs<br />
                                        <a href="http://qr.afip.gob.ar/?qr=9f8r_Z3k1J9HY600lv9z2w,," target="_F960AFIPInfo">
                                            <img src="https://www.diarco.com.ar/DATAWEB.jpg" border="0"></a></p>
                                </td>
                            </tr>
                            <tr>
                                <td class="lblwhitedata"></td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
