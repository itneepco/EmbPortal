using Domian.Common;

namespace Domian
{
    public class Project : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Project() { }

        public Project(string name)
        {
            Name = name;
        }
    }
}