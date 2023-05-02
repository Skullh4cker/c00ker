using Microsoft.Maui.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dishcovery
{
#if ANDROID
    public sealed partial class CursorEntryHandler : EntryHandler
    {
        public CursorEntryHandler()
        {
            Mapper.AppendToMapping("CursorEntryCustomization", MapCursorEntry);
        }

        private void MapCursorEntry(IEntryHandler entryHandler, IEntry entry)
        {
            if (entry is CursorEntry cursorEntry && entryHandler is CursorEntryHandler cursorEntryHandler)
            {
                SetCursorColor(cursorEntry);
            }
        }
    }
#else
    public partial class CursorEntryHandler : EntryHandler
    {
    }
#endif
}
