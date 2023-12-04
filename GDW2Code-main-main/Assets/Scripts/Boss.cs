using Unity.Mathematics;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Other Objects")]
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject objectPrefab;
    [SerializeField] private win wg;
    [SerializeField] private Player PCode;
    [SerializeField] private PlayerCollision PC;
    [Header("Bullet Options")]
    [SerializeField] private float launchDelay = 2f;
    [SerializeField] private float launchSpeed = 10f;
    [SerializeField] private float maxPredictionTime = 0.75f;
    [SerializeField] private float timeLimit = 40f;
    [Header("For Other Scripts")]
    public bool inBossFight;
    public bool inFlight;
    [Header("Boss Settings")]
    [SerializeField] private float Health = 250;

    private float timer;
    private float timer1;
    private float count;
    private int maxBurst = 5;

    private void Update()
    {
        if (inBossFight)
        {
            timer += Time.deltaTime;
            timer1 += Time.deltaTime;

            if (timer >= timeLimit)
            {
                PC.GameOver();
            }
            else if (timer >= timeLimit / 2)
            {
                maxBurst = 10;
                launchDelay = 0.25f;
            }
            if (timer1 >= launchDelay)
            {
                count++;
                if (count > 0 && count <= maxBurst)
                {
                    LaunchObjectDelayed();
                }
                else if (count > maxBurst)
                {
                    count = -1;
                }
                timer1 = 0;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("bullets"))
        {
            Destroy(collision.gameObject);
            HealthCheck(5);
        }
    }
    private void LaunchObjectDelayed()
    {
        Vector2 targetVelocity;
        if (!inFlight)
        {
            targetVelocity = new Vector2(Player.GetComponent<Rigidbody2D>().velocity.x, math.clamp(-Player.GetComponent<Rigidbody2D>().velocity.y, -5, 1));
        }
        else
        {
            targetVelocity = new Vector2(Player.GetComponent<Rigidbody2D>().velocity.x, math.clamp(Player.GetComponent<Rigidbody2D>().velocity.y, -5, 1));
        }

        Vector3 predictedPosition = PredictTargetPosition(Player.transform.position, targetVelocity, objectPrefab.transform.position, launchSpeed);
        LaunchObject(predictedPosition);
    }
    private Vector2 PredictTargetPosition(Vector2 targetPosition, Vector2 targetVelocity, Vector2 launchPosition, float launchSpeed)
    {
        Vector2 targetOffset = targetPosition - launchPosition;
        float maxTime = targetOffset.magnitude / launchSpeed;

        float time = Mathf.Min(maxTime, maxPredictionTime);

        Vector2 predictedPosition = targetPosition + targetVelocity * time;

        return predictedPosition;
    }
    private void LaunchObject(Vector3 targetPosition)
    {
        Vector2 launchDirection = (targetPosition - transform.position).normalized;
        Vector3 pos = new Vector3(transform.position.x - 3.5f, transform.position.y);
        GameObject launchedObject = Instantiate(objectPrefab, pos, Quaternion.identity);
        launchedObject.GetComponent<Rigidbody2D>().AddForce(launchDirection * launchSpeed, ForceMode2D.Impulse);

        Destroy(launchedObject, 3);
    }
    private void HealthCheck(int dmg)
    {
        Health -= dmg;
        if (Health <= 0)
        {
            Destroy(gameObject);
            wg.GameW();
        }
    }
}
