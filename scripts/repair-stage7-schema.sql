-- Emergency/manual schema repair for databases created by old EnsureCreated stages.
IF COL_LENGTH('Letters','ArchiveCode') IS NULL ALTER TABLE [Letters] ADD [ArchiveCode] nvarchar(max) NULL;
IF COL_LENGTH('Letters','ArchivedAt') IS NULL ALTER TABLE [Letters] ADD [ArchivedAt] datetime2 NULL;
IF COL_LENGTH('Letters','DueAt') IS NULL ALTER TABLE [Letters] ADD [DueAt] datetime2 NULL;
IF COL_LENGTH('Letters','RowVersion') IS NULL ALTER TABLE [Letters] ADD [RowVersion] rowversion NOT NULL;

IF COL_LENGTH('Referrals','IsDone') IS NULL ALTER TABLE [Referrals] ADD [IsDone] bit NOT NULL CONSTRAINT [DF_Referrals_IsDone] DEFAULT(0);
IF COL_LENGTH('Referrals','CompletedAt') IS NULL ALTER TABLE [Referrals] ADD [CompletedAt] datetime2 NULL;
