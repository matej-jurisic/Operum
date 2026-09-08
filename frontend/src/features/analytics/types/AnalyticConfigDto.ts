export interface PurposeDto {
    name: string;
    allowedDataTypes: string[];
}

export interface CodeDto {
    code: string;
    name: string;
    purposes: PurposeDto[];
}

export interface ResultTypeDto {
    name: string;
    /** True for result types only offered when building a saved widget (a Goal), not in
        Explore or a notification condition. */
    widgetOnly: boolean;
    codes: CodeDto[];
}

export interface AnalyticConfigDto {
    resultTypes: ResultTypeDto[];
}
