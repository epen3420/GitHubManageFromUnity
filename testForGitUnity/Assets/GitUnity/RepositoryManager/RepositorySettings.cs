namespace GitUnity.Repository
{
    [System.Serializable]
    public class RepositorySettings
    {
        public string name;
        public string description;
        public string gitignore_template = "Unity";
        public bool @private;
    }
}
