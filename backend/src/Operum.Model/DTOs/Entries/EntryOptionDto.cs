namespace Operum.Model.DTOs.Entries
{
    /// <summary>
    /// A single entry as an option in a reference-field picker: its id and the label derived
    /// from the chosen display field.
    /// </summary>
    public class EntryOptionDto
    {
        public string Id { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
    }
}
