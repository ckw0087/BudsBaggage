using UnityEngine;

public class SelectBuds : MonoBehaviour
{
    [SerializeField] SpriteRenderer playerSprite;
    [SerializeField] SpriteRenderer selectedSprite;
    [SerializeField] GameObject chooseBuds;
    [SerializeField] GameObject buttonCanvas;
    [SerializeField] GameObject gameManager;

    GameObject currGameObject;
    string btnName;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currGameObject = GetComponent<GameObject>();
        btnName = currGameObject.name;
        buttonCanvas.SetActive(false);
        gameManager.SetActive(false);
    }

    public void ChangeBuds()
    {
        playerSprite.sprite = selectedSprite.sprite;
        chooseBuds.SetActive(false);
        buttonCanvas.SetActive(true);
        gameManager.SetActive(true);
    }
}
