using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 卡牌视图，负责单张牌的视觉表现
/// </summary>
public class CardView : MonoBehaviour, IPointerClickHandler
{
    // 卡牌背景图片
    [SerializeField] private Image cardBackground;
    
    // 花色图片
    [SerializeField] private Image suitImage;
    
    // 点数文本
    [SerializeField] private Text rankText;
    
    // 卡牌正面和背面
    [SerializeField] private GameObject cardFront;
    [SerializeField] private GameObject cardBack;
    
    // 卡牌高亮效果
    [SerializeField] private GameObject highlightEffect;
    
    // 卡牌模型引用
    private CardModel cardModel;
    
    // 卡牌的目标位置和旋转
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    
    // 移动动画的持续时间
    private float moveDuration = 0.3f;
    
    // 是否正在移动
    private bool isMoving = false;

    /// <summary>
    /// 设置卡牌模型
    /// </summary>
    public void SetCardModel(CardModel model)
    {
        cardModel = model;
        
        // 更新卡牌外观
        UpdateCardAppearance();
        
        // 更新可点击状态
        UpdateClickableState();
    }

    /// <summary>
    /// 更新卡牌外观
    /// </summary>
    private void UpdateCardAppearance()
    {
        if (cardModel == null) return;
        
        // 设置花色图片
        SetSuitImage(cardModel.GetSuit());
        
        // 设置点数文本
        SetRankText(cardModel.GetRank());
        
        // 设置卡牌颜色（红色或黑色）
        SetCardColor(cardModel.GetSuit());
    }

    /// <summary>
    /// 设置花色图片
    /// </summary>
    private void SetSuitImage(CardSuit suit)
    {
        // 根据花色设置对应的Sprite
        string suitSpriteName = suit.ToString().ToLower();
        Sprite suitSprite = Resources.Load<Sprite>("CardIcons/" + suitSpriteName);
        
        if (suitSprite != null)
        {
            suitImage.sprite = suitSprite;
        }
    }

    /// <summary>
    /// 设置点数文本
    /// </summary>
    private void SetRankText(CardRank rank)
    {
        // 根据点数设置文本
        switch (rank)
        {
            case CardRank.Jack:
                rankText.text = "J";
                break;
            case CardRank.Queen:
                rankText.text = "Q";
                break;
            case CardRank.King:
                rankText.text = "K";
                break;
            case CardRank.Ace:
                rankText.text = "A";
                break;
            default:
                rankText.text = ((int)rank).ToString();
                break;
        }
    }

    /// <summary>
    /// 设置卡牌颜色
    /// </summary>
    private void SetCardColor(CardSuit suit)
    {
        // 红心和方块为红色，黑桃和梅花为黑色
        Color textColor = (suit == CardSuit.Heart || suit == CardSuit.Diamond) ? Color.red : Color.black;
        rankText.color = textColor;
    }

    /// <summary>
    /// 更新可点击状态
    /// </summary>
    public void UpdateClickableState()
    {
        if (cardModel == null) return;
        
        // 根据卡牌模型的可点击状态显示或隐藏高亮效果
        bool isClickable = cardModel.IsClickable();
        
        if (highlightEffect != null)
        {
            highlightEffect.SetActive(isClickable);
        }
    }

    /// <summary>
    /// 处理点击事件
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (cardModel == null) return;
        
        // 如果卡牌可点击，通知卡牌控制器处理点击事件
        if (cardModel.IsClickable())
        {
            CardController.Instance.OnCardClicked(cardModel);
        }
    }

    /// <summary>
    /// 移动卡牌到指定位置
    /// </summary>
    public void MoveToPosition(Vector3 position, Quaternion rotation)
    {
        targetPosition = position;
        targetRotation = rotation;
        
        // 启动移动协程
        if (!isMoving)
        {
            StartCoroutine(MoveCardCoroutine());
        }
    }

    /// <summary>
    /// 卡牌移动协程
    /// </summary>
    private IEnumerator MoveCardCoroutine()
    {
        isMoving = true;
        
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        
        float elapsedTime = 0f;
        
        while (elapsedTime < moveDuration)
        {
            // 计算插值系数
            float t = elapsedTime / moveDuration;
            
            // 使用平滑曲线
            t = Mathf.SmoothStep(0, 1, t);
            
            // 更新位置和旋转
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            
            // 更新时间
            elapsedTime += Time.deltaTime;
            
            yield return null;
        }
        
        // 确保最终位置和旋转准确
        transform.position = targetPosition;
        transform.rotation = targetRotation;
        
        isMoving = false;
    }

    /// <summary>
    /// 翻转卡牌
    /// </summary>
    public void FlipCard(bool showFront)
    {
        cardFront.SetActive(showFront);
        cardBack.SetActive(!showFront);
    }
} 