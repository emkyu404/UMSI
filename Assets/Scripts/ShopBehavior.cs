using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopBehavior : MonoBehaviour
{
    private Button currentButton;
    private Image spriteImage;
    public GameObject turretPrice;
    public GameObject turretIcon;
    private Color originalColor;
    private Color iconColor;

    private void Awake()
    {
        currentButton = GetComponent<Button>();
        spriteImage = GetComponent<Image>();
        originalColor = spriteImage.color;
        iconColor = turretIcon.GetComponent<Image>().color;
    }
    // Update is called once per frame
    void Update()
    {
        if (!currentButton.interactable)
        {
            spriteImage.color = new Color(25f / 255f, 25f / 255f, 25f / 255f);
            turretIcon.GetComponent<Image>().color = new Color(25f / 255f, 25f / 255f, 25f / 255f);
            turretPrice.SetActive(false);
        }
        else
        {
            spriteImage.color = originalColor;
            turretIcon.GetComponent<Image>().color = iconColor;
            turretPrice.SetActive(true);
        } 
    }
}
