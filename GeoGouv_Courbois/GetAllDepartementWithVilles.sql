USE [CesiGeoGouv]
GO
/****** Object:  StoredProcedure [dbo].[GetAllDepartementsWithVilles]    Script Date: 10/02/2025 13:34:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[GetAllDepartementsWithVilles]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        d.CodePostalDepartement,
        d.NomDepartement,
        v.CodeVilleCodePostalInsee,
        v.Commune,
        v.NomCommuneMinuscule,
        v.CodePostal,
        v.CodeInsee
    FROM dbo.Departement d
    LEFT JOIN dbo.Ville v ON d.CodePostalDepartement = v.CodePostalDepartement
    ORDER BY d.CodePostalDepartement, v.Commune;
END;
