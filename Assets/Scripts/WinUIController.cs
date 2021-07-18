using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinUIController : MonoBehaviour
{
    public void Restart()
    {
        SceneManager.LoadScene("Template");
    }
}
