using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.Common;
using System.Configuration;
using System.Data;
using System.IO;

namespace test.EntityHelper
{
    public class EntityHelper
    {

		public static DataSet GetDataSet(string strsql)
		{
			//string strcon = "server=你的IP;uid=用户名;pwd=密码;database=数据库名";
			string strcon = ConfigurationManager.AppSettings["conn"].ToString();
			SqlConnection con = new SqlConnection(strcon);
			DataSet ds = new DataSet();
			try
			{
				SqlDataAdapter DA = new SqlDataAdapter(strsql, con);
				DA.Fill(ds);
			}
			catch (SqlException E)
			{
				throw new Exception(E.Message);
			}
			con.Close();//关闭数据库

			return ds;
		}


		public  static void GenerateEF(string path,string tb_name)
        {

            string txt = string.Format(EfModel(), tb_name, GetTableInfo(tb_name));

            SaveFile(path, tb_name, txt);
          
            
        }


        public static void SaveFile(string path,string filename,string txt)
        {
             
            if (!Directory.Exists(path))
            { 
                Directory.CreateDirectory(path);

            }

            byte[] conBytes = System.Text.Encoding.UTF8.GetBytes(txt);
            File.WriteAllBytes(path+"\\"+ filename+".cs", conBytes);



        }



        public static string GetTableInfo(string tb_name)
        {
			string s = "";
			DataSet ds= GetDataSet(@"SELECT
                        B.name AS column_name,
                        D.name,
                        B.is_nullable,
                        C.value AS column_description
                        FROM sys.tables A
                        INNER JOIN sys.columns B ON B.object_id = A.object_id
                        LEFT JOIN sys.extended_properties C ON C.major_id = B.object_id AND C.minor_id = B.column_id
                        join systypes D on B.system_type_id=D.xusertype
                        WHERE A.name = '"+ tb_name + "' ");
            foreach(DataRow info in ds.Tables[0].Rows)
            {
                 
                s+="    "+SingleField(info);
            }
            return s;

		}
		public static string SingleField(DataRow fieldinfo)
		{
			string s = @"";

            switch (fieldinfo[1].ToString())
            {
                case "bigint":
                    s = "long";
                    break;

                case "binary":
                    s = "byte[]";
                    break;

                case "bit":
                    s = "bool";
                    break;
                case "char":
                    s = "string";
                    break;
                case "date":
                    s = "DateTime";
                    break;

                case "datetime":
                    s = "DateTime"; break;

                case "datetime2":
                    s = "DateTime"; break;

                case "datetimeoffset":
                    s = "DateTimeOffset"; break;

                case "decimal":
                    s = "decimal"; break;

                case "float":
                    s = "float"; break;

                case "image":
                    s = "byte[]"
                        ; break;
                case "int":
                    s = "int"; break;

                case "money":
                    s = "decimal"; break;

                case "nchar":
                    s = "char"; break;

                case "ntext":
                    s = "string"; break;

                case "numeric":
                    s = "decimal"; break;

                case "nvarchar":
                    s = "string"; break;

                case "real":
                    s = "double"; break;

                case "smalldatetime":
                    s = "DateTime"; break;

                case "smallint":
                    s = "short"; break;

                case "smallmoney":
                    s = "decimal"; break;

                case "text":
                    s = "string"; break;

                case "time":
                    s = "TimeSpan"; break;

                case "timestamp":
                    s = "DateTime"; break;

                case "tinyint":
                    s = "byte"; break;

                case "uniqueidentifier":
                    s = "Guid"; break;

                case "varbinary":
                    s = "byte[]"
; break;
                case "varchar":
                    s = "string"; break;

            }

            s = @"/// <summary>
    /// " + (fieldinfo[3].ToString() == "" ? fieldinfo[0].ToString() : fieldinfo[3].ToString()) + @"
    /// </summary> 
    public " + s;

            if ((bool)fieldinfo[2]) {
                s += "?";
            }
            s += " " + fieldinfo[0]+ " { get; set; }\n";
            return s;

		}
		public static string EfModel()
        {

			return @"/// <summary>
///  {0}   
/// </summary>
public class {0}
{{
	public {0}()
	{{
	}}
{1}
}}";
        }

	}
}
