using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using Codice.CM.Client.Differences.Graphic;

public class ObjInfoEditor : OdinMenuEditorWindow
{
    [MenuItem("Tools/Objective Data")]
    private static void OpenWindow()
    {
        GetWindow<ObjInfoEditor>().Show();
    }

    private CreateNewEnemyData createNewEnemyData;

    protected override void OnDestroy()
    {
        base.OnDestroy();

        if (createNewEnemyData != null)
            DestroyImmediate(createNewEnemyData.objectiveData);
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();
        tree.Selection.SupportsMultiSelect = false;

        createNewEnemyData = new CreateNewEnemyData();

        tree.Add("Create New", createNewEnemyData);
        tree.AddAllAssetsAtPath("OutOfFolder", "Assets/_Prefabs/Objectives/_InfoSOs/", typeof(ObjectiveInfoSO));        
        tree.AddAllAssetsAtPath("Rare", "Assets/_Prefabs/Objectives/_InfoSOs/Rare/", typeof(ObjectiveInfoSO));
        tree.AddAllAssetsAtPath("RareBlue", "Assets/_Prefabs/Objectives/_InfoSOs/RareBlue/", typeof(ObjectiveInfoSO));
        tree.AddAllAssetsAtPath("RarePink", "Assets/_Prefabs/Objectives/_InfoSOs/RarePink/", typeof(ObjectiveInfoSO));
        tree.AddAllAssetsAtPath("VeryRare", "Assets/_Prefabs/Objectives/_InfoSOs/VeryRare/", typeof(ObjectiveInfoSO));
        tree.AddAllAssetsAtPath("VeryRareBlue", "Assets/_Prefabs/Objectives/_InfoSOs/VeryRareBlue/", typeof(ObjectiveInfoSO));
        tree.AddAllAssetsAtPath("VeryRarePink", "Assets/_Prefabs/Objectives/_InfoSOs/VeryRarePink/", typeof(ObjectiveInfoSO));
        return tree;
    }

    public class CreateNewEnemyData
    {
        public CreateNewEnemyData()
        {
            objectiveData = ScriptableObject.CreateInstance<ObjectiveInfoSO>();
            objectiveData.Name = "New Objective";
        }

        [InlineEditor(Expanded = true)]
        public ObjectiveInfoSO objectiveData;

        [Button("Add New Enemy SO")]
        private void CreateNewData()
        {
            string type = "";
            if (objectiveData.ObjectiveMaterial == ObjectiveMaterial.Rare && objectiveData.ObjectiveCrystal == ObjectiveCrystal.None)
                type = "Rare/";
            if (objectiveData.ObjectiveMaterial == ObjectiveMaterial.Rare && objectiveData.ObjectiveCrystal == ObjectiveCrystal.Blue)
                type = "RareBlue/";
            if (objectiveData.ObjectiveMaterial == ObjectiveMaterial.Rare && objectiveData.ObjectiveCrystal == ObjectiveCrystal.Pink)
                type = "RarePink/";
            if (objectiveData.ObjectiveMaterial == ObjectiveMaterial.VeryRare && objectiveData.ObjectiveCrystal == ObjectiveCrystal.None)
                type = "VeryRare/";
            if (objectiveData.ObjectiveMaterial == ObjectiveMaterial.VeryRare && objectiveData.ObjectiveCrystal == ObjectiveCrystal.Blue)
                type = "VeryRareBlue/";
            if (objectiveData.ObjectiveMaterial == ObjectiveMaterial.VeryRare && objectiveData.ObjectiveCrystal == ObjectiveCrystal.Pink)
                type = "VeryRarePink/";

            AssetDatabase.CreateAsset(objectiveData, "Assets/_Prefabs/Objectives/_InfoSOs/" + type + objectiveData.Name + ".asset");
            AssetDatabase.SaveAssets();
        }
    }
}