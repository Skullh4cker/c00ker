using Android.Content;
using AndroidX.AppCompat.Widget;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dishcovery;

public partial class CursorEntryHandler
{
    AppCompatEditText _nativeEntry;

    protected override AppCompatEditText CreatePlatformView()
    {
        _nativeEntry = new AppCompatEditText(Context);
        return _nativeEntry;
    }

    internal void SetCursorColor(CursorEntry entry)
    {
        _nativeEntry.TextCursorDrawable.SetTint(entry.CursorColor.ToAndroid());
    }
}
