using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 牌的花色枚举
/// </summary>
public enum CardSuit
{
    Spade,   // 黑桃
    Heart,   // 红心
    Club,    // 梅花
    Diamond  // 方块
}

/// <summary>
/// 牌的点数枚举（2-14，其中11=J，12=Q，13=K，1=A）
/// </summary>
public enum CardRank
{
    Ace = 1,
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Six = 6,
    Seven = 7,
    Eight = 8,
    Nine = 9,
    Ten = 10,
    Jack = 11,
    Queen = 12,
    King = 13
}

/// <summary>
/// 牌所在区域的枚举
/// </summary>
public enum CardArea
{
    Hand,    // 手牌区
    Play,    // 出牌区
    Wait     // 等待区
}

/// <summary>
/// 牌模型，定义单张牌的数据结构
/// </summary>
public class CardModel
{
    // 牌的唯一ID
    private int id;

    // 牌的花色
    private CardSuit suit;

    // 牌的点数
    private CardRank rank;

    // 牌所在区域
    private CardArea area;

    // 牌是否可点击（未被遮盖）
    private bool isClickable = true;

    // 牌的位置
    private Vector2 position;

    // 构造函数
    public CardModel(int id, CardSuit suit, CardRank rank)
    {
        this.id = id;
        this.suit = suit;
        this.rank = rank;
        this.area = CardArea.Hand; // 默认在手牌区
    }

    /// <summary>
    /// 获取牌的唯一ID
    /// </summary>
    public int GetID()
    {
        return id;
    }

    /// <summary>
    /// 获取牌的花色
    /// </summary>
    public CardSuit GetSuit()
    {
        return suit;
    }

    /// <summary>
    /// 获取牌的点数
    /// </summary>
    public CardRank GetRank()
    {
        return rank;
    }

    /// <summary>
    /// 获取牌所在区域
    /// </summary>
    public CardArea GetArea()
    {
        return area;
    }

    /// <summary>
    /// 设置牌所在区域
    /// </summary>
    public void SetArea(CardArea area)
    {
        this.area = area;
    }

    /// <summary>
    /// 判断牌是否在手牌区
    /// </summary>
    public bool IsInHandArea()
    {
        return area == CardArea.Hand;
    }

    /// <summary>
    /// 判断牌是否在出牌区
    /// </summary>
    public bool IsInPlayArea()
    {
        return area == CardArea.Play;
    }

    /// <summary>
    /// 判断牌是否在等待区
    /// </summary>
    public bool IsInWaitArea()
    {
        return area == CardArea.Wait;
    }

    /// <summary>
    /// 判断牌是否可点击
    /// </summary>
    public bool IsClickable()
    {
        return isClickable;
    }

    /// <summary>
    /// 设置牌是否可点击
    /// </summary>
    public void SetClickable(bool clickable)
    {
        isClickable = clickable;
    }

    /// <summary>
    /// 获取牌的位置
    /// </summary>
    public Vector2 GetPosition()
    {
        return position;
    }

    /// <summary>
    /// 设置牌的位置
    /// </summary>
    public void SetPosition(Vector2 pos)
    {
        position = pos;
    }

    /// <summary>
    /// 获取牌的完整描述（用于调试）
    /// </summary>
    public override string ToString()
    {
        return $"Card {id}: {suit} {rank}, Area: {area}, Clickable: {isClickable}";
    }
} 