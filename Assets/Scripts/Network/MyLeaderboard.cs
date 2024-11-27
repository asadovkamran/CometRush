using System.Collections.Generic;
using UnityEngine;
using TMPro;

using Dan.Main;

namespace LeaderboardCreatorDemo
{
    public class LeaderboardManager : MonoBehaviour
    {
        [SerializeField] private GameObject _leaderboardItemPrefab;
        [SerializeField] private GameObject _leaderboardItemListContainer;
        [SerializeField] private TMP_InputField _usernameInputField;

        [SerializeField] private GameObject _loadingText;

        [SerializeField] private GameStatsSO _gameStatsSO;

        private List<GameObject> _entryGameObjects = new List<GameObject>();
        private int Score => _gameStatsSO.HiScore;

        private string _publicKey = "e206398ee812d421778e1d3ac6cfe799d210010274d9d0f861cffb2fb2a7fe09";
        private void Start()
        {
            LoadEntries();
        }

        private void OnEnable()
        {
            LeaderboardCreator.GetPersonalEntry(_publicKey, entry =>
            {
                _usernameInputField.text = entry.Username;
            });
        }

        private void LoadEntries()
        {
            Leaderboards.CometRush.GetEntries(entries =>
            {
                _loadingText.SetActive(false);
                foreach (var entry in entries)
                {

                    GameObject newEntry = Instantiate(_leaderboardItemPrefab, _leaderboardItemListContainer.transform);
                    _entryGameObjects.Add(newEntry);

                    newEntry.GetComponent<LeaderboardItem>().RankText.text = entry.RankSuffix();
                    newEntry.GetComponent<LeaderboardItem>().NameText.text = entry.Username;
                    newEntry.GetComponent<LeaderboardItem>().ScoreText.text = entry.Score.ToString();
                }
            });
        }

        public void UploadEntry()
        {
            ClearEntries();
            Leaderboards.CometRush.UploadNewEntry(_usernameInputField.text, Score, isSuccessful =>
            {
                if (isSuccessful)
                    LoadEntries();
            });
        }

        private void ClearEntries()
        {
            foreach (var entryObject in _entryGameObjects)
            {
                Destroy(entryObject);
            }

            _entryGameObjects.Clear();
        }
    }
}