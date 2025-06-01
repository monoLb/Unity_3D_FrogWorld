using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GamePanel : MonoBehaviour
{
    private VisualElement rootElement;
    private Label moneyAmountLabel,cheiflevelAmountLabel;

    private void OnEnable()
    {
        rootElement=GetComponent<UIDocument>().rootVisualElement;
        
        moneyAmountLabel = rootElement.Query<Label>("MoneyAmount");
        cheiflevelAmountLabel = rootElement.Query<Label>("ChiefLevelAmount");
    }


    public void UpdateMoneyAmount(int value)
    {
        moneyAmountLabel.text = value.ToString();
    }

    public void UpdateChiefLevelAmount(int value)
    {
        cheiflevelAmountLabel.text = value.ToString();
    }
}
