using System;
using System.Collections.Generic;

namespace OpenDTDC.Interface
{
    public interface IControlScript
    {
        void Update(long time, List<Tuple<string, int>> dataList);
    }
}
