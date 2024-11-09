using System.Collections.Generic;
using UnityEngine;

public class ShaderManagerBase
{
	public Shader shader;
	public List<string> togglePropNames;

	public ShaderManagerBase(List<string> togglePropNames, Shader shader) 
	{ 
		this.shader = shader;
		this.togglePropNames = togglePropNames;
	}
}
