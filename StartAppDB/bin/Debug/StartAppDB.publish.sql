﻿/*
Deployment script for StartAppDB

This code was generated by a tool.
Changes to this file may cause incorrect behavior and will be lost if
the code is regenerated.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "StartAppDB"
:setvar DefaultFilePrefix "StartAppDB"
:setvar DefaultDataPath "C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\"
:setvar DefaultLogPath "C:\Program Files\Microsoft SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\"

GO
:on error exit
GO
/*
Detect SQLCMD mode and disable script execution if SQLCMD mode is not supported.
To re-enable the script after enabling SQLCMD mode, execute the following:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'SQLCMD mode must be enabled to successfully execute this script.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET ANSI_NULLS ON,
                ANSI_PADDING ON,
                ANSI_WARNINGS ON,
                ARITHABORT ON,
                CONCAT_NULL_YIELDS_NULL ON,
                QUOTED_IDENTIFIER ON,
                ANSI_NULL_DEFAULT ON,
                CURSOR_DEFAULT LOCAL 
            WITH ROLLBACK IMMEDIATE;
    END


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET PAGE_VERIFY NONE 
            WITH ROLLBACK IMMEDIATE;
    END


GO
ALTER DATABASE [$(DatabaseName)]
    SET TARGET_RECOVERY_TIME = 0 SECONDS 
    WITH ROLLBACK IMMEDIATE;


GO
IF EXISTS (SELECT 1
           FROM   [master].[dbo].[sysdatabases]
           WHERE  [name] = N'$(DatabaseName)')
    BEGIN
        ALTER DATABASE [$(DatabaseName)]
            SET QUERY_STORE (CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 367)) 
            WITH ROLLBACK IMMEDIATE;
    END


GO
PRINT N'Rename refactoring operation with key e48d8fe4-49a0-4e62-97ce-c75df1404d1d is skipped, element [dbo].[FK_Post_ToTable] (SqlForeignKeyConstraint) will not be renamed to [FK_Post_User]';


GO
PRINT N'Rename refactoring operation with key 095b52bc-a15b-4c27-b03a-c5561520f1cb is skipped, element [dbo].[Category].[Nam] (SqlSimpleColumn) will not be renamed to Name';


GO
PRINT N'Rename refactoring operation with key 1d651616-267f-4fa3-98c9-34c2e33509a2 is skipped, element [dbo].[Category].[PAr] (SqlSimpleColumn) will not be renamed to ParentCategoryId';


GO
PRINT N'Rename refactoring operation with key 16ab6cd4-db0c-4e84-8948-4790887c989a is skipped, element [dbo].[User].[SeekingJob] (SqlSimpleColumn) will not be renamed to SeekingJobs';


GO
PRINT N'Creating [dbo].[Category]...';


GO
CREATE TABLE [dbo].[Category] (
    [Id]               INT            IDENTITY (1, 1) NOT NULL,
    [Name]             NVARCHAR (100) NOT NULL,
    [ParentCategoryId] INT            NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[Comment]...';


GO
CREATE TABLE [dbo].[Comment] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [OwnerId]        INT            NOT NULL,
    [CommentdUserId] INT            NOT NULL,
    [Data]           NVARCHAR (MAX) NOT NULL,
    [IsPositive]     BIT            NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[Message]...';


GO
CREATE TABLE [dbo].[Message] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [SenderId]    INT            NOT NULL,
    [RecipientId] INT            NOT NULL,
    [Text]        NVARCHAR (MAX) NOT NULL,
    [Readed]      BIT            NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[Offer]...';


GO
CREATE TABLE [dbo].[Offer] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [PostId]     INT            NOT NULL,
    [UserId]     INT            NOT NULL,
    [IsAccepted] BIT            NOT NULL,
    [Text]       NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[Post]...';


GO
CREATE TABLE [dbo].[Post] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [Name]            NVARCHAR (50)  NOT NULL,
    [Description]     NVARCHAR (MAX) NOT NULL,
    [OwnerId]         INT            NOT NULL,
    [CategoryId]      INT            NULL,
    [Heading]         NVARCHAR (50)  NOT NULL,
    [NumberOfPersons] INT            NULL,
    [Address]         NVARCHAR (100) NULL,
    [NeedWorkers]     BIT            NOT NULL,
    [NeedToWork]      NCHAR (10)     NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[PostImage]...';


GO
CREATE TABLE [dbo].[PostImage] (
    [Id]     INT            IDENTITY (1, 1) NOT NULL,
    [Name]   NVARCHAR (100) NOT NULL,
    [Path]   NVARCHAR (MAX) NOT NULL,
    [UserId] INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[User]...';


GO
CREATE TABLE [dbo].[User] (
    [Id]              INT            IDENTITY (1, 1) NOT NULL,
    [FirstName]       NVARCHAR (50)  NOT NULL,
    [LastName]        NVARCHAR (50)  NOT NULL,
    [UserName]        NVARCHAR (50)  NOT NULL,
    [Password]        NVARCHAR (50)  NOT NULL,
    [Description]     NVARCHAR (MAX) NULL,
    [PhoneNumber]     NVARCHAR (50)  NULL,
    [ShowPhoneNumber] BIT            NOT NULL,
    [SeekingJobs]     BIT            NOT NULL,
    [OfferJobs]       BIT            NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[UserCategory]...';


GO
CREATE TABLE [dbo].[UserCategory] (
    [Id]                  INT IDENTITY (1, 1) NOT NULL,
    [UserId]              INT NOT NULL,
    [CategoryId]          INT NOT NULL,
    [ReceiveNotification] BIT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[UserImage]...';


GO
CREATE TABLE [dbo].[UserImage] (
    [Id]     INT            IDENTITY (1, 1) NOT NULL,
    [Name]   NVARCHAR (100) NOT NULL,
    [Path]   NVARCHAR (MAX) NOT NULL,
    [UserId] INT            NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
PRINT N'Creating [dbo].[FK_Category_Category]...';


GO
ALTER TABLE [dbo].[Category] WITH NOCHECK
    ADD CONSTRAINT [FK_Category_Category] FOREIGN KEY ([ParentCategoryId]) REFERENCES [dbo].[Category] ([Id]);


GO
PRINT N'Creating [dbo].[FK_Comment_User1]...';


GO
ALTER TABLE [dbo].[Comment] WITH NOCHECK
    ADD CONSTRAINT [FK_Comment_User1] FOREIGN KEY ([OwnerId]) REFERENCES [dbo].[User] ([Id]);


GO
PRINT N'Creating [dbo].[FK_Comment_User2]...';


GO
ALTER TABLE [dbo].[Comment] WITH NOCHECK
    ADD CONSTRAINT [FK_Comment_User2] FOREIGN KEY ([CommentdUserId]) REFERENCES [dbo].[User] ([Id]);


GO
PRINT N'Creating [dbo].[FK_Message_User1]...';


GO
ALTER TABLE [dbo].[Message] WITH NOCHECK
    ADD CONSTRAINT [FK_Message_User1] FOREIGN KEY ([SenderId]) REFERENCES [dbo].[User] ([Id]);


GO
PRINT N'Creating [dbo].[FK_Message_User2]...';


GO
ALTER TABLE [dbo].[Message] WITH NOCHECK
    ADD CONSTRAINT [FK_Message_User2] FOREIGN KEY ([RecipientId]) REFERENCES [dbo].[User] ([Id]);


GO
PRINT N'Creating [dbo].[FK_Offer_User]...';


GO
ALTER TABLE [dbo].[Offer] WITH NOCHECK
    ADD CONSTRAINT [FK_Offer_User] FOREIGN KEY ([PostId]) REFERENCES [dbo].[User] ([Id]);


GO
PRINT N'Creating [dbo].[FK_Offer_Post]...';


GO
ALTER TABLE [dbo].[Offer] WITH NOCHECK
    ADD CONSTRAINT [FK_Offer_Post] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Post] ([Id]);


GO
PRINT N'Creating [dbo].[FK_Post_User]...';


GO
ALTER TABLE [dbo].[Post] WITH NOCHECK
    ADD CONSTRAINT [FK_Post_User] FOREIGN KEY ([OwnerId]) REFERENCES [dbo].[User] ([Id]);


GO
PRINT N'Creating [dbo].[FK_PostImage_User]...';


GO
ALTER TABLE [dbo].[PostImage] WITH NOCHECK
    ADD CONSTRAINT [FK_PostImage_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Post] ([Id]);


GO
PRINT N'Creating [dbo].[FK_UserCategory_User]...';


GO
ALTER TABLE [dbo].[UserCategory] WITH NOCHECK
    ADD CONSTRAINT [FK_UserCategory_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]);


GO
PRINT N'Creating [dbo].[FK_USerCategory_Category]...';


GO
ALTER TABLE [dbo].[UserCategory] WITH NOCHECK
    ADD CONSTRAINT [FK_USerCategory_Category] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Category] ([Id]);


GO
PRINT N'Creating [dbo].[FK_UserImage_User]...';


GO
ALTER TABLE [dbo].[UserImage] WITH NOCHECK
    ADD CONSTRAINT [FK_UserImage_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id]);


GO
-- Refactoring step to update target server with deployed transaction logs

IF OBJECT_ID(N'dbo.__RefactorLog') IS NULL
BEGIN
    CREATE TABLE [dbo].[__RefactorLog] (OperationKey UNIQUEIDENTIFIER NOT NULL PRIMARY KEY)
    EXEC sp_addextendedproperty N'microsoft_database_tools_support', N'refactoring log', N'schema', N'dbo', N'table', N'__RefactorLog'
END
GO
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = 'e48d8fe4-49a0-4e62-97ce-c75df1404d1d')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('e48d8fe4-49a0-4e62-97ce-c75df1404d1d')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '095b52bc-a15b-4c27-b03a-c5561520f1cb')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('095b52bc-a15b-4c27-b03a-c5561520f1cb')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '1d651616-267f-4fa3-98c9-34c2e33509a2')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('1d651616-267f-4fa3-98c9-34c2e33509a2')
IF NOT EXISTS (SELECT OperationKey FROM [dbo].[__RefactorLog] WHERE OperationKey = '16ab6cd4-db0c-4e84-8948-4790887c989a')
INSERT INTO [dbo].[__RefactorLog] (OperationKey) values ('16ab6cd4-db0c-4e84-8948-4790887c989a')

GO

GO
PRINT N'Checking existing data against newly created constraints';


GO
USE [$(DatabaseName)];


GO
ALTER TABLE [dbo].[Category] WITH CHECK CHECK CONSTRAINT [FK_Category_Category];

ALTER TABLE [dbo].[Comment] WITH CHECK CHECK CONSTRAINT [FK_Comment_User1];

ALTER TABLE [dbo].[Comment] WITH CHECK CHECK CONSTRAINT [FK_Comment_User2];

ALTER TABLE [dbo].[Message] WITH CHECK CHECK CONSTRAINT [FK_Message_User1];

ALTER TABLE [dbo].[Message] WITH CHECK CHECK CONSTRAINT [FK_Message_User2];

ALTER TABLE [dbo].[Offer] WITH CHECK CHECK CONSTRAINT [FK_Offer_User];

ALTER TABLE [dbo].[Offer] WITH CHECK CHECK CONSTRAINT [FK_Offer_Post];

ALTER TABLE [dbo].[Post] WITH CHECK CHECK CONSTRAINT [FK_Post_User];

ALTER TABLE [dbo].[PostImage] WITH CHECK CHECK CONSTRAINT [FK_PostImage_User];

ALTER TABLE [dbo].[UserCategory] WITH CHECK CHECK CONSTRAINT [FK_UserCategory_User];

ALTER TABLE [dbo].[UserCategory] WITH CHECK CHECK CONSTRAINT [FK_USerCategory_Category];

ALTER TABLE [dbo].[UserImage] WITH CHECK CHECK CONSTRAINT [FK_UserImage_User];


GO
PRINT N'Update complete.';


GO
