using UnityEngine;

public class Platform : MonoBehaviour
{
    public int platformIndex { get; private set; } // Index or ID of the platform
    private Rigidbody2D platformRb;

    private void Start()
    {
        platformRb = GetComponent<Rigidbody2D>();
        float dirX = (Random.Range(0f, 1f) > 0.5f)?1:-1;
        platformRb.velocity = new Vector2(GameManager.Instance.platformMoveSpeedX * GameManager.Instance.canMoveX * dirX, -GameManager.Instance.platformMoveSpeedY * GameManager.Instance.canMoveY);
        platformIndex = GameManager.Instance.GetNextPlatformIndex();
    }

    private void Update()
    {
        float newVelocityX = platformRb.velocity.x;
        if (transform.position.x >= GameManager.Instance.maxX || transform.position.x <= GameManager.Instance.minX)
        {
            newVelocityX = -platformRb.velocity.x;
        }
        platformRb.velocity = new Vector2(newVelocityX, -GameManager.Instance.platformMoveSpeedY * GameManager.Instance.canMoveY);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}