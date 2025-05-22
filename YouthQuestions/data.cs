using System.Data;
using System.Text;

namespace YouthQuestions
{
    static public class data
    {
        static DateTime today = DateTime.Today;
        static public DataTable getAllQustion()
        {
            string sql = @$"select * from (select * from Questions where date = '{today}' and Admin = 1 order by date ) order by LoveCount DESC";
            return sqlite.sqlReturnTable(sql);
        }

        internal static DataTable getpdfQuestionsAndAnswersTable()
        {
            string sql = @$"
select * from (
select q.id as 'questionID',q.Question,q.LoveCount,a.answer from Answers a
join Questions q where a.questionID = q.ID 
and q.Admin = 1 and a.admin = 1 and q.date = '{DateTime.Today}'
order by q.id) r
order by r.LoveCount 
";
            return sqlite.sqlReturnTable(sql);
        }

        static public DataTable getAllQustionSuperAdmin()
        {
            string sql = @$"select * from (select * from Questions  order by date ) order by LoveCount DESC";
            return sqlite.sqlReturnTable(sql);
        }
        static public DataTable getAllQustionAdmin()
        {
            string sql = @$"select * from (select * from Questions where date = '{today}' and Admin = 0 order by date ) order by LoveCount DESC";
            return sqlite.sqlReturnTable(sql);
        }
        static public void ApproveQuestionIDs(List<string> IDs)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@$"update Questions set admin = 1 where ");
            foreach (var item in IDs)
            {
                sql.Append($" ID = '{item}' OR");
            }
            sql = sql.Remove(sql.Length - 3, 3);
            sqlite.sqlStatment(sql.ToString());
        }
        static public void ApproveAnswersIDs(List<string> IDs)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@$"update Answers set admin = 1 where ");
            foreach (var item in IDs)
            {
                sql.Append($" ID = '{item}' OR");
            }
            sql = sql.Remove(sql.Length - 3, 3);
            sqlite.sqlStatment(sql.ToString());
        }

        static public void deleteQuestionID(List<string> IDs)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@$"update Questions set admin = -1 where ");
            foreach (var item in IDs)
            {
                sql.Append($" ID = '{item}' OR");
            }
            sql = sql.Remove(sql.Length - 3, 3);
            sqlite.sqlStatment(sql.ToString());
        }
        static public void deleteAnswersID(List<string> IDs)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@$"update Answers set admin = -1 where ");
            foreach (var item in IDs)
            {
                sql.Append($" ID = '{item}' OR");
            }
            sql = sql.Remove(sql.Length - 3, 3);
            sqlite.sqlStatment(sql.ToString());
        }

        internal static DataTable getAllEditQustionAdmin()
        {
            string sql = @$"select * from (select * from Questions where date = '{today}' and Admin = 4 order by date ) order by LoveCount DESC";
            return sqlite.sqlReturnTable(sql);
        }

        internal static void increaseAnswerLoveById(string id, int IncreaseAmount)
        {
            string sql = $"UPDATE Answers SET LoveCount = LoveCount + {IncreaseAmount} where ID = {id}";
            sqlite.sqlStatment(sql);
        }

        static public void deleteQuestionID(string ID)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@$"update Questions set admin = -1 where ID = {ID}");
            sqlite.sqlStatment(sql.ToString());
        }

        internal static DataTable getAnswersAdmin()
        {
            string sql = @$"
select a.id,q.Question,a.answer,a.admin from Answers a
join Questions q where a.questionID = q.ID 
and q.Admin = 1 and a.admin = 0
";
            return sqlite.sqlReturnTable(sql);
        }

        internal static void deleteAnswerID(string id)
        {
            StringBuilder sql = new StringBuilder();
            sql.AppendLine(@$"update Answers set admin = -1 where ID = {id}");
            sqlite.sqlStatment(sql.ToString());
        }

        internal static DataTable getAllPublicAnswers(string value)
        {
            string decodeValue = Helper.customBase64Decode(value);
            if (decodeValue == "NULL" || decodeValue == "") return null;

            string sql = @$"
select a.id,a.answer,a.LoveCount from Answers a
join Questions q where a.questionID = q.ID 
and q.Admin = 1 and a.admin = 1 and q.ID = '{decodeValue}'
order by a.LoveCount DESC
";
            return sqlite.sqlReturnTable(sql);
        }

        internal static string getQuestionFromCustom64Value(string value)
        {
            /*
             * decode 
             * else return question
             */

            string decodeValue = Helper.customBase64Decode(value);
            if (decodeValue == "NULL" || decodeValue == "") return "-1";
            string sqlQuestion = sqlite.sqlReturnValue($"select Question from Questions where ID = '{decodeValue}' and Admin = 1");
            if (sqlQuestion == null) return "-1";
            else if (value.Trim() == "") return "-1";
            else return sqlQuestion;
        }

        internal static void increaseLoveById(string id, int IncreaseAmount)
        {
            //sql statment 
            string sql = $"UPDATE Questions SET LoveCount = LoveCount + {IncreaseAmount} where ID = {id}";
            sqlite.sqlStatment(sql);
        }

        internal static void updateQuestion(string oldQuestion, string newQuestion)
        {
            string sql = $"UPDATE Questions SET Question = '{newQuestion}',admin = 4 where Question = '{oldQuestion}';";
            sqlite.sqlStatment(sql);
        }

        internal static void addAnswer(string id, string Answer, int authorized)
        {
            //sql statment 
            int admin = 0;
            //if (authorized == 1)
            //{
            //    admin = 1;
            //}
            //else
            //{
            //    admin = 0;
            //}
            string sql = $"insert into Answers (questionID,Answer,admin,LoveCount) values ({id},'{Answer}','{admin}',0)";
            sqlite.sqlStatment(sql);
        }

        static public void insertQuestion(string question, string Name)
        {

            string sql = @$"insert into Questions (Question,Name,Note,date,LoveCount,Admin) values ('{question}','{Name}',NULL,'{today}',0,0)";
            sqlite.sqlStatment(sql);
        }

        static public void insertSuggestion(string Suggestion, string Name)
        {
            string sql = @$"insert into Suggestions (Suggestion,Name,date) values ('{Suggestion}','{Name}','{today}')";
            sqlite.sqlStatment(sql);
        }

        static public bool ifPassTheSame(string localPass)
        {
            string sql = @$"select pass from settings limit 1;";
            return localPass == sqlite.sqlReturnValue(sql);
        }

        internal static string getpass64()
        {
            string sql = @$"select pass from settings limit 1;";
            return sqlite.sqlReturnValue(sql);
        }
    }
}
