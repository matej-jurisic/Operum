import { Box, Group, Progress, Text } from "@mantine/core";
import { renderValue } from "../../../shared/utils/formatters/ValueRenderer";
import { GoalAnalyticDto } from "../types/AnalyticDto";
import { useCardLayout } from "./cardSizing";
import { WidgetShell } from "./WidgetShell";

interface Props {
    analytic: GoalAnalyticDto;
    color: string | undefined;
    isConfiguring: boolean;
    onRemove?: (analyticId: string) => void;
    onEdit?: (analyticId: string) => void;
    /** Stretch to fill the height of the container instead of using a fixed one. */
    fillHeight?: boolean;
}

export function GoalCard({
    analytic,
    color,
    isConfiguring,
    onRemove,
    onEdit,
    fillHeight,
}: Props) {
    const layout = useCardLayout(fillHeight);

    const type = analytic.valueField?.type;
    const hasProgress =
        analytic.progress !== undefined && analytic.progress !== null;
    const percent = hasProgress ? Math.round(analytic.progress! * 100) : null;
    const achieved = hasProgress && analytic.progress! >= 1;

    return (
        <WidgetShell
            layout={layout}
            fillHeight={fillHeight}
            isConfiguring={isConfiguring}
            color={color}
            itemId={analytic.id}
            onRemove={onRemove}
            onEdit={onEdit}
            title={analytic.name}
        >
            <Box
                style={
                    fillHeight
                        ? {
                              flex: 1,
                              minHeight: 0,
                              display: "flex",
                              flexDirection: "column",
                              justifyContent: "center",
                              gap: 10,
                          }
                        : { display: "flex", flexDirection: "column", gap: 10 }
                }
            >
                <Group justify="space-between" align="baseline" gap="xs" wrap="nowrap">
                    <Text fw={700} size="xl" style={{ lineHeight: 1.1 }}>
                        {renderValue(type, analytic.value)}
                    </Text>
                    {percent !== null && (
                        <Text fw={600} size="sm" c={achieved ? color : "dimmed"}>
                            {percent}%
                        </Text>
                    )}
                </Group>

                <Progress
                    value={hasProgress ? Math.min(100, Math.max(0, percent!)) : 0}
                    color={color}
                    size="lg"
                    radius="xl"
                    striped={achieved}
                />

                <Text size="xs" c="dimmed">
                    {analytic.target
                        ? achieved
                            ? `Target reached: ${renderValue(type, analytic.target)}`
                            : `Target: ${renderValue(type, analytic.target)}`
                        : "No target set"}
                </Text>
            </Box>
        </WidgetShell>
    );
}
