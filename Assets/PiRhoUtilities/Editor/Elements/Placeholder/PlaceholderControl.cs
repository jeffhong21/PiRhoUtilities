﻿using UnityEngine.UIElements;

namespace PiRhoSoft.Utilities.Editor
{
	public class PlaceholderControl : Label
	{
		public const string Stylesheet = "Placeholder/PlaceholderStyle.uss";
		public const string UssClassName = "pirho-placeholder";

		public PlaceholderControl(string text)
		{
			this.text = text;
			this.AddStyleSheet(Configuration.ElementsPath, Stylesheet);
			AddToClassList(UssClassName);

			pickingMode = PickingMode.Ignore;
		}

		public void AddToField(TextField textField)
		{
			void UpdateDisplayed() => this.SetDisplayed(string.IsNullOrEmpty(textField.text));

			// Add this specifically to the input field in case the TextField has a label
			var input = textField.Q(className: TextField.inputUssClassName);
			input.Add(this);

			UpdateDisplayed();

			textField.RegisterCallback<ChangeEvent<string>>(evt => UpdateDisplayed());
		}
	}
}