namespace com.bosscastlestudios.farming
{
    public class PlayerResourceRepository
    {
        private static PlayerResourceRepository _inst;
        public static PlayerResourceRepository Instance => _inst ?? (_inst = new PlayerResourceRepository());

        public int maxCapacity = 1000;
        public int currentAmount = 0;
    }
}