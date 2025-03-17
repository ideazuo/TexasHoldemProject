using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 关卡配置，定义各关卡的配置参数
/// </summary>
public static class LevelConfig
{
    // 关卡1初始牌数量
    private static int level1InitialCardCount = 20;
    
    // 关卡2初始牌数量
    private static int level2InitialCardCount = 30;
    
    // 关卡1增加手牌数量
    private static int level1AdditionalCardCount = 5;
    
    // 关卡2增加手牌数量
    private static int level2AdditionalCardCount = 3;
    
    // 通关所需分数
    private static int winScore = 30000;

    /// <summary>
    /// 获取指定关卡的初始牌数量
    /// </summary>
    public static int GetInitialCardCount(int levelID)
    {
        switch (levelID)
        {
            case 1:
                return level1InitialCardCount;
            case 2:
                return level2InitialCardCount;
            default:
                Debug.LogWarning("未知关卡ID: " + levelID);
                return 10; // 默认值
        }
    }

    /// <summary>
    /// 获取指定关卡的增加手牌数量
    /// </summary>
    public static int GetAdditionalCardCount(int levelID)
    {
        switch (levelID)
        {
            case 1:
                return level1AdditionalCardCount;
            case 2:
                return level2AdditionalCardCount;
            default:
                Debug.LogWarning("未知关卡ID: " + levelID);
                return 3; // 默认值
        }
    }

    /// <summary>
    /// 获取通关所需分数
    /// </summary>
    public static int GetWinScore()
    {
        return winScore;
    }

    /// <summary>
    /// 设置关卡1初始牌数量
    /// </summary>
    public static void SetLevel1InitialCardCount(int count)
    {
        level1InitialCardCount = Mathf.Clamp(count, 5, 40);
    }

    /// <summary>
    /// 设置关卡2初始牌数量
    /// </summary>
    public static void SetLevel2InitialCardCount(int count)
    {
        level2InitialCardCount = Mathf.Clamp(count, 5, 40);
    }

    /// <summary>
    /// 设置关卡1增加手牌数量
    /// </summary>
    public static void SetLevel1AdditionalCardCount(int count)
    {
        level1AdditionalCardCount = Mathf.Clamp(count, 1, 10);
    }

    /// <summary>
    /// 设置关卡2增加手牌数量
    /// </summary>
    public static void SetLevel2AdditionalCardCount(int count)
    {
        level2AdditionalCardCount = Mathf.Clamp(count, 1, 10);
    }

    /// <summary>
    /// 设置通关所需分数
    /// </summary>
    public static void SetWinScore(int score)
    {
        winScore = Mathf.Max(1000, score);
    }
} 