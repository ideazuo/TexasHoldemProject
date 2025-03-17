using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 重叠检测器，负责检测牌之间的遮盖关系
/// </summary>
public class OverlapDetector
{
    // 检测是否重叠的阈值
    private float overlapThreshold = 0.4f;

    /// <summary>
    /// 计算牌之间的遮盖关系
    /// </summary>
    public void CalculateOverlap(List<CardModel> cards)
    {
        // 首先将所有牌设置为可点击
        foreach (var card in cards)
        {
            card.SetClickable(true);
        }
        
        // 如果牌的数量小于2，不需要检测重叠
        if (cards.Count < 2)
        {
            return;
        }

        // 检测遮盖关系
        for (int i = 0; i < cards.Count; i++)
        {
            for (int j = i + 1; j < cards.Count; j++)
            {
                // 如果两张牌重叠，设置被遮挡的牌为不可点击
                if (IsOverlapping(cards[i], cards[j]))
                {
                    // 在牌堆中，索引较小的牌在下面，索引较大的牌在上面
                    // 所以当有重叠时，索引较小的牌被遮挡
                    cards[i].SetClickable(false);
                }
            }
        }
    }

    /// <summary>
    /// 判断两张牌是否重叠
    /// </summary>
    private bool IsOverlapping(CardModel card1, CardModel card2)
    {
        // 获取牌的位置
        Vector2 pos1 = card1.GetPosition();
        Vector2 pos2 = card2.GetPosition();
        
        // 假设牌的宽高
        float cardWidth = 2.5f;
        float cardHeight = 3.5f;
        
        // 计算两张牌中心点之间的距离
        float distanceX = Mathf.Abs(pos1.x - pos2.x);
        float distanceY = Mathf.Abs(pos1.y - pos2.y);
        
        // 如果两张牌在两个方向上的重叠都超过阈值，则认为有遮挡
        bool isOverlappingX = distanceX < cardWidth * overlapThreshold;
        bool isOverlappingY = distanceY < cardHeight * overlapThreshold;
        
        return isOverlappingX && isOverlappingY;
    }

    /// <summary>
    /// 设置重叠检测阈值
    /// </summary>
    public void SetOverlapThreshold(float threshold)
    {
        this.overlapThreshold = Mathf.Clamp(threshold, 0.1f, 0.9f);
    }
}