using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TutoManager : MonoBehaviour
{
    [Header("타이머 및 대기 시간")]
    [SerializeField] private float tutorialStepDelay = 3f;
    [SerializeField] private float fadeSpeed = 0.5f;

    [Header("튜토리얼 타겟")]
    [SerializeField] private GameObject tutorialDummy;
    [SerializeField] private GameObject tutorialTarget; 

    [Header("UI 설정")]
    [SerializeField] private TMP_FontAsset tutorialFont;
    [SerializeField] private Color panelBackgroundColor = new Color(0, 0, 0, 0.7f);
    [SerializeField] private Color keyBackgroundColor = new Color(0.2f, 0.2f, 0.2f, 1f);
    [SerializeField] private Color textColor = Color.white;
    [SerializeField] private float tutorialFontSize = 36f;
    [SerializeField] private float keyFontSize = 24f;

    private GameObject tutorialPanel;
    private TextMeshProUGUI tutorialText;
    private Image tutorialBackgroundImage;

    private GameObject keyPromptPanel;
    private Image aKeyImage;
    private Image dKeyImage;
    private Image spaceKeyImage;
    private Image shiftKeyImage;
    private Image attackKeyImage;
    private Image qKeyImage;
    private Image eKeyImage;

    private enum TutorialState
    {
        Movement,
        Jump,
        MovementComplete,
        Dash,
        WeaponIntro,
        SwordTraining,
        GunTraining,
        Complete
    }

    private TutorialState currentState;
    private bool aPressedOnce;
    private bool dPressedOnce;
    private bool spacePressedOnce;
    private bool shiftPressedOnce;

    private PlayerM player;
    private GameObject spawnedDummy;
    private bool isTransitioning;
    private int swordHitCount = 0;
    private int gunHitCount = 0;
    private const int requiredSwordHits = 5;
    private const int requiredGunHits = 5;
    private bool isUsingGun = false;

    private readonly Vector2 referenceResolution = new Vector2(1920, 1080);

    private void Awake()
    {
        InitializeUISystem();
    }

    private void Start()
    {
        player = FindAnyObjectByType<PlayerM>();
        if (player == null)
            Debug.LogError("플레이어 컨트롤러를 찾을 수 없습니다!");

        tutorialPanel.SetActive(true);
        keyPromptPanel.SetActive(true);

        StartCoroutine(StartMovementTutorial());
    }

    private void InitializeUISystem()
    {
        GameObject canvasObj = new GameObject("TutorialCanvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = referenceResolution;
        scaler.matchWidthOrHeight = 0.5f;

        canvasObj.AddComponent<GraphicRaycaster>();

        CreateTutorialPanel(canvas.transform);
        CreateKeyPromptPanel(canvas.transform);
    }

    private void CreateTutorialPanel(Transform parent)
    {
        tutorialPanel = new GameObject("TutorialPanel");
        tutorialPanel.transform.SetParent(parent, false);

        RectTransform panelRect = tutorialPanel.AddComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0.8f);
        panelRect.anchorMax = new Vector2(0.5f, 0.8f);
        panelRect.pivot = new Vector2(0.5f, 0.5f);
        panelRect.sizeDelta = new Vector2(800, 150);
        panelRect.anchoredPosition = Vector2.zero;

        tutorialBackgroundImage = tutorialPanel.AddComponent<Image>();
        tutorialBackgroundImage.color = panelBackgroundColor;

        GameObject textObj = new GameObject("TutorialText");
        textObj.transform.SetParent(tutorialPanel.transform, false);

        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.pivot = new Vector2(0.5f, 0.5f);
        textRect.sizeDelta = new Vector2(-40, -20); 
        textRect.anchoredPosition = Vector2.zero;

        tutorialText = textObj.AddComponent<TextMeshProUGUI>();
        tutorialText.font = tutorialFont;
        tutorialText.fontSize = tutorialFontSize;
        tutorialText.color = textColor;
        tutorialText.alignment = TextAlignmentOptions.Center;
    }

    private void CreateKeyPromptPanel(Transform parent)
    {
        keyPromptPanel = new GameObject("KeyPromptPanel");
        keyPromptPanel.transform.SetParent(parent, false);

        RectTransform panelRect = keyPromptPanel.AddComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 0.2f);
        panelRect.anchorMax = new Vector2(0.5f, 0.2f);
        panelRect.pivot = new Vector2(0.5f, 0.5f);
        panelRect.sizeDelta = new Vector2(700, 100);
        panelRect.anchoredPosition = Vector2.zero;

        HorizontalLayoutGroup layout = keyPromptPanel.AddComponent<HorizontalLayoutGroup>();
        layout.childAlignment = TextAnchor.MiddleCenter;
        layout.spacing = 20;
        layout.childControlWidth = false;
        layout.childControlHeight = false;
        layout.childForceExpandWidth = false;
        layout.childForceExpandHeight = false;
        layout.padding = new RectOffset(10, 10, 10, 10);

        aKeyImage = CreateKeyImage(keyPromptPanel.transform, "AKeyImage", "A");
        dKeyImage = CreateKeyImage(keyPromptPanel.transform, "DKeyImage", "D");
        spaceKeyImage = CreateKeyImage(keyPromptPanel.transform, "SpaceKeyImage", "Space");
        shiftKeyImage = CreateKeyImage(keyPromptPanel.transform, "ShiftKeyImage", "Shift");
        attackKeyImage = CreateKeyImage(keyPromptPanel.transform, "AttackKeyImage", "LMB");
        qKeyImage = CreateKeyImage(keyPromptPanel.transform, "QKeyImage", "Q");
        eKeyImage = CreateKeyImage(keyPromptPanel.transform, "EKeyImage", "E");

        SetKeyPrompts();
    }

    private Image CreateKeyImage(Transform parent, string name, string keyText)
    {
        GameObject keyObj = new GameObject(name);
        keyObj.transform.SetParent(parent, false);

        RectTransform keyRect = keyObj.AddComponent<RectTransform>();
        keyRect.sizeDelta = new Vector2(80, 80);

        Image keyImage = keyObj.AddComponent<Image>();
        keyImage.color = keyBackgroundColor;

        GameObject textObj = new GameObject("Text");
        textObj.transform.SetParent(keyObj.transform, false);

        RectTransform textRect = textObj.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.pivot = new Vector2(0.5f, 0.5f);
        textRect.sizeDelta = Vector2.zero;
        textRect.anchoredPosition = Vector2.zero;

        TextMeshProUGUI keyTextTMP = textObj.AddComponent<TextMeshProUGUI>();
        keyTextTMP.font = tutorialFont;
        keyTextTMP.fontSize = keyFontSize;
        keyTextTMP.color = textColor;
        keyTextTMP.alignment = TextAlignmentOptions.Center;
        keyTextTMP.text = keyText;

        return keyImage;
    }

    private void Update()
    {
        if (isTransitioning) return;

        switch (currentState)
        {
            case TutorialState.Movement:
                CheckMovementInputs();
                break;
            case TutorialState.Jump:
                CheckJumpInput();
                break;
            case TutorialState.Dash:
                CheckDashInput();
                break;
            case TutorialState.SwordTraining:
                
                CheckWeaponSwitch();
                break;
            case TutorialState.GunTraining:
                
                CheckWeaponSwitch();
                break;
        }
    }

    private IEnumerator StartMovementTutorial()
    {
        currentState = TutorialState.Movement;
        yield return StartCoroutine(FadeInTutorialPanel());

        tutorialText.text = "A와 D 키를 눌러 좌우로 이동해 보세요.";
        SetKeyPrompts(a: true, d: true);
    }

    private void CheckMovementInputs()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            aPressedOnce = true;
            SetImageAlpha(aKeyImage, 0.5f);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            dPressedOnce = true;
            SetImageAlpha(dKeyImage, 0.5f);
        }

        if (aPressedOnce && dPressedOnce)
            StartTransition(TransitionToJumpTutorial);
    }

    private IEnumerator TransitionToJumpTutorial()
    {
        isTransitioning = true;
        tutorialText.text = "좋아요! 이제 점프를 배워봅시다.";
        yield return new WaitForSeconds(tutorialStepDelay);

        currentState = TutorialState.Jump;
        tutorialText.text = "스페이스 바를 눌러 점프해 보세요.";
        SetKeyPrompts(space: true);
        isTransitioning = false;
    }

    private void CheckJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            spacePressedOnce = true;
            SetImageAlpha(spaceKeyImage, 0.5f);
        }

        if (aPressedOnce && dPressedOnce && spacePressedOnce)
            StartTransition(TransitionToMovementComplete);
    }

    private IEnumerator TransitionToMovementComplete()
    {
        isTransitioning = true;
        currentState = TutorialState.MovementComplete;
        tutorialText.text = "훌륭해요! 기본 이동과 점프를 마스터했습니다.";
        yield return new WaitForSeconds(tutorialStepDelay);

        yield return TransitionToDashTutorial();
    }

    private IEnumerator TransitionToDashTutorial()
    {
        currentState = TutorialState.Dash;
        tutorialText.text = "이제 대시를 배워봅시다. Shift 키를 눌러 빠르게 대시해 보세요.";
        SetKeyPrompts(shift: true);
        isTransitioning = false;
        yield break;
    }

    private void CheckDashInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            shiftPressedOnce = true;
            SetImageAlpha(shiftKeyImage, 0.5f);
            StartTransition(TransitionToWeaponIntro);
        }
    }

    private IEnumerator TransitionToWeaponIntro()
    {
        isTransitioning = true;
        tutorialText.text = "잘했어요! 이제 무기 사용법을 배워볼까요?";
        yield return new WaitForSeconds(tutorialStepDelay);

        currentState = TutorialState.WeaponIntro;
        tutorialText.text = "현재 칼을 들고 있습니다. 좌클릭으로 허수아비 좀비를 5번 공격해보세요.";

        SpawnDummy();
        SetKeyPrompts(attack: true);

        currentState = TutorialState.SwordTraining;
        isTransitioning = false;
    }

    private void SpawnDummy()
    {
        if (tutorialDummy == null || tutorialTarget == null) return;

        
        spawnedDummy = Instantiate(tutorialDummy, tutorialTarget.transform.position, Quaternion.identity);

        
        var enemy = spawnedDummy.GetComponent<TutoEnemy>();
        if (enemy != null)
        {
            enemy.OnHit += OnDummyHit;
        }
    }

    private void OnDummyHit()
    {
        if (currentState == TutorialState.SwordTraining && !isUsingGun)
        {
            swordHitCount++;
            tutorialText.text = $"좌클릭으로 허수아비 좀비를 칼로 5번 공격해보세요. (현재: {swordHitCount}/5)";

            if (swordHitCount >= requiredSwordHits && !isTransitioning)
            {
                StartTransition(TransitionToGunTutorial);
            }
        }
        else if (currentState == TutorialState.GunTraining && isUsingGun)
        {
            gunHitCount++;
            tutorialText.text = $"좌클릭으로 허수아비 좀비를 총으로 5번 공격해보세요. (현재: {gunHitCount}/5)";

            if (gunHitCount >= requiredGunHits && !isTransitioning)
            {
                StartTransition(CompleteTutorial);
            }
        }
    }

    private void CheckWeaponSwitch()
    {
        
        if (Input.GetKeyDown(KeyCode.Q) && !isUsingGun)
        {
            isUsingGun = true;
            if (currentState == TutorialState.SwordTraining && swordHitCount >= requiredSwordHits)
            {
                tutorialText.text = "좋아요! 이제 Q키를 눌러 총으로 전환했습니다. 좌클릭으로 총 공격을 해보세요.";
            }
        }

        
        if (Input.GetKeyDown(KeyCode.E) && isUsingGun)
        {
            isUsingGun = false;
            if (currentState == TutorialState.GunTraining)
            {
                tutorialText.text = "E키를 눌러 다시 칼로 전환했습니다.";
            }
        }
    }

    private IEnumerator TransitionToGunTutorial()
    {
        isTransitioning = true;
        tutorialText.text = "좋아요! 칼 공격을 마스터했습니다.";
        yield return new WaitForSeconds(tutorialStepDelay);

        tutorialText.text = "이제 Q 키를 눌러 총으로 전환해보세요.";
        SetKeyPrompts(q: true);
        yield return new WaitForSeconds(tutorialStepDelay);

        currentState = TutorialState.GunTraining;
        tutorialText.text = $"좌클릭으로 허수아비 좀비를 총으로 5번 공격해보세요. (현재: {gunHitCount}/5)";
        SetKeyPrompts(attack: true, q: true, e: true);
        isTransitioning = false;
    }

    private IEnumerator CompleteTutorial()
    {
        isTransitioning = true;
        currentState = TutorialState.Complete;

        
        if (spawnedDummy != null)
        {
            var enemy = spawnedDummy.GetComponent<TutoEnemy>();
            if (enemy != null)
            {
                enemy.OnHit -= OnDummyHit;
            }
            Destroy(spawnedDummy);
        }

        tutorialText.text = "축하합니다! 튜토리얼을 모두 완료했습니다.";
        yield return new WaitForSeconds(tutorialStepDelay);

        tutorialText.text = "이제 실전으로 넘어가 볼까요?";
        yield return new WaitForSeconds(tutorialStepDelay);

        yield return StartCoroutine(FadeOutTutorialPanel());
        SceneManager.LoadScene(4, LoadSceneMode.Single);
    }

    private IEnumerator FadeInTutorialPanel()
    {
        tutorialPanel.SetActive(true);
        float alpha = 0f;
        var color = tutorialBackgroundImage.color;
        while (alpha < 1f)
        {
            alpha += fadeSpeed * Time.deltaTime;
            color.a = Mathf.Clamp01(alpha);
            tutorialBackgroundImage.color = color;
            yield return null;
        }
    }

    private IEnumerator FadeOutTutorialPanel()
    {
        float alpha = 1f;
        var color = tutorialBackgroundImage.color;
        while (alpha > 0f)
        {
            alpha -= fadeSpeed * Time.deltaTime;
            color.a = Mathf.Clamp01(alpha);
            tutorialBackgroundImage.color = color;
            yield return null;
        }
        tutorialPanel.SetActive(false);
    }

    private void SetKeyPrompts(bool a = false, bool d = false, bool space = false, bool shift = false, bool attack = false, bool q = false, bool e = false)
    {
        aKeyImage.gameObject.SetActive(a);
        dKeyImage.gameObject.SetActive(d);
        spaceKeyImage.gameObject.SetActive(space);
        shiftKeyImage.gameObject.SetActive(shift);
        attackKeyImage.gameObject.SetActive(attack);
        qKeyImage.gameObject.SetActive(q);
        eKeyImage.gameObject.SetActive(e);

        if (a) SetImageAlpha(aKeyImage, 1f);
        if (d) SetImageAlpha(dKeyImage, 1f);
        if (space) SetImageAlpha(spaceKeyImage, 1f);
        if (shift) SetImageAlpha(shiftKeyImage, 1f);
        if (attack) SetImageAlpha(attackKeyImage, 1f);
        if (q) SetImageAlpha(qKeyImage, 1f);
        if (e) SetImageAlpha(eKeyImage, 1f);
    }

    private void SetImageAlpha(Image img, float alpha)
    {
        var c = img.color;
        c.a = alpha;
        img.color = c;
    }

    private void StartTransition(System.Func<IEnumerator> transitionMethod)
    {
        if (!isTransitioning)
        {
            isTransitioning = true;
            StartCoroutine(transitionMethod());
        }
    }

    private void OnDestroy()
    {
        
        if (spawnedDummy != null)
        {
            var enemy = spawnedDummy.GetComponent<TutoEnemy>();
            if (enemy != null)
            {
                enemy.OnHit -= OnDummyHit;
            }
        }
    }
}