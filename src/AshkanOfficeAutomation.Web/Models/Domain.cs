using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
namespace AshkanOfficeAutomation.Web.Models;

public enum LetterType { Incoming=1, Outgoing=2, Internal=3 }
public enum LetterStatus { Draft=1, Registered=2, InWorkflow=3, Completed=4, Archived=5 }
public enum Priority { Normal=1, Important=2, Urgent=3, Immediate=4 }

public class AppUser : IdentityUser {
 [MaxLength(120)] public string FullName {get;set;}="";
 public int? DepartmentId {get;set;}
 public Department? Department {get;set;}
 public bool IsActive {get;set;}=true;
}
public class Department {
 public int Id {get;set;} [MaxLength(120)] public string Name {get;set;}="";
 public int? ParentId {get;set;} public Department? Parent {get;set;}
}
public class Letter {
 public long Id {get;set;}
 [MaxLength(40)] public string Number {get;set;}="";
 [Required,MaxLength(500)] public string Subject {get;set;}="";
 public string Body {get;set;}="";
 public LetterType Type {get;set;}
 public LetterStatus Status {get;set;}=LetterStatus.Draft;
 public Priority Priority {get;set;}=Priority.Normal;
 public DateTime CreatedAt {get;set;}=DateTime.UtcNow;
 public DateTime? RegisteredAt {get;set;}
 public string CreatorId {get;set;}="";
 public AppUser? Creator {get;set;}
 [MaxLength(200)] public string? Sender {get;set;}
 [MaxLength(200)] public string? Receiver {get;set;}
 public bool IsConfidential {get;set;}
 public ICollection<Referral> Referrals {get;set;}=new List<Referral>();
 public ICollection<Attachment> Attachments {get;set;}=new List<Attachment>();
 public ICollection<LetterTag> Tags {get;set;}=new List<LetterTag>();
 public DateTime? DueAt {get;set;}
 public DateTime? ArchivedAt {get;set;}
 public string? ArchiveCode {get;set;}
 [Timestamp] public byte[]? RowVersion {get;set;}
}
public class Referral {
 public long Id {get;set;} public long LetterId {get;set;} public Letter? Letter {get;set;}
 public string FromUserId {get;set;}=""; public string ToUserId {get;set;}="";
 [MaxLength(1000)] public string Note {get;set;}="";
 public DateTime CreatedAt {get;set;}=DateTime.UtcNow; public DateTime? SeenAt {get;set;} public DateTime? DoneAt {get;set;}
 public bool IsDone {get;set;} public DateTime? CompletedAt {get;set;}
}
public class Attachment {
 public long Id {get;set;} public long LetterId {get;set;} public Letter? Letter {get;set;}
 public string OriginalName {get;set;}=""; public string StoredName {get;set;}="";
 public long Size {get;set;} public string ContentType {get;set;}="";
}
public class LetterTag {
 public long Id {get;set;} public long LetterId {get;set;} public Letter? Letter {get;set;}
 [MaxLength(60)] public string Name {get;set;}="";
}
public class Notification {
 public long Id {get;set;} public string UserId {get;set;}="";
 [MaxLength(160)] public string Title {get;set;}=""; [MaxLength(800)] public string Message {get;set;}="";
 public string? Link {get;set;} public DateTime CreatedAt {get;set;}=DateTime.UtcNow; public DateTime? ReadAt {get;set;}
}
public class AuditEvent {
 public long Id {get;set;} public string UserId {get;set;}=""; public string Action {get;set;}="";
 public string Entity {get;set;}=""; public string EntityId {get;set;}=""; public string? Ip {get;set;}
 public DateTime At {get;set;}=DateTime.UtcNow;
}

public class RegistrySequence {
 public int Id {get;set;} public int Year {get;set;} public LetterType Type {get;set;} public int LastNumber {get;set;}
 [MaxLength(20)] public string Prefix {get;set;}="ASH";
}
public class WorkflowAction {
 public long Id {get;set;} public long LetterId {get;set;} public Letter? Letter {get;set;}
 public string UserId {get;set;}=""; [MaxLength(60)] public string Action {get;set;}="";
 [MaxLength(1000)] public string? Note {get;set;} public DateTime At {get;set;}=DateTime.UtcNow;
}
public class Delegation {
 public long Id {get;set;} public string OwnerUserId {get;set;}=""; public string DelegateUserId {get;set;}="";
 public DateTime From {get;set;} public DateTime To {get;set;} public bool IsActive {get;set;}=true;
}

public enum WorkItemStatus { Open=1, InProgress=2, Done=3, Cancelled=4 }
public class WorkItem {
 public long Id {get;set;} [MaxLength(220)] public string Title {get;set;}="";
 [MaxLength(1200)] public string? Description {get;set;} public string AssigneeId {get;set;}="";
 public string CreatorId {get;set;}=""; public DateTime CreatedAt {get;set;}=DateTime.UtcNow;
 public DateTime? DueAt {get;set;} public Priority Priority {get;set;}=Priority.Normal;
 public WorkItemStatus Status {get;set;}=WorkItemStatus.Open; public long? LetterId {get;set;}
}
public class Meeting {
 public long Id {get;set;} [MaxLength(220)] public string Title {get;set;}="";
 public DateTime StartsAt {get;set;} public DateTime EndsAt {get;set;} [MaxLength(220)] public string? Location {get;set;}
 public string OrganizerId {get;set;}=""; [MaxLength(4000)] public string? Agenda {get;set;}
}
public class MeetingAttendee {
 public long Id {get;set;} public long MeetingId {get;set;} public Meeting? Meeting {get;set;} public string UserId {get;set;}="";
}

public class UserSessionRecord {
 public long Id {get;set;} public string UserId {get;set;}="";
 [MaxLength(120)] public string SessionKey {get;set;}=""; [MaxLength(80)] public string? IpAddress {get;set;}
 [MaxLength(500)] public string? UserAgent {get;set;} public DateTime SignedInAt {get;set;}=DateTime.UtcNow;
 public DateTime LastSeenAt {get;set;}=DateTime.UtcNow; public DateTime? RevokedAt {get;set;}
}
public class RegistryEntry {
 public long Id {get;set;} public long LetterId {get;set;} public Letter? Letter {get;set;}
 [MaxLength(100)] public string Book {get;set;}="دفتر مرکزی"; [MaxLength(100)] public string? ExternalNumber {get;set;}
 public DateTime? ExternalDate {get;set;} public string RegisteredById {get;set;}=""; public DateTime RegisteredAt {get;set;}=DateTime.UtcNow;
}

public class LetterComment {
 public long Id {get;set;} public long LetterId {get;set;} public Letter? Letter {get;set;}
 public string UserId {get;set;}=""; [MaxLength(1500)] public string Text {get;set;}=""; public DateTime CreatedAt {get;set;}=DateTime.UtcNow;
}
public class LetterBookmark {
 public long Id {get;set;} public long LetterId {get;set;} public string UserId {get;set;}=""; public DateTime CreatedAt {get;set;}=DateTime.UtcNow;
}

public class LetterTemplate {
 public long Id {get;set;} [MaxLength(160)] public string Name {get;set;}=""; [MaxLength(500)] public string? SubjectTemplate {get;set;}
 [MaxLength(6000)] public string? BodyTemplate {get;set;} public LetterType Type {get;set;}=LetterType.Internal; public bool IsActive {get;set;}=true;
 public string CreatedById {get;set;}=""; public DateTime CreatedAt {get;set;}=DateTime.UtcNow;
}
public class KnowledgeArticle {
 public long Id {get;set;} [MaxLength(220)] public string Title {get;set;}=""; [MaxLength(80)] public string Category {get;set;}="عمومی";
 [MaxLength(8000)] public string Content {get;set;}=""; [MaxLength(500)] public string? Keywords {get;set;} public bool IsPublished {get;set;}=true;
 public string AuthorId {get;set;}=""; public DateTime CreatedAt {get;set;}=DateTime.UtcNow; public DateTime UpdatedAt {get;set;}=DateTime.UtcNow;
}
public class OrganizationSetting {
 public int Id {get;set;} [MaxLength(160)] public string OrganizationName {get;set;}="Ashkan Office";
 [MaxLength(30)] public string LetterPrefix {get;set;}="ASH"; public int DefaultSlaHours {get;set;}=48;
 [MaxLength(160)] public string? SupportEmail {get;set;} [MaxLength(30)] public string? SupportPhone {get;set;}
 public bool EnableKnowledgeBase {get;set;}=true; public bool EnableRealtimeNotifications {get;set;}=true;
}


public class NotificationPreference
{
    public long Id { get; set; }
    public string UserId { get; set; } = "";
    public bool SlaAlerts { get; set; } = true;
    public bool ReferralAlerts { get; set; } = true;
    public bool MeetingAlerts { get; set; } = true;
    public bool TaskAlerts { get; set; } = true;
    public bool SecurityAlerts { get; set; } = true;
    public bool DailyDigest { get; set; } = false;
}
