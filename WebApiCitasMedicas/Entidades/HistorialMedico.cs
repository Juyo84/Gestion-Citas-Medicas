namespace WebApiCitasMedicas.Entidades
{
    public class HistorialMedico
    {

        public int Id { get; set; }
        
        public string Descripcion { get; set; }

        public int PacienteId { get; set; }

    }
}
