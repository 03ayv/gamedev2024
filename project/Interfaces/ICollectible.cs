using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project.Interfaces
{
    public interface ICollectible:IGameObject
    {
        void Collect();

        bool IsCollected();
    }
}
