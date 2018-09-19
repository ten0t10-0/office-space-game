﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Data;
using System.Data.SqlClient;

public enum DBAuthenticationMode { Windows, SQLServer }

public class DatabaseManager : MonoBehaviour
{
    public string ServerName;
    public string NetworkLibrary;
    public string DatabaseName;
    public string Username;
    public string Password;

    public DBAuthenticationMode AuthenticationMode = DBAuthenticationMode.SQLServer;

    #region Connection
    private SqlConnection GetConnection()
    {
        string connectionString = null;

        switch (AuthenticationMode)
        {
            case DBAuthenticationMode.SQLServer:
                connectionString = string.Format("Data Source = {0}; Network Library = {1}; Initial Catalog = {2}; User id = {3}; Password = {4};", ServerName, NetworkLibrary, DatabaseName, Username, Password);
                break;

            case DBAuthenticationMode.Windows:
                connectionString = string.Format("Data Source = {0}; Initial Catalog = {1}; User id = {2}; Password = {3};", ServerName, DatabaseName, Username, Password);
                break;
        }

        if (connectionString != null)
            return new SqlConnection(connectionString);
        else
            return null;
    }
    #endregion

    #region General SQL
    private DataTable Query(string sql, Dictionary<string, object> pars = null, CommandType cmdType = CommandType.StoredProcedure)
    {
        DataTable result = null;

        SqlConnection conn = GetConnection();

        SqlCommand cmd = new SqlCommand(sql, conn)
        {
            CommandType = cmdType
        };

        if (pars != null)
        {
            foreach (KeyValuePair<string, object> param in pars)
            {
                cmd.Parameters.AddWithValue(param.Key, param.Value);
            }
        }

        SqlDataAdapter adapter = new SqlDataAdapter(cmd);

        try
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            result = new DataTable();
            adapter.Fill(result);
        }
        catch (SqlException ex)
        {
            Debug.Log("*SQL ERROR: " + ex.Message);
        }

        if (conn.State == ConnectionState.Open)
            conn.Close();

        return result;
    }

    private bool NonQuery(string sql, Dictionary<string, object> pars, CommandType cmdType = CommandType.StoredProcedure)
    {
        int result = 0;

        SqlConnection conn = GetConnection();

        SqlCommand cmd = new SqlCommand(sql, conn)
        {
            CommandType = cmdType
        };

        foreach (KeyValuePair<string, object> param in pars)
        {
            cmd.Parameters.AddWithValue(param.Key, param.Value);
        }

        try
        {
            if (conn.State == ConnectionState.Closed)
                conn.Open();

            result = cmd.ExecuteNonQuery();
        }
        catch (SqlException ex)
        {
            Debug.Log("*SQL ERROR: " + ex.Message);
        }

        if (conn.State == ConnectionState.Open)
            conn.Close();

        return result > 0;
    }
    #endregion

    #region Methods
    public List<DBPlayer> GetHighScores()
    {
        List<DBPlayer> highScores = new List<DBPlayer>();

        DataTable dt = Query("usp_GetHighScores");

        if (dt != null)
        {
            foreach (DataRow row in dt.Rows)
            {
                string name = Convert.ToString(row[0]);
                int score = Convert.ToInt32(row[1]);

                DBPlayer player = new DBPlayer()
                {
                    Username = name,
                    Experience = score
                };

                highScores.Add(player);
            }
        }
        else
        {
            Debug.Log("*SQL - GetHighScores: Table is empty.");
        }

        return highScores;
    }

    public bool CheckPlayerUsername(string username)
    {
        Dictionary<string, object> pars = new Dictionary<string, object>()
        {
            { "@username", username }
        };

        return Convert.ToInt32(Query("usp_CheckPlayerUsername", pars).Rows[0][0]) == 1;
    }

    public bool AddPlayer(DBPlayer player)
    {
        Dictionary<string, object> pars = new Dictionary<string, object>()
        {
            { "@username", player.Username },
            { "@experience", player.Experience },
            { "@money", player.Money }
        };

        return NonQuery("usp_AddPlayer", pars);
    }

    public bool UpdatePlayer(DBPlayer player)
    {
        Dictionary<string, object> pars = new Dictionary<string, object>()
        {
            { "@username", player.Username },
            { "@experience", player.Experience },
            { "@money", player.Money }
        };

        return NonQuery("usp_UpdatePlayer", pars);
    }
    #endregion
}