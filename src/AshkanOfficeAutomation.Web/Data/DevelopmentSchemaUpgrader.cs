using Microsoft.EntityFrameworkCore;

namespace AshkanOfficeAutomation.Web.Data;

/// <summary>
/// Development-only compatibility bridge for databases originally created with EnsureCreated.
/// Every statement is intentionally small, idempotent, and SQL Server specific.
/// Production deployments should use reviewed EF Core migrations.
/// </summary>
public static class DevelopmentSchemaUpgrader
{
    public static async Task UpgradeAsync(AppDbContext db)
    {
        if (!db.Database.IsSqlServer())
            return;

        await ExecuteAsync(db, """
IF OBJECT_ID(N'[Letters]', N'U') IS NOT NULL
BEGIN
    IF COL_LENGTH('Letters','ArchiveCode') IS NULL
        ALTER TABLE [Letters] ADD [ArchiveCode] nvarchar(max) NULL;
    IF COL_LENGTH('Letters','ArchivedAt') IS NULL
        ALTER TABLE [Letters] ADD [ArchivedAt] datetime2 NULL;
    IF COL_LENGTH('Letters','DueAt') IS NULL
        ALTER TABLE [Letters] ADD [DueAt] datetime2 NULL;
    IF COL_LENGTH('Letters','RowVersion') IS NULL
        ALTER TABLE [Letters] ADD [RowVersion] rowversion NOT NULL;
END;
""");

        await ExecuteAsync(db, """
IF OBJECT_ID(N'[Referrals]', N'U') IS NOT NULL
BEGIN
    IF COL_LENGTH('Referrals','IsDone') IS NULL
        ALTER TABLE [Referrals] ADD [IsDone] bit NOT NULL CONSTRAINT [DF_Referrals_IsDone] DEFAULT(0);
    IF COL_LENGTH('Referrals','CompletedAt') IS NULL
        ALTER TABLE [Referrals] ADD [CompletedAt] datetime2 NULL;
END;
""");

        await ExecuteAsync(db, """
IF OBJECT_ID(N'[RegistrySequences]', N'U') IS NULL
BEGIN
    CREATE TABLE [RegistrySequences](
        [Id] int IDENTITY(1,1) NOT NULL CONSTRAINT [PK_RegistrySequences] PRIMARY KEY,
        [Year] int NOT NULL,
        [Type] int NOT NULL,
        [LastNumber] int NOT NULL,
        [Prefix] nvarchar(20) NOT NULL
    );
    CREATE UNIQUE INDEX [IX_RegistrySequences_Year_Type]
        ON [RegistrySequences]([Year],[Type]);
END;
""");

        await ExecuteAsync(db, """
IF OBJECT_ID(N'[WorkflowActions]', N'U') IS NULL
BEGIN
    CREATE TABLE [WorkflowActions](
        [Id] bigint IDENTITY(1,1) NOT NULL CONSTRAINT [PK_WorkflowActions] PRIMARY KEY,
        [LetterId] bigint NOT NULL,
        [UserId] nvarchar(max) NOT NULL,
        [Action] nvarchar(60) NOT NULL,
        [Note] nvarchar(1000) NULL,
        [At] datetime2 NOT NULL,
        CONSTRAINT [FK_WorkflowActions_Letters_LetterId]
            FOREIGN KEY([LetterId]) REFERENCES [Letters]([Id]) ON DELETE CASCADE
    );
END;
""");

        await ExecuteAsync(db, """
IF OBJECT_ID(N'[Delegations]', N'U') IS NULL
BEGIN
    CREATE TABLE [Delegations](
        [Id] bigint IDENTITY(1,1) NOT NULL CONSTRAINT [PK_Delegations] PRIMARY KEY,
        [OwnerUserId] nvarchar(max) NOT NULL,
        [DelegateUserId] nvarchar(max) NOT NULL,
        [From] datetime2 NOT NULL,
        [To] datetime2 NOT NULL,
        [IsActive] bit NOT NULL
    );
END;
""");

        await ExecuteAsync(db, """
IF OBJECT_ID(N'[LetterTags]', N'U') IS NULL
BEGIN
    CREATE TABLE [LetterTags](
        [Id] bigint IDENTITY(1,1) NOT NULL CONSTRAINT [PK_LetterTags] PRIMARY KEY,
        [LetterId] bigint NOT NULL,
        [Name] nvarchar(60) NOT NULL,
        CONSTRAINT [FK_LetterTags_Letters_LetterId]
            FOREIGN KEY([LetterId]) REFERENCES [Letters]([Id]) ON DELETE CASCADE
    );
    CREATE UNIQUE INDEX [IX_LetterTags_LetterId_Name]
        ON [LetterTags]([LetterId],[Name]);
END;
""");

        await ExecuteAsync(db, """
IF OBJECT_ID(N'[Notifications]', N'U') IS NULL
BEGIN
    CREATE TABLE [Notifications](
        [Id] bigint IDENTITY(1,1) NOT NULL CONSTRAINT [PK_Notifications] PRIMARY KEY,
        [UserId] nvarchar(max) NOT NULL,
        [Title] nvarchar(160) NOT NULL,
        [Message] nvarchar(800) NOT NULL,
        [Link] nvarchar(max) NULL,
        [CreatedAt] datetime2 NOT NULL,
        [ReadAt] datetime2 NULL
    );
END;
""");

        await ExecuteAsync(db, """
IF OBJECT_ID(N'[WorkItems]', N'U') IS NULL
BEGIN
    CREATE TABLE [WorkItems](
        [Id] bigint IDENTITY(1,1) NOT NULL CONSTRAINT [PK_WorkItems] PRIMARY KEY,
        [Title] nvarchar(220) NOT NULL,
        [Description] nvarchar(1200) NULL,
        [AssigneeId] nvarchar(max) NOT NULL,
        [CreatorId] nvarchar(max) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [DueAt] datetime2 NULL,
        [Priority] int NOT NULL,
        [Status] int NOT NULL,
        [LetterId] bigint NULL
    );
END;
""");

        await ExecuteAsync(db, """
IF OBJECT_ID(N'[Meetings]', N'U') IS NULL
BEGIN
    CREATE TABLE [Meetings](
        [Id] bigint IDENTITY(1,1) NOT NULL CONSTRAINT [PK_Meetings] PRIMARY KEY,
        [Title] nvarchar(220) NOT NULL,
        [StartsAt] datetime2 NOT NULL,
        [EndsAt] datetime2 NOT NULL,
        [Location] nvarchar(220) NULL,
        [OrganizerId] nvarchar(max) NOT NULL,
        [Agenda] nvarchar(4000) NULL
    );
END;
""");

        await ExecuteAsync(db, """
IF OBJECT_ID(N'[MeetingAttendees]', N'U') IS NULL
BEGIN
    CREATE TABLE [MeetingAttendees](
        [Id] bigint IDENTITY(1,1) NOT NULL CONSTRAINT [PK_MeetingAttendees] PRIMARY KEY,
        [MeetingId] bigint NOT NULL,
        [UserId] nvarchar(max) NOT NULL,
        CONSTRAINT [FK_MeetingAttendees_Meetings_MeetingId]
            FOREIGN KEY([MeetingId]) REFERENCES [Meetings]([Id]) ON DELETE CASCADE
    );
    CREATE UNIQUE INDEX [IX_MeetingAttendees_MeetingId_UserId]
        ON [MeetingAttendees]([MeetingId],[UserId]);
END;
""");

        await ExecuteAsync(db, """
IF OBJECT_ID(N'[UserSessionRecords]', N'U') IS NULL
BEGIN
    CREATE TABLE [UserSessionRecords](
        [Id] bigint IDENTITY(1,1) NOT NULL CONSTRAINT [PK_UserSessionRecords] PRIMARY KEY,
        [UserId] nvarchar(max) NOT NULL,
        [SessionKey] nvarchar(120) NOT NULL,
        [IpAddress] nvarchar(80) NULL,
        [UserAgent] nvarchar(500) NULL,
        [SignedInAt] datetime2 NOT NULL,
        [LastSeenAt] datetime2 NOT NULL,
        [RevokedAt] datetime2 NULL
    );
END;
""");

        await ExecuteAsync(db, """
IF OBJECT_ID(N'[RegistryEntries]', N'U') IS NULL
BEGIN
    CREATE TABLE [RegistryEntries](
        [Id] bigint IDENTITY(1,1) NOT NULL CONSTRAINT [PK_RegistryEntries] PRIMARY KEY,
        [LetterId] bigint NOT NULL,
        [Book] nvarchar(100) NOT NULL,
        [ExternalNumber] nvarchar(100) NULL,
        [ExternalDate] datetime2 NULL,
        [RegisteredById] nvarchar(max) NOT NULL,
        [RegisteredAt] datetime2 NOT NULL,
        CONSTRAINT [FK_RegistryEntries_Letters_LetterId]
            FOREIGN KEY([LetterId]) REFERENCES [Letters]([Id]) ON DELETE CASCADE
    );
    CREATE UNIQUE INDEX [IX_RegistryEntries_LetterId]
        ON [RegistryEntries]([LetterId]);
END;
""");

        await ExecuteAsync(db, """
IF OBJECT_ID(N'[NotificationPreferences]', N'U') IS NULL
BEGIN
    CREATE TABLE [NotificationPreferences](
        [Id] bigint IDENTITY(1,1) NOT NULL CONSTRAINT [PK_NotificationPreferences] PRIMARY KEY,
        [UserId] nvarchar(450) NOT NULL,
        [SlaAlerts] bit NOT NULL CONSTRAINT [DF_NotificationPreferences_SlaAlerts] DEFAULT(1),
        [ReferralAlerts] bit NOT NULL CONSTRAINT [DF_NotificationPreferences_ReferralAlerts] DEFAULT(1),
        [MeetingAlerts] bit NOT NULL CONSTRAINT [DF_NotificationPreferences_MeetingAlerts] DEFAULT(1),
        [TaskAlerts] bit NOT NULL CONSTRAINT [DF_NotificationPreferences_TaskAlerts] DEFAULT(1),
        [SecurityAlerts] bit NOT NULL CONSTRAINT [DF_NotificationPreferences_SecurityAlerts] DEFAULT(1),
        [DailyDigest] bit NOT NULL CONSTRAINT [DF_NotificationPreferences_DailyDigest] DEFAULT(0)
    );
    CREATE UNIQUE INDEX [IX_NotificationPreferences_UserId]
        ON [NotificationPreferences]([UserId]);
END;
""");

        // Stage 9 collaboration tables: only create them when the corresponding entity tables do not exist.
        await ExecuteAsync(db, """
IF OBJECT_ID(N'[LetterComments]', N'U') IS NULL
BEGIN
    CREATE TABLE [LetterComments](
        [Id] bigint IDENTITY(1,1) NOT NULL CONSTRAINT [PK_LetterComments] PRIMARY KEY,
        [LetterId] bigint NOT NULL,
        [UserId] nvarchar(max) NOT NULL,
        [Text] nvarchar(1500) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        CONSTRAINT [FK_LetterComments_Letters_LetterId]
            FOREIGN KEY([LetterId]) REFERENCES [Letters]([Id]) ON DELETE CASCADE
    );
END;
""");

        await ExecuteAsync(db, """
IF OBJECT_ID(N'[LetterBookmarks]', N'U') IS NULL
BEGIN
    CREATE TABLE [LetterBookmarks](
        [Id] bigint IDENTITY(1,1) NOT NULL CONSTRAINT [PK_LetterBookmarks] PRIMARY KEY,
        [LetterId] bigint NOT NULL,
        [UserId] nvarchar(450) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        CONSTRAINT [FK_LetterBookmarks_Letters_LetterId]
            FOREIGN KEY([LetterId]) REFERENCES [Letters]([Id]) ON DELETE CASCADE
    );
    CREATE UNIQUE INDEX [IX_LetterBookmarks_LetterId_UserId]
        ON [LetterBookmarks]([LetterId],[UserId]);
END;
""");

        // Stage 10 tables. Keep text columns compatible with the EF model.
        await ExecuteAsync(db, """
IF OBJECT_ID(N'[LetterTemplates]', N'U') IS NULL
BEGIN
    CREATE TABLE [LetterTemplates](
        [Id] bigint IDENTITY(1,1) NOT NULL CONSTRAINT [PK_LetterTemplates] PRIMARY KEY,
        [Name] nvarchar(160) NOT NULL,
        [SubjectTemplate] nvarchar(300) NULL,
        [BodyTemplate] nvarchar(max) NULL,
        [Type] int NOT NULL,
        [IsActive] bit NOT NULL
    );
END;
ELSE IF COL_LENGTH('LetterTemplates','BodyTemplate') IS NOT NULL
BEGIN
    ALTER TABLE [LetterTemplates] ALTER COLUMN [BodyTemplate] nvarchar(max) NULL;
END;
""");

        await ExecuteAsync(db, """
IF OBJECT_ID(N'[KnowledgeArticles]', N'U') IS NULL
BEGIN
    CREATE TABLE [KnowledgeArticles](
        [Id] bigint IDENTITY(1,1) NOT NULL CONSTRAINT [PK_KnowledgeArticles] PRIMARY KEY,
        [Title] nvarchar(220) NOT NULL,
        [Category] nvarchar(100) NULL,
        [Body] nvarchar(max) NOT NULL,
        [CreatedById] nvarchar(max) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        [UpdatedAt] datetime2 NULL,
        [IsPublished] bit NOT NULL
    );
END;
""");


        await ExecuteAsync(db, """
IF OBJECT_ID(N'[OrganizationSettings]', N'U') IS NULL
BEGIN
    CREATE TABLE [OrganizationSettings](
        [Id] int IDENTITY(1,1) NOT NULL CONSTRAINT [PK_OrganizationSettings] PRIMARY KEY,
        [OrganizationName] nvarchar(200) NOT NULL CONSTRAINT [DF_OrganizationSettings_Name] DEFAULT(N'Ashkan Office Automation'),
        [LetterPrefix] nvarchar(20) NOT NULL CONSTRAINT [DF_OrganizationSettings_Prefix] DEFAULT(N'ASH'),
        [DefaultSlaHours] int NOT NULL CONSTRAINT [DF_OrganizationSettings_Sla] DEFAULT(48),
        [SupportEmail] nvarchar(200) NULL,
        [SupportPhone] nvarchar(60) NULL
    );
END;
""");

        await ExecuteAsync(db, """
IF OBJECT_ID(N'[OrganizationSettings]', N'U') IS NOT NULL
BEGIN
    IF COL_LENGTH('OrganizationSettings','OrganizationName') IS NULL
        ALTER TABLE [OrganizationSettings] ADD [OrganizationName] nvarchar(200) NOT NULL
            CONSTRAINT [DF_OrganizationSettings_Name_Compat] DEFAULT(N'Ashkan Office Automation');
    IF COL_LENGTH('OrganizationSettings','LetterPrefix') IS NULL
        ALTER TABLE [OrganizationSettings] ADD [LetterPrefix] nvarchar(20) NOT NULL
            CONSTRAINT [DF_OrganizationSettings_Prefix_Compat] DEFAULT(N'ASH');
    IF COL_LENGTH('OrganizationSettings','DefaultSlaHours') IS NULL
        ALTER TABLE [OrganizationSettings] ADD [DefaultSlaHours] int NOT NULL
            CONSTRAINT [DF_OrganizationSettings_Sla_Compat] DEFAULT(48);
    IF COL_LENGTH('OrganizationSettings','SupportEmail') IS NULL
        ALTER TABLE [OrganizationSettings] ADD [SupportEmail] nvarchar(200) NULL;
    IF COL_LENGTH('OrganizationSettings','SupportPhone') IS NULL
        ALTER TABLE [OrganizationSettings] ADD [SupportPhone] nvarchar(60) NULL;
END;
""");
    

        await ExecuteAsync(db, """
IF OBJECT_ID(N'[OrganizationSettings]', N'U') IS NOT NULL
BEGIN
    UPDATE [OrganizationSettings]
       SET [OrganizationName]=COALESCE(NULLIF([OrganizationName],N''),N'Ashkan Office Automation'),
           [LetterPrefix]=COALESCE(NULLIF([LetterPrefix],N''),N'ASH'),
           [DefaultSlaHours]=CASE WHEN [DefaultSlaHours] IS NULL OR [DefaultSlaHours] <= 0 THEN 48 ELSE [DefaultSlaHours] END;

    ALTER TABLE [OrganizationSettings] ALTER COLUMN [OrganizationName] nvarchar(200) NOT NULL;
    ALTER TABLE [OrganizationSettings] ALTER COLUMN [LetterPrefix] nvarchar(20) NOT NULL;
    ALTER TABLE [OrganizationSettings] ALTER COLUMN [DefaultSlaHours] int NOT NULL;
    ALTER TABLE [OrganizationSettings] ALTER COLUMN [SupportEmail] nvarchar(200) NULL;
    ALTER TABLE [OrganizationSettings] ALTER COLUMN [SupportPhone] nvarchar(60) NULL;
END;
""");
}

    private static Task ExecuteAsync(AppDbContext db, string sql) =>
        db.Database.ExecuteSqlRawAsync(sql);
}
