using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 卡牌生成器，负责生成随机牌或特定牌组
/// </summary>
public class CardGenerator
{
    // 已生成的牌的ID计数器
    private int cardIdCounter = 0;
    
    // 已生成的牌记录，用于确保不重复生成
    private List<(CardSuit, CardRank)> generatedCards = new List<(CardSuit, CardRank)>();

    /// <summary>
    /// 生成单张牌
    /// </summary>
    public CardModel GenerateCard()
    {
        // 如果已经生成了所有52张牌，则返回null
        if (generatedCards.Count >= 52)
        {
            Debug.LogWarning("已经生成了所有52张牌，无法继续生成");
            return null;
        }

        // 随机生成花色和点数
        CardSuit suit;
        CardRank rank;
        
        do
        {
            // 随机花色
            suit = (CardSuit)Random.Range(0, 4);
            
            // 随机点数（2-14）
            rank = (CardRank)Random.Range(2, 15);
            
        } while (generatedCards.Contains((suit, rank))); // 确保不重复
        
        // 记录已生成的牌
        generatedCards.Add((suit, rank));
        
        // 创建牌模型
        CardModel card = new CardModel(cardIdCounter++, suit, rank);
        
        return card;
    }

    /// <summary>
    /// 生成多张牌
    /// </summary>
    public List<CardModel> GenerateCards(int count)
    {
        List<CardModel> cards = new List<CardModel>();
        
        for (int i = 0; i < count; i++)
        {
            CardModel card = GenerateCard();
            if (card != null)
            {
                cards.Add(card);
            }
            else
            {
                // 如果无法继续生成，则中断
                break;
            }
        }
        
        return cards;
    }

    /// <summary>
    /// 重置卡牌生成器，清空已生成的牌记录
    /// </summary>
    public void Reset()
    {
        generatedCards.Clear();
        cardIdCounter = 0;
    }

    /// <summary>
    /// 生成特定牌组（用于测试）
    /// </summary>
    public List<CardModel> GenerateSpecificHand(PokerHandType handType)
    {
        List<CardModel> cards = new List<CardModel>();
        
        switch (handType)
        {
            case PokerHandType.RoyalFlush:
                // 皇家同花顺（同一花色的A, K, Q, J, 10）
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Spade, CardRank.Ace));
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Spade, CardRank.King));
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Spade, CardRank.Queen));
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Spade, CardRank.Jack));
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Spade, CardRank.Ten));
                break;
                
            case PokerHandType.StraightFlush:
                // 同花顺（同一花色的连续5张牌）
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Heart, CardRank.Nine));
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Heart, CardRank.Eight));
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Heart, CardRank.Seven));
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Heart, CardRank.Six));
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Heart, CardRank.Five));
                break;
                
            case PokerHandType.FourOfAKind:
                // 四条（4张相同点数的牌）
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Spade, CardRank.Queen));
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Heart, CardRank.Queen));
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Club, CardRank.Queen));
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Diamond, CardRank.Queen));
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Spade, CardRank.Two));
                break;
                
            case PokerHandType.FullHouse:
                // 葫芦（3张相同点数的牌 + 2张相同点数的牌）
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Spade, CardRank.Jack));
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Heart, CardRank.Jack));
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Club, CardRank.Jack));
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Spade, CardRank.Eight));
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Heart, CardRank.Eight));
                break;
                
            case PokerHandType.Flush:
                // 同花（5张相同花色的牌）
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Club, CardRank.Ace));
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Club, CardRank.Ten));
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Club, CardRank.Eight));
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Club, CardRank.Six));
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Club, CardRank.Four));
                break;
                
            case PokerHandType.Straight:
                // 顺子（5张连续点数的牌）
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Spade, CardRank.Ten));
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Heart, CardRank.Nine));
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Club, CardRank.Eight));
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Diamond, CardRank.Seven));
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Spade, CardRank.Six));
                break;
                
            case PokerHandType.ThreeOfAKind:
                // 三条（3张相同点数的牌）
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Spade, CardRank.Seven));
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Heart, CardRank.Seven));
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Club, CardRank.Seven));
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Spade, CardRank.King));
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Heart, CardRank.Five));
                break;
                
            case PokerHandType.TwoPair:
                // 两对（2张相同点数的牌 + 2张相同点数的牌）
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Spade, CardRank.Nine));
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Heart, CardRank.Nine));
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Spade, CardRank.Five));
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Heart, CardRank.Five));
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Spade, CardRank.Two));
                break;
                
            case PokerHandType.OnePair:
                // 一对（2张相同点数的牌）
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Spade, CardRank.Eight));
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Heart, CardRank.Eight));
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Spade, CardRank.King));
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Heart, CardRank.Queen));
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Club, CardRank.Three));
                break;
                
            default:
                // 高牌（5张不组成任何牌型的牌）
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Spade, CardRank.Ace));
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Heart, CardRank.Jack));
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Club, CardRank.Nine));
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Diamond, CardRank.Seven));
                cards.Add(new CardModel(cardIdCounter++, CardSuit.Spade, CardRank.Two));
                break;
        }
        
        // 记录已生成的牌
        foreach (var card in cards)
        {
            generatedCards.Add((card.GetSuit(), card.GetRank()));
        }
        
        return cards;
    }
} 