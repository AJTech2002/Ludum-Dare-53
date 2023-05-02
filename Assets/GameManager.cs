using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Animator fadeBlackAnimator;

    TextMeshProUGUI timerText;
    TextMeshProUGUI houseText;
    TextMeshProUGUI accomplishmentText;

    public string nextScene;
    
    private int totalHouses;

    private void Awake() {
        instance = this;
        fadeBlackAnimator.SetBool("playing", true);
        timerText = GameObject.Find("TimerText").GetComponent<TextMeshProUGUI>();
        houseText = GameObject.Find("HouseText").GetComponent<TextMeshProUGUI>();
        accomplishmentText = GameObject.Find("AccomplishmentText").GetComponent<TextMeshProUGUI>();
        timerText.gameObject.SetActive(false);
        houseText.gameObject.SetActive(false);
        accomplishmentText.gameObject.SetActive(false);
        
        totalHouses = GameObject.FindObjectsOfType<House>().Length;
        houseText.text = housesReached.ToString()+"/"+totalHouses.ToString();
    }

    public static void Death() {
        instance.ResetScene();
    }

    public async void ResetScene() {
        await System.Threading.Tasks.Task.Delay(3000);
        fadeBlackAnimator.SetBool("playing", false);
        await System.Threading.Tasks.Task.Delay(1000);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        Time.timeScale = 1f;
    }

    public bool timerStarted;

    private float timer = 0f;

    private bool retrying = false;

    private void Update() {
        if (timerStarted) {
            timer += Time.deltaTime;
            timerText.text = Mathf.Floor(timer/60).ToString() + ":" + Mathf.RoundToInt(timer%60).ToString("00");
        }

        if (Input.GetKeyDown(KeyCode.R) && finished) {
            retrying = true;
            accomplishmentText.text = "Sweet, lets go!";
        }
    }

    public static void TruckStarted() {
        instance.timerText.gameObject.SetActive(true);
        instance.houseText.gameObject.SetActive(true);
        instance.timerStarted = true;
        
    }

    private int housesReached;
    bool finished = false;

    public async void IncrememtHousesReahed() {
        housesReached ++;

        houseText.text = housesReached.ToString()+"/"+totalHouses.ToString();

        if (housesReached >= totalHouses) {
            timerStarted = false;
        
            
            float previousBest = PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name, -1f);
            float currentBest = timer;
            
            if (previousBest == -1) {
                string timeText = Mathf.Floor(timer/60).ToString() + ":" + Mathf.RoundToInt(timer%60).ToString("00");
                string levelAccomplishment = $"Nice! You finished in <color=#dwwd>{timeText}</color>, try again to beat it! (R) to Retry";
                accomplishmentText.text = levelAccomplishment;
            }
            else {
                string previousText = Mathf.Floor(previousBest/60).ToString() + ":" + Mathf.RoundToInt(previousBest%60).ToString("00");
                string timeText = Mathf.Floor(timer/60).ToString() + ":" + Mathf.RoundToInt(timer%60).ToString("00");
                if (previousBest > currentBest) {
                    string levelAccomplishment = $"CONGRATS! You set a new personal record of <color=#dwwd>{timeText}</color>, your previous was <color=#dwwd>{previousText}</color>! (R) to Retry";
                    accomplishmentText.text = levelAccomplishment;
                }
                else {
                    string levelAccomplishment = $"DARN! You did worse than before with <color=#dwwd>{timeText}</color>, your previous was <color=#dwwd>{previousText}</color> - try again! (R) to Retry";
                    accomplishmentText.text = levelAccomplishment;
                }
            }

            await System.Threading.Tasks.Task.Delay(1000);
            fadeBlackAnimator.SetBool("playing", false);
            await System.Threading.Tasks.Task.Delay(1000);
            accomplishmentText.gameObject.SetActive(true);
            finished = true;
            await System.Threading.Tasks.Task.Delay(3000);
            
            if (previousBest > currentBest) PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name, currentBest);

            if (!retrying)
                SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
            else 
                SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }
    }
    
    public static void HouseReached() {
        instance.IncrememtHousesReahed();
    }

}
