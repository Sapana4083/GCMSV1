
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

public class WrongCaseDecisionViewModel
{
    [Required(ErrorMessage = "Select Case Type")]
    public string? CaseType { get; set; }

    [Required(ErrorMessage = "Select Case Number")]
    public string? CaseNo { get; set; }

    public string? AppellantName { get; set; }

    public string? RespondentDepartment { get; set; }

    public string? AdvocateName { get; set; }

    public string? DecisionDate { get; set; }

    public string? DecisionType { get; set; }

    public List<SelectListItem> CaseTypes { get; set; }
        = new List<SelectListItem>();
}
