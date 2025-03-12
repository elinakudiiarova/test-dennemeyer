using System.ComponentModel.DataAnnotations;
using TestProjectDennemeyer.Controllers.DTO;

namespace TestProjectDennemeyer.Controllers.Validators;

/// <summary>
/// Ensures that `PartyShare` is required only when rejecting a proposal.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class RequiredIfRejectedAttribute : ValidationAttribute
{
    /// <summary>
    /// Validates if the property is required based on the `Decision` field.
    /// </summary>
    /// <param name="value">The value being validated.</param>
    /// <param name="validationContext">Validation context.</param>
    /// <returns>Validation result indicating success or failure.</returns>
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var request = validationContext.ObjectInstance as ProposalDecisionRequest;

        if (request != null && !request.Decision) // Only validate when Decision = false (Rejected)
        {
            if (value is not List<PartyShare> partyShares || !partyShares.Any())
            {
                return new ValidationResult("At least one PartyShare must be provided when rejecting a proposal.");
            }
        }

        return ValidationResult.Success;
    }
}