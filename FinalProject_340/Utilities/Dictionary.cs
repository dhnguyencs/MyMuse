namespace FinalProject_340.Utilities
{
    public static class DictionariesExt
    {
        public static Dictionary<string, string> toLowerCaseKey(this Dictionary<string, string> thisDict)
        {
            Dictionary<string, string> lo_case_conditions = new Dictionary<string, string>();

            foreach (string condition in thisDict.Keys)
                lo_case_conditions.Add(condition.ToLower(), thisDict[condition]);

            return lo_case_conditions;
        }
    }
}
