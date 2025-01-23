IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'MsgEmptyEvent' AND TABLE_SCHEMA = 'msgqueue')
    DROP TABLE [msgqueue].[MsgEmptyEvent]
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'MsgFlatFile' AND TABLE_SCHEMA = 'msgqueue')
    DROP TABLE [msgqueue].[MsgFlatFile]
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'MsgInlineJson' AND TABLE_SCHEMA = 'msgqueue')
    DROP TABLE [msgqueue].[MsgInlineJson]
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'MsgJsonFile' AND TABLE_SCHEMA = 'msgqueue')
    DROP TABLE [msgqueue].[MsgJsonFile]
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'MsgXmlFile' AND TABLE_SCHEMA = 'msgqueue')
    DROP TABLE [msgqueue].[MsgXmlFile]
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'PipelineDispatcerSemaphore' AND TABLE_SCHEMA = 'msgqueue')
    DROP TABLE [msgqueue].[PipelineDispatcerSemaphore]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [msgqueue].[MsgFlatFile](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SequenceNumber] [bigint] NULL,
	[MessageID] [varchar](36) NULL,
	[EnqueuedTime] [datetime2](7) NULL,
	[PipelineAction] [varchar](250) NULL,
	[OrchestrationAction] [varchar](250) NULL,
	[Payload] [nvarchar](max) NULL,
	[Processed] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] 
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [msgqueue].[MsgEmptyEvent](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SequenceNumber] [bigint] NULL,
	[MessageID] [varchar](36) NULL,
	[EnqueuedTime] [datetime2](7) NULL,
	[PipelineAction] [varchar](250) NULL,
	[OrchestrationAction] [varchar](250) NULL,
	[Payload] [nvarchar](max) NULL,
	[Processed] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] 
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
	[PipelineAction] [varchar](250) NULL,
	[OrchestrationAction] [varchar](250) NULL,
	[Payload] [nvarchar](max) NULL,
	[Processed] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] 
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
	[PipelineAction] [varchar](250) NULL,
	[OrchestrationAction] [varchar](250) NULL,
	[Payload] [nvarchar](max) NULL,
	[Processed] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] 
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
	[PipelineAction] [varchar](250) NULL,
	[OrchestrationAction] [varchar](250) NULL,
	[Payload] [nvarchar](max) NULL,
	[Processed] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] 
GO

ALTER TABLE [msgqueue].[MsgFlatFile] ADD  DEFAULT ((0)) FOR [Processed]
GO
ALTER TABLE [msgqueue].[MsgEmptyEvent] ADD  DEFAULT ((0)) FOR [Processed]
GO
ALTER TABLE [msgqueue].[MsgInlineJson] ADD  DEFAULT ((0)) FOR [Processed]
GO
ALTER TABLE [msgqueue].[MsgJsonFile] ADD  DEFAULT ((0)) FOR [Processed]
GO
ALTER TABLE [msgqueue].[MsgXmlFile] ADD  DEFAULT ((0)) FOR [Processed]
GO

CREATE TABLE [msgqueue].[PipelineDispatcerSemaphore](
	[Id] [int] UNIQUE NOT NULL,
	[QueueName] [varchar](36) NOT NULL,
	[IsPipelineDispatcerEnabled] BIT NOT NULL
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

INSERT INTO [msgqueue].[PipelineDispatcerSemaphore] 
VALUES
	(1, 'sbq-event-empty', 0),
	(2, 'sbq-event-json-inline', 0),
	(3, 'sbq-event-json-file', 0),
	(4, 'sbq-event-flat-file', 0),
	(5, 'sbq-event-xml-file', 0)

GO
