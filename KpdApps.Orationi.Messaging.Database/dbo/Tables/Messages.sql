CREATE TABLE [dbo].[Messages] (
    [Id]             UNIQUEIDENTIFIER CONSTRAINT [DF_Messages_Id] DEFAULT (newid()) NOT NULL,
    [Created]        DATETIME         CONSTRAINT [DF__Messages__Create__45F365D3] DEFAULT (getdate()) NOT NULL,
    [Modified]       DATETIME         CONSTRAINT [DF__Messages__Modifi__46E78A0C] DEFAULT (getdate()) NOT NULL,
    [RequestCodeId]    INT              NOT NULL,
    [RequestBody]    NVARCHAR(MAX)              NOT NULL,
    [RequestUser]    NVARCHAR (50)    NULL,
    [ExternalSystemId] UNIQUEIDENTIFIER            NOT NULL,
    [ResponseBody]   NVARCHAR (MAX)   NULL,
    [ResponseUser]   NVARCHAR (50)    NULL,
    [ResponseSystem] NVARCHAR (50)    NULL,
    [StatusCode]     INT              CONSTRAINT [DF__Messages__Status__47DBAE45] DEFAULT ((0)) NOT NULL,
    [ErrorCode]      INT              CONSTRAINT [DF__Messages__ErrorC__48CFD27E] DEFAULT ((0)) NULL,
    [ErrorMessage]   NVARCHAR (MAX)   NULL,
    [IsSyncRequest]  BIT              CONSTRAINT [DF__Messages__IsSync__49C3F6B7] DEFAULT ((1)) NOT NULL,
    [AttemptCount]   INT              CONSTRAINT [DF__Messages__Attemp__4AB81AF0] DEFAULT ((0)) NOT NULL,
    [IsCallback] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK__Messages__3214EC0777BFA700] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Messages_MessageStatusCode] FOREIGN KEY ([StatusCode]) REFERENCES [dbo].[MessageStatusCode] ([Id]),
    CONSTRAINT [FK_Messages_RequestCodes] FOREIGN KEY ([RequestCodeId]) REFERENCES [dbo].[RequestCodes] ([Id]),
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
go

create trigger TG_Messages_Update_StatusCode on [dbo].[Messages]
after insert, update as if update([StatusCode])
begin
	update a1
		set [CanBeSend] = 1,
		[StatusCode] = 2000,
		[Modified] = getdate()
	from [dbo].[CallbackMessages] a1
	inner join [dbo].[Messages] a2 on a2.[Id] = a1.[MessageId]
	inner join [inserted] a3 on a3.[Id] = a2.[Id]
	where
		a2.[IsCallback] = 1
		and
		a3.[StatusCode] in (3000, 9000);
end;
