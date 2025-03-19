using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 操作类型枚举
/// </summary>
public enum OperationType
{
    MoveCardToPlay,       // 将牌移到出牌区
    MoveCardsToWait,      // 将牌移到等待区
    AddCardsToHand,       // 增加手牌
    ShuffleHandCards      // 洗牌
}

/// <summary>
/// 单次操作的记录
/// </summary>
public class OperationRecord
{
    // 操作类型
    public OperationType Type { get; private set; }
    
    // 操作涉及的牌
    public List<CardModel> Cards { get; private set; }
    
    // 操作前牌所在的区域
    public Dictionary<int, CardArea> PreviousAreas { get; private set; }
    
    // 构造函数
    public OperationRecord(OperationType type, List<CardModel> cards)
    {
        Type = type;
        Cards = new List<CardModel>(cards);
        PreviousAreas = new Dictionary<int, CardArea>();
        
        // 记录每张牌的原始区域
        foreach (var card in cards)
        {
            PreviousAreas[card.GetID()] = card.GetArea();
        }
    }
    
    // 单张牌的简化构造
    public OperationRecord(OperationType type, CardModel card) 
        : this(type, new List<CardModel> { card })
    {
    }
}

/// <summary>
/// 操作历史模型，负责记录玩家的操作历史和实现反悔功能
/// </summary>
public class OperationHistoryModel
{
    // 操作历史记录
    private List<OperationRecord> operationRecords = new List<OperationRecord>();
    
    // 最大记录数量
    private const int MaxRecordCount = 20;

    /// <summary>
    /// 记录一次操作
    /// </summary>
    public void RecordOperation(OperationType type, List<CardModel> cards)
    {
        // 创建新的操作记录
        OperationRecord record = new OperationRecord(type, cards);
        
        // 添加到历史记录
        operationRecords.Add(record);
        
        // 如果记录数量超过上限，移除最早的记录
        if (operationRecords.Count > MaxRecordCount)
        {
            operationRecords.RemoveAt(0);
        }
    }
    
    /// <summary>
    /// 记录单张牌的操作
    /// </summary>
    public void RecordOperation(OperationType type, CardModel card)
    {
        RecordOperation(type, new List<CardModel> { card });
    }

    /// <summary>
    /// 判断是否可以反悔（历史记录中有操作）
    /// </summary>
    public bool CanUndo()
    {
        return operationRecords.Count > 0;
    }

    /// <summary>
    /// 获取最近一次操作
    /// </summary>
    public OperationRecord GetLastOperation()
    {
        if (CanUndo())
        {
            return operationRecords[operationRecords.Count - 1];
        }
        return null;
    }

    /// <summary>
    /// 移除最近一次操作记录
    /// </summary>
    public void RemoveLastOperation()
    {
        if (CanUndo())
        {
            operationRecords.RemoveAt(operationRecords.Count - 1);
        }
    }

    /// <summary>
    /// 清空所有操作记录
    /// </summary>
    public void ClearAllOperations()
    {
        operationRecords.Clear();
    }
} 