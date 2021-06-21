using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    [SerializeField] private Font[] fonts = null;
    [SerializeField] private Canvas menu = null;
    [SerializeField] private Image mainTextImage = null;
    [SerializeField] private Button languageButton = null;
    [SerializeField] private Text changeButtonText = null;
    [SerializeField] private Text languageText = null;
    [SerializeField] private Text[] enemyButtonText = null;
    [SerializeField] private Text highScoreText = null;
    [SerializeField] private Text[] mainText = null;
    [SerializeField] private Text[] itemText = null;
    [SerializeField] private Text doSelectText = null;
    [SerializeField] private GameObject enemySelect = null;
    [SerializeField] private GameObject enemyDictionaryObject = null;
    [SerializeField] private GameObject itemDictionaryObject = null;
    [SerializeField] private Sprite[] characterSprite = null;
    [SerializeField] private Sprite[] flagSprite = null;
    [SerializeField] private Sprite[] mainTextSprite = null;
    private Text dictionaryName = null;
    private Text dictionaryExplanation = null;
    private Text dictionaryStatus = null;
    private int languageCount = 0;
    private int selectedCount = 0;
    private int dictionaryType = 0;
    private Image dictionaryCharacter = null;


    private void Awake()
    {
        languageCount = PlayerPrefs.GetInt("LANGUAGE");
        dictionaryName = enemyDictionaryObject.transform.Find("Name").GetComponent<Text>();
        dictionaryExplanation = enemyDictionaryObject.transform.Find("Explanation").GetComponent<Text>();
        dictionaryStatus = enemyDictionaryObject.transform.Find("Status").GetComponent<Text>();
        dictionaryCharacter = enemyDictionaryObject.transform.Find("Character").GetComponent<Image>();
        SetMainText();
    }

    public void Game()
    {
        SceneManager.LoadScene("Main");
    }

    public void Help()
    {
        menu.enabled = true;
        SetButtonText();
    }

    public void CloseMenu()
    {
        menu.enabled = false;
        enemySelect.SetActive(true);
        enemyDictionaryObject.SetActive(false);
        itemDictionaryObject.SetActive(false);
        dictionaryType = 0;
    }

    public void BackMenu()
    {
        enemySelect.SetActive(true);
        enemyDictionaryObject.SetActive(false);
    }

    public void ShowDictionary(int count)
    {
        selectedCount = count;
        enemySelect.SetActive(false);
        enemyDictionaryObject.SetActive(true);
        dictionaryCharacter.sprite = characterSprite[count];
        SetDictionary(selectedCount);
    }

    public void ChangeDictionary()
    {
        dictionaryType++;
        if(dictionaryType == 1)
        {
            switch(languageCount){
                case 0:
                    changeButtonText.text = "아이템";
                    break;
                case 1:
                    changeButtonText.text = "Item";
                    break;
                case 2:
                    changeButtonText.text = "渡欧";
                    break;
            }
            SetItemDictionary();
            enemyDictionaryObject.SetActive(false);
            enemySelect.SetActive(false);
            itemDictionaryObject.SetActive(true);
        }
        else if (dictionaryType == 2)
        {
            switch(languageCount){
                case 0:
                    changeButtonText.text = "한자";
                    break;
                case 1:
                    changeButtonText.text = "Word";
                    break;
                case 2:
                    changeButtonText.text = "漢字";
                    break;
            }
            enemySelect.SetActive(true);
            itemDictionaryObject.SetActive(false);
            dictionaryType = 0;
        }
    }

    public void SelectLanguage()
    {
        languageCount++;
        if (languageCount > 2) languageCount = 0;
        languageButton.image.sprite = flagSprite[languageCount];
        switch(languageCount){
            case 0:
                languageText.font = fonts[0];
                languageText.text = "언어: ";
                break;
            case 1:
                languageText.text = "Language: ";
                break;
            case 2:
                languageText.font = fonts[1];
                languageText.text = "言語: ";
                break;
        }
        SetDictionary(selectedCount);
        SetButtonText();
        SetMainText();
        SetItemDictionary();
        PlayerPrefs.SetInt("LANGUAGE", languageCount);
    }

    private void SetMainText()
    {
        mainTextImage.sprite = mainTextSprite[languageCount];
        switch(languageCount){
            case 0:
                highScoreText.font = fonts[0];
                mainText[0].font = fonts[0];
                mainText[1].font = fonts[0];
                mainText[2].font = fonts[0];
                highScoreText.text = string.Format("최고 점수: {0 : 0}", PlayerPrefs.GetFloat("HIGHSCORE"));
                mainText[0].text = "시작";
                mainText[1].text = ": 한자 공부";
                mainText[2].text = ": 한자 공부";

                break;
            case 1:
                highScoreText.text = string.Format("Best Score: {0 : 0}", PlayerPrefs.GetFloat("HIGHSCORE"));
                mainText[0].text = "Start";
                mainText[1].text = ": Studying Chinese Characters";
                mainText[2].text = ": Studying Chinese Characters";

                break;
            case 2:
                highScoreText.font = fonts[1];
                mainText[0].font = fonts[1];
                mainText[1].font = fonts[1];
                mainText[2].font = fonts[1];
                highScoreText.text = string.Format("最高点数: {0 : 0}", PlayerPrefs.GetFloat("HIGHSCORE"));
                mainText[0].text = "始まり";
                mainText[1].text = ": 漢字勉強";
                mainText[2].text = ": 漢字勉強";
                break;
        }
    }

    private void SetButtonText()
    {
        switch(languageCount){
            case 0:
                languageText.font = fonts[0];
                languageText.text = "언어: ";
                break;
            case 1:
                languageText.text = "Language: ";
                break;
            case 2:
                languageText.font = fonts[1];
                languageText.text = "言語: ";
                break;
        }
        languageButton.image.sprite = flagSprite[languageCount];
        switch(languageCount){
            case 0:
                changeButtonText.font = fonts[0];
                if(dictionaryType == 0)
                    changeButtonText.text = "한자";
                else
                    changeButtonText.text = "아이템";
                enemyButtonText[0].text = "내 천";
                enemyButtonText[1].text = "하늘 천";
                enemyButtonText[2].text = "끊을 천";
                enemyButtonText[3].text = "일천 천";
                doSelectText.text = "클릭하세요.";
                break;
            case 1:
                if(dictionaryType == 0)
                    changeButtonText.text = "Word";
                else
                    changeButtonText.text = "Item";
                enemyButtonText[0].text = "River";
                enemyButtonText[1].text = "Sky";
                enemyButtonText[2].text = "Cutting";
                enemyButtonText[3].text = "Thousand";
                doSelectText.text = "Click These.";
                break;
            case 2:
                changeButtonText.font = fonts[1];
                if(dictionaryType == 0)
                    changeButtonText.text = "漢字";
                else
                    changeButtonText.text = "渡欧";
                enemyButtonText[0].text = "かわ";
                enemyButtonText[1].text = "あま";
                enemyButtonText[2].text = "キル";
                enemyButtonText[3].text = "ち";
                doSelectText.text = "クリックして。";
                break;
        }
    }

    private void SetDictionary(int count)
    {
        switch (languageCount)
        {
            
            case 0:
                dictionaryName.font = fonts[0];
                dictionaryExplanation.font = fonts[0];
                dictionaryStatus.font = fonts[0];
                switch (count)
                {
                    case 0:
                        dictionaryName.text = "내 천";
                        dictionaryExplanation.text = "아래로 내려가며, 획이 적다.";
                        dictionaryStatus.text = "속도: 2, 체력: 3, 점수: 100점";
                        break;
                    case 1:
                        dictionaryName.text = "하늘 천";
                        dictionaryExplanation.text = "좌우로 움직이며, 총알을 둥글게 뿌린다.";
                        dictionaryStatus.text = "속도: 0.8, 체력: 4, 점수: 250점";
                        break;
                    case 2:
                        dictionaryName.text = "끊을 천";
                        dictionaryExplanation.text = "물대포를 쏘며, 체력이 많다.";
                        dictionaryStatus.text = "부동 , 체력: 50, 점수: 맞을 때마다 10점";
                        break;
                    case 3:
                        dictionaryName.text = "일천 천";
                        dictionaryExplanation.text = "덩치가 크고, 체력이 매우 많다.";
                        dictionaryStatus.text = "부동 , 체력: 200, 점수: 맞을 때마다 10점";
                        break;
                }
                break;
            case 1:
                dictionaryName.fontSize = 110;
                switch (count)
                {
                    case 0:
                        dictionaryName.text = "River";
                        dictionaryExplanation.text = "Moving Down, Low Stroke";
                        dictionaryStatus.text = "Speed: 2, HP: 3, Score: 100p";
                        break;
                    case 1:
                        dictionaryName.text = "Sky";
                        dictionaryExplanation.text = "Moving Horizontally, Spread The Bullet Roundly";
                        dictionaryStatus.text = "Speed: 0.8, HP: 4, Score: 250p";
                        break;
                    case 2:
                        dictionaryName.text = "Cutting";
                        dictionaryExplanation.text = "Using Water Cannon, High HP";
                        dictionaryStatus.text = "UnMovable , HP: 50, Score: As You Hit 10p";
                        break;
                    case 3:
                        dictionaryName.text = "Thousand";
                        dictionaryExplanation.text = "Huge Size, Huge HP";
                        dictionaryStatus.text = "UnMovable , HP: 200, Score: As You Hit 10p";
                        break;
                }
                break;

            case 2:
                dictionaryName.fontSize = 150;
                dictionaryName.font = fonts[1];
                dictionaryExplanation.font = fonts[1];
                dictionaryStatus.font = fonts[1];
                switch (count)
                {
                    case 0:
                        dictionaryName.text = "かわ";
                        dictionaryExplanation.text = "下に動く、画がすくない。";
                        dictionaryStatus.text = "速度: 2, 体力: 3,　点数: 100点";
                        break;
                    case 1:
                        dictionaryName.text = "あま";
                        dictionaryExplanation.text = "左右に動く、弾丸を丸く発射する";
                        dictionaryStatus.text = "速度: 0.8, 体力: 4, 点数: 250点";
                        break;
                    case 2:
                        dictionaryName.text = "キル";
                        dictionaryExplanation.text = "水大砲を使かう、多い体力";
                        dictionaryStatus.text = "不動 , 体力: 50, 点数: 当たる瞬間 10点";
                        break;
                    case 3:
                        dictionaryName.text = "ち";
                        dictionaryExplanation.text = "大きい、無死";
                        dictionaryStatus.text = "不動 , 体力: 200, 点数: 当たる瞬間 10点";
                        break;
                }
                break;
        }
    }

    private void SetItemDictionary(){
        switch(languageCount){
            case 0:
                for(int i = 0; i < 4; i++)
                    itemText[i].font = fonts[0];
                itemText[0].text = "먹물 폭탄";
                itemText[1].text = "보스들을 제외한 적과 총알을 모두 파괴한다.";
                itemText[2].text = "벼루";
                itemText[3].text = "물감을 가득 품어 느려지지만 세진다";
                break;
            case 1:
                itemText[0].text = "Ink Bomb";
                itemText[1].text = "Destroy All Except Boss";
                itemText[2].text = "Ink Stone";
                itemText[3].text = "Get Slower By Getting Ink, But Get Stronger";
                break;
            case 2:
                for(int i = 0; i < 4; i++)
                    itemText[i].font = fonts[1];
                itemText[0].text = "墨汁爆弾";
                itemText[1].text = "ボス以外の全てを破壊";
                itemText[2].text = "硯";
                itemText[3].text = "重くなって遅くになれる、でも強くなる";
                break;
        }
    }
}
