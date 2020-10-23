using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using System;
using System.Threading;

public class bl_GameManager : Singleton<bl_GameManager> {

    public GameObject storePanel;
    public GameObject rewardIndicator;
    public GameObject noCoinsIndicator;
    public bl_ScoreManager score;
    public int TotalPots = 0;
    public string CurrentWord;
    public Timer time;
    [Header("Settings")]
    public DifficultMode m_DifficultMode = DifficultMode.EASY;
    public Mode m_Mode = Mode.SinglePlayer;
    [Range(2, 8)]
    public int MaxTrys = 6;
    [Range(0,6)]
    public int Trys = 6;
    [Header("Effects")]
    [SerializeField]public AudioSource FSXSource;
    [SerializeField] public AudioClip HitAudio;
    [SerializeField] public AudioClip SuccessAudio;
    [SerializeField] public AudioClip FailHitAudio;
    [SerializeField] public AudioClip FailAudio;
    [Header("References")]
	[SerializeField] public Image[] HangManParts;
    [SerializeField] public GameObject CharPrefab;
    [SerializeField] public GameObject SpacePrefab;
    [SerializeField] public GameObject CustomWordCreatorPanel;
    [SerializeField] public Transform WordPanel;
    [SerializeField] public Text TryText;
    [SerializeField] public Animator FlashSentenceAnim;
    [SerializeField] public Animator LoadAnimator;
    [SerializeField] public Rigidbody2D[] Rigid2D;
    [SerializeField] public Text WordCreatorLogText;
    [SerializeField] public InputField WordInput;
    [SerializeField] public InputField Clue1Input;
    [SerializeField] public InputField Clue2Input;
    [SerializeField] public Text Clue1Text;
    [SerializeField] public Text Clue2Text;
    [SerializeField] public Text WordCreatorInfoText;

    public List<bl_Char> AllChars = new List<bl_Char>();
    public List<bl_Char> cacheChars = new List<bl_Char>();
    public List<GameObject> cacheSpace = new List<GameObject>();
    public int CurrentPart = 0;
    public Vector2[] HangmanPartPositions;
    private string[] cheatCode;
    private int index;
    private bool cheat;

    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
        m_DifficultMode = bl_GameInfo.Instance.Category;
        m_Mode = bl_GameInfo.Instance.GameMode;
    }
    void OnGUI()
    {
        if (cheat)
        {
            // draw the hidden menu
            GUI.Box(new Rect(20, 80, 240, 240), "Cheat Menu :-) ");
            if (GUI.Button(new Rect(40, 120, 200, 20), "Criar Palavra "))
            {
                CreateCustomWord();
                CreateWord();
            }
            if (GUI.Button(new Rect(40, 270, 200, 20), "Close this menu"))
            {
                cheat = false;
                index = 0;
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public void Start()
    {
        if (bl_GameInfo.Instance.GameMode == Mode.SinglePlayer)
        {
            CreateWord();
        }
        else
        {
            OpenWordCreatorPanel();
        }
        Clue1Text.CrossFadeColor(new Color(0, 1, 0, 0), 0.01f, true, true);
        Clue2Text.CrossFadeColor(new Color(0, 1, 0, 0), 0.01f, true, true);
        foreach (Image im in HangManParts) { im.CrossFadeColor(new Color(1, 0, 0, 0), 0.01f, true, true); }
        StartCoroutine(DesactiveOnTime(LoadAnimator.gameObject));
        AudioListener.volume = bl_GameInfo.Instance.Volumen;
        AudioListener.pause = !bl_GameInfo.Instance.Audio;

        HangmanPartPositions = new Vector2[HangManParts.Length];
        for (int i = 0; i < HangManParts.Length; i++)
        {
            HangmanPartPositions[i] = HangManParts[i].rectTransform.anchoredPosition;
        }
        cheatCode = new string[] {
         "g",
         "r",
         "u",
         "p",
         "o",
         "1"
        };
              index = 0;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="k"></param>
    public bool NewSelect(KeyCode k)
    {
        string _char = k.ToString().ToLower();
        bool found = false;
        for (int i = 0; i < CurrentWord.Length; i++)
        {
            //If letter exist in the word
            if (CurrentWord[i].ToString().ToLower() == _char)
            {
                //show letter
                ActiveLetters(_char);
                bl_ScoreManager.Instance.SuccessLetter();
                found = true;
            }
        }

        //else if not exist, then descount a try.
        if (!found)
        {
            OnError();
            CheckProgress();
        }
        return found;
    }

    /// <summary>
    /// 
    /// </summary>
    public void OpenWordCreatorPanel()
    {

        //WordCreatorInfoText.text = string.Format("Write here the secret word that  {0} must guess (prevents {1} look now), and adds the two clue about the word. <i>(lenght max 10)</i>", bl_GameInfo.Instance.CurrentPlayerTurn.ToUpper(), bl_GameInfo.Instance.CurrentPlayerTurn.ToUpper());
       // WordCreatorLogText.CrossFadeAlpha(0, 0.1f, true);
        CustomWordCreatorPanel.SetActive(true);
        CustomWordCreatorPanel.GetComponent<Animator>().SetBool("show", true);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="word"></param>
    public void CreateCustomWord()
    {
        CustomWordCreatorPanel.SetActive(true);
        CustomWordCreatorPanel.GetComponent<Animator>().SetBool("show", true);
        if (string.IsNullOrEmpty(WordInput.text))
        {
            WordCreatorLogText.text = "Please writte a word.";
            WordCreatorLogText.CrossFadeAlpha(1, 0.5f, true);
            return;
        }
        if(WordInput.text.Length < 3) { WordCreatorLogText.text = "Word must have at least 3 characters"; WordCreatorLogText.CrossFadeAlpha(1, 0.5f, true);return; }
    
        WordCreatorLogText.CrossFadeAlpha(0, 0.5f, true);
        //Pass test, then create word.
        CurrentWord = WordInput.text;
        CleanSentence();

        //Instances Letters and spaces
        for (int i = 0; i < CurrentWord.Length; i++)
        {
            if (CurrentWord.ToCharArray()[i] == ' ' || CurrentWord.ToCharArray()[i] == '\0')
            {
                GameObject spa = Instantiate(SpacePrefab) as GameObject;
                cacheSpace.Add(spa);
                spa.transform.SetParent(WordPanel, false);
            }
            else
            {
                GameObject cha = Instantiate(CharPrefab) as GameObject;
                cha.GetComponent<bl_Char>().GetInfo(CurrentWord[i].ToString());
                AllChars.Add(cha.GetComponent<bl_Char>());
                cacheChars.Add(cha.GetComponent<bl_Char>());
                cha.transform.SetParent(WordPanel, false);
            }
        }
        //Hide WordCreator
        CustomWordCreatorPanel.GetComponent<Animator>().SetBool("show", false);
        WordInput.text = string.Empty;
        StartCoroutine(DesactiveOnTime(CustomWordCreatorPanel));
    }

    /// <summary>
    /// 
    /// </summary>
    public void CreateWord()
    {
        if (bl_GameInfo.Instance.GameMode == Mode.SinglePlayer)
        {
            CurrentWord = WordsDataBase.GetWord(m_DifficultMode);
            CleanSentence();

            for (int i = 0; i < CurrentWord.Length; i++)
            {
                if (CurrentWord.ToCharArray()[i] == ' ' || CurrentWord.ToCharArray()[i] == '\0')
                {
                    GameObject spa = Instantiate(SpacePrefab) as GameObject;
                    cacheSpace.Add(spa);
                    spa.transform.SetParent(WordPanel, false);
                }
                else
                {
                    GameObject cha = Instantiate(CharPrefab) as GameObject;
                    cha.GetComponent<bl_Char>().GetInfo(CurrentWord[i].ToString());
                    AllChars.Add(cha.GetComponent<bl_Char>());
                    cacheChars.Add(cha.GetComponent<bl_Char>());
                    cha.transform.SetParent(WordPanel, false);
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// 
    public void Store(int i)
    {
        if (i == 0)
        {
            storePanel.SetActive(true);
        }else if (i == 1)
        {
            storePanel.SetActive(false);
        }
    }

    public void CleanSentence()
    {
        foreach (GameObject g in cacheSpace)
        {
            Destroy(g);
        }
        foreach(bl_Char c in cacheChars)
        {
            Destroy(c.gameObject);
        }
        foreach(bl_Char ac in AllChars)
        {
            Destroy(ac.gameObject);
        }
        cacheChars.Clear();
        cacheSpace.Clear();
        AllChars.Clear();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="letter"></param>
    void ActiveLetters(string letter)
    {
        for (int i = 0; i < AllChars.Count; i++)
        {
            if(AllChars[i].Char.ToLower() == letter.ToLower())
            {
                AllChars[i].ActiveLetter();
                AllChars.RemoveAt(i);
                FSXSource.clip = HitAudio;
                FSXSource.Play();
                CheckProgress();

            }
        }
    }

    public void ToMainMenu() { Application.LoadLevel("Menu"); }

    /// <summary>
    /// 
    /// </summary>
    void CheckProgress()
    {
        if(Trys <= 0)
        {
            FailedSentence();
            return;    
        }
        if(LettersLeft <= 0)
        {
            SuccessSentence();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void SuccessSentence()
    {
        FlashSentenceAnim.enabled = true;
        FlashSentenceAnim.Play("Flash", 0, 0);
        bl_ScoreManager.Instance.SuccesSentence();
        FSXSource.clip = SuccessAudio;
        FSXSource.Play();
       // bl_AdsManager.Instance.AddMatch();
    }

    /// <summary>
    /// 
    /// </summary>
    void CheckForClues()
    {
        if(Trys <= 5)
        {
            Clue1Text.text = string.Format("<color=#00DFFCFF>CLUE 1:</color> {0}", Clue1Input.text.ToUpper());
            Clue1Text.CrossFadeColor(Color.white, 0.75f, true, true);
        }
        if(Trys <= 2)
        {
            Clue2Text.text = string.Format("<color=#00DFFCFF>CLUE 2:</color> {0}", Clue2Input.text.ToUpper());
            Clue2Text.CrossFadeColor(Color.white, 0.75f, true, true);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void FailedSentence()
    {
        bl_KeyboardManager.Instance.DesactiveAllKeys();
        FlashSentenceAnim.enabled = true;
        FlashSentenceAnim.Play("Flash", 0, 0);
        foreach (Rigidbody2D r in Rigid2D) { r.AddForce(new Vector2(700, 200), ForceMode2D.Force); }
        for (int i = 0; i < AllChars.Count; i++)
        {
            AllChars[i].Failed();
        }
        FSXSource.clip = FailAudio;
        FSXSource.Play();
       // bl_AdsManager.Instance.AddMatch();
        bl_ScoreManager.Instance.FailSentence();
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnError()
    {
        Trys--;
        if (HangManParts.Length - 1 >= CurrentPart)
        {
            HangManParts[CurrentPart].CrossFadeColor(Color.white, 1, true, true);
            CurrentPart++;
        }
        else { Debug.Log("Not have more part of hangman!"); }
        FSXSource.clip = FailHitAudio;
        FSXSource.Play();
        //if (bl_GameInfo.Instance.UseVibrate) { bl_HandleVibration.Vibrate(); }
        TryText.text = string.Format("<b>REMAIN:</b> {0}/{1}", Trys, MaxTrys);
        bl_ScoreManager.Instance.FailLetter();
        if(bl_GameInfo.Instance.GameMode == Mode.TwoPlayers) { CheckForClues(); }
    }

    /// <summary>
    /// 
    /// </summary>
    public void Reset()
    {
        Trys = MaxTrys;
        CurrentPart = 0;
        foreach(Image im in HangManParts) { im.CrossFadeColor(new Color(1, 0, 0, 0), 0.01f, true, true); }
        FlashSentenceAnim.enabled = false;
        bl_KeyboardManager.Instance.ResetKeys();
        bl_ScoreManager.Instance.Reset();
        Clue1Input.text = string.Empty;
        Clue2Input.text = string.Empty;
        Clue1Text.CrossFadeColor(new Color(0, 1, 0, 0), 0.01f, true, true);
        Clue2Text.CrossFadeColor(new Color(0, 1, 0, 0), 0.01f, true, true);
        Clue1Text.text = string.Empty;
        Clue2Text.text = string.Empty;

        for (int i = 0; i < HangManParts.Length; i++)
        {         
            HangManParts[i].rectTransform.anchoredPosition = HangmanPartPositions[i];
            HangManParts[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            HangManParts[i].GetComponent<Rigidbody2D>().angularVelocity = 0;
        }

        if (bl_GameInfo.Instance.GameMode == Mode.SinglePlayer)
        {
            CreateWord();
        }
        else
        {
            OpenWordCreatorPanel();
        }
    }

    IEnumerator DesactiveOnTime(GameObject g)
    {
        yield return new WaitForSeconds(1);
        g.SetActive(false);
    }
   //TEMPO
   public void BuyItem1(int cost)
   {
       if (TotalPots >= cost)
       {
           TotalPots -= cost;
           time.stopTime();
           rewardIndicator.SetActive(true);
       }
       else
       {
            rewardIndicator.SetActive(false);
            noCoinsIndicator.SetActive(true);
       }
   }
    // PALAVRA CERTA

    public void BuyItem2(int cost)
    {
        
        if (TotalPots >= cost)
        {
            TotalPots -= cost;
            SuccessSentence();
            rewardIndicator.SetActive(true);

        }
        else
        {
            rewardIndicator.SetActive(false);
            noCoinsIndicator.SetActive(true);
        }
    }
    // TENTATIVA
    public void BuyItem3(int cost)
    {
        
        if (TotalPots >= cost)
        {
            TotalPots -= cost;
            Trys += 1;
            rewardIndicator.SetActive(true);
        }
        else
        {
            rewardIndicator.SetActive(false);
            noCoinsIndicator.SetActive(true);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public int LettersLeft
    {
        get
        {
            return AllChars.Count;
        }
    }

    public void callKeyboard()
    {
        TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, true);
        Debug.Log("Teclado ADM");

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(cheatCode[index]))
            {
                // right input, check next digit
                index++;
            }
            else
            {
                // wrong input, restart from index 0
                index = 0;
            }
        }

        if (index == cheatCode.Length)
        {
            // cheat is ok, the hidden menu is now visible :-)
            cheat = true;
            index = 0;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public int TryDone
    {
        get
        {
            return MaxTrys - Trys;
        }
    }

    public static bl_GameManager Instance
    {
        get
        {
            return ((bl_GameManager)mInstance);
        }
        set
        {
            mInstance = value;
        }
    }

}