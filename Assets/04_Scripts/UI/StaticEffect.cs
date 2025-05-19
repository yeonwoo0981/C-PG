using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StaticEffect : MonoBehaviour
{
    [Header("Static Settings")]
    [SerializeField] private Texture2D[] staticTextures;
    [SerializeField] private float changeInterval = 0.1f;
    [SerializeField] private Image staticImage;

    private float timer = 0f;
    private int currentTextureIndex = 0;
    private bool isActive = false;

    private void Start()
    {
        if (staticImage == null)
            staticImage = GetComponent<Image>();

        // 초기에는 비활성화
        gameObject.SetActive(false);

        if (staticTextures.Length == 0)
        {
            GenerateStaticTextures();
        }
    }

    private void Update()
    {
        if (!isActive) return;

        timer += Time.unscaledDeltaTime;

        if (timer >= changeInterval)
        {
            timer = 0f;
            ChangeStaticTexture();
        }
    }

    public void StartStaticEffect()
    {
        isActive = true;
        gameObject.SetActive(true);
    }

    public void StopStaticEffect()
    {
        isActive = false;
        gameObject.SetActive(false);
    }

    private void ChangeStaticTexture()
    {
        if (staticTextures.Length == 0) return;

        currentTextureIndex = Random.Range(0, staticTextures.Length);

        if (staticImage != null && staticTextures[currentTextureIndex] != null)
        {
            Sprite staticSprite = Sprite.Create(
                staticTextures[currentTextureIndex],
                new Rect(0, 0, staticTextures[currentTextureIndex].width, staticTextures[currentTextureIndex].height),
                new Vector2(0.5f, 0.5f)
            );
            staticImage.sprite = staticSprite;
        }
    }

    private void GenerateStaticTextures()
    {
        staticTextures = new Texture2D[5];

        for (int i = 0; i < staticTextures.Length; i++)
        {
            staticTextures[i] = GenerateRandomStaticTexture(256, 256);
        }
    }

    private Texture2D GenerateRandomStaticTexture(int width, int height)
    {
        Texture2D texture = new Texture2D(width, height);
        Color[] pixels = new Color[width * height];

        for (int i = 0; i < pixels.Length; i++)
        {
            float gray = Random.Range(0f, 1f);
            pixels[i] = new Color(gray, gray, gray, Random.Range(0.3f, 0.8f));
        }

        texture.SetPixels(pixels);
        texture.Apply();

        return texture;
    }
}
