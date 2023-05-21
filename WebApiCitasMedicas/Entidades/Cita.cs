namespace WebApiCitasMedicas.Entidades
{
    public class Cita
    {

        public int Id { get; set; }

        public int MedicoId { get; set; }

        public int PacienteId { get; set; }

        public DateTime Fecha { get; set; }

    }
}
