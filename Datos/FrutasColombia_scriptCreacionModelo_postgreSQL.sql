-- Scripts de clase - Marzo 5 de 2024 
-- Curso de Tópicos Avanzados de base de datos - UPB 202320
-- Juan Dario Rodas - juand.rodasm@upb.edu.co

-- Proyecto: Atlas de Frutas Colombianas
-- Motor de Base de datos: PostgreSQL 16.x

-- ***********************************
-- Abastecimiento de imagen en Docker
-- ***********************************
 
-- Descargar la imagen
docker pull postgres:latest

-- Crear el contenedor
docker run --name postgres-frutas -e POSTGRES_PASSWORD=unaClav3 -d -p 5432:5432 postgres:latest

-- ****************************************
-- Creación de base de datos y usuarios
-- ****************************************

-- Con usuario Root:

-- crear el esquema la base de datos
create database frutas_db;

-- Conectarse a la base de datos
\c frutas_db;

-- Creamos un esquema para almacenar todo el modelo de datos del dominio
create schema core;
create schema auth;
create schema localizacion;
create schema auditoria;

-- crear el usuario con el que se implementará la creación del modelo
create user frutas_app with encrypted password 'unaClav3';

-- asignación de privilegios para el usuario
grant connect on database frutas_db to frutas_app;
grant create on database frutas_db to frutas_app;
grant create, usage on schema core to frutas_app;
alter user frutas_app set search_path to core;


-- crear el usuario con el que se conectará la aplicación
create user frutas_usr with encrypted password 'unaClav3';

-- asignación de privilegios para el usuario
grant connect on database frutas_db to frutas_usr;
grant usage on schema core, localizacion to frutas_usr;
alter default privileges for user frutas_app in schema core grant insert, update, delete, select on tables to frutas_usr;
alter default privileges for user frutas_app in schema core grant execute on routines TO frutas_usr;
alter user frutas_usr set search_path to core;


-- ----------------------------------------
-- Script de creación de tablas y vistas
-- ----------------------------------------

-- Tabla divipola
create table core.divipola
(
    codigo_departamento varchar(2) not null,
    codigo_municipio varchar(10) not null,
    nombre_departamento varchar(100) not null,
    nombre_municipio varchar(100) not null
);

comment on table core.divipola is 'División Político Administrativa de Colombia';
comment on column core.divipola.codigo_departamento is 'id del departamento según DIVIPOLA';
comment on column core.divipola.nombre_departamento is 'Nombre del departamento';
comment on column core.divipola.codigo_municipio is 'id del municipio según DIVIPOLA';
comment on column core.divipola.nombre_municipio is 'Nombre del municipio';

-- Tabla Departamentos
create table core.departamentos
(
    id          varchar(2)          constraint departamentos_pk primary key,
    nombre 		varchar(60)         not null constraint departamentos_nombre_uk unique
);

comment on table core.departamentos is 'Departamentos del país';
comment on column core.departamentos.id is 'id del departamento según DIVIPOLA';
comment on column core.departamentos.nombre is 'Nombre del departamento';

-- cargamos desde la tabla inicial del divipola
insert into core.departamentos (id, nombre)
(
    select distinct codigo_departamento, initcap(nombre_departamento)
    from core.divipola
);

-- Tabla Municipios
create table core.municipios
(
    id              varchar(10)     constraint municipios_pk primary key,
    nombre          varchar(100)    not null,
    departamento_id varchar(2)      not null constraint municipio_departamento_fk references core.departamentos,
    constraint municipio_departamento_uk unique (nombre, departamento_id)
);

comment on table core.municipios is 'Municipios del pais';
comment on column core.municipios.id is 'id del municipio según DIVIPOLA';
comment on column core.municipios.nombre is 'Nombre del municipio';
comment on column core.municipios.departamento_id is 'id del departamento asociado al municipio';

-- Cargamos datos desde el esquema inicial
insert into core.municipios (id, nombre, departamento_id)
(
    select codigo_municipio, initcap(nombre_municipio), codigo_departamento
    from core.divipola
);

-- Tabla Frutas
create table core.frutas
(
    id              integer generated always as identity constraint frutas_pk primary key,
    nombre          varchar(100) not null constraint frutas_nombre_uk unique,
    url_wikipedia   varchar(500) not null,
    url_imagen      varchar(500) not null
);

comment on table core.frutas is 'Frutas de Colombia';
comment on column core.frutas.id is 'id de la fruta';
comment on column core.frutas.nombre is 'Nombre de la fruta';
comment on column core.frutas.url_wikipedia is 'URL en Wikipedia de la fruta';
comment on column core.frutas.url_imagen is 'URL de la imagen en Wikipedia de la fruta';

-- -------------------------------------------
-- Tablas de Taxonomia - Información Botánica
-- -------------------------------------------
-- Tabla Reinos
create table core.reinos
(
    id              integer generated always as identity constraint reinos_pk primary key,
    nombre          varchar(100) not null constraint reino_nombre_uk unique
);

comment on table core.reinos is 'Reinos en la clasificación botánica';
comment on column core.reinos.id is 'id del reino';
comment on column core.reinos.nombre is 'nombre del reino';

-- Tabla Divisiones
create table core.divisiones
(
    id              integer generated always as identity constraint divisiones_pk primary key,
    nombre          varchar(100) not null constraint division_nombre_uk unique,
    reino_id        integer not null constraint division_reino_fk references core.reinos
);

comment on table core.divisiones is 'Divisiones en la clasificación botánica';
comment on column core.divisiones.id is 'id de la división';
comment on column core.divisiones.nombre is 'nombre de la división';
comment on column core.divisiones.reino_id is 'ID del reino al que pertenece la división';

-- Tabla Clases
create table core.clases
(
    id              integer generated always as identity constraint clases_pk primary key,
    nombre          varchar(100) not null constraint clase_nombre_uk unique,
    division_id        integer not null constraint clase_division_fk references core.divisiones
);

comment on table core.clases is 'Clases en la clasificación botánica';
comment on column core.clases.id is 'id de la clase';
comment on column core.clases.nombre is 'nombre de la clase';
comment on column core.clases.division_id is 'ID de la división al que pertenece la clase';

-- Tabla Ordenes
create table core.ordenes
(
    id              integer generated always as identity constraint ordenes_pk primary key,
    nombre          varchar(100) not null constraint orden_nombre_uk unique,
    clase_id        integer not null constraint orden_clase_fk references core.clases
);

comment on table core.ordenes is 'Ordenes en la clasificación botánica';
comment on column core.ordenes.id is 'id del orden';
comment on column core.ordenes.nombre is 'nombre del orden';
comment on column core.ordenes.clase_id is 'ID de la clase al que pertenece el orden';

-- Tabla Familias
create table core.familias
(
    id              integer generated always as identity constraint familias_pk primary key,
    nombre          varchar(100) not null constraint familia_nombre_uk unique,
    orden_id        integer not null constraint familia_orden_fk references core.ordenes
);

comment on table core.familias is 'Familias en la clasificación botánica';
comment on column core.familias.id is 'id de la familia';
comment on column core.familias.nombre is 'nombre de la familia';
comment on column core.familias.orden_id is 'ID del orden al que pertenece la familia';

-- Tabla Generos
create table core.generos
(
    id              integer generated always as identity constraint generos_pk primary key,
    nombre          varchar(100) not null constraint genero_nombre_uk unique,
    familia_id        integer not null constraint genero_familia_fk references core.familias
);

comment on table core.generos is 'Géneros en la clasificación botánica';
comment on column core.generos.id is 'id del género';
comment on column core.generos.nombre is 'nombre del género';
comment on column core.generos.familia_id is 'ID de la familia al que pertenece el género';

-- Tabla Especies
create table core.especies
(
    id              integer generated always as identity constraint especies_pk primary key,
    nombre          varchar(100) not null constraint especie_nombre_uk unique,
    genero_id        integer not null constraint especie_genero_fk references core.generos
);

comment on table core.especies is 'Especies en la clasificación botánica';
comment on column core.especies.id is 'id de la especie';
comment on column core.especies.nombre is 'nombre de la especie';
comment on column core.especies.genero_id is 'ID del genero al que pertenece la especie';

create table core.taxonomia_frutas
(
    especie_id integer not null constraint taxonomia_frutas_especie_fk references core.especies,
    fruta_id   integer not null constraint taxonomia_frutas_fruta_fk references core.frutas,
    constraint taxonomia_frutas_pk primary key (fruta_id, especie_id)
);

comment on table core.taxonomia_frutas is 'Relación de la fruta con su clasificación taxonómica';
comment on column core.taxonomia_frutas.fruta_id is 'Id de la fruta';
comment on column core.taxonomia_frutas.especie_id is 'Especie taxonómica a la que pertenece la fruta';

-- Inserción preliminar de información
insert into core.frutas (nombre, url_wikipedia, url_imagen)
values (
    'Mango',
    'https://es.wikipedia.org/wiki/Mango_(fruta)',
    'https://upload.wikimedia.org/wikipedia/commons/4/49/Mango_-_single.jpg'
);

insert into core.frutas (nombre, url_wikipedia, url_imagen)
values (
    'Feijoa',
    'https://es.wikipedia.org/wiki/Acca_sellowiana',
    'https://es.wikipedia.org/wiki/Acca_sellowiana#/media/Archivo:Acca_sellowiana_Fruit_MHNT_Fronton.jpg'
);

insert into core.taxonomias (fruta_id, reino, division, clase,orden,familia,genero,especie)
values (
    1,
    'Plantae',
    'Magnoliophyta',
    'Magnoliopsida',
    'Sapindales',
    'Anacardiaceae',
    'Mangifera',
    'Mangifera indica'
);

insert into core.taxonomias (fruta_id, reino, division, clase,orden,familia,genero,especie)
values (
    2,
    'Plantae',
    'Magnoliophyta',
    'Magnoliopsida',
    'Myrtales',
    'Myrtaceae',
    'Acca',
    'Acca sellowiana'
);

-- -----------------------
-- Creación de vistas
-- -----------------------
-- v_info_botanica
create or replace view v_info_botanica as
(
select r.id     reino_id,
       r.nombre reino_nombre,
       d.id     division_id,
       d.nombre division_nombre,
       c.id     clase_id,
       c.nombre clase_nombre,
       o.id     orden_id,
       o.nombre orden_nombre,
       f.id     familia_id,
       f.nombre familia_nombre,
       g.id     genero_id,
       g.nombre genero_nombre,
       e.id     especie_id,
       e.nombre especie_nombre
from reinos r
         left join divisiones d on r.id = d.reino_id
         left join clases c on d.id = c.division_id
         left join ordenes o on c.id = o.clase_id
         left join familias f on o.id = f.orden_id
         left join generos g on f.id = g.familia_id
         left join especies e on g.id = e.genero_id
);

create view core.v_info_frutas as (
    select
        f. id fruta_id,
        f.nombre fruta_nombre,
        f.url_wikipedia,
        f.url_imagen,
        v.reino_nombre,
        v.division_nombre,
        v.clase_nombre,
        v.orden_nombre,
        v.familia_nombre,
        v.genero_nombre,
        v.especie_nombre
    from core.frutas f
        left join core.taxonomia_frutas tf on f.id = tf.fruta_id
        left join v_info_botanica v on tf.especie_id = v.especie_id
);


-- -----------------------
-- Consultas de apoyo
-- -----------------------

-- Relación departamento -  municipio
select
    d.nombre departamento,
    m.nombre municipio
from departamentos d inner join municipios m on d.id = m.departamento_id
order by 1,2;

-- Total municipios por departamento
select distinct d.nombre, count(m.id) total
from departamentos d inner join municipios m on d.id = m.departamento_id
group by d.nombre
order by 2 desc;


--- ##########################################
--  Zona de peligro - Borrado de elementos
--- ##########################################

drop table core.especies;
drop table core.generos;
drop table core.familias;
drop table core.ordenes;
drop table core.clases;
drop table core.divisiones;
drop table core.reinos;

drop view core.v_info_frutas;

drop table core.taxonomias;
drop table core.frutas;

drop table core.municipios;
drop table core.departamentos;