using System.Text.RegularExpressions;

namespace Mellon.Common.Services
{
    public static class Guards
    {
        public static string StringNotNullOrEmpty(string argument, string argumentName)
        {
            if (string.IsNullOrEmpty(argument))
                throw new GuardException($"{argumentName} should not be null or empty.");
            return argument;
        }

        public static void StringMaximumLength(string argument, string argumentName, int maxLength)
        {
            var len = argument?.Length;
            if (len != null && len.Value > maxLength)
                throw new GuardException($"{argumentName} length should not be more than {maxLength} (value={len}).");
        }

        public static void StringExactLength(string argument, string argumentName, int exactLength)
        {
            var len = argument?.Length;
            if (len != null && len.Value != exactLength)
                throw new GuardException($"{argumentName} length should be exactly {exactLength} (value={len}).");
        }

        public static void EmailFormat(string argument, string argumentName)
        {
            string emailMatchingPattern = @"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])";
            RegexMatch(argument, argumentName, emailMatchingPattern);
        }

        public static void RegexMatch(string argument, string argumentName, string pattern)
        {
            if (argument != null)
            {
                Regex regex = new(pattern);
                if (!regex.IsMatch(argument))
                    throw new GuardException($"{argumentName} is not in the correct format.");
            }
        }

        public static int NumberNonNegative(int argument, string argumentName)
        {
            if (argument < 0)
                throw new GuardException($"{argumentName} should be non negative (value={argument}).");
            return argument;
        }

        public static int NumberInRange(int argument, int min, int max, string argumentName)
        {
            if (argument <= min || argument > max)
                throw new GuardException($"{argumentName} should be between {min} and {max} (value={argument}).");
            return argument;
        }

        public static int ValidIdentifier(int? id, string argumentName = null)
        {
            if (id == null || id.Value <= 0)
                throw new GuardException($"{argumentName ?? "Resource identifier"} is not valid.", ErrorCodes.GENERIC_INVALID_NUMERIC_RESOURCE_ID);
            return id.Value;
        }

        public static int? ValidOptionalIdentifier(int? id, string argumentName = null)
        {
            if (id.HasValue)
                return ValidIdentifier(id, argumentName);
            return id;
        }

        public static void ObjectNotNull(object argument, string argumentName)
        {
            if (argument == null)
                throw new GuardException($"{argumentName} should not be null.");
        }

    }
}
