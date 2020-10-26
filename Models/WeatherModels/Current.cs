namespace Models.WeatherModels
{
    public class Current
    {
        public float temp_c { get; set; }
        public float temp_f { get; set; }
        public Condition condition { get; set; }
        public int humidity { get; set; }
    }

}
