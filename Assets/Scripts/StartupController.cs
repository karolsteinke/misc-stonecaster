using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartupController : MonoBehaviour
{
    [SerializeField] private GameObject credits;

    public void LoadLevel() {
        SceneManager.LoadScene("Level01");
    }

    public void ShowCredits() {
        credits.SetActive(true);
    }

    public void HideCredits() {
        credits.SetActive(false);
    }
}
