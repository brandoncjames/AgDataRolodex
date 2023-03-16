using AgDataRolodex.Contracts.Models;
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace AgDataRolodex.Contracts.DTO
{
    public class ContactDTO
    {
        public Guid Id { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        public string? MiddleName { get; set; }

        [Required]
        public Address Address { get; set; } = new Address();
    }

    //TODO: Move to own file
    public class ContactDTOValidator : AbstractValidator<ContactDTO>
    {
        public ContactDTOValidator()
        {
            RuleFor(x => x.FirstName).NotNull()
            .Length(0, 25)
            .Must(IsValidName).WithMessage("First Name is not valid");

            RuleFor(x => x.LastName).NotNull()
            .Length(0, 25)
            .Must(IsValidName).WithMessage("Last Name is not valid");

            RuleFor(x => x.Address).NotNull()
                .SetValidator(new AddressValidator());
        }

        private bool IsValidName(string name)
        {
            return name.All(Char.IsLetter);
        }
    }

    public class AddressValidator: AbstractValidator<Address>
    {
        public AddressValidator()
        {
            RuleFor(a => a.Street1).NotNull()
                .Length(0, 50);

            RuleFor(a => a.Street2).Length(0, 50);

            RuleFor(a => a.City).NotNull()
                .Must(IsValidString).WithMessage("Must be a valid string");

            RuleFor(a => a.State).NotNull()
                .Must(IsValidString).WithMessage("Must be a valid string");

            RuleFor(a => a.PostalCode).NotNull();
        }

        private bool IsValidString(string s)
        {
            return s.All(Char.IsLetter);
        }
    }


}
