using EasyClap.Seneca.Common.LevelManagement.Levels;
using JetBrains.Annotations;

namespace EasyClap.Seneca.Common.LevelManagement.Databases
{
    /// <summary>
    /// Represents a type-free level database.
    /// You need to access the specific level database itself for specialized operations.
    /// </summary>
    [PublicAPI]
    public interface ILevelDatabase
    {
        /// <summary>
        /// The total amount of levels registered on this database.
        /// </summary>
        int LevelCount { get; }
        
        ILevel this[int index] { get; }
    }
}
