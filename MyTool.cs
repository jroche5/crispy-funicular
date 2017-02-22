using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MyTool : EditorWindow
{

    /*int myInt;
    bool myToggle;

    bool myFold;

    float minValue = 1;
    float maxValue = 100;
    float selectedMin = 1;
    float selectedMax = 10;*/

    public List<Enemies> enemyList = new List<Enemies>();
    int index = 0;

    string nameString;

    bool nameFlag = false;
    bool spriteFlag = false;

    int lastChoice = 0;

    string[] enemyNames;

    int myHealth = 1;
    int myAttack = 1;
    int myDefense = 1;
    int myAgility = 1;

    float myAtkTime = 1;

    bool magicUser = false;

    int mana = 0;

    Sprite mySprite = new Sprite();

    [MenuItem("Custom Tools/ Character Creation %l")]
    private static void showMyWindow()
    {
        EditorWindow.GetWindow<MyTool>(true, "Character Creation");
    }

    void Awake()
    {
        getEnemies();
    }

    private void getEnemies()
    {
        enemyList.Clear();
        string[] guids = AssetDatabase.FindAssets("t:Enemies",null);
        foreach (string guid in guids)
        {
            string myString = AssetDatabase.GUIDToAssetPath(guid);

            Enemies enemyInst = AssetDatabase.LoadAssetAtPath(myString, typeof(Enemies)) as Enemies;

            enemyList.Add(enemyInst);
        }
        List<string> enemyNameList = new List<string>();
        foreach(Enemies e in enemyList)
        {
            enemyNameList.Add(e.emname);
        }
        enemyNameList.Insert(0, "New");
        enemyNames = enemyNameList.ToArray();
    }


    private void OnGUI()
    {
        index = EditorGUILayout.Popup(index, enemyNames);

        //index = EditorGUILayout.Popup(index, enemyList);
        
        nameString = EditorGUILayout.TextField("Name", nameString);
        if (nameFlag)
        {
            EditorGUILayout.HelpBox("The name cannot be blank!", MessageType.Error);
        }

        mySprite = (Sprite)EditorGUILayout.ObjectField("Sprite", mySprite, typeof(Sprite), allowSceneObjects: true);
        if(spriteFlag)
        {
            EditorGUILayout.HelpBox("There needs to be a set sprite!", MessageType.Error);
        }

        EditorGUILayout.LabelField("Health");
        myHealth = EditorGUILayout.IntSlider(myHealth, 1, 300);

        EditorGUILayout.LabelField("Attack");
        myAttack = EditorGUILayout.IntSlider(myAttack, 1, 100);

        EditorGUILayout.LabelField("Defense");
        myDefense = EditorGUILayout.IntSlider(myDefense, 1, 100);

        EditorGUILayout.LabelField("Agility");
        myAgility = EditorGUILayout.IntSlider(myAgility, 1, 100);

        EditorGUILayout.LabelField("Attack Time");
        myAtkTime = EditorGUILayout.FloatField(myAtkTime);

        if(myAtkTime < 1.0f) { myAtkTime = 1.0f; }
        if(myAtkTime > 20.0f) { myAtkTime = 20.0f; }

        magicUser = EditorGUILayout.BeginToggleGroup("Magic User", magicUser);

        EditorGUILayout.LabelField("Mana");
        mana = EditorGUILayout.IntSlider(mana, 0, 100);

        EditorGUILayout.EndToggleGroup();

        //here


        if (index == 0)
        {
            if (GUILayout.Button("Create"))
            {
                createEnemy();
            }
        }
        else
        {
            if (GUILayout.Button("Save"))
            {
                alterEnemy();
            }
        }
        if(index != lastChoice)
        {
            if(index == 0)
            {
                //blank out fields for new enemy
                nameString = "";
                myHealth = 1;
                myAttack = 1;
                myDefense = 1;
                myAgility = 1;
                myAtkTime = 1.0f;
                magicUser = false;
                mana = 0;
                mySprite = null;
            }
            else
            {
                //Load fields with enemy data
                nameString = enemyList[index - 1].emname;
                myHealth = enemyList[index - 1].health;
                myAttack = enemyList[index - 1].atk;
                myDefense = enemyList[index - 1].def;
                myAgility = enemyList[index - 1].agi;
                myAtkTime = enemyList[index - 1].atkTime;
                magicUser = enemyList[index - 1].isMagic;
                mana = enemyList[index - 1].manaPool;
                mySprite = enemyList[index - 1].mySprite;
            }
            lastChoice = index;
        }
    }

    private void createEnemy()
    {
        if(nameString == null || nameString == "" || mySprite == null || nameFlag || spriteFlag)
        {
            Debug.Log(nameString);
            if (nameString == null || nameString == "")
            {
                nameFlag = true;
            }
            else
            {
                nameFlag = false;
            }

            if (mySprite == null)
            {
                spriteFlag = true;
            }
            else
            {
                spriteFlag = false;
            }
            return;
        }
        else
        {
            Enemies myEnemy = ScriptableObject.CreateInstance<Enemies>();
            myEnemy.emname = nameString;
            myEnemy.health = myHealth;
            myEnemy.atk = myAttack;
            myEnemy.def = myDefense;
            myEnemy.agi = myAgility;
            myEnemy.atkTime = myAtkTime;
            myEnemy.isMagic = magicUser;
            myEnemy.manaPool = mana;
            myEnemy.mySprite = mySprite;
            AssetDatabase.CreateAsset(myEnemy, "Assets/Resources/Data/EnemyData/" + myEnemy.emname.Replace(" ","_") + ".asset");
            nameFlag = false;
            spriteFlag = false;
            getEnemies();

            nameString = "";
            myHealth = 1;
            myAttack = 1;
            myDefense = 1;
            myAgility = 1;
            myAtkTime = 1.0f;
            magicUser = false;
            mana = 0;
            mySprite = null;
        }
    }

    private void alterEnemy()
    {
        enemyList[index - 1].emname = nameString;
        enemyList[index - 1].health = myHealth;
        enemyList[index - 1].atk = myAttack;
        enemyList[index - 1].def = myDefense;
        enemyList[index - 1].agi = myAgility;
        enemyList[index - 1].atkTime = myAtkTime;
        enemyList[index - 1].isMagic = magicUser;
        enemyList[index - 1].manaPool = mana;
        enemyList[index - 1].mySprite = mySprite;

        AssetDatabase.SaveAssets();
    }
}

/*myToggle = EditorGUILayout.BeginToggleGroup("My Toggle", myToggle);
        EditorGUILayout.HelpBox("This is a help box", MessageType.Error);
        myInt = EditorGUILayout.IntField("My Int", myInt);
        if (myInt < 0)
        {
            myInt = 0;
        }
        EditorGUILayout.LabelField(myInt.ToString());
        EditorGUILayout.EndToggleGroup();
        myInt = EditorGUILayout.DelayedIntField("Delayed Int", myInt);

        myFold = EditorGUILayout.Foldout(myFold, "Fold out");
        if (myFold)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Min Value: " + minValue.ToString());
            EditorGUILayout.LabelField("Max Value: " + maxValue.ToString());
            EditorGUILayout.LabelField("Selected Min: " + selectedMin.ToString("F2"));
            EditorGUILayout.LabelField("Selected Max: " + selectedMax.ToString("F2"));
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.LabelField("My Slider");
        EditorGUILayout.MinMaxSlider(ref selectedMin, ref selectedMax, minValue, maxValue);
        */
