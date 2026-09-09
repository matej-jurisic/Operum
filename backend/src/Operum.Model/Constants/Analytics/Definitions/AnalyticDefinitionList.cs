using Operum.Model.Constants.Fields;

namespace Operum.Model.Constants.Analytics.Definitions
{
    public static class AnalyticDefinitionList
    {
        // Number/duration value fields: every Line/Bar aggregation except Count reads one.
        private static readonly HashSet<string> Numeric = [DataTypes.Number, DataTypes.TimeSpan];

        // The aggregations offered for a bucketed Line grouping (Exact / Daily / ...).
        private static readonly HashSet<string> LineBucketCodes =
        [
            AnalyticCodes.Sum, AnalyticCodes.Average, AnalyticCodes.Count,
            AnalyticCodes.Min, AnalyticCodes.Max, AnalyticCodes.CumulativeSum
        ];

        // Same for a Bar grouping -- no raw and no cumulative.
        private static readonly HashSet<string> BarBucketCodes =
        [
            AnalyticCodes.Sum, AnalyticCodes.Average, AnalyticCodes.Count,
            AnalyticCodes.Min, AnalyticCodes.Max
        ];

        private static AnalyticPurposeDataTypes Agg(string valuePurpose, HashSet<string>? valueTypes) => new()
        {
            AllowedDataTypes = valueTypes == null
                ? []
                : new() { [valuePurpose] = valueTypes }
        };

        private static Dictionary<string, AnalyticGrouping> DateBucketGroupings(HashSet<string> codes) =>
            new()
            {
                [AnalyticGroupings.Daily] = new() { Label = "Daily", AllowedAxisTypes = [DataTypes.Date, DataTypes.DateTime], AllowedCodes = codes },
                [AnalyticGroupings.Weekly] = new() { Label = "Weekly", AllowedAxisTypes = [DataTypes.Date, DataTypes.DateTime], AllowedCodes = codes },
                [AnalyticGroupings.Monthly] = new() { Label = "Monthly", AllowedAxisTypes = [DataTypes.Date, DataTypes.DateTime], AllowedCodes = codes },
                [AnalyticGroupings.Yearly] = new() { Label = "Yearly", AllowedAxisTypes = [DataTypes.Date, DataTypes.DateTime], AllowedCodes = codes },
            };

        public static readonly Dictionary<string, AnalyticDefinition> ByResultType =
            new()
            {
                [AnalyticTypes.SingleValue] = new AnalyticDefinition
                {
                    Purposes = [AnalyticPurposes.Value],
                    Codes = new()
                    {
                        [AnalyticCodes.Count] = new AnalyticPurposeDataTypes
                        {
                            Label = "Count",
                            AllowedDataTypes = new()
                            {
                                [AnalyticPurposes.Value] = [.. DataTypes.All]
                            }
                        },
                        [AnalyticCodes.Min] = new AnalyticPurposeDataTypes
                        {
                            Label = "Minimum",
                            AllowedDataTypes = new()
                            {
                                [AnalyticPurposes.Value] = [DataTypes.Number, DataTypes.TimeSpan, DataTypes.Date, DataTypes.DateTime]
                            }
                        },
                        [AnalyticCodes.Max] = new AnalyticPurposeDataTypes
                        {
                            Label = "Maximum",
                            AllowedDataTypes = new()
                            {
                                [AnalyticPurposes.Value] = [DataTypes.Number, DataTypes.TimeSpan, DataTypes.Date, DataTypes.DateTime]
                            }
                        },
                        [AnalyticCodes.Average] = new AnalyticPurposeDataTypes
                        {
                            Label = "Average",
                            AllowedDataTypes = new()
                            {
                                [AnalyticPurposes.Value] = [DataTypes.Number, DataTypes.TimeSpan]
                            }
                        },
                        [AnalyticCodes.Sum] = new AnalyticPurposeDataTypes
                        {
                            Label = "Sum",
                            AllowedDataTypes = new()
                            {
                                [AnalyticPurposes.Value] = [DataTypes.Number, DataTypes.TimeSpan]
                            }
                        },
                        [AnalyticCodes.StdDev] = new AnalyticPurposeDataTypes
                        {
                            Label = "Std. Deviation",
                            AllowedDataTypes = new()
                            {
                                [AnalyticPurposes.Value] = [DataTypes.Number, DataTypes.TimeSpan]
                            }
                        },
                        [AnalyticCodes.TrueCount] = new AnalyticPurposeDataTypes
                        {
                            Label = "Yes Count",
                            AllowedDataTypes = new()
                            {
                                [AnalyticPurposes.Value] = [DataTypes.Bool]
                            }
                        },
                        [AnalyticCodes.FalseCount] = new AnalyticPurposeDataTypes
                        {
                            Label = "No Count",
                            AllowedDataTypes = new()
                            {
                                [AnalyticPurposes.Value] = [DataTypes.Bool]
                            }
                        },
                        [AnalyticCodes.TruePercentage] = new AnalyticPurposeDataTypes
                        {
                            Label = "Yes Percentage",
                            AllowedDataTypes = new()
                            {
                                [AnalyticPurposes.Value] = [DataTypes.Bool]
                            }
                        },
                        [AnalyticCodes.CountDistinct] = new AnalyticPurposeDataTypes
                        {
                            Label = "Unique Count",
                            AllowedDataTypes = new()
                            {
                                [AnalyticPurposes.Value] = [.. DataTypes.All]
                            }
                        },
                        [AnalyticCodes.MostCommon] = new AnalyticPurposeDataTypes
                        {
                            Label = "Most Common",
                            AllowedDataTypes = new()
                            {
                                [AnalyticPurposes.Value] = [.. DataTypes.All]
                            }
                        },
                        [AnalyticCodes.LeastCommon] = new AnalyticPurposeDataTypes
                        {
                            Label = "Least Common",
                            AllowedDataTypes = new()
                            {
                                [AnalyticPurposes.Value] = [.. DataTypes.All]
                            }
                        }
                    }
                },

                // A Goal is a Single Value calculation drawn as progress toward a target.
                // Same purpose and the same calculations, minus the ones whose result isn't a
                // number to compare (Most/Least Common). The target is typed when the Widget
                // is created and stored on it; see GoalAnalyticBuilder.
                [AnalyticTypes.Goal] = new AnalyticDefinition
                {
                    WidgetOnly = true,
                    Purposes = [AnalyticPurposes.Value],
                    Codes = new()
                    {
                        [AnalyticCodes.Count] = new AnalyticPurposeDataTypes
                        {
                            Label = "Count",
                            AllowedDataTypes = new()
                            {
                                [AnalyticPurposes.Value] = [.. DataTypes.All]
                            }
                        },
                        [AnalyticCodes.CountDistinct] = new AnalyticPurposeDataTypes
                        {
                            Label = "Unique Count",
                            AllowedDataTypes = new()
                            {
                                [AnalyticPurposes.Value] = [.. DataTypes.All]
                            }
                        },
                        [AnalyticCodes.Sum] = new AnalyticPurposeDataTypes
                        {
                            Label = "Sum",
                            AllowedDataTypes = new()
                            {
                                [AnalyticPurposes.Value] = [DataTypes.Number, DataTypes.TimeSpan]
                            }
                        },
                        [AnalyticCodes.Average] = new AnalyticPurposeDataTypes
                        {
                            Label = "Average",
                            AllowedDataTypes = new()
                            {
                                [AnalyticPurposes.Value] = [DataTypes.Number, DataTypes.TimeSpan]
                            }
                        },
                        [AnalyticCodes.Min] = new AnalyticPurposeDataTypes
                        {
                            Label = "Minimum",
                            AllowedDataTypes = new()
                            {
                                [AnalyticPurposes.Value] = [DataTypes.Number, DataTypes.TimeSpan]
                            }
                        },
                        [AnalyticCodes.Max] = new AnalyticPurposeDataTypes
                        {
                            Label = "Maximum",
                            AllowedDataTypes = new()
                            {
                                [AnalyticPurposes.Value] = [DataTypes.Number, DataTypes.TimeSpan]
                            }
                        },
                        [AnalyticCodes.TrueCount] = new AnalyticPurposeDataTypes
                        {
                            Label = "Yes Count",
                            AllowedDataTypes = new()
                            {
                                [AnalyticPurposes.Value] = [DataTypes.Bool]
                            }
                        },
                        [AnalyticCodes.FalseCount] = new AnalyticPurposeDataTypes
                        {
                            Label = "No Count",
                            AllowedDataTypes = new()
                            {
                                [AnalyticPurposes.Value] = [DataTypes.Bool]
                            }
                        },
                        [AnalyticCodes.TruePercentage] = new AnalyticPurposeDataTypes
                        {
                            Label = "Yes Percentage",
                            AllowedDataTypes = new()
                            {
                                [AnalyticPurposes.Value] = [DataTypes.Bool]
                            }
                        }
                    }
                },

                // Line and Bar: the calculation is a (Grouping, Code) pair. Grouping buckets
                // the axis field (X-axis / Name); Code aggregates each bucket. See
                // AnalyticGroupings and the *AnalyticBuilder processors.
                [AnalyticTypes.LineChart] = new AnalyticDefinition
                {
                    Purposes = [AnalyticPurposes.Xaxis, AnalyticPurposes.Yaxis],
                    GroupingPurpose = AnalyticPurposes.Xaxis,
                    AxisNoun = "value",
                    Codes = new()
                    {
                        [AnalyticCodes.RawValues] = Agg(AnalyticPurposes.Yaxis, Numeric),
                        [AnalyticCodes.Sum] = Agg(AnalyticPurposes.Yaxis, Numeric),
                        [AnalyticCodes.Average] = Agg(AnalyticPurposes.Yaxis, Numeric),
                        [AnalyticCodes.Count] = Agg(AnalyticPurposes.Yaxis, null),
                        [AnalyticCodes.Min] = Agg(AnalyticPurposes.Yaxis, Numeric),
                        [AnalyticCodes.Max] = Agg(AnalyticPurposes.Yaxis, Numeric),
                        [AnalyticCodes.CumulativeSum] = Agg(AnalyticPurposes.Yaxis, Numeric),
                    },
                    Groupings = new(DateBucketGroupings(LineBucketCodes))
                    {
                        [AnalyticGroupings.None] = new()
                        {
                            Label = "None",
                            AllowedAxisTypes = [.. DataTypes.All],
                            AllowedCodes = [AnalyticCodes.RawValues]
                        },
                        [AnalyticGroupings.Exact] = new()
                        {
                            Label = "By value",
                            AllowedAxisTypes = [.. DataTypes.All],
                            AllowedCodes = LineBucketCodes
                        },
                    }
                },

                [AnalyticTypes.BarChart] = new AnalyticDefinition
                {
                    Purposes = [AnalyticPurposes.Name, AnalyticPurposes.Value],
                    GroupingPurpose = AnalyticPurposes.Name,
                    AxisNoun = "category",
                    Codes = new()
                    {
                        [AnalyticCodes.Sum] = Agg(AnalyticPurposes.Value, Numeric),
                        [AnalyticCodes.Average] = Agg(AnalyticPurposes.Value, Numeric),
                        [AnalyticCodes.Count] = Agg(AnalyticPurposes.Value, null),
                        [AnalyticCodes.Min] = Agg(AnalyticPurposes.Value, Numeric),
                        [AnalyticCodes.Max] = Agg(AnalyticPurposes.Value, Numeric),
                    },
                    Groupings = new(DateBucketGroupings(BarBucketCodes))
                    {
                        [AnalyticGroupings.Exact] = new()
                        {
                            Label = "By category",
                            AllowedAxisTypes = [.. DataTypes.All],
                            AllowedCodes = BarBucketCodes
                        },
                    }
                },

                [AnalyticTypes.ScatterChart] = new AnalyticDefinition
                {
                    Purposes = [AnalyticPurposes.Xaxis, AnalyticPurposes.Yaxis, AnalyticPurposes.Match, AnalyticPurposes.Value],
                    Codes = new()
                    {
                        [AnalyticCodes.ScatterChart] = new AnalyticPurposeDataTypes
                        {
                            Label = "Scatter Plot",
                            AllowedDataTypes = new()
                            {
                                [AnalyticPurposes.Xaxis] = [DataTypes.Number, DataTypes.TimeSpan],
                                [AnalyticPurposes.Yaxis] = [DataTypes.Number, DataTypes.TimeSpan]
                            }
                        },
                        // Two sources, one per axis: each tracker maps a Match field (the
                        // join key, e.g. the day) and a Value field, and a point pairs the
                        // first source's value (x) with the second's (y) for every match
                        // key they share. See MultiSourceAnalyticMerger.MergeCorrelation.
                        [AnalyticCodes.CorrelationScatter] = new AnalyticPurposeDataTypes
                        {
                            Label = "Correlation",
                            AllowedDataTypes = new()
                            {
                                [AnalyticPurposes.Match] = [DataTypes.Date, DataTypes.DateTime, DataTypes.String, DataTypes.Number],
                                [AnalyticPurposes.Value] = [DataTypes.Number, DataTypes.TimeSpan]
                            }
                        }
                    }
                },

                [AnalyticTypes.Calendar] = new AnalyticDefinition
                {
                    Purposes = [AnalyticPurposes.When, AnalyticPurposes.What],
                    Codes = new()
                    {
                        [AnalyticCodes.Calendar] = new AnalyticPurposeDataTypes
                        {
                            Label = "Calendar",
                            AllowedDataTypes = new()
                            {
                                [AnalyticPurposes.When] = [DataTypes.Date, DataTypes.DateTime],
                                [AnalyticPurposes.What] = [.. DataTypes.All]
                            }
                        }
                    }
                },

                [AnalyticTypes.Donut] = new AnalyticDefinition
                {
                    Purposes = [AnalyticPurposes.Name, AnalyticPurposes.Value],
                    Codes = new()
                    {
                        [AnalyticCodes.DonutChart] = new AnalyticPurposeDataTypes
                        {
                            Label = "Sum per Category",
                            AllowedDataTypes = new()
                            {
                                [AnalyticPurposes.Name] = [.. DataTypes.All],
                                [AnalyticPurposes.Value] = [DataTypes.Number, DataTypes.TimeSpan]
                            }
                        }
                    }
                }
            };


        // Whether a calculation is valid for a result type. For Line/Bar this is the
        // (grouping, code) pair; for everything else grouping must be absent and the code
        // must be one the type defines.
        public static bool IsValidForType(string resultType, string code, string? grouping = null)
        {
            if (!ByResultType.TryGetValue(resultType, out var def))
                return false;

            if (def.UsesGrouping)
                return !string.IsNullOrEmpty(grouping)
                    && def.Groupings.TryGetValue(grouping, out var g)
                    && g.AllowedCodes.Contains(code);

            return string.IsNullOrEmpty(grouping) && def.Codes.ContainsKey(code);
        }

        public static bool SupportsPurpose(string resultType, string purpose) =>
            ByResultType.TryGetValue(resultType, out var def) && def.Purposes.Contains(purpose);

        // Whether a field of <paramref name="dataType"/> may fill <paramref name="purpose"/>
        // for this calculation. The grouping purpose is constrained by the grouping; every
        // other purpose by the code.
        public static bool IsValidDataType(string resultType, string code, string purpose, string dataType, string? grouping = null)
        {
            if (!ByResultType.TryGetValue(resultType, out var def))
                return false;

            if (def.UsesGrouping && purpose == def.GroupingPurpose)
                return !string.IsNullOrEmpty(grouping)
                    && def.Groupings.TryGetValue(grouping, out var g)
                    && g.AllowedAxisTypes.Contains(dataType);

            return def.Codes.TryGetValue(code, out var codeDef) &&
                codeDef.AllowedDataTypes.TryGetValue(purpose, out var allowed) &&
                allowed.Contains(dataType);
        }

        // The purposes a given calculation needs mapped to a field. For a grouping type
        // that's the grouping purpose plus whatever the aggregation reads (nothing, for
        // Count); for everything else it's exactly the keys of the code's AllowedDataTypes.
        public static IReadOnlyCollection<string> GetRequiredPurposes(string resultType, string code, string? grouping = null)
        {
            if (!ByResultType.TryGetValue(resultType, out var def) || !def.Codes.TryGetValue(code, out var codeDef))
                return [];

            if (!def.UsesGrouping)
                return codeDef.AllowedDataTypes.Keys;

            return [def.GroupingPurpose, .. codeDef.AllowedDataTypes.Keys];
        }

        // The human-readable name for an analytic, e.g. "Line Chart · Weekly average: Day,
        // Amount". Leads with the chart type because a calculation label alone doesn't
        // always identify the analytic. Skipped when it would just repeat the calculation
        // (e.g. Calendar's only code is also called "Calendar"). Shared by widget summaries
        // and dashboard sources so a saved and an ad hoc analytic with the same definition
        // read identically.
        public static string GetDisplayName(string resultType, string code, IEnumerable<string> fieldNames, string? grouping = null)
        {
            var label = GetLabel(resultType, code, grouping);
            var names = fieldNames.Where(n => !string.IsNullOrEmpty(n)).ToList();
            var calculation = names.Count > 0 ? $"{label}: {string.Join(", ", names)}" : label;
            return label == resultType ? calculation : $"{resultType} · {calculation}";
        }

        // The bare aggregation label for a Line/Bar code, for the "Calculation" dropdown
        // (the composed name with the grouping is built by the form).
        public static string GetAggregationLabel(string code) => code switch
        {
            AnalyticCodes.RawValues => "Raw values",
            AnalyticCodes.Sum => "Sum",
            AnalyticCodes.Average => "Average",
            AnalyticCodes.Count => "Count",
            AnalyticCodes.Min => "Minimum",
            AnalyticCodes.Max => "Maximum",
            AnalyticCodes.CumulativeSum => "Cumulative sum",
            _ => code
        };

        public static string GetLabel(string resultType, string code, string? grouping = null)
        {
            if (!ByResultType.TryGetValue(resultType, out var def))
                return code;

            if (def.UsesGrouping)
                return ComposeGroupedLabel(def, grouping, code);

            return def.Codes.TryGetValue(code, out var codeDef) && !string.IsNullOrEmpty(codeDef.Label)
                ? codeDef.Label
                : code;
        }

        // "Raw values", "Daily total", "Weekly average", "Count per category", "Cumulative
        // total per value".
        private static string ComposeGroupedLabel(AnalyticDefinition def, string? grouping, string code)
        {
            if (code == AnalyticCodes.RawValues)
                return "Raw values";

            var agg = code switch
            {
                AnalyticCodes.Sum => "total",
                AnalyticCodes.Average => "average",
                AnalyticCodes.Count => "count",
                AnalyticCodes.Min => "minimum",
                AnalyticCodes.Max => "maximum",
                AnalyticCodes.CumulativeSum => "cumulative total",
                _ => code.ToLowerInvariant()
            };

            var composed = grouping switch
            {
                AnalyticGroupings.Exact => $"{agg} per {def.AxisNoun}",
                AnalyticGroupings.Daily => $"Daily {agg}",
                AnalyticGroupings.Weekly => $"Weekly {agg}",
                AnalyticGroupings.Monthly => $"Monthly {agg}",
                AnalyticGroupings.Yearly => $"Yearly {agg}",
                _ => agg
            };

            return char.ToUpperInvariant(composed[0]) + composed[1..];
        }
    }
}
