create table LOYALTY.AltasPromocionesPendientes (
	idAlta bigint identity,
	tipoDocumentoCliente varchar(4),
	documentoCliente varchar(11),
	fechaPendiente datetime,
	fechaAltaConfirmada datetime,
	altaConfirmada tinyint, 
	request nvarchar(max)
)

go

create procedure LOYALTY.sp_Promociones_Pendiente_Insert (
	@tipoDocumentoCliente varchar(4),
	@documentoCliete varchar(11),
	@request nvarchar(max),
	@output int output
)
as
begin

	if (select COUNT(*) from LOYALTY.AltasPromocionesPendientes where tipoDocumentoCliente = @tipoDocumentoCliente and documentoCliente = @documentoCliete)<=0
	begin
		insert into 
			LOYALTY.AltasPromocionesPendientes (tipoDocumentoCliente, documentoCliente, fechaPendiente, fechaAltaConfirmada, altaConfirmada, request)
			values (@tipoDocumentoCliente, @documentoCliete, GETDATE(), null, 0, @request)
		select @output=1
	end
	else
	begin
		select @output=2
	end
end

go

create procedure LOYALTY.sp_Promociones_Pendiente_Update (
	@tipoDocumentoCliente varchar(4),
	@documentoCliete varchar(11)
)
as
begin
	update 
		LOYALTY.AltasPromocionesPendientes 
	set fechaAltaConfirmada = GETDATE(),
		altaConfirmada = 1
	where 
		tipoDocumentoCliente = @tipoDocumentoCliente
		and documentoCliente = @documentoCliete
end

go

create procedure LOYALTY.sp_Promociones_Pendientes_Select
as
begin
	select 
		idAlta,
		tipoDocumentoCliente,
		documentoCliente,
		request
	from LOYALTY.AltasPromocionesPendientes
	where fechaAltaConfirmada is null
		and altaConfirmada = 0
end