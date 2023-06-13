namespace Laba_ISTP_1.Helpers;

public static class Roles
{
    public static class Role
    {
        public const string Student = "student";
        public const string Teacher = "teacher";
        public const string Dean = "dean";
        public const string User = "user";
        public static ICollection<string> All => new[] { Student, Teacher, Dean, User };
    }
}