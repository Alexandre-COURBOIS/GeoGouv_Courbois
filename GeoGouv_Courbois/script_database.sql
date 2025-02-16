USE [master]
GO
/****** Object:  Database [CesiGeoGouv]    Script Date: 09/02/2025 19:31:30 ******/
CREATE DATABASE [CesiGeoGouv]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'CesiGeoGouv', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\CesiGeoGouv.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'CesiGeoGouv_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\CesiGeoGouv_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [CesiGeoGouv] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CesiGeoGouv].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CesiGeoGouv] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CesiGeoGouv] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CesiGeoGouv] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CesiGeoGouv] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CesiGeoGouv] SET ARITHABORT OFF 
GO
ALTER DATABASE [CesiGeoGouv] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [CesiGeoGouv] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CesiGeoGouv] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CesiGeoGouv] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CesiGeoGouv] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CesiGeoGouv] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CesiGeoGouv] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CesiGeoGouv] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CesiGeoGouv] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CesiGeoGouv] SET  ENABLE_BROKER 
GO
ALTER DATABASE [CesiGeoGouv] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CesiGeoGouv] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CesiGeoGouv] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CesiGeoGouv] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CesiGeoGouv] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CesiGeoGouv] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [CesiGeoGouv] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CesiGeoGouv] SET RECOVERY FULL 
GO
ALTER DATABASE [CesiGeoGouv] SET  MULTI_USER 
GO
ALTER DATABASE [CesiGeoGouv] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CesiGeoGouv] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CesiGeoGouv] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CesiGeoGouv] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [CesiGeoGouv] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [CesiGeoGouv] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'CesiGeoGouv', N'ON'
GO
ALTER DATABASE [CesiGeoGouv] SET QUERY_STORE = ON
GO
ALTER DATABASE [CesiGeoGouv] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [CesiGeoGouv]
GO
CREATE LOGIN [IIS APPPOOL\GeoGouvPool] FROM WINDOWS;
GO
CREATE USER GeoGouvUser FOR LOGIN [IIS APPPOOL\GeoGouvPool];
GO
ALTER ROLE db_owner ADD MEMBER GeoGouvUser;
GO
/****** Object:  Table [dbo].[Departement]    Script Date: 09/02/2025 19:31:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Departement](
	[CodePostalDepartement] [char](3) NOT NULL,
	[NomDepartement] [char](30) NOT NULL,
	[ts] [timestamp] NOT NULL,
 CONSTRAINT [PK_TDEPARTEMENT] PRIMARY KEY CLUSTERED 
(
	[CodePostalDepartement] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ville]    Script Date: 09/02/2025 19:31:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ville](
	[CodeVilleCodePostalInsee] [int] IDENTITY(1,1) NOT NULL,
	[CodePostalDepartement] [char](3) NOT NULL,
	[Commune] [varchar](255) NOT NULL,
	[NomCommuneMinuscule] [varchar](255) NOT NULL,
	[CodePostal] [varchar](5) NOT NULL,
	[CodeInsee] [varchar](5) NOT NULL,
	[Ts] [timestamp] NOT NULL,
 CONSTRAINT [PK_tVilleCodePostalInsee] PRIMARY KEY CLUSTERED 
(
	[CodeVilleCodePostalInsee] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[GetAllDepartementsWithVilles]    Script Date: 09/02/2025 19:31:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GetAllDepartementsWithVilles]
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
GO
/****** Object:  StoredProcedure [dbo].[InsertVilleAndDepartement]    Script Date: 09/02/2025 19:31:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[InsertVilleAndDepartement]
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
GO
USE [master]
GO
ALTER DATABASE [CesiGeoGouv] SET  READ_WRITE 
GO
