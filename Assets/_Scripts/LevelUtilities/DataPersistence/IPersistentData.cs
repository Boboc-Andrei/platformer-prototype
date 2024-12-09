public interface IPersistentData {
    public void LoadPersistentData(GameData data);
    public void SavePersistentData(ref GameData data);
    public void SetDefaultPersistentData(ref GameData data);
}