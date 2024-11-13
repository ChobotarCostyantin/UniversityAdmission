namespace UniversityAdmission.Models.Entities
{
    public enum Roles
    {
        Owner,
        Administrator,
        Operator,
        Default
    }

    public enum Permission
    {
        OwnerOnly,
        AdministratorOnly,
        OperatorOnly,
        Default
    }

    public enum Subjects
    {
        Математика,
        Інформатика,
        Фізика,
        Хімія,
        Географія,
        Біологія,
        Українська,
        Іноземна,
        Історія,
        Література
    }
}