using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 按钮控制器，处理四个功能按钮的点击事件
/// </summary>
public class ButtonController : MonoBehaviour
{
    // 单例模式
    public static ButtonController Instance { get; private set; }

    // 模型引用
    private GameModel gameModel;
    private CardsAreaModel cardsAreaModel;
    private OperationHistoryModel operationHistoryModel;

    // 初始化方法
    public void Initialize(GameModel gameModel, CardsAreaModel cardsAreaModel, OperationHistoryModel operationHistoryModel)
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

        this.gameModel = gameModel;
        this.cardsAreaModel = cardsAreaModel;
        this.operationHistoryModel = operationHistoryModel;
    }

    /// <summary>
    /// 处理"移除手牌区"按钮点击事件
    /// </summary>
    public void OnMoveToWaitButtonClicked()
    {
        // 判断游戏是否进行中
        if (!gameModel.IsGamePlaying()) return;
        
        // 调用卡牌控制器的方法，将可点击的手牌移到等待区
        GameController.Instance.GetCardController().MoveClickableCardsToWaitArea();
    }

    /// <summary>
    /// 处理"增加手牌"按钮点击事件
    /// </summary>
    public void OnAddCardsButtonClicked()
    {
        // 判断游戏是否进行中
        if (!gameModel.IsGamePlaying()) return;
        
        // 获取当前关卡应该增加的牌数量
        int cardCount = LevelConfig.GetAdditionalCardCount(gameModel.GetLevelID());
        
        // 生成新牌
        List<CardModel> newCards = GameController.Instance.GetCardGenerator().GenerateCards(cardCount);
        
        // 调用卡牌控制器的方法，添加牌到手牌区
        //GameController.Instance.GetCardController().AddCardsToHandArea(newCards);
    }

    /// <summary>
    /// 处理"反悔"按钮点击事件
    /// </summary>
    public void OnUndoButtonClicked()
    {
        // 判断游戏是否进行中
        if (!gameModel.IsGamePlaying()) return;
        
        // 判断是否可以反悔
        if (!CanUndo()) return;
        
        // 调用卡牌控制器的方法，执行反悔操作
        GameController.Instance.GetCardController().UndoLastOperation();
    }

    /// <summary>
    /// 处理"洗牌"按钮点击事件
    /// </summary>
    public void OnShuffleButtonClicked()
    {
        // 判断游戏是否进行中
        if (!gameModel.IsGamePlaying()) return;
        
        // 判断手牌区是否有至少2张牌
        if (cardsAreaModel.GetHandAreaCardCount() < 2) return;
        
        // 调用卡牌控制器的方法，执行洗牌操作
        GameController.Instance.GetCardController().ShuffleHandCards();
    }

    /// <summary>
    /// 判断是否可以反悔
    /// </summary>
    public bool CanUndo()
    {
        return operationHistoryModel.CanUndo();
    }
} 