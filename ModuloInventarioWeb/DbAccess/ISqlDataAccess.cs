﻿namespace ModuloInventarioWeb.DbAccess
{
    public interface ISqlDataAccess
    {
        Task<IEnumerable<T>> LoadData<T, U>(string storedProcedure, U parameters, string connectionId = "EddyConexion");
        Task SaveData<T>(string storedProcedure, T parameters, string connectionId = "EddyConexion");
    }
}