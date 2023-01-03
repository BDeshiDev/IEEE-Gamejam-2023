using System.Collections;
using System.Collections.Generic;
using Combat.Pickups;
using TMPro;
using UnityEngine;
/// <summary>
/// View for a single ItemSlot
/// </summary>
public class PickupView : MonoBehaviour
{
    public SpriteRenderer spriter;
    public TextMeshProUGUI countText;

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
    }
}
