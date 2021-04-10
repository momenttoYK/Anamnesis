﻿// © Anamnesis.
// Developed by W and A Walsh.
// Licensed under the MIT license.

namespace Anamnesis.Services
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Windows;
	using Anamnesis.GameData;
	using PropertyChanged;

	[Serializable]
	[AddINotifyPropertyChangedInterface]
	public class Settings : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;

		public enum HomeWidgetType
		{
			XmaTop,
			XmaLatest,
			Art,
			Wiki,
			None,
		}

		public string Language { get; set; } = "EN";
		public bool AlwaysOnTop { get; set; } = true;
		public bool ThemeDark { get; set; } = true;
		public string ThemeSwatch { get; set; } = @"deeporange";
		public double Opacity { get; set; } = 1.0;
		public bool StayTransparent { get; set; } = false;
		public double Scale { get; set; } = 1.0;
		public bool UseWindowsExplorer { get; set; } = false;
		public Point WindowPosition { get; set; }
		public string DefaultPoseDirectory { get; set; } = "%MyDocuments%/Anamnesis/Poses/";
		public string DefaultCharacterDirectory { get; set; } = "%MyDocuments%/Anamnesis/Characters/";
		public string DefaultSceneDirectory { get; set; } = "%MyDocuments%/Anamnesis/Scenes/";
		public HomeWidgetType HomeWidget { get; set; } = HomeWidgetType.Art;
		public bool UseCustomBorder { get; set; } = true;
		public bool ShowAdvancedOptions { get; set; } = true;
		public bool FlipPoseGuiSides { get; set; } = false;

		public List<IItem> FavoriteItems { get; set; } = new List<IItem>();

		public DateTimeOffset LastUpdateCheck { get; set; } = DateTimeOffset.MinValue;

		public bool IsFavorite(IItem item)
		{
			return this.FavoriteItems.Contains(item);
		}

		public void SetFavorite(IItem item, bool favorite)
		{
			bool isFavorite = this.IsFavorite(item);

			if (favorite == isFavorite)
				return;

			if (favorite)
			{
				this.FavoriteItems.Add(item);
			}
			else
			{
				this.FavoriteItems.Remove(item);
			}

			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Settings.FavoriteItems)));
		}
	}
}
