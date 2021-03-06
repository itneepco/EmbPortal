namespace EmbPortal.Shared.Responses
{
   public class UomResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DimensionId { get; set; }
        public string Dimension { get; set; }
        public string Description { get; set; }
        public string LongDescription
        {
            get
            {
                return Name + " - " + Description;
            }
        }
    }
}