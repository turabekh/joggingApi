namespace Models.WeatherModels
{
    public class Day
    {
        public float avgtemp_c { get; set; }
        public float avgtemp_f { get; set; }
        public float avghumidity { get; set; }
        public Condition condition { get; set; }
    }

}
