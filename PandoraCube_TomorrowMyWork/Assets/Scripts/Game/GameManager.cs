using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private int StageNum;

    public float StageTime;

    [SerializeField] private GameObject Menu;
    [SerializeField] private Text MenuText;
    [SerializeField] private Button button;
    [SerializeField] private Text buttonText;
    public GameObject Help;
    private bool isMenuShown = false;
    public bool isHelpShown = false;
    private bool isFinished = false;
    public bool isEnableMoving = true;

    // Start is called before the first frame update

    private void Awake()
    {
        if (Instance != this) Destroy(gameObject);
    }

    void Start()
    {
        SoundManager.Instance.playMapBGM(StageNum);
        MenuText = Menu.GetComponentInChildren<Text>();
        buttonText = button.GetComponentInChildren<Text>();

        UpdateMemo(StageNum);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isMenuShown)
                showMenu(0);
            else
                ShutMenu();
        }
        if (Input.GetMouseButtonDown(0) && isHelpShown)
        {
            if (isMenuShown && isHelpShown)
            {
                Help.SetActive(false);
                isHelpShown = false;
            }
        }

    }

    public void GameOver()
    {
        showMenu(1);
        SoundManager.Instance.playGameSFX(3);
        SoundManager.Instance.StopMapBGM();
        isFinished = true;
    }

    public void CallGameSuccess()
    {
        StartCoroutine(GameSuccess());
    }

    IEnumerator GameSuccess()
    {
        yield return new WaitForSeconds(2f);

        // 게임에 효과줘도 좋을듯함
        SoundManager.Instance.playGameSFX(2);
        SoundManager.Instance.StopMapBGM();
        buttonText.text = "Next";
        button.onClick.AddListener(goNext);
        showMenu(2);
        isFinished = true;
    }

    void goNext()
    {
        if (StageNum == 0)
        {
            SceneManager.LoadScene(3);
        }
        else
        {
            SceneManager.LoadScene(5);
        }
    }


    public void showMenu(int type)
    {
        if (!isMenuShown)
        {
            Menu.SetActive(true);
            switch (type)
            {
                case 0:
                    MenuText.text = "Game Paused";
                    break;

                case 1:
                    MenuText.text = "Game Over!";
                    break;

                case 2:
                    MenuText.text = "Game Clear!";
                    break;

                default:
                    break;
            }
            PauseGame();
            isMenuShown = true;

        }
    }

    public void ShutMenu()
    {
        if (isMenuShown && !isHelpShown)
        {
            if (!isFinished)
            {
                Menu.SetActive(false);
                ResumeGame();
                isMenuShown = false;
            }
        }
        else if (isMenuShown && isHelpShown)
        {
            Help.SetActive(false);
            isHelpShown = false;
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        isEnableMoving = false;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        isEnableMoving = true;
    }

    [SerializeField] private TMPro.TMP_Text textPrefab;
    [SerializeField] private RectTransform memoPanel;
    [SerializeField] private int taskCount;

    [SerializeField]
    private List<TMPro.TMP_Text> texts = null;

    public bool GetTaskComplete => texts != null && texts.FindAll(p => p.fontStyle == TMPro.FontStyles.Strikethrough).Count == taskCount;

    private void UpdateMemo(int index)
    {
        texts = new List<TMPro.TMP_Text>();
        var csv = CSVReader.Read("CSV/Stage" + (index + 1));

        for (int i = 0; i < csv.Count; i++)
        {
            string content = csv[i]["Line"].ToString();

            texts.Add(Instantiate(textPrefab,memoPanel.transform));
            texts[texts.Count - 1].text = content.Replace('@', '\n');
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(memoPanel);
    }

    public void TaskComplete(int index, bool flag)
    {
        if (index < 0 || index >= texts.Count) return;
        texts[index].fontStyle = flag ? TMPro.FontStyles.Strikethrough : TMPro.FontStyles.Normal;
        texts[index].color = flag ? Color.black * 0.5f : Color.black;
    }

}
