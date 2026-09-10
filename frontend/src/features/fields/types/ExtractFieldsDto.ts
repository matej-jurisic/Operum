export interface ExtractFieldsDto {
    fieldIds: string[];
    newTrackerName: string;
    referenceFieldName: string;
    displayFieldId?: string;
}

export interface ExtractFieldsResultDto {
    newTrackerId: string;
    newTrackerName: string;
    referenceFieldId: string;
    extractedEntryCount: number;
}
