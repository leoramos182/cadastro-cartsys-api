namespace CadastroCartsys.Crosscutting.Messages;

public struct UserMessages
{
    public static string NotFound => "User not found";

    public struct Name
    {
        public static string EmptyName => "Name is required";

    }

    public struct Email
    {
        public static string EmailInUse => "Email already in use";
        public static string InvalidEMail => "Invalid Email";
        public static string EmptyEMail => "Email is required";

    }

    public struct Password
    {
        public static string EmptyPassword => "Password is required";

    }
}