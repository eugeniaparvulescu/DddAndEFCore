using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace App
{
    public class Email : ValueObject
    {
        private Email(string value)
        {
            Value = value;
        }
        public string Value { get; set; }

        public static Result<Email> Create(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return Result.Failure<Email>("Email should not be null or empty.");
            }

            email.Trim();
            if(email.Length > 100)
            {
                return Result.Failure<Email>("Email is too long.");
            }

            if (!Regex.IsMatch(email, "^(.+)@(.+)$"))
            {
                return Result.Failure<Email>("Email is invalid");
            }

            return Result.Success(new Email(email));
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator string(Email email)
        {
            return email.Value;
        }
    }
}
