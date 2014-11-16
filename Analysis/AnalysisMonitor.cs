namespace Analysis
{
    public class AnalysisMonitor
    {
       public delegate void AnalysisOrgNumber(int newAnalysisedCount);
       public static event AnalysisOrgNumber NewAnalysisOrgNumberCount;
        
       public static void InvokeAnalysisOrgNumber(int newAnalysisedCount)
       {
           NewAnalysisOrgNumberCount.Invoke(newAnalysisedCount);
       }
      
    }
}
