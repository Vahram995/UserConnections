namespace Users.DAL.Entities
{
    public class UserConnection
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string IpAddress { get; set; }
        public DateTime ConnectedAt { get; set; }

        public User User { get; set; }
    }
}