using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler {

    public enum Slot { DEFAULT, ITEM_SKILLS_EMPTY, ITEM_SKILLS_TAKEN, ITEM_SWORD_EMPTY, ITEM_SWORD_TAKEN, SKILL_EMPTY, SKILL_TAKEN, SWORD_EMPTY, SWORD_TAKEN };
    public Slot typeOfSlot = Slot.DEFAULT;
    CardDragBeh1 cd1 = null;

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public void OnDrop(PointerEventData eventData)
    {
        CardDragBeh1 cd = eventData.pointerDrag.GetComponent<CardDragBeh1>();


        CardInfo ci = eventData.pointerDrag.GetComponent<CardInfo>();
        GameObject cardInSlot;

        switch (typeOfSlot)
        {
            #region Item Skills Empty
            case Slot.ITEM_SKILLS_EMPTY:
                
                if (cd1.TypeOfItem == CardDragBeh1.Card.ITEM_SKILLS)
                {
                    if (cd1 != null)
                    {
                        cd1.dropped = true;
                        cd.transformToAttach = this.transform;
                    }
                    else cd1.dropped = false;
                }

                break;
            #endregion

            #region Item Sword Empty
            case Slot.ITEM_SWORD_EMPTY:

                if (cd1.TypeOfItem == CardDragBeh1.Card.ITEM_SWORD)
                {
                    if (cd1 != null)
                    {
                        cd1.dropped = true;
                        cd.transformToAttach = this.transform;
                    }
                    else cd1.dropped = false;
                }

                break;
            #endregion

            #region Item Skills Taken
            case Slot.ITEM_SKILLS_TAKEN:   

                

                break;
            #endregion

            #region Item Sword Taken
            case Slot.ITEM_SWORD_TAKEN:



                break;
            #endregion

            #region Skill Empty - done
            case Slot.SKILL_EMPTY:

                if (cd.TypeOfItem == CardDragBeh1.Card.MAGIC)                                                                           // Checking if card can be placed in this slot.
                {
                    if (eventData.pointerDrag.GetComponent<CardDragBeh1>().draggedCardCopy != null)
                        cd1 = eventData.pointerDrag.GetComponent<CardDragBeh1>().draggedCardCopy.GetComponent<CardDragBeh1>();          // Getting dragged card, which is copy of card in inventory.
                    else
                        cd1 = null;

                    if (cd1 != null)
                    {
                        cd1.dropped = true;                                                                                             // Sending information to CardDragBeh1, that card was dropped in slot
                        cd.transformToAttach = this.transform;                                                                          // Sending information to CardDragBeh1, about new transform for dropped card.
                        typeOfSlot = Slot.SKILL_TAKEN;                                                                                  // Chenging slot type, to taken by card.

                        GM.Instance.cardByIdInSlots[ci.id]++;                                                                           // Updating card copies in all slots after droping new card to slot.
                        if (GM.Instance.cardByIdLimits[ci.id] ==                                                                        // Checking if limit for this card in slots is reached -
                            GM.Instance.cardByIdInSlots[ci.id])                                                                         // - .
                        {
                            cd.GetComponent<CanvasGroup>().blocksRaycasts = false;                                                      // Mouse click on card will be ignored.
                            cd.cardFrame.SetActive(true);                                                                               // Turning on graphic effect of disabled card. 
                        }
                    }
                    else                                                                                                                // Unnecesery, but i had to add this! Dont judge me.
                    {
                        cd.dropped = true;
                        cd.transformToAttach = this.transform;
                        typeOfSlot = Slot.SKILL_TAKEN;

                        GM.Instance.cardByIdInSlots[ci.id]++;
                        if (GM.Instance.cardByIdLimits[ci.id] ==                                                                        // Checking if limit for this card in slots is reached -
                            GM.Instance.cardByIdInSlots[ci.id])                                                                         // - .
                        {
                            eventData.pointerDrag.GetComponent<CardDragBeh1>().motherCard.GetComponent<CanvasGroup>().blocksRaycasts    // Mouse click on card will be ignored -
                                = false;                                                                                                // - .
                            eventData.pointerDrag.GetComponent<CardDragBeh1>().                                                         // Turning on graphic effect of disabled card -
                                motherCard.GetComponent<CardDragBeh1>().cardFrame.SetActive(true);                                      // - . 
                        }
                    }
                }

                break;
            #endregion

            #region Skill Taken - done
            case Slot.SKILL_TAKEN:

                if (this.transform.childCount > 0)                                                                                      // If this slot has card, then assign this card to cardInSlot variable -
                    cardInSlot = this.transform.GetChild(0).gameObject;                                                                 // - if not, then clear cardInSlot variable.
                else
                    cardInSlot = null;

                if (cd.itemState == CardDragBeh1.CardState.EQUIPMENT)                                                                   // Checking if dropped card is dragged from inventory (or from slots).
                {

                    if (cd.TypeOfItem == CardDragBeh1.Card.MAGIC)                                                                       // Checking if this particular slot, can take this particular card type. 
                    {
                        #region Clearing card amount from card limiter.
                        if (cardInSlot != null)
                        {
                            if (GM.Instance.cardByIdLimits[cardInSlot.GetComponent<CardInfo>().id] ==                                   // Checking if limit for this card in slots is reached -
                            GM.Instance.cardByIdInSlots[cardInSlot.GetComponent<CardInfo>().id])                                        // - .
                            {
                                cardInSlot.GetComponent<CardDragBeh1>().GetComponent<CanvasGroup>().blocksRaycasts = true;              // Card will react on mouse clicks.
                                cardInSlot.GetComponent<CardDragBeh1>().cardFrame.SetActive(false);                                     // Turning off graphic effect of disabled card.
                            }
                            GM.Instance.cardByIdInSlots[cardInSlot.GetComponent<CardInfo>().id]--;                                      // Updating card copies in all slots after dopping new card to slot. 
                        }
                        #endregion

                        cd1 = eventData.pointerDrag.GetComponent<CardDragBeh1>().draggedCardCopy.GetComponent<CardDragBeh1>();          // Getting dragged card, which is copy of card in inventory.

                        GM.Instance.cardByIdInSlots[eventData.pointerDrag.GetComponent<CardInfo>().id]++;                               // Updating card copies in all slots after droping new card to slot.
                        if (GM.Instance.cardByIdLimits[eventData.pointerDrag.GetComponent<CardInfo>().id] ==                            // Checking if limit for this card in slots is reached -
                            GM.Instance.cardByIdInSlots[eventData.pointerDrag.GetComponent<CardInfo>().id])                             // - .
                        {
                            cd.GetComponent<CanvasGroup>().blocksRaycasts = false;                                                      // Mouse click on card will be ignored.
                            cd.cardFrame.SetActive(true);                                                                               // Turning on graphic effect of disabled card. 
                        }

                        Destroy(cardInSlot);                                                                                            // Destroying card currently in slot, to make place for dropped card. 
                        cd1.dropped = true;                                                                                             // Sending info to CardDrogBeh1.cs, that card was dropped.
                        cd.transformToAttach = this.transform;                                                                          // Giving CardDragBeh1.cs script new transform for dropped card. 
                    }

                }
                else                                                                                                                    // Else: Drapped card is dragged from slot (CardDragBeh1.CardState.SLOT).
                {
                    if (cd.TypeOfItem == CardDragBeh1.Card.MAGIC)                                                                       // Checking if this particular slot, can take this particular card type.
                    {
                        cd.dropped = true;                                                                                              // Sending info to CardDrogBeh1.cs, that card was dropped.
                        cardInSlot.transform.SetParent(cd.transformToAttach);                                                           // Setting new parent for card from this slot.
                        cd.transformToAttach.gameObject.GetComponent<DropZone>().typeOfSlot = DropZone.Slot.SKILL_TAKEN;
                        cardInSlot.transform.position = cd.transformToAttach.position;                                                  // Setting position for card from this slot.
                        cd.transformToAttach = this.transform;                                                                          // Giving CardDragBeh1.cs script new transform for dropped card.
                    }
                }

                break;
            #endregion

            #region Sword Empty
            case Slot.SWORD_EMPTY:

                if (cd.TypeOfItem == CardDragBeh1.Card.SWORD)                                                                           // Checking if card can be placed in this slot.
                {
                    if (eventData.pointerDrag.GetComponent<CardDragBeh1>().draggedCardCopy != null)
                        cd1 = eventData.pointerDrag.GetComponent<CardDragBeh1>().draggedCardCopy.GetComponent<CardDragBeh1>();          // Getting dragged card, which is copy of card in inventory.
                    else
                        cd1 = null;

                    if (cd1 != null)
                    {
                        typeOfSlot = Slot.SWORD_TAKEN;                                                                                  // Chenging slot type, to taken by card.
                        cd1.dropped = true;                                                                                             // Sending information to CardDragBeh1, that card was dropped in slot
                        cd.transformToAttach = this.transform;                                                                          // Sending information to CardDragBeh1, about new transform for dropped card.
                        
                        GM.Instance.cardByIdInSlots[ci.id]++;                                                                           // Updating card copies in all slots after droping new card to slot.
                        if (GM.Instance.cardByIdLimits[ci.id] ==                                                                        // Checking if limit for this card in slots is reached -
                            GM.Instance.cardByIdInSlots[ci.id])                                                                         // - .
                        {
                            cd.GetComponent<CanvasGroup>().blocksRaycasts = false;                                                      // Mouse click on card will be ignored.
                            cd.cardFrame.SetActive(true);                                                                               // Turning on graphic effect of disabled card. 
                        }
                    }
                    else                                                                                                                // Unnecesery, but i had to add this! Dont judge me.
                    {
                        typeOfSlot = Slot.SWORD_TAKEN;
                        cd.dropped = true;
                        cd.transformToAttach = this.transform;

                        GM.Instance.cardByIdInSlots[ci.id]++;
                        if (GM.Instance.cardByIdLimits[ci.id] ==                                                                        // Checking if limit for this card in slots is reached -
                            GM.Instance.cardByIdInSlots[ci.id])                                                                         // - .
                        {
                            eventData.pointerDrag.GetComponent<CardDragBeh1>().motherCard.GetComponent<CanvasGroup>().blocksRaycasts    // Mouse click on card will be ignored -
                                = false;                                                                                                // - .
                            eventData.pointerDrag.GetComponent<CardDragBeh1>().                                                         // Turning on graphic effect of disabled card -
                                motherCard.GetComponent<CardDragBeh1>().cardFrame.SetActive(true);                                      // - . 
                        }
                    }
                }

                    break;
            #endregion

            #region Sword Taken - done
            case Slot.SWORD_TAKEN:

                #region Assigning value for CardInSlot variable
                if (this.transform.childCount > 0)                                                                                      // If this slot has card, then assign this card to cardInSlot variable -
                    cardInSlot = this.transform.GetChild(0).gameObject;                                                                 // - if not, then clear cardInSlot variable.
                else
                    cardInSlot = null;
                #endregion

                if (cd.itemState == CardDragBeh1.CardState.EQUIPMENT)                                                                   // Checking if dropped card is dragged from inventory (or from slots).
                {

                    if (cd.TypeOfItem == CardDragBeh1.Card.SWORD)                                                                       // Checking if this particular slot, can take this particular card type. 
                    {
                        #region Clearing card amount from card limiter.
                        if(cardInSlot != null)
                        {
                            if (GM.Instance.cardByIdLimits[cardInSlot.GetComponent<CardInfo>().id] ==                                   // Checking if limit for this card in slots is reached -
                            GM.Instance.cardByIdInSlots[cardInSlot.GetComponent<CardInfo>().id])                                        // - .
                            {
                                cardInSlot.GetComponent<CardDragBeh1>().GetComponent<CanvasGroup>().blocksRaycasts = true;              // Card will react on mouse clicks.
                                cardInSlot.GetComponent<CardDragBeh1>().cardFrame.SetActive(false);                                     // Turning off graphic effect of disabled card.
                            }
                            GM.Instance.cardByIdInSlots[cardInSlot.GetComponent<CardInfo>().id]--;                                      // Updating card copies in all slots after dopping new card to slot. 
                        }
                        #endregion

                        cd1 = eventData.pointerDrag.GetComponent<CardDragBeh1>().draggedCardCopy.GetComponent<CardDragBeh1>();          // Getting dragged card, which is copy of card in inventory.

                        GM.Instance.cardByIdInSlots[eventData.pointerDrag.GetComponent<CardInfo>().id]++;                               // Updating card copies in all slots after droping new card to slot.
                        if (GM.Instance.cardByIdLimits[eventData.pointerDrag.GetComponent<CardInfo>().id] ==                            // Checking if limit for this card in slots is reached -
                            GM.Instance.cardByIdInSlots[eventData.pointerDrag.GetComponent<CardInfo>().id])                             // - .
                        {
                            cd.GetComponent<CanvasGroup>().blocksRaycasts = false;                                                      // Mouse click on card will be ignored.
                            cd.cardFrame.SetActive(true);                                                                               // Turning on graphic effect of disabled card. 
                        }
                        Destroy(cardInSlot);                                                                                            // Destroying card currently in slot, to make place for dropped card. 
                        cd1.dropped = true;                                                                                             // Sending info to CardDrogBeh1.cs, that card was dropped.
                        cd.transformToAttach = this.transform;                                                                          // Giving CardDragBeh1.cs script new transform for dropped card. 
                    }

                }
                else                                                                                                                    // Else: Drapped card is dragged from slot (CardDragBeh1.CardState.SLOT).
                {
                    if (cd.TypeOfItem == CardDragBeh1.Card.SWORD)                                                                       // Checking if this particular slot, can take this particular card type.
                    {
                        cd.dropped = true;                                                                                              // Sending info to CardDrogBeh1.cs, that card was dropped.
                        cd.transformToAttach.gameObject.GetComponent<DropZone>().typeOfSlot = DropZone.Slot.SWORD_TAKEN;
                        cardInSlot.transform.SetParent(cd.transformToAttach);                                                           // Setting new parent for card from this slot.
                        cardInSlot.transform.position = cd.transformToAttach.position;                                                  // Setting position for card from this slot.
                        cd.transformToAttach = this.transform;                                                                          // Giving CardDragBeh1.cs script new transform for dropped card.

                    }
                }

                break;
                #endregion
        }

    }
}
