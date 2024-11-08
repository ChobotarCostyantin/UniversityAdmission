namespace UniversityAdmission.Models.Entities
{
    public enum Roles
    {
        Owner = 1,
        Administrator = 2,
        Operator = 3,
        Default = 4
    }

    public enum Permission
    {
        Create,
        Read,
        Update,
        Delete
    }
}