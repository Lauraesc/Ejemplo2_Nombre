using Microsoft.EntityFrameworkCore;
using API_Sat_2023II.DAL.Entities;

namespace API_Sat_2023II.DAL.Entities
{
    public class DataBaseContext : DbContext
    {
        /*Con este constructor me podré conectar a la BD, me brinda unas opciones de configuración que ya
        están definidas internamente*/
        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {



        }

        //índices
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder es un objeto que va a construir el modelo
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().HasIndex(c => c.Name).IsUnique(); /*Este índice es para evitar nombres duplicados de países */
            
            //índice para el Estado
            modelBuilder.Entity<State>().HasIndex("Name", "CountryId").IsUnique();
            /*hay dos parámetros, el primero es la propiedad de la que se quiere evitar duplicidad y 
             el segundo es la condición de que evite la duplicidad únicamente si se está
            bajo el mismo Id del país.*/
        }

        //un DbSet se usa para convertir las entidades lógicas en entidades de tablas.
        public DbSet<Country> Countries { get; set; } /*Esta línea la toma la clase Country y la mapea en SQL Server para 
        crear una tabla llamada COUNTRIES (siempre se pluralizan las tablas)*/

        public DbSet<State> States { get; set; }

        //Por cada nueva entidad que creo, debo crearle su DbSet para que funcione, pq si no, no va a funcionar




        /*Índices agrupados: son los de PK, los que permiten tener una búsqueda interna de la BD.
          Índices no agrupados: son los que permiten buscar en la BD un valor en específico de una tabla.
         */
    }
}
