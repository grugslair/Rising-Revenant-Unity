public class GameStatsPageBehaviour : Menu
{
    public TableOfEntTypeDataPageBehaviour tableOfDataPageBehaviour { get; set; }
    public PlayerSpecifiDataPageBehaviour playerSpecifiDataPageBehaviour { get; set; }
    public WholeGameDataPageBehaviour wholeGameDataPageBehaviour { get; set; }

    private void Awake()
    {
        UiEntitiesReferenceManager.gameStatsPageBehaviour = this;
    }
}
