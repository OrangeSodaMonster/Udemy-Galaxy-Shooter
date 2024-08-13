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
            DestroyImmediate(createNewEnemyData.asteroidData);
    }

    protected override OdinMenuTree BuildMenuTree()
    {
        var tree = new OdinMenuTree();
        tree.Selection.SupportsMultiSelect = false;

        createNewEnemyData = new CreateNewEnemyData();

        tree.Add("Create New", createNewEnemyData);
        tree.AddAllAssetsAtPath("OutOfFolder", "Assets/_Prefabs/Asteroids/_InfoSOs/", typeof(AsteroidInfoSO));
        tree.AddAllAssetsAtPath("Base", "Assets/_Prefabs/Asteroids/_InfoSOs/Base/", typeof(AsteroidInfoSO));
        tree.AddAllAssetsAtPath("Metal", "Assets/_Prefabs/Asteroids/_InfoSOs/Metal/", typeof(AsteroidInfoSO));
        tree.AddAllAssetsAtPath("Rare", "Assets/_Prefabs/Asteroids/_InfoSOs/Rare/", typeof(AsteroidInfoSO));
        tree.AddAllAssetsAtPath("RareBlue", "Assets/_Prefabs/Asteroids/_InfoSOs/RareBlue/", typeof(AsteroidInfoSO));
        tree.AddAllAssetsAtPath("RarePink", "Assets/_Prefabs/Asteroids/_InfoSOs/RarePink/", typeof(AsteroidInfoSO));
        tree.AddAllAssetsAtPath("VeryRare", "Assets/_Prefabs/Asteroids/_InfoSOs/VeryRare/", typeof(AsteroidInfoSO));
        tree.AddAllAssetsAtPath("VeryRareBlue", "Assets/_Prefabs/Asteroids/_InfoSOs/VeryRareBlue/", typeof(AsteroidInfoSO));
        tree.AddAllAssetsAtPath("VeryRarePink", "Assets/_Prefabs/Asteroids/_InfoSOs/VeryRarePink/", typeof(AsteroidInfoSO));
        return tree;
    }

    public class CreateNewEnemyData
    {
        public CreateNewEnemyData()
        {
            asteroidData = ScriptableObject.CreateInstance<AsteroidInfoSO>();
            asteroidData.Name = "New Asteroid";
        }

        [InlineEditor(Expanded = true)]
        public AsteroidInfoSO asteroidData;

        [Button("Add New Enemy SO")]
        private void CreateNewData()
        {
            string type = "";
            if (asteroidData.AsteroidMaterial == AsteroidMaterial.Base) type = "Base/";
            if (asteroidData.AsteroidMaterial == AsteroidMaterial.Metal) type = "Metal/";
            if (asteroidData.AsteroidMaterial == AsteroidMaterial.Rare && asteroidData.AsteroidCrystal == AsteroidCrystal.None)
                type = "Rare/";
            if (asteroidData.AsteroidMaterial == AsteroidMaterial.Rare && asteroidData.AsteroidCrystal == AsteroidCrystal.Blue)
                type = "RareBlue/";
            if (asteroidData.AsteroidMaterial == AsteroidMaterial.Rare && asteroidData.AsteroidCrystal == AsteroidCrystal.Pink)
                type = "RarePink/";
            if (asteroidData.AsteroidMaterial == AsteroidMaterial.VeryRare && asteroidData.AsteroidCrystal == AsteroidCrystal.None)
                type = "VeryRare/";
            if (asteroidData.AsteroidMaterial == AsteroidMaterial.VeryRare && asteroidData.AsteroidCrystal == AsteroidCrystal.Blue)
                type = "VeryRareBlue/";
            if (asteroidData.AsteroidMaterial == AsteroidMaterial.VeryRare && asteroidData.AsteroidCrystal == AsteroidCrystal.Pink)
                type = "VeryRarePink/";

            AssetDatabase.CreateAsset(asteroidData, "Assets/_Prefabs/Asteroids/_InfoSOs/" + type + asteroidData.Name + ".asset");
            AssetDatabase.SaveAssets();
        }
    }
}