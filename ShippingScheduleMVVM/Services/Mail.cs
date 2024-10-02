using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Configuration;
using System.Collections.Specialized;
using Microsoft.Win32;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Exchange.WebServices.Data;
using System.Windows;
using ShippingScheduleMVVM.Models;
using System.Collections.ObjectModel;

namespace ShippingScheduleMVVM.Services
{
    public class Mail
    {
        private string _body = "";
        private string _subject = "";

        //private ExchangeService _service;

        private MailMessage _message = new();

        private SmtpClient _client = new();


        #region "Constructors ..."
        public Mail()
        {
            Init();
        }

        public Mail(string ToUser, string subject, string body)
        {
            Init();
            AddUser2ToList(ToUser);
            _subject = subject;
            _body = body;
        }

        private void Init()
        {
            var Settings = ShippingScheduleMVVM.Properties.Settings.Default;
            _message = new MailMessage();

            _message.From = new MailAddress(Settings.smtpUser, Settings.smtpUserDisplayName);

            _client = new SmtpClient(Settings.smtpServer, Convert.ToInt32(Settings.smtpPort));
            _client.Credentials = CredentialCache.DefaultNetworkCredentials;

            _client.Timeout = Convert.ToInt32(Settings.smtpTimeOut);

            _message.IsBodyHtml = true;
        }

        #endregion

        #region "Properties ..."
        public string Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }

        public string Body
        {
            get { return _body; }
            set { _body = value; }
        }
        # endregion

        public void AddUser2ToList(string ToUser)
        {
            if (!_message.To.Contains(new MailAddress(ToUser)))
                _message.To.Add(new MailAddress(ToUser));
        }

        public void AddUser2CcList(string CcUser)
        {
            if (!_message.CC.Contains(new MailAddress(CcUser)))
                _message.CC.Add(new MailAddress(CcUser));
        }

        public void AddUser2BccList(string BccUser)
        {
            if (!_message.Bcc.Contains(new MailAddress(BccUser)))
                _message.Bcc.Add(new MailAddress(BccUser));
        }

        public async void SendMailAsync()
        {
            _message.Subject = _subject;
            _message.Body = new MessageBody(BodyType.HTML, _body);

            try
            {
                await _client.SendMailAsync(_message);
                //MessageBox.Show("Mail Async Sent!");
                //Console.WriteLine("Mail Async Sent!");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public string SendMail()
        {
            _message.Subject = _subject;
            _message.Body = new MessageBody(BodyType.HTML, _body);

            try
            {
                _client.Send(_message);
                return "Mail Sent!";
            }
            catch (Exception e)
            {
                return e.Message + " - " + e.StackTrace;
            }
        }

        public static void SendMailOnCancel(Record record, ObservableCollection<Part> parts)
        {
            try
            {
                string id = record.Id.ToString();
                if (record.ShipmentType == "Regular")
                {
                    id = "#R" + id;
                }
                if (record.ShipmentType == "Especial")
                {
                    id = "#S" + id;
                }
                var date = record.Day.Substring(0, 10) + " " + record.Time;
                var shipTos = Part.ReturnUniqueShipTosInParts(parts);
                string subject = "<div style=\"font-family: Arial, Helvetica, sans-serif;\">"
                                + "    <h2 style=\"margin-bottom: 15px; font-color:red\">This record was cancelled:</h2>"

                                + "      <table style=\"width: 100%;  border-collapse: collapse;font-family: Arial, Helvetica, sans-serif;margin-top: 0px;\">"
                                + "        <tr>"
                                + "          <th style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">ID</th>"
                                + "          <th style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">Date</th>"
                                + "          <th style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">Ship To (s)</th>"
                                + "          <th style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">Pallets</th>"
                                + "          <th style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">Category</th>"
                                + "          <th style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">Comments</th>"
                                + "        </tr>"
                                + "        <tr>"
                                + $"          <td style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">{id}</td>"
                                + $"          <td style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">{date}</td>"
                                + $"          <td style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">{shipTos}</td>"
                                + $"          <td style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">{record.PaleteNumber}</td>"
                                + $"          <td style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">{record.Category}</td>"
                                + $"          <td style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">{record.Comment}</td>"
                                + "        </tr>"

                                + "      </table>"
                                + "    </div>";


                Mail mail = new Mail("emanuel.cunha1@aptiv.com", "Schedule: Cancelled Record", subject);
                DatabaseOperations database = new();

                foreach (string s in database.SelectSupervisorsEmails())
                {
                    mail.AddUser2ToList(s);
                }

                mail.AddUser2ToList("emanuel.cunha1@aptiv.com");

                System.Threading.Tasks.Task.Run(() =>
                {
                    mail.SendMailAsync();
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception -> " + ex.ToString(), "Email Error");
            }
        }

        public static void SendMailIfCreatedRecordToday(Record record, ObservableCollection<Part> parts, string originalSchedule)
        {
            try
            {
                if (DateTime.Parse(record.Day).Date != DateTime.Today.Date){ return; }

                string id = record.Id.ToString();
                if (record.ShipmentType == "Regular")
                {
                    id = "#R" + id;
                }
                if (record.ShipmentType == "Especial")
                {
                    id = "#S" + id;
                }
                var date = record.Day.Substring(0, 10) + " " + record.Time;
                var shipTos = Part.ReturnUniqueShipTosInParts(parts);

                string subject = "<div style=\"font-family: Arial, Helvetica, sans-serif;\">"
                            + "    <h2 style=\"margin-bottom: 10px;\"><svg xmlns=\"http://www.w3.org/2000/svg\" style=\"margin-bottom: -5px; margin-right: 10px;\" id=\"Layer_1\" data-name=\"Layer 1\" viewBox=\"0 0 24 24\" width=\"30\" height=\"30\"><path d=\"M24,7v1H0v-1C0,4.239,2.239,2,5,2h1V1c0-.552,.448-1,1-1h0c.552,0,1,.448,1,1v1h8V1c0-.552,.448-1,1-1h0c.552,0,1,.448,1,1v1h1c2.761,0,5,2.239,5,5Zm0,10c0,3.86-3.141,7-7,7s-7-3.14-7-7,3.141-7,7-7,7,3.14,7,7Zm-5,.586l-1-1v-1.586c0-.552-.448-1-1-1h0c-.552,0-1,.448-1,1v2c0,.265,.105,.52,.293,.707l1.293,1.293c.39,.39,1.024,.39,1.414,0h0c.39-.39,.39-1.024,0-1.414Zm-11-.586c0-2.829,1.308-5.35,3.349-7H0v9c0,2.761,2.239,5,5,5h6.349c-2.041-1.65-3.349-4.171-3.349-7Z\"/></svg>Inserted or Rescheduled this record on the same day of plan: </h2>"

                            + "      <table style=\"width: 100%;  border-collapse: collapse;font-family: Arial, Helvetica, sans-serif;\">"
                            + "        <tr>"
                            + "          <th style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">Id</th>"
                            + "          <th style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">Status</th>"
                            + "          <th style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">Shipment Type</th>"
                            + "          <th style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">Transport Mode</th>"
                            + "          <th style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">Ship To (s)</th>"
                            + "          <th style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">Pallets</th>"
                            + "          <th style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">Original Date</th>"
                            + "          <th style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">Current Date</th>"
                            + "        </tr>"
                            + "        <tr>"
                            + $"          <td style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">{id}</td>"
                            + $"          <td style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">{record.Category}</td>"
                            + $"          <td style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">{record.ShipmentType}</td>"
                            + $"          <td style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">{record.TransportMode}</td>"
                            + $"          <td style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">{shipTos}</td>"
                            + $"          <td style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">{record.PaleteNumber}</td>"
                            + $"          <td style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">{originalSchedule.Replace("+", " ")}</td>"
                            + $"          <td style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">{date}</td>"
                            + "        </tr>"

                            + "      </table>"
                            + "    </div>";
                DatabaseOperations database = new();
                Mail mail = new Mail(database.SelectUserEmail(record.CreatedBy), "Schedule: Inserted or Rescheduled this record on the same day of plan", subject);

                foreach (string s in database.SelectNotifyDailyPlanChangesEmails())
                {
                    mail.AddUser2ToList(s);
                }

                System.Threading.Tasks.Task.Run(() =>
                {
                    mail.SendMailAsync();
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception -> " + ex.ToString(), "Email Error");
            }
        }

        public static void SendMailOnReschedule(Record record, ObservableCollection<Part> parts, string originalSchedule)
        {
            try
            {
                if(record.Category == "Created") { return; }
                string id = record.Id.ToString();
                if (record.ShipmentType == "Regular")
                {
                    id = "#R" + id;
                }
                if (record.ShipmentType == "Especial")
                {
                    id = "#S" + id;
                }
                var date = record.Day.Substring(0, 10) + " " + record.Time;
                var shipTos = Part.ReturnUniqueShipTosInParts(parts);

                string subject = "<div style=\"font-family: Arial, Helvetica, sans-serif;\">"
                            + "    <h2 style=\"margin-bottom: 10px;\"><svg xmlns=\"http://www.w3.org/2000/svg\" style=\"margin-bottom: -5px; margin-right: 10px;\" id=\"Layer_1\" data-name=\"Layer 1\" viewBox=\"0 0 24 24\" width=\"30\" height=\"30\"><path d=\"M24,7v1H0v-1C0,4.239,2.239,2,5,2h1V1c0-.552,.448-1,1-1h0c.552,0,1,.448,1,1v1h8V1c0-.552,.448-1,1-1h0c.552,0,1,.448,1,1v1h1c2.761,0,5,2.239,5,5Zm0,10c0,3.86-3.141,7-7,7s-7-3.14-7-7,3.141-7,7-7,7,3.14,7,7Zm-5,.586l-1-1v-1.586c0-.552-.448-1-1-1h0c-.552,0-1,.448-1,1v2c0,.265,.105,.52,.293,.707l1.293,1.293c.39,.39,1.024,.39,1.414,0h0c.39-.39,.39-1.024,0-1.414Zm-11-.586c0-2.829,1.308-5.35,3.349-7H0v9c0,2.761,2.239,5,5,5h6.349c-2.041-1.65-3.349-4.171-3.349-7Z\"/></svg>This record was reschedulled: </h2>"

                            + "      <table style=\"width: 100%;  border-collapse: collapse;font-family: Arial, Helvetica, sans-serif;\">"
                            + "        <tr>"
                            + "          <th style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">Id</th>"
                            + "          <th style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">Status</th>"
                            + "          <th style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">Shipment Type</th>"
                            + "          <th style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">Transport Mode</th>"
                            + "          <th style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">Ship To (s)</th>"
                            + "          <th style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">Pallets</th>"
                            + "          <th style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">Original Date</th>"
                            + "          <th style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">Current Date</th>"
                            + "        </tr>"
                            + "        <tr>"
                            + $"          <td style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">{id}</td>"
                            + $"          <td style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">{record.Category}</td>"
                            + $"          <td style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">{record.ShipmentType}</td>"
                            + $"          <td style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">{record.TransportMode}</td>"
                            + $"          <td style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">{shipTos}</td>"
                            + $"          <td style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">{record.PaleteNumber}</td>"
                            + $"          <td style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">{originalSchedule.Replace("+"," ")}</td>"
                            + $"          <td style=\"border: 1px solid #dddddd; text-align: left; padding: 8px;\">{date}</td>"
                            + "        </tr>"

                            + "      </table>"
                            + "    </div>";
                DatabaseOperations database = new();
                Mail mail = new Mail(database.SelectUserEmail(record.CreatedBy), "Schedule: Reschedulled Record", subject);
                System.Threading.Tasks.Task.Run(() =>
                {
                    mail.SendMailAsync();
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception -> " + ex.ToString(), "Email Error");
            }
        }

    }
}
