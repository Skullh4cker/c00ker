using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dishcovery
{
    public class CursorEntry : Entry
    {
        public static BindableProperty CursorColorProperty = BindableProperty.Create(
        nameof(CursorColor), typeof(Color), typeof(CursorEntry), Colors.Black);

        public Color CursorColor
        {
            get => (Color)GetValue(CursorColorProperty);
            set => SetValue(CursorColorProperty, value);
        }

        public CursorEntry()
        {
        }
    }
}
