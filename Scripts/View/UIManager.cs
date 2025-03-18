using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI管理器，负责管理所有UI元素和界面切换
/// </summary>
public class UIManager : MonoBehaviour
{
    // 开始界面
    [SerializeField] private GameObject startPanel;
    
    // 游戏界面
    [SerializeField] private GameObject gamePanel;
    
    // 结果界面
    [SerializeField] private GameObject resultPanel;
    
    // 游戏界面上显示关卡ID的文本
    [SerializeField] private Text levelText;
    
    // 游戏界面上显示分数的文本
    [SerializeField] private Text scoreText;
    
    // 游戏界面上显示手牌区牌数量的文本
    [SerializeField] private Text handCardCountText;

    // 手牌区坐标容器
    [SerializeField] private Transform posAreaContainer;

    // 手牌区容器
    [SerializeField] private Transform handAreaContainer;

    // 手牌
    
    // 出牌区容器
    [SerializeField] private Transform playAreaContainer;
    
    // 等待区容器
    [SerializeField] private Transform waitAreaContainer;
    
    // 结果界面上的结果文本
    [SerializeField] private Text resultText;
    
    // 卡片预制体
    [SerializeField] private GameObject cardPrefab;
    
    // 四个功能按钮
    [SerializeField] private Button moveToWaitButton;
    [SerializeField] private Button addCardsButton;
    [SerializeField] private Button undoButton;
    [SerializeField] private Button shuffleButton;
    
    // 开始游戏按钮
    [SerializeField] private Button startButton;
    
    // 重新开始按钮
    [SerializeField] private Button restartButton;
    
    // 模型引用
    private GameModel gameModel;
    private CardsAreaModel cardsAreaModel;
    
    // 所有卡牌视图字典，用于快速查找卡牌对应的视图对象
    private Dictionary<int, CardView> cardViewDict = new Dictionary<int, CardView>();

    // 手牌区卡牌视图字典
    private Dictionary<int, CardView> handCardViewDict = new Dictionary<int, CardView>();



    /// <summary>
    /// 初始化UI管理器
    /// </summary>
    public void Initialize(GameModel gameModel, CardsAreaModel cardsAreaModel)
    {
        this.gameModel = gameModel;
        this.cardsAreaModel = cardsAreaModel;
        
        // 初始化按钮点击事件
        InitializeButtons();
        
        // 默认显示开始界面
        ShowStartPanel();
    }

    /// <summary>
    /// 初始化按钮点击事件
    /// </summary>
    private void InitializeButtons()
    {
        // 开始游戏按钮
        startButton.onClick.AddListener(() => {
            GameController.Instance.StartGame();
        });
        
        // 移到等待区按钮
        moveToWaitButton.onClick.AddListener(() => {
            ButtonController.Instance.OnMoveToWaitButtonClicked();
        });
        
        // 增加手牌按钮
        addCardsButton.onClick.AddListener(() => {
            ButtonController.Instance.OnAddCardsButtonClicked();
        });
        
        // 反悔按钮
        undoButton.onClick.AddListener(() => {
            ButtonController.Instance.OnUndoButtonClicked();
        });
        
        // 洗牌按钮
        shuffleButton.onClick.AddListener(() => {
            ButtonController.Instance.OnShuffleButtonClicked();
        });
        
        // 重新开始按钮
        restartButton.onClick.AddListener(() => {
            GameController.Instance.StartGame();
        });
    }

    /// <summary>
    /// 显示开始界面
    /// </summary>
    public void ShowStartPanel()
    {
        startPanel.SetActive(true);
        gamePanel.SetActive(false);
        resultPanel.SetActive(false);
    }

    /// <summary>
    /// 显示游戏界面
    /// </summary>
    public void ShowGamePanel()
    {
        startPanel.SetActive(false);
        gamePanel.SetActive(true);
        resultPanel.SetActive(false);
        
        // 更新界面显示
        UpdateLevelDisplay();
        UpdateScoreDisplay();
        UpdateHandCardCountDisplay();
    }

    /// <summary>
    /// 显示结果界面
    /// </summary>
    public void ShowResultPanel()
    {
        startPanel.SetActive(false);
        gamePanel.SetActive(false);
        resultPanel.SetActive(true);
        
        // 更新结果显示
        bool isWin = gameModel.GetGameResult();
        resultText.text = isWin ? "游戏胜利！" : "游戏失败！";
        resultText.color = isWin ? Color.green : Color.red;
    }

    /// <summary>
    /// 更新关卡显示
    /// </summary>
    public void UpdateLevelDisplay()
    {
        levelText.text = "关卡: " + gameModel.GetLevelID();
    }

    /// <summary>
    /// 更新分数显示
    /// </summary>
    public void UpdateScoreDisplay()
    {
        scoreText.text = "分数: " + gameModel.GetScore();
    }

    /// <summary>
    /// 更新手牌区牌数量显示
    /// </summary>
    public void UpdateHandCardCountDisplay()
    {
        handCardCountText.text = "手牌: " + cardsAreaModel.GetHandAreaCardCount();
    }

    /// <summary>
    /// 创建卡牌视图对象
    /// </summary>
    public CardView CreateCardView(CardModel cardModel)
    {
        // 实例化卡牌预制体
        GameObject cardObject = Instantiate(cardPrefab);
        
        // 获取CardView组件
        CardView cardView = cardObject.GetComponent<CardView>();
        
        // 设置卡牌数据
        cardView.SetCardModel(cardModel);
        
        // 根据卡牌所在区域设置父物体
        //Transform parent = GetParentTransformByArea(cardModel.GetArea());
        //cardObject.transform.SetParent(parent, false);
        
        // 添加到字典
        cardViewDict[cardModel.GetID()] = cardView;
        
        return cardView;
    }

    private void SetHandCardsPos(Dictionary<int,CardView> cardViews)
    {
        List<Transform> cardPositions = new List<Transform>();
        for (int i = 0; i < posAreaContainer.childCount; i++)
        {
            cardPositions.Add(posAreaContainer.GetChild(i));
        }

        int j = 0;
        foreach (var cardView in cardViews.Values)
        {
            cardView.transform.SetParent(handAreaContainer, false);
            cardView.transform.position = cardPositions[j].position;
            j++;
            if (j >= cardPositions.Count - 1)
            {
                j = 0;
            }
        }
    }

    /// <summary>
    /// 根据卡牌区域获取父物体Transform
    /// </summary>
    private Transform GetParentTransformByArea(CardArea area)
    {
        switch (area)
        {
            case CardArea.Hand:
                return handAreaContainer;
            case CardArea.Play:
                return playAreaContainer;
            case CardArea.Wait:
                return waitAreaContainer;
            default:
                return null;
        }
    }

    /// <summary>
    /// 更新所有卡牌的显示
    /// </summary>
    public void UpdateCardsDisplay()
    {
        // 清空现有的卡牌视图
        ClearAllCardViews();
        
        // 重新创建手牌区卡牌视图
        foreach (var card in cardsAreaModel.GetHandAreaCards())
        {
            handCardViewDict[card.GetID()] = CreateCardView(card);
        }
        SetHandCardsPos(handCardViewDict);


        // 重新创建出牌区卡牌视图
        foreach (var card in cardsAreaModel.GetPlayAreaCards())
        {
            CreateCardView(card);
        }
        
        // 重新创建等待区卡牌视图
        foreach (var card in cardsAreaModel.GetWaitAreaCards())
        {
            CreateCardView(card);
        }
        
        // 更新手牌数量显示
        UpdateHandCardCountDisplay();
    }

    /// <summary>
    /// 清空所有卡牌视图
    /// </summary>
    private void ClearAllCardViews()
    {
        // 清空手牌区
        foreach (Transform child in handAreaContainer)
        {
            Destroy(child.gameObject);
        }
        
        // 清空出牌区
        foreach (Transform child in playAreaContainer)
        {
            Destroy(child.gameObject);
        }
        
        // 清空等待区
        foreach (Transform child in waitAreaContainer)
        {
            Destroy(child.gameObject);
        }
        
        // 清空字典
        cardViewDict.Clear();
    }

    /// <summary>
    /// 获取卡牌视图
    /// </summary>
    public CardView GetCardView(int cardID)
    {
        if (cardViewDict.ContainsKey(cardID))
        {
            return cardViewDict[cardID];
        }
        return null;
    }

    /// <summary>
    /// 更新按钮的可交互状态
    /// </summary>
    public void UpdateButtonStates()
    {
        // 判断是否有可移动到等待区的牌
        bool hasClickableHandCards = cardsAreaModel.GetClickableHandAreaCards().Count > 0;
        moveToWaitButton.interactable = hasClickableHandCards;
        
        // 判断是否可以反悔
        bool canUndo = ButtonController.Instance.CanUndo();
        undoButton.interactable = canUndo;
        
        // 判断是否有手牌可以洗牌
        bool canShuffle = cardsAreaModel.GetHandAreaCardCount() > 1;
        shuffleButton.interactable = canShuffle;
    }
} 