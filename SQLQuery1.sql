--el inicio de pago es el mes siguiente
--de acuerdo al mismo : si el prestamo es el 18 las cuotas de pago
--son el 18 de cada mes

----------------------------------------------------------------------
----------------------------------------------------------------------

create database prestamo5
use prestamo5

select * from cliente;
select * from cuota where idPrest=2;

update cuota set fechaPa='06-18-2019' where idCuo=28;
select * from pago;

---------------------------------------------------------------------
-------------------------------TABLAS--------------------------------
create table cliente(
idCli int identity(1,1) primary key,
nombres varchar(30) not null,
apellidos varchar(30) not null,
dni varchar(8) not null,
direccion varchar(40),
fechaRegistro date not null
)

create table prestamo(
idPres int primary key identity(1,1),
fechaIni date not null,
fechaTerm date not null,
monto decimal(7,2) not null,
tea decimal(7,2) not null,
cantCut int not null,
idClie int foreign key references cliente(idCli) on delete cascade
)

create table cuota(
idCuo int primary key identity(1,1),
periodo int not null,
saldo decimal(7,2) not null,
amortizacion decimal(7,2) not null,
interes decimal(7,2) not null,
cuota decimal(7,2) not null,
estado bit,
fechaPa date not null,
idPrest int foreign key references prestamo(idPres) on delete cascade
)

create table DetPago(
idDetPag int primary key identity(1,1),
efectivo decimal(7,2) not null,
vuelto decimal(7,2) default 0,
iM decimal(7,2) default 0,
iCv decimal(7,2) default 0,
)

create table pago(
idPag int primary key identity(1,1),
fechaPago date not null,
mora decimal(7,2),
total decimal(7,2) not null,
idClin int foreign key references cliente(idCli) on delete cascade,
idPres int foreign key references prestamo(idPres),
idCuot int foreign key references cuota(idCuo),
idDetPg int foreign key references DetPago(idDetPag) on delete cascade
)

--------------------------------------------------------------------------
-----------------------------PROCEDIMIENTOS-------------------------------

create PROC bus_cliente_dni
@prmintDni varchar(8)
as
select nombres, apellidos, idCli, dni, direccion, fechaRegistro 
from cliente 
where dni like @prmintDni;

exec bus_cliente_dni @prmintDni= '78546957';

---------------------------------------------------------------------------

create PROC veri_prests
@prmintCliId int,
@valido int output
as
declare @vali int
declare @cantPres int
declare @cantCuo int
declare @valCuo int

select @vali= count(p.idPres) 
from prestamo p
where p.idClie = @prmintCliId

if @vali < 1
begin
	set @valido = 1;
end
else
	begin
	select @cantPres = count(p.cantCut), @cantCuo = sum(p.cantCut)
	from prestamo as p
	where p.idClie = @prmintCliId

	select @valCuo = COUNT(c.estado)
	from prestamo as p inner join cuota as c on p.idPres = c.idPrest
	where idClie = @prmintCliId and c.estado = 1

	declare @toCuo int = @cantPres + @cantCuo

	if @toCuo = @valCuo
	begin
		set @valido = 1
	end
	else
	begin
		set @valido = 2
	end
	end
return @valido

 declare @validar int
exec veri_prests 1,@valido=@validar output
print (@validar)

-------------------------------------------------------------------------

create PROC RegistrarPrestamo
@prmtDtFechaIni date,
@prmtDtFechaTerm date,
@prmtFloatMonto decimal(7,2),
@prmtFloatTea decimal(7,2),
@prmtIntCantCu int,
@prmtCliIdCli int,
@idPres int output
as

insert into prestamo (fechaIni, fechaTerm, monto, tea, cantCut, idClie)
values (@prmtDtFechaIni, @prmtDtFechaTerm, @prmtFloatMonto, @prmtFloatTea, @prmtIntCantCu,
		@prmtCliIdCli)

select @idPres = scope_identity() 

return @idPres
-------------------------------------------------------------------------


create PROC RegistrarCuota
@prmtIntPeriodo int,
@prmtFloatSaldo decimal(7,2),
@prmtFloatAmortizacion decimal(7,2),
@prmtFloatInteres decimal(7,2),
@prmtFloatCuota decimal(7,2),
@prmtDTFechPa date,
@prmtIntPrestamoId int
as

if(@prmtIntPeriodo = 0)
begin

insert into cuota (periodo, saldo, amortizacion, interes, cuota, fechaPa, idPrest, estado)
values (@prmtIntPeriodo, @prmtFloatSaldo, @prmtFloatAmortizacion, @prmtFloatInteres,
		@prmtFloatCuota, @prmtDTFechPa, @prmtIntPrestamoId, 1)
end
else
begin
insert into cuota (periodo, saldo, amortizacion, interes, cuota, fechaPa, idPrest, estado)
values (@prmtIntPeriodo, @prmtFloatSaldo, @prmtFloatAmortizacion, @prmtFloatInteres,
		@prmtFloatCuota, @prmtDTFechPa, @prmtIntPrestamoId, 0)
end

--------------------------------------------------------------------------

create  procedure LsCuotas
@prmintDni varchar(8)
as
declare @con int
select c.amortizacion, c.cuota, c.estado, c.fechaPa, c.idCuo,
c.interes, c.periodo, c.saldo, c.idPrest, p.idClie, p.cantCut, p.fechaIni,
p.fechaTerm, p.monto, p.tea
 from cuota c inner join prestamo p on p.idPres=c.idPrest
where p.idClie = (select idCli
from cliente 
where dni = @prmintDni)

-----------------------------------------------------------------------------

create procedure GuardarPago
@prmtDateFechaPago date,
@prmtDeciMora decimal(7,2),
@prmtDeciTotal decimal(7,2),
@prmtIntidCli int,
@prmtIntIdPres int,
@prmtIntIdCuo int,
@prmtDeciEfectivo decimal(7,2),
@prmtDeciVuelto decimal(7,2),
@prmtDeciIm decimal(7,2),
@prmtDeciIcv decimal(7,2)
as
declare @idDetP int
insert into DetPago(efectivo, vuelto, iM, iCv)
values(@prmtDeciEfectivo,@prmtDeciVuelto,@prmtDeciIm,@prmtDeciIcv)
select @idDetP = scope_identity() 
insert into pago(fechaPago, idClin, idCuot, idPres, mora, total,idDetPg)
values (@prmtDateFechaPago, @prmtIntidCli, @prmtIntIdCuo, @prmtIntIdPres,
@prmtDeciMora, @prmtDeciTotal,@idDetP)

update cuota set estado = 1 where idCuo = @prmtIntIdCuo;

-------------------------------------------------------------------------
------------------------PROCEDIMIENTO OPCIONAL --------------------------
create PROC veri_prest
@prmintCliId int,
@valido bit output
as
declare @cuota int
declare @cant int
select @cant = p.cantCut, @cuota = COUNT(c.estado)-1
from prestamo as p inner join cuota as c on p.idPres = c.idPrest
where idClie = @prmintCliId and c.estado = 1
group by p.cantCut
if @cuota = @cant
begin
 set @valido = 1
 end
else
begin
 set @valido = 0
 end
return @valido

declare @validar bit
exec veri_prest 10,@valido=@validar output
print (@validar)


delete from prestamo
where idPres = 11;
delete from cuota
where idPrest = 11;

select * from prestamo
where idPres = 12;


----------------------------------------------------------------------------------------
-----------------------------------------INSERT-----------------------------------------

insert into cliente(nombres,apellidos,dni,direccion,fechaRegistro)
values('Rodrigo Andres','Xernal Diaz','78546957','Av. America #2121','12-03-2019')

insert into cliente (apellidos, direccion, dni, nombres, fechaRegistro)
values ('Diosdado','Diaz Cmapen #2039', '87452145', 'Jimeno', '12/03/2018')

insert into cliente(nombres,apellidos,dni,direccion,fechaRegistro)
values('Diana Carol','Atre Centeno','17596484','Av. America #5234','2019-03-15')

insert into cliente(nombres,apellidos,dni,direccion,fechaRegistro)
values('Mireya Fernanda','Casanova Cervantez','74848752','Av. America #6548','2019-03-20')

insert into cliente(nombres,apellidos,dni,direccion,fechaRegistro)
values('Cinthya Ximena','Vernal Romano','79632568','Av. America #1245','2018-04-03')

insert into cliente(nombres,apellidos,dni,direccion,fechaRegistro)
values(' Ximena','Zariñaga','87458748','Av. America #1245','2018-04-03')

insert into cliente(nombres,apellidos,dni,direccion,fechaRegistro)
values(' Diana Carolina','Zervantres Moreno','70658945','Av. Ricardo #457','2019-01-03')

insert into cliente(nombres,apellidos,dni,direccion,fechaRegistro)
values('Roberto Cesar','Santos Jimenez','17895612','Av. America Sur #125','2018-12-12')

insert into cliente(nombres,apellidos,dni,direccion,fechaRegistro)
values('Wilder Segundo','Turizo Vernal','70564816','Av. America Norte #125','2018-12-23')

insert into cliente(nombres,apellidos,dni,direccion,fechaRegistro)
values('Marco Antonio','Quezquen Romano','17457845','Av. America Norte #464','2018-12-24')
-----------------------------------------------------------------------------------------

insert into prestamo (fechaIni, fechaTerm, monto, tea, cantCut, idClie) 
values('2019-03-03','2021-03-03',1000.00,0.10,24,1)

------------------------------------------------------------------------------------------

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(0,1000,0,0,0,1,1,'2019-03-03')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(1,962.93,37.07,10.00,47.07,0,1,'2019-04-03')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(2,925.49,37.44,9.63,47.07,0,1,'2019-05-03')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(3,887.67,37.82,9.25,47.07,0,1,'2019-06-03')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(4,849.48,38.19,8.88,47.07,0,1,'2019-07-03')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(5,810.90,38.58,8.49,47.07,0,1,'2019-08-03')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(6,771.94,38.96,8.11,47.07,0,1,'2019-09-03')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(7,732.59,39.35,7.72,47.07,0,1,'2019-10-03')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(8,692.85,39.74,7.33,47.07,0,1,'2019-11-03')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(9,652.71,40.14,6.93,47.07,0,1,'2019-12-03')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(10,612.17,40.54,6.53,47.07,0,1,'2020-01-03')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(11,571.22,40.95,6.12,47.07,0,1,'2020-02-03')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(12,529.86,41.36,5.71,47.07,0,1,'2020-03-03')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(13,488.09,41.77,5.30,47.07,0,1,'2020-04-03')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(14,445.90,42.19,4.88,47.07,0,1,'2020-07-03')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(15,403.29,42.61,4.46,47.07,0,1,'2020-08-03')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(16,360.25,43.04,4.03,47.07,0,1,'2020-09-03')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(17,316.78,43.47,3.60,47.07,0,1,'2020-10-03')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(18,272.88,43.90,3.17,47.07,0,1,'2020-11-03')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(19,228.54,44.34,2.73,47.07,0,1,'2020-12-03')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(20,183.76,44.78,2.29,47.07,0,1,'2021-01-03')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(21,138.53,45.23,1.84,47.07,0,1,'2021-02-03')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(22,92.86,45.68,1.39,47.07,0,1,'2021-03-03')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(23,46.71,46.14,0.93,47.07,0,1,'2021-04-03')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(24,0.11,46.60,0.47,47.07,0,1,'2021-05-03')
-----------------------------------------------------------------------------------------

insert into prestamo (fechaIni, fechaTerm, monto, tea, cantCut, idClie) 
values('2019-05-12','2020-06-12',3000,0.2,12,2)

------------------------------------------------------------------------------------------

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(0,3000,0,0,0,1,2,'2019-05-12')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(1,2776.32,223.68,60.00,283.68,0,2,'2019-06-12')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(2,2548.17,228.15,55.53,283.68,0,2,'2019-07-12')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(3,2315.45,232.72,50.96,283.68,0,2,'2019-08-12')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(4,2078.08,237.37,46.31,283.68,0,2,'2019-09-12')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(5,1835.96,242.12,41.56,283.68,0,2,'2019-10-12')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(6,1589.00,246.96,36.72,283.68,0,2,'2019-11-12')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(7,1337.10,251.90,31.78,283.68,0,2,'2019-12-12')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(8,1080.16,256.94,26.74,283.68,0,2,'2020-01-12')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(9,818.08,262.08,21.60,283.68,0,2,'2020-02-12')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(10,550.76,267.32,16.36,283.68,0,2,'2020-03-12')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(11,278.10,272.66,11.02,283.68,0,2,'2020-04-12')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(12,-0.02,278.12,5.56,283.68,0,2,'2020-05-12')

------------------------------------------------------------------------------------------

insert into prestamo (fechaIni, fechaTerm, monto, tea, cantCut, idClie) 
values('2019-05-25', '2020-10-25',16000,0.15,16,3)

-----------------------------------------------------------------------------------------

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(0,16000,0,0,0,1,3,'2019-05-25')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(1,15072.89,927.11,160.00,1087.11,0,3,'2019-06-25')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(2,14136.51,936.38,150.73,1087.11,0,3,'2019-07-25')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(3,13190.77,945.74,141.37,1087.11,0,3,'2019-08-25')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(4,12235.57,955.20,131.91,1087.11,0,3,'2019-09-25')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(5,11270.82,964.75,122.36,1087.11,0,3,'2019-10-25')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(6,10296.42,974.40,112.71,1087.11,0,3,'2019-11-25')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(7,9312.27,984.15,102.96,1087.11,0,3,'2019-12-25')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(8,8318.28,993.99,93.12,1087.11,0,3,'2020-01-25')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(9,7314.35,1003.93,83.18,1087.11,0,3,'2020-02-25')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(10,6300.38,1013.97,73.14,1087.11,0,3,'2020-03-25')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(11,5276.27,1024.11,63.00,1087.11,0,3,'2020-04-25')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(12,4241.92,1034.35,52.76,1087.11,0,3,'2020-05-25')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(13,3197.23,1044.69,42.42,1087.11,0,3,'2020-06-25')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(14,2142.09,1055.14,31.97,1087.11,0,3,'2020-07-25')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(15,1076.40,1065.69,21.42,1087.11,0,3,'2020-08-25')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(16,0.05,1076.35,10.76,1087.11,0,3,'2020-09-25')

-----------------------------------------------------------------------------------------

insert into prestamo (fechaIni, fechaTerm, monto, tea, cantCut, idClie) 
values('2019-05-25', '2020-10-25',20000,0.18,10,4)

-----------------------------------------------------------------------------------------

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(0,20000,0,0,0,1,4,'2019-05-25')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(1,18088.36,1911.64,200.00,2111.64,0,4,'2019-06-25')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(2,16157.60,1930.76,180.88,2111.64,0,4,'2019-07-25')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(3,14207.54,1950.06,161.58,2111.64,0,4,'2019-08-25')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(4,12237.98,1969.56,142.08,2111.64,0,4,'2019-09-25')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(5,10248.72,1989.26,122.38,2111.64,0,4,'2019-10-25')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(6,8239.57,2009.15,102.49,2111.64,0,4,'2019-11-25')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(7,6210.33,2029.54,82.40,2111.64,0,4,'2019-12-25')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(8,4160.79,2029.24,62.12,2111.64,0,4,'2020-01-25')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(9,2090.76,2070.03,41.61,2111.64,0,4,'2020-02-25')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(10,0.03,2090.73,20.91,2111.64,0,4,'2020-03-25')

-----------------------------------------------------------------------------------------

insert into prestamo (fechaIni, fechaTerm, monto, tea, cantCut, idClie) 
values('2018-11-13', '2019-05-13',3800,0.1,6,5)

-----------------------------------------------------------------------------------------

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(0,3800,0,0,0,1,5,'2018-11-13')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(1,3182.32,617.68,38.00,0,1,5,'2018-12-13')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(2,2558.46,623.86,31.82,655.68,1,5,'2019-01-13')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(3,1928.36,630.10,25.58,655.68,1,5,'2019-02-13')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(4,1291.96,636.40,19.28,655.68,1,5,'2019-03-13')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(5,649.20,642.76,12.92,655.68,1,5,'2019-04-13')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(6,0.01,649.19,6.49,655.68,1,5,'2019-05-13')

-----------------------------------------------------------------------------------------

insert into prestamo (fechaIni, fechaTerm, monto, tea, cantCut, idClie) 
values('2019-01-05', '2019-06-05',2800,0.1,5,7)

-----------------------------------------------------------------------------------------

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(0,2800,0,0,0,1,7,'2019-01-05')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(1,2251.09,548.91,28.00,576.91,1,7,'2019-02-05')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(2,1696.69,554.40,22.51,576.91,1,7,'2019-03-05')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(3,1136.75,559.94,16.97,576.91,1,7,'2019-04-05')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(4,571.21,565.54,11.37,576.91,1,7,'2019-05-05')

insert into cuota(periodo,saldo,amortizacion,interes,cuota,estado,idPrest,fechaPa) 
values(5,0.01,571.20,5.71,576.91,0,7,'2019-06-05')

