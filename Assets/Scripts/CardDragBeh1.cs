using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragBeh1 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    #region Variables
    public enum Card { DEFAULT, ITEM_SWORD, ITEM_SKILLS, MAGIC, SWORD, MAGIC_AND_SWORD };
    public Card TypeOfItem = Card.DEFAULT;

    public enum CardState { EQUIPMENT, SLOT };
    [SerializeField]
    public CardState itemState = CardState.EQUIPMENT;

    public GameObject draggedCardCopy = null;
    public bool dropped = false;
    public Transform transformToAttach;
    public GameObject cardFrame;
    public GameObject motherCard;
    #endregion

    public void OnBeginDrag(PointerEventData eventData)
    {
        switch (itemState)
        {
            case CardState.EQUIPMENT:
                
                draggedCardCopy = Instantiate(gameObject, transform.parent.parent.parent);
                draggedCardCopy.GetComponent<CardDragBeh1>().motherCard = gameObject;
                draggedCardCopy.GetComponent<CanvasGroup>().blocksRaycasts = false;
                
                break;

            case CardState.SLOT:

                #region Open slot
                DropZone dz = this.transform.parent.GetComponent<DropZone>();
                switch (dz.typeOfSlot)
                {
                    case DropZone.Slot.ITEM_SKILLS_TAKEN:

                        dz.typeOfSlot = DropZone.Slot.ITEM_SKILLS_EMPTY;
                        GM.Instance.cardByIdInSlots[this.GetComponent<CardInfo>().id]--;
                        if (GM.Instance.cardByIdLimits[this.GetComponent<CardInfo>().id] == GM.Instance.cardByIdInSlots[this.GetComponent<CardInfo>().id])
                        {
                            motherCard.GetComponent<CardDragBeh1>().GetComponent<CanvasGroup>().blocksRaycasts = false;
                            motherCard.GetComponent<CardDragBeh1>().cardFrame.SetActive(true);
                        }

                        break;

                    case DropZone.Slot.ITEM_SWORD_TAKEN:

                        dz.typeOfSlot = DropZone.Slot.ITEM_SWORD_EMPTY;
                        GM.Instance.cardByIdInSlots[this.GetComponent<CardInfo>().id]--;
                        if (GM.Instance.cardByIdLimits[this.GetComponent<CardInfo>().id] == GM.Instance.cardByIdInSlots[this.GetComponent<CardInfo>().id])
                        {
                            motherCard.GetComponent<CardDragBeh1>().GetComponent<CanvasGroup>().blocksRaycasts = false;
                            motherCard.GetComponent<CardDragBeh1>().cardFrame.SetActive(true);
                        }

                        break;

                    case DropZone.Slot.SKILL_TAKEN:

                        dz.typeOfSlot = DropZone.Slot.SKILL_EMPTY;
                        GM.Instance.cardByIdInSlots[this.GetComponent<CardInfo>().id]--;
                        if (GM.Instance.cardByIdLimits[this.GetComponent<CardInfo>().id] == GM.Instance.cardByIdInSlots[this.GetComponent<CardInfo>().id])
                        {
                            motherCard.GetComponent<CardDragBeh1>().GetComponent<CanvasGroup>().blocksRaycasts = false;
                            motherCard.GetComponent<CardDragBeh1>().cardFrame.SetActive(true);
                        }

                        break;

                    case DropZone.Slot.SWORD_TAKEN:

                        dz.typeOfSlot = DropZone.Slot.SWORD_EMPTY;
                        GM.Instance.cardByIdInSlots[this.GetComponent<CardInfo>().id]--;
                        if (GM.Instance.cardByIdLimits[this.GetComponent<CardInfo>().id] == GM.Instance.cardByIdInSlots[this.GetComponent<CardInfo>().id])
                        {
                            motherCard.GetComponent<CardDragBeh1>().GetComponent<CanvasGroup>().blocksRaycasts = false;
                            motherCard.GetComponent<CardDragBeh1>().cardFrame.SetActive(true);
                        }

                        break;
                }
                #endregion

                transformToAttach = this.transform.parent;
                this.transform.SetParent(this.transform.parent.parent.parent);
                GetComponent<CanvasGroup>().blocksRaycasts = false;

                break;                
        }
    }

    public void OnDrag(PointerEventData eventData)
    {

        switch (itemState)
        {
            case CardState.EQUIPMENT:
                
                draggedCardCopy.transform.position = eventData.position;

                break;

            case CardState.SLOT:

                this.transform.position = eventData.position;

                break;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        switch (itemState)
        {
            #region Case CardState.Equipment
            case CardState.EQUIPMENT:

                if (draggedCardCopy.GetComponent<CardDragBeh1>().dropped == false)
                {
                    Destroy(draggedCardCopy);
                }
                else
                {
                    draggedCardCopy.transform.SetParent(transformToAttach);
                    draggedCardCopy.transform.position = transformToAttach.position;
                    draggedCardCopy.GetComponent<CanvasGroup>().blocksRaycasts = true;
                    draggedCardCopy.GetComponent<CardDragBeh1>().itemState = CardState.SLOT;
                    draggedCardCopy.GetComponent<CardDragBeh1>().dropped = false;
                    draggedCardCopy = null;
                    transformToAttach = null;
                }

                break;
            #endregion

            #region Case CardState.Slot
            case CardState.SLOT:

                if (dropped == false)
                {
                    if (motherCard.GetComponent<CardDragBeh1>().cardFrame == true)
                    {
                        motherCard.GetComponent<CanvasGroup>().blocksRaycasts = true;
                        motherCard.GetComponent<CardDragBeh1>().cardFrame.SetActive(false);
                    }
                    Destroy(gameObject);
                }
                else
                {
                    this.transform.SetParent(transformToAttach);
                    this.transform.position = transformToAttach.position;
                    this.GetComponent<CanvasGroup>().blocksRaycasts = true;
                    this.GetComponent<CardDragBeh1>().itemState = CardState.SLOT;
                    this.GetComponent<CardDragBeh1>().dropped = false;
                    this.transformToAttach = null;
                }

                break;
                #endregion
        }

    }
}
