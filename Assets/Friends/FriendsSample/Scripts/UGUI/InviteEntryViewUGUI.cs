using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.Services.Samples.Friends.UGUI
{
    public class InviteEntryViewUGUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_NameText = null;

        public Button acceptButton = null;
        public Button declineButton = null;
        public Button blockButton = null;

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