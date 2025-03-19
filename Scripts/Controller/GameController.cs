using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏的核心控制器，负责协调各个模块
/// </summary>
public class GameController : MonoBehaviour
{
    // 单例模式
    public static GameController Instance { get; private set; }

    // 模型引用
    private GameModel gameModel;
    private CardsAreaModel cardsAreaModel;
    private OperationHistoryModel operationHistoryModel;

    // 视图引用
    private UIManager uiManager;

    // 控制器引用
    private CardController cardController;
    private ButtonController buttonController;

    // 工具类引用
    private CardGenerator cardGenerator;
    private OverlapDetector overlapDetector;

    private void Awake()
    {
        // 实现单例模式
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 初始化游戏
        InitializeGame();
    }

    /// <summary>
    /// 初始化游戏所有必要的模型和视图
    /// </summary>
    private void InitializeGame()
    {
        // 初始化模型
        InitializeModels();

        // 初始化视图
        InitializeViews();

        // 初始化控制器
        InitializeControllers();

        // 初始化工具类
        InitializeUtils();

        // 显示开始界面
        uiManager.ShowStartPanel();
    }

    /// <summary>
    /// 初始化所有模型
    /// </summary>
    private void InitializeModels()
    {
        // 创建并初始化游戏模型
        gameModel = new GameModel();
        
        // 创建并初始化牌区域模型
        cardsAreaModel = new CardsAreaModel();
        
        // 创建并初始化操作历史模型
        operationHistoryModel = new OperationHistoryModel();
    }

    /// <summary>
    /// 初始化所有视图
    /// </summary>
    private void InitializeViews()
    {
        // 查找并获取UI管理器
        uiManager = FindObjectOfType<UIManager>();
        if (uiManager == null)
        {
            Debug.LogError("未找到UIManager组件！");
        }
        else
        {
            // 初始化UI管理器
            uiManager.Initialize();
        }
    }

    /// <summary>
    /// 初始化所有控制器
    /// </summary>
    private void InitializeControllers()
    {
        // 创建并初始化卡牌控制器
        cardController = gameObject.AddComponent<CardController>();
        cardController.Initialize(cardsAreaModel);
        
        // 创建并初始化按钮控制器
        buttonController = gameObject.AddComponent<ButtonController>();
        buttonController.Initialize(gameModel, cardsAreaModel, operationHistoryModel);
    }

    /// <summary>
    /// 初始化工具类
    /// </summary>
    private void InitializeUtils()
    {
        // 创建并初始化卡牌生成器
        cardGenerator = new CardGenerator();
        
        // 创建并初始化重叠检测器
        overlapDetector = new OverlapDetector();
    }

    /// <summary>
    /// 获取游戏模型
    /// </summary>
    public GameModel GetGameModel()
    {
        return gameModel;
    }

    /// <summary>
    /// 获取牌区域模型
    /// </summary>
    public CardsAreaModel GetCardsAreaModel()
    {
        return cardsAreaModel;
    }

    /// <summary>
    /// 获取操作历史模型
    /// </summary>
    public OperationHistoryModel GetOperationHistoryModel()
    {
        return operationHistoryModel;
    }

    /// <summary>
    /// 获取UI管理器
    /// </summary>
    public UIManager GetUIManager()
    {
        return uiManager;
    }

    /// <summary>
    /// 获取卡牌控制器
    /// </summary>
    public CardController GetCardController()
    {
        return cardController;
    }

    /// <summary>
    /// 获取按钮控制器
    /// </summary>
    public ButtonController GetButtonController()
    {
        return buttonController;
    }

    /// <summary>
    /// 获取卡牌生成器
    /// </summary>
    public CardGenerator GetCardGenerator()
    {
        return cardGenerator;
    }

    /// <summary>
    /// 获取重叠检测器
    /// </summary>
    public OverlapDetector GetOverlapDetector()
    {
        return overlapDetector;
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    public void StartGame()
    {
        // 设置游戏状态为进行中
        gameModel.SetGameState(GameState.Playing);
        
        // 设置关卡ID为1
        gameModel.SetLevelID(1);
        
        // 重置分数
        gameModel.ResetScore();
        
        // 显示游戏界面
        uiManager.ShowGamePanel();
        
        // 生成初始手牌
        GenerateInitialCards();
    }

    /// <summary>
    /// 生成初始手牌
    /// </summary>
    private void GenerateInitialCards()
    {
        // 获取当前关卡应该生成的牌数量
        int cardCount = LevelConfig.GetInitialCardCount(gameModel.GetLevelID());
        
        // 生成牌并添加到手牌区
        List<CardModel> cards = cardGenerator.GenerateCards(cardCount);
        cardsAreaModel.AddCardsToHandArea(cards);

        // 更新UI显示
        uiManager.UpdateCardsDisplay();

        // 计算牌的遮盖关系
        overlapDetector.CalculateOverlap(uiManager.HandAreaContainer);
        
    }

    /// <summary>
    /// 检查游戏状态
    /// </summary>
    public void CheckGameState()
    {
        // 检查手牌区是否为空
        if (cardsAreaModel.IsHandAreaEmpty())
        {
            // 获取当前关卡ID
            int currentLevelID = gameModel.GetLevelID();
            
            // 如果是第一关结束
            if (currentLevelID == 1)
            {
                // 进入第二关
                EnterNextLevel();
            }
            // 如果是第二关结束
            else if (currentLevelID == 2)
            {
                // 结束游戏
                EndGame();
            }
        }
    }

    /// <summary>
    /// 进入下一关
    /// </summary>
    private void EnterNextLevel()
    {
        // 关卡ID增加
        gameModel.IncreaseLevelID();
        
        // 生成新的手牌
        GenerateInitialCards();
        
        // 更新UI显示当前关卡
        uiManager.UpdateLevelDisplay();
    }

    /// <summary>
    /// 结束游戏
    /// </summary>
    private void EndGame()
    {
        // 设置游戏状态为已结束
        gameModel.SetGameState(GameState.Ended);
        
        // 判断胜负
        bool isWin = gameModel.GetScore() >= LevelConfig.GetWinScore(2);
        gameModel.SetGameResult(isWin);
        
        // 显示结果界面
        uiManager.ShowResultPanel();
    }

    /// <summary>
    /// 处理出牌区牌数量达到5张的情况
    /// </summary>
    public void HandlePlayAreaFull()
    {
        // 获取出牌区的牌
        List<CardModel> playAreaCards = cardsAreaModel.GetPlayAreaCards();
        
        // 如果出牌区有5张牌
        if (playAreaCards.Count == 5)
        {
            // 计算牌型得分
            int score = PokerHandCalculator.CalculateScore(playAreaCards);
            
            // 更新累计分数
            gameModel.AddScore(score);
            
            // 清空出牌区
            cardsAreaModel.ClearPlayArea();
            
            // 更新UI显示
            uiManager.UpdateScoreDisplay();
            uiManager.UpdateCardsDisplay();
        }
    }
} 