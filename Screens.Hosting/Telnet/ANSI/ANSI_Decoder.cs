using System;
using System.Collections.Generic;
using System.Text;

namespace Screens.Hosting
{
    public class ANSI_Decoder
    {
        
        public SequenceParser<byte, KeyInfo> Parser { get; }
        public Action<KeyInfo> KeyReady;

        public ANSI_Decoder()
        {
            Parser = new SequenceParser<byte, KeyInfo>(ANSI_Decoder.SequencesDB);
            Parser.FoundMatch = (key) => KeyReady?.Invoke(key);
            Parser.FallBack = DecoderFallback; 
        }

        public void Decode(byte[] data)
        {
            foreach(byte b in data)
            {
                if (b < 0xF0) Parser.Post(b);
            }
                
        }

        #region SEQUENCES DB

        private static Sequence<byte, KeyInfo> seq;

        public static Sequence<byte, KeyInfo> SequencesDB
        {
            get
            {
                if (seq == null)
                {
                    seq = new Sequence<byte, KeyInfo>();

                    seq.AddSequence(new byte[] { 27, 91, 66 }, KeyInfo.Make(SpecialKey.DownArrow));
                    seq.AddSequence(new byte[] { 27, 91, 65 }, KeyInfo.Make(SpecialKey.UpArrow));
                    seq.AddSequence(new byte[] { 27, 91, 67 }, KeyInfo.Make(SpecialKey.RightArrow));
                    seq.AddSequence(new byte[] { 27, 91, 68 }, KeyInfo.Make(SpecialKey.LeftArrow));
                    seq.AddSequence(new byte[] { 27, 79, 80 }, KeyInfo.Make(SpecialKey.F1));
                    seq.AddSequence(new byte[] { 27, 79, 81 }, KeyInfo.Make(SpecialKey.F2));
                    seq.AddSequence(new byte[] { 27, 79, 82 }, KeyInfo.Make(SpecialKey.F3));
                    seq.AddSequence(new byte[] { 27, 79, 83 }, KeyInfo.Make(SpecialKey.F4));
                    seq.AddSequence(new byte[] { 27, 91, 53, 126 }, KeyInfo.Make(SpecialKey.PageUp));
                    seq.AddSequence(new byte[] { 27, 91, 54, 126 }, KeyInfo.Make(SpecialKey.PageDown));
                    seq.AddSequence(new byte[] { 27, 91, 49, 126 }, KeyInfo.Make(SpecialKey.Home));
                    seq.AddSequence(new byte[] { 27, 91, 52, 126 }, KeyInfo.Make(SpecialKey.End));
                    seq.AddSequence(new byte[] { 27, 91, 51, 126 }, KeyInfo.Make(SpecialKey.Delete));
                    seq.AddSequence(new byte[] { 27, 91, 49, 53, 126 }, KeyInfo.Make(SpecialKey.F5));
                    seq.AddSequence(new byte[] { 27, 91, 49, 55, 126 }, KeyInfo.Make(SpecialKey.F6));
                    seq.AddSequence(new byte[] { 27, 91, 49, 56, 126 }, KeyInfo.Make(SpecialKey.F7));
                    seq.AddSequence(new byte[] { 27, 91, 49, 57, 126 }, KeyInfo.Make(SpecialKey.F8));
                    seq.AddSequence(new byte[] { 27, 91, 50, 48, 126 }, KeyInfo.Make(SpecialKey.F9));
                    seq.AddSequence(new byte[] { 27, 91, 50, 49, 126 }, KeyInfo.Make(SpecialKey.F10));
                    seq.AddSequence(new byte[] { 27, 91, 50, 51, 126 }, KeyInfo.Make(SpecialKey.F11));
                    seq.AddSequence(new byte[] { 27, 91, 50, 52, 126 }, KeyInfo.Make(SpecialKey.F12));
                }
                return seq;

                /*
                 *     F1       | SS3 P
                       F2       | SS3 Q
                       F3       | SS3 R
                       F4       | SS3 S
                       F5       | CSI 1 5 ~     49 53
                       F6       | CSI 1 7 ~     49 54
                       F7       | CSI 1 8 ~     49 55
                       F8       | CSI 1 9 ~     49 56
                       F9       | CSI 2 0 ~     50 49
                       F10      | CSI 2 1 ~     50 50
                       F11      | CSI 2 3 ~     50 51
                       F12      | CSI 2 4 ~     50 52
                 * 
                 * 
                 * */
            }
        }

        public static KeyInfo DecoderFallback(byte b)
        {
            switch (b)
            {
                case 9:
                    return KeyInfo.Make(SpecialKey.Tab);
                case 127:
                    return KeyInfo.Make(SpecialKey.Backspace);
                case 13:
                    return KeyInfo.Make(SpecialKey.Enter);

                default:
                    //otherwise it's a normal char
                    return KeyInfo.Make((char)b);
            }
        }

        #endregion

    }
}
