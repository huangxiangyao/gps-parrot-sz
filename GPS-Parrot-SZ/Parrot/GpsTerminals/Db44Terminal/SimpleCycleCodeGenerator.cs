using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parrot
{
    public class SimpleCycleCodeGenerator
    {
        private byte value;

        public SimpleCycleCodeGenerator() : this(0) { }

        public SimpleCycleCodeGenerator(byte initialValue)
        {
            value = initialValue;
        }

        public byte LastTrace
        {
            get { return value; }
        }

        public byte NextTrace()
        {
            lock (this)
            {
                return value++;
            }
        }
    }
}
