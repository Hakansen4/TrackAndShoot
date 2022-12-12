using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EasyClap.Seneca.Common.Editor
{
	public static class ToonyColorsProOptimizationTools
	{
		private const string Menu = "Easy Clap/Materials/";

		[MenuItem(Menu + "Convert Hybrid Shaders to Custom Toony")]
		private static void ConvertHybridShadersToCustomToony()
		{
			var confirmation = SenecaEditorUtility.Confirm($"You are about to perform the following shader conversions on the materials of this project:\n{SenecaEditorUtility.BulletPoint} Toony Hybrid Shader -> Custom Toony Shader\n{SenecaEditorUtility.BulletPoint} Toony Hybrid Shader Outline -> Custom Toony Shader Outline\nThis will improve the build times and performance drastically, but you may need to do some adjustments to match the visuals again. Do you wish to continue?");

			if (!confirmation)
			{
				return;
			}

			var hybridShader = FindShader("Toony Colors Pro 2/Hybrid Shader");
			var hybridOutlineShader = FindShader("Toony Colors Pro 2/Hybrid Shader Outline");
			var customShader = FindShader("Toony Colors Pro 2/User/Custom Toony Shader");
			var customOutlineShader = FindShader("Toony Colors Pro 2/User/Custom Toony Shader Outline");

			var allMaterialGUIDs = AssetDatabase.FindAssets("t:Material");
			foreach (var materialGuid in allMaterialGUIDs)
			{
				var path = AssetDatabase.GUIDToAssetPath(materialGuid);
				var material = AssetDatabase.LoadAssetAtPath<Material>(path);
				if (material.shader == hybridShader)
				{
					material.shader = customShader;
					Debug.Log($"Converted material shader to Custom Toony for: <b>{material}</b>", material);
				}
				else if (material.shader == hybridOutlineShader)
				{
					material.shader = customOutlineShader;
					Debug.Log($"Converted material shader to Custom Toony Outline for: <b>{material}</b>", material);
				}
			}

			AssetDatabase.SaveAssets();
		}

		[MenuItem(Menu + "List Unconverted Toony Materials")]
		private static void ListUnconvertedToonyMaterials()
		{
			var convertedShaderNames = new[]
			{
				"Toony Colors Pro 2/User/Custom Toony Shader",
				"Toony Colors Pro 2/User/Custom Toony Shader Outline",
			};
			var unconvertedShaderNameStartsWith = "Toony Colors Pro 2/";

			var count = 0;
			var guids = AssetDatabase.FindAssets("t:Material");
			foreach (var guid in guids)
			{
				var path = AssetDatabase.GUIDToAssetPath(guid);
				var material = AssetDatabase.LoadAssetAtPath<Material>(path);
				var shaderName = material.shader.name;
				if (!convertedShaderNames.Contains(shaderName) &&
				    shaderName.StartsWith(unconvertedShaderNameStartsWith))
				{
					count++;
					Debug.LogError($"Shader '<b>{shaderName}</b>' detected in material '<b>{material.name}</b>' which is heavy to render and increases build times drastically.", material);
				}
			}
			if (count > 0)
			{
				throw new Exception($"Found '{count}' materials that should be looked into. Click on errors above to go to these materials. Use bulk converter to use optimized shaders.");
			}
			else
			{
				Debug.Log("OK! There are no Toony Colors Pro materials left with unoptimized shaders.");
			}
		}

		// [MenuItem(Menu + "Set Mobile Mode For Toony", priority = 5021)]
		// public static void SetMobileMod()
		// {
		// 	var hybridShader = FindShader("Toony Colors Pro 2/Hybrid Shader");
		//
		// 	var allMaterialGUIDs = AssetDatabase.FindAssets("t:Material");
		// 	for (int i = 0; i < allMaterialGUIDs.Length; i++)
		// 	{
		// 		var path = AssetDatabase.GUIDToAssetPath(allMaterialGUIDs[i]);
		// 		var material = AssetDatabase.LoadAssetAtPath<Material>(path);
		// 		if (material.shader == hybridShader)
		// 		{
		// 			if (material.GetFloat("_UseMobileMode") != 1.0f)
		// 			{
		// 				material.SetFloat("_UseMobileMode", 1.0f);
		// 				Debug.Log("Enabled mobile mode for material: " + material, material);
		// 			}
		// 		}
		// 	}
		//
		// 	AssetDatabase.SaveAssets();
		// }

		private static Shader FindShader(string shaderName)
		{
			var shader = Shader.Find(shaderName);
			if (!shader)
			{
				throw new Exception($"Failed to find '{shaderName}'");
			}
			return shader;
		}
	}

}
