using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unity.Services.Samples.Friends.UGUI
{
    public class InvitesViewUGUI : ListViewUGUI, IRequestListView
    {
        [SerializeField] RectTransform m_ParentTransform = null;
        [SerializeField] InviteEntryViewUGUI m_InviteEntryViewPrefab = null;

        List<InviteEntryViewUGUI> m_RequestEntries = new List<InviteEntryViewUGUI>();
        List<PlayerProfile> m_PlayerProfiles = new List<PlayerProfile>();
        public Action<string> onAccept { get; set; }
        public Action<string> onDecline { get; set; }
        public Action<string> onBlock { get; set; }

        public void BindList(List<PlayerProfile> playerProfiles)
        {
            m_PlayerProfiles = playerProfiles;
        }

        public override void Refresh()
        {
            m_RequestEntries.ForEach(entry => Destroy(entry.gameObject));
            m_RequestEntries.Clear();

            foreach (var playerProfile in m_PlayerProfiles)
            {
                var entry = Instantiate(m_InviteEntryViewPrefab, m_ParentTransform);
                entry.Init(playerProfile.Name, playerProfile.Id);
                entry.acceptButton.onClick.AddListener(() =>
                {
                    entry.gameObject.SetActive(false);
                    onAccept?.Invoke(playerProfile.Lobby);
                });
                entry.declineButton.onClick.AddListener(() =>
                {
                    entry.gameObject.SetActive(false);
                    onDecline?.Invoke(playerProfile.Id);
                });
                entry.blockButton.onClick.AddListener(() =>
                {
                    entry.gameObject.SetActive(false);
                    onBlock?.Invoke(playerProfile.Id);
                });
                m_RequestEntries.Add(entry);
            }
        }
    }
}
