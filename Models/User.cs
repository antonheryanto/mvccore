namespace MvcCore {
    public class User 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        
        public override string ToString() => $"{Title} {FullName}";
		public bool IsAnonymous => Id == 0;
    }
}