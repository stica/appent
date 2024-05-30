CREATE TABLE [SecurityManagement].[UserGroupPolicy]
(
	[Id] INT NOT NULL IDENTITY (1, 1),
	[PolicyId] INT NOT NULL,
	[UserGroupId] INT NOT NULL,
	CONSTRAINT [PK_UserGroupPolicy] PRIMARY KEY CLUSTERED (Id ASC),
	CONSTRAINT FK_UserGroupPolicy_Policy FOREIGN KEY (PolicyId) REFERENCES [SecurityManagement].[Policy](Id),
    CONSTRAINT FK_UserGroupPolicy_UserGroup FOREIGN KEY (UserGroupId) REFERENCES [SecurityManagement].[UserGroup](Id)
)