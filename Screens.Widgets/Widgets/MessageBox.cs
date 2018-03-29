using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualBasic;

namespace Screens
{

    //TODO: redo better

    public class MessageBox
    {
        public static DialogResult Show(string text, string caption = "", MessageBoxButtons buttons = default(MessageBoxButtons), int icon = 0, int defaultButton = 0)
        {
            return DialogResult.OK;
        }
    }

    public enum MessageBoxButtons
    {
        AbortRetryIgnore,
        OK,
        OKCancel,
        RetryCancel,
        YesNo,
        YesNoCancel
    }

    // <System.Diagnostics.DebuggerStepThrough()>
    public enum MessageBoxDefaultButton
    {
        Button1,
        Button2,
        Button3
    }

    // <System.Diagnostics.DebuggerStepThrough()>
    public enum MessageBoxIcon
    {
        Asterisk,
        Error,
        Exclamation,
        Hand,
        Information,
        None,
        Question,
        Stop,
        Warning
    }

    // '<System.Diagnostics.DebuggerStepThrough()>
    public enum DialogResult
    {
        Abort,
        Cancel,
        Ignore,
        No,
        None,
        OK,
        Retry,
        Yes
    }

}