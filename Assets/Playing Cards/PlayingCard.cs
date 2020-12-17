using UnityEngine;

public class PlayingCard : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Sprite cardBack = null;

    [Header("Card Properties")]
    [SerializeField] private bool isAnAce = false;
    [SerializeField] private int value = 1;
    public bool IsAnAce { get => isAnAce; }
    public int Value { get => value; }

    public bool IsHidden { get; private set; }
    private SpriteRenderer spriteRenderer = null;
    private Animator animator = null;
    private Sprite originalSprite = null;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        originalSprite = spriteRenderer.sprite;

        // TODO: Play card sound
    }
    public void SetCardHidden(bool value, bool shouldAnimate)
    {
        IsHidden = value;
        if (value)
            spriteRenderer.sprite = cardBack;
        else
            spriteRenderer.sprite = originalSprite;

        if (shouldAnimate)
        {
            animator.SetTrigger("Flip");
            // TODO: Play card sound
        }
    }
}
