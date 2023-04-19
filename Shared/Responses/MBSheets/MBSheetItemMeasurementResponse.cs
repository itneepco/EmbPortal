namespace EmbPortal.Shared.Responses.MBSheets;

public class MBSheetItemMeasurementResponse
{
    public int Id { get; private set; }
    public int MBSheetItemId { get; set; }
    public string Description { get; set; }
    public int No { get; set; } = 1;
    public string Val1 { get; set; }
    public string Val2 { get; set; }
    public string Val3 { get; set; }
    public float Total { get; set; }
}
