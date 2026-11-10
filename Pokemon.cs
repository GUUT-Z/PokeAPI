namespace PokeAPI
{
    public class Pokemon
    {
        public int id { get; set; }
        public string name { get; set; }
        public int base_experience { get; set; }
        public int height { get; set; }
        public AbilityEntry[] abilities { get; set; }
        public TypeEntry[] types { get; set; }
        public Sprites sprites { get; set; }

        public override string ToString()
        {
            return "id : " + this.id
                + " nom : " + this.name
                + " expérience de base : " + this.base_experience
                + " taille : " + this.height;
        }
    }
}