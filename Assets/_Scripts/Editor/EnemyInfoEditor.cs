using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using Codice.CM.Client.Differences.Graphic;

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
        tree.AddAllAssetsAtPath("Enemy Info", "Assets/_Prefabs/Enemies/_InfoSOs/", typeof(EnemyInfoSO));
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
            AssetDatabase.CreateAsset(enemyData, "Assets/_Prefabs/Enemies/_InfoSOs/" + enemyData.Name + ".asset");
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