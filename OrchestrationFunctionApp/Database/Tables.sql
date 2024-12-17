SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [msgqueue].[MsgCsvFile](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SequenceNumber] [bigint] NULL,
	[MessageID] [varchar](36) NULL,
	[EnqueuedTime] [datetime2](7) NULL,
	[Action] [varchar](250) NULL,
	[Payload] [nvarchar](max) NULL,
	[Processed] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [msgqueue].[MsgEvent](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SequenceNumber] [bigint] NULL,
	[MessageID] [varchar](36) NULL,
	[EnqueuedTime] [datetime2](7) NULL,
	[Action] [varchar](250) NULL,
	[Payload] [nvarchar](max) NULL,
	[Processed] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [msgqueue].[MsgInlineJson](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SequenceNumber] [bigint] NULL,
	[MessageID] [varchar](36) NULL,
	[EnqueuedTime] [datetime2](7) NULL,
	[Action] [varchar](250) NULL,
	[Payload] [nvarchar](max) NULL,
	[Processed] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [msgqueue].[MsgJsonFile](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SequenceNumber] [bigint] NULL,
	[MessageID] [varchar](36) NULL,
	[EnqueuedTime] [datetime2](7) NULL,
	[Action] [varchar](250) NULL,
	[Payload] [nvarchar](max) NULL,
	[Processed] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [msgqueue].[MsgXmlFile](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SequenceNumber] [bigint] NULL,
	[MessageID] [varchar](36) NULL,
	[EnqueuedTime] [datetime2](7) NULL,
	[Action] [varchar](250) NULL,
	[Payload] [nvarchar](max) NULL,
	[Processed] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [msgqueue].[MsgCsvFile] ADD  DEFAULT ((0)) FOR [Processed]
GO
ALTER TABLE [msgqueue].[MsgEvent] ADD  DEFAULT ((0)) FOR [Processed]
GO
ALTER TABLE [msgqueue].[MsgInlineJson] ADD  DEFAULT ((0)) FOR [Processed]
GO
ALTER TABLE [msgqueue].[MsgJsonFile] ADD  DEFAULT ((0)) FOR [Processed]
GO
ALTER TABLE [msgqueue].[MsgXmlFile] ADD  DEFAULT ((0)) FOR [Processed]
GO
