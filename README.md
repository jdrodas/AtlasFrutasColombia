# Atlas de Frutas Colombianas

For the english version of this readme file, scroll past the spanish version! ;-)

Aplicaciones usando C# y Python para demostrar conectividad a PostgreSQL y MongoDB, 
usando API REST y GraphQL


Por favor tenga presente:

- Es un proyecto académico que pretende evolucionar el aprendizaje de conceptos relacionados con bases de datos. 
**Esta no es una aplicación "lista para producción"**-

- Toda la información almacenada en las tablas y colecciones de las bases de datos es información pública disponible 
en Wikipedia. No es 100% confiable y no pretende serlo.

- Puede clonar el repositorio e inclusive proponer mejoras a travós de issues, pero no necesariamente serán 
implementadas en el tiempo asignado para el curso. Siempre será un trabajo en constante modificación.

## PoC
### [FrutasColombia_CS_PoC_Consola](https://github.com/jdrodas/AtlasFrutasColombia/tree/main/FrutasColombia_CS_PoC_Consola)
- Prueba de Concepto para validar funcionamiento del ORM Dapper, con base de datos PostgreSQL. Aplicación de **consola** en C# con framework .NET 8.x
- Para cada base de datos, se realizan las operaciones CRUD básicas sobre una entidad especófica.

## API
### [FrutasColombia_CS_REST_API](https://github.com/jdrodas/AtlasFrutasColombia/tree/main/FrutasColombia_CS_REST_API)
- WebAPI en C# con framework .NET 8.x implementando **Patrón Repositorio** con capa de persistencia de datos en PostgreSQL a través de Dapper como ORM, utilizando lógica almacenada para realizar operaciones CRUD.



# Atlas of Colombian Fruits

Apps using C# y Python to demo database connectivity using PostgreSQL and MongoDB,
using REST and GraphQL API

Please keep in mind:

- This is an academic project that aims to evolve the learning of concepts related to databases. 
**This is not a "production ready" application**.

- All information stored in the tables and collections of the databases is public information available 
on Wikipedia. It is not 100% reliable and it does not claim to be.

- You can clone the repository and even propose improvements through github issues, but they will not necessarily 
be implemented in the time allotted for the course. It will always be a Work in progress.

## PoC
### [FrutasColombia_CS_PoC_Consola](https://github.com/jdrodas/AtlasFrutasColombia/tree/main/FrutasColombia_CS_PoC_Consola)
- Proof of Concept (PoC) to learn how the Dapper ORM works, using PostgreSQL as Database. **Console** application using .NET 8.x framework and C#

- For each database, basic CRUD operations are performed on a specific entity.

## API
### [FrutasColombia_CS_REST_API](https://github.com/jdrodas/AtlasFrutasColombia/tree/main/FrutasColombia_CS_REST_API)
- .NET 8.x WebAPI implementing **Repository Pattern** with infrastructure persistence layer in PostgreSQL using Dapper as ORM, using stored procedures and functions to perform CRUD operations.
