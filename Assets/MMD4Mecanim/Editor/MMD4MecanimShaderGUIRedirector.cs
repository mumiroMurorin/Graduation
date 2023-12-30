#if UNITY_2019_1_OR_NEWER || UNITY_2020_1_OR_NEWER || UNITY_2021_1_OR_NEWER || UNITY_2022_1_OR_NEWER || UNITY_2023_1_OR_NEWER || UNITY_2024_1_OR_NEWER
using System;
using UnityEngine;
using UnityEditor;

using System.Reflection;
using System.Collections.Generic;

public class MMD4MecanimShaderGUIRedirector : ShaderGUI
{
	static Type GetType( string assemblyName, string className )
	{
		var assembly = System.Reflection.Assembly.Load(assemblyName);
		if( assembly != null ) {
			return assembly.GetType(className);
		}

		return null;
	}

	class MaterialInt
	{
		public string name;
		public int value;
	}

	class MaterialFloat
	{
		public string name;
		public float value;
	}

	class MaterialColor
	{
		public string name;
		public Color value;
	}

	class MaterialKeyword
	{
		public string name;
		public bool value;
	}

	class MaterialShaderPass
	{
		public string name;
		public bool value;
	}

	bool _isKeepingMaterialRenderQueues = false;
	int _keepedMaterialRenderQueue = 0;
	List<MaterialInt> _keepingMaterialInts = new List<MaterialInt>();
	List<MaterialFloat> _keepingMaterialFloats = new List<MaterialFloat>();
	List<MaterialColor> _keepingMaterialColors = new List<MaterialColor>();
	List<MaterialKeyword> _keepingMaterialKeywords = new List<MaterialKeyword>();
	List<MaterialShaderPass> _keepingMaterialShaderPasses = new List<MaterialShaderPass>();

	Type _classType;
	object _instance;
	MethodInfo _AssignNewShaderToMaterial;
	MethodInfo _OnClosed;
	MethodInfo _OnGUI;
	MethodInfo _OnMaterialInteractivePreviewGUI;
	MethodInfo _OnMaterialPreviewGUI;
	MethodInfo _OnMaterialPreviewSettingsGUI;

	//"Unity.RenderPipelines.HighDefinition.Editor", "UnityEditor.Rendering.HighDefinition.LitGUI"
	public MMD4MecanimShaderGUIRedirector(string assemblyName, string className)
	{
		var _classType = GetType( assemblyName, className );
		if( _classType != null ) {
			_instance = Activator.CreateInstance( _classType );
			if( _instance != null ) {
				_AssignNewShaderToMaterial = _classType.GetMethod("_AssignNewShaderToMaterial");
				_OnClosed = _classType.GetMethod("OnClosed");
				_OnGUI = _classType.GetMethod("OnGUI");
				_OnMaterialInteractivePreviewGUI = _classType.GetMethod("OnMaterialInteractivePreviewGUI");
				_OnMaterialPreviewGUI = _classType.GetMethod("OnMaterialPreviewGUI");
				_OnMaterialPreviewSettingsGUI = _classType.GetMethod("OnMaterialPreviewSettingsGUI");
			}
		}
	}

	~MMD4MecanimShaderGUIRedirector()
	{
	}

	public override void AssignNewShaderToMaterial( Material material, Shader oldShader, Shader newShader )
	{
		if( _AssignNewShaderToMaterial != null ) {
			_AssignNewShaderToMaterial.Invoke(_instance, new object[] { material, oldShader, newShader });
		}
	}

	public override void OnClosed( Material material )
	{
		if( _OnClosed != null ) {
			_OnClosed.Invoke(_instance, new object[] { material });
		}
	}

	public override void OnGUI( MaterialEditor materialEditor, MaterialProperty[] properties )
	{
		OnPreGUI( materialEditor );
		if( _OnGUI != null ) {
			_OnGUI.Invoke(_instance, new object[] { materialEditor, properties });
		}
		OnPostGUI( materialEditor );
	}

	public override void OnMaterialInteractivePreviewGUI( MaterialEditor materialEditor, Rect r, GUIStyle background )
	{
		OnPreGUI( materialEditor );
		if( _OnMaterialInteractivePreviewGUI != null ) {
			_OnMaterialInteractivePreviewGUI.Invoke(_instance, new object[] { materialEditor, r, background });
		}
		OnPostGUI( materialEditor );
	}

	public override void OnMaterialPreviewGUI( MaterialEditor materialEditor, Rect r, GUIStyle background )
	{
		OnPreGUI( materialEditor );
		if( _OnMaterialPreviewGUI != null ) {
			_OnMaterialPreviewGUI.Invoke(_instance, new object[] { materialEditor, r, background });
		}
		OnPostGUI( materialEditor );
	}

	public override void OnMaterialPreviewSettingsGUI( MaterialEditor materialEditor )
	{
		OnPreGUI( materialEditor );
		if( _OnMaterialPreviewSettingsGUI != null ) {
			_OnMaterialPreviewSettingsGUI.Invoke(_instance, new object[] { materialEditor });
		}
		OnPostGUI( materialEditor );
	}

	protected virtual void OnPreGUI( MaterialEditor materialEditor )
	{
		Material material = materialEditor.target as Material;
		if( _isKeepingMaterialRenderQueues ) {
			_keepedMaterialRenderQueue = material.renderQueue;
		}

		foreach( var materialInt in _keepingMaterialInts ) {
			if( material.HasProperty(materialInt.name) ) {
				materialInt.value = material.GetInt(materialInt.name);
			}
		}
		foreach( var materialFloat in _keepingMaterialFloats ) {
			if( material.HasProperty(materialFloat.name) ) {
				materialFloat.value = material.GetFloat(materialFloat.name);
			}
		}
		foreach( var materialColor in _keepingMaterialColors ) {
			if( material.HasProperty(materialColor.name) ) {
				materialColor.value = material.GetColor(materialColor.name);
			}
		}
		foreach( var materialKeyword in _keepingMaterialKeywords ) {
			materialKeyword.value = material.IsKeywordEnabled(materialKeyword.name);
		}
		foreach( var materialShaderPass in _keepingMaterialShaderPasses ) {
			materialShaderPass.value = material.GetShaderPassEnabled(materialShaderPass.name);
		}
	}

	protected virtual void OnPostGUI( MaterialEditor materialEditor )
	{
		Material material = materialEditor.target as Material;
		if( _isKeepingMaterialRenderQueues ) {
			if( material.renderQueue != _keepedMaterialRenderQueue ) {
				material.renderQueue = _keepedMaterialRenderQueue;
			}
		}

		foreach( var materialInt in _keepingMaterialInts ) {
			if( material.HasProperty(materialInt.name) ) {
				if( materialInt.value != material.GetInt(materialInt.name) ) {
					material.SetInt(materialInt.name, materialInt.value);
				}
			}
		}
		foreach( var materialFloat in _keepingMaterialFloats ) {
			if( material.HasProperty(materialFloat.name) ) {
				if( materialFloat.value != material.GetFloat(materialFloat.name) ) {
					material.SetFloat(materialFloat.name, materialFloat.value);
				}
			}
		}
		foreach( var materialColor in _keepingMaterialColors ) {
			if( material.HasProperty(materialColor.name) ) {
				if( materialColor.value != material.GetColor(materialColor.name) ) {
					material.SetColor(materialColor.name, materialColor.value);
				}
			}
		}
		foreach( var materialKeyword in _keepingMaterialKeywords ) {
			if( material.IsKeywordEnabled(materialKeyword.name) != materialKeyword.value ) {
				if( materialKeyword.value ) {
					material.EnableKeyword(materialKeyword.name);
				} else {
					material.DisableKeyword(materialKeyword.name);
				}
			}
		}
		foreach( var materialShaderPass in _keepingMaterialShaderPasses ) {
			if( material.GetShaderPassEnabled(materialShaderPass.name) != materialShaderPass.value ) {
				material.SetShaderPassEnabled(materialShaderPass.name, materialShaderPass.value);
			}
		}
	}

	public void SetKeepingMaterialRenderQueue( bool isKeeping )
	{
		_isKeepingMaterialRenderQueues = isKeeping;
	}

	public void SetKeepingMaterialInt(string name_)
	{
		_keepingMaterialInts.Add( new MaterialInt() { name = name_ } );
	}

	public void SetKeepingMaterialFloat(string name_)
	{
		_keepingMaterialFloats.Add( new MaterialFloat() { name = name_ } );
	}

	public void SetKeepingMaterialColor(string name_)
	{
		_keepingMaterialColors.Add( new MaterialColor() { name = name_ } );
	}

	public void SetKeepingMaterialKeyword(string name_)
	{
		_keepingMaterialKeywords.Add( new MaterialKeyword() { name = name_ } );
	}

	public void SetKeepingMaterialShaderPass(string name_)
	{
		_keepingMaterialShaderPasses.Add( new MaterialShaderPass() { name = name_ } );
	}

	public static void SetFloat( Material material, string name, float value )
	{
		if( !Mathf.Approximately( material.GetFloat( name ), value ) ) {
			material.SetFloat( name, value );
		}
	}
}
#endif