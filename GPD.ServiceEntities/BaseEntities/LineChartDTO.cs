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

        [DataMember(Name = "color", Order = 2)]
        public string Color;

        [DataMember(Name = "dates", Order = 3)]
        public List<string> Dates;

        [DataMember(Name = "values", Order = 4)]
        public List<int> Values;
    }
}
