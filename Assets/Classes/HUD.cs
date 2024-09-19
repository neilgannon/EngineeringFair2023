using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private GameObject AstronautIconPrefab;
    [SerializeField] private Transform IconDisplayPanel;
    [SerializeField] private Image imgHealthBar;
    [SerializeField] private GameObject GameOverScreen;
    [SerializeField] private GameObject GameCompleteScreen;

    void Start()
    {
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        player.AstronautCollected.AddListener(OnAstronautCollected);
        player.HealthUpdated.AddListener(OnPlayerHealthUpdate);
        player.Died.AddListener(ShowGameOverScreen);
        player.GameComplete.AddListener(ShowGameCompleteScreen);

        FillPanelWithIcons(GameObject.FindGameObjectsWithTag("Astronaut").Length);
    }

    private void OnPlayerHealthUpdate(float currentHealth, float maxHealth)
    {
        imgHealthBar.fillAmount = currentHealth / maxHealth;
    }  
    
    private void OnAstronautCollected()
    {
        Destroy(IconDisplayPanel.transform.GetChild(0).gameObject);
    }

    private void FillPanelWithIcons(int numberOfAstronauts)
    {
        for (int i = 0; i < numberOfAstronauts; i++)
        {
            Instantiate(AstronautIconPrefab, IconDisplayPanel);
        }
    }

    private void ShowGameOverScreen()
    {
        GameOverScreen.SetActive(true);
    }

    private void ShowGameCompleteScreen()
    {
        GameCompleteScreen.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
