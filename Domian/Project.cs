using Domian.Common;

namespace Domian
{
    public class Project : AuditableEntity
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        private Project() { }

        public Project(string name)
        {
            Name = name;
        }
    }
}