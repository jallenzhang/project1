using UnityEngine;
using System.Collections;
using Mono.Data.Sqlite;
using System;

public class DbAccess {
	private SqliteConnection dbConnection;
	
	private SqliteCommand dbCommand;
	
	private SqliteDataReader reader;
	
	public DbAccess (string connectionString)
	{
		OpenDB (connectionString);
	}

	public DbAccess ()
	{
		
	}
	
	public void OpenDB (string connectionString)
	{
		try
		{
			dbConnection = new SqliteConnection (connectionString);
			
			dbConnection.Open ();
			
            //Debug.Log ("Connected to db " + connectionString);
		}
		catch(Exception e)
		{
			string temp1 = e.ToString();
			Debug.Log(temp1);
		}
		
	}
	
	public void CloseSqlConnection ()
	{
		
		if (dbCommand != null) {
			
			dbCommand.Dispose ();
			
		}
		
		dbCommand = null;
		
		if (reader != null) {
			
			reader.Dispose ();
			
		}
		
		reader = null;
		
		if (dbConnection != null) {
			
			dbConnection.Close ();
			
		}
		
		dbConnection = null;
		
		Debug.Log ("Disconnected from db.");
		
	}
	
	public SqliteDataReader ExecuteQuery (string sqlQuery)
	{
	    try
        {
            dbCommand = dbConnection.CreateCommand();

            dbCommand.CommandText = sqlQuery;

            reader = dbCommand.ExecuteReader();
        }
		catch(Exception ex)
        {
            UnityEngine.Debug.logger.Log("Exception is: " + ex.Message);
            UnityEngine.Debug.logger.Log(ex.StackTrace);
        }
        return reader;
	}

    public void ExecuteUpdateQuery(string sqlQuery)
    {
        try
        {
            dbCommand = dbConnection.CreateCommand();

            dbCommand.CommandText = sqlQuery;

            dbCommand.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.logger.Log("Exception is: " + ex.Message);
            UnityEngine.Debug.logger.Log(ex.StackTrace);
        }
    }

    public object ExecuteInsertQuery(string sqlQuery)
    {
        object ret = null;
        try
        {
            dbCommand = dbConnection.CreateCommand();

            dbCommand.CommandText = sqlQuery;

            ret = dbCommand.ExecuteScalar();
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.logger.Log("Exception is: " + ex.Message);
            UnityEngine.Debug.logger.Log(ex.StackTrace);
        }
        return ret;
    }
	
	public SqliteDataReader ReadFullTable (string tableName)
	{
		
		string query = "SELECT * FROM " + tableName;
		
		return ExecuteQuery (query);
		
	}
	
	public object InsertInto (string tableName, string[] values)
	{
		
		string query = "INSERT INTO " + tableName + " VALUES (" + values[0];
		
		for (int i = 1; i < values.Length; ++i) {
			
			query += ", " + values[i];
			
		}

        query += "); select last_insert_rowid();";

        Debug.logger.Log(query);
        return ExecuteInsertQuery(query);
		
	}
	
	public SqliteDataReader UpdateInto (string tableName, string []cols,string []colsvalues,string selectkey,string selectvalue)
	{
		
		string query = "UPDATE "+tableName+" SET "+cols[0]+" = '"+colsvalues[0];
		
		for (int i = 1; i < colsvalues.Length; ++i) {
			
			query += "', " +cols[i]+" ='"+ colsvalues[i];
		}
		
		query += "' WHERE "+selectkey+" = "+selectvalue+" ";
		
		return ExecuteQuery (query);
	}
	
	public SqliteDataReader Delete(string tableName,string []cols,string []colsvalues)
	{
		string query = "DELETE FROM "+tableName + " WHERE " +cols[0] +" = " + colsvalues[0];
		
		for (int i = 1; i < colsvalues.Length; ++i) {
			
			query += " or " +cols[i]+" = "+ colsvalues[i];
		}
		Debug.Log(query);
		return ExecuteQuery (query);
	}
	
	public object InsertIntoSpecific (string tableName, string[] cols, string[] values)
	{
		
		if (cols.Length != values.Length) {
			
			throw new SqliteException ("columns.Length != values.Length");
			
		}
		
		string query = "INSERT INTO " + tableName + "(" + cols[0];
		
		for (int i = 1; i < cols.Length; ++i) {
			
			query += ", " + cols[i];
			
		}
		
		query += ") VALUES ('" + values[0];
		
		for (int i = 1; i < values.Length; ++i) {
			
			query += "', '" + values[i];
			
		}

        query += "'); select last_insert_rowid();";

        Debug.logger.Log(query);
        return ExecuteInsertQuery(query);
		
	}
	
	public SqliteDataReader DeleteContents (string tableName)
	{
		
		string query = "DELETE FROM " + tableName;
		
		return ExecuteQuery (query);
		
	}
	
	public SqliteDataReader CreateTable (string name, string[] col, string[] colType)
	{
		
		if (col.Length != colType.Length) {
			
			throw new SqliteException ("columns.Length != colType.Length");
			
		}
		
		string query = "CREATE TABLE IF NOT EXISTS " + name + " (" + col[0] + " " + colType[0];
		
		for (int i = 1; i < col.Length; ++i) {
			
			query += ", " + col[i] + " " + colType[i];
			
		}
		
		query += ")";

        //UnityEngine.Debug.logger.Log(query);
		return ExecuteQuery (query);
		
	}
	
	public SqliteDataReader SelectWhere (string tableName, string[] items, string[] col, string[] operation, string[] values)
	{
		
		if (col.Length != operation.Length || operation.Length != values.Length) {
			
			throw new SqliteException ("col.Length != operation.Length != values.Length");
			
		}
		
		string query = "SELECT " + items[0];
		
		for (int i = 1; i < items.Length; ++i) {
			
			query += ", " + items[i];
			
		}
		
		query += " FROM " + tableName + " WHERE " + col[0] + operation[0] + "'" + values[0] + "' ";
		
		for (int i = 1; i < col.Length; ++i) {
			
			query += " AND " + col[i] + operation[i] + "'" + values[0] + "' ";
			
		}
		
		return ExecuteQuery (query);
		
	}
}
