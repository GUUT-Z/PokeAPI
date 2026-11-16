using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PokeAPI
{
     partial class Form1 : Form
    {
        static WebProxy proxy = new WebProxy
        {
            Address = new Uri("http://192.168.100.1:3128"),
            BypassProxyOnLocal = false,
            UseDefaultCredentials = false,

            Credentials = new NetworkCredential(
        userName: "indiaye",
        password: "RaidirHuelva40")
        };

        static HttpClientHandler httpClientHandler = new HttpClientHandler
        {
            Proxy = proxy
        };

        private static readonly HttpClient client =
            new HttpClient(handler: httpClientHandler, disposeHandler: true);

        public Form1()
        {
            InitializeComponent();
            button1.Click += button1_Click;
        }

        //Elle lance le traitement de la requête des données du Pokémon Pikachu
        private async void button1_Click(object sender, EventArgs e)
        {
            await ProcessRepositories();
        }

        //Prépare les en-têtes HTTP de la requête et lance la récup des données du pokémon Pikachu
        private async Task ProcessRepositories()
        {
            client.DefaultRequestHeaders.Accept.Clear();

            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            client.DefaultRequestHeaders.Add(
                "User-Agent", ".NET Foundation Repository Reporter");

            textBox1.Clear();

            if (int.TryParse(txtPokemonId.Text, out int id) && id > 0)
            {
                await StreamTaskPokemon(id);
            }
            else
            {
                MessageBox.Show("Veuillez saisir un ID de Pokémon valide.");
            }
        }

        // Interroge l'API PokeAPI, récupère toutes les informations de Pikachu au format JSON
        //Puis affiche le résultat dans la TextBox
        private async Task StringTaskPikachu()
        {
            var stringTask = client.GetStringAsync(
                "https://pokeapi.co/api/v2/pokemon/pikachu");

            string msg = await stringTask;

            textBox1.Text = msg;
        }

        // Méthode qui récupère les données de Pikachu sous forme de flux,
        // désérialise le JSON en objet Pokemon et affiche ses informations.
        private async Task StreamTaskPokemon(int id)
        {
            try
            {
                var streamTask = client.GetStreamAsync(
                "https://pokeapi.co/api/v2/pokemon/" + id);

                var pokemon = await JsonSerializer.DeserializeAsync<Pokemon>(
                    await streamTask);

                textBox1.Text = pokemon.ToString();

                // Afficher les capacités
                textBox1.AppendText(Environment.NewLine + Environment.NewLine);
                textBox1.AppendText("CAPACITÉS :" + Environment.NewLine);

                foreach (var ability in pokemon.abilities)
                {
                    textBox1.AppendText("- " + ability.ability.name + Environment.NewLine);
                }

                // Afficher les types
                textBox1.AppendText(Environment.NewLine + "TYPES :" + Environment.NewLine);

                foreach (var type in pokemon.types)
                {
                    textBox1.AppendText("- " + type.type.name + Environment.NewLine);
                }

                byte[] imageBytes = await client.GetByteArrayAsync(
                pokemon.sprites.front_default);

                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    using (Image img = Image.FromStream(ms))
                    {
                        pictureBox1.Image = new Bitmap(img);
                    }
                }
            }
            catch(HttpRequestException)
            {
                MessageBox.Show("Pokémon introuvable ou erreur de connexion à l'API.");
            }
            
        }

        // Récupère les 151 premiers Pokémon depuis l'API
        // et affiche leur nom et leur URL dans la TextBox.
        private async Task StreamTaskMultiple()
        {
            var streamTask = client.GetStreamAsync(
                "https://pokeapi.co/api/v2/pokemon?limit=151");

            var pokemons = await JsonSerializer.DeserializeAsync<LesPokemons>(
                await streamTask);

            foreach (var unResult in pokemons.results)
            {
                textBox1.AppendText(
                    unResult.name + " - " + unResult.url + Environment.NewLine);
            }
        }
     }
}
