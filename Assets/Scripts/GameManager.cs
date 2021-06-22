using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Font[] fonts = null;
    [SerializeField] private GameObject[] enemyPref = null;
    [SerializeField] private GameObject boss = null;
    [SerializeField] private GameObject[] middleBoss = null;
    [SerializeField] private GameObject[] itemPref = null;
    [SerializeField] private Canvas[] menus = null;
    [SerializeField] private Text[] menuTexts = null;
    [SerializeField] private Text[] texts = null;
    [SerializeField] private Text[] overTexts = null;
    [SerializeField] private Image mainOverImage = null;
    [SerializeField] private Image soundButtonImage = null;
    [SerializeField] private Button languageButton = null;
    [SerializeField] private Text languageText = null;
    [SerializeField] private Text[] buttonOverText = null;
    [SerializeField] private Sprite[] mainOverSprite = null;
    [SerializeField] private Sprite[] soundSprite = null;
    [SerializeField] private Sprite[] flagSprite = null;
    [SerializeField] private int life = 0;
    [SerializeField] private int boom = 0;
    private Animator animator = null;

    public PoolingManager poolingManager { get; private set; }
    private AudioSource audioManager = null;
    private AudioListener audioListener = null;
    private Shaking[] shaking = null;
    public Vector2 MaxPos { get; private set; }
    public Vector2 MinPos { get; private set; }
    private int languageCount = 0;
    private float score = 0;
    private float highScore = 0;
    private float scoreBoss = 0f;
    private float middleScoreBoss = 0f;
    public int bossCount { get; private set; }
    private int sound = 1;
    public bool isBossSpawned { get; private set; }
    public bool isMenu { get; private set; }
    private bool isBoom = false;


    void Start()
    {
        sound = PlayerPrefs.GetInt("SOUND", 1);
        audioListener = Camera.main.GetComponent<AudioListener>();
        audioManager = GetComponent<AudioSource>();
        languageCount = PlayerPrefs.GetInt("LANGUAGE");
        poolingManager = FindObjectOfType<PoolingManager>();
        highScore = PlayerPrefs.GetFloat("HIGHSCORE");
        animator = GetComponent<Animator>();
        shaking = FindObjectsOfType<Shaking>();

        isBossSpawned = false;
        isMenu = false;
        StartCoroutine(SpawnRiver());
        StartCoroutine(SpawnSky());
        StartCoroutine(SpawnItem(itemPref[0]));
        UpdateUI();
    }

    void Update()
    {
        if (Time.timeScale == 1)
            audioListener.enabled = sound == 1 ? true : false;
        else
            audioListener.enabled = false;
        MaxPos = new Vector2(Camera.main.aspect * Camera.main.orthographicSize - 0.1f, Camera.main.orthographicSize - 1f);
        MinPos = new Vector2(-Camera.main.aspect * Camera.main.orthographicSize + 0.1f, -Camera.main.orthographicSize);

        if (!boss.activeInHierarchy && (!middleBoss[0].activeInHierarchy && !middleBoss[1].activeInHierarchy))
            isBossSpawned = false;
        else
            isBossSpawned = true;

        score += Time.deltaTime * 5;
        UpdateUI();
        if (score >= scoreBoss + 10000 && !isBossSpawned)
            SpawningBoss();
        if (score >= middleScoreBoss + 3000 && !isBossSpawned)
            SpawnMiddleBoss();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            score += 2000;
        }
    }

    private IEnumerator SpawnRiver()
    {
        while (true)
        {
            if (score < 1000)
                yield return new WaitForSeconds(Random.Range(1, 2.5f));
            else if (score < 2000)
                yield return new WaitForSeconds(1f);
            else
                yield return new WaitForSeconds(0.8f);
            if (!isBossSpawned)
            Pooling(enemyPref[0] , new Vector2(Random.Range(MinPos.x, MaxPos.x), MaxPos.y + 0.1f), Quaternion.identity); 
            
        }
    }

    private IEnumerator SpawnSky()
    {
        while (true)
        {
            yield return new WaitForSeconds(7f);
            if (!isBossSpawned)
            if (score > 2000)
                Pooling(enemyPref[1], new Vector2(MaxPos.x + 0.1f, Random.Range(2f, 3f)), Quaternion.identity);
        }
    }

    private IEnumerator SpawnItem(GameObject item)
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(18f, 22f));
            Pooling(item, new Vector2(Random.Range(MinPos.x, MaxPos.x), MaxPos.y + 0.1f), Quaternion.identity);
        }
    }
            

    public void Despawn(GameObject any)
    {
        any.SetActive(false);
        any.transform.SetParent(poolingManager.transform);
    }

    public GameObject Pooling(GameObject original, Vector3 pos, Quaternion rot)
    {
        GameObject result = null;
        int i = 0;
        if (poolingManager.transform.childCount > 0)
        {
            while (true)
            {
                result = poolingManager.transform.GetChild(i).gameObject;
                i++;
                if(result.name == original.name)
                    if(result.layer == original.layer)
                        if (result.CompareTag(original.tag))
                            break;
                if (i >= poolingManager.transform.childCount)
                {
                    result = Instantiate(original, pos, rot);
                    result.name = original.name;
                    result.transform.SetParent(null);
                    result.transform.position = pos;
                    result.transform.rotation = rot;
                    return result;

                }
            }
            result.name = original.name;
            result.transform.SetParent(null);
            result.transform.position = pos;
            result.transform.rotation = rot;
            result.SetActive(true);
            //result.GetComponent<Animator>().runtimeAnimatorController = enemyPref[count].GetComponent<Animator>().runtimeAnimatorController;
        }
        else
        {
            result = Instantiate(original, pos, rot);
            result.name = original.name;
            result.transform.SetParent(null);
            result.transform.position = pos;
        }
        return result;
    }

    public void ResettingScore(int count)
    {
        if(count == 1)
            scoreBoss = score;
        middleScoreBoss = score;
    }

    public void OpenMenu()
    {
        if (sound == -1) soundButtonImage.sprite = soundSprite[1];
        else soundButtonImage.sprite = soundSprite[0];
        SetMenu();
        menus[0].enabled = true;
        isMenu = true;
        Time.timeScale = 0;
    }
    public void CloseMenu()
    {
        menus[0].enabled = false;
        isMenu = false;
        Time.timeScale = 1;
    }

    public void ReStart()
    {
        SceneManager.LoadScene("Main");
        isMenu = false;
        Time.timeScale = 1;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Dead()
    {
        life--;
        audioListener.enabled = false;
        StartCoroutine(shaking[0].Shake());
        StartCoroutine(shaking[1].Shake());
        if (score - 150 > 0)
            score -= 150;
        else
            score = 0;
        if (life <= 0)
        {
            PlayerPrefs.SetFloat("HIGHSCORE", highScore);
            menus[1].enabled = true;
            mainOverImage.sprite = mainOverSprite[languageCount];
            switch(languageCount){
                case 0:
                    for(int i = 0; i < 2; i ++){
                        buttonOverText[i].font = fonts[0];
                        overTexts[i].font = fonts[0];
                    }
                    buttonOverText[0].text = "재시작";
                    buttonOverText[1].text = "종료";
                    overTexts[0].text = string.Format("점수 {0 : 0}점", score);
                    overTexts[1].text = string.Format("최고 점수 {0 : 0}점", highScore);
                    break;
                case 1:
                    for(int i = 0; i < 2; i ++){
                        buttonOverText[i].font = fonts[0];
                        overTexts[i].font = fonts[0];
                    }
                    buttonOverText[0].text = "Retry";
                    buttonOverText[1].text = "Quit";
                    overTexts[0].text = string.Format("Score {0 : 0}P", score);
                    overTexts[1].text = string.Format("BestScore {0 : 0}p", highScore);
                     break;
                case 2:
                    for(int i = 0; i < 2; i ++){
                        buttonOverText[i].font = fonts[1];
                        overTexts[i].font = fonts[1];
                    }   
                    buttonOverText[0].text = "も一回";
                    buttonOverText[1].text = "終了";
                    overTexts[0].text = string.Format("点数 {0 : 0}点", score);
                    overTexts[1].text = string.Format("最高点数 s{0 : 0}点", highScore);
                     break;

            }
            Time.timeScale = 0;
        }
    }

    public void AddScore(float addScore)
    {
        score += addScore;
        if (score > highScore)
            highScore = score;
    }

    private void UpdateUI()
    {
        texts[0].text = string.Format("SCORE {0 : 0}", score);
        texts[1].text = string.Format("LIFE {0}", life);
        texts[2].text = string.Format("{0}", boom);
    }

    public void Bomb()
    {
        if (boom <= 0 || isBoom)
            return;
        StartCoroutine(Boom());
    }

    private IEnumerator Boom()
    {
        isBoom = true;
        boom--;
        transform.localScale = Vector3.one;
        animator.Play("Bomb");
        audioManager.Play();
        yield return new WaitForSeconds(0.2f);

        transform.localScale = new Vector3(Camera.main.aspect * Camera.main.orthographicSize * 2 / 5, 1, 1);
        yield return new WaitForSeconds(0.3f);

        GameObject[] enemy = FindObjectsOfType(typeof(GameObject)) as GameObject[];


        for (int i = 0; i < enemy.Length; i++)
        {
            if (enemy[i].layer == LayerMask.NameToLayer("Enemy"))
            {
                Debug.Log(enemy[i]);
                if (enemy[i].CompareTag("River") || enemy[i].CompareTag("Sky"))
                    StartCoroutine(enemy[i].GetComponent<EnemyRiver>().Dead());
                else
                    Despawn(enemy[i]);
            }
        }

        animator.Play("Idle");
        isBoom = false;
    }

    private void SpawningBoss()
    {
        boss.SetActive(true);
        bossCount += 1;
        isBossSpawned = true;
    }

    private void SpawnMiddleBoss()
    {
        middleBoss[0].SetActive(true);
        middleBoss[1].SetActive(true);
        isBossSpawned = true;
    }

    public void SelectLanguage()
    {
        languageCount++;
        if (languageCount > 2) languageCount = 0;
        SetMenu();
        PlayerPrefs.SetInt("LANGUAGE", languageCount);
    }

    private void SetMenu(){
        languageButton.image.sprite = flagSprite[languageCount];
        switch(languageCount){
            case 0:
                languageText.font = fonts[0];
                languageText.text = "언어: ";
                for(int i = 0; i < 3; i++)
                    menuTexts[i].font = fonts[0];
                menuTexts[0].text = "계속";
                menuTexts[1].text = "재시작";
                menuTexts[2].text = "종료";
                break;
            case 1:
                menuTexts[0].text = "Continue";
                menuTexts[1].text = "Retry";
                menuTexts[2].text = "Quit";
                languageText.text = "Language: ";
                break;
            case 2:
                languageText.font = fonts[1];
                for(int i = 0; i < 3; i++)
                    menuTexts[i].font = fonts[1];
                menuTexts[0].text = "続く";
                menuTexts[1].text = "リスタート";
                menuTexts[2].text = "終了";
                languageText.text = "言語: ";
                break;
        }
    }
    public void TurnSound()
    {
        PlayerPrefs.SetInt("SOUND", sound *= -1);
        if (sound == -1) soundButtonImage.sprite = soundSprite[1];
        else soundButtonImage.sprite = soundSprite[0];
    }
}
