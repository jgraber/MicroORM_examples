using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroORM_PetaPoco
{
    public class SemanticBook : Book
    {
        public float Score { get; set; }

        public override string ToString()
        {
            return String.Format("{0}: {1}", Score, base.ToString());
        }
    }
}
