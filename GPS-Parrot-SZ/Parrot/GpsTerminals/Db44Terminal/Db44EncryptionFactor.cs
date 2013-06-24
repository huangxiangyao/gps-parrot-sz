using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parrot.Models.Db44
{
    /// <summary>
    /// DB44加密算法因子。
    /// </summary>
    public class Db44EncryptionFactor
    {
        public Db44EncryptionFactor(uint ia1, uint ic1, uint m1, uint key)
        {
            this.IA1 = ia1;
            this.IC1 = ic1;
            this.M1 = m1;
            this.Key = key;
        }

        public uint IA1 { get; private set; }
        public uint IC1 { get; private set; }
        public uint M1 { get; private set; }
        public uint Key { get; private set; }
    }
}
