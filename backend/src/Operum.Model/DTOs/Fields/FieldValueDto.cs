namespace Operum.Model.DTOs.Fields
{
    public class FieldValueDto
    {
        public string FieldId { get; set; } = string.Empty;
        public string FieldName { get; set; } = string.Empty;
        public string FieldType { get; set; } = string.Empty;
        public object? Value { get; set; }

        /// <summary>
        /// For a <c>reference</c> field: the linked entry's id, so the client can render a
        /// link and preselect the picker. <see cref="Value"/> carries the display label.
        /// </summary>
        public string? ReferencedEntryId { get; set; }
    }
}
