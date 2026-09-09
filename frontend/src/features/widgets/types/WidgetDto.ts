import { CreateAnalyticFieldDto } from "../../analytics/types/requests/CreateAnalyticDto";

export interface WidgetSourceFieldDto {
    purpose: string;
    fieldId: string;
    fieldName: string;
}

export interface WidgetSourceDto {
    id: string;
    /** The widget's definition read through this source's fields, e.g. "Monthly Totals:
        Day, Amount". */
    name: string;
    fields: WidgetSourceFieldDto[];
    trackerId: string;
    trackerName: string;
    order: number;
}

/** A reusable chart definition, as the Widget Library reads and edits it. Not scoped to
    any one dashboard -- see DashboardWidgetDto (dashboard feature) for how a placement of
    this renders on a board. */
export interface WidgetDto {
    id: string;
    name: string;
    description?: string;
    resultType: string;
    code: string;
    /** Line/Bar only: how the axis field is bucketed before the code aggregates it. */
    grouping?: string;
    /** Combined charts only: whether the chart is restricted to x-axis values shared by
        every source. */
    matchedValuesOnly: boolean;
    /** Goal widgets only: the target the value is shown as progress toward, in the value
        field's format (a number, or hh:mm:ss for a duration). */
    goalTarget?: string;
    sources: WidgetSourceDto[];
}

/** The Widget Library's view of an Entries widget's definition -- just which tracker it
    reads from. */
export interface EntriesWidgetDefinitionDto {
    id: string;
    name: string;
    trackerId: string;
    trackerName: string;
}

export interface CreateWidgetSourceRequestDto {
    trackerId: string;
    fields: CreateAnalyticFieldDto[];
}

export interface CreateWidgetDto {
    name?: string;
    description?: string;
    resultType: string;
    code: string;
    /** Line/Bar only: how the axis field is bucketed before the code aggregates it. */
    grouping?: string;
    matchedValuesOnly?: boolean;
    /** Goal widgets only, and required for them: a number or an hh:mm:ss duration. */
    goalTarget?: string;
    sources: CreateWidgetSourceRequestDto[];
}

/** Edits a widget's name/description, plus a Goal's target -- the rest of the definition
    (result type, code, sources, field mapping) is fixed at creation. Create a new widget
    instead of changing it. */
export interface UpdateWidgetDto {
    name?: string;
    description?: string;
    /** Goal widgets only. Omitted, the current target is kept. */
    goalTarget?: string;
}

export interface CreateEntriesWidgetDto {
    trackerId: string;
    name?: string;
}

export interface UpdateEntriesWidgetDto {
    name?: string;
}
