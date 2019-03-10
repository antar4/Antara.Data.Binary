using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antara.Data.Binary
{
    /// <summary>
    /// An object that can be serialized by using a BVector as storage
    /// </summary>
    public interface IBinarySerializable
    {
        /// <summary>
        /// Serializes the object to the specified BVector
        /// </summary>
        /// <param name="d">The BVector where the object will be serialized into</param>
        void AddToBinary(BVector d);

        /// <summary>
        /// Deserializes from the specified BVector
        /// </summary>
        /// <param name="d">The BVector where the object is stored</param>
        void ReadFromBinary(BVector d);

        /// <summary>
        /// Serializes the object to a byte[]
        /// </summary>
        /// <returns>The byte[] where the object was serialized into</returns>
        byte[] ToBytes();
    }
}
