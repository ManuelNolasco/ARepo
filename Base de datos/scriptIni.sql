CREATE DATABASE UsuariosBd;
USE UsuariosBd;

CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `Usuarios` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `Nombres` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `Apellidos` varchar(100) CHARACTER SET utf8mb4 NOT NULL,
    `FechaNacimiento` datetime(6) NOT NULL,
    `Direccion` varchar(500) CHARACTER SET utf8mb4 NOT NULL,
    `Password` varchar(120) CHARACTER SET utf8mb4 NOT NULL,
    `Telefono` varchar(20) CHARACTER SET utf8mb4 NOT NULL,
    `Email` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `Estado` varchar(1) CHARACTER SET utf8mb4 NOT NULL,
    `FechaCreacion` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    `FechaModificacion` datetime(6) NULL,
    CONSTRAINT `PK_Usuarios` PRIMARY KEY (`Id`),
    CONSTRAINT `CK_Usuario_Estado` CHECK (Estado IN ('A', 'I'))
) CHARACTER SET=utf8mb4;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20251217163945_InitialCreate', '8.0.22');

COMMIT;

-- TRIGGERS

USE UsuariosBd;
/* Trigger para INSERT: llena FechaCreacion y FechaModificacion */
DELIMITER $$

CREATE TRIGGER TRG_SET_FECHAS_CREA_MOD_BEFORE_INSERT
BEFORE INSERT ON Usuarios
FOR EACH ROW
BEGIN
    SET NEW.FechaCreacion    = CURRENT_TIMESTAMP(6);
    SET NEW.FechaModificacion = CURRENT_TIMESTAMP(6);
END$$

/* Trigger para UPDATE: actualiza solo FechaModificacion */
CREATE TRIGGER TRG_SET_FECHA_MODIFICACION_BEFORE_UPDATE
BEFORE UPDATE ON Usuarios
FOR EACH ROW
BEGIN
    SET NEW.FechaModificacion = CURRENT_TIMESTAMP(6);
END$$

DELIMITER ;

-- Verifico
SHOW TRIGGERS LIKE 'Usuarios';