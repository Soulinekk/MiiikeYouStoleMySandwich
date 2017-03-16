using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

    public enum Slot { DEFAULT, ITEM_SKILLS_EMPTY, ITEM_SKILLS_TAKEN, ITEM_SWORD_EMPTY, ITEM_SWORD_TAKEN, SKILL_EMPTY, SKILL_TAKEN, SWORD_EMPTY, SWORD_TAKEN };
    public Slot typeOfSlot = Slot.DEFAULT;


    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public void OnDrop(PointerEventData eventData)
    {

    }
}
