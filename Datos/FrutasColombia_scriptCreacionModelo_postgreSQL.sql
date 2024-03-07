-- Scripts de clase - Marzo 5 de 2024 
-- Curso de Tópicos Avanzados de base de datos - UPB 202320
-- Juan Dario Rodas - juand.rodasm@upb.edu.co

-- Proyecto: Atlas de Frutas Colombianas
-- Motor de Base de datos: PostgreSQL

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

-- crear el usuario con el que se floatizará la creación del modelo
create user frutas_app with encrypted password 'unaClav3';

-- asignación de privilegios para el usuario
grant connect on database frutas_db to frutas_app;
grant create on database frutas_db to frutas_app;
grant create, usage on schema core to frutas_app;
alter user frutas_app set search_path to core;

set timezone='America/Bogota';

-- Script de creación de tablas y vistas

create table core.tmp_divipola
(
    codigo_departamento varchar(2) not null,
    codigo_municipio varchar(10) not null,
    nombre_departamento varchar(100) not null,
    nombre_municipio varchar(100) not null
);

-- -----------------------
-- Tabla Departamentos
-- -----------------------
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
    from core.tmp_divipola
);

-- -----------------------
-- Tabla Municipios
-- -----------------------
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
    from core.tmp_divipola
);

-- -----------------------
-- Tabla Frutas
-- -----------------------

create table core.frutas
(
    id              integer generated always as identity constraint frutas_pk primary key,
    nombre          varchar(100) not null constraint frutas_nombre_uk unique,
    url_wikipedia   varchar(500),
    url_imagen      varchar(500)
);

-- -----------------------
-- Tabla Taxonomia
-- -----------------------

create table core.taxonomias
(
    fruta_id        integer not null constraint taxonomia_fruta_fk references core.frutas,
    reino           varchar(50) not null,
    division        varchar(50) not null,
    clase           varchar(50) not null,
    orden           varchar(50) not null,
    familia         varchar(50) not null,
    genero          varchar(50) not null
);

alter table taxonomias add constraint taxonomias_pk primary key (fruta_id);

-- Inserción preliminar de información
insert into frutas (nombre, url_wikipedia, url_imagen)
values (
    'Mango',
    'https://es.wikipedia.org/wiki/Mango_(fruta)',
    'https://upload.wikimedia.org/wikipedia/commons/4/49/Mango_-_single.jpg'
);

insert into taxonomias (fruta_id, reino, division, clase,orden,familia,genero)
values (
    1,
    'Plantae',
    'Magnoliophyta',
    'Magnoliopsida',
    'Sapindales',
    'Anacardiaceae',
    'Mangifera'
);

-- -----------------------
-- Creación de vistas
-- -----------------------

create or replace view core.v_info_frutas as (
    select 
        t.fruta_id,
        f.nombre,
        f.url_wikipedia,
        f.url_imagen,
        t.reino,
        t.division,
        t.clase,
        t.orden,
        t.familia,
        t.genero        
    from frutas f inner join taxonomias t on f.id = t.fruta_id
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
