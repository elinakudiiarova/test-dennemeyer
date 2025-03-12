using System.ComponentModel.DataAnnotations;
using TestProjectDennemeyer.Controllers.DTO;

namespace TestProjectDennemeyer.Controllers.Validators;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ValidateProposalDecisionAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not ProposalDecisionRequest request)
        {
            return new ValidationResult("Invalid request object.");
        }

        if (request.Decision) return ValidationResult.Success;
        
        if (string.IsNullOrWhiteSpace(request.Comment))
        {
            return new ValidationResult("Comment is required when rejecting a proposal.");
        }

        if (request.PartyShare == null || !request.PartyShare.Any())
        {
            return new ValidationResult("At least one PartyShare must be provided when rejecting a proposal.");
        }
        
        return ValidationResult.Success;
    }
}