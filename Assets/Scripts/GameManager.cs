using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject platformPrefab;
    public GameObject basePrefab;
    [SerializeField] public AudioSource bgm;
    [SerializeField] public AudioSource endfx;

    public float platformSpawnInterval = 1.5f;
    public float platformMoveSpeedY = 2f;
    public float platformMoveSpeedX = 0.5f;
    private float ogMoveSpeedY;
    private float ogMoveSpeedX;

    [HideInInspector] public int canMoveY = 0;
    [HideInInspector] public int canMoveX = 0;
    private int platformCount = 0;

    private Camera mainCamera;
    private int nextPlatformIndex = 0;
    [HideInInspector] public float minX, maxX, maxY, minY;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        canMoveY = 0;
        ogMoveSpeedY = platformMoveSpeedY;
        ogMoveSpeedX = platformMoveSpeedX;
        
        mainCamera = Camera.main;
        if (CameraScale.Instance != null) // Disable for android
        {
            CameraScale.Instance.Update();
        }
        CalculateCameraBounds();
        SpawnInitial();
        InvokeRepeating("SpawnPlatform", 0f, platformSpawnInterval);
    }

    private void SpawnInitial()
    {
        Instantiate(basePrefab, new Vector3(0, minY + 1.5f, 0f), Quaternion.identity);
        Instantiate(platformPrefab, new Vector3(Random.Range(minX, maxX), minY + 4f, 0f), Quaternion.identity);
        Instantiate(platformPrefab, new Vector3(Random.Range(minX, maxX), minY + 6.6f, 0f), Quaternion.identity);
        Instantiate(platformPrefab, new Vector3(Random.Range(minX, maxX), maxY, 0f), Quaternion.identity);
        platformCount += 3;
    }

    public void SpawnPlatform()
    {
        if(canMoveY == 1)
        {
            if(platformCount % 150 == 0){
                platformMoveSpeedX = ogMoveSpeedX + (platformCount/150);
                if(platformMoveSpeedX <= 2*ogMoveSpeedX){
                    platformMoveSpeedX = ogMoveSpeedX + (platformCount/100);
                }
            }
            if(platformCount % 100 == 0){
                Instantiate(basePrefab, new Vector3(0, maxY, 0f), Quaternion.identity);
                if(platformMoveSpeedY <= 2*ogMoveSpeedY){
                    platformMoveSpeedY = ogMoveSpeedY + (platformCount/100);
                }
            }
            else
            {
                GameObject specialPlatform = Instantiate(platformPrefab, new Vector3(Random.Range(minX, maxX), maxY + 1f, 0f), Quaternion.identity);
                float randomScale = Random.Range(1f, 2.5f);
                specialPlatform.transform.localScale = new Vector3(randomScale, specialPlatform.transform.localScale.y, specialPlatform.transform.localScale.z);

                float probability = (Mathf.FloorToInt(platformCount / 50)) / 10;
                canMoveX = (Random.Range(0f, 0.7f) <= probability)?1:0;
            }
            platformCount++;
        }
    }

    public int GetNextPlatformIndex()
    {
        return nextPlatformIndex++;
    }

    public void CalculateCameraBounds()
    {
        // Get the camera's orthographic size
        float cameraSize = mainCamera.orthographicSize;

        // Calculate camera bounds based on the size and aspect ratio
        float aspectRatio = mainCamera.aspect;
        minX = mainCamera.transform.position.x - cameraSize * aspectRatio + 0.5f;
        maxX = mainCamera.transform.position.x + cameraSize * aspectRatio - 0.5f;
        maxY = mainCamera.transform.position.y + cameraSize;
        minY = mainCamera.transform.position.y - cameraSize;
    }
}