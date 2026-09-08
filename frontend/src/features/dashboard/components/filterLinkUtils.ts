import { fieldTypesCompatible } from "../../../shared/constants/DataTypes";
import { FieldDto } from "../../fields/types/FieldDto";
import { QueryKinds } from "../../../shared/constants/QueryKinds";
import {
    ClauseDto,
    DashboardWidgetDto,
    SaveFilterItemDto,
    WidgetLink,
    WidgetTypes,
    parseFilterWidgetConfig,
} from "../types/DashboardDto";
import { clauseLabel } from "./filterClauseInput";

/** One of the board's existing filter widgets, offered as something a newly created widget
    can follow -- its clauses, keyed by their stable slot id since (unlike the creation
    forms) this widget's shape is already saved. */
export interface FilterCandidate {
    itemId: string;
    label: string;
    queries: { slotId: string; dataType: string; describe: string }[];
}

/** The board's filter widgets a new widget's tracker(s) could follow, one entry per Filter
    item with complete clauses. Labeled by its own clause set so two filter widgets never
    read the same. */
export function filterCandidatesFor(widgets: DashboardWidgetDto[]): FilterCandidate[] {
    const filterWidgets = widgets.filter(
        (w) => w.type === WidgetTypes.Filter && (w.filter?.clauses.length ?? 0) > 0,
    );
    const seen = new Map<string, number>();
    return filterWidgets.map((w) => {
        const clauses = w.filter!.clauses;
        const base = clauses.map((c) => clauseLabel(c.dataType, c.operator)).join(", ");
        const n = (seen.get(base) ?? 0) + 1;
        seen.set(base, n);
        return {
            itemId: w.id,
            label: n > 1 ? `${base} (${n})` : base,
            queries: clauses.map((c) => ({
                slotId: c.slotId,
                dataType: c.dataType,
                describe: clauseLabel(c.dataType, c.operator),
            })),
        };
    });
}

/** One new widget's tracker source and which of the board's filter widgets it should
    follow, each mapped to the field on that tracker it filters by. Passed alongside a
    widget's create/place dto to have it linked up in the same step. */
export interface FilterFollowLinks {
    trackerId: string;
    /** filterItemId -> (that filter's clause slot id -> field id on `trackerId`) */
    links: Record<string, Record<string, string>>;
}

/** Whether every filter checked in `links` has every one of its clauses mapped to a field
    `fields` actually offers -- the same completeness check a filter widget's own edit
    dialog runs, gating the "Add" button the same way. */
export function followLinksComplete(
    links: Record<string, Record<string, string>>,
    filters: FilterCandidate[],
    fields: FieldDto[],
): boolean {
    const eligibleFields = (dataType: string) =>
        fields.filter((f) => fieldTypesCompatible(f.type, dataType));
    return Object.entries(links).every(([filterItemId, fieldBySlot]) => {
        const filter = filters.find((f) => f.itemId === filterItemId);
        if (!filter) return false;
        return filter.queries.every((q) => {
            const fieldId = fieldBySlot[q.slotId];
            return !!fieldId && eligibleFields(q.dataType).some((f) => f.id === fieldId);
        });
    });
}

/** A filter widget's clause list, keyed by its slot id -> that clause's index in the
    widget's own clause order -- the key SaveFilterItemDto's links expect, since the backend
    rewrites those indices back to slot ids on save. */
export function filterWidgetIndexBySlotId(widget: DashboardWidgetDto): Map<string, string> {
    return new Map((widget.filter?.clauses ?? []).map((c, i) => [c.slotId, String(i)]));
}

/** Rebuilds the SaveFilterItemDto an existing filter widget would resubmit unchanged: same
    clause shape (values omitted -- the backend carries the current ones across any save
    whose clauses still pool to the same query), its links translated back to clause-index
    keys, and its presets. Lets a follower link be appended without going through the
    filter widget's own edit dialog. */
export function filterWidgetToSaveDto(widget: DashboardWidgetDto): SaveFilterItemDto {
    const config = parseFilterWidgetConfig(widget.config);
    const clauseDtos = widget.filter?.clauses ?? [];
    const indexBySlotId = filterWidgetIndexBySlotId(widget);

    const clauses: ClauseDto[] = clauseDtos.map((c) => ({
        kind: QueryKinds.Filter,
        dataType: c.dataType,
        operator: c.operator ?? "",
        value: null,
        descending: false,
    }));

    const links: WidgetLink[] = (config?.links ?? []).map((l) => ({
        itemId: l.itemId,
        trackerId: l.trackerId,
        fieldByQuery: Object.fromEntries(
            Object.entries(l.fieldByQuery).flatMap(([slotId, fieldId]) => {
                const index = indexBySlotId.get(slotId);
                return index !== undefined ? [[index, fieldId]] : [];
            }),
        ),
    }));

    return { clauses, links, presetIds: config?.presetIds ?? [] };
}

/** Converts one follower's slot-id-keyed field mapping into the clause-index-keyed
    WidgetLink a filter widget's SaveFilterItemDto expects. */
export function toFollowerLink(
    indexBySlotId: Map<string, string>,
    follower: { itemId: string; trackerId: string; fieldBySlotId: Record<string, string> },
): WidgetLink {
    return {
        itemId: follower.itemId,
        trackerId: follower.trackerId,
        fieldByQuery: Object.fromEntries(
            Object.entries(follower.fieldBySlotId).flatMap(([slotId, fieldId]) => {
                const index = indexBySlotId.get(slotId);
                return index !== undefined ? [[index, fieldId]] : [];
            }),
        ),
    };
}
