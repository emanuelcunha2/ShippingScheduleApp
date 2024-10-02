using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using ShippingScheduleMVVM.Helpers;
using ShippingScheduleMVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Windows;

namespace ShippingScheduleMVVM.Services
{
    public class DatabaseOperations
    {
        private SqlConnection sqlConnection = new SqlConnection(App.db_connectionString);
        #region "User Database Operations"
        /// <summary>
        /// Authenticates a user based on their username and password.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>A User object representing the authenticated user, including their email, id, and authentication status.</returns>
        public User LoginUser(string username, string password)
        {
            try
            {
                sqlConnection.Open();

                string insertedPassword = password;
                string databasePassword = "";
                string email = "";
                int authenticationStatus = 0;
                int id = -1;

                bool userExists = false;

                string queryString = $"SELECT TOP 1 * FROM Users Where username = '{username}'";

                SqlCommand command = new SqlCommand(queryString, sqlConnection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    username = reader["username"].ToString() ?? "";
                    userExists = true;
                    databasePassword = reader["password"].ToString() ?? string.Empty;
                    id = (int?)reader["id"] ?? -1;
                    email = reader["email"].ToString() ?? string.Empty;
                }

                if (!userExists)
                {
                    sqlConnection.Close();
                    return new User(403);
                }

                if (BcryptAuthentication.ValidatePassword(insertedPassword, databasePassword))
                {
                    authenticationStatus = 200;
                }
                else
                {
                    authenticationStatus = 401;
                }

                sqlConnection.Close();
                return new User(username, email, id, authenticationStatus);
            }
            catch (SqlException ex)
            {
                sqlConnection.Close();
                MessageBox.Show("SQL Error: " + (ex.ToString()));
                return new User(500);
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
                MessageBox.Show("Error: " + (ex.ToString()));
                return new User(500);
            }
        }
        /// <summary>
        /// This method retrieves a list of roles for a given user ID by querying the database
        /// </summary>
        /// <param name="id">The ID of the user whose roles are being retrieved</param>
        /// <returns>A list of strings representing the roles of the user, or null if no roles are found</returns>
        public List<string>? GetUserRoles(int id)
        {
            try
            {
                sqlConnection.Open();
                List<string>? roles = new();
                string queryString = " SELECT role FROM UsersInRoles ur "
                                    + " JOIN Users u ON u.id = ur.user_id "
                                    + " JOIN Roles r ON r.id = ur.role_id "
                                    + $" Where u.id = '{id}'";

                SqlCommand command = new SqlCommand(queryString, sqlConnection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    roles.Add(reader["role"].ToString() ?? string.Empty);
                }

                if (roles.Count == 0) { roles = null; }
                sqlConnection.Close();
                return roles;
            }
            catch (SqlException ex)
            {
                sqlConnection.Close();
                MessageBox.Show("SQL Error: " + (ex.ToString()));
                return null;
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
                MessageBox.Show("Error: " + (ex.ToString()));
                return null;
            }
        }
        public ObservableCollection<string>? GetRoles()
        {
            try
            {
                sqlConnection.Open();
                ObservableCollection<string>? roles = new();
                string queryString = " SELECT role FROM Roles ";

                SqlCommand command = new SqlCommand(queryString, sqlConnection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    roles.Add(reader["role"].ToString() ?? string.Empty);
                }

                if (roles.Count == 0) { roles = null; }
                sqlConnection.Close();
                return roles;
            }
            catch (SqlException ex)
            {
                sqlConnection.Close();
                MessageBox.Show("SQL Error: " + (ex.ToString()));
                return null;
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
                MessageBox.Show("Error: " + (ex.ToString()));
                return null;
            }
        }
        public bool UsernameExists(string username)
        {
            try
            {
                sqlConnection.Open();
                bool userExists = false;
                // Query
                string queryString = $"SELECT * FROM Users Where username = '{username}'";

                SqlCommand command = new SqlCommand(queryString, sqlConnection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    userExists = true;
                }

                if (!userExists)
                {
                    sqlConnection.Close();
                    return false;
                }
                sqlConnection.Close();
                return true;
            }
            catch (SqlException ex)
            {
                sqlConnection.Close();
                MessageBox.Show("SQL Error: " + (ex.ToString()));
                return true;
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
                MessageBox.Show("Error: " + (ex.ToString()));
                return true;
            }
        }
        public bool RegisterUser(string username, string password, string email)
        {
            try
            {
                sqlConnection.Open();

                var hashedPassword = BcryptAuthentication.HashPassword(password);
                // Query
                string queryString = "INSERT INTO Users(username,email,password,isActive) "
                                   + $"VALUES ('{username}','{email}','{hashedPassword}','false')";

                SqlCommand command = new SqlCommand(queryString, sqlConnection);
                command.ExecuteNonQuery();

                sqlConnection.Close();
                return true;
            }
            catch (SqlException ex)
            {
                sqlConnection.Close();
                MessageBox.Show("SQL Error: " + (ex.ToString()));
                return false;
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
                MessageBox.Show("Error: " + (ex.ToString()));
                return false;
            }
        }
        #endregion
        #region "ScheduleDatabaseOperations"
        public ObservableCollection<ScheduleRecord> SelectScheduleRecords(DateTime dateStart)
        {
            dateStart = new DateTime(dateStart.AddDays(-1).Year, dateStart.AddDays(-1).Month, dateStart.AddDays(-1).Day);

            var dateEnd = dateStart.AddDays(8);
            sqlConnection.Open();

            ObservableCollection<ScheduleRecord> scheduleRecords = new();

            string queryString = "SELECT tm.mode, b.id, b.paleteNumber, b.dn, b.cancelled, b.rescheduled, b.originalSchedule, b.wasNotified, b.plate, c.category, s.day, st.type, wasNotified, B.shippedDate, b.carrier, s.time FROM BillsInSchedule bs "
                               + "JOIN ShippingBills b ON shippingBill_id = b.id "
                               + "JOIN ShippingSchedules s on shippingSchedule_id = s.id "
                               + "JOIN Categories c on b.category_id = c.id "
                               + "JOIN ShipmentTypes st on b.shipmentType_id = st.id "
                               + "JOIN TransportModes tm ON transportMode_id = tm.id "
                               + $" WHERE s.day > '{dateStart.Month}-{dateStart.Day}-{dateStart.Year}' AND s.day < '{dateEnd.Month}-{dateEnd.Day}-{dateEnd.Year}'";

            SqlCommand command = new(queryString, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ScheduleRecord sr = new();

                sr.ShipmentType = reader["type"].ToString() ?? "";
                sr.Id = (int)reader["id"];
                sr.Category = reader["category"].ToString() ?? "";
                sr.Plate = reader["plate"].ToString() ?? "";
                sr.Time = reader["time"].ToString() ?? "";
                sr.Date = DateTime.Parse(reader["day"].ToString() ?? "01-01-1901");
                sr.TransportMode = reader["mode"].ToString() ?? "";

                sr.WasNotified = false;
                if ((reader["wasNotified"].ToString() ?? "") == "true")
                {
                    sr.WasNotified = true;
                }

                sr.WasRescheduled = false;
                if ((reader["rescheduled"].ToString() ?? "") == "true")
                {
                    sr.WasRescheduled = true;
                }

                sr.WasCancelled = false;
                if ((reader["cancelled"].ToString() ?? "") == "true")
                {
                    sr.WasCancelled = true;
                }
                var shippedDate = reader["shippedDate"].ToString() ?? "";
                sr.ShippedDate = DateTime.Parse(shippedDate.IsNullOrEmpty() ? "01-01-1901" : shippedDate);

                var paleteNumber = reader["paleteNumber"].ToString().IsNullOrEmpty() ? "0" : reader["paleteNumber"].ToString();
                sr.PaleteNumber = Int32.Parse(paleteNumber ?? "0");
                sr.OriginalSchedule = reader["originalSchedule"].ToString() ?? "";
                sr.ShipTos = reader["dn"].ToString() ?? "";
                sr.Carrier = reader["carrier"].ToString() ?? "";
                scheduleRecords.Add(sr);
            }
            sqlConnection.Close();
            return scheduleRecords;
        }
         
        public ObservableCollection<Record> SelectScheduleRecordsBetweenDates(DateTime dateStart, DateTime dateEnd)
        {
            dateStart = new DateTime(dateStart.AddDays(-1).Year, dateStart.AddDays(-1).Month, dateStart.AddDays(-1).Day);
            dateEnd = new DateTime(dateEnd.AddDays(1).Year, dateEnd.AddDays(1).Month, dateEnd.AddDays(1).Day);

            sqlConnection.Open();

            ObservableCollection<Record> scheduleRecords = new();

            string queryString = "SELECT b.id, b.paleteNumber, b.transportArrivalDate, b.dn, b.rescheduled,b.originalSchedule, b.wasNotified , b.cancelled, c.category, b.carrier, deliveryDate ,b.plate, b.leavingTime, s.time, s.day, st.type as shipmentType, tm.mode as transportMode, b.bid, b.pta, b.unloading, b.comment,b.creationDate,b.updatedDate , releasedUser, releasedDate, processUser, processDate, preparedUser, preparedDate, shippedUser, shippedDate, creationUser FROM BillsInSchedule bs  "
                               + "JOIN ShippingBills b ON shippingBill_id = b.id "
                               + "JOIN ShippingSchedules s ON shippingSchedule_id = s.id "
                               + "JOIN Categories c ON b.category_id = c.id "
                               + "JOIN ShipmentTypes st ON shipmentType_id = st.id "
                               + "JOIN TransportModes tm ON transportMode_id = tm.id "
                               + $" WHERE s.day > '{dateStart.Month}-{dateStart.Day}-{dateStart.Year}' AND s.day < '{dateEnd.Month}-{dateEnd.Day}-{dateEnd.Year}'";

            SqlCommand command = new(queryString, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Record sr = new();

                sr.Id = (reader["id"] != DBNull.Value) ? (int)reader["id"] : 0;
                sr.Category = reader["category"].ToString() ?? "";
                sr.Carrier = reader["carrier"].ToString() ?? "";
                sr.Plate = reader["plate"].ToString() ?? "";
                sr.Time = reader["time"].ToString() ?? "";
                sr.Day = reader["day"].ToString() ?? "";
                sr.ShipmentType = reader["shipmentType"].ToString() ?? "";
                sr.TransportMode = reader["transportMode"].ToString() ?? "";
                sr.BID = reader["bid"].ToString() ?? "";
                sr.PTA = reader["pta"].ToString() ?? "";
                sr.UnloadingId = reader["unloading"].ToString() ?? "";
                sr.Comment = reader["comment"].ToString() ?? "";
                sr.CreationDate = (reader["creationDate"] != DBNull.Value) ? (DateTime)reader["creationDate"] : DateTime.MinValue;
                sr.WasNotified = (reader["wasNotified"].ToString() == "true") ? true : false;
                sr.ShipTo = reader["dn"].ToString() ?? "";

                if ((reader["wasNotified"].ToString() ?? "") == "true") { sr.WasNotified = true; }
                else
                {
                    sr.WasNotified = true;
                    sr.WasNotified = false;
                }

                sr.DeliveryDate = (reader["deliveryDate"] != DBNull.Value) ? (DateTime)reader["deliveryDate"] : null;
                sr.UpdatedDate = (reader["updatedDate"] != DBNull.Value) ? (DateTime)reader["updatedDate"] : DateTime.MinValue;
                sr.CreatedBy = reader["creationUser"].ToString() ?? "";
                sr.ReleasedBy = reader["releasedUser"].ToString() ?? "";
                sr.ReleasedDate = (reader["releasedDate"] != DBNull.Value) ? (DateTime)reader["releasedDate"] : DateTime.MinValue;
                sr.ProcessBy = reader["processUser"].ToString() ?? "";
                sr.ProcessDate = (reader["processDate"] != DBNull.Value) ? (DateTime)reader["processDate"] : DateTime.MinValue;
                sr.PreparedBy = reader["preparedUser"].ToString() ?? "";
                sr.PreparedDate = (reader["preparedDate"] != DBNull.Value) ? (DateTime)reader["preparedDate"] : DateTime.MinValue;
                sr.ShippedBy = reader["shippedUser"].ToString() ?? "";
                sr.ShippedDate = (reader["shippedDate"] != DBNull.Value) ? (DateTime)reader["shippedDate"] : DateTime.MinValue;
                sr.PaleteNumber = reader["paleteNumber"].ToString() ?? "";

                sr.IsCancelled = (reader["cancelled"].ToString() == "true") ? true : false;
                sr.Rescheduled = (reader["rescheduled"].ToString() == "true") ? true : false;

                sr.OriginalSchedule = reader["originalSchedule"].ToString() ?? "";
                sr.TransportArrivalDate = (reader["transportArrivalDate"] != DBNull.Value) ? (DateTime)reader["transportArrivalDate"] : DateTime.MinValue;
                if (sr.TransportArrivalDate != DateTime.MinValue) { sr.HasTransportArrived = true; }
                else { sr.HasTransportArrived = false; }

                scheduleRecords.Add(sr);
            }
            sqlConnection.Close();
            return scheduleRecords;
        }

        public ObservableCollection<ScheduleRecord> SelectScheduleRecordsId(string id)
        {
            sqlConnection.Open();

            ObservableCollection<ScheduleRecord> scheduleRecords = new();

            string queryString = "SELECT TOP 10 " +
                                 "b.id, " +
                                 "b.dn, " +
                                 "s.day, " +
                                 "s.time " +
                                 "FROM " +
                                 "BillsInSchedule bs " +
                                 "JOIN ShippingBills b ON bs.shippingBill_id = b.id " +
                                 "JOIN ShippingSchedules s ON bs.shippingSchedule_id = s.id " +
                                 "JOIN Categories c ON b.category_id = c.id " +
                                 "JOIN ShipmentTypes st ON b.shipmentType_id = st.id " +
                                 "JOIN TransportModes tm ON transportMode_id = tm.id " +
                                 "LEFT JOIN (SELECT shippingBill_id, deliveryNote FROM BillsDataList GROUP BY deliveryNote, shippingBill_id) bdl " +
                                 "ON bdl.shippingBill_id = bs.shippingBill_id " +
                                 $"WHERE b.id LIKE '{id}%' OR deliveryNote LIKE '{id}%' " +
                                 "GROUP BY b.id, b.dn, s.day, s.time " +
                                 "ORDER BY b.id;";

            SqlCommand command = new(queryString, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ScheduleRecord sr = new();

                sr.Id = (int)reader["id"];
                sr.Date = DateTime.Parse(reader["day"].ToString() ?? "01-01-1901");
                 
                DateTime date = new DateTime(sr.Date.Year, sr.Date.Month, sr.Date.Day); 
                string timeString = reader["time"].ToString();

                try
                {
                    TimeSpan timeSpan = TimeSpan.ParseExact(timeString, "hh\\:mm", null);
                    sr.Date = date + timeSpan;
                } 
                catch(Exception) {}
                

                sr.ShipTos = reader["dn"].ToString() ?? "";
                scheduleRecords.Add(sr);
            }
            sqlConnection.Close();
            return scheduleRecords;
        }

        public ObservableCollection<ScheduleRecord> SelectScheduleRecordsDay(DateTime dateStart)
        {
            sqlConnection.Open();

            dateStart = dateStart.Date;
            DateTime dateEnd = dateStart.AddHours(24);
            ObservableCollection<ScheduleRecord> scheduleRecords = new();

            string queryString = "SELECT tm.mode, b.id, b.paleteNumber, b.dn, b.cancelled, b.rescheduled, b.originalSchedule, b.wasNotified, b.plate, c.category, s.day, st.type, wasNotified, B.shippedDate, B.preparedDate, s.time FROM BillsInSchedule bs "
                               + "JOIN ShippingBills b ON shippingBill_id = b.id "
                               + "JOIN ShippingSchedules s on shippingSchedule_id = s.id "
                               + "JOIN Categories c on b.category_id = c.id "
                               + "JOIN ShipmentTypes st on b.shipmentType_id = st.id "
                               + "JOIN TransportModes tm ON transportMode_id = tm.id "
                               + $" WHERE s.day = '{dateStart.Month}-{dateStart.Day}-{dateStart.Year}' AND c.category ='Shipped' ";

            SqlCommand command = new(queryString, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ScheduleRecord sr = new();

                sr.ShipmentType = reader["type"].ToString() ?? "";
                sr.Id = (int)reader["id"];
                sr.Category = reader["category"].ToString() ?? "";
                sr.Plate = reader["plate"].ToString() ?? "";
                sr.Time = reader["time"].ToString() ?? "";
                sr.Date = DateTime.Parse(reader["day"].ToString() ?? "01-01-1901");
                sr.TransportMode = reader["mode"].ToString() ?? "";

                sr.WasNotified = false;
                if ((reader["wasNotified"].ToString() ?? "") == "true")
                {
                    sr.WasNotified = true;
                }

                sr.WasRescheduled = false;
                if ((reader["rescheduled"].ToString() ?? "") == "true")
                {
                    sr.WasRescheduled = true;
                }

                sr.WasCancelled = false;
                if ((reader["cancelled"].ToString() ?? "") == "true")
                {
                    sr.WasCancelled = true;
                }
                var shippedDate = reader["shippedDate"].ToString() ?? "";
                sr.ShippedDate = DateTime.Parse(shippedDate.IsNullOrEmpty() ? "01-01-1901" : shippedDate);

                var paleteNumber = reader["paleteNumber"].ToString().IsNullOrEmpty() ? "0" : reader["paleteNumber"].ToString();
                sr.PaleteNumber = Int32.Parse(paleteNumber ?? "0");
                sr.OriginalSchedule = reader["originalSchedule"].ToString() ?? "";
                sr.ShipTos = reader["dn"].ToString() ?? "";

                sr.PreparedDate = (reader["preparedDate"] != DBNull.Value) ? (DateTime)reader["preparedDate"] : DateTime.MinValue;

                scheduleRecords.Add(sr);
            }
            sqlConnection.Close();
            return scheduleRecords;
        }

        public RecordsStatus CountStatus()
        {
            RecordsStatus rStatus = new();

            sqlConnection.Open();

            SqlCommand command = new("CountStatus_V2", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;

            var returnParameter1 = command.Parameters.Add("@countCreated", SqlDbType.Int, 10, "countCreated");
            returnParameter1.Direction = ParameterDirection.Output;

            var returnParameter2 = command.Parameters.Add("@countReleased", SqlDbType.Int, 10, "countReleased");
            returnParameter2.Direction = ParameterDirection.Output;

            var returnParameter3 = command.Parameters.Add("@countProcess", SqlDbType.Int, 10, "countProcess");
            returnParameter3.Direction = ParameterDirection.Output;

            var returnParameter4 = command.Parameters.Add("@countPrepared", SqlDbType.Int, 10, "countPrepared");
            returnParameter4.Direction = ParameterDirection.Output;

            var returnParameter5 = command.Parameters.Add("@countShipped", SqlDbType.Int, 10, "countShipped");
            returnParameter5.Direction = ParameterDirection.Output;

            var returnParameter6 = command.Parameters.Add("@countNotified", SqlDbType.Int, 10, "countNotified");
            returnParameter6.Direction = ParameterDirection.Output;

            var returnParameter7 = command.Parameters.Add("@countCanceled", SqlDbType.Int, 10, "countCanceled");
            returnParameter7.Direction = ParameterDirection.Output;

            var returnParameter8 = command.Parameters.Add("@countRescheduled", SqlDbType.Int, 10, "countRescheduled");
            returnParameter8.Direction = ParameterDirection.Output;

            command.ExecuteNonQuery();

            var created = returnParameter1.Value;
            var released = returnParameter2.Value;
            var process = returnParameter3.Value;
            var prepared = returnParameter4.Value;
            var shipped = returnParameter5.Value;
            var notified = returnParameter6.Value;
            var canceled = returnParameter7.Value;
            var rescheduled = returnParameter8.Value;

            rStatus.Created = (int)created;
            rStatus.Released = (int)released;
            rStatus.Handled = (int)process;
            rStatus.Prepared = (int)prepared;
            rStatus.Shipped = (int)shipped;
            rStatus.Notified = (int)notified;
            rStatus.Canceled = (int)canceled;
            rStatus.Rescheduled = (int)rescheduled;

            sqlConnection.Close();

            return rStatus;
        }
        public RecordsStatus SelectDailyStatus(DateTime date)
        {
            RecordsStatus rStatus = new();

            sqlConnection.Open();

            SqlCommand command = new("CountStatusDaily", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@scheduleDay", SqlDbType.Date).Value = new DateTime(date.Year,date.Month,date.Day);

            var returnParameter1 = command.Parameters.Add("@countCreated", SqlDbType.Int, 10, "countCreated");
            returnParameter1.Direction = ParameterDirection.Output;

            var returnParameter2 = command.Parameters.Add("@countReleased", SqlDbType.Int, 10, "countReleased");
            returnParameter2.Direction = ParameterDirection.Output;

            var returnParameter3 = command.Parameters.Add("@countProcess", SqlDbType.Int, 10, "countProcess");
            returnParameter3.Direction = ParameterDirection.Output;

            var returnParameter4 = command.Parameters.Add("@countPrepared", SqlDbType.Int, 10, "countPrepared");
            returnParameter4.Direction = ParameterDirection.Output;

            var returnParameter5 = command.Parameters.Add("@countShipped", SqlDbType.Int, 10, "countShipped");
            returnParameter5.Direction = ParameterDirection.Output;

            var returnParameter6 = command.Parameters.Add("@countNotified", SqlDbType.Int, 10, "countNotified");
            returnParameter6.Direction = ParameterDirection.Output;

            var returnParameter7 = command.Parameters.Add("@countCanceled", SqlDbType.Int, 10, "countCanceled");
            returnParameter7.Direction = ParameterDirection.Output;

            var returnParameter8 = command.Parameters.Add("@countRescheduled", SqlDbType.Int, 10, "countRescheduled");
            returnParameter8.Direction = ParameterDirection.Output;

            command.ExecuteNonQuery();

            var created = returnParameter1.Value;
            var released = returnParameter2.Value;
            var process = returnParameter3.Value;
            var prepared = returnParameter4.Value;
            var shipped = returnParameter5.Value;
            var notified = returnParameter6.Value;
            var canceled = returnParameter7.Value;
            var rescheduled = returnParameter8.Value;

            rStatus.Created = (int)created;
            rStatus.Released = (int)released;
            rStatus.Handled = (int)process;
            rStatus.Prepared = (int)prepared;
            rStatus.Shipped = (int)shipped;
            rStatus.Notified = (int)notified;
            rStatus.Canceled = (int)canceled;
            rStatus.Rescheduled = (int)rescheduled;

            sqlConnection.Close();

            return rStatus;
        }
        public int InsertNewRecord(Record record, string user, string originalSchedule, string shipTosList)
        {
            sqlConnection.Open();

            SqlCommand command = new("InsertShippingBill_v3", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@scheduleDay", SqlDbType.Date).Value = record.Day;
            command.Parameters.AddWithValue("@scheduleTime", SqlDbType.VarChar).Value = record.Time;
            command.Parameters.AddWithValue("@category", SqlDbType.VarChar).Value = record.Category;
            command.Parameters.AddWithValue("@shipTo", SqlDbType.VarChar).Value = record.ShipTo;
            command.Parameters.AddWithValue("@shipmentType", SqlDbType.VarChar).Value = record.ShipmentType;
            command.Parameters.AddWithValue("@transportMode", SqlDbType.VarChar).Value = record.TransportMode;
            command.Parameters.AddWithValue("@plate", SqlDbType.VarChar).Value = record.Plate;
            command.Parameters.AddWithValue("@bid", SqlDbType.VarChar).Value = record.BID;
            command.Parameters.AddWithValue("@carrier", SqlDbType.VarChar).Value = record.Carrier;
            command.Parameters.AddWithValue("@pta", SqlDbType.VarChar).Value = record.PTA;
            command.Parameters.AddWithValue("@unloading", SqlDbType.VarChar).Value = record.UnloadingId;
            command.Parameters.AddWithValue("@comment", SqlDbType.VarChar).Value = record.Comment;
            command.Parameters.AddWithValue("@creationDate", SqlDbType.DateTime).Value = record.CreationDate;
            command.Parameters.AddWithValue("@leavingTime", SqlDbType.Time).Value = "00:00:00";
            command.Parameters.AddWithValue("@dn", SqlDbType.VarChar).Value = shipTosList;
            command.Parameters.AddWithValue("@createdUser", SqlDbType.VarChar).Value = user;
            command.Parameters.AddWithValue("@paleteNumber", SqlDbType.Int).Value = record.PaleteNumber;
            command.Parameters.AddWithValue("@originalSchedule", SqlDbType.VarChar).Value = originalSchedule;

            if (record.DeliveryDate == null || record.DeliveryDate == DateTime.MinValue)
            {
                command.Parameters.AddWithValue("@deliveryDate", SqlDbType.DateTime).Value = DBNull.Value;
            }
            else { command.Parameters.AddWithValue("@deliveryDate", SqlDbType.DateTime).Value = record.DeliveryDate; }

            var returnParameter = command.Parameters.Add("@return_id", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.ReturnValue;

            command.ExecuteNonQuery();
            sqlConnection.Close();

            var recordId = returnParameter.Value;

            return (int)recordId;
        }
        public List<string> SelectSupervisorsEmails()
        {
            sqlConnection.Open();
            List<string> mails = new();
            // Query
            string queryString = " SELECT email"
                                  + " FROM [dbo].[Users]"
                                  + " JOIN UsersInRoles ur ON id = ur.user_id"
                                  + " JOIN Roles r ON ur.role_id = r.id"
                                  + " WHERE role = 'Supervisor'";

            SqlCommand command = new(queryString, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                mails.Add(reader["email"].ToString() ?? "x");
            }
            sqlConnection.Close();
            return mails;
        }
        public List<string> SelectNotifyDailyPlanChangesEmails()
        {
            sqlConnection.Open();
            List<string> mails = new();
            // Query
            string queryString = " SELECT email"
                                  + " FROM [dbo].[Users]"
                                  + " JOIN UsersInRoles ur ON id = ur.user_id"
                                  + " JOIN Roles r ON ur.role_id = r.id"
                                  + " WHERE notifyDailyPlanChanges = 'T'";

            SqlCommand command = new(queryString, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                mails.Add(reader["email"].ToString() ?? "x");
            }
            sqlConnection.Close();
            return mails;
        }
        public string SelectUserEmail(string user)
        {
            sqlConnection.Open();
            string mail = "";
            // Query
            string queryString = $"SELECT email FROM Users WHERE username='{user}'";

            SqlCommand command = new(queryString, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                mail = reader["email"].ToString() ?? "x";
            }
            sqlConnection.Close();
            return mail;
        }
        public int InsertCopyRecord(Record record, string user, string originalSchedule, string shipTosList)
        {
            sqlConnection.Open();

            SqlCommand command = new("InsertShippingBill_v3", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@scheduleDay", SqlDbType.Date).Value = record.Day;
            command.Parameters.AddWithValue("@scheduleTime", SqlDbType.VarChar).Value = record.Time;
            command.Parameters.AddWithValue("@category", SqlDbType.VarChar).Value = "Created";
            command.Parameters.AddWithValue("@shipTo", SqlDbType.VarChar).Value = record.ShipTo;
            command.Parameters.AddWithValue("@shipmentType", SqlDbType.VarChar).Value = record.ShipmentType;
            command.Parameters.AddWithValue("@transportMode", SqlDbType.VarChar).Value = record.TransportMode;
            command.Parameters.AddWithValue("@plate", SqlDbType.VarChar).Value = record.Plate;
            command.Parameters.AddWithValue("@bid", SqlDbType.VarChar).Value = record.BID;
            command.Parameters.AddWithValue("@carrier", SqlDbType.VarChar).Value = record.Carrier;
            command.Parameters.AddWithValue("@pta", SqlDbType.VarChar).Value = record.PTA;
            command.Parameters.AddWithValue("@unloading", SqlDbType.VarChar).Value = record.UnloadingId;
            command.Parameters.AddWithValue("@comment", SqlDbType.VarChar).Value = record.Comment;
            command.Parameters.AddWithValue("@creationDate", SqlDbType.DateTime).Value = DateTime.Now;
            command.Parameters.AddWithValue("@leavingTime", SqlDbType.Time).Value = "00:00:00";
            command.Parameters.AddWithValue("@dn", SqlDbType.VarChar).Value = shipTosList;
            command.Parameters.AddWithValue("@createdUser", SqlDbType.VarChar).Value = user;
            command.Parameters.AddWithValue("@paleteNumber", SqlDbType.Int).Value = record.PaleteNumber;
            command.Parameters.AddWithValue("@originalSchedule", SqlDbType.VarChar).Value = originalSchedule;

            if (record.DeliveryDate == null || record.DeliveryDate == DateTime.MinValue)
            {
                command.Parameters.AddWithValue("@deliveryDate", SqlDbType.DateTime).Value = DBNull.Value;
            }
            else { command.Parameters.AddWithValue("@deliveryDate", SqlDbType.DateTime).Value = record.DeliveryDate; }

            var returnParameter = command.Parameters.Add("@return_id", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.ReturnValue;

            command.ExecuteNonQuery();
            sqlConnection.Close();

            var recordId = returnParameter.Value;

            return (int)recordId;
        }
        public void UpdateRecord(Record record, string user, string shipTosList)
        {
            sqlConnection.Open();
            SqlCommand command = new SqlCommand("UpdateShippingBill_v4", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@shippingBill_id", SqlDbType.Int).Value = record.Id;
            command.Parameters.AddWithValue("@category", SqlDbType.VarChar).Value = record.Category;
            command.Parameters.AddWithValue("@shipTo", SqlDbType.VarChar).Value = record.ShipTo;
            command.Parameters.AddWithValue("@shipmentType", SqlDbType.VarChar).Value = record.ShipmentType;
            command.Parameters.AddWithValue("@transportMode", SqlDbType.VarChar).Value = record.TransportMode;
            command.Parameters.AddWithValue("@plate", SqlDbType.VarChar).Value = record.Plate.IsNullOrEmpty() ? DBNull.Value : record.Plate;
            command.Parameters.AddWithValue("@bid", SqlDbType.VarChar).Value = record.BID.IsNullOrEmpty() ? DBNull.Value : record.BID;
            command.Parameters.AddWithValue("@carrier", SqlDbType.VarChar).Value = record.Carrier.IsNullOrEmpty() ? DBNull.Value : record.Carrier;
            command.Parameters.AddWithValue("@pta", SqlDbType.VarChar).Value = record.PTA.IsNullOrEmpty() ? DBNull.Value : record.PTA;
            command.Parameters.AddWithValue("@unloading", SqlDbType.VarChar).Value = record.UnloadingId.IsNullOrEmpty() ? DBNull.Value : record.UnloadingId;
            command.Parameters.AddWithValue("@comment", SqlDbType.VarChar).Value = record.Comment.IsNullOrEmpty() ? DBNull.Value : record.Comment;
            command.Parameters.AddWithValue("@updatedTime", SqlDbType.DateTime).Value = record.UpdatedDate;
            command.Parameters.AddWithValue("@leavingTime", SqlDbType.Time).Value = DBNull.Value;
            command.Parameters.AddWithValue("@dn", SqlDbType.VarChar).Value = shipTosList;
            command.Parameters.AddWithValue("@wasNotified", SqlDbType.VarChar).Value = record.WasNotified.ToString().ToLower();
            command.Parameters.AddWithValue("@selectedCategory", SqlDbType.VarChar).Value = record.Category; 
            command.Parameters.AddWithValue("@selectedDate", SqlDbType.DateTime).Value = record.UpdatedDate;
            command.Parameters.AddWithValue("@selectedUser", SqlDbType.VarChar).Value = user;
            command.Parameters.AddWithValue("@paleteNumber", SqlDbType.Int).Value = record.PaleteNumber;
            command.Parameters.AddWithValue("@cancelled", SqlDbType.VarChar).Value = record.IsCancelled.ToString().ToLower();
            command.Parameters.AddWithValue("@rescheduled", SqlDbType.VarChar).Value = record.Rescheduled.ToString().ToLower();

            if (!record.HasTransportArrivePreviously && record.HasTransportArrived)
            {
                command.Parameters.AddWithValue("@transportArrivalDate", SqlDbType.DateTime).Value = DateTime.Now;
            }
            else if (record.HasTransportArrivePreviously && record.HasTransportArrived)
            {
                command.Parameters.AddWithValue("@transportArrivalDate", SqlDbType.DateTime).Value = record.TransportArrivalDate;
            }
            else
            {
                command.Parameters.AddWithValue("@transportArrivalDate", SqlDbType.DateTime).Value = DBNull.Value;
            }
            
            if (record.DeliveryDate == null || record.DeliveryDate == DateTime.MinValue)
            {
                command.Parameters.AddWithValue("@deliveryDate", SqlDbType.DateTime).Value = DBNull.Value;
            }
            else { command.Parameters.AddWithValue("@deliveryDate", SqlDbType.DateTime).Value = record.DeliveryDate; }

            command.ExecuteNonQuery();
            sqlConnection.Close();

            if (!record.HasRecordBeenCancelledPreviously && record.IsCancelled)
            {
                var changedRecord = SelectRecordById(record.Id);
                var changedRecordParts = SelectRecordParts(record.Id);
                Mail.SendMailOnCancel(changedRecord, changedRecordParts);
            }
        }

        public void UpdateRecordShippedData(Record record, string user)
        {
            sqlConnection.Open(); 

            // Query
            string queryString = "UPDATE ShippingBills "
                              + $"SET paleteNumber = '{record.PaleteNumber}', "
                              + $"plate = '{record.Plate}' WHERE id = '{record.Id}'";

            SqlCommand command = new(queryString, sqlConnection);
            command.ExecuteNonQuery();
             
            sqlConnection.Close(); 
        }

        public void DeleteRecord(int id)
        {
            sqlConnection.Open();

            string queryString = $"DELETE BillsDataList WHERE shippingBill_id = '{id}' "
                               + $"DELETE DeliveryNotes WHERE shippingBill_id = '{id}' "
                               + $"DELETE BillsInSchedule WHERE shippingBill_id = '{id}' "
                               + $"DELETE ShippingBills WHERE id = '{id}' ";

            SqlCommand command = new SqlCommand(queryString, sqlConnection);
            command.ExecuteNonQuery();

            sqlConnection.Close();
        }
        public void DeleteTntRecord(int id)
        {
            sqlConnection.Open();

            string queryString = $"DELETE BillsDataList WHERE shippingBill_id = '{id}' "
                               + $"DELETE BillsInSchedule WHERE shippingBill_id = '{id}' "
                               + $"DELETE Projects WHERE shippingBill_id = '{id}' "
                               + $"DELETE ShippingBills WHERE id = '{id}' ";

            SqlCommand command = new SqlCommand(queryString, sqlConnection);
            command.ExecuteNonQuery();

            sqlConnection.Close();
        }
        public void UpdateRecordDate(string time, DateTime day, string id, string category)
        {
            sqlConnection.Open();

            SqlCommand command = new("UpdateBillSchedule", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@scheduleDay", SqlDbType.Date).Value = day.Date;
            command.Parameters.AddWithValue("@scheduleTime", SqlDbType.VarChar).Value = time;
            command.Parameters.AddWithValue("@Bill_id", SqlDbType.Int).Value = id;

            command.ExecuteNonQuery();
            sqlConnection.Close();

            sqlConnection.Open();

            string queryString;
            if (category == "Created")
            {
                queryString = $"UPDATE ShippingBills set originalSchedule = '{time + "+" + day.ToShortDateString()}' WHERE id = '{id}'";
            }
            else
            {
                queryString = $"UPDATE ShippingBills set rescheduled = 'true' WHERE id = '{id}'";
            }

            SqlCommand command2 = new(queryString, sqlConnection);
            command2.ExecuteNonQuery();

            sqlConnection.Close();
        }
        public void DeleteAllParts(int id)
        {
            sqlConnection.Open();
            
            string queryString = $"DELETE FROM BillsDataList WHERE shippingBill_id = '{id}'";

            SqlCommand command = new SqlCommand(queryString, sqlConnection);
            command.ExecuteNonQuery();

            sqlConnection.Close();
        }

        public void DeleteAllDeliveryNotes(int id)
        {
            sqlConnection.Open();

            string queryString = $"DELETE FROM DeliveryNotes WHERE shippingBill_id = '{id}'";

            SqlCommand command = new SqlCommand(queryString, sqlConnection);
            command.ExecuteNonQuery();

            sqlConnection.Close();
        }

        public void InsertProjects(Record record)
        { 
            foreach(Project p in record.RecordProjects)
            {
                if(p.Id != -69) { continue; }
                sqlConnection.Open();

                string deliveryDate = "NULL";

                if(p.DeliveryDate != null)
                {
                    DateTime date = new DateTime(p.DeliveryDate.Value.Year, p.DeliveryDate.Value.Month, p.DeliveryDate.Value.Day);
                    deliveryDate = "'" + date.ToString("yyyy-MM-dd HH:mm:ss") + "'"; 
                }

                string queryString = $"INSERT INTO Projects (phoneNumber, name, person, address, internal_id, shippingBill_id, user_id, pType,pta,comments,deliveryDate,hue) OUTPUT INSERTED.id VALUES ('{p.Phone}', '{p.Name}', '{p.Person}', '{p.Address}', {p.InternalId}, {record.Id}, {p.UserId}, '{p.Type}', '{p.PTA}','{p.Comment}',{deliveryDate},{p.UniqueHSVColorHue})";
                

                SqlCommand command = new SqlCommand(queryString, sqlConnection);
                int insertedId = (int)command.ExecuteScalar();

                p.Id = insertedId;
                sqlConnection.Close();
            }
        }

        public void UpdateProjects(Record record)
        {
            foreach (Project p in record.RecordProjects)
            {
                if (p.Id == -69 || p.Id == -1) { continue; }
                sqlConnection.Open();

                string deliveryDate = "NULL";

                if (p.DeliveryDate != null)
                {
                    DateTime date = new DateTime(p.DeliveryDate.Value.Year, p.DeliveryDate.Value.Month, p.DeliveryDate.Value.Day);
                    deliveryDate = "'" + date.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                }

                string queryString = $"UPDATE Projects SET phoneNumber = '{p.Phone}', person = '{p.Person}', address = '{p.Address}', pType = '{p.Type}', pta = '{p.PTA}', comments = '{p.Comment}', deliveryDate = {deliveryDate}  WHERE id = '{p.Id}'";

                SqlCommand command = new SqlCommand(queryString, sqlConnection);
                command.ExecuteNonQuery();

                sqlConnection.Close();
            }
        }

        public bool ExistShipTo(string shipTo)
        {
            try
            {
                sqlConnection.Open();

                string queryString = $"SELECT * FROM ShipTos Where shipTo = '{shipTo}'";

                SqlCommand command = new SqlCommand(queryString, sqlConnection);
                SqlDataReader reader = command.ExecuteReader();

                int count = 0;
                while (reader.Read())
                {
                    count++;
                }
                sqlConnection.Close();
                return count != 0;
            }
            catch (SqlException ex)
            {
                sqlConnection.Close();
                MessageBox.Show("SQL Error: " + (ex.ToString()));
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
                MessageBox.Show("Error: " + (ex.ToString()));
            }
            return true;
        }
        public bool InsertShipTo(ShipTo shipTo)
        {
            if (ExistShipTo(shipTo.ShipToNumber)) { return false; }
            try
            {
                sqlConnection.Open();
                // Query
                string queryString = $"INSERT INTO ShipTos(shipTo,description) VALUES ('{shipTo.ShipToNumber}','{shipTo.ShipToName}')";

                SqlCommand command = new SqlCommand(queryString, sqlConnection);
                command.ExecuteNonQuery();

                sqlConnection.Close();
                return true;
            }
            catch (SqlException ex)
            {
                sqlConnection.Close();
                MessageBox.Show("SQL Error: " + (ex.ToString()));
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
                MessageBox.Show("Error: " + (ex.ToString())); 
            }
            return false;
        }
        public bool InsertUnloadingPoint(ShipTo shipTo, string unloadingPoint)
        {
            if (!ExistShipTo(shipTo.ShipToNumber)) { return false; }
            try
            {
                int insertedId = -1;
                sqlConnection.Open();
                // Query
                string queryString = $"INSERT INTO UnloadingPoints(name) OUTPUT INSERTED.id VALUES ('{unloadingPoint}') ";

                SqlCommand command = new SqlCommand(queryString, sqlConnection);
                insertedId = (int)command.ExecuteScalar();

                sqlConnection.Close();

                return InsertUnloadingPointIntoShipTo(insertedId,shipTo.Id);
            }
            catch (SqlException ex)
            {
                sqlConnection.Close();
                MessageBox.Show("SQL Error: " + (ex.ToString()));
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
                MessageBox.Show("Error: " + (ex.ToString()));
            }
            return false;
        } 
        public bool InsertUnloadingPointIntoShipTo(int unloadingId, int shipToId)
        {
            try
            {
                sqlConnection.Open();
                // Query
                string queryString = $"INSERT INTO UnloadingPointsInShipTos(up_id,shipTo_id) VALUES ('{unloadingId}','{shipToId}')";

                SqlCommand command = new SqlCommand(queryString, sqlConnection);
                command.ExecuteNonQuery();

                sqlConnection.Close();
                return true;
            }
            catch (SqlException ex)
            {
                sqlConnection.Close();
                MessageBox.Show("SQL Error: " + (ex.ToString()));
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
                MessageBox.Show("Error: " + (ex.ToString()));
            }
            return false;
        } 
        public bool DeleteShipTo(ShipTo shipTo)
        {
            if (!ExistShipTo(shipTo.ShipToNumber)) { return false; }
            try
            {
                sqlConnection.Open();
                // Query
                string queryString = $"DELETE FROM UnloadingPointsInShipTos WHERE shipTo_id = '{shipTo.Id}'; DELETE FROM ShipTos WHERE shipTo = '{shipTo.ShipToNumber}'";

                SqlCommand command = new SqlCommand(queryString, sqlConnection);
                command.ExecuteNonQuery();

                sqlConnection.Close();
                return true;
            }
            catch (SqlException ex)
            {
                sqlConnection.Close();
                MessageBox.Show("SQL Error: " + (ex.ToString()));
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
                MessageBox.Show("Error: " + (ex.ToString()));
            }
            return false;
        } 
        public bool DeleteUnloadingPoint(ShipTo shipTo)
        {
            if (!ExistShipTo(shipTo.ShipToNumber)) { return false; }
            try
            {
                sqlConnection.Open();
                // Query
                string queryString = $"DELETE FROM UnloadingPointsInShipTos WHERE up_id = '{shipTo.UnloadingPointId}'; DELETE FROM UnloadingPoints WHERE id = '{shipTo.UnloadingPointId}'";

                SqlCommand command = new SqlCommand(queryString, sqlConnection);
                command.ExecuteNonQuery();

                sqlConnection.Close();
                return true;
            }
            catch (SqlException ex)
            {
                sqlConnection.Close();
                MessageBox.Show("SQL Error: " + (ex.ToString()));
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
                MessageBox.Show("Error: " + (ex.ToString()));
            }
            return false;
        } 
        public bool UpdateUnloadingPoint(ShipTo shipTo, string newUnloading)
        {
            if (!ExistShipTo(shipTo.ShipToNumber)) { return false; }
            try
            {
                sqlConnection.Open();
                // Query
                string queryString = $"UPDATE UnloadingPoints SET name = '{newUnloading}' WHERE id = '{shipTo.UnloadingPointId}'";

                SqlCommand command = new SqlCommand(queryString, sqlConnection);
                command.ExecuteNonQuery();

                sqlConnection.Close();
                return true;
            }
            catch (SqlException ex)
            {
                sqlConnection.Close();
                MessageBox.Show("SQL Error: " + (ex.ToString()));
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
                MessageBox.Show("Error: " + (ex.ToString()));
            }
            return false;
        } 
        public bool UpdateShipTo(ShipTo shipTo)
        {
            if (!ExistShipTo(shipTo.ShipToNumber)) { return false; }
            try
            {
                sqlConnection.Open();
                // Query
                string queryString = $"UPDATE ShipTos SET description = '{shipTo.ShipToName}' WHERE shipTo = '{shipTo.ShipToNumber}'";

                SqlCommand command = new SqlCommand(queryString, sqlConnection);
                command.ExecuteNonQuery();

                sqlConnection.Close();
                return true;
            }
            catch (SqlException ex)
            {
                sqlConnection.Close();
                MessageBox.Show("SQL Error: " + (ex.ToString()));
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
                MessageBox.Show("Error: " + (ex.ToString()));
            }
            return false;
        }

        public void InsertDeliveryNote(DeliveryNote dn, int id)
        { 
            sqlConnection.Open();

            SqlCommand command = new SqlCommand("InsertDeliveryNote", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@shippingBill_id", SqlDbType.VarChar).Value = id;
            command.Parameters.AddWithValue("@number", SqlDbType.VarChar).Value = dn.Number.Substring(1);
            command.ExecuteNonQuery();
            sqlConnection.Close();
        }

        public void InsertPartBasedOnDeliveryNote(DeliveryNote dn, int id)
        {
            sqlConnection.Open();

            SqlCommand command = new SqlCommand("InsertPartsOnDeliveryNoteData", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@shippingBill_id", SqlDbType.VarChar).Value = id;
            command.Parameters.AddWithValue("@deliveryNoteNumber", SqlDbType.VarChar).Value = dn.Number.Substring(1);

            command.ExecuteNonQuery();
            sqlConnection.Close();
        }

        public void ReInsertPartComments(int id, ObservableCollection<Part> parts)
        {
            sqlConnection.Open();

            foreach (Part part in parts)
            {
                // Query
                string queryString = "UPDATE BillsDataList " +
                      $"SET comment = '{part.Comment}' " +
                      "FROM BillsDataList b " +
                      "JOIN UnloadingPoints u ON u.id = b.unloadingPoint_id " +
                      "JOIN ShipTos s ON s.id = b.shipTo_id " +
                      $"WHERE shippingBill_id = '{id}' AND apn = '{part.APN}' AND u.name = '{part.UnloadingPoint}' AND s.description = '{part.ShipToName}'";

                SqlCommand command = new SqlCommand(queryString, sqlConnection);
                command.ExecuteNonQuery();
            }
 
            sqlConnection.Close();
        }

        public void InsertPart(Part part, int id)
        {
            if (part.ShipTo.IsNullOrEmpty() && part.APN.IsNullOrEmpty()) { return; }
            sqlConnection.Open();

            SqlCommand command = new SqlCommand("InsertBillList_v2", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@shippingBill_id", SqlDbType.VarChar).Value = id;
            command.Parameters.AddWithValue("@shipTo", SqlDbType.VarChar).Value = part.ShipTo.IsNullOrEmpty() ? DBNull.Value : part.ShipTo;
            command.Parameters.AddWithValue("@up", SqlDbType.VarChar).Value = part.UnloadingPoint.IsNullOrEmpty() ? DBNull.Value : part.UnloadingPoint;
            command.Parameters.AddWithValue("@apn", SqlDbType.VarChar).Value = part.APN.IsNullOrEmpty() ? DBNull.Value : part.APN;
            command.Parameters.AddWithValue("@cpn", SqlDbType.VarChar).Value = part.CPN.IsNullOrEmpty() ? DBNull.Value : part.CPN;
            command.Parameters.AddWithValue("@expectedQuantity", SqlDbType.Int).Value = part.ExpectedQuantity;
            command.Parameters.AddWithValue("@designation", SqlDbType.VarChar).Value = part.Designation.IsNullOrEmpty() ? DBNull.Value : part.Designation;
            command.Parameters.AddWithValue("@stockSAP", SqlDbType.Int).Value = 0;
            command.Parameters.AddWithValue("@finalQuantity", SqlDbType.Int).Value = part.FinalQuantity;
            command.Parameters.AddWithValue("@deliveryNote", SqlDbType.VarChar).Value = part.DeliveryNote.IsNullOrEmpty() ? DBNull.Value : part.DeliveryNote;
            command.Parameters.AddWithValue("@comment", SqlDbType.VarChar).Value = part.Comment.IsNullOrEmpty() ? DBNull.Value : part.Comment;
            command.Parameters.AddWithValue("@transportNumber", SqlDbType.VarChar).Value = part.TransportNumber.IsNullOrEmpty() ? DBNull.Value : part.TransportNumber;
            command.Parameters.AddWithValue("@cancelled", SqlDbType.VarChar).Value = false;

            command.ExecuteNonQuery();
            sqlConnection.Close();
        }

        public void InsertPartTNT(Part part, int id)
        {
            if (part.ShipTo.IsNullOrEmpty() && part.APN.IsNullOrEmpty()) { return; }
            sqlConnection.Open();

            SqlCommand command = new SqlCommand("InsertBillList_TNT_DHL", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@shippingBill_id", SqlDbType.VarChar).Value = id;
            command.Parameters.AddWithValue("@shipTo", SqlDbType.VarChar).Value = part.ShipTo.IsNullOrEmpty() ? DBNull.Value : part.ShipTo;
            command.Parameters.AddWithValue("@up", SqlDbType.VarChar).Value = part.UnloadingPoint.IsNullOrEmpty() ? DBNull.Value : part.UnloadingPoint;
            command.Parameters.AddWithValue("@apn", SqlDbType.VarChar).Value = part.APN.IsNullOrEmpty() ? DBNull.Value : part.APN;
            command.Parameters.AddWithValue("@cpn", SqlDbType.VarChar).Value = part.CPN.IsNullOrEmpty() ? DBNull.Value : part.CPN;
            command.Parameters.AddWithValue("@expectedQuantity", SqlDbType.Int).Value = part.ExpectedQuantity;
            command.Parameters.AddWithValue("@designation", SqlDbType.VarChar).Value = part.Designation.IsNullOrEmpty() ? DBNull.Value : part.Designation;
            command.Parameters.AddWithValue("@stockSAP", SqlDbType.Int).Value = 0;
            command.Parameters.AddWithValue("@finalQuantity", SqlDbType.Int).Value = part.FinalQuantity;
            command.Parameters.AddWithValue("@deliveryNote", SqlDbType.VarChar).Value = part.DeliveryNote.IsNullOrEmpty() ? DBNull.Value : part.DeliveryNote;
            command.Parameters.AddWithValue("@comment", SqlDbType.VarChar).Value = part.Comment.IsNullOrEmpty() ? DBNull.Value : part.Comment;
            command.Parameters.AddWithValue("@transportNumber", SqlDbType.VarChar).Value = part.TransportNumber.IsNullOrEmpty() ? DBNull.Value : part.TransportNumber;
            command.Parameters.AddWithValue("@cancelled", SqlDbType.VarChar).Value = false;

            command.Parameters.AddWithValue("@category", SqlDbType.VarChar).Value = part.SelectedCategory.Replace("System.Windows.Controls.ComboBoxItem: ", "");
            command.Parameters.AddWithValue("@project_id", SqlDbType.Int).Value = (part.SelectedProject.Id != -1) ? part.SelectedProject.Id : DBNull.Value;

            // Define the culture-specific format that uses comma as decimal separator
            CultureInfo cultureInfo = new CultureInfo("en-US");
            cultureInfo.NumberFormat.NumberDecimalSeparator = ",";
            var xPrice = float.Parse(part.Price, cultureInfo);

            command.Parameters.AddWithValue("@price", SqlDbType.Float).Value = xPrice;
            command.Parameters.AddWithValue("@accountNumber", SqlDbType.VarChar).Value = part.AccountNumber;
            command.Parameters.AddWithValue("@po", SqlDbType.VarChar).Value = part.PO;
            command.Parameters.AddWithValue("@trackingNumber", SqlDbType.VarChar).Value = part.TrackNumber;

            command.ExecuteNonQuery();
            sqlConnection.Close();
        }
        #endregion
        #region "Record Database Operations"
        public ObservableCollection<DeliveryNote> SelectRecordDeliveryNotes(int sId)
        {
            sqlConnection.Open();
            ObservableCollection<DeliveryNote> dnList = new();

            SqlCommand command = new("SelectDeliveryNotes", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@shippingBill_id", SqlDbType.Int).Value = sId;

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                DeliveryNote dn = new(reader["number"].ToString() ?? "");
                dnList.Add(dn);
            }
            sqlConnection.Close();
            return dnList;
        }

        public ObservableCollection<Part> SelectRecordParts(int partId)
        {
            sqlConnection.Open();
            ObservableCollection<Part> partList = new();

            SqlCommand command = new("SelectShippingBillList", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@shippingBill_id", SqlDbType.Int).Value = partId;

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            { 
                Part part = new()
                {
                    Id = (reader["id"] != DBNull.Value) ? (int)reader["id"] : 0,
                    RecordId = (reader["shippingBill_id"] != DBNull.Value) ? (int)reader["shippingBill_id"] : 0,
                    APN = reader["apn"].ToString() ?? "",
                    CPN = reader["cpn"].ToString() ?? "",
                    UnloadingPoint = reader["unloadingPoint"].ToString() ?? "",
                    ExpectedQuantity = (reader["expectedQuantity"] != DBNull.Value) ? (int)reader["expectedQuantity"] : 0,
                    Designation = reader["designation"].ToString() ?? "",
                    FinalQuantity = (reader["finalQuantity"] != DBNull.Value) ? (int)reader["finalQuantity"] : 0,
                    SapQuantity = (reader["stockSAP"] != DBNull.Value) ? (int)reader["stockSAP"] : 0,
                    DeliveryNote = reader["deliveryNote"].ToString() ?? "",                    
                    ShipTo = reader["shipTo"].ToString() ?? "",
                    ShipToName = reader["description"].ToString() ?? "",
                    Comment = reader["comment"].ToString() ?? "",
                    TransportNumber = reader["transportNumber"].ToString() ?? "",
                };

                if (part.Designation.IsNullOrEmpty()) { part.Designation = "NOT APPROVED"; }
                
                partList.Add(part);
            }
            sqlConnection.Close();

            foreach(Part part in partList)
            {
                if (part.APN.Length > 0) { part.SapQuantity = SelectSapQuantity(part.APN); }
            }
            return partList;
        }
        public ObservableCollection<Part> SelectParts(DateTime dateStart, DateTime dateEnd)
        {
            dateStart = new DateTime(dateStart.AddDays(-1).Year, dateStart.AddDays(-1).Month, dateStart.AddDays(-1).Day);
            dateEnd = new DateTime(dateEnd.AddDays(1).Year, dateEnd.AddDays(1).Month, dateEnd.AddDays(1).Day);
            sqlConnection.Open();

            ObservableCollection<Part> partList = new();
            // Query
            string queryString = "SELECT bd.id, bd.shippingBill_id,transportNumber, apn, cpn, bd.comment, expectedQuantity, designation, stockSAP, st.description, finalQuantity, deliveryNote, createdBy, st.shipTo as shipTo, up.name as unloadingPoint  FROM [dbo].[BillsDataList] bd "
                                             + "LEFT OUTER JOIN ShipTos st ON  ShipTo_id = st.id "
                                             + "LEFT OUTER JOIN UnloadingPoints up ON  unloadingPoint_id = up.id "
                                             + "LEFT OUTER JOIN ShippingBills spb ON bd.shippingBill_id = spb.id "
                                             + "LEFT OUTER JOIN BillsInSchedule bis ON spb.id = bis.shippingBill_id "
                                             + "LEFT OUTER JOIN ShippingSchedules scd ON bis.shippingSchedule_id = scd.id "
                                             + $" WHERE scd.day > '{dateStart.Month}-{dateStart.Day}-{dateStart.Year}' AND scd.day < '{dateEnd.Month}-{dateEnd.Day}-{dateEnd.Year}'"
                                             + "ORDER BY up.name ";

            SqlCommand command = new(queryString, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Part part = new()
                {
                    Id = (reader["id"] != DBNull.Value) ? (int)reader["id"] : 0,
                    RecordId = (reader["shippingBill_id"] != DBNull.Value) ? (int)reader["shippingBill_id"] : 0,
                    APN = reader["apn"].ToString() ?? "",
                    CPN = reader["cpn"].ToString() ?? "",
                    UnloadingPoint = reader["unloadingPoint"].ToString() ?? "",
                    ExpectedQuantity = (reader["expectedQuantity"] != DBNull.Value) ? (int)reader["expectedQuantity"] : 0,
                    Designation = reader["designation"].ToString() ?? "",
                    FinalQuantity = (reader["finalQuantity"] != DBNull.Value) ? (int)reader["finalQuantity"] : 0,
                    DeliveryNote = reader["deliveryNote"].ToString() ?? "",
                    ShipTo = reader["shipTo"].ToString() ?? "",
                    ShipToName = reader["description"].ToString() ?? "",
                    Comment = reader["comment"].ToString() ?? "",
                    TransportNumber = reader["transportNumber"].ToString() ?? "",
                };
                partList.Add(part);
            }
            sqlConnection.Close();
            return partList;
        }
        public ObservableCollection<Part> SelectRecordPartsTNT(int partId)
        {
            sqlConnection.Open();
            ObservableCollection<Part> partList = new();

            SqlCommand command = new("SelectShippingBillList_TNT_DHL", sqlConnection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@shippingBill_id", SqlDbType.Int).Value = partId;

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Part part = new()
                {
                    Id = (reader["id"] != DBNull.Value) ? (int)reader["id"] : 0,
                    RecordId = (reader["shippingBill_id"] != DBNull.Value) ? (int)reader["shippingBill_id"] : 0,
                    APN = reader["apn"].ToString() ?? "",
                    CPN = reader["cpn"].ToString() ?? "",
                    UnloadingPoint = reader["unloadingPoint"].ToString() ?? "",
                    ExpectedQuantity = (reader["expectedQuantity"] != DBNull.Value) ? (int)reader["expectedQuantity"] : 0,
                    Designation = reader["designation"].ToString() ?? "",
                    FinalQuantity = (reader["finalQuantity"] != DBNull.Value) ? (int)reader["finalQuantity"] : 0,
                    DeliveryNote = reader["deliveryNote"].ToString() ?? "",
                    ShipTo = reader["shipTo"].ToString() ?? "",
                    ShipToName = reader["description"].ToString() ?? "",
                    Comment = reader["comment"].ToString() ?? "",
                    TransportNumber = reader["transportNumber"].ToString() ?? "",
                    SelectedCategory = reader["category"].ToString() ?? "",
                    SelectedProjectInternalId = (reader["internal_id"] != DBNull.Value) ? (int)reader["internal_id"] : 0,
                    Price = (reader["price"] != DBNull.Value) ? (reader["price"].ToString() ?? "") : "",
                    AccountNumber = reader["accountNumber"].ToString() ?? "",
                    PO = reader["po"].ToString() ?? "",
                    TrackNumber = reader["trackingNumber"].ToString() ?? "",
                };
                partList.Add(part);
            }
            sqlConnection.Close();
            return partList;
        }
        public Record SelectRecordById(int id)
        {
            sqlConnection.Open();
            Record Record = new();

            string queryString = "SELECT b.id, b.paleteNumber, b.toBeHandled , b.transportArrivalDate, b.dn, b.rescheduled,b.originalSchedule, b.wasNotified , b.cancelled, c.category, b.carrier, deliveryDate ,b.plate, b.leavingTime, s.time, s.day, st.type as shipmentType, tm.mode as transportMode, b.bid, b.pta, b.unloading, b.comment,b.creationDate,b.updatedDate , releasedUser, releasedDate, processUser, processDate, preparedUser, preparedDate, shippedUser, shippedDate, creationUser FROM BillsInSchedule bs  "
                               + "JOIN ShippingBills b ON shippingBill_id = b.id "
                               + "JOIN ShippingSchedules s ON shippingSchedule_id = s.id "
                               + "JOIN Categories c ON b.category_id = c.id "
                               + "JOIN ShipmentTypes st ON shipmentType_id = st.id "
                               + "JOIN TransportModes tm ON transportMode_id = tm.id "
                               + $" WHERE b.id = '{id}' ";

            SqlCommand command = new(queryString, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Record.Id = (reader["id"] != DBNull.Value) ? (int)reader["id"] : 0;
                Record.Category = reader["category"].ToString() ?? "";
                Record.Carrier = reader["carrier"].ToString() ?? "";
                Record.Plate = reader["plate"].ToString() ?? "";
                Record.Time = reader["time"].ToString() ?? "";
                Record.Day = reader["day"].ToString() ?? "";
                Record.ShipmentType = reader["shipmentType"].ToString() ?? "";
                Record.TransportMode = reader["transportMode"].ToString() ?? "";
                Record.BID = reader["bid"].ToString() ?? "";
                Record.PTA = reader["pta"].ToString() ?? "";
                Record.UnloadingId = reader["unloading"].ToString() ?? "";
                Record.Comment = reader["comment"].ToString() ?? "";
                Record.CreationDate = (reader["creationDate"] != DBNull.Value) ? (DateTime)reader["creationDate"] : DateTime.MinValue;
                Record.WasNotified = (reader["wasNotified"].ToString() == "true") ? true : false;
                Record.IsWaitingForHandle = (reader["toBeHandled"] != DBNull.Value) ? (bool)reader["toBeHandled"] : false;


                if ((reader["wasNotified"].ToString() ?? "") == "true"){Record.WasNotified = true;}
                else
                {
                    Record.WasNotified = true;
                    Record.WasNotified = false;
                }
                Record.DeliveryDate = (reader["deliveryDate"] != DBNull.Value) ? (DateTime)reader["deliveryDate"] : null;
                Record.UpdatedDate = (reader["updatedDate"] != DBNull.Value) ? (DateTime)reader["updatedDate"] : DateTime.MinValue;
                Record.CreatedBy = reader["creationUser"].ToString() ?? "";
                Record.ReleasedBy = reader["releasedUser"].ToString() ?? "";
                Record.ReleasedDate = (reader["releasedDate"] != DBNull.Value) ? (DateTime)reader["releasedDate"] : DateTime.MinValue;
                Record.ProcessBy = reader["processUser"].ToString() ?? "";
                Record.ProcessDate = (reader["processDate"] != DBNull.Value) ? (DateTime)reader["processDate"] : DateTime.MinValue;
                Record.PreparedBy = reader["preparedUser"].ToString() ?? "";
                Record.PreparedDate = (reader["preparedDate"] != DBNull.Value) ? (DateTime)reader["preparedDate"] : DateTime.MinValue;
                Record.ShippedBy = reader["shippedUser"].ToString() ?? "";
                Record.ShippedDate = (reader["shippedDate"] != DBNull.Value) ? (DateTime)reader["shippedDate"] : DateTime.MinValue;
                Record.PaleteNumber = reader["paleteNumber"].ToString() ?? "";

                Record.IsCancelled = (reader["cancelled"].ToString() == "true") ? true : false;
                Record.Rescheduled = (reader["rescheduled"].ToString() == "true") ? true : false;

                Record.OriginalSchedule = reader["originalSchedule"].ToString() ?? "";
                Record.TransportArrivalDate = (reader["transportArrivalDate"] != DBNull.Value) ? (DateTime)reader["transportArrivalDate"] : DateTime.MinValue;
                if(Record.TransportArrivalDate != DateTime.MinValue) { Record.HasTransportArrived = true; }
                else { Record.HasTransportArrived = false; }
            }
            sqlConnection.Close();
            return Record;
        }
        
        public ObservableCollection<string> SelectUnloadingPoints(string shipTo)
        {
            sqlConnection.Open();
            ObservableCollection<string> UPs = new();
            // Query
            string queryString = "SELECT up.name FROM UnloadingPointsInShipTos j "
                                + "JOIN ShipTos st ON j.shipTo_id = st.id "
                                + "JOIN UnloadingPoints up ON j.up_id = up.id "
                                + $"WHERE st.shipTo = '{shipTo}'";
            SqlCommand command = new(queryString, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                UPs.Add(reader["name"].ToString() ?? "");
            }
            sqlConnection.Close();
            return UPs;
        }

        public int SelectSapQuantity(string partNumber)
        {
            int res = 0;
            sqlConnection.Open(); 

            // Query
            string queryString = $"SELECT * FROM FIS.DBO.tbStocks WHERE sap_store ='0030' AND partnr='{partNumber}'";
            SqlCommand command = new(queryString, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                res = (reader["sap_qty"] != DBNull.Value) ? (int)reader["sap_qty"] : 0;
            }
            sqlConnection.Close();
            return res;
        }

        public DateTime SelectZVT11Update()
        {
            sqlConnection.Open();
            DateTime res = DateTime.MinValue;
            ObservableCollection<string> UPs = new();
            // Query
            string queryString = "SELECT TOP(1) * FROM ShipmentsZVT11 WHERE isOpen = 1";
            SqlCommand command = new(queryString, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                res = (reader["importDate"] != DBNull.Value) ? (DateTime)reader["importDate"] : DateTime.MinValue;
            }
            sqlConnection.Close();
            res = res.AddMinutes(15);
            return res;
        }

        public List<string> SelectTransportModes(string shipmentType)
        {
            string type = shipmentType.Substring(0, 1);
            sqlConnection.Open();
            List<string> modes = new();
            // Query
            string queryString = $"SELECT * FROM TransportModes Where sType = '{type}' OR sType ='M' ";

            SqlCommand command = new(queryString, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                modes.Add(reader["mode"].ToString() ?? "");
            }
            sqlConnection.Close();
            return modes;
        }
        public List<string> SelectTypesOfShipment()
        {
            sqlConnection.Open();
            List<string> typesShipmentList = new();
            // Query
            string queryString = "SELECT * FROM ShipmentTypes";

            SqlCommand command = new(queryString, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                typesShipmentList.Add(reader["type"].ToString() ?? "");
            }
            sqlConnection.Close();
            return typesShipmentList;
        }
        public ObservableCollection<ShipTo> SelectShipTos()
        {
            sqlConnection.Open();
            ObservableCollection<ShipTo> shipTos = new();
            // Query
            string queryString = "SELECT * FROM ShipTos";

            SqlCommand command = new(queryString, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                shipTos.Add(new ShipTo()
                {
                    Id = (reader["id"] != DBNull.Value) ? (int)reader["id"] : 0,
                    ShipToName = reader["description"].ToString() ?? "",
                    ShipToNumber = reader["shipTo"].ToString() ?? ""
                });

            }
            sqlConnection.Close();
            return shipTos;
        }
        public ObservableCollection<ShipTo> SelectShipTosUnloadingPoints()
        {
            sqlConnection.Open();
            ObservableCollection<ShipTo> shipTos = new();
            // Query
            string queryString = "SELECT s.id as id, s.description as description, s.shipTo as shipTo, u.id as unload_id, u.name as name FROM ShipTos s "
                                + "LEFT JOIN UnloadingPointsInShipTos us ON us.shipTo_id = s.id "
                                + "LEFT JOIN UnloadingPoints u ON us.up_id = u.id ";

            SqlCommand command = new(queryString, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                shipTos.Add(new ShipTo()
                {
                    Id = (reader["id"] != DBNull.Value) ? (int)reader["id"] : 0,
                    ShipToName = reader["description"].ToString() ?? "",
                    ShipToNumber = reader["shipTo"].ToString() ?? "",
                    UnloadingPoint = (reader["name"] != DBNull.Value) ? (string)reader["name"] : string.Empty,
                    UnloadingPointId = (reader["unload_id"] != DBNull.Value) ? (int)reader["unload_id"] : -1,
                });
            }
            sqlConnection.Close();
            return shipTos;
        }
        public ObservableCollection<Project> SelectProjects(int id)
        {
            sqlConnection.Open();
            ObservableCollection<Project> projects = new();

            // Query
            string queryString = $"SELECT * FROM Projects WHERE shippingBill_id = {id}";

            SqlCommand command = new(queryString, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                projects.Add(new Project()
                {
                    Name = reader["name"].ToString() ?? "",
                    Person = reader["person"].ToString() ?? "",
                    Phone = reader["phoneNumber"].ToString() ?? "",
                    Address = reader["address"].ToString() ?? "",
                    InternalId = (int)reader["internal_id"],
                    Id = (int)reader["id"],
                    UserId= (reader["user_id"] != DBNull.Value) ? (int)reader["user_id"] : -2,
                    Type = reader["pType"].ToString() ?? "",
                    PTA = reader["pta"].ToString() ?? "",
                    Comment = reader["comments"].ToString() ?? "",
                    DeliveryDate = (reader["deliveryDate"] != DBNull.Value) ? (DateTime)reader["deliveryDate"] : null,
                    UniqueHSVColorHue = (reader["hue"] != DBNull.Value) ? Int32.Parse(reader["hue"].ToString() ?? "0") : 0,
                });
            }
            sqlConnection.Close();
            return projects;
        }
        public bool IsDeliveryNotePrepared(string deliveryNote)
        {
            try
            {
                sqlConnection.Open(); 
                string queryString = " SELECT * FROM SAPVL06O " 
                                    + $" Where DeliveryNr = '{deliveryNote}' AND OPS = 'C'";

                SqlCommand command = new SqlCommand(queryString, sqlConnection);
                SqlDataReader reader = command.ExecuteReader();

                int count = 0;
                while (reader.Read())
                {
                    count++;
                }
                 
                sqlConnection.Close(); 
                return count > 0;
            }
            catch (SqlException ex)
            {
                sqlConnection.Close();
                MessageBox.Show("SQL Error: " + (ex.ToString()));
                return false;
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
                MessageBox.Show("Error: " + (ex.ToString()));
                return false;
            }
        }
        public bool DoesRecordExist(int id)
        {
            try
            {
                sqlConnection.Open();
                string queryString = " SELECT * FROM ShippingBills "
                                    + $" Where id = '{id}'";

                SqlCommand command = new SqlCommand(queryString, sqlConnection);
                SqlDataReader reader = command.ExecuteReader();

                int count = 0;
                while (reader.Read())
                {
                    count++;
                }

                sqlConnection.Close();
                return count > 0;
            }
            catch (SqlException ex)
            {
                sqlConnection.Close();
                MessageBox.Show("SQL Error: " + (ex.ToString()));
                return false;
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
                MessageBox.Show("Error: " + (ex.ToString()));
                return false;
            }
        }
        public void InsertMessageForum(string message, string user)
        {
            List<ChatMessage> chatList = new List<ChatMessage>();
            try
            {
                sqlConnection.Open();
                string queryString = $" INSERT INTO [ShippingScheduleForum] (username,content, sys_date,user_id) VALUES('{user}','{message}', GETDATE(),0) ";

                SqlCommand command = new SqlCommand(queryString, sqlConnection);
                command.ExecuteNonQuery();


                sqlConnection.Close();
            }
            catch (SqlException ex)
            {
                sqlConnection.Close();
                MessageBox.Show("SQL Error: " + (ex.ToString())); 
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
                MessageBox.Show("Error: " + (ex.ToString())); 
            }
             
        }
        public List<ChatMessage> GetTodaysChat() 
        {
            List<ChatMessage> chatList = new List<ChatMessage>();
            try
            {
                sqlConnection.Open();
                string queryString = " SELECT * FROM [ShippingScheduleForum] WHERE CONVERT(DATE, [sys_date]) = CONVERT(DATE, GETDATE()); ";

                SqlCommand command = new SqlCommand(queryString, sqlConnection);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    chatList.Add( new ChatMessage()
                    {
                        Date = reader["sys_date"].ToString() ?? "",
                        User = reader["username"].ToString() ?? "",
                        MessageContent = reader["content"].ToString() ?? "",
                        Id = (int)reader["message_id"]
                    });
                }

                sqlConnection.Close();
            }
            catch (SqlException ex)
            {
                sqlConnection.Close();
                MessageBox.Show("SQL Error: " + (ex.ToString()));
                return chatList;
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
                MessageBox.Show("Error: " + (ex.ToString()));
                return chatList;
            }

            return chatList;
        }
        public bool DoesDeliveryNoteExistOnRecord(string deliveryNote)
        {
            try
            {
                sqlConnection.Open();
                string queryString = $" SELECT * FROM [DeliveryNotes] WHERE number = '{deliveryNote}'";

                SqlCommand command = new SqlCommand(queryString, sqlConnection);
                SqlDataReader reader = command.ExecuteReader();

                int count = 0;
                while (reader.Read())
                {
                    count++;
                }

                sqlConnection.Close();
                return count > 0;
            }
            catch (SqlException ex)
            {
                sqlConnection.Close();
                MessageBox.Show("SQL Error: " + (ex.ToString()));
                return false;
            }
            catch (Exception ex)
            {
                sqlConnection.Close();
                MessageBox.Show("Error: " + (ex.ToString()));
                return false;
            }
        }
        #endregion
    }
}
