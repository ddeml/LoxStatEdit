using System;
using System.Collections.Generic;

namespace LoxStatEdit
{
    public class LoxStatProblem
    {
        public readonly LoxStatObject Source;
        public readonly object Problem;

        public LoxStatProblem(LoxStatObject source, object problem)
        {
            Source = source;
            Problem = problem;
        }

        public static IList<LoxStatProblem> GetProblems(
            IEnumerable<LoxStatObject> loxStatObjects)
        {
            IList<LoxStatProblem> list = new List<LoxStatProblem>();
            foreach(var loxStatObject in loxStatObjects)
                loxStatObject.AddProblems(list);
            return list;
        }

        public static IList<LoxStatProblem> GetProblems(
            params LoxStatObject[] loxStatObjects)
        {
            return GetProblems((IEnumerable<LoxStatObject>)loxStatObjects);
        }

        public override string ToString()
        {
            try
            {
                return string.Format("{0}: {1}", Source, Problem);
            }
            catch(Exception ex)
            {
                return ex.ToString();
            }
        }
    }
}
