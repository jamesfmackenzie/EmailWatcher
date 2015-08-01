namespace EmailWatcher.Public
{
    public class EmailWatcherOptions
    {
        public string Host { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        private int? _timeBetweenRefreshes;
        public int? TimeBetweenRefreshes
        {
            get
            {
                if (_timeBetweenRefreshes == null)
                {
                    return 30;
                }
                
                return _timeBetweenRefreshes;
            }
            set { _timeBetweenRefreshes = value; }
        }
    }
}
