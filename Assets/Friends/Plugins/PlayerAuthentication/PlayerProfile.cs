using UnityEngine;

namespace Unity.Services.Samples
{
    [System.Serializable]
    public class PlayerProfile
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public string Lobby { get; private set; }

        public PlayerProfile(string name, string id)
        {
            Name = name;
            Id = id;
        }

        public PlayerProfile(string name, string id, string lobby)
        {
            Name = name;
            Id = id;
            Lobby = lobby;
        }

        public override string ToString()
        {
            return $"{Name} , Id :{Id}";
        }
    }
}
