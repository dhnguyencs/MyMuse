namespace FinalProject_340.Models
{
    public static class DictionariesExt
    {
        public static Dictionary<string, string> keysToLower(this Dictionary<String, String> thisDict)
        {
            Dictionary<String, String> lo_case_conditions = new Dictionary<string, string>();
            
            foreach (string condition in thisDict.Keys)
                lo_case_conditions.Add(condition.ToLower(), thisDict[condition]);

            return lo_case_conditions;
        }
    }
}
