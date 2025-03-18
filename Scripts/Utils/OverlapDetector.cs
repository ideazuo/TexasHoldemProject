using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 重叠检测器，负责检测牌之间的遮盖关系
/// </summary>
public class OverlapDetector
{
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
        
        // 计算每张牌的边界
        float card1Left = pos1.x - cardWidth / 2;
        float card1Right = pos1.x + cardWidth / 2;
        float card1Bottom = pos1.y - cardHeight / 2;
        float card1Top = pos1.y + cardHeight / 2;
        
        float card2Left = pos2.x - cardWidth / 2;
        float card2Right = pos2.x + cardWidth / 2;
        float card2Bottom = pos2.y - cardHeight / 2;
        float card2Top = pos2.y + cardHeight / 2;
        
        // 判断是否不重叠
        bool noOverlap = 
            card1Right < card2Left || // 卡牌1在卡牌2左侧
            card1Left > card2Right || // 卡牌1在卡牌2右侧
            card1Top < card2Bottom || // 卡牌1在卡牌2下方
            card1Bottom > card2Top;   // 卡牌1在卡牌2上方
        
        // 返回重叠结果
        return !noOverlap;
    }
}