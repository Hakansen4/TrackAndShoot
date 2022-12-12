using System;

namespace EasyClap.Seneca.Common.Submodules.EasyClapFramework.Scripts.Core
{
    public class PossibleInfiniteLoopException : Exception
    {
        
        public PossibleInfiniteLoopException()
            : base("Possible infinite loop!")
        { }
        
        public PossibleInfiniteLoopException(int loopCount)
            : base($"Possible infinite loop! Loop is not finished in {loopCount} cycle.")
        { }
    }
}