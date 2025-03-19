using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// 牌区域模型，负责管理不同区域的牌组数据
/// </summary>
public class CardsAreaModel
{
    // 手牌区的牌集合
    private List<CardModel> handAreaCards = new List<CardModel>();

    // 出牌区的牌集合
    private List<CardModel> playAreaCards = new List<CardModel>();

    // 等待区的牌集合
    private List<CardModel> waitAreaCards = new List<CardModel>();

    /// <summary>
    /// 获取手牌区的牌集合
    /// </summary>
    public List<CardModel> GetHandAreaCards()
    {
        return handAreaCards;
    }

    /// <summary>
    /// 获取出牌区的牌集合
    /// </summary>
    public List<CardModel> GetPlayAreaCards()
    {
        return playAreaCards;
    }

    /// <summary>
    /// 获取等待区的牌集合
    /// </summary>
    public List<CardModel> GetWaitAreaCards()
    {
        return waitAreaCards;
    }

    /// <summary>
    /// 添加牌到手牌区
    /// </summary>
    public void AddCardToHandArea(CardModel card)
    {
        card.SetArea(CardArea.Hand);
        handAreaCards.Add(card);
    }

    /// <summary>
    /// 添加多张牌到手牌区
    /// </summary>
    public void AddCardsToHandArea(List<CardModel> cards)
    {
        foreach (var card in cards)
        {
            AddCardToHandArea(card);
        }
    }

    /// <summary>
    /// 添加牌到出牌区
    /// </summary>
    public void AddCardToPlayArea(CardModel card)
    {
        card.SetArea(CardArea.Play);
        playAreaCards.Add(card);
    }

    /// <summary>
    /// 添加牌到等待区
    /// </summary>
    public void AddCardToWaitArea(CardModel card)
    {
        card.SetArea(CardArea.Wait);
        waitAreaCards.Add(card);
    }

    /// <summary>
    /// 移除手牌区的牌
    /// </summary>
    public void RemoveCardFromHandArea(CardModel card)
    {
        handAreaCards.Remove(card);
    }

    /// <summary>
    /// 移除出牌区的牌
    /// </summary>
    public void RemoveCardFromPlayArea(CardModel card)
    {
        playAreaCards.Remove(card);
    }

    /// <summary>
    /// 移除等待区的牌
    /// </summary>
    public void RemoveCardFromWaitArea(CardModel card)
    {
        waitAreaCards.Remove(card);
    }

    /// <summary>
    /// 清空出牌区
    /// </summary>
    public void ClearPlayArea()
    {
        playAreaCards.Clear();
    }

    /// <summary>
    /// 获取手牌区的牌数量
    /// </summary>
    public int GetHandAreaCardCount()
    {
        return handAreaCards.Count;
    }

    /// <summary>
    /// 获取出牌区的牌数量
    /// </summary>
    public int GetPlayAreaCardCount()
    {
        return playAreaCards.Count;
    }

    /// <summary>
    /// 获取等待区的牌数量
    /// </summary>
    public int GetWaitAreaCardCount()
    {
        return waitAreaCards.Count;
    }

    /// <summary>
    /// 判断手牌区是否为空
    /// </summary>
    public bool IsHandAreaEmpty()
    {
        return handAreaCards.Count == 0;
    }

    /// <summary>
    /// 判断出牌区是否已满（5张牌）
    /// </summary>
    public bool IsPlayAreaFull()
    {
        return playAreaCards.Count >= 5;
    }

    /// <summary>
    /// 获取手牌区中可点击的牌
    /// </summary>
    public List<CardModel> GetClickableHandAreaCards()
    {
        return handAreaCards.Where(card => card.IsClickable()).ToList();
    }

    /// <summary>
    /// 将手牌区中可点击的牌移到等待区
    /// </summary>
    public void MoveClickableCardsToWaitArea()
    {
        // 获取可点击的牌
        List<CardModel> clickableCards = GetClickableHandAreaCards();

        // 将牌移到等待区
        foreach (var card in clickableCards)
        {
            // 从手牌区移除
            handAreaCards.Remove(card);
            // 添加到等待区
            AddCardToWaitArea(card);
        }
    }

    /// <summary>
    /// 将牌从手牌区移到出牌区
    /// </summary>
    public void MoveCardFromHandToPlay(CardModel card)
    {
        if (card.IsInHandArea() && card.IsClickable())
        {
            // 从手牌区移除
            RemoveCardFromHandArea(card);
            // 添加到出牌区
            AddCardToPlayArea(card);
        }
    }

    /// <summary>
    /// 将牌从等待区移到出牌区
    /// </summary>
    public void MoveCardFromWaitToPlay(CardModel card)
    {
        if (card.IsInWaitArea())
        {
            // 从等待区移除
            RemoveCardFromWaitArea(card);
            // 添加到出牌区
            AddCardToPlayArea(card);
        }
    }

    /// <summary>
    /// 洗牌（打乱手牌区的牌顺序）
    /// </summary>
    public void ShuffleHandAreaCards()
    {
        // 复制当前手牌
        List<CardModel> currentCards = new List<CardModel>(handAreaCards);
        // 清空手牌区
        handAreaCards.Clear();
        
        // 随机打乱顺序
        System.Random rng = new System.Random();
        int n = currentCards.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            CardModel temp = currentCards[k];
            currentCards[k] = currentCards[n];
            currentCards[n] = temp;
        }
        
        // 重新添加到手牌区
        handAreaCards.AddRange(currentCards);
    }
} 