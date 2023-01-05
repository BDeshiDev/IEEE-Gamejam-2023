using System.Collections;
using System.Collections.Generic;
using Combat.Pickups;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// View for a single ItemSlot
/// </summary>
public class PickupView : MonoBehaviour
{
    public Image icon;
    public Image background;
    public TextMeshProUGUI countText;
    
    public GameObject inactiveIndicator;
    public RectTransform rect;
    public Vector2 activeStateSize = new Vector2(100,100);
    public Vector2 inactiveStateSize = new Vector2(50,50);
    public void refreshUI(ItemSlot pickupslot)
    {
        if (pickupslot.shouldShowCountInUI)
        {
            countText.gameObject.SetActive(true);
            countText.text = "x" + pickupslot.itemCount;
        }
        else
        {
            countText.gameObject.SetActive(false);
        }

        icon.sprite = pickupslot.itemIconSprite;
        background.color = pickupslot.itemColor;
    }

    public void setIconActiveState(bool shouldBeActive)
    {
        if (shouldBeActive)
        {
            rect.sizeDelta = new Vector2(activeStateSize.x ,activeStateSize.y);
            inactiveIndicator.gameObject.SetActive(false);


            countText.rectTransform.offsetMin = new Vector2(70, countText.rectTransform.offsetMin.y); // new Vector2(left, bottom);
            countText.rectTransform.offsetMax = new Vector2(countText.rectTransform.offsetMax.x, -70); // new Vector2(-right, -top);
        }
        else
        {
            
            rect.sizeDelta = new Vector2(inactiveStateSize.x ,inactiveStateSize.y);

            inactiveIndicator.gameObject.SetActive(true);
            
            countText.rectTransform.offsetMin = new Vector2(30, countText.rectTransform.offsetMin.y); // new Vector2(left, bottom);
            countText.rectTransform.offsetMax = new Vector2(countText.rectTransform.offsetMax.x, -30); // new Vector2(-right, -top);
        }
    }
}
