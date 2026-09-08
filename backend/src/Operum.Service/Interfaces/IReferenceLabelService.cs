using Operum.Model.Models;

namespace Operum.Service.Interfaces
{
    /// <summary>
    /// Keeps the cached display label on <c>reference</c> field values in sync with the entry
    /// each one links to. The label lives in <see cref="FieldValue.StringValue"/> so that
    /// filters, sorts and analytics can treat a reference exactly like a string.
    /// </summary>
    public interface IReferenceLabelService
    {
        /// <summary>
        /// After an entry is written: for every reference value on it, resolve the linked
        /// entry's display-field value into the cached label, or clear the link if the target
        /// no longer exists or sits in the wrong tracker.
        /// </summary>
        /// <param name="currentFieldValues">The entry's full set of field values (tracked).</param>
        Task ResolveEntryReferences(string entryId, List<FieldValue> currentFieldValues, List<Field> allFields);

        /// <summary>
        /// After an entry is written: refresh the cached label on every reference value in any
        /// tracker that points at it, so a rename of the target propagates.
        /// </summary>
        Task RefreshReferencesToEntry(string changedEntryId);

        /// <summary>
        /// After a reference field's target tracker or display field changes: recompute the
        /// cached label on all of its values.
        /// </summary>
        Task RefreshFieldReferences(string fieldId);
    }
}
