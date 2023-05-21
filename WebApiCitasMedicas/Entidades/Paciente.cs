namespace WebApiCitasMedicas.Entidades
{
    public class Paciente
    {

        public int Id { get; set; }

        public string Nombre { get; set; }
        
        public string Apellido_Paterno { get; set; }

        public string Apellido_Materno { get; set; }

        public float Peso { get; set; }

        public float Altura { get; set; }


    }
}
