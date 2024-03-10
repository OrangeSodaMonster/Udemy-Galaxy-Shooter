using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using Codice.CM.Client.Differences.Graphic;

public class AsteroidInfoEditor : OdinMenuEditorWindow
{
    [MenuItem("Tools/Asteroid Data")]
    private static void OpenWindow()
    {
        GetWindow<AsteroidInfoEditor>().Show();
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
        tree.AddAllAssetsAtPath("Enemy Info", "Assets/_Prefabs/Asteroids/_InfoSOs/", typeof(EnemyInfoSO));
        return tree;
    }

    public class CreateNewEnemyData
    {
        public CreateNewEnemyData()
        {
            enemyData = ScriptableObject.CreateInstance<EnemyInfoSO>();
            enemyData.Name = "New Asteroid";
        }

        [InlineEditor(Expanded = true)]
        public EnemyInfoSO enemyData;

        [Button("Add New Enemy SO")]
        private void CreateNewData()
        {
            AssetDatabase.CreateAsset(enemyData, "Assets/_Prefabs/Asteroids/_InfoSOs/" + enemyData.Name + ".asset");
            AssetDatabase.SaveAssets();
        }
    }
}