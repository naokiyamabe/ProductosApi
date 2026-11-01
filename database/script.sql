-- =============================================================
-- Script de base de datos: API de Productos (MySQL 8)
-- Ejecutar completo en MySQL Workbench o en la terminal:
--   mysql -u root -p < database/script.sql
-- =============================================================

CREATE DATABASE IF NOT EXISTS productos_db
    CHARACTER SET utf8mb4
    COLLATE utf8mb4_unicode_ci;

USE productos_db;

DROP TABLE IF EXISTS productos;

CREATE TABLE productos (
    id              INT            NOT NULL AUTO_INCREMENT,
    nombre          VARCHAR(100)   NOT NULL,
    descripcion     VARCHAR(255)   NULL,
    categoria       VARCHAR(50)    NOT NULL,
    precio          DECIMAL(10,2)  NOT NULL DEFAULT 0.00,
    stock           INT            NOT NULL DEFAULT 0,
    fecha_creacion  DATETIME       NOT NULL DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (id),
    CONSTRAINT chk_precio CHECK (precio >= 0),
    CONSTRAINT chk_stock  CHECK (stock >= 0)
);

CREATE INDEX ix_productos_nombre    ON productos (nombre);
CREATE INDEX ix_productos_categoria ON productos (categoria);

-- Datos de ejemplo
INSERT INTO productos (nombre, descripcion, categoria, precio, stock) VALUES
('Laptop Lenovo IdeaPad 3', 'Laptop 15.6", Ryzen 5, 8 GB RAM, 512 GB SSD', 'Electrónica', 11999.00, 12),
('Mouse inalámbrico Logitech M185', 'Mouse óptico inalámbrico USB', 'Electrónica', 249.50, 40),
('Teclado mecánico Redragon Kumara', 'Teclado mecánico switches rojos, retroiluminado', 'Electrónica', 899.00, 25),
('Silla ergonómica', 'Silla de oficina con soporte lumbar', 'Muebles', 3499.00, 8),
('Escritorio de madera 120 cm', 'Escritorio para home office', 'Muebles', 2799.00, 5),
('Cuaderno profesional', 'Cuaderno de 100 hojas cuadro chico', 'Papelería', 45.00, 150),
('Bolígrafo gel azul (paquete 10)', 'Bolígrafos de tinta gel punto fino', 'Papelería', 89.90, 80);

SELECT * FROM productos;
