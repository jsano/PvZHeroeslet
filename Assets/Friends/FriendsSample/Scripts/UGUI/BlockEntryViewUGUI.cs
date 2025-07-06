using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.Services.Samples.Friends.UGUI
{
    public class BlockEntryViewUGUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI m_NameText = null;

        public Button unblockButton = null;

        private string ID;

        public void Init(string playerName, string ID)
        {
            m_NameText.text = playerName;
            this.ID = ID;
        }

        void Start()
        {
            GetComponentInChildren<ProfileThumbnail>().LoadProfileThumbnail(ID);
        }
    }
}