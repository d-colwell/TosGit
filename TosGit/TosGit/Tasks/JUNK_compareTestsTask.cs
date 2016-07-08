using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tricentis.TCAddOns;
using Tricentis.TCAPIObjects.Objects;

namespace TosGit.Tasks
{
    class JUNK_compareTestsTask : TCAddOnTask
    {
        public override Type ApplicableType => typeof(TestCase);

        public override string Name => "Compare Tests";
        public override bool IsTaskPossible(TCObject obj)
        {
            return true;
        }
        public override TCObject Execute(TCObject objectToExecuteOn, TCAddOnTaskContext taskContext)
        {
            taskContext.ShowWarningMessage("Select Multiple", "You must select 2 tests to compare");
            return objectToExecuteOn;
        }

        public override TCObject Execute(List<TCObject> objs, TCAddOnTaskContext context)
        {
            if (objs.Count > 2)
            {
                context.ShowErrorMessage("Too Many Tests", "You must select only 2 test cases");
                return null;
            }
            
            return objs.First();
        }
    }
}
