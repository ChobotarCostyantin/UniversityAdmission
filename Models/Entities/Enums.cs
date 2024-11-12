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
        Create,
        Read,
        Update,
        Delete
    }
}