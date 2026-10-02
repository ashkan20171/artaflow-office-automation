using AshkanOfficeAutomation.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace AshkanOfficeAutomation.Web.Data;
public class AppDbContext : IdentityDbContext<AppUser> {
 public AppDbContext(DbContextOptions<AppDbContext> o):base(o){}
 public DbSet<Department> Departments=>Set<Department>(); public DbSet<Letter> Letters=>Set<Letter>();
 public DbSet<Referral> Referrals=>Set<Referral>(); public DbSet<Attachment> Attachments=>Set<Attachment>();
 public DbSet<AuditEvent> AuditEvents=>Set<AuditEvent>();
 public DbSet<LetterTag> LetterTags=>Set<LetterTag>(); public DbSet<Notification> Notifications=>Set<Notification>();
 public DbSet<RegistrySequence> RegistrySequences=>Set<RegistrySequence>(); public DbSet<WorkflowAction> WorkflowActions=>Set<WorkflowAction>();
 public DbSet<Delegation> Delegations=>Set<Delegation>();
 public DbSet<WorkItem> WorkItems=>Set<WorkItem>(); public DbSet<Meeting> Meetings=>Set<Meeting>(); public DbSet<MeetingAttendee> MeetingAttendees=>Set<MeetingAttendee>();
 public DbSet<UserSessionRecord> UserSessionRecords=>Set<UserSessionRecord>(); public DbSet<RegistryEntry> RegistryEntries=>Set<RegistryEntry>();
 public DbSet<LetterComment> LetterComments=>Set<LetterComment>(); public DbSet<LetterBookmark> LetterBookmarks=>Set<LetterBookmark>();
 public DbSet<LetterTemplate> LetterTemplates=>Set<LetterTemplate>(); public DbSet<KnowledgeArticle> KnowledgeArticles=>Set<KnowledgeArticle>(); public DbSet<OrganizationSetting> OrganizationSettings=>Set<OrganizationSetting>();
 public DbSet<NotificationPreference> NotificationPreferences => Set<NotificationPreference>();

 protected override void OnModelCreating(ModelBuilder b){base.OnModelCreating(b); b.Entity<Letter>().HasIndex(x=>x.Number).IsUnique();
 b.Entity<LetterTag>().HasIndex(x=>new{x.LetterId,x.Name}).IsUnique();
 b.Entity<Notification>().HasIndex(x=>new{x.UserId,x.ReadAt});
 b.Entity<RegistrySequence>().HasIndex(x=>new{x.Year,x.Type}).IsUnique();
 b.Entity<Delegation>().HasIndex(x=>new{x.OwnerUserId,x.DelegateUserId,x.From,x.To});
 b.Entity<WorkItem>().HasIndex(x=>new{x.AssigneeId,x.Status,x.DueAt});
 b.Entity<MeetingAttendee>().HasIndex(x=>new{x.MeetingId,x.UserId}).IsUnique();
 b.Entity<UserSessionRecord>().HasIndex(x=>new{x.UserId,x.RevokedAt});
 b.Entity<RegistryEntry>().HasIndex(x=>x.LetterId).IsUnique();
 b.Entity<LetterComment>().HasIndex(x=>new{x.LetterId,x.CreatedAt});
 b.Entity<LetterBookmark>().HasIndex(x=>new{x.LetterId,x.UserId}).IsUnique();
 b.Entity<LetterTemplate>().HasIndex(x=>x.Name); b.Entity<KnowledgeArticle>().HasIndex(x=>new{x.IsPublished,x.Category});
  b.Entity<LetterTemplate>().Property(x=>x.BodyTemplate).HasColumnType("nvarchar(max)");

  b.Entity<NotificationPreference>().HasIndex(x=>x.UserId).IsUnique();
}
}
