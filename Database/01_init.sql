USE [ShineGuacDB]
GO

/****** Object:  Table [dbo].[Connections] ******/

CREATE TABLE [dbo].[Connections](
	[ConnectionId] [uniqueidentifier] IDENTITY(1, 1) NOT NULL,
	[UserId] [varchar](80) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Type] [nvarchar](10) NOT NULL,
	[Image] [image] NULL,
	[Properties] [nvarchar](max) NOT NULL,
	[Tags] [nvarchar](100) NULL,
    CONSTRAINT PK_Connections_ConnectionId PRIMARY KEY ConnectionId
);
GO

