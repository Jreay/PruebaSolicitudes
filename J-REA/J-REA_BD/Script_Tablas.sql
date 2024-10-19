create table Usuarios(
UsuarioId int primary key identity(1,1),
NombreUsuario nvarchar(25) not null,
Clave nvarchar(25) not null,
EsAdministrador bit not null
)

create table Solicitudes(
SolicitudId int primary key identity(1,1),
ClienteId int not null,
Tipo nvarchar(100) not null,
Descripcion nvarchar(100) not null,
Justificativo nvarchar(max) not null,
Estado nvarchar(100) not null,
DetalleGestion nvarchar(100),
FechaIngreso nvarchar(100),
FechaActualizacion nvarchar(100),
FechaGestion nvarchar(100)
)

insert into Usuarios (NombreUsuario, Clave, EsAdministrador)
values ('admin', 'admin', 1),
       ('cliente1', 'cliente1', 0),
	   ('cliente2', 'cliente2', 0)