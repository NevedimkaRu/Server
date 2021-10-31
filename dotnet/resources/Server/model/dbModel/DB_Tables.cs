﻿using GTANetworkAPI;
using MySqlConnector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Server.model
{
    public class DB_Tables
    {
        public int Id { get; set; }

        public void SetId(int id)
        {
            string tbname = getDBTableName();
            string sql = $"select * from `{tbname.ToLower()}` where `id` = {id}";
            DataTable dt = MySql.QueryRead(sql);

            LoadByDataRow(dt.Rows[0]);
        }

        public int Insert()
        {
            string tbname = this.GetType().Name;
            return this.Insert(tbname);
        }
        
        private int Insert(String tbname)
        {
            Dictionary<string, object> props = GetParamsForQuery();

            string fieldsStr = "";
            string parametersStr = "";

            foreach (string f in props.Keys) {
                fieldsStr += "`" + f + "`,";
                parametersStr += "@" + f + ",";
            }
            fieldsStr = fieldsStr.Remove(fieldsStr.Length - 1, 1);
            parametersStr = parametersStr.Remove(parametersStr.Length - 1, 1);

            string sql = $"insert into `{tbname.ToLower()}` " +
                "(" + fieldsStr + ") " +
                "values (" + parametersStr + "); select last_insert_id()";

            DataTable dt = MySql.QueryRead(sql, props);
            this.Id = Convert.ToInt32(dt.Rows[0].ItemArray[0]);

            return this.Id;
        }

        public void Update()
        {
            string tbname = getDBTableName();

            Dictionary<string, object> props = GetParamsForQuery();
            string valuesParamStr = GetParamStrForAll();

            string sql = $"update `{tbname.ToLower()}` " +
            $"set {valuesParamStr}" +
            $" where id = {this.Id}";
            MySql.Query(sql, props);

        }

        public async Task UpdateAsync()
        {
            string tbname = getDBTableName();

            Dictionary<string, object> props = GetParamsForQuery();
            string valuesParamStr = GetParamStrForAll();

            string sql = $"update `{tbname.ToLower()}` " +
            $"set {valuesParamStr}" +
            $" where id = {this.Id}";
            await MySql.QueryAsync(sql, props);

        }

        public void Update(string fields)
        {
            string tbname = getDBTableName();

            Dictionary<string, object> props = GetParamsForQuery();
            string valuesParamStr = GetParamStrForFields(fields);

            string sql = $"update `{tbname.ToLower()}` " +
            $"set {valuesParamStr}" +
            $" where id = {this.Id}";
            MySql.Query(sql, props);

        }

        public async Task UpdateAsync(string fields)
        {
            string tbname = getDBTableName();

            Dictionary<string, object> props = GetParamsForQuery();
            string valuesParamStr = GetParamStrForFields(fields);

            string sql = $"update `{tbname.ToLower()}` " +
            $"set {valuesParamStr}" +
            $" where id = {this.Id}";
            await MySql.QueryAsync(sql, props);
        }

        public bool LoadByOtherId(string field, int fldId) 
        {

            string sql = $"select * from {getDBTableName()} where `{field}` = {fldId}";
            DataTable dt = MySql.QueryRead(sql);

            if (dt == null || dt.Rows.Count == 0)
            {
                return false;
            }

            LoadByDataRow(dt.Rows[0]);
            return true;
        }


        public async Task<bool> LoadByOtherFieldAsync(string field, string value)
        {
            string sql = $"select * from `{getDBTableName()}` where `{field}` = '{value}'";
            DataTable dt = await MySql.QueryReadAsync(sql);

            if (dt == null || dt.Rows.Count == 0)
            {
                return false;
            }

            LoadByDataRow(dt.Rows[0]);
            return true;
        }

        public void Delete()
        {
            if (this.Id == 0) return;
            Delete(this.Id);
        }

        public void Delete(int Id)
        {
            string sql = $"delete from `{getDBTableName()}` where Id = {Id}";
            MySql.Query(sql);
        }

        public void LoadByDataRow(DataRow row)
        {
            foreach (var obj in this.GetType().GetProperties())
            {
                if (!IsDbTable(obj.Name)) continue;
                if (!row.Table.Columns.Contains(obj.Name)) continue;
                if (row.Table.Columns.Contains(obj.Name))
                {
                    if (obj.PropertyType == typeof(Vector3))
                    {
                        obj.SetValue(this, JsonConvert.DeserializeObject<Vector3>(row[obj.Name].ToString()));
                    }
                    else if (obj.PropertyType == typeof(Color))
                    {
                        obj.SetValue(this, JsonConvert.DeserializeObject<Color>(row[obj.Name].ToString()));
                    }
                    else if (obj.PropertyType == typeof(List<Vector3>))
                    {
                        obj.SetValue(this, utils.Parser.ParseToListVector3(row[obj.Name].ToString()));
                    }
                    else if (obj.PropertyType == typeof(bool))
                    {
                        obj.SetValue(this, Convert.ToBoolean(row[obj.Name]));
                    }
                    else if (obj.PropertyType == typeof(UInt64))
                    {
                        obj.SetValue(this, Convert.ToUInt64(row[obj.Name]));
                    }
                    else if (obj.PropertyType == typeof(UInt32))
                    {
                        obj.SetValue(this, Convert.ToUInt32(row[obj.Name]));
                    }
                    else if (obj.PropertyType == typeof(DateTime))
                    {
                        obj.SetValue(this, Convert.ToDateTime(row[obj.Name]));
                    }
                    else
                    {
                        obj.SetValue(this, row[obj.Name]);
                    }
                }
            }
        }
        //
        private Dictionary<string, object> GetParamsForQuery()
        {
            Dictionary<string, object> props = new Dictionary<string, object>();

            string valuesParamStr = "";
            foreach (var obj in this.GetType().GetProperties())
            {
                string fldName = obj.Name;
                object value = obj.GetValue(this, null);
                if (fldName != "Id" && value != null)
                {
                    valuesParamStr += "`" + fldName + "` = @" + fldName + ",";
                    if (obj.PropertyType == typeof(List<Vector3>))
                    {
                        List<Vector3> val = (List<Vector3>)value;
                        props.Add(fldName, utils.Parser.ParseFromListVector3(val));
                    }
                    else if (obj.PropertyType == typeof(Vector3) || obj.PropertyType == typeof(Color))
                    {
                        props.Add(fldName, JsonConvert.SerializeObject(value));
                    }
                    else
                    {
                        props.Add(fldName, value);
                    }

                }
            }
            valuesParamStr = valuesParamStr.Remove(valuesParamStr.Length - 1, 1);
            return props;
        }

        private string GetParamStrForAll()
        {
            string valuesParamStr = "";
            foreach (var obj in this.GetType().GetProperties())
            {
                string fldName = obj.Name;
                object value = obj.GetValue(this, null);
                if (fldName != "Id" && value != null)
                {
                    valuesParamStr += "`" + fldName + "` = @" + fldName + ",";
                }
            }
            valuesParamStr = valuesParamStr.Remove(valuesParamStr.Length - 1, 1);
            return valuesParamStr;
        }

        private string GetParamStrForFields(string fields)
        {
            string[] flds = fields.Split(",");
            string valuesParamStr = "";

            foreach (string f in flds)
            {
                valuesParamStr += "`" + f + "` = @" + f + ",";
            }

            valuesParamStr = valuesParamStr.Remove(valuesParamStr.Length - 1, 1);

            return valuesParamStr;
        }

        private string getDBTableName()
        {
            return this.GetType().Name.ToLower();
        }

        private bool IsDbTable(string fldName)
        {
            if (fldName.IndexOf("_") == 0)
            {
                return false;
            }
            return true;
        }
    }
}
