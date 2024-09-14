USE [ShineGuacDB]
GO

/****** Object:  Table [dbo].[Connections] ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Connections](
	[ConnectionId] [uniqueidentifier] NOT NULL,
	[UserId] [varchar](80) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Type] [nvarchar](10) NOT NULL,
	[Image] [image] NULL,
	[Properties] [nvarchar](max) NOT NULL,
	[Tags] [nvarchar](100) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

