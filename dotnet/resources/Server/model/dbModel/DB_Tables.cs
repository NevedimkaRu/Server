using GTANetworkAPI;
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
            string tbname = this.GetType().Name;
            string sql = $"select * from `{tbname.ToLower()}` where `id` = {id}";
            DataTable dt = MySql.QueryRead(sql);

            foreach (var obj in this.GetType().GetProperties())
            {
                //if (!IsDbTable(obj.Name)) continue;
                if (dt.Columns.Contains(obj.Name))
                {
                    if (obj.PropertyType.Name == "Vector3") 
                    {
                        obj.SetValue(this, JsonConvert.DeserializeObject<Vector3>(dt.Rows[0][obj.Name].ToString()));
                    } 
                    else if(obj.PropertyType.Name == "List`1")
                    {
                        obj.SetValue(this, utils.Parser.ParseToListVector3(dt.Rows[0][obj.Name].ToString()));
                    }
                    else
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
                if (fldName !=  "Id" && value != null && IsDbTable(fldName)) 
                {
                    if(obj.PropertyType.Name == "List`1")
                    {
                        List<Vector3> val = (List<Vector3>)value;
                        props.Add(fldName, utils.Parser.ParseFromListVector3(val));
                    }
                    else
                    {
                        props.Add(fldName, value);
                    }
                    
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

            string sql = $"insert into `{tbname.ToLower()}` " +
                "(" + fieldsStr + ") " +
                "values (" + parametersStr + "); select last_insert_id()";

            DataTable dt = MySql.QueryRead(sql, props);
            this.Id = Convert.ToInt32(dt.Rows[0].ItemArray[0]);

            return this.Id;
        }

        public void Update()
        {
            string tbname = this.GetType().Name;

            Dictionary<string, object> props = new Dictionary<string, object>();

            string valuesParamStr = "";
            foreach (var obj in this.GetType().GetProperties())
            {
                string fldName = obj.Name;
                object value = obj.GetValue(this, null);
                if (fldName != "Id" && value != null)
                {
                    valuesParamStr += "`" + fldName + "` = @" + fldName + ",";
                    if (obj.PropertyType.Name == "List`1")
                    {
                        List<Vector3> val = (List<Vector3>)value;
                        props.Add(fldName, utils.Parser.ParseFromListVector3(val));
                    }
                    else
                    {
                        props.Add(fldName, value);
                    }
                    
                }
            }
            valuesParamStr = valuesParamStr.Remove(valuesParamStr.Length - 1, 1);

            string sql = $"update `{tbname.ToLower()}` " +
            $"set {valuesParamStr}" +
            $" where id = {this.Id}";
            MySql.Query(sql, props);

        }

        public void Update(string fields)
        {
            string tbname = this.GetType().Name;

            Dictionary<string, object> props = new Dictionary<string, object>();

            foreach (var obj in this.GetType().GetProperties())
            {
                string fldName = obj.Name;
                object value = obj.GetValue(this, null);
                if (fldName != "Id" && value != null && IsDbTable(fldName))
                {
                    if (obj.PropertyType.Name == "List`1")
                    {
                        List<Vector3> val = (List<Vector3>)value;
                        props.Add(fldName, utils.Parser.ParseFromListVector3(val));
                    }
                    else
                    {
                    props.Add(fldName, value);
                    }
                }
            }
            string[] flds = fields.Split(",");

            string valuesParamStr = "";

            foreach (string f in flds)
            {
                valuesParamStr += "`" + f + "` = @" + f + ",";
            }
            valuesParamStr = valuesParamStr.Remove(valuesParamStr.Length - 1, 1);



            string sql = $"update `{tbname.ToLower()}` " +
            $"set {valuesParamStr}" +
            $" where id = {this.Id}";
            MySql.Query(sql, props);

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
