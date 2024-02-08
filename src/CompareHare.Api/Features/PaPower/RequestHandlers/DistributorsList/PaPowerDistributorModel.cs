namespace CompareHare.Api.Features.PaPower.RequestHandlers.DistributorsList
{
    public class PaPowerDistributorModel
    {
        public PaPowerDistributorModel() {
            nid = "";
            name = "";
            phone = "";
            website = "";
            rates = new List<PaPowerDistributorRateModel>();
        }

        public int id { get; set; }
        public string nid { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string website { get; set; }

        public List<PaPowerDistributorRateModel> rates { get; set; }

        public class PaPowerDistributorRateModel
        {
            public PaPowerDistributorRateModel()
            {
                rateSchedule = "";
                futureRateTimeFrame = "";
                lastUpdatedDate = "";
            }

            public int id { get; set; }
            public string rateSchedule { get; set; }
            public float rate { get; set; }
            public float futureRate { get; set; }
            public string futureRateTimeFrame { get; set; }
            public string lastUpdatedDate { get; set; }
        }
    }


}
