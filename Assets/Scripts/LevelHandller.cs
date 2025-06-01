using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static LevelHandller;
using static Unity.VisualScripting.Metadata;
using Image = UnityEngine.UI.Image;

public class LevelHandller : MonoBehaviour, IExpGetter
{
    [System.Serializable]
    struct TreeFormat
    {
        public Data[] Data;
    }
    [System.Serializable]
    struct Data {
        public string SkillName;
        public string KeyString;
        public bool IsRoot;
        public string[] ExclusiveStrings;
        public Vector2 ViewPosition;
        public string[] ParentsKeyStrings;
        public string[] ChildrenKeyStrings;
    }
    
    struct SkillTree
    {
        public Data current;
        public bool isReach;
        public List<SkillTree> children;
    }
    
    int Max_Level;
    [SerializeField] int[] levelExpNeeded;
    [SerializeField] UIDocument chooseSkillUI;
    [SerializeField] public TextAsset treeData;
    [SerializeField] public List<MySkill> mySkills = new List<MySkill>();
    [SerializeField] public Image expUI;
    [SerializeField] private PlayerInput playerInput;


    [SerializeField] private CargoWeaponSTangent tangentSkill;
    [SerializeField] private CargoWeaponFiield fieldSkill;

    private List<VisualElement> skillCubes;
    SkillTree skillTree;
    VisualElement root;
    Dictionary<string, Data> skillDict;
    Dictionary<string, MySkill> mySkillDict;
    float currentExp = 0;
    int currentLevel = 0;

    private void Awake()
    {
        Max_Level = levelExpNeeded.Length-1;
        BuildTree();
        BuildMyskillDict();
    }

    void BuildMyskillDict()
    {
        mySkillDict = new Dictionary<string, MySkill>();
        foreach (MySkill skill in mySkills) {
            mySkillDict.Add(skill.skillName, skill);
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        List<Data> datas = GetChoices();
        root = chooseSkillUI.rootVisualElement;
        EnabledSkillWindow(datas);
        root.visible = false;

    }

    private List<Data> GetChoices()
    {
        List<Data> datas = new List<Data>();
        AddChoices(skillTree, ref datas);
        Shuffle<Data>(datas);
        return datas;
    }

    public static void Shuffle<T>(IList<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
    void OnSkillClicked(ClickEvent evt)
    {
        // 獲取按下的按鈕
        VisualElement clickedButton = (VisualElement)evt.target;

        // 獲取按鈕的資訊（例如，技能名稱）
        string skillName = clickedButton.Q<TextElement>(name = "skillName").text;
        if (mySkillDict.TryGetValue(skillName, out MySkill value))
            value.Activate();
        else
        {
            Debug.LogError($"skill:{skillName} not exisit");
        }
        //mySkillDict[skillName].Activate();

        //Debug.Log("click skill");
        root.visible = false;
        Time.timeScale = 1;
        
        //teardown
        var skillCubes = root.Query(classes: "SkillCube").ToList();
        foreach (var skill in skillCubes)
        {
            skill.UnregisterCallback<ClickEvent>(OnSkillClicked);
        }
        // 記得解除事件訂閱
        playerInput.actions["Square"].performed -= OnSquarePressed;
        playerInput.actions["Triangle"].performed -= OnTrianglePressed;
        playerInput.actions["Circle"].performed -= OnCirclePressed;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void Get(float exp)
    {
        currentExp += exp;
        if (expUI != null)
            expUI.DOFillAmount(currentExp / levelExpNeeded[currentLevel], .3f);
        while (currentExp >= levelExpNeeded[currentLevel])
        {
            currentExp -= levelExpNeeded[currentLevel];
            currentLevel = Mathf.Min(Max_Level, currentLevel+1);
            expUI.DOFillAmount(currentExp / levelExpNeeded[currentLevel], .1f).SetDelay(.3f).OnComplete(() => {
                OnLevelUp();
            });
            
        }
        
    }
    void OnLevelUp()
    {
        Time.timeScale = 0;
        List<Data> datas = GetChoices();
        EnabledSkillWindow(datas);

    }
    
    private void EnabledSkillWindow(IList<Data> datas)
    {
        playerInput.actions["Square"].performed += OnSquarePressed;
        playerInput.actions["Triangle"].performed += OnTrianglePressed;
        playerInput.actions["Circle"].performed += OnCirclePressed;

        chooseSkillUI.gameObject.SetActive(true);
        skillCubes = root.Query(classes: "SkillCube").ToList();
        int i = 0;
        foreach (var skill in skillCubes)
        {
            if (i >= datas.Count) break;
            skill.RegisterCallback<ClickEvent>(OnSkillClicked);
            skill.Q<TextElement>("skillName").text = datas[i].SkillName;
            skill.Q<TextElement>("des").text = datas[i].SkillName;
            i++;
            
        }
        root.visible = true;

        
    }

    private void OnSquarePressed(InputAction.CallbackContext context)
    {
        // 模擬對應 SkillCube 的點擊事件
        Debug.Log("OnSquarePressed");
        TriggerSkill(0); // 假設方塊對應第一個技能
    }

    private void OnTrianglePressed(InputAction.CallbackContext context)
    {
        // 模擬對應 SkillCube 的點擊事件
        Debug.Log("OnTrianglePressed");
        TriggerSkill(1); // 假設三角形對應第二個技能
    }

    private void OnCirclePressed(InputAction.CallbackContext context)
    {
        // 模擬對應 SkillCube 的點擊事件
        Debug.Log("OnCirclePressed");
        TriggerSkill(2); // 假設圓形對應第三個技能
    }

    // 手動觸發 SkillCube 的點擊事件
    private void TriggerSkill(int skillIndex)
    {
        if (skillIndex >= 0 && skillIndex < skillCubes.Count)
        {
            // 手動觸發點擊事件
            ClickEvent clickEvent = new ClickEvent();
            clickEvent.target = skillCubes[skillIndex];
            skillCubes[skillIndex].SendEvent(clickEvent);
        }
    }


    void AddChoices(SkillTree root,ref List<Data> choices)
    {
        if (root.isReach)
        {
            foreach(var child in root.children)
            {
                if (!child.isReach) 
                {
                    Debug.Log(child.current.SkillName);
                    choices.Add(child.current); 
                }
                else
                {
                    AddChoices(child, ref choices);
                }
            }
        }
    }
    private void BuildTree()
    {
        TreeFormat treeFormat = JsonUtility.FromJson<TreeFormat>(treeData.text);
        skillDict = new Dictionary<string, Data>();
        Data rootData = new Data();
        foreach (var data in treeFormat.Data)
        {
            if (data.IsRoot)
            {
                rootData = data;
            }
            skillDict.Add(data.SkillName, data);
        }
        skillTree = CreateTreeNode(rootData, skillDict);
        skillTree.isReach = true;
        Debug.Log($"root:{skillTree.current.SkillName}");
    }

    SkillTree CreateTreeNode(Data data, Dictionary<string, Data> dict)
    {
        SkillTree skillTree = new SkillTree();
        skillTree.current = data;
        skillTree.children = new List<SkillTree>();
        skillTree.isReach = false;
        foreach (var childKey in data.ChildrenKeyStrings)
        {
            Data childData = dict[childKey];
            skillTree.children.Add(CreateTreeNode(childData,dict));
        }
        return skillTree;
    }
}
