﻿namespace Products_lost_helps.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Apellido { get; set; }
        public string? Correo { get; set; }
        public string? Clave { get; set; }
        public Rol IdRol { get; set; }
    }
}
