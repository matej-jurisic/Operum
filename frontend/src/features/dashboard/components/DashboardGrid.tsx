import { ReactNode, useCallback, useMemo, useRef } from "react";
import { MdDragIndicator } from "react-icons/md";
import { DragDropProvider } from "@dnd-kit/react";
import {
  GridLayout,
  Layout,
  LayoutItem,
  useContainerWidth,
  useGridContainer,
  useGridItem,
  useGridPlaceholder,
  useGridResizeHandle,
  verticalCompactor,
} from "@snapgridjs/react";
import {
  DashboardItemDisplayMode,
  DashboardLayoutItemDto,
  DashboardWidgetDto,
  LayoutVariant,
  LayoutVariants,
  WidgetTypes,
} from "../types/DashboardDto";
import {
  COLS,
  CONTAINER_PADDING,
  DASHBOARD_GRID_COLUMNS,
  DRAG_CANCEL_SELECTOR,
  DRAG_HANDLE_CLASS,
  DashboardTileCallbacks,
  ROOT_KEY,
  ROW_HEIGHT,
  VARIANTS,
  toLayoutDto,
  toLayoutItem,
  variantForWidth,
} from "./dashboardGridLayout";
import "./DashboardGrid.css";
import { DashboardWidget } from "./DashboardWidget";
import { DashboardContainerTile } from "./DashboardContainerTile";
import { TabsContainerTile } from "./TabsContainerTile";

interface Props extends DashboardTileCallbacks {
  widgets: DashboardWidgetDto[];
  color: string | undefined;
  isConfiguring: boolean;
  /** Box the board to a phone-width frame so the narrow grid renders and every drag saves
      to the Mobile arrangement, whatever the real viewport is. Arrange mode only. */
  previewMobile?: boolean;
  onLayoutSave: (
    variant: LayoutVariant,
    layout: DashboardLayoutItemDto[],
  ) => void;
}

export function DashboardGrid({
  widgets,
  color,
  isConfiguring,
  previewMobile = false,
  onLayoutSave,
  ...callbacks
}: Props) {
  // Measured with a ResizeObserver. The grid renders only once `mounted` is true, so it
  // never lays itself out at the hook's assumed default width and overflows a narrower
  // container for a frame. In mobile preview the frame around this element caps it to a
  // phone width, so the measurement itself lands in the narrow variant.
  const { width, containerRef, mounted } = useContainerWidth();
  const variant = previewMobile
    ? LayoutVariants.Mobile
    : variantForWidth(width);

  const board = (
    <div ref={containerRef}>
      {mounted &&
        (variant === LayoutVariants.Mobile ? (
          <FlatBoard
            widgets={widgets}
            width={width}
            color={color}
            isConfiguring={isConfiguring}
            onLayoutSave={onLayoutSave}
            {...callbacks}
          />
        ) : (
          <NestedBoard
            widgets={widgets}
            width={width}
            color={color}
            isConfiguring={isConfiguring}
            onLayoutSave={onLayoutSave}
            {...callbacks}
          />
        ))}
    </div>
  );

  // The frame carries the border and the inset; the measured element inside it stays
  // padding-free so the width the grid is handed is the width it renders into (a padded
  // measured element leaves the grid overflowing it).
  return previewMobile ? (
    <div className="dashboard-mobile-frame">{board}</div>
  ) : (
    board
  );
}

/** snapgrid renders the layout it is handed as-is and only compacts during a drag. The
    desktop board is never recompacted server-side, so a stored stack with gaps -- a
    widget deleted, resized smaller, set Hidden, or moved into a container -- shows those
    gaps as dead space, and a freshly added widget (the backend seeds it below the lowest
    row any item ever reached) floats far below the real content. Close the gaps on the
    way in; the first drag in arrange mode then persists the compacted stack. minW/minH
    aren't carried through the compactor, so re-attach them. */
function compactLayout(items: Layout, cols: number): Layout {
  const constraintsById = new Map(items.map((it) => [it.i, it]));
  return verticalCompactor.compact(items, cols).map((it) => {
    const src = constraintsById.get(it.i);
    return src ? { ...it, minW: src.minW, minH: src.minH } : it;
  });
}

interface BoardProps extends DashboardTileCallbacks {
  widgets: DashboardWidgetDto[];
  width: number;
  color: string | undefined;
  isConfiguring: boolean;
  onLayoutSave: (
    variant: LayoutVariant,
    layout: DashboardLayoutItemDto[],
  ) => void;
}

// -- The narrow grid --------------------------------------------------------------------
// A phone flattens containers away: every widget sits on one four-column grid in reading
// order, so the turnkey component is enough. A container item itself draws nothing here.

function FlatBoard({
  widgets,
  width,
  color,
  isConfiguring,
  onLayoutSave,
  ...callbacks
}: BoardProps) {
  const config = VARIANTS[LayoutVariants.Mobile];
  const cols = COLS[LayoutVariants.Mobile];

  // Both container kinds are flattened away on the narrow grid (their children just join
  // the single-column flow); a widget set Hidden on mobile is dropped from it entirely
  // (reachable from the board's hidden-widgets list instead).
  const shown = useMemo(
    () =>
      widgets.filter(
        (w) =>
          w.type !== WidgetTypes.Container &&
          w.type !== WidgetTypes.TabsContainer &&
          w.mobileLayout.displayMode !== DashboardItemDisplayMode.Hidden,
      ),
    [widgets],
  );

  // Containers are flattened away here, but every widget still carries the mobileLayout.y
  // it was seeded with, and that stack is never recompacted server-side when the wide
  // grid's container tree changes -- a widget moved into a container on the desktop board
  // leaves a screen-tall hole on the phone. compactLayout closes it.
  const layout = useMemo(
    () =>
      compactLayout(
        shown.map((widget, index) =>
          toLayoutItem(widget, index, LayoutVariants.Mobile, cols),
        ),
        cols,
      ),
    [shown, cols],
  );

  const handleArranged = (newLayout: Layout) => {
    if (!isConfiguring) return;
    onLayoutSave(LayoutVariants.Mobile, toLayoutDto(newLayout, null));
  };

  return (
    <GridLayout
      className={`dashboard-grid${isConfiguring ? " is-editing" : ""}`}
      width={width}
      layout={layout}
      gridConfig={{
        cols,
        rowHeight: ROW_HEIGHT,
        margin: config.margin,
        containerPadding: [0, 0],
      }}
      compactor={verticalCompactor}
      isDraggable={isConfiguring}
      isResizable={isConfiguring}
      dragConfig={{
        enabled: isConfiguring,
        bounded: true,
        handle: config.dragHandle,
        cancel: DRAG_CANCEL_SELECTOR,
      }}
      resizeConfig={{ enabled: isConfiguring }}
      onLayoutChange={handleArranged}
    >
      {shown.map((widget) => (
        <div key={widget.id} className="dashboard-widget">
          {isConfiguring && config.dragHandle && (
            <div className={DRAG_HANDLE_CLASS} aria-hidden="true">
              <MdDragIndicator size={18} />
            </div>
          )}
          <DashboardWidget
            widget={widget}
            variant={LayoutVariants.Mobile}
            color={color}
            isConfiguring={isConfiguring}
            {...callbacks}
          />
        </div>
      ))}
    </GridLayout>
  );
}

// -- The wide grid ---------------------------------------------------------------------
// Containers each hold their own sub-grid. Every grid on the board -- the root one and
// one per container -- shares a single dnd-kit provider, which is what lets a widget be
// dragged from one into another.

function NestedBoard({
  widgets,
  width,
  color,
  isConfiguring,
  onLayoutSave,
  ...callbacks
}: BoardProps) {
  const containerIds = useMemo(
    () =>
      new Set(
        widgets
          .filter(
            (w) =>
              w.type === WidgetTypes.Container ||
              w.type === WidgetTypes.TabsContainer,
          )
          .map((w) => w.id),
      ),
    [widgets],
  );

  const { topWidgets, childrenByContainer, parentById } = useMemo(() => {
    // A widget belongs to a container only if that container still exists; a stale
    // parent (its container was deleted out from under it) falls back to the board.
    const parentOf = (w: DashboardWidgetDto) =>
      w.parentItemId && containerIds.has(w.parentItemId)
        ? w.parentItemId
        : null;

    const top: DashboardWidgetDto[] = [];
    const byContainer = new Map<string, DashboardWidgetDto[]>();
    // Where each widget currently lives (container id, or null for the board), read just
    // before a drop so a widget that changed grids can be told apart from one that only
    // moved within its own.
    const byId = new Map<string, string | null>();
    for (const w of widgets) {
      byId.set(w.id, parentOf(w));
      // A widget set Hidden on the wide grid is dropped from it entirely -- both from the
      // board and from whatever container it belongs to -- and reached from the board's
      // hidden-widgets list instead.
      if (w.layout.displayMode === DashboardItemDisplayMode.Hidden) continue;

      const parent = parentOf(w);
      if (parent === null) {
        top.push(w);
        continue;
      }
      const list = byContainer.get(parent) ?? [];
      list.push(w);
      byContainer.set(parent, list);
    }
    return {
      topWidgets: top,
      childrenByContainer: byContainer,
      parentById: byId,
    };
  }, [widgets, containerIds]);

  // Each grid's measured inner width, so a widget crossing from one grid to another can
  // be rescaled to keep its on-screen size. The board's width is known directly; each
  // container reports its sub-grid's width once mounted.
  const gridWidths = useRef(new Map<string, number>());
  const reportGridWidth = useCallback((id: string, w: number) => {
    gridWidths.current.set(id, w);
  }, []);

  // The pixel span of one column-plus-gap on the given grid (null for the board). A
  // widget keeps its size across a move when its width in columns scales by the ratio of
  // these: same reasoning as CONTAINER_MARGIN matching the board's for height.
  const colStepOf = (parentItemId: string | null): number | null => {
    const margin = VARIANTS[LayoutVariants.Desktop].margin[0];
    if (parentItemId === null) return (width + margin) / DASHBOARD_GRID_COLUMNS;
    const bodyWidth = gridWidths.current.get(parentItemId);
    if (!bodyWidth) return null;
    return (
      (bodyWidth + margin - CONTAINER_PADDING[0] * 2) / DASHBOARD_GRID_COLUMNS
    );
  };

  const clampCol = (v: number, max: number) => Math.max(0, Math.min(max, v));

  // Rescale a just-dropped widget so its width in pixels survives the move between grids
  // of different widths; leaves a widget that stayed in its own grid untouched.
  const keepSizeAcrossMove = (
    item: LayoutItem,
    from: string | null,
    to: string | null,
  ): LayoutItem => {
    if (from === to) return item;
    const fromStep = colStepOf(from);
    const toStep = colStepOf(to);
    if (!fromStep || !toStep) return item;
    const scale = fromStep / toStep;
    if (Math.abs(scale - 1) < 0.05) return item;
    const w = clampCol(Math.round(item.w * scale), DASHBOARD_GRID_COLUMNS) || 1;
    const x = clampCol(
      Math.round(item.x * scale),
      DASHBOARD_GRID_COLUMNS - w,
    );
    return { ...item, w, x };
  };

  // A cross-grid drop reports the item leaving one grid and joining another as two
  // separate layout changes, both fired synchronously. Rather than persist each on its
  // own -- and race them -- each grid drops its latest layout here and one microtask
  // later they are assembled into a single whole-board save.
  // Keyed by grid: ROOT_KEY for the board, a container id for a plain container, and
  // `${containerId}:${tabId}` for one tab of a tabs container. Each entry remembers which
  // parent (and, for a tabs container, which tab) its rows belong to.
  const pending = useRef(
    new Map<
      string,
      { parentItemId: string | null; parentTabId: string | null; layout: Layout }
    >(),
  );
  const flushQueued = useRef(false);

  const queueSave = (
    key: string,
    layout: Layout,
    parentItemId: string | null = null,
    parentTabId: string | null = null,
  ) => {
    if (!isConfiguring) return;
    pending.current.set(key, { parentItemId, parentTabId, layout });
    if (flushQueued.current) return;
    flushQueued.current = true;
    queueMicrotask(() => {
      flushQueued.current = false;
      const items: DashboardLayoutItemDto[] = [];
      for (const { parentItemId, parentTabId, layout } of pending.current.values()) {
        const sized = layout.map((item) =>
          keepSizeAcrossMove(item, parentById.get(item.i) ?? null, parentItemId),
        );
        items.push(...toLayoutDto(sized, parentItemId, parentTabId));
      }
      pending.current.clear();
      if (items.length > 0) onLayoutSave(LayoutVariants.Desktop, items);
    });
  };

  return (
    <DragDropProvider>
      <BoardSubGrid
        gridKey={ROOT_KEY}
        width={width}
        widgets={topWidgets}
        margin={VARIANTS[LayoutVariants.Desktop].margin}
        isConfiguring={isConfiguring}
        onArranged={(layout) => queueSave(ROOT_KEY, layout)}
        renderContent={(widget, handleRef) =>
          widget.type === WidgetTypes.Container ? (
            <DashboardContainerTile
              widget={widget}
              handleRef={handleRef}
              childWidgets={childrenByContainer.get(widget.id) ?? []}
              color={color}
              isConfiguring={isConfiguring}
              onChildrenArranged={(layout) =>
                queueSave(widget.id, layout, widget.id)
              }
              onBodyWidth={(w) => reportGridWidth(widget.id, w)}
              {...callbacks}
            />
          ) : widget.type === WidgetTypes.TabsContainer ? (
            <TabsContainerTile
              widget={widget}
              handleRef={handleRef}
              childWidgets={childrenByContainer.get(widget.id) ?? []}
              color={color}
              isConfiguring={isConfiguring}
              onChildrenArranged={(tabId, layout) =>
                queueSave(`${widget.id}:${tabId}`, layout, widget.id, tabId)
              }
              onBodyWidth={(w) => reportGridWidth(widget.id, w)}
              onSaveTabs={(dto) =>
                callbacks.onSaveTabsContainer?.(widget.id, dto)
              }
              {...callbacks}
            />
          ) : (
            <DashboardWidget
              widget={widget}
              variant={LayoutVariants.Desktop}
              color={color}
              isConfiguring={isConfiguring}
              {...callbacks}
            />
          )
        }
      />
    </DragDropProvider>
  );
}

// -- One grid surface, headless ------------------------------------------------------

interface BoardSubGridProps {
  gridKey: string;
  width: number;
  widgets: DashboardWidgetDto[];
  margin: [number, number];
  isConfiguring: boolean;
  onArranged: (layout: Layout) => void;
  /** The tile body for a widget. `handleRef`, when attached to an element, restricts a
      pointer drag of the tile to that element (used by a container's header so a drag
      that starts inside its sub-grid doesn't move the whole panel). */
  renderContent: (
    widget: DashboardWidgetDto,
    handleRef: (element: Element | null) => void,
  ) => ReactNode;
  /** Floor on the surface's height, so an empty grid still offers an area a widget can
      be dragged onto. */
  minHeight?: number;
  /** Inset between the grid's edge and its cells. Given as the grid's own padding rather
      than CSS padding on the wrapper, so the measured width the grid is handed matches
      the box it actually renders into. */
  containerPadding?: [number, number];
}

export function BoardSubGrid({
  gridKey,
  width,
  widgets,
  margin,
  isConfiguring,
  onArranged,
  renderContent,
  minHeight,
  containerPadding = [0, 0],
}: BoardSubGridProps) {
  const layout = useMemo(
    () =>
      compactLayout(
        widgets.map((widget, index) =>
          toLayoutItem(
            widget,
            index,
            LayoutVariants.Desktop,
            DASHBOARD_GRID_COLUMNS,
          ),
        ),
        DASHBOARD_GRID_COLUMNS,
      ),
    [widgets],
  );

  const { containerProps, group } = useGridContainer({
    id: gridKey,
    width,
    layout,
    onLayoutChange: onArranged,
    gridConfig: {
      cols: DASHBOARD_GRID_COLUMNS,
      rowHeight: ROW_HEIGHT,
      margin,
      containerPadding,
    },
    compactor: verticalCompactor,
    isDraggable: isConfiguring,
    isResizable: isConfiguring,
    dragConfig: {
      enabled: isConfiguring,
      bounded: true,
      cancel: DRAG_CANCEL_SELECTOR,
    },
    resizeConfig: { enabled: isConfiguring },
  });

  return (
    <div
      {...containerProps}
      style={
        minHeight
          ? { ...containerProps.style, minHeight }
          : containerProps.style
      }
      className={`dashboard-grid${isConfiguring ? " is-editing" : ""}`}
    >
      {widgets.map((widget) => (
        <BoardTile
          key={widget.id}
          id={widget.id}
          group={group}
          isConfiguring={isConfiguring}
        >
          {(handleRef) => renderContent(widget, handleRef)}
        </BoardTile>
      ))}
      <BoardPlaceholder group={group} />
    </div>
  );
}

function BoardTile({
  id,
  group,
  isConfiguring,
  children,
}: {
  id: string;
  group: string;
  isConfiguring: boolean;
  children: (handleRef: (element: Element | null) => void) => ReactNode;
}) {
  const { ref, handleRef, style, isDragging } = useGridItem({ id, group });

  return (
    <div
      ref={ref}
      style={style}
      className={`snapgrid-item${isDragging ? " is-dragging" : ""}`}
    >
      <div className="dashboard-widget">{children(handleRef)}</div>
      {isConfiguring && <ResizeHandle id={id} group={group} />}
    </div>
  );
}

function ResizeHandle({ id, group }: { id: string; group: string }) {
  const { ref, handleProps } = useGridResizeHandle({
    id,
    handle: "se",
    group,
  });
  return (
    <span
      ref={ref}
      {...handleProps}
      aria-hidden="true"
      className="snapgrid-resize-handle snapgrid-resize-handle--se"
    />
  );
}

function BoardPlaceholder({ group }: { group: string }) {
  const placeholder = useGridPlaceholder(group);
  if (!placeholder) return null;
  return (
    <div
      aria-hidden="true"
      className="snapgrid-placeholder"
      style={placeholder.style}
    />
  );
}
