-- Creating table 'JobChecklists'
CREATE TABLE [dbo].[JobChecklists] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [dtEntered] datetime  NOT NULL,
    [dtDue] datetime  NOT NULL,
    [dtNotification] datetime  NOT NULL,
    [Description] nvarchar(250)  NOT NULL,
    [Remarks] nvarchar(250)  NULL,
    [RefId] int  NULL
);
GO
