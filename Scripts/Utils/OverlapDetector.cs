using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

/// <summary>
/// 重叠检测器，负责检测牌之间的遮盖关系
/// </summary>
public class OverlapDetector
{
    /// <summary>
    /// 计算牌之间的遮盖关系
    /// </summary>
    public void CalculateOverlap(Transform handTransform)
    {
        // 首先将所有牌设置为可点击
        for (int i = 0; i < handTransform.childCount; i++)
        {
            handTransform.GetChild(i).GetComponent<CardView>().GetCardModel().SetClickable(true);
        }

        // 检测遮盖关系
        for (int i = 0; i < handTransform.childCount; i++)
        {
            if (IsOverlapping(handTransform.GetChild(i).GetComponent<CardView>(), handTransform))
            {
                // 在牌堆中，索引较小的牌在下面，索引较大的牌在上面
                // 所以当有重叠时，索引较小的牌被遮挡
                handTransform.GetChild(i).GetComponent<CardView>().GetCardModel().SetClickable(false);
                handTransform.GetChild(i).GetComponent<CardView>().UpdateClickableState();
            }
        }
    }

    /// <summary>
    /// 判断两张牌是否重叠
    /// </summary>
    private bool IsOverlapping(CardView card1, Transform handTransform)
    {
        // 获取当前卡牌在容器中的索引
        int myIndex = card1.transform.GetSiblingIndex();

        // 获取当前卡牌的矩形区域
        Rect myRect = GetWorldRect(card1);

        // 检查是否被其他卡牌遮挡
        for (int i = 0; i < handTransform.childCount; i++)
        {
            // 只检查在我之上（层级更高）的卡牌
            if (i > myIndex)
            {
                CardView otherCard = handTransform.GetChild(i).GetComponent<CardView>();

                if (otherCard != null)
                {
                    Rect otherWorldRect = GetWorldRect(otherCard);

                    // 如果两个矩形相交且重叠面积超过阈值，认为被遮挡
                    if (myRect.Overlaps(otherWorldRect))
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    /// <summary>
    /// 获取CardView在世界空间的矩形
    /// </summary>
    /// <param name="cardView">要转换的CardView</param>
    /// <returns>世界空间中的矩形</returns>
    private Rect GetWorldRect(CardView cardView)
    {
        Vector3[] corners = new Vector3[4];
        cardView.transform.GetComponent<RectTransform>().GetWorldCorners(corners);

        float xMin = corners[0].x;
        float yMin = corners[0].y;
        float xMax = corners[2].x;
        float yMax = corners[2].y;

        return new Rect(xMin, yMin, xMax - xMin, yMax - yMin);
    }
}