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

-- activación de la extensión uuid-ossp - Permite usar los uuid
create extension if not exists "uuid-oosp";

-- ----------------------------------------
-- Script de creación de tablas y vistas
-- ----------------------------------------

-- Tabla divipola
create table core.divipola
(
    codigo_departamento varchar(2) not null,
    codigo_municipio varchar(10) not null,
    nombre_departamento varchar(100) not null,
    nombre_municipio varchar(100) not null,
    guid_departamento uuid;
);

comment on table core.divipola is 'División Político Administrativa de Colombia';
comment on column core.divipola.codigo_departamento is 'id del departamento según DIVIPOLA';
comment on column core.divipola.nombre_departamento is 'Nombre del departamento';
comment on column core.divipola.codigo_municipio is 'id del municipio según DIVIPOLA';
comment on column core.divipola.nombre_municipio is 'Nombre del municipio';
comment on column core.divipola.guid_departamento is 'GUID del departamento';

-- Tabla Departamentos
create table core.departamentos
(
    id              uuid default uuid_generate_v4() constraint departamentos_pk primary key,    
    divipola_id     varchar(2) not null,
    nombre 		    varchar(60) not null constraint departamentos_nombre_uk unique
);

comment on table core.departamentos is 'Departamentos del país';
comment on column core.departamentos.id is 'id del departamento';
comment on column core.departamentos.id is 'id DIVIPOLA del departamento';
comment on column core.departamentos.nombre is 'Nombre del departamento';

-- cargamos desde la tabla inicial del divipola
insert into core.departamentos (divipola_id, nombre)
(
    select distinct codigo_departamento, initcap(nombre_departamento)
    from core.divipola
);

-- devolvemos el guid del departamento al divipola

update divipola set guid_departamento =
    (select id from departamentos where upper(nombre) = nombre_departamento
                                  and codigo_departamento = divipola_id)
where departamento_guid is null;

-- Tabla Municipios
create table core.municipios
(
    id              uuid default uuid_generate_v4() constraint municipios_pk primary key,
    divipola_id     varchar(5) not null,    
    nombre          varchar(100) not null,    
    departamento_id uuid not null constraint municipio_departamento_fk references core.departamentos,
    constraint municipio_departamento_uk unique (nombre, departamento_id)
);

comment on table core.municipios is 'Municipios del pais';
comment on column core.municipios.id is 'id del municipio';
comment on column core.departamentos.id is 'id DIVIPOLA del municipio';
comment on column core.municipios.nombre is 'Nombre del municipio';
comment on column core.municipios.departamento_id is 'id del departamento asociado al municipio';

-- Cargamos datos desde el esquema inicial
insert into core.municipios (divipola_id, nombre, departamento_id)
(
    select codigo_municipio, initcap(nombre_municipio), departamento_guid
    from core.divipola
);

-- Tabla Frutas
create table core.frutas
(
    id              uuid default uuid_generate_v4() constraint frutas_pk primary key,
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
    id              uuid default uuid_generate_v4() constraint reinos_pk primary key,
    nombre          varchar(100) not null constraint reino_nombre_uk unique
);

comment on table core.reinos is 'Reinos en la clasificación botánica';
comment on column core.reinos.id is 'id del reino';
comment on column core.reinos.nombre is 'nombre del reino';

-- Tabla Divisiones
create table core.divisiones
(
    id              uuid default uuid_generate_v4() constraint divisiones_pk primary key,
    nombre          varchar(100) not null constraint division_nombre_uk unique,
    reino_id        uuid not null constraint division_reino_fk references core.reinos
);

comment on table core.divisiones is 'Divisiones en la clasificación botánica';
comment on column core.divisiones.id is 'id de la división';
comment on column core.divisiones.nombre is 'nombre de la división';
comment on column core.divisiones.reino_id is 'ID del reino al que pertenece la división';

-- Tabla Clases
create table core.clases
(
    id              uuid default uuid_generate_v4() constraint clases_pk primary key,
    nombre          varchar(100) not null constraint clase_nombre_uk unique,
    division_id     uuid not null constraint clase_division_fk references core.divisiones
);

comment on table core.clases is 'Clases en la clasificación botánica';
comment on column core.clases.id is 'id de la clase';
comment on column core.clases.nombre is 'nombre de la clase';
comment on column core.clases.division_id is 'ID de la división al que pertenece la clase';

-- Tabla Ordenes
create table core.ordenes
(
    id              uuid default uuid_generate_v4() constraint ordenes_pk primary key,
    nombre          varchar(100) not null constraint orden_nombre_uk unique,
    clase_id        uuid not null constraint orden_clase_fk references core.clases
);

comment on table core.ordenes is 'Ordenes en la clasificación botánica';
comment on column core.ordenes.id is 'id del orden';
comment on column core.ordenes.nombre is 'nombre del orden';
comment on column core.ordenes.clase_id is 'ID de la clase al que pertenece el orden';

-- Tabla Familias
create table core.familias
(
    id              uuid default uuid_generate_v4() constraint familias_pk primary key,
    nombre          varchar(100) not null constraint familia_nombre_uk unique,
    orden_id        uuid not null constraint familia_orden_fk references core.ordenes
);

comment on table core.familias is 'Familias en la clasificación botánica';
comment on column core.familias.id is 'id de la familia';
comment on column core.familias.nombre is 'nombre de la familia';
comment on column core.familias.orden_id is 'ID del orden al que pertenece la familia';

-- Tabla Generos
create table core.generos
(
    id              uuid default uuid_generate_v4() constraint generos_pk primary key,
    nombre          varchar(100) not null constraint genero_nombre_uk unique,
    familia_id      uuid not null constraint genero_familia_fk references core.familias
);

comment on table core.generos is 'Géneros en la clasificación botánica';
comment on column core.generos.id is 'id del género';
comment on column core.generos.nombre is 'nombre del género';
comment on column core.generos.familia_id is 'ID de la familia al que pertenece el género';

-- Tabla Especies
create table core.especies
(
    id              uuid default uuid_generate_v4() constraint especies_pk primary key,
    nombre          varchar(100) not null constraint especie_nombre_uk unique,
    genero_id       uuid not null constraint especie_genero_fk references core.generos
);

comment on table core.especies is 'Especies en la clasificación botánica';
comment on column core.especies.id is 'id de la especie';
comment on column core.especies.nombre is 'nombre de la especie';
comment on column core.especies.genero_id is 'ID del genero al que pertenece la especie';

create table core.taxonomia_frutas
(
    especie_id uuid not null constraint taxonomia_frutas_especie_fk references core.especies,
    fruta_id   uuid not null constraint taxonomia_frutas_fruta_fk references core.frutas,
    constraint taxonomia_frutas_pk primary key (fruta_id, especie_id)
);

comment on table core.taxonomia_frutas is 'Relación de la fruta con su clasificación taxonómica';
comment on column core.taxonomia_frutas.fruta_id is 'Id de la fruta';
comment on column core.taxonomia_frutas.especie_id is 'Id de la especie taxonómica a la que pertenece la fruta';

-- -------------------------------------------
-- Tablas de Producción y Cultivo
-- -------------------------------------------
-- Tabla Climas
create table core.climas
(
    id              	uuid default uuid_generate_v4() constraint climas_pk primary key,
    nombre          	varchar(50) not null constraint clima_nombre_uk unique,
	altitud_minima		integer not null,
	altitud_maxima		integer not null
);

comment on table core.climas is 'Climas donde se producen las frutas';
comment on column core.climas.id is 'id del clima';
comment on column core.climas.nombre is 'nombre del clima';
comment on column core.climas.altitud_minima is 'Altitud mínima del piso térmico';
comment on column core.climas.altitud_maxima is 'Altitud máxima del piso térmico';

-- Tabla Epocas
create table core.epocas
(
    id              uuid default uuid_generate_v4() constraint epocas_pk primary key,
    nombre          varchar(50) not null constraint epocas_nombre_uk unique,
	mes_inicio		integer not null,
	mes_final		integer not null
);

comment on table core.epocas is 'épocas cuando se producen las frutas';
comment on column core.epocas.id is 'id de la época';
comment on column core.epocas.nombre is 'nombre de la época';
comment on column core.epocas.mes_inicio is 'Mes inicial de la época de producción';
comment on column core.epocas.mes_final is 'Mes final de la época de producción';

-- Tabla Produccion_Fruta
create table core.produccion_frutas
(
    fruta_id        uuid not null constraint produccion_frutas_fruta_fk references core.frutas,
    clima_id        uuid not null constraint produccion_frutas_clima_fk references core.climas,    
    epoca_id        uuid not null constraint produccion_frutas_epoca_fk references core.epocas,
    municipio_id    uuid not null constraint produccion_frutas_municipio_fk references core.municipios,
    constraint produccion_frutas_pk primary key (fruta_id, clima_id, epoca_id, municipio_id)
);

comment on table core.produccion_frutas is 'Relación de la fruta con su producción';
comment on column core.produccion_frutas.fruta_id is 'Id de la fruta';
comment on column core.produccion_frutas.clima_id is 'Id del clima';
comment on column core.produccion_frutas.epoca_id is 'Id de la epoca';
comment on column core.produccion_frutas.municipio_id is 'Id del municipio';

-- Tabla nutricion_frutas
create table core.nutricion_frutas
(
    fruta_id        uuid not null constraint nutricion_frutas_fruta_fk references core.frutas,
    carbohidratos   float not null,
    azucares        float not null,
    grasas          float not null,
    proteinas       float not null,
    constraint nutricion_frutas_pk primary key (fruta_id)
);

comment on table "core"."nutricion_frutas" is 'Valores nutricionales de la fruta por cada 100 gr';
comment on column "core"."nutricion_frutas"."fruta_id" is 'Id de la fruta';
comment on column "core"."nutricion_frutas"."carbohidratos" is 'Contenido en gr de carbohidratos';
comment on column "core"."nutricion_frutas"."azucares" IS 'Contenido en gr de azúcares';
comment on column "core"."nutricion_frutas"."proteinas" is 'Contenido en gr de proteinas';
comment on column "core"."nutricion_frutas"."grasas" is 'Contenido en gr de grasas';

-- -----------------------
-- Creación de vistas
-- -----------------------
-- v_info_botanica
create or replace view core.v_info_botanica as
(
    select distinct
        r.id     reino_id,
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

-- v_info_frutas
create or replace view core.v_info_frutas as
(
    select distinct
        f.id        fruta_id,
        f.nombre    fruta_nombre,
        f.url_wikipedia,
        f.url_imagen,
        v.reino_id,
        v.reino_nombre,
        v.division_id,
        v.division_nombre,
        v.clase_id,
        v.clase_nombre,
        v.orden_id,
        v.orden_nombre,
        v.familia_id,
        v.familia_nombre,
        v.genero_id,
        v.genero_nombre,
        v.especie_id,
        v.especie_nombre
    from core.frutas f
        left join core.taxonomia_frutas tf on f.id = tf.fruta_id
        left join v_info_botanica v on tf.especie_id = v.especie_id
);

-- v_info_produccion_frutas
create or replace view core.v_info_produccion_frutas as
(
    select distinct
        f.id                fruta_id,
        f.nombre            fruta_nombre,
        f.url_wikipedia     fruta_wikipedia,
        f.url_imagen        fruta_imagen,
        c.id                clima_id,
        c.nombre            clima_nombre,
        c.altitud_minima,
        c.altitud_maxima,
        e.nombre            epoca_nombre,
        e.mes_inicio,
        e.mes_final,
        m.id                municipio_id,
        m.nombre            municipio_nombre,
        d.id                departamento_id,
        d.nombre            departamento_nombre
    from core.frutas f
        left join  core.produccion_frutas pf on f.id = pf.fruta_id
        inner join core.climas c on pf.clima_id = c.id
        inner join core.epocas e on pf.epoca_id = e.id
        inner join core.municipios m on pf.municipio_id = m.id
        inner join core.departamentos d on m.departamento_id = d.id
);

-- v_info_nutricion_frutas
create or replace view core.v_info_nutricion_frutas as
(
    select distinct
        f.id fruta_id,
        f.nombre fruta_nombre,
        f.url_wikipedia,
        f.url_imagen,
        nf.azucares,
        nf.grasas,
        nf.carbohidratos,
        nf.proteinas
    from core.frutas f
        left join  core.nutricion_frutas nf on f.id = nf.fruta_id
);

-- v_info_ubicaciones
create or replace view core.v_info_ubicaciones as
(select distinct m.id municipio_id,
                 m.nombre municipio_nombre,
                 d.id departamento_id,
                 d.nombre departamento_nombre
 from core.municipios m join core.departamentos d on m.departamento_id = d.id);



-- ----------------------------
-- Creación de Funciones
-- ----------------------------
-- f_obtiene_reino_id
create or replace function core.f_obtiene_reino_id(in p_reino text)
returns uuid language plpgsql as
$$
    declare
        l_reino_id     uuid;

    begin
        select id into l_reino_id
        from core.reinos
        where lower(nombre) = lower(p_reino);

        return l_reino_id;
    end;
$$;

-- f_obtiene_division_id
create or replace function core.f_obtiene_division_id(in p_division text, in p_reino_id uuid)
returns uuid language plpgsql as
$$
    declare
        l_division_id     uuid;

    begin
        select id into l_division_id
        from core.divisiones
        where lower(nombre) = lower(p_division)
        and reino_id = p_reino_id;

        return l_division_id;
    end;
$$;

-- f_obtiene_clase_id
create or replace function core.f_obtiene_clase_id(in p_clase text, in p_division_id uuid)
returns uuid language plpgsql as
$$
    declare
        l_clase_id     uuid;

    begin
        select id into l_clase_id
        from core.clases
        where lower(nombre) = lower(p_clase)
        and division_id = p_division_id;

        return l_clase_id;
    end;
$$;

-- f_obtiene_orden_id
create or replace function core.f_obtiene_orden_id(in p_orden text, in p_clase_id uuid)
returns uuid language plpgsql as
$$
    declare
        l_orden_id     uuid;

    begin
        select id into l_orden_id
        from core.ordenes
        where lower(nombre) = lower(p_orden)
        and clase_id = p_clase_id;

        return l_orden_id;
    end;
$$;

-- f_obtiene_familia_id
create or replace function core.f_obtiene_familia_id(in p_familia text, in p_orden_id uuid)
returns uuid language plpgsql as
$$
    declare
        l_familia_id     uuid;

    begin
        select id into l_familia_id
        from core.familias
        where lower(nombre) = lower(p_familia)
        and orden_id = p_orden_id;

        return l_familia_id;
    end;
$$;

-- f_obtiene_genero_id
create or replace function core.f_obtiene_genero_id(in p_genero text, in p_familia_id uuid)
returns uuid language plpgsql as
$$
    declare
        l_genero_id     uuid;

    begin
        select id into l_genero_id
        from core.generos
        where lower(nombre) = lower(p_genero)
        and familia_id = p_familia_id;

        return l_genero_id;
    end;
$$;

-- f_obtiene_especie_id
create or replace function core.f_obtiene_especie_id(in p_especie text, in p_genero_id uuid)
returns uuid language plpgsql as
$$
    declare
        l_especie_id     uuid;

    begin
        select id into l_especie_id
        from core.especies
        where lower(nombre) = lower(p_especie)
        and genero_id = p_genero_id;

        return l_especie_id;
    end;
$$;

-- ----------------------------
-- Creación de Procedimientos
-- ----------------------------

-- p_inserta_frutas
create or replace procedure core.p_inserta_fruta(
                    in p_nombre         varchar,
                    in p_url_wikipedia  varchar,
                    in p_url_imagen     varchar
) language plpgsql as
$$
    begin
        -- Insertamos la fruta
        insert into core.frutas (nombre, url_wikipedia, url_imagen)
        values (p_nombre, p_url_wikipedia, p_url_imagen);
    end;
$$;

-- p_actualiza_fruta
create or replace procedure core.p_actualiza_fruta(
                    in p_id             uuid,
                    in p_nombre         varchar,
                    in p_url_wikipedia  varchar,
                    in p_url_imagen     varchar
) language plpgsql as
$$
    begin
        -- Actualizamos la fruta
        update core.frutas
        set nombre = p_nombre,
            url_wikipedia = p_url_wikipedia,
            url_imagen = p_url_imagen
        where id = p_id;
    end;
$$;

-- p_elimina_fruta
create or replace procedure core.p_elimina_fruta(
                    in p_id uuid
) language plpgsql as
$$
    declare
        l_total_registros integer :=0;
        l_especie_id uuid;
    begin
        -- Borramos informacion Nutricional
        select count(*) into l_total_registros
        from core.nutricion_frutas
        where fruta_id = p_id;

        -- Si hay registros, se borra de la tabla nutricion_frutas
        if(l_total_registros >0) then
            delete from core.nutricion_frutas
            where fruta_id = p_id;
        end if;   

        -- Borramos informacion taxonómica
        select count(*) into l_total_registros
        from core.taxonomia_frutas
        where fruta_id = p_id;

        -- Si hay registros, hay especie por borrar
        if(l_total_registros >0) then
            select especie_id into l_especie_id
            from core.taxonomia_frutas
            where fruta_id = p_id;

            delete from core.taxonomia_frutas
            where fruta_id = p_id;

            delete from especies
            where id = l_especie_id;
        end if;

        -- Borramos informacion de Producción
        select count(*) into l_total_registros
        from core.produccion_frutas
        where fruta_id = p_id;

        -- Si hay registros, se borra de la tabla produccion_frutas
        if(l_total_registros >0) then
            delete from core.produccion_frutas
            where fruta_id = p_id;
        end if;

        -- borramos la fruta
        delete from core.frutas
        where id = p_id;
    end;
$$;

-- p_inserta_nutricion_fruta
create or replace procedure core.p_inserta_nutricion_fruta(
                    in p_fruta_id       uuid,
                    in p_azucares       double precision,
                    in p_carbohidratos  double precision,
                    in p_grasas         double precision,
                    in p_proteinas      double precision
) language plpgsql as
$$
    declare
    l_total_registros integer :=0;

    begin
        -- identificamos si hay información nutricional previa
        select count(fruta_id) into l_total_registros
        from core.nutricion_frutas
        where fruta_id = p_fruta_id;

        -- Si no hay registros, se puede insertar
        if(l_total_registros=0) then
            -- Insertamos la información nutricional de la fruta
            insert into core.nutricion_frutas (fruta_id, azucares, carbohidratos, grasas, proteinas)
            values (p_fruta_id, p_azucares, p_carbohidratos, p_grasas, p_proteinas);
        end if;
    end;
$$;

-- p_actualiza_nutricion_fruta
create or replace procedure core.p_actualiza_nutricion_fruta(
                    in p_fruta_id       uuid,
                    in p_azucares       double precision,
                    in p_carbohidratos  double precision,
                    in p_grasas         double precision,
                    in p_proteinas      double precision
) language plpgsql as
$$
    declare
    l_total_registros integer :=0;

    begin
        -- identificamos si hay información nutricional previa
        select count(fruta_id) into l_total_registros
        from core.nutricion_frutas
        where fruta_id = p_fruta_id;

        -- Si hay registros, se puede actualizar
        if(l_total_registros!=0) then

            -- Actualizamos la información nutricional de la fruta
            update core.nutricion_frutas
            set
                azucares = p_azucares,
                carbohidratos = p_carbohidratos,
                grasas = p_grasas,
                proteinas = p_proteinas
                where fruta_id = p_fruta_id;
        end if;
    end;
$$;

-- p_elimina_nutricion_fruta
create or replace procedure core.p_elimina_nutricion_fruta(
                    in p_fruta_id       uuid
) language plpgsql as
$$
    declare
    l_total_registros integer :=0;

    begin
        -- identificamos si hay información nutricional previa
        select count(fruta_id) into l_total_registros
        from core.nutricion_frutas
        where fruta_id = p_fruta_id;

        -- Si hay registros, se puede eliminar
        if(l_total_registros!=0) then
            -- eliminamos la información nutricional de la fruta
            delete from nutricion_frutas
            where fruta_id = p_fruta_id;
        end if;
    end;
$$;

-- p_inserta_taxonomia_fruta
create or replace procedure core.p_inserta_taxonomia_fruta(
                    in p_fruta_id       uuid,
                    in p_reino          text,
                    in p_division       text,
                    in p_clase          text,
                    in p_orden          text,
                    in p_familia        text,
                    in p_genero         text,
                    in p_especie        text
) language plpgsql as
$$
    declare
        l_total_registros integer :=0;
        l_reino_id        uuid;
        l_division_id     uuid;
        l_clase_id        uuid;
        l_orden_id        uuid;
        l_familia_id      uuid;
        l_genero_id       uuid;
        l_especie_id      uuid;

    begin
        -- identificamos si hay información taxonomica previa
        select count(fruta_id) into l_total_registros
        from core.taxonomia_frutas
        where fruta_id = p_fruta_id;

        if l_total_registros != 0 then
            RAISE EXCEPTION 'Ya hay registros taxonómicos para la fruta';
        end if;

        -- Validamos que el reino esté registrado
        l_reino_id := core.f_obtiene_reino_id(p_reino);

        if l_reino_id is null then
            RAISE EXCEPTION 'El reino no es válido';
        end if;

        -- Validamos que la división esté registrada
        l_division_id := core.f_obtiene_division_id(p_division,l_reino_id);

        if l_division_id is null then
            RAISE EXCEPTION 'la división no es válida';
        end if;

        -- Validamos que la clase esté registrada
        l_clase_id := core.f_obtiene_clase_id(p_clase,l_division_id);

        if l_clase_id is null then
            RAISE EXCEPTION 'la clase no es válida';
        end if;

        -- Validamos que el orden esté registrado
        l_orden_id := core.f_obtiene_orden_id(p_orden,l_clase_id);

        if l_orden_id is null then
            RAISE EXCEPTION 'El orden no es válido';
        end if;

        -- Validamos que la familia esté registrada
        l_familia_id := core.f_obtiene_familia_id(p_familia,l_orden_id);

        if l_familia_id is null then
            RAISE EXCEPTION 'La familia no es válida';
        end if;

        -- Validamos que el género esté registrado
        l_genero_id := core.f_obtiene_genero_id(p_genero,l_familia_id);

        if l_genero_id is null then
            RAISE EXCEPTION 'El género no es válido';
        end if;

        -- Validamos que la especie esté registrada
        l_especie_id := core.f_obtiene_especie_id(p_especie,l_genero_id);

        if l_especie_id is null then
            RAISE EXCEPTION 'La especie no es válida';
        end if;

        -- Si todas las validaciones son correctas, insertamos
        insert into taxonomia_frutas (fruta_id, especie_id)
        values (p_fruta_id, l_especie_id);

    end;
$$;

-- p_actualiza_taxonomia_fruta
create or replace procedure core.p_actualiza_taxonomia_fruta(
                    in p_fruta_id       uuid,
                    in p_reino          text,
                    in p_division       text,
                    in p_clase          text,
                    in p_orden          text,
                    in p_familia        text,
                    in p_genero         text,
                    in p_especie        text
) language plpgsql as
$$
    declare
        l_total_registros integer :=0;
        l_reino_id        uuid;
        l_division_id     uuid;
        l_clase_id        uuid;
        l_orden_id        uuid;
        l_familia_id      uuid;
        l_genero_id       uuid;
        l_especie_id      uuid;

    begin
        -- identificamos si hay información taxonomica previa
        select count(fruta_id) into l_total_registros
        from core.taxonomia_frutas
        where fruta_id = p_fruta_id;

        if l_total_registros = 0 then
            RAISE EXCEPTION 'No hay registros taxonómicos para actualizar de la fruta';
        end if;

        -- Validamos que el reino esté registrado
        l_reino_id := core.f_obtiene_reino_id(p_reino);

        if l_reino_id is null then
            RAISE EXCEPTION 'El reino no es válido';
        end if;

        -- Validamos que la división esté registrada
        l_division_id := core.f_obtiene_division_id(p_division,l_reino_id);

        if l_division_id is null then
            RAISE EXCEPTION 'la división no es válida';
        end if;

        -- Validamos que la clase esté registrada
        l_clase_id := core.f_obtiene_clase_id(p_clase,l_division_id);

        if l_clase_id is null then
            RAISE EXCEPTION 'la clase no es válida';
        end if;

        -- Validamos que el orden esté registrado
        l_orden_id := core.f_obtiene_orden_id(p_orden,l_clase_id);

        if l_orden_id is null then
            RAISE EXCEPTION 'El orden no es válido';
        end if;

        -- Validamos que la familia esté registrada
        l_familia_id := core.f_obtiene_familia_id(p_familia,l_orden_id);

        if l_familia_id is null then
            RAISE EXCEPTION 'La familia no es válida';
        end if;

        -- Validamos que el género esté registrado
        l_genero_id := core.f_obtiene_genero_id(p_genero,l_familia_id);

        if l_genero_id is null then
            RAISE EXCEPTION 'El género no es válido';
        end if;

        -- Validamos que la especie esté registrada
        l_especie_id := core.f_obtiene_especie_id(p_especie,l_genero_id);

        if l_especie_id is null then
            RAISE EXCEPTION 'La especie no es válida';
        end if;

        -- Si todas las validaciones son correctas, actualizamos
        update core.taxonomia_frutas
            set especie_id = l_especie_id
        where fruta_id = p_fruta_id;

    end;
$$;


-- p_elimina_taxonomia_fruta
create or replace procedure core.p_elimina_taxonomia_fruta(
                    in p_fruta_id       uuid
) language plpgsql as
$$
    declare
    l_total_registros integer :=0;

    begin
        -- identificamos si hay información nutricional previa
        select count(fruta_id) into l_total_registros
        from core.taxonomia_frutas
        where fruta_id = p_fruta_id;

        -- Si hay registros, se puede eliminar
        if(l_total_registros!=0) then
            -- eliminamos la información de taxonomia de la fruta
            delete from taxonomia_frutas
            where fruta_id = p_fruta_id;
        end if;
    end;
$$;


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

-- Borrado de Procedimientos
drop procedure core.p_inserta_fruta;
drop procedure core.p_actualiza_fruta;
drop procedure core.p_elimina_fruta;

-- Borrado de vistas
drop view core.v_info_botanica;
drop view core.v_info_frutas;
drop view core.v_info_produccion_frutas;
drop view core.v_info_ubicaciones;
drop view core.v_info_botanica;

-- Borrado de tablas
drop table core.nutricion_frutas;
drop table core.produccion_frutas;
drop table core.taxonomia_frutas;

drop table core.especies;
drop table core.generos;
drop table core.familias;
drop table core.ordenes;
drop table core.clases;
drop table core.divisiones;
drop table core.reinos;

drop table core.frutas;

drop table core.municipios;
drop table core.departamentos;
drop table core.epocas;
drop table core.climas;

drop table core.divipola;