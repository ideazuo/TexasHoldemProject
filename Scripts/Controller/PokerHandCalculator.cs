using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 扑克牌型枚举，按照德州扑克规则从小到大排序
/// </summary>
public enum PokerHandType
{
    Null = 0,          // 无牌型
    HighCard = 1,      // 高牌
    OnePair = 2,       // 一对
    TwoPair = 3,       // 两对
    ThreeOfAKind = 4,  // 三条
    Straight = 5,      // 顺子
    Flush = 6,         // 同花
    FullHouse = 7,     // 葫芦
    FourOfAKind = 8,   // 四条
    StraightFlush = 9, // 同花顺
    FiveOfAKind = 10,  // 五条
    FlushFullHouse = 11, // 同花葫芦
    FlushFiveOfAKind = 12 // 同花五条
}

/// <summary>
/// 扑克牌型计算器，实现德州扑克牌型的计算逻辑
/// </summary>
public static class PokerHandCalculator
{
    /// <summary>
    /// 计算牌的得分
    /// </summary>
    public static int CalculateScore(List<CardView> cards)
    {
        // 判断牌的数量
        if (cards == null || cards.Count > 5)
        {
            Debug.LogError("计算牌型需要0-5张牌");
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
    /// 评估牌的牌型
    /// </summary>
    private static PokerHandType EvaluateHand(List<CardView> cards)
    {
        // 检查牌数量
        if (cards == null || cards.Count == 0)
        {
            return PokerHandType.Null;
        }
        
        // 检查同花五条（FlushFiveOfAKind）
        if (IsFlushFiveOfAKind(cards))
        {
            return PokerHandType.FlushFiveOfAKind;
        }
        
        // 检查同花葫芦（FlushFullHouse）
        if (IsFlushFullHouse(cards))
        {
            return PokerHandType.FlushFullHouse;
        }
        
        // 检查五条（FiveOfAKind）
        if (IsFiveOfAKind(cards))
        {
            return PokerHandType.FiveOfAKind;
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
        
        // 如果牌数量大于0，默认为高牌（HighCard）
        if (cards.Count > 0)
        {
            return PokerHandType.HighCard;
        }
        
        // 否则为无牌型
        return PokerHandType.Null;
    }

    /// <summary>
    /// 获取排序后的点数列表
    /// </summary>
    private static List<int> GetSortedRanks(List<CardView> cards)
    {
        if (cards == null || cards.Count == 0)
        {
            return new List<int>();
        }
        
        return cards.Select(c => (int)c.GetCardModel().GetRank()).OrderByDescending(r => r).ToList();
    }

    /// <summary>
    /// 检查是否是同花五条
    /// </summary>
    private static bool IsFlushFiveOfAKind(List<CardView> cards)
    {
        if (cards == null || cards.Count < 5)
        {
            return false;
        }
        
        // 同花五条：5张相同花色和相同点数的牌
        CardSuit firstSuit = cards[0].GetCardModel().GetSuit();
        CardRank firstRank = cards[0].GetCardModel().GetRank();
        
        foreach (var card in cards)
        {
            if (card.GetCardModel().GetSuit() != firstSuit || card.GetCardModel().GetRank() != firstRank)
            {
                return false;
            }
        }
        
        return true;
    }
    
    /// <summary>
    /// 检查是否是同花葫芦
    /// </summary>
    private static bool IsFlushFullHouse(List<CardView> cards)
    {
        if (cards == null || cards.Count != 5)
        {
            return false;
        }
        
        // 先检查是否为同花
        CardSuit firstSuit = cards[0].GetCardModel().GetSuit();
        bool isFlush = cards.All(c => c.GetCardModel().GetSuit() == firstSuit);
        
        if (!isFlush)
        {
            return false;
        }
        
        // 再检查是否为葫芦结构
        // 统计每个点数出现的次数
        Dictionary<CardRank, int> rankCounts = new Dictionary<CardRank, int>();
        foreach (var card in cards)
        {
            if (!rankCounts.ContainsKey(card.GetCardModel().GetRank()))
            {
                rankCounts[card.GetCardModel().GetRank()] = 0;
            }
            rankCounts[card.GetCardModel().GetRank()]++;
        }
        
        // 葫芦结构必须有且仅有两种点数，一种出现3次，另一种出现2次
        return rankCounts.Count == 2 && rankCounts.Values.Contains(3) && rankCounts.Values.Contains(2);
    }
    
    /// <summary>
    /// 检查是否是五条
    /// </summary>
    private static bool IsFiveOfAKind(List<CardView> cards)
    {
        if (cards == null || cards.Count < 5)
        {
            return false;
        }
        
        // 检查所有牌的点数是否相同
        CardRank firstRank = cards[0].GetCardModel().GetRank();
        return cards.All(c => c.GetCardModel().GetRank() == firstRank);
    }

    /// <summary>
    /// 检查是否是同花顺
    /// </summary>
    private static bool IsStraightFlush(List<CardView> cards)
    {
        if (cards == null || cards.Count != 5)
        {
            return false;
        }
        
        // 同花顺：同一花色的顺子
        return IsFlush(cards) && IsStraight(cards);
    }

    /// <summary>
    /// 检查是否是四条
    /// </summary>
    private static bool IsFourOfAKind(List<CardView> cards)
    {
        if (cards == null || cards.Count < 4)
        {
            return false;
        }
        
        // 统计每个点数出现的次数
        Dictionary<CardRank, int> rankCounts = new Dictionary<CardRank, int>();
        foreach (var card in cards)
        {
            if (!rankCounts.ContainsKey(card.GetCardModel().GetRank()))
            {
                rankCounts[card.GetCardModel().GetRank()] = 0;
            }
            rankCounts[card.GetCardModel().GetRank()]++;
        }
        
        // 如果有任何一个点数出现了4次或更多，则为四条
        return rankCounts.Values.Any(count => count >= 4);
    }

    /// <summary>
    /// 检查是否是葫芦
    /// </summary>
    private static bool IsFullHouse(List<CardView> cards)
    {
        if (cards == null || cards.Count != 5)
        {
            return false;
        }
        
        // 统计每个点数出现的次数
        Dictionary<CardRank, int> rankCounts = new Dictionary<CardRank, int>();
        foreach (var card in cards)
        {
            if (!rankCounts.ContainsKey(card.GetCardModel().GetRank()))
            {
                rankCounts[card.GetCardModel().GetRank()] = 0;
            }
            rankCounts[card.GetCardModel().GetRank()]++;
        }
        
        // 葫芦结构必须有且仅有两种点数，一种出现3次，另一种出现2次
        return rankCounts.Count == 2 && rankCounts.Values.Contains(3) && rankCounts.Values.Contains(2);
    }

    /// <summary>
    /// 检查是否是同花
    /// </summary>
    private static bool IsFlush(List<CardView> cards)
    {
        if (cards == null || cards.Count < 5)
        {
            return false;
        }
        
        // 同花：所有牌都是相同花色
        CardSuit firstSuit = cards[0].GetCardModel().GetSuit();
        return cards.All(c => c.GetCardModel().GetSuit() == firstSuit);
    }

    /// <summary>
    /// 检查是否是顺子
    /// </summary>
    private static bool IsStraight(List<CardView> cards)
    {
        if (cards == null || cards.Count != 5)
        {
            return false;
        }
        
        // 获取所有牌的点数，并排序
        List<int> ranks = cards.Select(c => (int)c.GetCardModel().GetRank()).OrderBy(r => r).ToList();
        
        // 特殊情况：A-2-3-4-5 顺子
        bool isLowStraight = ranks.Contains((int)CardRank.Ace) && 
                             ranks.Contains((int)CardRank.Two) && 
                             ranks.Contains((int)CardRank.Three) && 
                             ranks.Contains((int)CardRank.Four) && 
                             ranks.Contains((int)CardRank.Five);
        
        if (isLowStraight)
        {
            return true;
        }
        
        // 特殊情况：10-J-Q-K-A 顺子
        bool isHighStraight = ranks.Contains((int)CardRank.Ten) && 
                              ranks.Contains((int)CardRank.Jack) && 
                              ranks.Contains((int)CardRank.Queen) && 
                              ranks.Contains((int)CardRank.King) && 
                              ranks.Contains((int)CardRank.Ace);
        
        if (isHighStraight)
        {
            return true;
        }
        
        // 常规顺子：检查排序后的点数是否连续
        for (int i = 1; i < ranks.Count; i++)
        {
            if (ranks[i] != ranks[i-1] + 1)
            {
                return false;
            }
        }
        
        return true;
    }

    /// <summary>
    /// 检查是否是三条
    /// </summary>
    private static bool IsThreeOfAKind(List<CardView> cards)
    {
        if (cards == null || cards.Count < 3)
        {
            return false;
        }
        
        // 统计每个点数出现的次数
        Dictionary<CardRank, int> rankCounts = new Dictionary<CardRank, int>();
        foreach (var card in cards)
        {
            if (!rankCounts.ContainsKey(card.GetCardModel().GetRank()))
            {
                rankCounts[card.GetCardModel().GetRank()] = 0;
            }
            rankCounts[card.GetCardModel().GetRank()]++;
        }
        
        // 如果是5张牌，确保不是葫芦（葫芦已经在前面的判断中被排除）
        if (cards.Count == 5)
        {
            return rankCounts.Values.Any(count => count >= 3);
        }
        else
        {
            // 小于5张牌时，只需要判断是否有3张相同点数的牌
            return rankCounts.Values.Any(count => count >= 3);
        }
    }

    /// <summary>
    /// 检查是否是两对
    /// </summary>
    private static bool IsTwoPair(List<CardView> cards)
    {
        if (cards == null || cards.Count < 4)
        {
            return false;
        }
        
        // 统计每个点数出现的次数
        Dictionary<CardRank, int> rankCounts = new Dictionary<CardRank, int>();
        foreach (var card in cards)
        {
            if (!rankCounts.ContainsKey(card.GetCardModel().GetRank()))
            {
                rankCounts[card.GetCardModel().GetRank()] = 0;
            }
            rankCounts[card.GetCardModel().GetRank()]++;
        }
        
        // 计算有多少对子
        int pairCount = rankCounts.Values.Count(count => count >= 2);
        
        // 如果有2对或更多，则为两对
        return pairCount >= 2;
    }

    /// <summary>
    /// 检查是否是一对
    /// </summary>
    private static bool IsOnePair(List<CardView> cards)
    {
        if (cards == null || cards.Count < 2)
        {
            return false;
        }
        
        // 统计每个点数出现的次数
        Dictionary<CardRank, int> rankCounts = new Dictionary<CardRank, int>();
        foreach (var card in cards)
        {
            if (!rankCounts.ContainsKey(card.GetCardModel().GetRank()))
            {
                rankCounts[card.GetCardModel().GetRank()] = 0;
            }
            rankCounts[card.GetCardModel().GetRank()]++;
        }
        
        // 如果有任何一个点数出现了2次或更多，则为对子
        // 但如果有两对或三条，则不是一对（这些牌型已在前面的判断中被识别）
        if (cards.Count == 5)
        {
            int pairCount = rankCounts.Values.Count(count => count == 2);
            int threeCount = rankCounts.Values.Count(count => count >= 3);
            
            // 只有一个对子且没有三条
            return pairCount == 1 && threeCount == 0;
        }
        else
        {
            // 小于5张牌时，只要有对子就返回true（因为已经排除了两对和三条的情况）
            return rankCounts.Values.Any(count => count >= 2);
        }
    }

    /// <summary>
    /// 计算基础分数（基于牌型）
    /// </summary>
    private static int CalculateBaseScore(PokerHandType handType)
    {
        // 每种牌型的基础分数
        switch (handType)
        {
            case PokerHandType.FlushFiveOfAKind:
                return 12000;
            case PokerHandType.FlushFullHouse:
                return 11000;
            case PokerHandType.FiveOfAKind:
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
            case PokerHandType.Null:
                return 0;
            default:
                return 0;
        }
    }

    /// <summary>
    /// 计算额外分数（基于牌点数）
    /// </summary>
    private static int CalculateExtraScore(List<CardView> cards, PokerHandType handType)
    {
        if (cards == null || cards.Count == 0)
        {
            return 0;
        }
        
        // 获取排序后的点数
        List<int> ranks = GetSortedRanks(cards);
        
        // 基于不同牌型的额外分数计算
        switch (handType)
        {
            case PokerHandType.FlushFiveOfAKind:
            case PokerHandType.FiveOfAKind:
                // 五条，使用这个点数作为额外分数
                return cards.Count > 0 ? (int)cards[0].GetCardModel().GetRank() : 0;
                
            case PokerHandType.FlushFullHouse:
                // 同花葫芦，使用三张相同点数的牌点数作为额外分数
                Dictionary<CardRank, int> fbRankCounts = new Dictionary<CardRank, int>();
                foreach (var card in cards)
                {
                    if (!fbRankCounts.ContainsKey(card.GetCardModel().GetRank()))
                    {
                        fbRankCounts[card.GetCardModel().GetRank()] = 0;
                    }
                    fbRankCounts[card.GetCardModel().GetRank()]++;
                }
                var threeOfAKindRank = fbRankCounts.FirstOrDefault(kv => kv.Value == 3).Key;
                return (int)threeOfAKindRank;
                
            case PokerHandType.FourOfAKind:
                // 四条，使用这个点数作为额外分数
                Dictionary<CardRank, int> focRankCounts = new Dictionary<CardRank, int>();
                foreach (var card in cards)
                {
                    if (!focRankCounts.ContainsKey(card.GetCardModel().GetRank()))
                    {
                        focRankCounts[card.GetCardModel().GetRank()] = 0;
                    }
                    focRankCounts[card.GetCardModel().GetRank()]++;
                }
                var fourCardRank = focRankCounts.FirstOrDefault(kv => kv.Value >= 4).Key;
                return (int)fourCardRank;
                
            case PokerHandType.FullHouse:
            case PokerHandType.ThreeOfAKind:
                // 葫芦或三条，使用三张相同点数的牌点数作为额外分数
                Dictionary<CardRank, int> fhRankCounts = new Dictionary<CardRank, int>();
                foreach (var card in cards)
                {
                    if (!fhRankCounts.ContainsKey(card.GetCardModel().GetRank()))
                    {
                        fhRankCounts[card.GetCardModel().GetRank()] = 0;
                    }
                    fhRankCounts[card.GetCardModel().GetRank()]++;
                }
                var threeCardRank = fhRankCounts.FirstOrDefault(kv => kv.Value >= 3).Key;
                return (int)threeCardRank;
                
            case PokerHandType.TwoPair:
                // 两对，使用较大的对子点数作为主要额外分数，较小的对子点数作为次要额外分数
                Dictionary<CardRank, int> tpRankCounts = new Dictionary<CardRank, int>();
                foreach (var card in cards)
                {
                    if (!tpRankCounts.ContainsKey(card.GetCardModel().GetRank()))
                    {
                        tpRankCounts[card.GetCardModel().GetRank()] = 0;
                    }
                    tpRankCounts[card.GetCardModel().GetRank()]++;
                }
                var pairRanks = tpRankCounts.Where(kv => kv.Value >= 2)
                                            .OrderByDescending(kv => (int)kv.Key)
                                            .ToList();
                if (pairRanks.Count >= 2)
                {
                    return (int)pairRanks[0].Key * 10 + (int)pairRanks[1].Key;
                }
                return 0;
                
            case PokerHandType.OnePair:
                // 一对，使用对子点数作为额外分数
                Dictionary<CardRank, int> opRankCounts = new Dictionary<CardRank, int>();
                foreach (var card in cards)
                {
                    if (!opRankCounts.ContainsKey(card.GetCardModel().GetRank()))
                    {
                        opRankCounts[card.GetCardModel().GetRank()] = 0;
                    }
                    opRankCounts[card.GetCardModel().GetRank()]++;
                }
                var pairRank = opRankCounts.FirstOrDefault(kv => kv.Value >= 2).Key;
                return (int)pairRank;
                
            default:
                // 对于其他牌型，使用最大牌的点数作为额外分数
                return ranks.Count > 0 ? ranks[0] : 0;
        }
    }
}