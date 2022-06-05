using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private GameObject fadeInImage;
    private float fadeWait = 0.5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerMovement>().freezeMovement();
            StartCoroutine(fadeIn());
        }
    }

    private IEnumerator fadeIn()
    {
        Instantiate(fadeInImage, Vector3.zero, Quaternion.identity);
        yield return new WaitForSeconds(fadeWait);
        SceneManager.LoadScene(sceneToLoad);
    }
}
