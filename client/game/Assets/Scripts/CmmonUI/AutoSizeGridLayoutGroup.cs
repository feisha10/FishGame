using UnityEngine;
using System.Collections.Generic;

namespace UnityEngine.UI
{

    public class AutoSizeGridLayoutGroup : GridLayoutGroup
    {
        List<BaseItemRender> items = new List<BaseItemRender>();

        public int ItemNum = 0; //又外部赋值
        public int LayoutGroupHeight;
        private bool _isUpdatePos;

        public override void CalculateLayoutInputHorizontal()
        {
            if (ItemNum == items.Count)
                return;

            _isUpdatePos = false;
            items.Clear();
            rectChildren.Clear();

            for (int i = 0; i < rectTransform.childCount; i++)
            {
                var rect = rectTransform.GetChild(i) as RectTransform;
                if (rect == null || !rect.gameObject.activeInHierarchy)
                    continue;

                BaseItemRender item = rect.GetComponent<BaseItemRender>();
                if (item!=null)
                {
                    items.Add(item);
                    rectChildren.Add(rect);
                }
            }
            m_Tracker.Clear();
        }

        public override void CalculateLayoutInputVertical()
        {

        }

        public override void SetLayoutHorizontal()
        {
            SetCellsAlongAxis(0);
        }

        public override void SetLayoutVertical()
        {
            SetCellsAlongAxis(1);
        }

        private void SetCellsAlongAxis(int axis)
        {
            if (_isUpdatePos)
                return;
            // Normally a Layout Controller should only set horizontal values when invoked for the horizontal axis
            // and only vertical values when invoked for the vertical axis.
            // However, in this case we set both the horizontal and vertical position when invoked for the vertical axis.
            // Since we only set the horizontal position and not the size, it shouldn't affect children's layout,
            // and thus shouldn't break the rule that all horizontal layout must be calculated before all vertical layout.

            if (axis == 0)
            {
                // Only set the sizes when invoked for horizontal axis, not the positions.
//                for (int i = 0; i < rectChildren.Count; i++)
//                {
//                    RectTransform rect = rectChildren[i];
//
//                    m_Tracker.Add(this, rect,
//                        DrivenTransformProperties.Anchors |
//                        DrivenTransformProperties.AnchoredPosition |
//                        DrivenTransformProperties.SizeDelta);
//
//                    rect.anchorMin = Vector2.up;
//                    rect.anchorMax = Vector2.up;
//                    rect.sizeDelta = cellSize;
//                }
                return;
            }

            float width = rectTransform.rect.size.x;
            float height = rectTransform.rect.size.y;

            int cellCountX = 1;
            int cellCountY = 1;
            if (m_Constraint == Constraint.FixedColumnCount)
            {
                cellCountX = m_ConstraintCount;
                cellCountY = Mathf.CeilToInt(rectChildren.Count / (float)cellCountX - 0.001f);
            }
            else if (m_Constraint == Constraint.FixedRowCount)
            {
                cellCountY = m_ConstraintCount;
                cellCountX = Mathf.CeilToInt(rectChildren.Count / (float)cellCountY - 0.001f);
            }
            else
            {
                if (cellSize.x + spacing.x <= 0)
                    cellCountX = int.MaxValue;
                else
                    cellCountX = Mathf.Max(1, Mathf.FloorToInt((width - padding.horizontal + spacing.x + 0.001f) / (cellSize.x + spacing.x)));

                if (cellSize.y + spacing.y <= 0)
                    cellCountY = int.MaxValue;
                else
                    cellCountY = Mathf.Max(1, Mathf.FloorToInt((height - padding.vertical + spacing.y + 0.001f) / (cellSize.y + spacing.y)));
            }

            int cornerX = (int)startCorner % 2;

            int cellsPerMainAxis, actualCellCountX, actualCellCountY;
            if (startAxis == Axis.Horizontal)
            {
                cellsPerMainAxis = cellCountX;
                actualCellCountX = Mathf.Clamp(cellCountX, 1, rectChildren.Count);
                actualCellCountY = Mathf.Clamp(cellCountY, 1, Mathf.CeilToInt(rectChildren.Count / (float)cellsPerMainAxis));
            }
            else
            {
                cellsPerMainAxis = cellCountY;
                actualCellCountY = Mathf.Clamp(cellCountY, 1, rectChildren.Count);
                actualCellCountX = Mathf.Clamp(cellCountX, 1, Mathf.CeilToInt(rectChildren.Count / (float)cellsPerMainAxis));
            }

            Vector2 requiredSpace = new Vector2(
                    actualCellCountX * cellSize.x + (actualCellCountX - 1) * spacing.x,
                    actualCellCountY * cellSize.y + (actualCellCountY - 1) * spacing.y
                    );
            Vector2 startOffset = new Vector2(
                    GetStartOffset(0, requiredSpace.x),
                    GetStartOffset(1, requiredSpace.y)
                    );

            LayoutGroupHeight = 0;

            for (int i = 0; i < rectChildren.Count; i++)
            {
                int positionX;
                if (startAxis == Axis.Horizontal)
                {
                    positionX = i % cellsPerMainAxis;
                }
                else
                {
                    positionX = i / cellsPerMainAxis;
                }

                if (cornerX == 1)
                    positionX = actualCellCountX - 1 - positionX;

                int itemheight = items[i].ItemHeight;

                SetChildAlongAxis(rectChildren[i], 0, startOffset.x + (cellSize[0] + spacing[0]) * positionX, cellSize[0]);

                SetChildAlongAxis(rectChildren[i], 1, LayoutGroupHeight - items[i].YOffset, itemheight);

                LayoutGroupHeight += (itemheight);

            }

            _isUpdatePos = true;
        }

    }


}


