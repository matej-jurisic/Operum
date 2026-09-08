using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Operum.Model.Models
{
    public class FieldValue
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string EntryId { get; set; } = string.Empty;
        [ForeignKey(nameof(EntryId))]
        public virtual Entry Entry { get; set; } = null!;

        public string FieldId { get; set; } = string.Empty;
        [ForeignKey(nameof(FieldId))]
        public virtual Field Field { get; set; } = null!;

        public string? StringValue { get; set; }
        public double? NumberValue { get; set; }
        public DateTime? DateTimeValue { get; set; }
        public TimeSpan? TimeSpanValue { get; set; }
        public bool? BooleanValue { get; set; }

        /// <summary>
        /// For a <c>reference</c> field: the entry in another tracker this value links to.
        /// Nulled when that entry is deleted (see EntriesService delete paths, which also
        /// clear the cached label in <see cref="StringValue"/>). The display label is cached
        /// in <see cref="StringValue"/> so filters, sorts and analytics treat it as a string.
        /// </summary>
        public string? ReferencedEntryId { get; set; }
        [ForeignKey(nameof(ReferencedEntryId))]
        public virtual Entry? ReferencedEntry { get; set; }
    }
}
