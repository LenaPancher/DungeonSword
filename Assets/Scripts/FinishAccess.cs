using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class FinishAccess : MonoBehaviour
{
    private bool isTriggered = false;
    public SpriteRenderer spriteRenderer;
    public Sprite newSprite;
    private Rigidbody2D rb;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isTriggered == false)
        {
            isTriggered = true;
            if (newSprite != null)
            {
                spriteRenderer.sprite = newSprite;
            }
            rb = collision.GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Static;
            StartCoroutine(ChangeScene());
        }
    }

    IEnumerator ChangeScene(){
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
