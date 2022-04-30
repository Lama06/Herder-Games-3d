namespace HerderGames.Util
{
    public interface PersistentDataContainer
    {
        public void LoadData();

        public void SaveData();

        public void ResetData();
    }
}
