namespace Operum.Model.DTOs.Fields
{
    public class ExtractFieldsResultDto
    {
        public string NewTrackerId { get; set; } = string.Empty;
        public string NewTrackerName { get; set; } = string.Empty;
        public string ReferenceFieldId { get; set; } = string.Empty;

        /// <summary>How many rows the new tracker ended up with (one per distinct combination).</summary>
        public int ExtractedEntryCount { get; set; }
    }
}
