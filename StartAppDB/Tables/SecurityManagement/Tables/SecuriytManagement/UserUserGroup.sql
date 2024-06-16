CREATE TABLE [SecurityManagement].[UserUserGroup]
(
	[Id] INT NOT NULL IDENTITY (1, 1),
	[UserId] INT NOT NULL,
	[UserGroupId] INT NOT NULL,
	CONSTRAINT [PK_UserUserGroup] PRIMARY KEY CLUSTERED (Id ASC),
	CONSTRAINT FK_UserUserGroup_User FOREIGN KEY (UserId) REFERENCES [SecurityManagement].[User](Id),
    CONSTRAINT FK_UserUserGroup_UserGroup FOREIGN KEY (UserGroupId) REFERENCES [SecurityManagement].[UserGroup](Id)
)
