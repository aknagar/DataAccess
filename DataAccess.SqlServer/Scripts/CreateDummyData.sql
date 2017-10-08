/****** Object:  Table [dbo].[Item]    Script Date: 10/9/2017 12:31:22 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Item](
	[ItemID] [int] NOT NULL Identity(1,1),
	[PartNumber] [varchar](16) NOT NULL,
	[ItemName] [varchar](80) NOT NULL,
	[Description] [varchar](150) NOT NULL,
	) 

GO

SET ANSI_PADDING OFF
GO


Insert into Item
values('A-1000', 'Dummy Name 1', 'Dummy Description of Item')
GO

Insert into Item
values('A-1000', 'Dummy Name 2', 'Dummy Description of Item')
GO

Insert into Item
values('A-1000', 'Dummy Name 3', 'Dummy Description of Item')
GO

Insert into Item
values('A-1000', 'Dummy Name 4', 'Dummy Description of Item')
GO

Insert into Item
values('A-1000', 'Dummy Name 5', 'Dummy Description of Item')
GO