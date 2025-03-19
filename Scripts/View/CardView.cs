using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening; // 需要引入DOTween命名空间

/// <summary>
/// 卡牌视图，负责单张牌的视觉表现
/// </summary>
public class CardView : MonoBehaviour, IPointerClickHandler
{
    // 花色图片
    [SerializeField] private Image suitImage;
    
    // 点数文本
    [SerializeField] private Text rankText;
    
    // 卡牌无法点击效果
    [SerializeField] private GameObject coverEffect;
    
    // 卡牌模型引用
    private CardModel cardModel;
    
    // 卡牌的目标位置和旋转
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    
    // 移动动画的持续时间
    [SerializeField] private float moveDuration = 0.5f;
    [SerializeField] private Ease moveEase = Ease.InOutQuad; // DOTween内置的缓动函数
    
    // 是否正在移动
    private Sequence moveSequence;

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
    /// 获取卡牌模型
    /// </summary>
    public CardModel GetCardModel()
    {
        return cardModel;
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
    /// 设置花色和点图片
    /// </summary>
    private void SetSuitImage(CardSuit suit)
    {
        // 根据花色设置对应的Sprite
        string suitSpriteName = suit.ToString().ToLower();
        Sprite suitSprite = Resources.Load<Sprite>("Images/" + suitSpriteName);
        
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
        
        if (coverEffect != null)
        {
            coverEffect.SetActive(!isClickable);
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
    public void MoveToPosition(Vector3 position, System.Action onComplete = null)
    {
        targetPosition = position;
        
        // 如果有正在进行的动画，先停止
        if (moveSequence != null && moveSequence.IsActive())
        {
            moveSequence.Kill();
        }
        
        // 创建新的动画序列
        moveSequence = DOTween.Sequence();
        
        // 添加位置动画
        moveSequence.Join(transform.DOMove(targetPosition, moveDuration).SetEase(moveEase));
        
        // 添加完成回调(可选)
        moveSequence.OnComplete(() => {
            // 确保最终位置和旋转准确
            transform.position = targetPosition;
            transform.rotation = targetRotation;
            OnDestroyMove();

            // 调用外部传入的回调函数
            onComplete?.Invoke();
        });
    }

    private void OnDestroyMove()
    {
        // 确保在销毁对象时停止所有动画
        if (moveSequence != null && moveSequence.IsActive())
        {
            moveSequence.Kill();
        }
    }
} 