﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 卡牌控制器，处理卡牌相关的交互逻辑
/// </summary>
public class CardController : MonoBehaviour
{
    // 单例模式
    public static CardController Instance { get; private set; }

    // 模型引用
    private CardsAreaModel cardsAreaModel;
    private OperationHistoryModel operationHistoryModel;

    // 初始化方法
    public void Initialize(CardsAreaModel cardsModel)
    {
        // 实现单例模式
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
            return;
        }

        this.cardsAreaModel = cardsModel;
        this.operationHistoryModel = GameController.Instance.GetOperationHistoryModel();
    }

    /// <summary>
    /// 处理卡牌点击事件
    /// </summary>
    public void OnCardClicked(CardModel card)
    {
        // 判断卡牌所在区域
        if (card.IsInHandArea())
        {
            // 如果是手牌区的牌，移动到出牌区
            MoveCardFromHandToPlay(card);
        }
        else if (card.IsInWaitArea())
        {
            // 如果是等待区的牌，移动到出牌区
            MoveCardFromWaitToPlay(card);
        }
        // 出牌区的牌不响应点击
    }

    /// <summary>
    /// 将牌从手牌区移动到出牌区
    /// </summary>
    private void MoveCardFromHandToPlay(CardModel card)
    {
        // 检查牌是否可点击
        if (!card.IsClickable()) return;
        
        // 记录操作
        operationHistoryModel.RecordOperation(OperationType.MoveCardToPlay, card);

        // 数据层和UI层移动牌
        GameController.Instance.GetUIManager().MoveCardFromHandToPlay(card);
        cardsAreaModel.MoveCardFromHandToPlay(card);
        
        // 播放动画
        GameController.Instance.GetUIManager().CardViewDict[card.GetID()].MoveToPosition(GameController.Instance.GetUIManager().GetPlayCardPos(), CheckPlayAreaFull);

        // 更新牌的遮盖关系
        UpdateCardsOverlap();

        // 更新UI显示
        UpdateHandCardNumUI();

        // 更新出牌区牌型显示
        UpdatePlayCardTypeUI();

    }

    /// <summary>
    /// 将牌从等待区移动到出牌区
    /// </summary>
    private void MoveCardFromWaitToPlay(CardModel card)
    {
        // 记录操作
        operationHistoryModel.RecordOperation(OperationType.MoveCardToPlay, card);
        
        // 移动牌
        cardsAreaModel.MoveCardFromWaitToPlay(card);
        
        // 更新UI显示
        UpdateUI();
        
        // 检查出牌区是否已有5张牌
        CheckPlayAreaFull();
    }

    /// <summary>
    /// 更新牌的遮盖关系
    /// </summary>
    public void UpdateCardsOverlap()
    {
        // 使用OverlapDetector计算遮盖关系
        GameController.Instance.GetOverlapDetector().CalculateOverlap(GameController.Instance.GetUIManager().HandAreaContainer);
    }

    /// <summary>
    /// 更新UI显示
    /// </summary>
    private void UpdateUI()
    {
        // 更新卡牌显示
        GameController.Instance.GetUIManager().UpdateCardsDisplay();
        
        // 更新按钮状态
        GameController.Instance.GetUIManager().UpdateButtonStates();
    }

    /// <summary>
    /// 更新UI手牌数显示
    /// </summary>
    private void UpdateHandCardNumUI()
    {
        // 更新卡牌显示
        GameController.Instance.GetUIManager().UpdateHandCardCountDisplay();
    }

    /// <summary>
    /// 检查出牌区是否已满
    /// </summary>
    private void CheckPlayAreaFull()
    {
        if (cardsAreaModel.IsPlayAreaFull())
        {
            // 通知游戏控制器处理出牌区已满的情况
            GameController.Instance.HandlePlayAreaFull();
        }

        if (cardsAreaModel.GetHandAreaCardCount() == 0)
        {
            // 检查游戏状态
            GameController.Instance.CheckGameState();
        }
    }

    /// <summary>
    /// 更新出牌区牌型显示
    /// </summary>
    private void UpdatePlayCardTypeUI()
    {
        GameController.Instance.HandlePlayType();
    }

    /// <summary>
    /// 将可点击的手牌移到等待区
    /// </summary>
    public void MoveClickableCardsToWaitArea()
    {
        // 获取可点击的手牌
        List<CardModel> clickableCards = cardsAreaModel.GetClickableHandAreaCards();
        
        // 如果没有可点击的牌，直接返回
        if (clickableCards.Count == 0) return;
        
        // 记录操作
        operationHistoryModel.RecordOperation(OperationType.MoveCardsToWait, clickableCards);
        
        // 移动牌
        cardsAreaModel.MoveClickableCardsToWaitArea();
        
        // 更新牌的遮盖关系
        UpdateCardsOverlap();
        
        // 更新UI显示
        UpdateUI();
    }

    /// <summary>
    /// 添加新牌到手牌区
    /// </summary>
    public void AddCardsToHandArea(List<CardModel> cards)
    {
        // 记录操作
        operationHistoryModel.RecordOperation(OperationType.AddCardsToHand, cards);
        
        // 添加牌
        cardsAreaModel.AddCardsToHandArea(cards);
        
        // 更新牌的遮盖关系
        UpdateCardsOverlap();
        
        // 更新UI显示
        UpdateUI();
    }

    /// <summary>
    /// 洗牌
    /// </summary>
    public void ShuffleHandCards()
    {
        // 记录操作
        List<CardModel> handCards = new List<CardModel>(cardsAreaModel.GetHandAreaCards());
        operationHistoryModel.RecordOperation(OperationType.ShuffleHandCards, handCards);
        
        // 洗牌
        cardsAreaModel.ShuffleHandAreaCards();
        
        // 更新牌的遮盖关系
        UpdateCardsOverlap();
        
        // 更新UI显示
        UpdateUI();
    }

    /// <summary>
    /// 反悔上一步操作
    /// </summary>
    public void UndoLastOperation()
    {
        // 判断是否可以反悔
        if (!operationHistoryModel.CanUndo()) return;
        
        // 获取最近一次操作
        OperationRecord lastOperation = operationHistoryModel.GetLastOperation();
        
        // 根据操作类型执行反悔操作
        switch (lastOperation.Type)
        {
            case OperationType.MoveCardToPlay:
                UndoMoveCardToPlay(lastOperation);
                break;
            case OperationType.MoveCardsToWait:
                UndoMoveCardsToWait(lastOperation);
                break;
            case OperationType.AddCardsToHand:
                UndoAddCardsToHand(lastOperation);
                break;
            case OperationType.ShuffleHandCards:
                // 洗牌操作无法真正撤销，只能重新洗一次
                ShuffleHandCards();
                break;
        }
        
        // 移除该操作记录
        operationHistoryModel.RemoveLastOperation();
        
        // 更新UI显示
        UpdateUI();
    }

    /// <summary>
    /// 反悔将牌移到出牌区的操作
    /// </summary>
    private void UndoMoveCardToPlay(OperationRecord operation)
    {
        foreach (var card in operation.Cards)
        {
            // 获取原始区域
            CardArea previousArea = operation.PreviousAreas[card.GetID()];
            
            // 从出牌区移除
            cardsAreaModel.RemoveCardFromPlayArea(card);
            
            // 添加到原始区域
            if (previousArea == CardArea.Hand)
            {
                cardsAreaModel.AddCardToHandArea(card);
            }
            else if (previousArea == CardArea.Wait)
            {
                cardsAreaModel.AddCardToWaitArea(card);
            }
        }
        
        // 更新牌的遮盖关系
        UpdateCardsOverlap();
    }

    /// <summary>
    /// 反悔将牌移到等待区的操作
    /// </summary>
    private void UndoMoveCardsToWait(OperationRecord operation)
    {
        foreach (var card in operation.Cards)
        {
            // 从等待区移除
            cardsAreaModel.RemoveCardFromWaitArea(card);
            
            // 添加到手牌区
            cardsAreaModel.AddCardToHandArea(card);
        }
        
        // 更新牌的遮盖关系
        UpdateCardsOverlap();
    }

    /// <summary>
    /// 反悔添加手牌的操作
    /// </summary>
    private void UndoAddCardsToHand(OperationRecord operation)
    {
        foreach (var card in operation.Cards)
        {
            // 从手牌区移除
            cardsAreaModel.RemoveCardFromHandArea(card);
        }
        
        // 更新牌的遮盖关系
        UpdateCardsOverlap();
    }
} 