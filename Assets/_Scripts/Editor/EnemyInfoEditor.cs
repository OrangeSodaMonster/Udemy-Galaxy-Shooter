using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using Codice.CM.Client.Differences.Graphic;
using System.Collections.Generic;

public class EnemyInfoEditor : OdinMenuEditorWindow
{
    [MenuItem("Tools/Enemy Data")]
    private static void OpenWindow()
    {
        GetWindow<EnemyInfoEditor>().Show();
    }

    private CreateNewEnemyData createNewEnemyData;

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (createNewEnemyData != null)
            DestroyImmediate(createNewEnemyData.enemyData);
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();
        tree.Selection.SupportsMultiSelect = false;

        createNewEnemyData = new CreateNewEnemyData();

        tree.Add("Create New", createNewEnemyData);
        tree.AddAllAssetsAtPath("OutOfFolder", "Assets/_Prefabs/Enemies/_InfoSOs/", typeof(EnemyInfoSO));
        tree.AddAllAssetsAtPath("Greens", "Assets/_Prefabs/Enemies/_InfoSOs/Green/", typeof(EnemyInfoSO));
        tree.AddAllAssetsAtPath("Yellows", "Assets/_Prefabs/Enemies/_InfoSOs/Yellow/", typeof(EnemyInfoSO));
        tree.AddAllAssetsAtPath("Orange", "Assets/_Prefabs/Enemies/_InfoSOs/Orange/", typeof(EnemyInfoSO));
        tree.AddAllAssetsAtPath("Red", "Assets/_Prefabs/Enemies/_InfoSOs/Red/", typeof(EnemyInfoSO));
        
        return tree;
    }

    public class CreateNewEnemyData
    {
        public CreateNewEnemyData()
        {
            enemyData = ScriptableObject.CreateInstance<EnemyInfoSO>();
            enemyData.Name = "New Enemy";
        }

        [InlineEditor(Expanded = true)]
        public EnemyInfoSO enemyData;

        [Button("Add New Enemy SO")]
        private void CreateNewData()
        {
            string color = "";
            if (enemyData.EnemyColor == EnemyColor.Green) color = "Green/";
            if (enemyData.EnemyColor == EnemyColor.Yellow) color = "Yellow/";
            if (enemyData.EnemyColor == EnemyColor.Orange) color = "Orange/";
            if (enemyData.EnemyColor == EnemyColor.Red) color = "Red/";              

            AssetDatabase.CreateAsset(enemyData, "Assets/_Prefabs/Enemies/_InfoSOs/" + color + enemyData.Name + ".asset");
            AssetDatabase.SaveAssets();
        }
    }

    //protected override void OnBeginDrawEditors()
    //{
    //    OdinMenuTreeSelection selected = this.MenuTree.Selection;

    //    SirenixEditorGUI.BeginHorizontalToolbar();
    //    {
    //        GUILayout.FlexibleSpace();

    //        if (SirenixEditorGUI.ToolbarButton("Delete Current"))
    //        {
    //            EnemyInfoSO asset = selected.SelectedValue as EnemyInfoSO;
    //            string path = AssetDatabase.GetAssetPath(asset);
    //            AssetDatabase.DeleteAsset(path);
    //            AssetDatabase.SaveAssets();
    //        }
    //    }
    //    SirenixEditorGUI.EndHorizontalToolbar();
    //}
}