using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Operum.Model.Models
{
    public class Field
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Type { get; set; } = string.Empty;
        public bool Required { get; set; } = false;
        public bool Visible { get; set; } = true;
        public int Order { get; set; }
        public string? SelectOptions { get; set; }
        public bool IsCalculated { get; set; } = false;
        public string? Formula { get; set; }

        /// <summary>
        /// For a <c>reference</c> field: the tracker its values link into. Nulled when that
        /// tracker is deleted, which leaves the field degraded (read-only, values keep their
        /// last cached label).
        /// </summary>
        public string? ReferencedTrackerId { get; set; }
        [ForeignKey(nameof(ReferencedTrackerId))]
        public virtual Tracker? ReferencedTracker { get; set; }

        /// <summary>
        /// For a <c>reference</c> field: the field of <see cref="ReferencedTrackerId"/> whose
        /// value is shown as the link label and cached into each FieldValue's StringValue.
        /// Null (or nulled on delete) falls back to the target entry's creation date.
        /// </summary>
        public string? ReferencedDisplayFieldId { get; set; }
        [ForeignKey(nameof(ReferencedDisplayFieldId))]
        public virtual Field? ReferencedDisplayField { get; set; }

        public string TrackerId { get; set; } = string.Empty;
        [ForeignKey(nameof(TrackerId))]
        public virtual Tracker Tracker { get; set; } = null!;

        public virtual List<FieldValue> FieldValues { get; set; } = [];
    }
}
