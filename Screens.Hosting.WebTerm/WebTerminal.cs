using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Screens.Hosting.WebTerm
{
    public class WebTerminal : Terminal
    {
        public override void Beep()
        {
            throw new NotImplementedException();
        }

        public override void Clear()
        {
            throw new NotImplementedException();
        }

        public override void HideCursor()
        {
            throw new NotImplementedException();
        }

        public override void SetBackGroundColor(ConsoleColor back)
        {
            throw new NotImplementedException();
        }

        public override void SetForeGroundColor(ConsoleColor back)
        {
            throw new NotImplementedException();
        }

        public override void SetScreenSize(int width, int height)
        {
            throw new NotImplementedException();
        }

        public override void ShowCursor()
        {
            throw new NotImplementedException();
        }

        public override void SubmitChanges(TerminalChanges changes)
        {
            
        }

        protected override void SetCursorPositionImpl()
        {
            throw new NotImplementedException();
        }
    }
}
