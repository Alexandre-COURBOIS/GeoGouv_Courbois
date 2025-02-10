USE [CesiGeoGouv]
GO
/****** Object:  StoredProcedure [dbo].[InsertVilleAndDepartement]    Script Date: 10/02/2025 13:42:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[InsertVilleAndDepartement]
    @CodePostalDepartement CHAR(3),
    @NomDepartement CHAR(30),
    @Commune VARCHAR(255),
    @NomCommuneMinuscule VARCHAR(255),
    @CodePostal VARCHAR(5),
    @CodeInsee VARCHAR(5)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM dbo.Departement WHERE CodePostalDepartement = @CodePostalDepartement)
    BEGIN
        INSERT INTO dbo.Departement (CodePostalDepartement, NomDepartement)
        VALUES (@CodePostalDepartement, @NomDepartement);
    END

    INSERT INTO dbo.Ville (CodePostalDepartement, Commune, NomCommuneMinuscule, CodePostal, CodeInsee)
    VALUES (@CodePostalDepartement, @Commune, @NomCommuneMinuscule, @CodePostal, @CodeInsee);
END;
