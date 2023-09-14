namespace Audits.Domain.Entities
{
    public class Paged<T>
    {
        public List<T> Results { get; set; }
        public int CurrentPage { get; set; }
        public int SizeLimit { get; set; }
        public int Total { get; set; }

        public int Pages => (int)Math.Ceiling((double)Total / SizeLimit);

        public Paged()
        {
            Results = new List<T>();
        }

        public Paged(List<T> results, int currentPage, int sizeLimit, int total)
        {
            Results = results;
            CurrentPage = currentPage;
            SizeLimit = sizeLimit;
            Total = total;
        }
    }
}
