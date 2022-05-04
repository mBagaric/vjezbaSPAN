IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Authors] (
    [Id] uniqueidentifier NOT NULL,
    [FirstName] nvarchar(50) NOT NULL,
    [LastName] nvarchar(50) NOT NULL,
    [DateOfBirth] datetimeoffset NOT NULL,
    [MainCategory] nvarchar(50) NOT NULL,
    CONSTRAINT [PK_Authors] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Courses] (
    [Id] uniqueidentifier NOT NULL,
    [Title] nvarchar(100) NOT NULL,
    [Description] nvarchar(1500) NULL,
    [AuthorId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_Courses] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Courses_Authors_AuthorId] FOREIGN KEY ([AuthorId]) REFERENCES [Authors] ([Id]) ON DELETE CASCADE
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'DateOfBirth', N'FirstName', N'LastName', N'MainCategory') AND [object_id] = OBJECT_ID(N'[Authors]'))
    SET IDENTITY_INSERT [Authors] ON;
INSERT INTO [Authors] ([Id], [DateOfBirth], [FirstName], [LastName], [MainCategory])
VALUES ('d28888e9-2ba9-473a-a40f-e38cb54f9b35', '1650-07-23T00:00:00.0000000+02:00', N'Berry', N'Griffin Beak Eldritch', N'Ships'),
('da2fd609-d754-4feb-8acd-c4f9ff13ba96', '1668-05-21T00:00:00.0000000+02:00', N'Nancy', N'Swashbuckler Rye', N'Rum'),
('2902b665-1190-4c70-9915-b9c2d7680450', '1701-12-16T00:00:00.0000000+01:00', N'Eli', N'Ivory Bones Sweet', N'Singing'),
('102b566b-ba1f-404c-b2df-e2cde39ade09', '1702-03-06T00:00:00.0000000+01:00', N'Arnold', N'The Unseen Stafford', N'Singing'),
('5b3621c0-7b12-4e80-9c8b-3398cba7ee05', '1690-11-23T00:00:00.0000000+01:00', N'Seabury', N'Toxic Reyson', N'Maps'),
('2aadd2df-7caf-45ab-9355-7f6332985a87', '1723-04-05T00:00:00.0000000+02:00', N'Rutherford', N'Fearless Cloven', N'General debauchery'),
('2ee49fe3-edf2-4f91-8409-3eb25ce6ca51', '1721-10-11T00:00:00.0000000+02:00', N'Atherton', N'Crow Ridley', N'Rum');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'DateOfBirth', N'FirstName', N'LastName', N'MainCategory') AND [object_id] = OBJECT_ID(N'[Authors]'))
    SET IDENTITY_INSERT [Authors] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AuthorId', N'Description', N'Title') AND [object_id] = OBJECT_ID(N'[Courses]'))
    SET IDENTITY_INSERT [Courses] ON;
INSERT INTO [Courses] ([Id], [AuthorId], [Description], [Title])
VALUES ('5b1c2b4d-48c7-402a-80c3-cc796ad49c6b', 'd28888e9-2ba9-473a-a40f-e38cb54f9b35', N'Commandeering a ship in rough waters isn''t easy.  Commandeering it without getting caught is even harder.  In this course you''ll learn how to sail away and avoid those pesky musketeers.', N'Commandeering a Ship Without Getting Caught'),
('d8663e5e-7494-4f81-8739-6e0de1bea7ee', 'd28888e9-2ba9-473a-a40f-e38cb54f9b35', N'In this course, the author provides tips to avoid, or, if needed, overthrow pirate mutiny.', N'Overthrowing Mutiny'),
('d173e20d-159e-4127-9ce9-b0ac2564ad97', 'da2fd609-d754-4feb-8acd-c4f9ff13ba96', N'Every good pirate loves rum, but it also has a tendency to get you into trouble.  In this course you''ll learn how to avoid that.  This new exclusive edition includes an additional chapter on how to run fast without falling while drunk.', N'Avoiding Brawls While Drinking as Much Rum as You Desire'),
('40ff5488-fdab-45b5-bc3a-14302d59869a', '2902b665-1190-4c70-9915-b9c2d7680450', N'In this course you''ll learn how to sing all-time favourite pirate songs without sounding like you actually know the words or how to hold a note.', N'Singalong Pirate Hits');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AuthorId', N'Description', N'Title') AND [object_id] = OBJECT_ID(N'[Courses]'))
    SET IDENTITY_INSERT [Courses] OFF;
GO

CREATE INDEX [IX_Courses_AuthorId] ON [Courses] ([AuthorId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20220504123807_init', N'5.0.16');
GO

COMMIT;
GO

