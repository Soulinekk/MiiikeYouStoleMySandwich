using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardDragBeh1 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public enum Card { DEFAULT, ITEM_SWORD, ITEM_SKILLS, MAGIC, SWORD };
    public Card TypeOfItem = Card.DEFAULT;

    private enum CardState { EQUIPMENT, SLOT };
    [SerializeField]
    private CardState itemState = CardState.EQUIPMENT;

    private GameObject draggedCardCopy = null;
    public bool dropped = false;
    public Transform transformToAttach;
    public GameObject cardFrame;
    public GameObject motherCard;

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
                    GM.Instance.cardByIdInSlots[this.GetComponent<CardInfo>().id]++;
                    if (GM.Instance.cardByIdLimits[this.GetComponent<CardInfo>().id] == GM.Instance.cardByIdInSlots[this.GetComponent<CardInfo>().id])
                    {
                        this.GetComponent<CanvasGroup>().blocksRaycasts = false;
                        cardFrame.SetActive(true);
                    }
                    draggedCardCopy = null;
                    transformToAttach = null;
                }

                break;

            case CardState.SLOT:

                if (dropped == false)
                {
                    if (GM.Instance.cardByIdLimits[this.GetComponent<CardInfo>().id] == GM.Instance.cardByIdInSlots[this.GetComponent<CardInfo>().id])
                    {
                        motherCard.GetComponent<CanvasGroup>().blocksRaycasts = true;
                        motherCard.GetComponent<CardDragBeh1>().cardFrame.SetActive(false);
                    }
                    GM.Instance.cardByIdInSlots[this.GetComponent<CardInfo>().id]--;
                    Destroy(gameObject);
                }
                else
                {
                    this.transform.SetParent(transformToAttach);
                    this.transform.position = transformToAttach.position;
                    this.GetComponent<CanvasGroup>().blocksRaycasts = true;
                    this.GetComponent<CardDragBeh1>().itemState = CardState.SLOT;
                    this.GetComponent<CardDragBeh1>().dropped = false;
                }

                break;
        }

    }

}
