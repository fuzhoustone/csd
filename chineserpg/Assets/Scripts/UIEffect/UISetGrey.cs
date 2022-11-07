using UnityEngine;
using UnityEngine.UI;

public class UISetGrey
{
	static private Material _uiGreyMat = null;
	static private Material uiGreyMat
	{
		get
		{
			if (!_uiGreyMat)
			{
				var shader = Shader.Find("UI/roleUIGray");
				if (shader)
				{
					_uiGreyMat = new Material(shader);
					_uiGreyMat.hideFlags = HideFlags.HideAndDontSave;
				}
			}
			return _uiGreyMat;
		}
	}
	public static void SetUIDrey(GameObject uiObj, bool isGrey = true)
	{
		var graphics = uiObj.GetComponentsInChildren<Graphic>();
		int length = graphics.Length;
		for (int i = 0; i < length; i++)
		{
			graphics[i].material = isGrey ? uiGreyMat : null;
		}
	}
}


