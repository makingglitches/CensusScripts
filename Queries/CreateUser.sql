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


