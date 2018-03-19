using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEditor;

public class WorldSelector : Singleton<WorldSelector>
{
    [SerializeField] BeatCreator m_creator = null;
    [SerializeField] GameObject m_playerControls = null;
    [SerializeField] GameObject m_levelIconTemplate = null;
    [SerializeField] GameObject m_levelRowTemplate = null;
    [SerializeField] Transform m_levelsLocation = null;

    GameObject m_currentRow = null;
    string m_generalPath = "Assets\\Resources\\CustomLevels\\";
    string m_generalPathSuffix = "\\Track.asset";

    private void Start()
    {
        m_currentRow = AddNewRow();
        CreateLevelIcons();
    }

    void CreateLevelIcons()
    {
        string[] levels = AssetDatabase.GetSubFolders("Assets\\Resources\\CustomLevels");

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
            string fullPath = m_generalPath + level + m_generalPathSuffix;
            BeatTrack track = AssetDatabase.LoadAssetAtPath<BeatTrack>(fullPath);
            AddLevelIcon(track);

            rowCount++;
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
