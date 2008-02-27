// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
// Copyright (c) 2004-2005 Novell, Inc.
//
// Authors:
//	Peter Bartok	pbartok@novell.com
//

using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Runtime.InteropServices;

namespace System.Windows.Forms {
	[DefaultEvent("Load")]
	[DesignerCategory("UserControl")]
	[Designer("System.Windows.Forms.Design.ControlDesigner, " + Consts.AssemblySystem_Design, "System.ComponentModel.Design.IDesigner")]
	[Designer("System.Windows.Forms.Design.UserControlDocumentDesigner, " + Consts.AssemblySystem_Design, typeof(IRootDesigner))]
#if NET_2_0
	[ClassInterface (ClassInterfaceType.AutoDispatch)]
	[ComVisible (true)]
#endif
	public class UserControl : ContainerControl {
		#region Public Constructors
		public UserControl() {
#if NET_2_0
			SetStyle (ControlStyles.SupportsTransparentBackColor, true);
#endif
		}
		#endregion	// Public Constructors

		#region Public Instance Properties
#if NET_2_0
		[Browsable (true)]
		[EditorBrowsable (EditorBrowsableState.Always)]
		[DesignerSerializationVisibility (DesignerSerializationVisibility.Visible)]
		public override bool AutoSize {
			get { return base.AutoSize; }
			set { base.AutoSize = value; }
		}
		
		[Browsable (true)]
		[LocalizableAttribute(true)] 
		[DefaultValue (AutoSizeMode.GrowOnly)]
		public AutoSizeMode AutoSizeMode {
			get { return base.GetAutoSizeMode (); } 
			set {
				if (base.GetAutoSizeMode () != value) {
					base.SetAutoSizeMode (value);
				}
			} 
		}

		[Browsable (true)]
		[EditorBrowsable (EditorBrowsableState.Always)]
		public override AutoValidate AutoValidate {
			get { return base.AutoValidate; }
			set { base.AutoValidate = value; }
		}
#endif
		protected override Size DefaultSize {
			get {
				return new Size(150, 150);
			}
		}


		[Bindable(false)]
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public override string Text {
			get {
				return base.Text;
			}

			set {
				base.Text = value;
			}
		}
		#endregion	// Public Instance Properties

		#region Public Instance Methods
#if NET_2_0
		[Browsable (true)]
		[EditorBrowsable (EditorBrowsableState.Always)]
		public override bool ValidateChildren ()
		{
			return base.ValidateChildren ();
		}

		[Browsable (true)]
		[EditorBrowsable (EditorBrowsableState.Always)]
		public override bool ValidateChildren (ValidationConstraints validationConstraints)
		{
			return base.ValidateChildren (validationConstraints);
		}
#endif
		#endregion
		
		#region Protected Instance Methods
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnCreateControl() {
			base.OnCreateControl();

			// The OnCreateControl isn't neccessarily raised *before* it
			// becomes first visible, but that's the best we've got
			OnLoad(EventArgs.Empty);
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected virtual void OnLoad(EventArgs e) {
			EventHandler eh = (EventHandler)(Events [LoadEvent]);
			if (eh != null)
				eh (this, e);
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void OnMouseDown(MouseEventArgs e) {
			base.OnMouseDown(e);
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected override void WndProc(ref Message m) {
			base.WndProc(ref m);
		}
		#endregion	// Protected Instance Methods

		#region Protected Properties
#if NET_2_0
		protected override CreateParams CreateParams {
			get { 
				CreateParams cp = base.CreateParams;
				cp.Style |= (int)WindowStyles.WS_TABSTOP;
				cp.ExStyle |= (int)WindowExStyles.WS_EX_CONTROLPARENT;
				return cp;
			}
		}
#endif
		#endregion
		#region Events
		static object LoadEvent = new object ();

#if NET_2_0
		[Browsable (true)]
		[EditorBrowsable (EditorBrowsableState.Always)]
		public new event EventHandler AutoSizeChanged {
			add { base.AutoSizeChanged += value; }
			remove { base.AutoSizeChanged -= value; }
		}

		[Browsable (true)]
		[EditorBrowsable (EditorBrowsableState.Always)]
		public new event EventHandler AutoValidateChanged {
			add { base.AutoValidateChanged += value; }
			remove { base.AutoValidateChanged -= value; }
		}
#endif

		public event EventHandler Load {
			add { Events.AddHandler (LoadEvent, value); }
			remove { Events.RemoveHandler (LoadEvent, value); }
		}

		[Browsable(false)]
		[EditorBrowsable(EditorBrowsableState.Never)]
		public new event EventHandler TextChanged {
			add { base.TextChanged += value; }
			remove { base.TextChanged -= value; }
		}

		#endregion	// Events

#if NET_2_0
		protected override void OnResize (EventArgs e)
		{
			base.OnResize (e);
		}

		[Browsable (true)]
		[DefaultValue (BorderStyle.None)]
		[EditorBrowsable (EditorBrowsableState.Always)]
		public BorderStyle BorderStyle {
			get { return InternalBorderStyle; }
			set { InternalBorderStyle = value; }
		}

		internal override Size GetPreferredSizeCore (Size proposedSize)
		{
			Size retsize = Size.Empty;

			// Add up the requested sizes for Docked controls
			foreach (Control child in Controls) {
				if (!child.is_visible)
					continue;
					
				if (child.Dock == DockStyle.Left || child.Dock == DockStyle.Right)
					retsize.Width += child.PreferredSize.Width;
				else if (child.Dock == DockStyle.Top || child.Dock == DockStyle.Bottom)
					retsize.Height += child.PreferredSize.Height;	
			}
			
			// See if any non-Docked control is positioned lower or more right than our size
			foreach (Control child in Controls) {
				if (!child.is_visible)
					continue;

				if (child.Dock != DockStyle.None)
					continue;
					
				// If its anchored to the bottom or right, that doesn't really count
				if ((child.Anchor & AnchorStyles.Bottom) == AnchorStyles.Bottom || (child.Anchor & AnchorStyles.Right) == AnchorStyles.Right)
					continue;
					
				retsize.Width = Math.Max (retsize.Width, child.Bounds.Right + child.Margin.Right);
				retsize.Height = Math.Max (retsize.Height, child.Bounds.Bottom + child.Margin.Bottom);
			}

			return retsize;
		}
#endif
	}
}
