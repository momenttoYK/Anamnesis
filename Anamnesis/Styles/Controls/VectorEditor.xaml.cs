﻿// Concept Matrix 3.
// Licensed under the MIT license.

namespace Anamnesis.WpfStyles.Controls
{
	using System.ComponentModel;
	using System.Windows;
	using System.Windows.Controls;
	using Anamnesis.WpfStyles.DependencyProperties;
	using PropertyChanged;

	using Vector = Anamnesis.Memory.Vector;

	/// <summary>
	/// Interaction logic for Vector3DEditor.xaml.
	/// </summary>
	[SuppressPropertyChangedWarnings]
	[AddINotifyPropertyChangedInterface]
	public partial class VectorEditor : UserControl, INotifyPropertyChanged
	{
		public static readonly IBind<bool> ExpandedDp = Binder.Register<bool, VectorEditor>(nameof(Expanded));
		public static readonly IBind<Vector> ValueDp = Binder.Register<Vector, VectorEditor>(nameof(Value), OnValueChanged);
		public static readonly IBind<double> TickFrequencyDp = Binder.Register<double, VectorEditor>(nameof(TickFrequency));
		public static readonly IBind<bool> WrapDp = Binder.Register<bool, VectorEditor>(nameof(Wrap));
		public static readonly IBind<NumberBox.SliderModes> SlidersDp = Binder.Register<NumberBox.SliderModes, VectorEditor>(nameof(Sliders));
		public static readonly IBind<double> MinDp = Binder.Register<double, VectorEditor>(nameof(Minimum));
		public static readonly IBind<double> MaxDp = Binder.Register<double, VectorEditor>(nameof(Maximum), OnMaximumChanged);
		public static readonly IBind<bool> CanLinkDp = Binder.Register<bool, VectorEditor>(nameof(CanLink));
		public static readonly IBind<bool> LinkedDp = Binder.Register<bool, VectorEditor>(nameof(Linked));

		private bool lockChangedEvent = false;

		public VectorEditor()
		{
			this.InitializeComponent();
			this.ContentArea.DataContext = this;
			this.TickFrequency = 0.1;
			this.Sliders = NumberBox.SliderModes.Absolute;
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		public double Minimum
		{
			get => MinDp.Get(this);
			set => MinDp.Set(this, value);
		}

		public double Maximum
		{
			get => MaxDp.Get(this);
			set => MaxDp.Set(this, value);
		}

		public bool Expanded
		{
			get => ExpandedDp.Get(this);
			set => ExpandedDp.Set(this, value);
		}

		public NumberBox.SliderModes Sliders
		{
			get => SlidersDp.Get(this);
			set => SlidersDp.Set(this, value);
		}

		public Vector Value
		{
			get => ValueDp.Get(this);
			set => ValueDp.Set(this, value);
		}

		public double TickFrequency
		{
			get => TickFrequencyDp.Get(this);
			set => TickFrequencyDp.Set(this, value);
		}

		public bool Wrap
		{
			get => WrapDp.Get(this);
			set => WrapDp.Set(this, value);
		}

		public bool CanLink
		{
			get => CanLinkDp.Get(this);
			set => CanLinkDp.Set(this, value);
		}

		public bool Linked
		{
			get => LinkedDp.Get(this);
			set => LinkedDp.Set(this, value);
		}

		[AlsoNotifyFor(nameof(VectorEditor.Value))]
		[DependsOn(nameof(VectorEditor.Value))]
		public float X
		{
			get
			{
				return this.Value.X;
			}

			set
			{
				this.Value = new Vector(value, this.Y, this.Z);
			}
		}

		[AlsoNotifyFor(nameof(VectorEditor.Value))]
		[DependsOn(nameof(VectorEditor.Value))]
		public float Y
		{
			get
			{
				return this.Value.Y;
			}

			set
			{
				this.Value = new Vector(this.X, value, this.Z);
			}
		}

		[AlsoNotifyFor(nameof(VectorEditor.Value))]
		[DependsOn(nameof(VectorEditor.Value))]
		public float Z
		{
			get
			{
				return this.Value.Z;
			}

			set
			{
				this.Value = new Vector(this.X, this.Y, value);
			}
		}

		private static void OnValueChanged(VectorEditor sender, Vector oldValue, Vector newValue)
		{
			if (sender.Linked && !sender.lockChangedEvent)
			{
				sender.lockChangedEvent = true;
				Vector deltaV = newValue - oldValue;
				float delta = deltaV.X + deltaV.Y + deltaV.Z;
				sender.Value = oldValue + delta;
				sender.lockChangedEvent = false;
			}

			sender.PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(nameof(VectorEditor.X)));
			sender.PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(nameof(VectorEditor.Y)));
			sender.PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(nameof(VectorEditor.Z)));
		}

		private static void OnMaximumChanged(VectorEditor sender, double value)
		{
			sender.ExpandedX.Maximum = value;
		}

		private void LinkClicked(object sender, RoutedEventArgs e)
		{
			if (!this.CanLink)
			{
				this.Linked = false;
				return;
			}

			this.Linked = !this.Linked;
		}
	}
}
