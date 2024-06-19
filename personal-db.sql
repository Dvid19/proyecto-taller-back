
CREATE DATABASE personal;

USE personal;


CREATE TABLE personalizacion(
	idPersonalizacion int primary key,
	nav text,
	footer text
);
INSERT INTO personalizacion (idPersonalizacion, nav, footer) VALUES
(1, 'MARCA', 'Nombre Completo');


CREATE TABLE header(
	idHeader int primary key,
	titulo varchar(150),
	subtitulo varchar(200),
	documentoCv text,
	fotoFondo text,
);
INSERT INTO header (idHeader, titulo, subtitulo, documentoCv, fotoFondo) VALUES
(1, 'Titulo de la pagína', 'El subtitulo de la pagina', '', '');

CREATE TABLE header_foto_carrusel(
	idHeaderFotoCarrusel int primary key identity(1,1),
	foto text
);


-- PARTE DEL CONTACTO
CREATE TABLE contacto(
	idContacto int primary key,
	foto text,
	titulo varchar(50),
	subtitulo varchar(100)
);
INSERT INTO contacto (idContacto, foto, titulo, subtitulo) VALUES 
(1, '', 'Descripción corta', 'Descripcion extensa');

CREATE TABLE link (
	idLink int primary key identity(1,1),
	foto text,
	link text,
	texto varchar(90),
);