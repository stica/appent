CREATE TABLE [Events].[EventAttendee]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[EventId] INT NOT NULL,
	[UserId] INT NOT NULL,
	CONSTRAINT FK_EventAttendee_Event FOREIGN KEY (EventId) REFERENCES [Events].[Event](Id),
	CONSTRAINT FK_EventAttendee_User FOREIGN KEY (UserId) REFERENCES [SecurityManagement].[User](Id),

)
