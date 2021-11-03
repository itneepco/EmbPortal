using Domain.Common;

namespace Domain.Entities
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

        public void SetName(string name) {
            this.Name = name;
        }
    }
}