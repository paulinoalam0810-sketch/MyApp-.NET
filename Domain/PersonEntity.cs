using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace Domain
{
    public class PersonEntity
    {
        public Guid Id { get; private set; }
        public string Code { get; private set; } = string.Empty;
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PhoneNumber { get; private set; } = string.Empty;

        public string FullName => $"{FirstName} {LastName}";

        public ICollection<VisitEntity> Visits { get; private set; } = new List<VisitEntity>();

        public PersonEntity(string code, string firstName, string lastName, string email, string phoneNumber)
        {
            ValidateCode(code);
            ValidateFirstName(firstName);
            ValidateLastName(lastName);
            ValidateEmail(email);
            ValidatePhoneNumber(phoneNumber);

            Id = Guid.NewGuid();
            Code = code.Trim().ToUpper();
            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            Email = email.Trim().ToLower();
            PhoneNumber = phoneNumber.Trim();
        }

        public void UpdatePersonalInfo(string firstName, string lastName, string email, string phoneNumber)
        {
            ValidateFirstName(firstName);
            ValidateLastName(lastName);
            ValidateEmail(email);
            ValidatePhoneNumber(phoneNumber);

            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            Email = email.Trim().ToLower();
            PhoneNumber = phoneNumber.Trim();
        }

        private void ValidateCode(string code)
        {
            if (string.IsNullOrEmpty(code))
                throw new ArgumentException("Code cannot be null or empty.", nameof(code));

            if (code.Length < 3 || code.Length > 10)
                throw new ArgumentException("Code must be between 3 and 10 characters.", nameof(code));

        }

        private void ValidateFirstName(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentException("First name cannot be null, empty, or whitespace.", nameof(firstName));

            if (firstName.Length < 2 || firstName.Length > 50)
                throw new ArgumentException("First name must be between 2 and 50 characters.", nameof(firstName));
        }

        private void ValidateLastName(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentException("Last name cannot be null, empty, or whitespace.", nameof(lastName));

            if (lastName.Length < 2 || lastName.Length > 50)
                throw new ArgumentException("Last name must be between 2 and 50 characters.", nameof(lastName));
        }

        private void ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email cannot be null, empty, or whitespace.", nameof(email));

            if (email.Length < 5 || email.Length > 100)
                throw new ArgumentException("Email must be between 5 and 100 characters.", nameof(email));

            var emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (!Regex.IsMatch(email, emailPattern))
                throw new ArgumentException("Invalid email format.", nameof(email));
        }

        private void ValidatePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Phone number cannot be null, empty, or whitespace.", nameof(phoneNumber));

            if (phoneNumber.Trim().Length < 7 || phoneNumber.Trim().Length > 15)
                throw new ArgumentException("Phone number must be between 7 and 15 characters.", nameof(phoneNumber));

        }
    }
}
