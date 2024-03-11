using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfilePageReinforcementTypeView : MonoBehaviour
{

    [SerializeField]
    private RectTransform[] reinforcementTypeCards;
    [SerializeField]
    private TMP_Text subheadingText;


    [SerializeField]
    private RawImage revenantProfilePic;
    [SerializeField]
    private TMP_Text revenantDataText;

    [SerializeField]
    private Vector2 standardSizeOfCards;
    [SerializeField]
    private Outpost savedOutpost;

    private void OnEnable()
    {
        savedOutpost = DojoEntitiesDataManager.outpostDictInstance[UiEntitiesReferenceManager.profilePageBehaviour.currentlySelectedOutpost.Value];

        Texture2D revImage = Resources.Load<Texture2D>($"Revenants_Pics/{RisingRevenantUtils.GetConsistentRandomNumber((int)(savedOutpost.position.x * savedOutpost.position.y), RisingRevenantUtils.FieldElementToInt(DojoEntitiesDataManager.currentGameId), 1, 24)}");
        revenantProfilePic.texture = revImage;

        revenantDataText.text = $"{RisingRevenantUtils.GetFullRevenantName(savedOutpost.position)}\nX:{savedOutpost.position.x} || Y:{savedOutpost.position.y}";

        subheadingText.text = $"Choose the best reinforcements for {RisingRevenantUtils.GetFullRevenantName(savedOutpost.position)}'s outpost and get ready against all Attack!";

        SetSizeOfCards();

        savedOutpost.OnValueChange += SetSizeOfCards;
    }

    private void OnDisable()
    {
        savedOutpost.OnValueChange -= SetSizeOfCards;
    }

    private void SetSizeOfCards()
    {
        for (int i = 0; i < reinforcementTypeCards.Length; i++)
        {
            if (i == (int)savedOutpost.reinforcementType)
            {
                reinforcementTypeCards[i].sizeDelta = new Vector2(standardSizeOfCards.x * 1.1f, standardSizeOfCards.y * 1.1f);
            }
            else
            {
                reinforcementTypeCards[i].sizeDelta = standardSizeOfCards;
            }
        }
    }

    public async void SetReinforcementType(int reinforcementType)
    {
        var dataStruct = new DojoCallsManager.SetReinforcementType
        {
            type = (RisingRevenantUtils.ReinforcementType)reinforcementType,
            gameId = DojoEntitiesDataManager.currentGameId,
            outpostId = UiEntitiesReferenceManager.profilePageBehaviour.currentlySelectedOutpost.Value
        };

        var endpoint = new DojoCallsManager.EndpointDojoCallStruct
        {
            account = DojoEntitiesDataManager.currentAccount,
            functionName = "set_reinforcement_type",
            addressOfSystem = DojoCallsManager.outpostActionsAddress
        };

        await DojoCallsManager.SetReinforcementTypeCall(dataStruct, endpoint);
    }
}
