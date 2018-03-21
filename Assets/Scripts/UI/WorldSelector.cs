using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class WorldSelector : Singleton<WorldSelector>
{
    [SerializeField] BeatCreator m_creator = null;
    [SerializeField] GameObject m_playerControls = null;
    [SerializeField] GameObject m_levelIconTemplate = null;
    [SerializeField] GameObject m_levelRowTemplate = null;
    [SerializeField] Transform m_levelsLocation = null;

    Game m_game;
    GameObject m_currentRow = null;

    private void Start()
    {
        m_game = Game.Instance;

        UpdateList();
    }

    private void OnEnable()
    {
        m_game = Game.Instance;

        UpdateList();
    }

    void CreateLevelIcons()
    {
        string[] levels = Directory.GetDirectories(Application.dataPath + "/StreamingAssets/CustomLevels/");

        int rowCount = 0;
        foreach (string s in levels)
        {
            if (rowCount >= 3)
            {
                rowCount = 0;
                m_currentRow = AddNewRow();
            }

            string[] split = s.Split('/');
            string level = split[split.Length - 1];
            BeatTrack track = m_game.LoadObjectFromJson<BeatTrack>("Track", level);
            AddLevelIcon(track);

            rowCount++;
        }
    }

    void UpdateList()
    {
        ClearList();
        m_currentRow = AddNewRow();
        CreateLevelIcons();
        UpdateSelectorSize();
    }

    void ClearList()
    {
        Transform[] levels = m_levelsLocation.GetComponentsInChildren<Transform>();

        for (int i = levels.Length - 1; i >= 0; i--)
        {
            if (levels[i] != m_levelsLocation)
            {
                Destroy(levels[i].gameObject);
            }
        }

        m_levelsLocation.DetachChildren();
    }

    void UpdateSelectorSize()
    {
        for (int i = 0; i < m_levelsLocation.childCount - 2; i++)
        {
            m_levelsLocation.GetComponent<RectTransform>().sizeDelta += Vector2.up * 100.0f;
        }
    }

    GameObject AddNewRow()
    {
        GameObject row = Instantiate(m_levelRowTemplate, Vector3.zero, Quaternion.identity, m_levelsLocation);

        return row;
    }

    LevelIcon AddLevelIcon(BeatTrack track)
    {
        GameObject gameObject = Instantiate(m_levelIconTemplate, Vector3.zero, Quaternion.identity, m_currentRow.transform);
        LevelIcon icon = gameObject.GetComponent<LevelIcon>();
        icon.m_track = track;   
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = track.level;

        return icon;
    }

    public void StartTrack(BeatTrack track)
    {
        m_creator.LoadTrack(track);
        gameObject.SetActive(false);
        m_playerControls.SetActive(true);
    }
}
