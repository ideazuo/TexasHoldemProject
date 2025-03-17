using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 扑克牌型枚举，按照德州扑克规则从小到大排序
/// </summary>
public enum PokerHandType
{
    HighCard = 0,      // 高牌
    OnePair = 1,       // 一对
    TwoPair = 2,       // 两对
    ThreeOfAKind = 3,  // 三条
    Straight = 4,      // 顺子
    Flush = 5,         // 同花
    FullHouse = 6,     // 葫芦
    FourOfAKind = 7,   // 四条
    StraightFlush = 8, // 同花顺
    RoyalFlush = 9     // 皇家同花顺
}

/// <summary>
/// 扑克牌型计算器，实现德州扑克牌型的计算逻辑
/// </summary>
public static class PokerHandCalculator
{
    /// <summary>
    /// 计算五张牌的得分
    /// </summary>
    public static int CalculateScore(List<CardModel> cards)
    {
        // 判断牌的数量是否为5
        if (cards.Count != 5)
        {
            Debug.LogError("计算牌型需要5张牌");
            return 0;
        }

        // 计算牌型
        PokerHandType handType = EvaluateHand(cards);
        
        // 基于牌型计算基础分数
        int baseScore = CalculateBaseScore(handType);
        
        // 计算额外分数（基于牌点数）
        int extraScore = CalculateExtraScore(cards, handType);
        
        // 返回总分
        return baseScore + extraScore;
    }

    /// <summary>
    /// 评估五张牌的牌型
    /// </summary>
    private static PokerHandType EvaluateHand(List<CardModel> cards)
    {
        // 检查皇家同花顺（RoyalFlush）
        if (IsRoyalFlush(cards))
        {
            return PokerHandType.RoyalFlush;
        }
        
        // 检查同花顺（StraightFlush）
        if (IsStraightFlush(cards))
        {
            return PokerHandType.StraightFlush;
        }
        
        // 检查四条（FourOfAKind）
        if (IsFourOfAKind(cards))
        {
            return PokerHandType.FourOfAKind;
        }
        
        // 检查葫芦（FullHouse）
        if (IsFullHouse(cards))
        {
            return PokerHandType.FullHouse;
        }
        
        // 检查同花（Flush）
        if (IsFlush(cards))
        {
            return PokerHandType.Flush;
        }
        
        // 检查顺子（Straight）
        if (IsStraight(cards))
        {
            return PokerHandType.Straight;
        }
        
        // 检查三条（ThreeOfAKind）
        if (IsThreeOfAKind(cards))
        {
            return PokerHandType.ThreeOfAKind;
        }
        
        // 检查两对（TwoPair）
        if (IsTwoPair(cards))
        {
            return PokerHandType.TwoPair;
        }
        
        // 检查一对（OnePair）
        if (IsOnePair(cards))
        {
            return PokerHandType.OnePair;
        }
        
        // 默认为高牌（HighCard）
        return PokerHandType.HighCard;
    }

    /// <summary>
    /// 获取排序后的点数列表
    /// </summary>
    private static List<int> GetSortedRanks(List<CardModel> cards)
    {
        return cards.Select(c => (int)c.GetRank()).OrderByDescending(r => r).ToList();
    }

    /// <summary>
    /// 检查是否是皇家同花顺
    /// </summary>
    private static bool IsRoyalFlush(List<CardModel> cards)
    {
        // 皇家同花顺：同一花色的A, K, Q, J, 10
        return IsFlush(cards) && 
               cards.Any(c => c.GetRank() == CardRank.Ace) &&
               cards.Any(c => c.GetRank() == CardRank.King) &&
               cards.Any(c => c.GetRank() == CardRank.Queen) &&
               cards.Any(c => c.GetRank() == CardRank.Jack) &&
               cards.Any(c => c.GetRank() == CardRank.Ten);
    }

    /// <summary>
    /// 检查是否是同花顺
    /// </summary>
    private static bool IsStraightFlush(List<CardModel> cards)
    {
        // 同花顺：同一花色的顺子
        return IsFlush(cards) && IsStraight(cards);
    }

    /// <summary>
    /// 检查是否是四条
    /// </summary>
    private static bool IsFourOfAKind(List<CardModel> cards)
    {
        // 四条：4张相同点数的牌
        var groups = cards.GroupBy(c => c.GetRank()).ToList();
        return groups.Any(g => g.Count() == 4);
    }

    /// <summary>
    /// 检查是否是葫芦
    /// </summary>
    private static bool IsFullHouse(List<CardModel> cards)
    {
        // 葫芦：3张相同点数的牌 + 2张相同点数的牌
        var groups = cards.GroupBy(c => c.GetRank()).ToList();
        return groups.Count == 2 && groups.Any(g => g.Count() == 3);
    }

    /// <summary>
    /// 检查是否是同花
    /// </summary>
    private static bool IsFlush(List<CardModel> cards)
    {
        // 同花：5张相同花色的牌
        var suit = cards[0].GetSuit();
        return cards.All(c => c.GetSuit() == suit);
    }

    /// <summary>
    /// 检查是否是顺子
    /// </summary>
    private static bool IsStraight(List<CardModel> cards)
    {
        // 顺子：5张连续点数的牌
        var ranks = GetSortedRanks(cards);
        
        // 检查是否是A-5的顺子
        if (ranks.Contains((int)CardRank.Ace) && 
            ranks.Contains((int)CardRank.Two) &&
            ranks.Contains((int)CardRank.Three) &&
            ranks.Contains((int)CardRank.Four) &&
            ranks.Contains((int)CardRank.Five))
        {
            return true;
        }
        
        // 检查常规顺子
        for (int i = 1; i < ranks.Count; i++)
        {
            if (ranks[i-1] != ranks[i] + 1)
            {
                return false;
            }
        }
        
        return true;
    }

    /// <summary>
    /// 检查是否是三条
    /// </summary>
    private static bool IsThreeOfAKind(List<CardModel> cards)
    {
        // 三条：3张相同点数的牌 + 2张不同点数的牌
        var groups = cards.GroupBy(c => c.GetRank()).ToList();
        return groups.Count == 3 && groups.Any(g => g.Count() == 3);
    }

    /// <summary>
    /// 检查是否是两对
    /// </summary>
    private static bool IsTwoPair(List<CardModel> cards)
    {
        // 两对：2张相同点数的牌 + 2张相同点数的牌 + 1张不同点数的牌
        var groups = cards.GroupBy(c => c.GetRank()).ToList();
        return groups.Count == 3 && groups.Count(g => g.Count() == 2) == 2;
    }

    /// <summary>
    /// 检查是否是一对
    /// </summary>
    private static bool IsOnePair(List<CardModel> cards)
    {
        // 一对：2张相同点数的牌 + 3张不同点数的牌
        var groups = cards.GroupBy(c => c.GetRank()).ToList();
        return groups.Count == 4 && groups.Any(g => g.Count() == 2);
    }

    /// <summary>
    /// 计算基础分数（基于牌型）
    /// </summary>
    private static int CalculateBaseScore(PokerHandType handType)
    {
        // 每种牌型的基础分数
        switch (handType)
        {
            case PokerHandType.RoyalFlush:
                return 10000;
            case PokerHandType.StraightFlush:
                return 9000;
            case PokerHandType.FourOfAKind:
                return 8000;
            case PokerHandType.FullHouse:
                return 7000;
            case PokerHandType.Flush:
                return 6000;
            case PokerHandType.Straight:
                return 5000;
            case PokerHandType.ThreeOfAKind:
                return 4000;
            case PokerHandType.TwoPair:
                return 3000;
            case PokerHandType.OnePair:
                return 2000;
            case PokerHandType.HighCard:
                return 1000;
            default:
                return 0;
        }
    }

    /// <summary>
    /// 计算额外分数（基于牌点数）
    /// </summary>
    private static int CalculateExtraScore(List<CardModel> cards, PokerHandType handType)
    {
        // 获取排序后的点数
        List<int> ranks = GetSortedRanks(cards);
        
        // 基于不同牌型的额外分数计算
        switch (handType)
        {
            case PokerHandType.FourOfAKind:
                // 四张相同点数的牌，使用这个点数作为额外分数
                var fourGroup = cards.GroupBy(c => c.GetRank()).FirstOrDefault(g => g.Count() == 4);
                return fourGroup != null ? (int)fourGroup.Key : 0;
                
            case PokerHandType.FullHouse:
            case PokerHandType.ThreeOfAKind:
                // 三张相同点数的牌，使用这个点数作为额外分数
                var threeGroup = cards.GroupBy(c => c.GetRank()).FirstOrDefault(g => g.Count() == 3);
                return threeGroup != null ? (int)threeGroup.Key : 0;
                
            case PokerHandType.TwoPair:
                // 两对，使用较大的对子点数作为主要额外分数，较小的对子点数作为次要额外分数
                var pairs = cards.GroupBy(c => c.GetRank()).Where(g => g.Count() == 2).OrderByDescending(g => (int)g.Key).ToList();
                if (pairs.Count >= 2)
                {
                    return (int)pairs[0].Key * 10 + (int)pairs[1].Key;
                }
                return 0;
                
            case PokerHandType.OnePair:
                // 一对，使用对子点数作为额外分数
                var pair = cards.GroupBy(c => c.GetRank()).FirstOrDefault(g => g.Count() == 2);
                return pair != null ? (int)pair.Key : 0;
                
            default:
                // 对于其他牌型，使用最大牌的点数作为额外分数
                return ranks.Count > 0 ? ranks[0] : 0;
        }
    }
}