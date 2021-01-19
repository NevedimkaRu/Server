﻿using GTANetworkAPI;
using MySqlConnector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Server.model
{
    public class DB_Tables
    {
        public int Id { get; set; }

        public void SetId(int id)
        {
            //todo необходимо согласование, класс должен иметь такое же название что и теблица БД
            string tbname = "teleports";
            string sql = $"select * from `{tbname}` where `id` = {id}";
            DataTable dt = MySql.QueryRead(sql);

            foreach (var obj in this.GetType().GetProperties())
            {
                if (dt.Columns.IndexOf(obj.Name) != 1)
                {
                    if (obj.PropertyType.Name == "Vector3") 
                    {
                        obj.SetValue(this, JsonConvert.DeserializeObject<Vector3>(dt.Rows[0][obj.Name].ToString()));
                    } else
                    {
                        obj.SetValue(this, dt.Rows[0][obj.Name]);
                    }
                }
            }
        }
        public int Insert()
        {
            string tbname = this.GetType().Name;
            return this.Insert(tbname);
        }
        
        public int Insert(String tbname)
        {
            Dictionary<string, object> props = new Dictionary<string, object>();

            foreach (var obj in this.GetType().GetProperties())
            {
                string fldName = obj.Name;
                object value = obj.GetValue(this, null);
                if (fldName !=  "Id" && value != null) {
                    props.Add(fldName, value);
                }
            }

            string fieldsStr = "";
            string parametersStr = "";

            foreach (string f in props.Keys) {
                fieldsStr += "`" + f + "`,";
                parametersStr += "@" + f + ",";
            }
            fieldsStr = fieldsStr.Remove(fieldsStr.Length - 1, 1);
            parametersStr = parametersStr.Remove(parametersStr.Length - 1, 1);

            string sql = $"insert into `{tbname}` " +
                "(" + fieldsStr + ") " +
                "values (" + parametersStr + "); select last_insert_id()";

            DataTable dt = MySql.QueryRead(sql, props);
            this.Id = Convert.ToInt32(dt.Rows[0].ItemArray[0]);

            return this.Id;
        }

        public void Update(string fields)
        {
            //todo убрать после того как модели будут переименованны в соответсвии с названиями таблиц
            string tbname = "teleports";

            Dictionary<string, object> props = new Dictionary<string, object>();

            foreach (var obj in this.GetType().GetProperties())
            {
                string fldName = obj.Name;
                object value = obj.GetValue(this, null);
                if (fldName != "Id" && value != null)
                {
                    props.Add(fldName, value);
                }
            }
            string[] flds = fields.Split(",");

            string valuesParamStr = "";

            foreach (string f in flds)
            {
                valuesParamStr += "`" + f + "` = @" + f + ",";
            }
            valuesParamStr = valuesParamStr.Remove(valuesParamStr.Length - 1, 1);



            string sql = $"update `{tbname}` " +
            $"set {valuesParamStr}" +
            $" where id = {this.Id}";
            MySql.Query(sql, props);

        }
    }
}
