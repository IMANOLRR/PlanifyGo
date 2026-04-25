-- Crea la base de datos
CREATE DATABASE IF NOT EXISTS gestortareasdb;

-- Usa la base de datos
USE gestortareasdb;

-- Carga el contenido del archivo SQL exportado
SOURCE Gestor-de-Tareas\DataBase\gestortareasdb_users.sql;
SOURCE Gestor-de-Tareas\DataBase\gestortareasdb_tasks.sql;