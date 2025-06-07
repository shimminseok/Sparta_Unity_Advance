using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(ScrollRect))]
    [RequireComponent(typeof(RectTransform))]
    public class ReuseScrollview<T> : MonoBehaviour
    {
        protected List<T> tableData = new List<T>();
        [SerializeField] protected GameObject cellBase = null;
        [SerializeField] private RectOffset padding;
        [SerializeField] private float spacingHeight;
        [SerializeField] private float spacingWidth;
        [SerializeField] private RectOffset visibleRectPadding = null;

        private LinkedList<ReuseCellData<T>> cells = new LinkedList<ReuseCellData<T>>();
        private Rect visibleRect;
        private Vector2 prevScrollPos;
        public RectTransform                CachedRectTransform => GetComponent<RectTransform>();
        public ScrollRect                   CachedScrollRect    => GetComponent<ScrollRect>();
        public List<T>                      TableData           => tableData;
        public LinkedList<ReuseCellData<T>> Cells               => cells;

        protected virtual void Start()
        {
            cellBase.SetActive(false);
            CachedScrollRect.onValueChanged.AddListener(OnScrollPosChanged);
        }

        protected void InitTableView()
        {
            if (CachedScrollRect.vertical)
            {
                UpdateScrollViewSizeVertical();
            }
            else
            {
                UpdateScrollViewSizeHorizontal();
            }

            UpdateVisibleRect();

            if (cells.Count < 1)
            {
                Vector2 cellStart = CachedScrollRect.vertical ? new Vector2(0f, -padding.top) : new Vector2(padding.left, 0f);
                float   height    = GetCellHeight();
                float   width     = GetCellWidth();
                for (int i = 0; i < tableData.Count; i++)
                {
                    float   cellSize = CachedScrollRect.vertical ? height : width;
                    Vector2 cellEnd  = CachedScrollRect.vertical ? cellStart + new Vector2(0f, -cellSize) : cellStart + new Vector2(cellSize, 0f);

                    if (IsWithinVisibleRect(cellStart, cellEnd))
                    {
                        ReuseCellData<T> cell = CreateCellForIndex(i);
                        if (CachedScrollRect.vertical)
                        {
                            cell.Top = cellStart;
                            break;
                        }
                        else
                        {
                            cell.Left = cellStart;
                            break;
                        }
                    }

                    cellStart = CachedScrollRect.vertical
                        ? cellEnd + new Vector2(0f, spacingHeight)
                        : cellEnd + new Vector2(spacingWidth, 0f);
                }

                SetFillVisibleRectWithCells();
            }
            else
            {
                LinkedListNode<ReuseCellData<T>> node = cells.First;
                UpdateCellForIndex(node.Value, node.Value.Index);
                node = node.Next;

                while (node != null)
                {
                    UpdateCellForIndex(node.Value, node.Previous.Value.Index + 1);
                    if (CachedScrollRect.vertical)
                    {
                        node.Value.Top = node.Previous.Value.Bottom + new Vector2(0f, -spacingHeight);
                    }
                    else
                    {
                        node.Value.Left = node.Previous.Value.Right + new Vector2(spacingWidth, 0f);
                    }

                    node = node.Next;
                }

                SetFillVisibleRectWithCells();
            }
        }

        protected void ResetCell()
        {
            int i = 0;
            foreach (var cell in cells)
            {
                UpdateCellForIndex(cell, i++);
            }
        }

        protected virtual float GetCellHeight()
        {
            return cellBase.GetComponent<RectTransform>().sizeDelta.y;
        }

        protected virtual float GetCellWidth()
        {
            return cellBase.GetComponent<RectTransform>().sizeDelta.x;
        }

        protected void UpdateScrollViewSize()
        {
            float contentHeigth = 0f;
            float cellHeight    = GetCellHeight();
            for (int i = 0; i < tableData.Count; i++)
            {
                contentHeigth += cellHeight;

                if (i > 0)
                {
                    contentHeigth += spacingHeight;
                }
            }

            Vector2 sizeDelta = CachedScrollRect.content.sizeDelta;
            sizeDelta.y = padding.top + contentHeigth + padding.bottom;
            CachedScrollRect.content.sizeDelta = sizeDelta;
        }

        protected void UpdateScrollViewSizeVertical()
        {
            float contentHeight = 0f;
            float cellHeight    = GetCellHeight();
            for (int i = 0; i < tableData.Count; i++)
            {
                contentHeight += cellHeight;

                if (i > 0)
                {
                    contentHeight += spacingHeight;
                }
            }

            Vector2 sizeDelta = CachedScrollRect.content.sizeDelta;
            sizeDelta.y = padding.top + contentHeight + padding.bottom;
            CachedScrollRect.content.sizeDelta = sizeDelta;
        }

        protected void UpdateScrollViewSizeHorizontal()
        {
            float contentWidth = 0f;
            float cellWidth    = GetCellWidth();
            for (int i = 0; i < tableData.Count; i++)
            {
                contentWidth += cellWidth;

                if (i > 0)
                {
                    contentWidth += spacingWidth;
                }
            }

            Vector2 sizeDelta = CachedScrollRect.content.sizeDelta;
            sizeDelta.x = padding.left + contentWidth + padding.right;
            CachedScrollRect.content.sizeDelta = sizeDelta;
        }

        private ReuseCellData<T> CreateCellForIndex(int index)
        {
            GameObject go = Instantiate(cellBase);
            go.SetActive(true);
            ReuseCellData<T> cell      = go.GetComponent<ReuseCellData<T>>();
            Vector3          scale     = cell.transform.localScale;
            Vector2          sizeDelta = cell.CachedRectTransform.sizeDelta;
            Vector2          offsetMin = cell.CachedRectTransform.offsetMin;
            Vector2          offsetMax = cell.CachedRectTransform.offsetMax;

            cell.transform.SetParent(cellBase.transform.parent);

            cell.transform.localScale = scale;
            cell.CachedRectTransform.sizeDelta = sizeDelta;
            cell.CachedRectTransform.offsetMin = offsetMin;
            cell.CachedRectTransform.offsetMax = offsetMax;

            UpdateCellForIndex(cell, index);
            cells.AddLast(cell);

            return cell;
        }

        protected void UpdateCellForIndex(ReuseCellData<T> cell, int index)
        {
            cell.Index = index;

            if (cell.Index >= 0 && cell.Index <= tableData.Count - 1)
            {
                cell.gameObject.SetActive(true);
                cell.UpdateContent(tableData[cell.Index]);
                if (CachedScrollRect.vertical)
                {
                    cell.Height = GetCellHeight();
                }
                else
                {
                    cell.Width = GetCellWidth();
                }

                cell.m_data = tableData[cell.Index];
            }
            else
            {
                cell.gameObject.SetActive(false);
            }
        }

        public void UpdateAllCells()
        {
            foreach (var cell in cells)
            {
                if (cell.Index >= 0 && cell.Index < tableData.Count)
                    cell.UpdateContent(tableData[cell.Index]);
            }
        }

        private void UpdateVisibleRect()
        {
            visibleRect.x = -CachedScrollRect.content.anchoredPosition.x + visibleRectPadding.left;
            visibleRect.y = -CachedScrollRect.content.anchoredPosition.y + visibleRectPadding.top;

            visibleRect.width = CachedRectTransform.rect.width + visibleRectPadding.left + visibleRectPadding.right;
            visibleRect.height = CachedRectTransform.rect.height + visibleRectPadding.top + visibleRectPadding.bottom;
        }

        private bool IsWithinVisibleRect(Vector2 start, Vector2 end)
        {
            if (CachedScrollRect.vertical)
            {
                return (start.y <= visibleRect.y && start.y >= visibleRect.y - visibleRect.height) || (end.y <= visibleRect.y && end.y >= visibleRect.y - visibleRect.height);
            }
            else
            {
                return (start.x <= visibleRect.x + visibleRect.width && start.x >= visibleRect.x) || (end.x <= visibleRect.x + visibleRect.width && end.x >= visibleRect.x);
            }
        }

        private void SetFillVisibleRectWithCells()
        {
            if (cells.Count < 1)
            {
                return;
            }

            ReuseCellData<T> lastCell          = cells.Last.Value;
            int              nextCellDataIndex = lastCell.Index + 1;
            Vector2 nextCellStart = CachedScrollRect.vertical
                ? lastCell.Bottom + new Vector2(0f, -spacingHeight)
                : lastCell.Right + new Vector2(spacingWidth, 0f);

            while (nextCellDataIndex < tableData.Count && IsWithinVisibleRect(nextCellStart, nextCellStart))
            {
                ReuseCellData<T> cell = CreateCellForIndex(nextCellDataIndex);
                if (CachedScrollRect.vertical)
                {
                    cell.Top = nextCellStart;
                }
                else
                {
                    cell.Left = nextCellStart;
                }

                lastCell = cell;
                nextCellDataIndex = lastCell.Index + 1;
                nextCellStart = CachedScrollRect.vertical
                    ? lastCell.Bottom + new Vector2(0f, -spacingHeight)
                    : lastCell.Right + new Vector2(spacingWidth, 0f);
            }
        }

        public void OnScrollPosChanged(Vector2 scrollPos)
        {
            UpdateVisibleRect();
            if (CachedScrollRect.vertical)
            {
                UpdateVirtical((scrollPos.y < prevScrollPos.y) ? 1 : -1);
            }
            else
            {
                UpdateHorizontal((scrollPos.x < prevScrollPos.x) ? 1 : -1);
            }

            prevScrollPos = scrollPos;
        }

        private void UpdateVirtical(int scrollDirection)
        {
            if (cells.Count < 1)
                return;

            if (scrollDirection > 0)
            {
                ReuseCellData<T> firstCell = cells.First.Value;
                while (IsBeyondVisibleRect(firstCell))
                {
                    ReuseCellData<T> lastCell = cells.Last.Value;
                    UpdateCellForIndex(firstCell, lastCell.Index + 1);
                    firstCell.Top = lastCell.Bottom + new Vector2(0f, -spacingHeight);
                    cells.AddLast(firstCell);
                    cells.RemoveFirst();
                    firstCell = cells.First.Value;
                }

                SetFillVisibleRectWithCells();
            }
            else if (scrollDirection < 0)
            {
                ReuseCellData<T> lastCell = cells.Last.Value;
                while (IsBeyondVisibleRect(lastCell))
                {
                    ReuseCellData<T> firstCell = cells.First.Value;
                    UpdateCellForIndex(lastCell, firstCell.Index - 1);
                    lastCell.Bottom = firstCell.Top + new Vector2(0f, spacingHeight);
                    cells.AddFirst(lastCell);
                    cells.RemoveLast();
                    lastCell = cells.Last.Value;
                }

                SetFillVisibleRectWithCells();
            }
        }

        private void UpdateHorizontal(int _scrollDirection)
        {
            if (cells.Count < 1)
                return;

            if (_scrollDirection > 0)
            {
                ReuseCellData<T> lastCell = cells.Last.Value;
                while (IsBeyondVisibleRect(lastCell))
                {
                    ReuseCellData<T> firstCell = cells.First.Value;
                    UpdateCellForIndex(lastCell, firstCell.Index - 1);
                    lastCell.Right = firstCell.Left + new Vector2(-spacingWidth, 0f);
                    cells.AddFirst(lastCell);
                    cells.RemoveLast();
                    lastCell = cells.Last.Value;
                }

                SetFillVisibleRectWithCells();
            }
            else if (_scrollDirection < 0)
            {
                ReuseCellData<T> firstCell = cells.First.Value;
                while (IsBeyondVisibleRect(firstCell))
                {
                    ReuseCellData<T> lastCell = cells.Last.Value;
                    UpdateCellForIndex(firstCell, lastCell.Index + 1);
                    firstCell.Left = lastCell.Right + new Vector2(spacingWidth, 0f);
                    cells.AddLast(firstCell);
                    cells.RemoveFirst();
                    firstCell = cells.First.Value;
                }

                SetFillVisibleRectWithCells();
            }
        }

        private bool IsBeyondVisibleRect(ReuseCellData<T> cell)
        {
            if (CachedScrollRect.vertical)
            {
                if (cell.Bottom.y > visibleRect.y || cell.Top.y < visibleRect.y - visibleRect.height)
                    return true;
                else
                    return false;
            }
            else
            {
                if (cell.Right.x < visibleRect.x || cell.Left.x > visibleRect.x + visibleRect.width)
                {
                    return true;
                }
                else
                    return false;
            }
        }
    }
}