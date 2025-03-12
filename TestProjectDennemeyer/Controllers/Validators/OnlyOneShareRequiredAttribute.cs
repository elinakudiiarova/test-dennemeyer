using System.ComponentModel.DataAnnotations;
using TestProjectDennemeyer.Controllers.DTO;

namespace TestProjectDennemeyer.Controllers.Validators;

/// <summary>
/// Validator for the share amount and percentage
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class OnlyOneShareRequiredAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not List<PartyShare> partyShares)
        {
            return new ValidationResult("Invalid input type. Expected a list of PartyShare.");
        }

        foreach (var partyShare in partyShares)
        {
            bool hasAmount = partyShare.Amount.HasValue;
            bool hasPercentage = partyShare.Percentage.HasValue;

            if (hasAmount && hasPercentage)
            {
                return new ValidationResult($"Party {partyShare.PartyId} cannot have both Amount and Percentage.");
            }

            if (!hasAmount && !hasPercentage)
            {
                return new ValidationResult($"Party {partyShare.PartyId} must have either Amount or Percentage.");
            }
        }

        return ValidationResult.Success;
    }
}