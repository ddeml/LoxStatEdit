using System.Collections.Generic;

namespace LoxStatEdit
{
    public abstract class LoxStatObject
    {
        protected void AddProblem(ICollection<LoxStatProblem> list, object problem)
        {
            list.Add(new LoxStatProblem(this, problem));
        }

        public abstract void AddProblems(ICollection<LoxStatProblem> collection);
    }
}
