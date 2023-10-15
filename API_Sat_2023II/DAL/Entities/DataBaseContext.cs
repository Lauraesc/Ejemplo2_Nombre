﻿using Microsoft.EntityFrameworkCore;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder es un objeto que va a construir el modelo
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().HasIndex(c => c.Name).IsUnique(); /*Este índice es para evitar nombres duplicados
            decimal países */
        }

        //un DbSet se usa para convertir las entidades lógicas en entidades de tablas.
        public DbSet<Country> Countries { get; set; } /*Esta línea la toma la clase Country y la mapea en SQL Server para 
        crear una tabla llamada COUNTRIES (siempre se pluralizan las tablas)*/

        /*Índices agrupados: son los de PK, los que permiten tener una búsqueda interna de la BD.
          Índices no agrupados: son los que permiten buscar en la BD un valor en específico de una tabla.
         */
    }
}
