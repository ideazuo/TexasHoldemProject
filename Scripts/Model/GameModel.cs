using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏状态枚举
/// </summary>
public enum GameState
{
    NotStarted, // 未开始
    Playing,    // 游戏中
    Ended       // 已结束
}

/// <summary>
/// 游戏模型，负责管理游戏的核心数据和状态
/// </summary>
public class GameModel
{
    // 游戏当前关卡ID
    private int levelID = 0;

    // 游戏当前得分
    private int score = 0;

    // 游戏当前状态
    private GameState gameState = GameState.NotStarted;

    // 游戏结果（胜利/失败）
    private bool isWin = false;

    /// <summary>
    /// 获取当前关卡ID
    /// </summary>
    public int GetLevelID()
    {
        return levelID;
    }

    /// <summary>
    /// 设置关卡ID
    /// </summary>
    public void SetLevelID(int id)
    {
        levelID = id;
    }

    /// <summary>
    /// 增加关卡ID
    /// </summary>
    public void IncreaseLevelID()
    {
        levelID++;
    }

    /// <summary>
    /// 获取当前得分
    /// </summary>
    public int GetScore()
    {
        return score;
    }

    /// <summary>
    /// 增加得分
    /// </summary>
    public void AddScore(int value)
    {
        score += value;
    }

    /// <summary>
    /// 重置得分
    /// </summary>
    public void ResetScore()
    {
        score = 0;
    }

    /// <summary>
    /// 获取游戏状态
    /// </summary>
    public GameState GetGameState()
    {
        return gameState;
    }

    /// <summary>
    /// 设置游戏状态
    /// </summary>
    public void SetGameState(GameState state)
    {
        gameState = state;
    }

    /// <summary>
    /// 判断游戏是否进行中
    /// </summary>
    public bool IsGamePlaying()
    {
        return gameState == GameState.Playing;
    }

    /// <summary>
    /// 获取游戏结果
    /// </summary>
    public bool GetGameResult()
    {
        return isWin;
    }

    /// <summary>
    /// 设置游戏结果
    /// </summary>
    public void SetGameResult(bool win)
    {
        isWin = win;
    }
} 