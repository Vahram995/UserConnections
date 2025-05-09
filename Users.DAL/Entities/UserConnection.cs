namespace Users.DAL.Entities
{
    public class UserConnection
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public string IpAddress { get; set; }
        public DateTime ConnectedAt { get; set; }
    }
}