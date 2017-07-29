using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GPD.ServiceEntities.BaseEntities
{
    [DataContract(Namespace = "http://www.gpd.com", Name = "LineChart")]
    public class LineChartDTO
    {
        #region Constr
        public LineChartDTO() : base() {
            Dates = new List<string>();
            Values = new List<int>();
        }
        #endregion Constr

        [DataMember(Name = "name", Order = 1)]
        public string Name;
        
        [DataMember(Name = "dates", Order = 2)]
        public List<string> Dates;

        [DataMember(Name = "values", Order = 3)]
        public List<int> Values;
    }

    [DataContract(Namespace = "http://www.gpd.com", Name = "ChartData")]
    public class ChartDTO
    {
        #region Constr
        public ChartDTO() : base()
        {
            Dates = new List<string>();
            Lines = new List<LinesDTO>();
        }
        #endregion Constr

        [DataMember(Name = "dates", Order = 1)]
        public List<string> Dates;

        [DataMember(Name = "lines", Order = 3)]
        public List<LinesDTO> Lines;
    }

    [DataContract(Namespace = "http://www.gpd.com", Name = "LineData")]
    public class LinesDTO
    {
        #region Constr
        public LinesDTO() : base()
        {
            Values = new List<int>();
        }
        #endregion Constr

        [DataMember(Name = "name", Order = 1)]
        public string Name;
        
        [DataMember(Name = "values", Order = 2)]
        public List<int> Values;
    }
}
