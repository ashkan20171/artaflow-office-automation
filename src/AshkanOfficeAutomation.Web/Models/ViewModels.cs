using System.ComponentModel.DataAnnotations;
namespace AshkanOfficeAutomation.Web.Models;
public class LetterInput {
 [Required(ErrorMessage="موضوع الزامی است"),StringLength(500)] public string Subject {get;set;}="";
 [Required] public LetterType Type {get;set;}=LetterType.Internal;
 public Priority Priority {get;set;}=Priority.Normal;
 [StringLength(200)] public string? Sender {get;set;}
 [StringLength(200)] public string? Receiver {get;set;}
 public string Body {get;set;}="";
 public bool IsConfidential {get;set;}
}
public class ReferralInput {
 [Required] public long LetterId {get;set;}
 [Required(ErrorMessage="گیرنده الزامی است")] public string ToUserId {get;set;}="";
 [StringLength(1000)] public string Note {get;set;}="";
}
public class NewUserInput {
 [Required,StringLength(120)] public string FullName {get;set;}="";
 [Required,StringLength(60)] public string UserName {get;set;}="";
 [Required,EmailAddress] public string Email {get;set;}="";
 [Required,MinLength(8)] public string Password {get;set;}="";
 [Required] public string Role {get;set;}="Employee";
 public int? DepartmentId {get;set;}
}

public class DepartmentInput {
 [Required,StringLength(120)] public string Name {get;set;}=""; public int? ParentId {get;set;}
}
public class RolePermissionsInput {
 [Required] public string RoleName {get;set;}=""; public List<string> Permissions {get;set;}=new();
}
public class LetterSearchInput {
 public string? Q {get;set;} public LetterType? Type {get;set;} public LetterStatus? Status {get;set;}
 public Priority? Priority {get;set;} public DateTime? From {get;set;} public DateTime? To {get;set;}
}
