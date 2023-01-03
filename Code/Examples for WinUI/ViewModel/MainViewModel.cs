using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HandyValidation;

namespace Examples.ViewModel
{
    public class MainViewModel
    {
        public MainViewModel()
        {
            SetupValidation();
        }

        public Property<string> FirstName = new("John")
        {
            Validator = new RulesValidator<string>(
                Rule.NotNullOrWhiteSpace().WithResourceString("FirstNameCannotBeEmpty"),
                Rule.MinLength(2).WithFormattedResourceString("FirstNameCannotBeInitials")),
        };

        public Property<string> LastName = new("Smith")
        {
            Validator = new RulesValidator<string>(
                Rule.NotNullOrWhiteSpace().WithMessage("Please fill the Last Name field"),
                Rule.MinLength(2).WithMessage("Last Name must be at least two characters long")),
        };

        public Property<DateTimeOffset> Dob = new(DateTimeOffset.Now, ValidatorState.Invalid)
        {
            Validator = new RulesValidator<DateTimeOffset>(
                Rule.Range(DateTimeOffset.Now.AddYears(-60), DateTimeOffset.Now.AddYears(-21)).WithMessage("The borrower must be at least 21 and no older than 60"))
        };

        public Property<string> PhoneNumber = new()
        {
            Validator = new RulesValidator<string>(
                Rule.NotNullOrWhiteSpace().WithMessage("Please enter phone number"),
                Rule.NumberOfDigits(8, 11).WithMessage("Please enter either 8-digit local number or 11-digit mobile number"),
                Rule.AllowedSymbols("+()- 0123456789").WithMessage("The phone number you entered contains invalid characters")),
            ValueChanged = info => { info.Property.MetaData = string.Concat(info.NewValue.Where(c => char.IsDigit(c))); }
        };

        public Property<string> Email = new()
        {
            Validator = new RulesValidator<string>(
                Rule.NotNullOrWhiteSpace().WithMessage("Please enter email address"),
                Rule.Email().WithMessage("Email address is incorrect")),
            Delay = TimeSpan.FromSeconds(0.8)
        };

        public Property<string> Password = new()
        {
            Validator = new RulesValidator<string>(
                Rule.NotNullOrWhiteSpace().WithMessage("Please enter the password"),
                Rule.LengthIsInRange(8, 20).WithMessage("Password length must be between eight and twenty characters"),
                Rule.Custom<string>(password =>
                {
                    if (password == null) return null;

                    if (!password.Any(c => char.IsLetter(c)) || !password.Any(c => char.IsDigit(c)) || !password.Any(c => !char.IsLetterOrDigit(c)))
                    {
                        return "Password must contain at least one letter, digit and special character";
                    }

                    return null;
                }))
        };

        public Property<string> ConfirmPassword = new();

        public CustomValidator ConfirmPasswordValidator;

        public CompositeValidator PropertiesValidator;

        public CustomValidator ApiAvailabilityValidator = new CustomValidator(async (issues, token) => 
        {
            await Task.Delay(500, token);

            issues.Add("Unfortunately, we cannot accept your application right now because our server is temporarily down. Our experts are already working on fixing this problem. Please try again later.");
        });

        public CompositeValidator FormValidator;

        public ValidationStateWatcher SubmitButtonWatcher;

        private void SetupValidation()
        {
            ConfirmPasswordValidator = new CustomValidator(ValidatePasswordsMatch);

            ConfirmPassword.ValueChangedAsync = async info => { await ConfirmPasswordValidator.Validate(info.CancellationToken); };

            PropertiesValidator = new CompositeValidator(FirstName, LastName, Dob, PhoneNumber, Email, Password);

            FormValidator = new CompositeValidator(PropertiesValidator, ConfirmPasswordValidator, ApiAvailabilityValidator);
            
            SubmitButtonWatcher = new ValidationStateWatcher(FirstName, LastName, Dob, PhoneNumber, Email, Password, ConfirmPasswordValidator) { HasIssues = true };
        }

        private Task ValidatePasswordsMatch(ObservableCollection<object> issues, CancellationToken token)
        {
            if (Password.Value != null && !Password.Value.Equals(ConfirmPassword.Value, StringComparison.Ordinal)) issues.Add("Passwords do not match");

            return Task.CompletedTask;
        }

        public async Task Submit()
        {
            await FormValidator.Validate(CancellationToken.None);

            if (!FormValidator.HasIssues)
            {
                // Submit
            }
        }
    }
}
