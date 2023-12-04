using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float SpeedFactor;
    [SerializeField] private float JumpForce;
    [SerializeField] private LayerMask GroundLayers;
    [Header("Other Objects")]
    [SerializeField] private Boss Boss;
    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private PlayerCollision PC;
    [Header("Player Settings")]
    [SerializeField] private int Health;

    private Rigidbody2D Rb;
    private float Depth;
    private bool IsGrounded;
    private Vector3 NewPos;
    private bool inFight;
    private float timer;

    void Start()
    {
        ImputManager.Init(this);
        ImputManager.GameMode();

        Rb = GetComponent<Rigidbody2D>();
        Depth = GetComponent<Collider2D>().bounds.size.y;

        NewPos = Vector3.right / SpeedFactor;
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (!inFight)
        {
            transform.position += NewPos;
        }
        else if (inFight)
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (timer >= 0.75)
                {
                    Vector3 pos = new Vector3(transform.position.x + 1, transform.position.y);
                    GameObject launchedObject = Instantiate(BulletPrefab, pos, Quaternion.identity);
                    launchedObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * 10, ForceMode2D.Impulse);
                    timer = 0;
                }
            }
        }
        CheckGrounded();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Side Block"))
        {
            PC.GameOver();
        }
        else if (collision.transform.CompareTag("bullets"))
        {
            Destroy(collision.gameObject);
            HealthCheck(10);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Enter Fight"))
        {
            JumpForce = 6;
            inFight = !inFight;
            Boss.inBossFight = !Boss.inBossFight;
            ImputManager.inFight = !ImputManager.inFight;
        }
    }
    private void CheckGrounded()
    {
        IsGrounded = Physics2D.Raycast(transform.position, Vector3.down, Depth, GroundLayers);
    }
    public void Jump()
    {
        if (!inFight)
        {
            if (IsGrounded)
            {
                Rb.AddForce(Vector3.up * JumpForce, ForceMode2D.Impulse);
            }
        }
    }
    public void Fly()
    {
        if (inFight)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Rb.gravityScale = 0;
                Rb.AddForce(Vector2.up * JumpForce, ForceMode2D.Impulse);
                Boss.inFlight = true;
            }
            else
            {
                Rb.gravityScale = 1;
                Boss.inFlight = false;
            }
        }
    }
    public void DetachChild()
    {
        if (transform.childCount > 0)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Transform child = transform.GetChild(i);
                child.SetParent(null);
            }
        }
    }
    private void HealthCheck(int dmg)
    {
        Health -= dmg;
        if (Health <= 0)
        {
            PC.GameOver();
        }
    }
}
