
CREATE DATABASE Aempap2023
GO

use Aempap2023
GO

set dateformat dmy
go



--*************
--tabla pais

CREATE TABLE tb_paises(
  idPais char(3) primary key,
  NombrePais varchar(40) not null
)
go

insert into tb_paises values('001','Peru'),
('002','Argentina'),
('003','Chile'),
('004','USA'),
('005','España'),
('006','Francia'),
('007','Colombia'),
('008','Canada'),
('009','China'),
('010','Brasil')
go

create or alter proc usp_listarPais
as
select * from tb_paises
go

exec usp_listarPais
go

--DROP PROCEDURE [usp_accounts];
--GO
--******************
--tabla usuarios
create table tb_usuarios
(
usuario varchar(60) primary key,
clave varchar(60),
nombre varchar(255),
apellido varchar(255),
idPais char(3) references tb_paises,
telefono char(9),
correo varchar(255)
)
go

-- Insertar el primer usuario
INSERT INTO tb_usuarios (usuario, clave, nombre, apellido, idPais, telefono, correo)
VALUES ('ecastillo', '72728607', 'Enzo', 'Castillo', '001', '971680273', 'ecastillo@gmail.com');

-- Insertar el segundo usuario
INSERT INTO tb_usuarios (usuario, clave, nombre, apellido, idPais, telefono, correo)
VALUES ('fnovoa', '123456', 'Fernando', 'Novoa', '001', '987654321', 'fnovoa@gmail.com');

CREATE or alter PROCEDURE usp_Login
AS
BEGIN
    SELECT usuario, clave,CONCAT(nombre,' ',apellido)
    FROM tb_usuarios;
END

exec usp_Login

--******************
--tabla categorias

create table tb_categorias
(
idCat char(3) primary key,
nomCat varchar(35)
)
go

insert into tb_categorias values('001','Pasteles y tartas'),
('002','Galletas y pastas'),
('003','Panadería'),
('004','Pastelería francesa'),
('005','Postres'),
('006','Bollería'),
('007','Repostería sin gluten o sin azúcar'),
('008','Dulces'),
('009','Salados')
go

--listar
create or alter proc usp_listarCate
as
select * from tb_categorias
go

exec usp_listarCate
go

--*************
--tabla cliente

create sequence Cod_Cli
as int
start with 1
increment by 1
no cycle
no cache
go

--Restablece el codigo "idcli"
alter sequence Cod_Cli restart with 1;
--

create table tb_clientes
(
idCli char(5) not null default 'C' + right('0000' + cast(next value for Cod_Cli as varchar),4 ) ,
nomCli varchar(40) not null,
apeCli varchar(40) not null,
dniCli varchar(10) not null,
DireccCli varchar(60) not null,
idPais char(3) references tb_paises(idPais),
correoCli varchar(50) not null,
TelefCli varchar(24) not null,
Constraint pk_idCli primary key(idCli)
)
go

insert into tb_clientes (nomCli,apeCli,dniCli,DireccCli,idPais,correoCli,TelefCli) 
values ('Venta','Rapida','72728607','Av. Riva Aguero 950','001','aempap@gmail.com','965412895')
go
                          
--listar

create or alter proc usp_listarClientes
as
select C.idCli,C.nomCli,C.apeCli,C.dniCli,C.DireccCli,C.idPais,P.NombrePais,C.correoCli,C.TelefCli from tb_clientes C inner join tb_paises P on C.idPais=P.idPais
go

exec usp_listarClientes
go

--insertar

create or alter proc usp_GuardarCliente
@nomCli varchar(40),
@apeCli varchar(40),
@dniCli varchar(10),
@DireccCli varchar(60),
@idpais char(3),
@correoCli varchar(50) ,
@TelefCli varchar(24)
As
Begin
insert into tb_clientes(nomCli,apeCli,dniCli,DireccCli,idPais,correoCli,
TelefCli)
Values(@nomCli,@apeCli,@dniCli,@DireccCli,@idpais,@correoCli,@TelefCli)
End
go

--actualizar

create or alter proc usp_EditarCliente
@idCli char(5),
@nomCli varchar(40),
@apeCli varchar(40),
@dniCli varchar(10),
@DireccCli varchar(60),
@idPais char(3),
@correoCli varchar(50) ,
@TelefCli varchar(24)
As
Begin
Update tb_clientes set nomCli=@nomCli,apeCli=@apeCli,dniCli=@dniCli,DireccCli=@DireccCli,idPais=@idPais,correoCli=@correoCli,TelefCli=@TelefCli
Where idCli=@idCli
End
go

--eliminar

create or alter proc usp_EliminarCliente
@idCli char(5)
As
Begin
delete from  tb_clientes Where idCli=@idCli
End
go


--*************
--tabla provedores

create sequence Cod_Prove
as int
start with 1
increment by 1
no cycle
no cache
go

--Restablece el codigo "idProv"
alter sequence Cod_Prove restart with 1;


create table tb_proveedores
(
idProv char(5) not null default 'PV' + right('000' + cast(next value for Cod_Prove as varchar),3 ) ,
razProv varchar(35) not null,
rucProv varchar(25) not null,
telProv varchar(24) not null,
correoProv varchar(50) not null,
direcProv varchar(50) not null,
idPais char(3) references tb_paises,
Constraint pk_idProv primary key(idProv)
)
go


--listar
create or alter proc usp_listarProve
as
select O.idProv,O.razProv,O.rucProv,O.telProv,O.correoProv,O.direcProv,O.idPais,A.NombrePais from tb_proveedores O inner join tb_paises A on O.idPais=A.idPais
go

exec usp_listarProve
go


--insertar
create or alter proc usp_GuardarProve
@razProv varchar(35),
@rucProv varchar(25),
@telProv varchar(24),
@correoProv varchar(50),
@direcProv varchar(50),
@idPais char(3)
as
begin
insert into tb_proveedores(razProv,rucProv,telProv,correoProv,direcProv,idPais)
values(@razProv,@rucProv,@telProv,@correoProv,@direcProv,@idPais)
end
go


--actualizar
create or alter proc usp_EditarProve
@idProv char(5),
@razProv varchar(35),
@rucProv varchar(25),
@telProv varchar(24),
@correoProv varchar(50),
@direcProv varchar(50),
@idPais char(3)
as
begin
update tb_proveedores set razProv=@razProv,rucProv=@rucProv,telProv=@telProv,correoProv=@correoProv,direcProv=@direcProv,idPais=@idPais
where idProv=@idProv
end
go


--eliminar
create or alter proc usp_EliminarProve
@idProv char(5)
as
begin
delete from tb_proveedores where idProv=@idProv
end
go

--*************
--tabla insumos

create sequence Cod_Insu
as int
start with 1
increment by 1
no cycle
no cache
go

--Restablece el codigo "idProv"
alter sequence Cod_Insu restart with 1;


create table tb_insumos
(
idIn char(5) not null default 'I' + right('0000' + cast(next value for Cod_Insu as varchar),4 ) ,
desIn varchar(60) not null,
idProv char(5) references tb_proveedores,
fecComIn date,
fecVenIn date
Constraint pk_idind primary key(idIn)
)
go

create or alter proc usp_listarInsu
as
select I.idIn,I.desIn,I.idProv,P.razProv,I.fecComIn,I.fecVenIn from tb_insumos I inner join tb_proveedores P on I.idProv=p.idProv
go


exec usp_listarInsu
go


--insertar

create or alter proc usp_GuardarInsu
@desIn varchar(60),
@idProv char(5),
@fecComIn date,
@fecVenIn date
as
begin
insert into tb_insumos(desIn,idProv,fecComIn,fecVenIn)
values(@desIn,@idProv,@fecComIn,@fecVenIn)
end
go

--editar
create or alter proc usp_EditarInsu
@idIn char(5),
@desIn varchar(60),
@idProv char(5),
@fecComIn date,
@fecVenIn date
as
begin
update tb_insumos set desIn=@desIn,idProv=@idProv,fecComIn=@fecComIn,fecVenIn=@fecVenIn
where idIn=@idIn
end
go

--eliminar
create or alter proc usp_EliminarInsu
@idIn char(5)
as
begin
delete tb_insumos where idIn=@idIn
end
go


--*********************
--tabla productos
create sequence Cod_Prod
as int
start with 1
increment by 1
no cycle
no cache
go

--Restablece el codigo "idProd"
alter sequence Cod_Prod restart with 1;


create table tb_productos
(
idProd char(5) not null default 'PO' + right('000' + cast(next value for Cod_Prod as varchar),3 ) ,
nomProd varchar(40) not null,
fotoProd varchar(255),
idIn char(5) references tb_insumos,
idCate char(3) references tb_categorias,
descProd varchar(100),
preProd decimal(10,0) not null,
stockProd smallint not null
Constraint pk_idProd primary key(idProd)
)
go

--listar
create or alter proc usp_listarProductos
as
select P.idProd,P.nomProd,P.fotoProd,I.desIn,P.idCate,C.nomCat,P.descProd,P.preProd,P.stockProd from tb_insumos I inner join tb_productos P on I.idIn=P.idIn inner join tb_categorias C on P.idCate=C.idCat
go

exec usp_listarProductos
go


--insertar
create or alter proc usp_AgregarProductos
@nomProd varchar(40),
@fotoProd varchar(255),
@idIn char(5),
@idCate char(3),
@descProd varchar(100),
@preProd decimal(10,0),
@stockProd smallint
as
begin
insert into tb_productos(nomProd,fotoProd,idIn,idCate,descProd,preProd,stockProd)
values(@nomProd,@fotoProd,@idIn,@idCate,@descProd,@preProd,@stockProd)
end
go

--Actualizar
create or alter proc usp_EditarProductos
@idProd char(5),
@nomProd varchar(40),
@fotoProd varchar(255),
@idIn char(5),
@idCate char(3),
@descProd varchar(100),
@preProd decimal(10,0),
@stockProd smallint
as
begin
update tb_productos set nomProd=@nomProd,fotoProd=@fotoProd,idIn=@idIn,idCate=@idCate,descProd=@descProd,preProd=@preProd,stockProd=@stockProd
where idProd=@idProd
end
go


--eliminar
create or alter proc usp_EliminarProductos
@idProd char(5)
as
begin
delete tb_productos where idProd=@idProd
end
go

--COMPRA

create table tb_pedidos(
	idPed int primary key,
	fechaPed datetime default(getdate()),
	idCli char(5) references tb_clientes,
	metodoPed varchar(30),
	EntregaPed varchar(30),
	EstadoPed varchar(30)
)
go

create table tb_pedidosdeta(
	idPed int references tb_pedidos,
	idProd char(5) references tb_productos,
	preUni decimal,
	cantidad int
)
go


--listar pedido
create or alter proc usp_ListarPedido
as
select P.idPed,P.fechaPed,D.idProd,O.nomProd,P.idCli,CONCAT(C.nomCli,' ',C.apeCli),P.metodoPed,P.EntregaPed,P.EstadoPed,O.preProd,D.cantidad,(D.preUni*D.cantidad) from tb_pedidos P inner join tb_clientes C on P.idCli=C.idCli inner join tb_pedidosdeta D on P.idPed=D.idPed inner join tb_productos O on D.idProd=O.idProd
go

create or alter proc usp_ListarPedidoExcel
as
select P.idPed as NroPedido,P.fechaPed as Fecha,D.idProd as CódigoProd,O.nomProd as NombreProd,P.idCli as CódigoCliente,CONCAT(C.nomCli,' ',C.apeCli) as NombreCliente,P.metodoPed as Metodo,P.EntregaPed as Entrega,P.EstadoPed as Estado,preProd as PrecioUnitario,cantidad as Cantidad,(D.preUni*D.cantidad) as Monto from tb_pedidos P inner join tb_clientes C on P.idCli=C.idCli inner join tb_pedidosdeta D on P.idPed=D.idPed inner join tb_productos O on D.idProd=O.idProd
go


exec usp_ListarPedido
go

--agregar pedido
create or alter proc usp_AgregarPedido
@idPed int,
@idCli char(5),
@metodoPed varchar(30),
@EntregaPed varchar(30),
@EstadoPed varchar(30)
as
begin
insert tb_pedidos(idPed,idCli,metodoPed,EntregaPed,EstadoPed) values(@idPed,@idCli,@metodoPed,@EntregaPed,@EstadoPed)
end
go

--agregar detalle de pedido
create or alter proc usp_AgregarDetaPedido
@idPed int,
@idProd char(5),
@preUni decimal,
@cantidad int
as
begin
insert tb_pedidosdeta(idPed,idProd,preUni,cantidad) values(@idPed,@idProd,@preUni,@cantidad)
end
go

--actualiza el estado del pedido
create or alter proc usp_EditarPedidoEsta
@idPed int,
@idCli char(5),
@metodoPed varchar(30),
@EntregaPed varchar(30),
@EstadoPed varchar(30)
As
Begin
Update tb_pedidos set idCli=@idCli,metodoPed=@metodoPed,EntregaPed=@EntregaPed,EstadoPed=@EstadoPed
where idPed=@idPed
End
go

--elimina el  pedido
create or alter proc usp_EliminarPedido
@idPed int
As
Begin
delete from tb_pedidosdeta where idPed = @idPed
delete from tb_pedidos where idPed=@idPed
End
go

--actualiza el stock de los productos
create or alter proc usp_productosActualizar
@idProd char(5),
@cantidad int
As
Update tb_productos Set stockProd-= @cantidad Where idProd=@idProd
go


select * from tb_pedidos go
select * from tb_pedidosdeta go

--Dashboard

--resumen venta
create or alter proc usp_ResumenVenta
as
begin
    set nocount on;

    select CONVERT(date, fechaPed) as fecha, COUNT(*) as totalPedidos
    from tb_pedidos
    where fechaPed >= DATEADD(DAY, -5, GETDATE())
    group by CONVERT(date, fechaPed)
    order by fecha desc;
end


--Resumen Productos
create or alter proc usp_ResumenProducto
as
begin
    set nocount on;

    select top 5 O.nomProd, SUM(cantidad) as TotalVentas
    from tb_pedidosdeta P inner join tb_productos O on P.idProd=O.idProd
    group by O.nomProd
    order by TotalVentas desc;
end


create or alter proc usp_ResumenVentaxCli
as
begin
    set nocount on;

    select CONCAT(C.nomCli, ' ', C.apeCli) as Nombre, COUNT(*) AS totalPedidos
    from tb_pedidos P inner join tb_clientes C on P.idCli=C.idCli
    group by CONCAT(C.nomCli, ' ', C.apeCli)
    order by totalPedidos desc;
end


exec usp_ResumenProducto
exec usp_ResumenVenta
exec usp_ResumenVentaxCli
