using System;
using Eto.Forms;
#if XAMMAC2
using AppKit;
using Foundation;
using CoreGraphics;
using ObjCRuntime;
using CoreAnimation;
#else
using MonoMac.AppKit;
using MonoMac.Foundation;
using MonoMac.CoreGraphics;
using MonoMac.ObjCRuntime;
using MonoMac.CoreAnimation;
#endif

namespace Eto.Mac
{
	public interface IMenuActionHandler
	{
		void HandleClick();

		bool Enabled { get; }

		MenuItem Widget { get; }

		MenuItem.ICallback Callback { get; }
	}

	[Register("EtoMenuActionHandler")]
	public class MenuActionHandler : NSObject
	{
		internal static Selector selActivate = new Selector("activate:");
		WeakReference handler;

		public IMenuActionHandler Handler { get { return (IMenuActionHandler)handler.Target; } set { handler = new WeakReference(value); } }

		[Export("activate:")]
		public void Activate(NSObject sender)
		{
			var h = Handler;
			if (h != null)
				h.HandleClick();
		}

		[Export("validateMenuItem:")]
		public bool ValidateMenuItem(NSMenuItem item)
		{
			var h = Handler;
			if (h != null)
			{
				h.Callback.OnValidate(h.Widget, EventArgs.Empty);
				return h.Enabled;
			}
			return false;
		}
	}
}

