using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.IO;
using TMPro;

public class LevelSearch : Singleton<WorldSelector>
{
    public enum SearchType
    {
        CUSTOM,
        DEFAULT,
        BOTH
    }

    [SerializeField] BeatPlayer m_beatPlayer = null;
    [SerializeField] GameObject m_playerHUD = null;
    [SerializeField] GameObject m_levelIconTemplate = null;
    [SerializeField] Transform m_levelsLocation = null;
    [SerializeField] Toggle m_toggleCustom = null;
    [SerializeField] Toggle m_toggleDefault = null;
    [SerializeField] Toggle m_toggleBoth = null;

    Game m_game;
    List<GameObject> m_iconList;
    string m_generalPathSuffix = "\\Track.asset";
    SearchType m_searchType;

    private void Start()
    {
        m_iconList = new List<GameObject>();
        m_game = Game.Instance;

        UpdateSearchType();

        CreateLevelIcons();

        RectTransform trans = m_levelsLocation.GetComponent<RectTransform>();
        Vector2 pos = trans.position;
        pos.y = 0.0f;
        trans.position = pos;
    }

    void CreateLevelIcons()
    {
        switch (m_searchType)
        {
            case SearchType.CUSTOM:
                CreateCustomLevelIcons();
                break;
            case SearchType.DEFAULT:
                CreateDefaultLevelIcons();
                break;
            case SearchType.BOTH:
                CreateBothLevelIcons();
                break;
            default:
                break;
        }
    }

    void CreateCustomLevelIcons()
    {
        string[] levels = Directory.GetDirectories(Application.dataPath + "/StreamingAssets/CustomLevels/");
        //string[] levels = AssetDatabase.GetSubFolders("Assets\\Resources\\CustomLevels");
        //string prefix = "Assets\\Resources\\CustomLevels\\";
        string prefix = Application.dataPath + "/StreamingAssets/CustomLevels/";
        CreateIconsFromPaths(levels, prefix);
    }

    void CreateDefaultLevelIcons()
    {
        string[] levels = Directory.GetDirectories(Application.dataPath + "/StreamingAssets/PrebuiltLevels/");
        string prefix = Application.dataPath + "/StreamingAssets/PrebuiltLevels/";
        CreateIconsFromPaths(levels, prefix);
    }

    void CreateBothLevelIcons()
    {
        string[] levels = Directory.GetDirectories(Application.dataPath + "/StreamingAssets/CustomLevels/");
        string prefix = Application.dataPath + "/StreamingAssets/CustomLevels/";
        CreateIconsFromPaths(levels, prefix);

        string[] levels2 = Directory.GetDirectories(Application.dataPath + "/StreamingAssets/PrebuiltLevels/");
        string prefix2 = Application.dataPath + "/StreamingAssets/PrebuiltLevels/";
        CreateIconsFromPaths(levels2, prefix2);
    }

    void CreateIconsFromPaths(string[] paths, string pathPrefix)
    {
        foreach (string s in paths)
        {
            string[] split = s.Split('/');
            string level = split[split.Length - 1];
            string fullPath = pathPrefix + level + m_generalPathSuffix;
            //BeatTrack track = AssetDatabase.LoadAssetAtPath<BeatTrack>(fullPath);
            //print(level);
            BeatTrack track = m_game.LoadObjectFromJson<BeatTrack>("Track", level);
            AddLevelIcon(track);
        }
    }

    LevelIcon AddLevelIcon(BeatTrack track)
    {
        GameObject gameObject = Instantiate(m_levelIconTemplate, Vector3.zero, Quaternion.identity, m_levelsLocation);
        LevelIcon icon = gameObject.GetComponent<LevelIcon>();
        icon.m_track = track;
        gameObject.GetComponentInChildren<TextMeshProUGUI>().text = track.level;

        if (m_levelsLocation.childCount > 3) m_levelsLocation.GetComponent<RectTransform>().sizeDelta += Vector2.right * 200.0f;

        m_iconList.Add(gameObject);

        return icon;
    }

    void ClearCurrentList()
    {
        for (int i = m_iconList.Count - 1; i >= 0; i--)
        {
            Destroy(m_iconList[i]);
        }

        m_iconList.Clear();
        m_levelsLocation.DetachChildren();
    }

    public void StartTrack(BeatTrack track)
    {
        //m_player.LoadTrack(track);
        gameObject.SetActive(false);
        m_playerHUD.SetActive(true);
    }

    private void UpdateSearchType()
    {
        if (m_toggleCustom.isOn) m_searchType = SearchType.CUSTOM;
        else if (m_toggleDefault.isOn) m_searchType = SearchType.DEFAULT;
        else m_searchType = SearchType.BOTH;
        //print(m_searchType);
        m_levelsLocation.GetComponent<RectTransform>().sizeDelta = Vector2.up * 133.0f;

        ClearCurrentList();
    }

    public void OnToggleChange()
    {
        UpdateSearchType();
        CreateLevelIcons();
    }

    public void Randomize()
    {
        m_levelsLocation.DetachChildren();
        RandomizeList();
        ReAddIcons();
    }

    private void RandomizeList(int randomnessValue = 10)
    {
        for (int j = 0; j < randomnessValue; j++)
        {
            for (int i = 0; i < m_iconList.Count; i++)
            {
                int x = Random.Range(0, m_iconList.Count - 1);
                GameObject temp = m_iconList[i];
                m_iconList[i] = m_iconList[x];
                m_iconList[x] = temp;
            }
        }
    }

    private void ReAddIcons()
    {
        foreach (GameObject icon in m_iconList)
        {
            icon.transform.SetParent(m_levelsLocation);
        }
    }

    public void LaunchPlaylist()
    {
        m_beatPlayer.LoadPlaylist(m_iconList);
    }
}
