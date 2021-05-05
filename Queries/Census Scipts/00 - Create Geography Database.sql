USE [master]
GO

/****** Object:  Database [Geography]    Script Date: 9/1/2020 7:00:20 PM ******/
DROP DATABASE [Geography]
GO

/****** Object:  Database [Geography]    Script Date: 9/1/2020 7:00:20 PM ******/
CREATE DATABASE [Geography]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Geography', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\Geography.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Geography_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\Geography_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Geography].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [Geography] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [Geography] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [Geography] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [Geography] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [Geography] SET ARITHABORT OFF 
GO

ALTER DATABASE [Geography] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [Geography] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [Geography] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [Geography] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [Geography] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [Geography] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [Geography] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [Geography] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [Geography] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [Geography] SET  DISABLE_BROKER 
GO

ALTER DATABASE [Geography] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [Geography] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [Geography] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [Geography] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [Geography] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [Geography] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [Geography] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [Geography] SET RECOVERY FULL 
GO

ALTER DATABASE [Geography] SET  MULTI_USER 
GO

ALTER DATABASE [Geography] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [Geography] SET DB_CHAINING OFF 
GO

ALTER DATABASE [Geography] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [Geography] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO

ALTER DATABASE [Geography] SET DELAYED_DURABILITY = DISABLED 
GO

ALTER DATABASE [Geography] SET QUERY_STORE = OFF
GO

ALTER DATABASE [Geography] SET  READ_WRITE 
GO

USE [master]
GO

/****** Object:  Login [nodelogin]    Script Date: 8/29/2020 5:26:53 PM ******/
DROP LOGIN [nodelogin]
GO

/* For security reasons the login is created disabled and with a random password. */
/****** Object:  Login [nodelogin]    Script Date: 8/29/2020 5:26:53 PM ******/
CREATE LOGIN [nodelogin] WITH PASSWORD=N'SRX9WtlfdKwenaobtDfrSbHKimhTlnK8EZmXZRu4fQM=', DEFAULT_DATABASE=[Geography], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=ON, CHECK_POLICY=ON
GO

ALTER LOGIN [nodelogin] DISABLE
GO


USE [Geography]
GO

/****** Object:  User [nodelogin]    Script Date: 9/7/2020 7:00:02 AM ******/
DROP USER [nodelogin]
GO

/****** Object:  User [nodelogin]    Script Date: 9/7/2020 7:00:02 AM ******/
CREATE USER [nodelogin] FOR LOGIN [nodelogin] WITH DEFAULT_SCHEMA=[dbo]
GO




