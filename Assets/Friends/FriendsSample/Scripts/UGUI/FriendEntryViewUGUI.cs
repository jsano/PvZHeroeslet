using System.Collections;
using TMPro;
using Unity.Services.Friends.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Unity.Services.Samples.Friends.UGUI
{
    public class FriendEntryViewUGUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI m_NameText = null;
        [SerializeField] TextMeshProUGUI m_ActivityText = null;
        [SerializeField] Image m_PresenceColorImage = null;

        public Button removeFriendButton = null;
        public Button blockFriendButton = null;
        public Button inviteFriendButton = null;

        private string ID;

        public void Init(string playerName, Availability presenceAvailabilityOptions, string activity, string ID)
        {
            m_NameText.text = playerName;
            var index = (int)presenceAvailabilityOptions - 1;
            var presenceColor = ColorUtils.GetPresenceColor(index);
            m_PresenceColorImage.color = presenceColor;
            m_ActivityText.text = activity;
            this.ID = ID;
            if (SessionManager.Instance.ActiveSession != null) inviteFriendButton.interactable = true;
        }

        void Start()
        {
            GetComponentInChildren<ProfileThumbnail>().LoadProfileThumbnail(ID);
        }
    }
}