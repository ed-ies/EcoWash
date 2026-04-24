------------------------------------------------------------
-- Tiendas
------------------------------------------------------------

INSERT INTO Tiendas (Nombre, Direccion, IsActive, Potencia) VALUES
('Plaza Mar 2', 'Av. de Denia', 'True', 3),
('CC/ Gran Via', 'C. José García Sellés', 'True', 3),
('Outlet San Vicente', 'C/ Alicante', 'True', 3),
('Repsol San Vicente', 'CR AC-2203', 'True', 3),
('Universidad de Alicante', 'C/ Alicante', 'True', 3.5),
('IES San Vicente', 'C. Lillo Juan', 'True', 3),
('Parque Lo Torrent', 'Carrer lo Torrent', 'False', 3),
('Plaza de los Luceros', 'Placa de los Luceros', 'False', 3),
('Parking Maisonnave', 'Av. Maisonnave', 'True', 3),
('CC/ Puerta de Alicante', 'Av. José María Hernández Mata','True', 3.5);
GO

------------------------------------------------------------
-- Lavadoras
------------------------------------------------------------
INSERT INTO Lavadoras (Tipo, IsOccupied, TiendaId) VALUES
(0, 'True', 1),
(1, 'False', 1),
(0, 'False', 2),
(1, 'False', 2),
(0, 'False', 3),
(1, 'False', 3),
(0, 'False', 4),
(1, 'False', 4),
(0, 'False', 5),
(1, 'False', 5),
(0, 'False', 6),
(1, 'False', 6),
(0, 'False', 7),
(1, 'False', 7),
(0, 'False', 8),
(1, 'False', 8),
(0, 'False', 9),
(1, 'False', 9),
(0, 'False', 10),
(1, 'False', 10);
GO
