CREATE TABLE [dbo].[Messages] (
    [Id]             UNIQUEIDENTIFIER CONSTRAINT [DF_Messages_Id] DEFAULT (newid()) NOT NULL,
    [Created]        DATETIME         CONSTRAINT [DF__Messages__Create__45F365D3] DEFAULT (getdate()) NOT NULL,
    [Modified]       DATETIME         CONSTRAINT [DF__Messages__Modifi__46E78A0C] DEFAULT (getdate()) NOT NULL,
    [RequestCode]    INT              NOT NULL,
    [RequestBody]    XML              NOT NULL,
    [RequestUser]    NVARCHAR (50)    NULL,
    [ExternalSystemId] INT            NOT NULL,
    [ResponseBody]   NVARCHAR (MAX)   NULL,
    [ResponseUser]   NVARCHAR (50)    NULL,
    [ResponseSystem] NVARCHAR (50)    NULL,
    [StatusCode]     INT              CONSTRAINT [DF__Messages__Status__47DBAE45] DEFAULT ((0)) NOT NULL,
    [ErrorCode]      INT              CONSTRAINT [DF__Messages__ErrorC__48CFD27E] DEFAULT ((0)) NULL,
    [ErrorMessage]   NVARCHAR (MAX)   NULL,
    [IsSyncRequest]  BIT              CONSTRAINT [DF__Messages__IsSync__49C3F6B7] DEFAULT ((1)) NOT NULL,
    [AttemptCount]   INT              CONSTRAINT [DF__Messages__Attemp__4AB81AF0] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK__Messages__3214EC0777BFA700] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Messages_MessageStatusCode] FOREIGN KEY ([StatusCode]) REFERENCES [dbo].[MessageStatusCode] ([Id]),
    CONSTRAINT [FK_Messages_RequestCodes] FOREIGN KEY ([RequestCode]) REFERENCES [dbo].[RequestCodes] ([Id]),
	CONSTRAINT [FK_Messages_ExternalSystems] FOREIGN KEY ([ExternalSystemId]) REFERENCES [dbo].[ExternalSystems]([Id])
);








GO
CREATE TRIGGER [dbo].[MessagesModified] ON [dbo].[Messages]
AFTER INSERT, UPDATE 
AS
  UPDATE m set [Modified] = GETDATE()
  FROM 
  dbo.Messages AS m
  INNER JOIN inserted
  AS i
  ON m.Id = i.Id;